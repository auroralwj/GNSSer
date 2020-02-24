
//2013.06.20, czs, Created.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Times;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Algorithm.Adjust;
using Gnsser.Data.Sinex;
using Geo.Times; 

namespace Gnsser.Service
{ 
    /// <summary>
    /// 基于 Sinex 文件的分区平差。
    /// </summary>
    public  class SinexBlockAdjust
    {
        /// <summary>
        /// 基于 Sinex 文件的分区平差。
        /// </summary>
        /// <param name="files">具有公共参数的分区</param>
        public SinexBlockAdjust(params SinexFile[] files)
        {
            if (files.Length <= 1) throw new ArgumentException("输入的SINEX文件数量不可少于2！");
            this.files = files;

            //检查一下是否只包含坐标，如否，则清理。
            foreach (var item in files) if (!item.IsOnlyEstimateCoordValue) item.CleanNonCoordSolutionValue();

            //提取公共测站名称
            List<string> sameSites = null; new List<string>();
            foreach (var item in files)
            {
                if (sameSites == null) { sameSites = new List<string>(); sameSites.AddRange(item.GetSiteCods()); }
                else sameSites = SinexFile.GetSameSiteCodes(item, sameSites);
            }
            if (sameSites.Count == 0) throw new ArgumentException("分区没有公共点，不可执行分区平差！");

            //计算统一单位权中误差
            double commonVarFactor = GetVarianceFactor(files);


            //组建分区  
            DateTime from = DateTime.Now;
            List<BlockAdjustItem> items = new List<BlockAdjustItem>();
            foreach (var file in files)
            {
                //获取公共参数所在的编号,都转化为X，Y，Z分别的参数
                List<int> commonParamIndexes = file.GetParamIndexes(sameSites);//公共参数的索引
                List<int> blockParamIndexes = file.GetParamIndexesExcept(sameSites);//区内参数的索引
                List<int> newIndexes = new List<int>(blockParamIndexes);//新索引顺序

                newIndexes.AddRange(commonParamIndexes);

                int blockParamCount = blockParamIndexes.Count;
                int obsCount = file.EstimateParamCount;
                int commonParamCount = commonParamIndexes.Count;
                //分区内参数系数阵 obsCount x paramCount
                double[][] coeffA = MatrixUtil.Create(obsCount, blockParamCount);
                for (int i = 0; i < blockParamCount; i++) coeffA[i][i] = 1.0;
                //观测值 obsCount x 1
                double[][] obs = MatrixUtil.Create(obsCount, 1);
                for (int i = 0; i < obsCount; i++)
                {
                    if (i < blockParamCount)//区内参数
                        obs[i][0] = file.SolutionEstimateBlock.Items[blockParamIndexes[i]].ParameterValue;
                    else//公共参数
                        obs[i][0] = file.SolutionEstimateBlock.Items[commonParamIndexes[i - blockParamCount]].ParameterValue;
                }
                //观测值权逆阵 obsCount x obsCount
                //注意：此处应该统一化权阵
                double[][] inverseOfObs = null;
                if (file.HasEstimateCovaMatrix)
                {
                    inverseOfObs = file.GetEstimateCovaMatrix();
                    MatrixUtil.SymmetricExchange(inverseOfObs, newIndexes);
                    MatrixUtil.Multiply(inverseOfObs, commonVarFactor / file.GetStatistic().VarianceOfUnitWeight);
                }
                //else if (file.HasNormalEquationMatrix && file.HasNormalEquationVectorMatrix)
                //{
                //    //方法待验证
                //    double[][]  normal = file.GetNormalEquationMatrix();
                //    double[] righHand = file.GetNormalEquationVector();
                //    Geo.Algorithm.Matrix n = new Geo.Algorithm.Matrix(normal);
                //    Geo.Algorithm.Matrix u = new Geo.Algorithm.Matrix(MatrixUtil.Create( righHand));
                //    inverseOfObs = n.Inverse.Array;
                //    double lTpl = file.GetStatistic().WeightedSqureSumOfOMinusC;
                //    double vTpv = lTpl - (u.Transpose() * n * u)[0,0];
                //    double varFactor = vTpv / file.GetStatistic().NumberOfDegreesOfFreedom;
                //    MatrixUtil.Multiply(inverseOfObs, varFactor);

                //    MatrixUtil.SymmetricExchange(inverseOfObs, newIndexes);
                //    MatrixUtil.Multiply(inverseOfObs, commonVarFactor / file.GetStatistic().VarianceOfUnitWeight);
                //}
                else//去相关
                {
                    inverseOfObs = MatrixUtil.Create(obsCount);                   
                    for (int i = 0; i < obsCount; i++)
                    {
                        if (i < blockParamCount)//区内参数
                            inverseOfObs[i][i] = Math.Pow(file.SolutionEstimateBlock.Items[blockParamIndexes[i]].StdDev, 2);
                        else//公共参数
                            inverseOfObs[i][i] = Math.Pow(file.SolutionEstimateBlock.Items[commonParamIndexes[i - blockParamCount]].StdDev, 2);
                    }
                    MatrixUtil.Multiply(inverseOfObs, commonVarFactor / file.GetStatistic().VarianceOfUnitWeight);
                }

                //分区内对公共参数的系数阵 obsCount x commonParamCount
                double[][] coeffB = MatrixUtil.Create(obsCount, commonParamCount);
                for (int i = blockParamCount; i < obsCount; i++) coeffB[i][i - blockParamCount] = 1;

                BlockAdjustItem item = new BlockAdjustItem(coeffA, obs, inverseOfObs, coeffB);
                items.Add(item);
            }

            ba = new BlockAdjustment(items.ToArray(), false);


            //创建结果SINEX文件，文件只包含公共参数的内容
            ResultSinexFile = new SinexFile("Gnsser");
            //测站名称
            int commonSiteCount = ba.CommonParamCount / 3;
            List<SiteId> siteIds = new List<SiteId>();
            for (int i = 0; i < commonSiteCount; i++) siteIds.Add(files[0].GetSiteId(sameSites[i]));
            ResultSinexFile.SiteIdBlock = SinexFactory.CreateSiteIdBlock(siteIds);
            //统计信息
            SinexStatistic statistic = new SinexStatistic()
                       {
                           NumberOfUnknown = ba.CommonParamCount,
                           NumberOfDegreesOfFreedom = ba.Freedom,
                           VarianceOfUnitWeight = ba.VarianceOfUnitWeight,
                           SquareSumOfResidualsVTPV = ba.SquareSumOfResidualsVTPV,
                           NumberOfObservations = ba.ObsCount
                       };
            ResultSinexFile.SolutionStattisticsBlock =
                SinexFactory.CreateSolutionStattisticsBlock(statistic.GetSolutionStatistics());

            //估值
            List<SolutionValue> estList = new List<SolutionValue>();
            Time refEchop = files[0].SolutionEstimateBlock.Items[0].RefEpoch;
            for (int i = 0; i < commonSiteCount; i++)
            {
                int paramIndex = i * 3;
                SolutionValue svX = new SolutionValue(paramIndex + 1, siteIds[i].SiteCode, "STAX", ba.CommonParams[paramIndex + 0][0], refEchop, ba.CommonParamRmsVector[paramIndex + 0]);
                SolutionValue svY = new SolutionValue(paramIndex + 2, siteIds[i].SiteCode, "STAY", ba.CommonParams[paramIndex + 1][0], refEchop, ba.CommonParamRmsVector[paramIndex + 1]);
                SolutionValue svZ = new SolutionValue(paramIndex + 3, siteIds[i].SiteCode, "STAZ", ba.CommonParams[paramIndex + 2][0], refEchop, ba.CommonParamRmsVector[paramIndex + 2]);
                estList.AddRange(new SolutionValue[]{
                   svX, svY, svZ
               });
            }
            ResultSinexFile.SolutionEstimateBlock = SinexFactory.CreateSolutionEstimateBlock(estList);
            //协方差
            ResultSinexFile.SolutionMatrixEstimateCova = SinexFactory.CreateSolutionMatrixEstimateCova(ba.CovaOfCommonParams);

            this.TimeSpan = DateTime.Now - from;
        }
         BlockAdjustment ba = null;

