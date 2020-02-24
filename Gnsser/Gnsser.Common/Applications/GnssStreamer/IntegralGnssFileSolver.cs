//2016.09.02, czs, create in 西安洪庆, 多站观测数据集成解算器
//2018.07.27, czs, edit in HMX, 单站与多种进行多函数合并

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using Gnsser; 
using Geo.Referencing; 
using Geo.Utils; 
using Gnsser.Checkers;

namespace Gnsser
{
    /// <summary>
    /// 多站观测数据集成解算器
    /// </summary>
    public class IntegralGnssFileSolver : MultiSiteObsStreamer, IFileGnssSolver
    {
        Log log = new Log(typeof(IntegralGnssFileSolver));
        /// <summary>
        /// 构造函数
        /// </summary>
        public IntegralGnssFileSolver():base(Gnsser.Setting.GnsserConfig.TempDirectory )
        {
        }

        #region 属性
        /// <summary>
        /// 输出文件名称
        /// </summary>
        public string OutputFileName { get; set; }
        /// <summary>
        /// 模糊度文件存储路径
        /// </summary>
        public string AmbiguityStoragePath { get; set; }
        /// <summary>
        /// 模糊度管理器
        /// </summary>
        public AmbiguityManager AmbiguityManager { get; set; }
          
        /// <summary>
        /// 多历元构建器，用于多历元差分
        /// </summary>
         public MultiSitePeriodInfoBuilder MultiSitePeriodInfoBuilder { get; set; }
        /// <summary>
        /// 时段信息构建器
        /// </summary>
        public PeriodInformationBuilder PeriodInformationBuilder{ get; set; }
        /// <summary>
        /// 基准站名称
        /// </summary>
         public string BaseSiteName { get { return Context.ObservationDataSources.BaseDataSource.Name; } }

         /// <summary>
         /// 是否具有表格数据
         /// </summary>
         public bool HasTableData
         {
             get { return (this.TableTextManager != null && this.TableTextManager.Count != 0); }
         }
        
        #endregion
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pathes"></param>
         public override void Init(string [] pathes)
         {
             base.Init(pathes);

             this.OutputFileName = BuildOutFileNamePrefix(pathes);
             this.PeriodInformationBuilder = new Domain.PeriodInformationBuilder(true,this.Option.MultiEpochCount);

             AmbiguityManager = new AmbiguityManager(Option);
             AmbiguityStoragePath = Path.Combine(Option.OutputDirectory, Path.GetFileNameWithoutExtension(Pathes[0]) + "_AmbiguityStorage.ambi");

             if (Option.IsIndicatingApproxXyz)
             { 
                 DataSource.BaseDataSource.SiteInfo.SetApproxXyz(Option.InitApproxXyz);
             }
             if (Option.IsIndicatingApproxXyzRms)
             {
                 DataSource.BaseDataSource.SiteInfo.EstimatedXyzRms = Option.InitApproxXyzRms;
             }

             this.GnssResultBuilder = new GnssResultBuilder(this.TableTextManager, this.AioAdjustFileBuilder, AdjustEquationFileBuilder, Option, Context);
             Solver = BuildRolver(Context, Option);
         }

        public override void Complete()
        {
            base.Complete();
        }
        /// <summary>
        /// 构建文件输出前缀
        /// </summary>
        /// <param name="pathes"></param>
        /// <returns></returns>
        private  string BuildOutFileNamePrefix(string[] pathes)
         {
             StringBuilder sb = new StringBuilder();  
             sb.Append(this.Option.AdjustmentType + "Of" + pathes.Length + "FilesOf_");
             int i = 0;
             foreach (var path in pathes)
             {
                 if (i < 3) {
                     var name = Path.GetFileNameWithoutExtension(path);
                     sb.Append(name+"_");
                 }
                 if (i >= 3) { sb.Append("Etc"); break; }
                 i++;
             }
             return sb.ToString() ;
         }

         /// <summary>
         /// 处理一个历元
         /// </summary>
         /// <param name="mEpochInfo"></param>
         public override SimpleGnssResult Produce(MultiSiteEpochInfo mEpochInfo)
         {
             //3.计算
             // this.CurrentGnssResult
             BaseGnssResult GnssResult = null;
             if (Solver is MultiSitePeriodSolver)//多站多历元
             {
                if (this.MultiSitePeriodInfoBuilder == null)
                {
                    this.MultiSitePeriodInfoBuilder = new Domain.MultiSitePeriodInfoBuilder(Option);
                }

                MultiSitePeriodInfoBuilder.Add(mEpochInfo);
                 var period = MultiSitePeriodInfoBuilder.Build();
                 if (period == null || !period.Enabled) { return null; }

                 GnssResult = ((MultiSitePeriodSolver)Solver).Get(period);
             }
             else if (Solver is SingleSitePeriodSolver) //单站多历元
             {
                 PeriodInformationBuilder.Add(mEpochInfo.First);
                 var period = PeriodInformationBuilder.Build();
                 if (period == null || !period.Enabled) { return null; }

                 GnssResult = ((SingleSitePeriodSolver)Solver).Get(period);
             }
             else if (Solver is MultiSiteEpochSolver) {
                GnssResult = ((MultiSiteEpochSolver)Solver).Get(mEpochInfo);
            }
             else if (Solver is SingleSiteGnssSolver)//此处只计算基准流
             {
                 if (mEpochInfo.Contains(BaseSiteName)) { GnssResult = ((SingleSiteGnssSolver)Solver).Get(mEpochInfo[BaseSiteName]); }
                 else { GnssResult = null; }
             }

            if (GnssResult == null) { return null; }          
                           
             //4.结果后处理
             if (Option.IsFixingAmbiguity)
             {
                 //PPP模糊度处理
                 if (GnssResult is PppResult)
                 {
                     var PppResult = GnssResult as PppResult;
                 }

                 if (GnssResult is PeriodDoubleDifferPositionResult)
                 {
                     var result = GnssResult as PeriodDoubleDifferPositionResult;
                     AmbiguityManager.Regist(result);
                 }
             }
            return GnssResult;
         }
    }

}