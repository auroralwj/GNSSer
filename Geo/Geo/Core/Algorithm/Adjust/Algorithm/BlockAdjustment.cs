//2013.06.03.10.13, czs, Creating.
//2013.06.04.10.00, czs, edit, 初次调通串行执行
//2013.06.06, czs, edit, 实现并行计算

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using System.Threading.Tasks;

namespace Geo.Algorithm.Adjust
{
  
    ///
    /// <summary>
    /// 分区平差，是将一个大型网分成若干分区，每分区单独平差，然后把各个分区联解成一个整体。
    /// </summary>
    public class BlockAdjustment
    {
        //输入参数为r个分区的 Ai, Pi, Li, Bi
        //输出参数为Xi,XB, Qxb        
        /// <summary>
        /// 分区平差
        /// </summary>
        /// <param name="BlockAdjustItems">分区集合</param>
        /// <param name="parallel">是否采用并行计算</param>
        public BlockAdjustment(BlockAdjustItem[] BlockAdjustItems, bool parallel = true, int MaxDegreeOfParallelism = 8)
        {
            //参数赋值，数量统计

            this.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
            this.BlockAdjustItems = BlockAdjustItems;
            this.BlockCount = BlockAdjustItems.Length;
            this.CommonParamCount = BlockAdjustItems[0].CommonParamCount;
            int obsCount = 0;
            int sumOfBlockParam = 0;
            foreach (var item in BlockAdjustItems)
            {
                obsCount += item.ObsCount;
                sumOfBlockParam += item.BlockParamCount;
            }
            this.ObsCount = obsCount;
            this.Freedom = ObsCount - sumOfBlockParam - CommonParamCount;
 
            //最耗时的计算
            if (parallel)
                ParallelSolve(BlockAdjustItems); 
            else
               SerialSolve(BlockAdjustItems);

            PrecisionEstimation();    
        }

        /// <summary>
        /// 多核并行计算
        /// </summary>
        /// <param name="BlockAdjustItems"></param>
        private void ParallelSolve(BlockAdjustItem[] BlockAdjustItems)
        {
            List<Action> actions = new List<Action>();

            ParallelOptions opt = new ParallelOptions() { MaxDegreeOfParallelism = MaxDegreeOfParallelism };
            //并行计算      
            actions.Clear();
            foreach (var item in BlockAdjustItems)   actions.Add( item.Solve_step1 );
            Parallel.Invoke(opt, actions.ToArray());

            //汇总
            Summary(BlockAdjustItems);

            //并行计算各分区内参数
            actions.Clear();
            foreach (var item in BlockAdjustItems) actions.Add(item.Solve_step2);
            Parallel.Invoke(opt, actions.ToArray()); 
        }

        /// <summary>
        /// 串行计算。
        /// </summary>
        /// <param name="BlockAdjustItems"></param>
        private void SerialSolve(BlockAdjustItem[] BlockAdjustItems)
        {
            //逐个计算
            foreach (var item in BlockAdjustItems) item.Solve_step1(); 

            //汇总
            Summary(BlockAdjustItems);

            //计算各分区内参数
            foreach (var item in BlockAdjustItems)  item.Solve_step2(); 
        }

        /// <summary>
        /// 汇总计算。由各分区的Ni，Ui，计算公共参数值和协方差阵。
        /// </summary>
        /// <param name="BlockAdjustItems"></param>
        private void Summary(BlockAdjustItem[] BlockAdjustItems)
        {
            int commonParaCount = BlockAdjustItems[0].Normal.Length;
            ArrayMatrix N = new ArrayMatrix(commonParaCount, commonParaCount);
            ArrayMatrix U = new ArrayMatrix(commonParaCount, 1);
            foreach (var item in BlockAdjustItems)
            {
                N += new ArrayMatrix(item.Normal);
                U += new ArrayMatrix(item.RightHand);
            }
            ArrayMatrix inverN = N.Inverse;
            ArrayMatrix Xb = inverN * U;

            //精度估计，计算单位权方差，中误差
            this.InverseWeithOfCommonParam = inverN.Array;
            this.CommonParams = Xb.Array;

            //赋值给子分区
            foreach (var item in BlockAdjustItems) item.CommonParams = this.CommonParams;
        }

        /// <summary>
        /// 最后的精度估计
        /// </summary>
        private void PrecisionEstimation()
        {
            double vTpv = 0;
            foreach (var item in this.BlockAdjustItems) 
                vTpv += item.VTPV;

            this.SquareSumOfResidualsVTPV = vTpv;
            this.VarianceOfUnitWeight = this.SquareSumOfResidualsVTPV / this.Freedom;
            this.StdDev = Math.Sqrt(this.VarianceOfUnitWeight);
            ArrayMatrix D = new ArrayMatrix(InverseWeithOfCommonParam) * this.VarianceOfUnitWeight;
            this.CovaOfCommonParams = D.Array;

            double[] ParamCovaVector = MatrixUtil.GetDiagonal(D.Array);
            this.CommonParamRmsVector = MatrixUtil.GetPow(ParamCovaVector, 0.5);
        }

        #region 属性  
        /// <summary>
        /// 并行度
        /// </summary>
        public int MaxDegreeOfParallelism { get; set; }
        /// <summary>
        /// 分区集合，数组。
        /// </summary>
        public BlockAdjustItem[] BlockAdjustItems { get; private set; }

        /// <summary>
        /// 公共参数的协方差阵。D = InverseWeithOfCommonParam * VarianceOfUnitWeight.
        /// </summary>
        public double[][] CovaOfCommonParams { get; private set; }

