//2014.08.26, czs, create, 抽象化单点定位计算
//2014.09.16, czs, refactor, 梳理各个过程，分为历元算前、算中和算后，增加初始化、检核等方法。
//2014.10.06，czs, edit in hailutu, 将EpochInfomation的构建独立开来，采用历元信息构建器IEpochInfoBuilder初始化
//2014.11.20，czs, edit in namu, 将PointPositioner命名为AbstractPointPositioner
//2016.03.10, czs, edit in hongqing, 重构设计
//2016.04.23, czs, edit in huoda, 分离数据源，名称修改为 StreamGnssService，意思为GNSS产品服务
//2016.05.02, czs, edit in hongqing, 分离检核，矫正，solver只是计算，其它的认为都准备好了
//2016.10.26, czs, edit in hongqing, 选星考虑周跳因素

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Times;
using Geo.IO;


namespace Gnsser.Service
{
 
    //2018.05.15, czs, create in hmx, 顶层抽象类
    /// <summary>
    /// GNSS 计算器，实质应该为服务，将生产分为赋值，矫正和计算。
    /// </summary>
    /// <typeparam name="TProduct"></typeparam>
    /// <typeparam name="TMaterial"></typeparam>
    public abstract class BaseAbstractGnssSolver<TProduct, TMaterial>
        : AbstractProcessService<TProduct, TMaterial>, IGnssSolver,   Namable
        where TProduct : AdjustmentResult
    {
        /// <summary>
        /// 日志
        /// </summary>
        Log log = new Log(typeof(BaseAbstractGnssSolver<TProduct, TMaterial>));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="option"></param>
        public BaseAbstractGnssSolver(DataSourceContext context, GnssProcessOption option)
        {
            this.DataSourceContext = context;
            this.Option = option;
            this.Name = context.ObservationDataSource == null ? "GNSS计算器" : context.ObservationDataSource.Name;

            if (option.IsEphemerisRequired && (this.DataSourceContext.EphemerisService == null || this.DataSourceContext.EphemerisService.Prns.Count == 0))
            {
                var info = "不可计算！星历不可为空！";
                log.Error(info);
                throw new ArgumentException(info);
            }
            this.IsBaseSatelliteRequried = Option.IsBaseSatelliteRequried;
            this.IsBaseSiteRequried = Option.IsBaseSiteRequried;
            this.BaseSiteName = Option.IndicatedBaseSiteName;
            this.IsFixingAmbiguity = Option.IsFixingAmbiguity;
            this.IsPostCheckEnabled = Option.IsResultCheckEnabled;
            this.MatrixAdjuster = AdjusterFactory.Create(this.Option.AdjustmentType);
            if(MatrixAdjuster is RecursiveAdjuster)
            {
                ((RecursiveAdjuster)MatrixAdjuster).SetStepOfRecursive(Option.StepOfRecursive);
            }

            this.ElementResidualCheckers = new PostResidualCheckerManager(this.Option.MaxErrorTimesOfPostResdual);
            AdjustChecker = GnssResultCheckingManager.GetDefault(Option);

            this.Init();
        }

        #region  基础属性
        /// <summary>
        /// 是否需要基准卫星类型
        /// </summary>
        public bool IsBaseSatelliteRequried { get; set; }
        /// <summary>
        /// 是否需要基准测站类型
        /// </summary>
        public bool IsBaseSiteRequried { get; set; }
        /// <summary>
        /// 是否固定模糊度
        /// </summary>
        public bool IsFixingAmbiguity { get; set; }
        /// <summary>
        /// 当前的基准卫星
        /// </summary>
        public SatelliteNumber CurrentBasePrn { get; set; }
        /// <summary>
        /// 基准测站名称
        /// </summary>
        public string BaseSiteName { get;
            set; }
        /// <summary>
        /// 基准卫星系统
        /// </summary>
        public SatelliteType BaseSatType { get; set; }
        /// <summary>
        /// 数据分析专家。对数据预先进行分析。
        /// </summary>
        public IObsDataAnalyst ObsDataAnalyst { get; set; }
        /// <summary>
        /// 基础的，不变的参数数量，如伪距定位通常为 4.如果PPP则为5，其它为可变的卫星模糊度参数。 
        /// </summary>
        public int BaseParamCount { get; set; }
        /// <summary>
        /// 数据源上下文
        /// </summary>
        public DataSourceContext DataSourceContext { get; set; }

        /// <summary>
        /// 定位计算选项
        /// </summary>
        public GnssProcessOption Option { get; set; }

        /// <summary>
        /// 平差计算器
        /// </summary>
        public AdjustResultMatrix Adjustment { get; set; }
        /// <summary>
        /// 平差计算器
        /// </summary>
        public MatrixAdjuster MatrixAdjuster{ get; set; } 
        /// <summary>
        /// 平差结果检核管理器
        /// </summary>
        public  GnssResultCheckingManager AdjustChecker { get; set; }
        /// <summary>
        /// 矩阵生成器
        /// </summary>
        public SimpleBaseGnssMatrixBuilder MatrixBuilder { get; set; }

        #endregion
        /// <summary>
        /// 即将生产前产生。
        /// </summary>
        /// <param name="material"></param>
        protected override void OnProducing(TMaterial material)
        {
            base.OnProducing(material);
        }

        /// <summary>
        ///如果具有缓存，则可以在此根据缓存进行预处理
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <returns></returns>
        public virtual void HandleBuffer(ref IBuffer<TMaterial> epochInfo)
        {

        }

        /// <summary>
        /// 生成产品，包括矩阵生成，计算和结果输出。
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public override TProduct Produce(TMaterial material)
        {
            var product = default(TProduct);
            //do{
                //设置矩阵
                BuildAdjustMatrix();
                //展开计算
                if (this.Option.CaculateType == CaculateType.Filter)
                {
                    product = CaculateKalmanFilter(material, this.CurrentProduct);
                }
                if (Option.CaculateType == CaculateType.Independent)
                {
                    product = CaculateIndependent(material);
                }
            
            //残差分量分析
            //} while (Option.IsResidualCheckEnabled && !ElementResidualCheck(product));
            
            return product;
        }
        /// <summary>
        /// 残差分量检查
        /// </summary>
        protected PostResidualCheckerManager  ElementResidualCheckers { get; set; }

        BaseAdjustMatrixBuilder IGnssSolver.MatrixBuilder => throw new NotImplementedException();
        
        /// <summary>
        /// 更新矩阵构建器
        /// </summary>
        public virtual void BuildAdjustMatrix()
        {
             this.MatrixBuilder.SetMaterial(this.CurrentMaterial).SetPreviousProduct(this.CurrentProduct).Build();
        }

        /// <summary>
        /// 独立计算，默认为无先验信息的卡尔曼滤波
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public virtual TProduct CaculateIndependent(TMaterial material)
        {
            return CaculateKalmanFilter(material, default(TProduct));
        }

        /// <summary>
        /// Kalmam滤波。
        /// </summary>
        /// <param name="material">接收信息</param> 
        /// <param name="last">上次解算结果（用于 Kalman 滤波）,若为null则使用初始值计算</param>
        /// <returns></returns>
        public virtual TProduct CaculateKalmanFilter(TMaterial material, TProduct last = default(TProduct))
        {
            AdjustObsMatrix obsMatrix = BuildAdjustObsMatrix(material);
              
            this.Adjustment = this.RunAdjuster(obsMatrix);

            if (Adjustment.Estimated == null) return default(TProduct);
            // return BuildResult();

            return BuildResult();
        }
        /// <summary>
        /// 运行计算器
        /// </summary>
        /// <param name="obsMatrix"></param>
        /// <returns></returns>
        protected virtual AdjustResultMatrix RunAdjuster(AdjustObsMatrix obsMatrix)
        {
            AdjustResultMatrix result = null;
            if (this.Option.IsAdjustEnabled)
            {
                result = this.MatrixAdjuster.Run(obsMatrix);
            }
            else
            {
                result = new AdjustResultMatrix() { ObsMatrix = obsMatrix };
            }
            return result;
        }
        /// <summary>
        /// 构建平差观测矩阵 
        /// </summary>
        /// <param name="material">接收信息</param> 
        /// <returns></returns>
        public virtual AdjustObsMatrix BuildAdjustObsMatrix(TMaterial material)
        {
            return new AdjustObsMatrix(this.MatrixBuilder) { Tag = material};
        }


        #region 检验


        /// <summary>
        /// 算后结果处理，主要检查方差是否过大，剔除陡峭数据
        /// </summary> 
        /// <param name="product">本次历元计算结果</param>
        /// <returns>如果为null，则表示计算失败，或结果被剔除</returns>
        public override bool CheckProduct(TProduct product)
        {
            if (product is BaseGnssResult)
            {
                BaseGnssResult ppResult = product as BaseGnssResult;
                if (ppResult == null) return false;

                //对解算结果进行评价，决定计算结果是否有效。 
                if (!AdjustChecker.Check(ppResult))
                {
                    //throw checker.Exception;
                   log.Info(ppResult.Name + " 计算结果未通过检核！在：" + ppResult + ", " + AdjustChecker.Exception.Message);
                    return false;
                }
            }
            return true;
        }

        #endregion

        /// <summary>
        /// 生成结果
        /// </summary>
        /// <returns></returns>
        public abstract TProduct BuildResult();

    }
} 