        /// <summary>
         /// 计算所有的单位权方差。
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
         public static double GetVarianceFactor(SinexFile[] files)
         {
             List<SinexStatistic> list = new List<SinexStatistic>();
             foreach (var item in files)
             {
                 list.Add(item.GetStatistic());
             }
             return GetVarianceFactor(list.ToArray());
         }
        /// <summary>
        /// 计算所有的单位权方差。
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static double GetVarianceFactor(SinexStatistic[] files)
        {
            double vf = 0;
            double upper = 0;
            double lower = 0;
            foreach (var item in files)
            {
                upper += item.VarianceOfUnitWeight * item.NumberOfDegreesOfFreedom;
                lower += item.NumberOfDegreesOfFreedom;
            }

            vf = upper/lower;
            return vf;
        }

        /// <summary>
        /// 时间间隔
        /// </summary>
        public TimeSpan TimeSpan { get; private set; }

        public override string ToString()
        {
            if (TimeSpan != null && ba != null)
                return "timeSpan:" + TimeSpan  + "\r\n" + ba.ToString();

            return base.ToString();
        }

        SinexFile [] files;
        SinexFile result;

        /// <summary>
        /// 结果SINEX文件，只包含公共参数的内容。
        /// </summary>
        public SinexFile ResultSinexFile { get; private set; }
    }
}