        /// <summary>
        /// 公共参数的权逆阵
        /// </summary>
        public double [][] InverseWeithOfCommonParam { get; private set; }
        /// <summary>
        /// 公共参数
        /// </summary>
        public double[][] CommonParams {  get; private set; }
        /// <summary>
        /// 分区数量
        /// </summary>
        public int BlockCount { get; private set; }
        /// <summary>
        /// 公共参数数量
        /// </summary>
        public int CommonParamCount{ get; private set; } 
        /// <summary>
        /// 观测量。
        /// </summary>
        public int ObsCount { get; private set; }
        /// <summary>
        ///自由度，样本中独立或能自由变化的变量个数,通常为：样本个数 - 被限制的变量个数或条件数，或多余观测数。
        /// </summary>
        public int Freedom { get; private set; }
        /// <summary>
        /// 残差平方和
        /// </summary>
        public double SquareSumOfResidualsVTPV { get; private set; }
        /// <summary>
        /// 单位权方差  
        /// </summary>
        public double VarianceOfUnitWeight { get; private set; }

        /// <summary>
        ///   单位权中误差,均方差(Standard deviation )估值。 
        /// </summary>
        public double StdDev { get; private set; }
        /// <summary>
        /// 参数中误差，均方差估值。
        /// </summary>
        public double[] CommonParamRmsVector { get; private set; }
        #endregion

        #region IO
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Block Adjustment Report,  By Geo                " + DateTime.Now);
            sb.AppendLine("------------------------------------------------------------------");
            sb.AppendLine(String.Format("        ObsCount:{0}", ObsCount));
            sb.AppendLine(String.Format("      BlockCount:{0}", BlockCount));
            sb.AppendLine(String.Format("CommonParamCount:{0}", CommonParamCount));
            sb.AppendLine(String.Format("         Freedom:{0}", Freedom)); 
            sb.AppendLine(String.Format("  VarianceOfUnitWeight:{0}", VarianceOfUnitWeight));
            sb.AppendLine(String.Format("          StdDev:{0}", StdDev));
            sb.AppendLine("------------------------------------------------------------------");
            sb.AppendLine("CommonParams");
            sb.AppendLine(MatrixUtil.GetFormatedText(CommonParams));
            sb.AppendLine("CovaOfCommonParams");
            sb.AppendLine(MatrixUtil.GetFormatedText(CovaOfCommonParams));
            int num = 0;
            foreach (var item in BlockAdjustItems)
            {
                sb.AppendLine("Block " + num);
                sb.AppendLine(MatrixUtil.GetFormatedText(item.EstimatedParam));
                sb.AppendLine("ObsError " + num);
                sb.AppendLine(MatrixUtil.GetFormatedText(item.ObsError));
                num++;
            }
            return sb.ToString();
        }
        #endregion

        #region 测试分区平差
        /// <summary>
        /// 测试分区平差
        /// </summary>
        public static void Test(int allObsCount = 1000, bool useParalell = true, int core = 8, int blockCount = 8)
        {
            DateTime from = DateTime.Now;

            //观测噪声
            Random random = new Random();

            //设有blockObsCount个观测量，paramCount个区内参数，commonParamCount个公共参数 
            int blockObsCount = allObsCount / blockCount;
            int paramCount = blockObsCount/2 +1;
            int commonParamCount = blockObsCount/4 + 1; 

            List<BlockAdjustItem> items = new List<BlockAdjustItem>();
            for (int m = 0; m < blockCount; m++)
            {
                //分区内参数系数阵 obsCount x paramCount
                double[][] coeffA = GenCoeffA(blockObsCount, paramCount);
                //观测值 obsCount x 1
                double[][] obs = MatrixUtil.Create(blockObsCount, 1);
                for (int i = 0; i < blockObsCount; i++) obs[i][0] = i;// +random.NextDouble() - 0.5;
                //观测值权逆阵 obsCount x obsCount
                double[][] inverseOfObs = MatrixUtil.CreateIdentity(blockObsCount);
                //分区内对公共参数的系数阵 obsCount x commonParamCount
                double[][] coeffB = MatrixUtil.Create(blockObsCount, commonParamCount);
                for (int i = 0; i < commonParamCount; i++) coeffB[i][i] = 1; 

                //System.IO.File.WriteAllText(
                //     "C:\\Block"+m+".txt",
                //     "A" + "\r\n" +  MatrixUtil.GetFormatedText(coeffA) + "\r\n"
                //   + "L" + "\r\n" + MatrixUtil.GetFormatedText(obs) + "\r\n"
                //   + "Q" + "\r\n" + MatrixUtil.GetFormatedText(inverseOfObs) + "\r\n"
                //   + "Ab" + "\r\n" + MatrixUtil.GetFormatedText(coeffB) + "\r\n"                    
                //    );  

                BlockAdjustItem item = new BlockAdjustItem(coeffA, obs, inverseOfObs, coeffB);
                items.Add(item);
            }

            BlockAdjustment ba = new BlockAdjustment(items.ToArray(), useParalell, core);

            TimeSpan span = DateTime.Now - from;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("耗时:" + span + " = " + span.TotalMinutes + " 分");
            System.IO.File.WriteAllText("C:\\IsP" + useParalell.ToString() + "obs" + +allObsCount + "_core" + core + "_block" + blockCount + ".txt", sb.ToString() + ba.ToString()); 
        }

        private static double[][] GenCoeffA(int obsCount, int paraCount)
        {
            double[][] coeffA = MatrixUtil.Create(obsCount, paraCount);
            for (int i = 0; i < obsCount; i++)
            {
                int col = i % paraCount;
                coeffA[i][col] = 1.0;
            }
            return coeffA;
        }
        #endregion

    }
}
