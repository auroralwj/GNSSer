//2017.08.31, czs, create, 通用文件平差器

using System;
using System.Collections.Generic; 
using System.Text;  
using Geo.Algorithm.Adjust; 
using Geo.Utils;
using Geo;
using System.Linq;
using Gnsser;
using Geo.Times;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 平差参数名称
    /// </summary>
    public class AdjustName
    {
        public const string Epoch = "Epoch";
        public const string Approx = "Approx";
        public const string ParamName = "ParamName";
        public const string Obs = "Obs";
        public const string RmsOfObs = "RmsOfObs";
        public const string Design = "Design";
        public const string Trans = "Trans";
        public const string RmsOfTrans = "RmsOfTrans";
        public const string Apriori = "Apriori";
        public const string RmsOfApriori = "RmsOfApriori";
        public const string Estimated = "Estimated";
        public const string RmsOfEstimated = "RmsOfEstimated";
    }


    /// <summary>
    /// 通用平差器
    /// </summary>
    public class CommonFileAdjuster : AbstractProcess<string>, ICancelAbale, IProgressNotifier
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="AdjustmentType"></param>
        /// <param name="OutDirectory"></param>
        /// <param name="ProgressNotifier"></param>
        public CommonFileAdjuster(AdjustmentType AdjustmentType = AdjustmentType.卡尔曼滤波, string OutDirectory = null, IProgressViewer ProgressNotifier = null)
        {
            if (String.IsNullOrWhiteSpace(OutDirectory))
            {
                OutDirectory = Setting.TempDirectory;
            }
            this.OutDirectory = OutDirectory;
            Geo.Utils.FileUtil.CheckOrCreateDirectory(OutDirectory);
            this.AdjustmentType = AdjustmentType;
            ResultTables = new ObjectTableManager(OutDirectory);
            IsCancel = false;
            this.ProgressViewer = ProgressNotifier;

            adjuster = AdjusterFactory.Create(AdjustmentType);
        }

        #region  属性 
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutDirectory { get; set; }

        /// <summary>
        /// 平差器类型
        /// </summary>
        public AdjustmentType AdjustmentType { get; set; }
        /// <summary>
        /// 平差结果表格
        /// </summary>
        public ObjectTableManager ResultTables { get; set; } 
        /// <summary>
        /// 当前计算结果。
        /// </summary>
        public AdjustmentResult CurrentResult { get; set; }
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool IsCancel { get; set; }
        #endregion

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="filePath"></param>
        public override void Run(string filePath)
        {
            ResultTables.Clear();
            IsCancel = false;
            ObjectTableManagerReader reader = new ObjectTableManagerReader(filePath);
            ObjectTableManager tables = reader.Read();

            var approxTable = tables.Get(AdjustName.Approx);
            var paramNameTable = tables.Get(AdjustName.ParamName);
            var obsTable = tables.Get(AdjustName.Obs);
            var rmsOfObsTable = tables.Get(AdjustName.RmsOfObs);
            var designTable = tables.Get(AdjustName.Design);
            var transTable = tables.Get(AdjustName.Trans);
            var rmsOfTransTable = tables.Get(AdjustName.RmsOfTrans);
            var aprioriTable = tables.Get(AdjustName.Apriori);
            var rmsOfAprioriTable = tables.Get(AdjustName.RmsOfApriori);

            int length = obsTable.RowCount;

            InitProcess(length); 

            Vector approx = null;
            if (approxTable != null && approxTable.RowCount > 0)
            {
                approx = new Vector(approxTable.BufferedValues[0]);
            }
            WeightedVector apriori = null;
            if (aprioriTable != null && aprioriTable.RowCount > 0 && rmsOfAprioriTable != null && rmsOfAprioriTable.RowCount > 0)
            {
                apriori = new WeightedVector(new Vector(aprioriTable.BufferedValues[0]),
                    new Matrix(new Vector(rmsOfAprioriTable.BufferedValues[0])));

            }

            for (int i = 0; i < length; i++)
            {
                if (IsCancel) { break; }

                Dictionary<string, Object> obsRow = obsTable.BufferedValues[i];
                Dictionary<string, Object> rmsOfObsRow = rmsOfObsTable.BufferedValues[i];
                Dictionary<string, Object> designRow = designTable.BufferedValues[i];
                Dictionary<string, Object> paramNameRow = paramNameTable.BufferedValues[i];

                Vector obs = new Vector(obsRow);
                Vector RmsOfObs = new Vector(rmsOfObsRow);
                Vector Design = new Vector(designRow);
                int obsCount = obsRow.Count;
                int paramCount = paramNameRow.Count;

                Matrix RmsOfObsMatrix = new Matrix(RmsOfObs);
                Matrix designMatrix = new Matrix(Design.OneDimArray, obsCount, paramCount);
                List<string> paramNames = new List<string>();
                foreach (var item in paramNameRow)
                {
                    paramNames.Add(item.Key);
                }              //平差矩阵生成
                ManualAdjustMatrixBuilder matrixBuilder = new ManualAdjustMatrixBuilder();
                matrixBuilder.ApproxParam = approx;
                matrixBuilder.SetCoeffOfDesign(designMatrix)
                    .SetObsMinusApprox(new WeightedVector(obs, RmsOfObsMatrix.Pow(2.0)))
                    .SetParamNames(paramNames);

                #region 先验值
                if (apriori == null)
                {
                    if (CurrentResult == null)
                    {
                        apriori = CreateInitAprioriParam(paramCount);
                        apriori.ParamNames = paramNames;
                    }
                    else if (!IsParamsChanged(paramNames))
                    {
                        apriori = CurrentResult.ResultMatrix.Estimated;
                    }
                    else
                    {
                        apriori = SimpleAdjustMatrixBuilder.GetNewWeighedVectorInOrder(paramNames, CurrentResult.ResultMatrix.Estimated);
                    }
                }
                matrixBuilder.SetAprioriParam(apriori);
                apriori = null;
                #endregion

                //非必须的转移矩阵                
                if ((transTable != null && transTable.BufferedValues.Count > i)
                    && (rmsOfTransTable != null && rmsOfTransTable.BufferedValues.Count > i))
                {
                    var transRow = transTable.BufferedValues[i];
                    Vector Trans = new Vector(transRow);
                    var transMatrix = new Matrix(Trans.OneDimArray, paramCount, paramCount);

                    var rmsOfTransRow = rmsOfTransTable.BufferedValues[i];
                    Vector RmsOfTrans = new Vector(rmsOfTransRow);
                    var rmsOfTransMatrix = new Matrix(RmsOfTrans.OneDimArray, paramCount, paramCount);

                    matrixBuilder.SetTransfer(new WeightedMatrix(transMatrix, rmsOfTransMatrix.Pow(2.0)));
                }

                Process(matrixBuilder);

                PerformProcessStep(); 
             
            }

            this.FullProcess(); 
        }
        /// <summary>
        /// 参数是否改变
        /// </summary>
        /// <param name="ParamNames"></param>
        /// <returns></returns>
        public bool IsParamsChanged(List<string> ParamNames)
        {
            if (this.CurrentResult == null) return false;

            return !ListUtil.IsEqual(CurrentResult.ResultMatrix.ParamNames, ParamNames);
        }


        /// <summary>
        /// 第一次参数先验值。 创建初始先验参数值和协方差阵。只会执行一次。
        /// </summary>
        /// <returns></returns>
        protected virtual WeightedVector CreateInitAprioriParam(int ParamCount)
        {
            return new WeightedVector(new Vector(ParamCount), new DiagonalMatrix(ParamCount, 10000));
        }
        MatrixAdjuster adjuster;
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="matrixBuilder"></param>
        private void Process(ManualAdjustMatrixBuilder matrixBuilder)
        {
            var matrixResult =  adjuster.Run(new AdjustObsMatrix(matrixBuilder));

            this.CurrentResult = new AdjustmentResult(matrixResult);


            var paramTalbe = ResultTables.GetOrCreate("Param");
            var rmsTable = ResultTables.GetOrCreate("Rms");

            paramTalbe.NewRow((IVector)CurrentResult.ResultMatrix.Estimated);
            rmsTable.NewRow(CurrentResult.ResultMatrix.StdOfEstimatedParam);
        }

        /// <summary>
        /// 输出结果。
        /// </summary>
        public void OutputResult()
        {
            this.ResultTables.WriteAllToFileAndClearBuffer();
        }
    }

    /// <summary>
    /// All in one. 平差文件构造器
    /// </summary>
    public class AioAdjustFileBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Directory"></param>
        /// <param name="Name"></param>
        public AioAdjustFileBuilder(string Directory, string Name = "平差文件")
        {
            AdjustTables = new ObjectTableManager(Directory);
            if (!String.IsNullOrWhiteSpace(Name))
            {
                AdjustTables.Name = Name;
            }            
        }
        /// <summary>
        /// 数据表
        /// </summary>
        public ObjectTableManager AdjustTables { get; set; }

        /// <summary>
        /// 添加一个。
        /// </summary>
        /// <param name="Adjustment"></param>
        public void AddAdjustment(AdjustResultMatrix Adjustment)
        {
            #region 必须的
            if(Adjustment.ObsMatrix.Tag is Time)
            {
                var epoch = (Time)Adjustment.ObsMatrix.Tag;

                var table = AdjustTables.GetOrCreate(AdjustName.Epoch);
                table.NewRow();
                table.AddItem(AdjustName.Epoch, epoch);
            }


            AdjustTables.GetOrCreate(AdjustName.ParamName).NewRow((List<string>)Adjustment.ParamNames);
            AdjustTables.GetOrCreate(AdjustName.Obs).NewRow((IVector)Adjustment.ObsMatrix.Observation);
            AdjustTables.GetOrCreate(AdjustName.RmsOfObs).NewRow(Adjustment.ObsMatrix.Observation.GetRmsVector());
            AdjustTables.GetOrCreate(AdjustName.Design).NewRow(Adjustment.ObsMatrix.Coefficient);
            #endregion

            if (!Vector.IsEmpty(Adjustment.ObsMatrix.ApproxVector))
            {
                AdjustTables.GetOrCreate(AdjustName.Approx).NewRow(Adjustment.ObsMatrix.ApproxVector);
            }         

            if (!Matrix.IsEmpty(Adjustment.ObsMatrix.Transfer))
            {
                AdjustTables.GetOrCreate(AdjustName.Trans).NewRow(Adjustment.ObsMatrix.Transfer);
                AdjustTables.GetOrCreate(AdjustName.RmsOfTrans).NewRow(Adjustment.ObsMatrix.Transfer.InverseWeight.Pow(0.5));
            }
            if (!Vector.IsEmpty(Adjustment.ObsMatrix.Apriori))
            {
                AdjustTables.GetOrCreate(AdjustName.Apriori).NewRow((IVector)Adjustment.ObsMatrix.Apriori);
                AdjustTables.GetOrCreate(AdjustName.RmsOfApriori).NewRow(Adjustment.ObsMatrix.Apriori.GetRmsVector());
            }
            if (!Vector.IsEmpty((IVector)Adjustment.Estimated))
            {
                AdjustTables.GetOrCreate(AdjustName.Estimated).NewRow((IVector)Adjustment.Estimated);
                AdjustTables.GetOrCreate(AdjustName.RmsOfEstimated).NewRow((IVector)Adjustment.Estimated.GetRmsVector());
            }
        }
      
        /// <summary>
        /// 写到文件
        /// </summary>
        public void WriteToFile()
        {
            AdjustTables.WriteAsOneFile();
        }

        /// <summary>
        /// 清空内存
        /// </summary>
        public void Clear()
        {
            AdjustTables.Clear();
        }
    }

    //2019.02.23, czs, create in hongqing, 平差文件构造器
    /// <summary>
    /// All in one. 平差文件构造器
    /// </summary>
    public class AdjustEquationFileBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Directory"></param>
        /// <param name="Name"></param>
        public AdjustEquationFileBuilder(string Directory,string Name = "平差文件")
        {
            this.Directory = Directory;
            AdjustTables = new MatrixEquationManager();

            if (!String.IsNullOrWhiteSpace(Name))
            {
                AdjustTables.Name = Name;
            } 
        }
        /// <summary>
        /// 输出目录
        /// </summary>
        public string Directory { get; set; }
        /// <summary>
        /// 数据表
        /// </summary>
        public MatrixEquationManager AdjustTables { get; set; }

        /// <summary>
        /// 添加一个。
        /// </summary>
        /// <param name="Adjustment"></param>
        public void AddAdjustment(AdjustResultMatrix Adjustment)
        {
            string name = "";
            if(Adjustment.ObsMatrix.Tag is Time)
            {
                var epoch = (Time)Adjustment.ObsMatrix.Tag;
                name = epoch.ToString();

                //var table = AdjustTables.GetOrCreate(AdjustName.Epoch);
                //table.NewRow();
                //table.AddItem(AdjustName.Epoch, epoch);
            }

            var obsMatrixEq = Adjustment.ObsMatrix.GetObsMatrixEquation(name);
            AdjustTables.Add(obsMatrixEq.Name, obsMatrixEq); 
        }
      
        /// <summary>
        /// 写到文件
        /// </summary>
        public void WriteToFile()
        {
            var tables = AdjustTables.GetObjectTables();
            tables.OutputDirectory = Directory;
            tables.WriteAsOneFile();
        }

        /// <summary>
        /// 清空内存
        /// </summary>
        public void Clear()
        {
            AdjustTables.Clear();
        }
    }
}