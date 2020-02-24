//2014.08.26, czs, create, 抽象化单点定位计算
//2014.09.16, czs, refactor, 梳理各个过程，分为历元算前、算中和算后，增加初始化、检核等方法。
//2014.10.06，czs, edit in hailutu, 将EpochInfomation的构建独立开来，采用历元信息构建器IEpochInfoBuilder初始化
//2014.11.20，czs, edit in namu, 将PointPositioner命名为AbstractPointPositioner
//2016.03.10, czs, edit in hongqing, 重构设计
//2016.04.23, czs, edit in huoda, 分离数据源，名称修改为 StreamGnssService，意思为GNSS产品服务
//2016.05.02, czs, edit in hongqing, 分离检核，矫正，solver只是计算，其它的认为都准备好了
//2016.10.26, czs, edit in hongqing, 选星考虑周跳因素
//2018.08.03, czs, edit in HMX, 提取公共函数，适合动态定位
//2018.10.19, czs, edit in hmx， 模糊度增加RMS信息

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

    /// <summary>
    /// GNSS 计算器，实质应该为服务，将生产分为赋值，矫正和计算。
    /// </summary>
    /// <typeparam name="TProduct"></typeparam>
    /// <typeparam name="TMaterial"></typeparam>
    public abstract class AbstractGnssSolver<TProduct, TMaterial>
        : BaseAbstractGnssSolver<TProduct, TMaterial>,
        Namable, IGnssSolver<TProduct, TMaterial>
        where TMaterial : ISiteSatObsInfo
        where TProduct : BaseGnssResult
    {
        /// <summary>
        /// 日志
        /// </summary>
        Log log = new Log(typeof(AbstractGnssSolver<TProduct, TMaterial>));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="option"></param>
        public AbstractGnssSolver(DataSourceContext context, GnssProcessOption option) : base(context, option)
        {
            this.Name = context.ObservationDataSource == null ? "GNSS计算器" : context.ObservationDataSource.Name;
            //为所有测站附加天线信息，为了保险起见多赋值一次。
            if (context.AntennaDataSource != null)
            {
                foreach (var item in context.ObservationDataSources.DataSources)
                {
                    item.SiteInfo.Antenna = context.AntennaDataSource.Get(item.SiteInfo.AntennaType);
                }
            }

            //是否需要伪距预先定位
            if (Option.IsNeedPseudorangePositionWhenProcess && !(this is SimpleRangePositioner))
            {
                int interval = (int)this.DataSourceContext.ObservationDataSource.ObsInfo.Interval;
                GnssProcessOption opt = GnssProcessOption.GetPsuedoRangeOption(interval, option.IsSmoothRangeWhenPrevPseudorangePosition);

                RangePositioner = new SimpleRangePositioner(DataSourceContext, opt);
            }

            this.IsUpdateEstimatePostition = option.IsUpdateEstimatePostition;
            this.EpochEphemerisSetter = new EpochEphemerisSetter(DataSourceContext, option);

            //模糊度
            IsOutputAmbiguity = true;
            AmbiguityManager = new AmbiguityManager(base.Option);
            IonoFreeAmbiguitySolverManager = new IonoFreeAmbiguitySolverManager();
        }

        #region  基础属性 
        /// <summary>
        /// 基准卫星信息
        /// </summary>
        public FlexiblePeriodSatSelector PeriodPrnManager { get; set; }
        /// <summary>
        /// 无电离层模糊度系数计算
        /// </summary>
        public IonoFreeAmbiguitySolverManager IonoFreeAmbiguitySolverManager { get; set; }
        /// <summary>
        /// 星历赋值器，以备不时之需
        /// </summary>
        protected EpochEphemerisSetter EpochEphemerisSetter { get; set; }
        /// <summary>
        /// 简单伪距定位
        /// </summary>
        public SimpleRangePositioner RangePositioner { get; set; }

        /// <summary>
        /// 是否更新测站估计坐标
        /// </summary>
        public bool IsUpdateEstimatePostition { get; set; }

        ///<summary>
        /// 平差结果检核管理器
        /// </summary>
        //GnssResultCheckingManager AdjustChecker { get; set; }
        /// <summary>
        /// 矩阵生成器
        /// </summary>
        public new BaseGnssMatrixBuilder<TProduct, TMaterial> MatrixBuilder { get; set; }
        /// <summary>
        /// Override
        /// </summary>
        BaseAdjustMatrixBuilder IGnssSolver.MatrixBuilder { get { return MatrixBuilder; } }
        #region 模糊度固定配置
        /// <summary>
        /// 模糊度管理器
        /// </summary>
        public AmbiguityManager AmbiguityManager { get; set; }
        /// <summary>
        /// 是否使用外部模糊度
        /// </summary>
        public bool IsUsingOutComeAmbituity { get; set; }
        /// <summary>
        /// 是否输出模糊度
        /// </summary>
        public bool IsOutputAmbiguity { get; set; }
        /// <summary>
        /// 模糊度存储
        /// </summary>
        public PeriodRmsedNumeralStoarge AmbiguityStoarge { get => AmbiguityManager.AmbiguityProduct; }
        #endregion

        #endregion

        /// <summary>
        /// 即将生产前产生。
        /// </summary>
        /// <param name="material"></param>
        protected override void OnProducing(TMaterial material)
        {
            if (material.EnabledPrns.Count == 0) { return; }
            if (this.IsBaseSiteRequried)
            {
                this.BaseSiteName = SelectBaseSiteName();
            }

            if (this.IsBaseSatelliteRequried)
            {
                if (IsRequreReselectBaseSat())
                {
                    var prevBasePrn = this.CurrentBasePrn;

                    this.CurrentBasePrn = SelectBaseSatellite();

                    if (this.Adjustment == null) { return; }

                    if (!this.Option.GnssSolverType.ToString().Contains("定轨") && this.Option.PositionType == PositionType.动态定位)
                    {
                        //如果是双差动态计算，在基准星改变后，应采用上一计算XYZ和新的基准星，估计此基准星的模糊度值，做为初值, 这样可以避免发生结果跳跃
                        var prevMaterial = this.PrevMaterial;
                        var prevResult = this.PrevProduct;
                        //依然用上一矩阵，只修改基准星和XYZ转移阵
                        this.MatrixBuilder.SetBasePrn(this.CurrentBasePrn).Build();
                        var obsMatrix = BuildAdjustObsMatrix(this.CurrentMaterial);//  BuildAdjustObsMatrix(this.MatrixBuilder);

                        //此处采用静态转移阵
                        obsMatrix.Transfer[0, 0] = 1;
                        obsMatrix.Transfer[1, 1] = 1;
                        obsMatrix.Transfer[2, 2] = 1;
                        obsMatrix.Transfer.InverseWeight[0, 0] = Math.Pow(Option.StdDevOfStaticTransferModel, 2);
                        obsMatrix.Transfer.InverseWeight[1, 1] = Math.Pow(Option.StdDevOfStaticTransferModel, 2);
                        obsMatrix.Transfer.InverseWeight[2, 2] = Math.Pow(Option.StdDevOfStaticTransferModel, 2);

                        var paramCount = Adjustment.ParamCount;
                        for (int i = 3; i < paramCount; i++)
                        {
                            obsMatrix.Transfer[i, i] = 0;
                            obsMatrix.Transfer.InverseWeight[i, i] = Math.Pow(Option.StdDevOfCycledPhaseModel, 2);
                        }

                        //平差
                        this.Adjustment = this.RunAdjuster(obsMatrix);

                        var result = BuildResult();

                        this.SetProduct(result);
                    }
                }
            }
            base.OnProducing(material);
        }

        /// <summary>
        ///如果具有缓存，则可以在此根据缓存进行预处理
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <returns></returns>
        public override void HandleBuffer(ref IBuffer<TMaterial> epochInfo)
        {

        }

        /// <summary>
        /// 生成产品，包括矩阵生成，计算和结果输出。
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public override TProduct Produce(TMaterial material)
        {
            if (material.EnabledSatCount == 0) { log.Warn("可用卫星数为 0 ！是否选错了系统？"); return null; }

            int loopIndex = 0;
            var product = default(TProduct);
            do
            {
                loopIndex++;

                //如果观测数量不足，舍弃本历元
                if (material.EnabledSatCount < Option.MinSatCount)
                {
                    log.Warn("卫星数量不足（可能是验后残差删除的原因）， " + this.CurrentMaterial.EnabledSatCount + " < " + Option.MinSatCount);
                    return null;
                }
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
                if (product == null)
                {
                    return product;
                }
                //结果检核
            } while (IsPostCheckEnabled && loopIndex < this.Option.MaxLoopCount && Option.IsResidualCheckEnabled && !ElementResidualCheck(product));

            return product;
        }



        /// <summary>
        /// 更新矩阵构建器
        /// </summary>
        public override void BuildAdjustMatrix()
        {
            if (!this.CurrentMaterial.SatelliteTypes.Contains(BaseSatType))
            {
                log.Info("切换基准系统 " + BaseSatType + " -> " + this.CurrentMaterial.SatelliteTypes[0]);
                BaseSatType = this.CurrentMaterial.SatelliteTypes[0];
            }

            this.MatrixBuilder.SetMaterial(this.CurrentMaterial).SetBaseSiteName(BaseSiteName).SetPreviousProduct(this.CurrentProduct).SetBaseSatType(BaseSatType).SetBasePrn(this.CurrentBasePrn).Build();
        }

        /// <summary>
        /// 独立计算，默认为无先验信息的卡尔曼滤波
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public override TProduct CaculateIndependent(TMaterial material)
        {
            return CaculateKalmanFilter(material, default(TProduct));
        }

        /// <summary>
        /// Kalmam滤波。
        /// </summary>
        /// <param name="material">接收信息</param> 
        /// <param name="last">上次解算结果（用于 Kalman 滤波）,若为null则使用初始值计算</param>
        /// <returns></returns>
        public override TProduct CaculateKalmanFilter(TMaterial material, TProduct last = default(TProduct))
        {

            //检查是否采用伪距预先定位且是动态定位，若是，则采用新坐标和钟差更新先验向量
            this.CheckOrUpdateNextPrevEstCoordWithRangePositioning(material, last);


            //构建矩阵并进行平差计算
            this.BuildMatrixAndAdjust();

            if (Adjustment.Estimated == null)
            {
                return default(TProduct);
            }

            //结果
            var theResult = BuildResult();

            theResult.ResultMatrix.ResultType = ResultType.Float;

            if (!Option.TopSpeedModel)
            {
                //检查并尝试固定模糊度
                theResult = this.CheckOrTryToGetAmbiguityFixedResult(theResult);
            }

            //是否更新测站估值
            this.CheckOrUpdateEstimatedCoord(material, theResult);

            return theResult;
        }

        /// <summary>
        /// 生成平差观测矩阵
        /// </summary>
        public override AdjustObsMatrix BuildAdjustObsMatrix(TMaterial material)
        {
            var obsMatrix = new AdjustObsMatrix(MatrixBuilder) { Tag = material.ReceiverTime };
            return obsMatrix;
        }

        public override void Complete()
        {
            base.Complete();
            if (this.IsFixingAmbiguity)
            {
                //双差模糊度浮点解存储
                if (AmbiguityManager.AmbiguityStorage != null)
                {
                    var name = this.CurrentProduct == null ? "NoName" : this.CurrentProduct.Name;
                    var path = System.IO.Path.Combine(this.Option.OutputDirectory, name + "_Ambiguities" + Setting.AmbiguityFileExtension);

                    AmbiguityManager.SaveProduct(path);
                }
            }

        }

        #region 计算细节
        /// <summary>
        /// 构建矩阵并进行平差计算
        /// </summary>
        protected virtual void BuildMatrixAndAdjust()
        {
            //是否提供了初始先验信息，即：第一个历元不用计算，其值已知。
            //if (this.MatrixBuilder.IsEstmatedProvided)
            //{
            //    this.Adjustment = new AdjustResultMatrix();
            //    this.Adjustment.ObsMatrix = BuildAdjustObsMatrix(this.MatrixBuilder);

            //    this.Adjustment.Estimated = this.Adjustment.ObsMatrix.Apriori;
            //    log.Info("由于采用了初始估值信息，本历元不进行计算。");
            //}
            //else
            {
                //如果指定了先验值，则认为其准确，则移除周跳标记。
                if (this.CurrentIndex == 0 && this.Option.IsInitAprioriAvailable)
                {
                    this.CurrentMaterial.RemoveUnStableMarkers();
                }

                AdjustObsMatrix obsMatrix = BuildAdjustObsMatrix(this.CurrentMaterial);
                 
                this.Adjustment = RunAdjuster(obsMatrix);
            }
        }
       

        /// <summary>
        /// 检查，如果采用动态定位，则采用伪距预先定位进行更新
        /// </summary>
        /// <param name="material"></param>
        /// <param name="lastResult">上一次定位结果</param>
        protected virtual void CheckOrUpdateNextPrevEstCoordWithRangePositioning(TMaterial material, TProduct lastResult)
        {
            //动态定位  EpochInformation  
            if (RangePositioner != null && Option.PositionType == PositionType.动态定位)
            {
                EpochInformation epochInfo = null;

                if (material is EpochInformation)
                {
                    epochInfo = material as EpochInformation;
                }
                if (material is MultiSiteEpochInfo)
                {
                    var epochInfos = material as MultiSiteEpochInfo;
                    epochInfo = epochInfos.OtherEpochInfo;
                }


                if (epochInfo == null) { return; }

                var psuedoResult = RangePositioner.Get(epochInfo);

                //更新
                if (psuedoResult != null)
                {
                    epochInfo.SiteInfo.EstimatedXyz = psuedoResult.EstimatedXyz;
                }
                if (lastResult != null && psuedoResult != null)
                {
                    //更新矩阵
                    if (this is SingleSiteGnssSolver)
                    {
                        //以当前伪距定位计算结果作为更新值
                        lastResult.ResultMatrix.Estimated.SetSubVector(psuedoResult.ResultMatrix.Estimated);
                        //lastResult.ResultMatrix.Estimated[0] = 0;
                        //lastResult.ResultMatrix.Estimated[1] = 0;
                        //lastResult.ResultMatrix.Estimated[2] = 0;
                        //lastResult.ResultMatrix.Estimated[3] = 0;

                        //方差
                        var cova = psuedoResult.ResultMatrix.CovaOfEstimatedParam;
                        lastResult.ResultMatrix.Estimated.InverseWeight.SetRowColValue(0, 3, 0); //clear

                        lastResult.ResultMatrix.Estimated.InverseWeight[0, 0] = cova[0, 0];
                        lastResult.ResultMatrix.Estimated.InverseWeight[1, 1] = cova[1, 1];
                        lastResult.ResultMatrix.Estimated.InverseWeight[2, 2] = cova[2, 2];
                        lastResult.ResultMatrix.Estimated.InverseWeight[3, 3] = cova[3, 3];
                    }

                    if (false && this is EpochDoubleDifferPositioner || this is IonFreeDoubleDifferPositioner)
                    {
                        //以当前伪距定位计算结果作为更新值
                        lastResult.ResultMatrix.Estimated.SetSubVector(psuedoResult.ResultMatrix.Estimated.GetSubVector(0, 3));

                        //方差
                        var cova = psuedoResult.ResultMatrix.CovaOfEstimatedParam;
                        lastResult.ResultMatrix.Estimated.InverseWeight.SetRowColValue(0, 2, 0); //clear

                        lastResult.ResultMatrix.Estimated.InverseWeight[0, 0] = cova[0, 0];
                        lastResult.ResultMatrix.Estimated.InverseWeight[1, 1] = cova[1, 1];
                        lastResult.ResultMatrix.Estimated.InverseWeight[2, 2] = cova[2, 2];
                    }
                }
            }
        }


        /// <summary>
        /// 检查更新估值坐标
        /// </summary>
        /// <param name="material"></param>
        /// <param name="currentResult"></param>
        protected void CheckOrUpdateEstimatedCoord(TMaterial material, TProduct currentResult)
        {
            //实时更新测站坐标
            if (this.IsUpdateEstimatePostition)
            {
                if (material is EpochInformation)
                {
                    var epochResult = material as EpochInformation;

                    epochResult.SiteInfo.EstimatedXyz = currentResult.EstimatedXyz;
                }
                else if (currentResult is IWithEstimatedBaseline)
                {
                    var epochInfos = material as MultiSiteEpochInfo;
                    var result = currentResult as IWithEstimatedBaseline;

                    epochInfos.OtherEpochInfo.SiteInfo.EstimatedXyz = result.GetEstimatedBaseline().EstimatedXyzOfRov;
                }

            }
        }

        #endregion

        #region 检验

        /// <summary>
        /// 残差分量检查
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool ElementResidualCheck(TProduct product)
        {
            var postResiduals = product.ResultMatrix.PostfitResidual;

            var badPrns = ElementResidualCheckers.GetBadPrns(product);
            if (badPrns.Count == 0) { return true; }

            if (this.CurrentMaterial is EpochInformation)
            {
                var epochInfo = this.CurrentMaterial as EpochInformation;
                var info = epochInfo.Name + ", " + product.ReceiverTime + ", 验后残差检查不通过，即将删除：" + Geo.Utils.EnumerableUtil.ToString(badPrns);

                epochInfo.Remove(badPrns, true, info);
                return false;
            }
            else if (this.CurrentMaterial is MultiSiteEpochInfo)
            {
                var epochInfo = this.CurrentMaterial as MultiSiteEpochInfo;
                var info = epochInfo.Name + ", " + product.ReceiverTime + ", 验后残差检查不通过，即将删除：" + Geo.Utils.EnumerableUtil.ToString(badPrns);
                foreach (var item in epochInfo)
                {
                    item.Remove(badPrns, true, info);
                }
                return false;
            }
            else if (this.CurrentMaterial is MultiSitePeriodInfo)
            {
                var epochInfo = this.CurrentMaterial as MultiSitePeriodInfo;
                var info = epochInfo.Name + ", " + product.ReceiverTime + ", 验后残差检查不通过，即将禁用：" + Geo.Utils.EnumerableUtil.ToString(badPrns);
                log.Warn(info);
                epochInfo.Disable(badPrns);
                return false;
            }

            return true;
        }


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
                    log.Warn(ppResult.Name + " 计算结果未通过检核！在观测历元：" + ppResult + ", " + AdjustChecker.Exception.Message);
                    return false;
                }
            }
            return true;
        }

        #endregion         

        #region  基准星的选择
        /// <summary>
        /// 判断是否需要重新选基准星。选星考虑存在否和周跳因素
        /// </summary>
        /// <returns></returns>
        public bool IsRequreReselectBaseSat()
        {
            //判断是否需要重新选星
            if (this.CurrentBasePrn == SatelliteNumber.Default)
            {
                return true;
            }
            //启用星不包含基准星
            if (!this.CurrentMaterial.EnabledPrns.Contains(CurrentBasePrn))
            {
                return true;
            }
            //基准星具有周跳，其它存在没有周跳的星。
            if (this.CurrentMaterial.UnstablePrns.Count < this.CurrentMaterial.EnabledPrns.Count)
            {
                if (this.CurrentMaterial.HasCycleSlip(CurrentBasePrn)) return true;
            }

            return false;
        }

        /// <summary>
        /// 选择基准测站
        /// </summary>
        /// <returns></returns>
        public virtual string SelectBaseSiteName()
        {
            if (String.IsNullOrWhiteSpace(this.BaseSiteName))
            {
                if (CurrentMaterial is MultiSitePeriodInfo)
                {
                    var material = (CurrentMaterial as MultiSitePeriodInfo);
                    this.BaseSiteName = material[0].BaseSiteName;
                }
                else if (CurrentMaterial is MultiSiteEpochInfo)
                {
                    var material = (CurrentMaterial as MultiSiteEpochInfo);
                    this.BaseSiteName = material.BaseSiteName;
                }
                else if (CurrentMaterial is EpochInformation)
                {
                    this.BaseSiteName = (CurrentMaterial as EpochInformation).SiteName;
                }
            }
            return BaseSiteName;
        }

        /// <summary>
        /// 根据历史和预处理统计信息，选择基准卫星。 
        /// 还应该加入周跳判断！！ czs , 2016.10.26,
        /// </summary>
        /// <returns></returns>
        public virtual SatelliteNumber SelectBaseSatellite()
        {
            //选星
            var prevBasePrn = this.CurrentBasePrn;

            //if(PeriodPrnManager == null)
            //{
            //    string path = this.DataSourceContext.ObservationDataSource.Path;
            //    if (this.CurrentMaterial is MultiSiteEpochInfo)
            //    {
            //        path = this.DataSourceContext.ObservationDataSources.OtherDataSource.Path;
            //    }
            //    this.PeriodPrnManager = BaseSatelliteSelectorFactory.GetPeriodPrnManager(Option, path); 
            //}

            //if(this.PeriodPrnManager != null)
            //{
            //    var obj = this.PeriodPrnManager.BuildOnePeriodPrnByCenterTop(CurrentMaterial.ReceiverTime.DateTime);
            //    prevBasePrn = obj.Value;
            //    log.Info(this.Name + " " + this.CurrentMaterial.ReceiverTime + ", 当前选择的基准卫星：" + prevBasePrn + ", 选星器选择结果。");
            //}

            //1.优先手动指定
            if (!this.CurrentMaterial.EnabledPrns.Contains(prevBasePrn))
            {
                if (Option.IsIndicatedPrn)
                {
                    prevBasePrn = this.Option.IndicatedPrn;
                    log.Info(this.Name + " " + this.CurrentMaterial.ReceiverTime + ", 当前选择的基准卫星：" + prevBasePrn + ", 配置文件指定的。");
                }
            }

            if (!this.CurrentMaterial.EnabledPrns.Contains(prevBasePrn))
            {
                if (ObsDataAnalyst == null)
                {
                    var path = this.DataSourceContext.ObservationDataSource.Path;
                    if (this.CurrentMaterial is MultiSiteEpochInfo)
                    {
                        path = this.DataSourceContext.ObservationDataSources.OtherDataSource.Path;
                    }

                    var stream = new RinexFileObsDataSource(path);
                    ObsDataAnalyst = new ObsDataAnalyst(stream, Option.SatelliteTypes);
                }
                if (ObsDataAnalyst != null)  //2.没有上次卫星，重新选一颗前途较好的卫星
                {
                    var prns = ObsDataAnalyst.SatelliteSelector.GetSortedSatPeriods(CurrentMaterial.ReceiverTime);
                    var sat = prns[0];
                    foreach (var item in prns)
                    {
                        if (this.CurrentMaterial.EnabledPrns.Contains(item.Prn))
                        {
                            sat = item;
                            prevBasePrn = item.Prn;
                            break;
                        }
                    }
                    log.Info(this.Name + " " + this.CurrentMaterial.ReceiverTime + ", 当前选择的基准卫星：" + sat + ", 外置分析器选择结果。");
                }
            }

            //3.如果没有，则选择高度角最大的一个。
            if (!this.CurrentMaterial.EnabledPrns.Contains(prevBasePrn))
            {
                if (CurrentMaterial is MultiSitePeriodInfo)
                {
                    MultiSitePeriodInfo info = CurrentMaterial as MultiSitePeriodInfo;
                    var list = info.First.BaseEpochInfo.GetMaxElevationPrns();
                    foreach (var item in list)
                    {
                        if (this.CurrentMaterial.EnabledPrns.Contains(item))
                        {
                            prevBasePrn = item;
                            break;
                        }
                    }
                }
                else if (CurrentMaterial is EpochInformation)
                {
                    var info = CurrentMaterial as EpochInformation;
                    prevBasePrn = info.GetMaxElevationPrn();
                }
                else if (CurrentMaterial is MultiSiteEpochInfo)
                {
                    var info = (CurrentMaterial as MultiSiteEpochInfo);
                    prevBasePrn = info.First.GetMaxElevationPrn();
                }
                //     BasePrn = this.CurrentMaterial.First.BaseEpochInfo.GetMaxElevationPrns();
                log.Info(this.Name + " " + this.CurrentMaterial.ReceiverTime + ", 当前选择的基准卫星：" + prevBasePrn + ", 最大高度角法。");
            }

            //4.最后，如果都没有，则选择第一个,这一句不可能啦
            if (!this.CurrentMaterial.EnabledPrns.Contains(prevBasePrn))
            {
                prevBasePrn = this.CurrentMaterial.EnabledPrns[0];
                log.Info(this.Name + " " + this.CurrentMaterial.ReceiverTime + ", 当前选择的基准卫星为第一个：" + prevBasePrn + ", 注意，算法可能发生了更改，请检核！！");
            }
            return prevBasePrn;
        }

        #endregion

        #region  模糊度固定算法

        /// <summary>
        /// 检查是否可以固定模糊度，如果可以，则尝试固定，并返回结果
        /// </summary>
        /// <param name="theResult"></param>
        /// <returns></returns>
        protected TProduct CheckOrTryToGetAmbiguityFixedResult(TProduct theResult)
        {
            //模糊度固定解
            var norm = theResult.ResultMatrix.Estimated.GetRmsVector().Norm();
            if (theResult is IFixableParamResult
                && this.IsFixingAmbiguity
                && (//固定参数启用条件
                norm < Option.MaxFloatRmsNormToFixAmbiguity
                || Option.IsUseFixedParamDirectly
                ))
            {
                var result = theResult as IFixableParamResult;
                WeightedVector fixedIntAmbiCycles = null;
                if (!TryFixeAmbiguites(result, out fixedIntAmbiCycles))
                {
                    theResult.ResultMatrix.ResultType = ResultType.Float;
                    return theResult;
                }

                //固定成功，则将浮点解作为虚拟观测值，整数解作为约束进行条件平差
                //保存到结果，用于输出
                var fixedVec = fixedIntAmbiCycles;
                var preFixedVec = result.FixedParams;//上一次固定结果，这里好像没有用，//2018.07.31， czs
                //RtkLib中具有瞬时、hold，fixd等选选项，此处我们暂时只为瞬时。。。//2018.10.16，czs


                //此处应该考虑非模糊度，如坐标的情况。有所区分，可以从名称入手
                //外部文件不参与单位换算
                if (!IsOuterAmbiguityFileAvailable && Option.IsPhaseInMetterOrCycle)  //恢复模糊度为米,计算固定解
                {
                    fixedVec = ConvertCycleAmbiguityToMeter(fixedIntAmbiCycles);
                    preFixedVec = ConvertCycleAmbiguityToMeter(preFixedVec);
                }

                //条件平差
                WeightedVector NewEstimated = Adjustment.SolveAmbiFixedResult(fixedVec, preFixedVec, Option.IsFixParamByConditionOrHugeWeight);

                //update
                result.FixedParams = fixedIntAmbiCycles;
                result.ResultMatrix.Estimated = NewEstimated;
                result.ResultMatrix.ResultType = ResultType.Fixed;

                //重新计算残差,不必重新计算，残差总是实时计算的！！2018.12.17, czs, hmx
                // result.ResultMatrix.PostfitResidual


                return (TProduct)result;
            }
            return theResult;
        }



        /// <summary>
        /// 尝试固定模糊度，成功返回ture，并传递成功后的赋值。
        /// </summary>
        /// <param name="result"></param>
        /// <param name="fixedParams"></param>
        /// <returns></returns>
        public virtual bool TryFixeAmbiguites(IFixableParamResult result, out WeightedVector fixedParams)
        {
            var floatSolMetter = result.ResultMatrix.Estimated;
            //尝试固定模糊度
            fixedParams = FixePhaseAmbiguity(result);

            #region 判断是否成功
            //模糊度固定失败，直接返回浮点数结果。 
            var floatSolCyle = result.GetFixableVectorInUnit();
            RemoveBiasedIntSolution(floatSolCyle, fixedParams, Option.MaxAmbiDifferOfIntAndFloat);
            var ratio = fixedParams.Count * 1.0 / (floatSolCyle.Count * 1.0);
            if (ratio < this.Option.MinFixedAmbiRatio)
            {
                log.Warn("模糊度固定成功率只有 " + ratio + ",少于指定的 " + this.Option.MinFixedAmbiRatio + ",  本历元取消固定 " + this.CurrentMaterial.ReceiverTime);
                return false;
            }

            //保存模糊度
            if (IsOutputAmbiguity)//&& !IsUsingOutComeAmbituity)
            {
                AmbiguityManager.Regist(this.CurrentMaterial.ReceiverTime, fixedParams);
            }

            #endregion
            return true;
        }

        /// <summary>
        /// 移除固定时变的。
        /// 允许整数与浮点数的最大偏差，如 浮点数为 0.1 而整数为 1，则认为失败。
        /// </summary>
        /// <param name="floatSolCyle"></param>
        /// <param name="fixedIntAmbiCycles"></param>
        /// <param name="maxDifferOfIntAndFloatAmbi"></param>
        protected void RemoveBiasedIntSolution(WeightedVector floatSolCyle, WeightedVector fixedIntAmbiCycles, double maxDifferOfIntAndFloatAmbi = 0.6)
        {
            // 关键问题：模糊度固定是否正确？？
            //允许整数与浮点数的最大偏差，如 浮点数为 0.1 而整数为 1，则认为失败。
            List<string> failedParams = new List<string>();
            foreach (var name in fixedIntAmbiCycles.ParamNames)
            {
                var differ = Math.Abs(fixedIntAmbiCycles[name] - floatSolCyle[name]);
                if (differ > maxDifferOfIntAndFloatAmbi)
                {
                    log.Debug(name + " 模糊度固值解与浮点值偏差达 " + differ + " > " + maxDifferOfIntAndFloatAmbi + ", 标记失败！");
                    failedParams.Add(name);
                }
            }
            fixedIntAmbiCycles.Remove(failedParams);
        }


        /// <summary>
        /// 尝试固定模糊度，成功返回ture，和成功后的赋值。
        /// </summary>
        /// <param name="result"></param>
        /// <param name="fixedIntAmbiCycles"></param>
        /// <returns></returns>
        public bool TryFixeAmbiguites(EpochDouFreDoubleDifferPositionResult result, out WeightedVector fixedIntAmbiCycles)
        {
            int minFixedAmbiCount = 1;
            //AmbiguityManager.Regist(result); //用于存储和输出模糊度。

            var floatSolMetter = result.ResultMatrix.Estimated;
            //尝试固定模糊度
            fixedIntAmbiCycles = FixePhaseAmbiguity(result);

            //模糊度固定失败，直接返回浮点数结果。 
            // 关键问题：模糊度固定是否正确？？

            //模糊度固定失败，直接返回浮点数结果。 
            var floatSolCyle = result.GetWeightedAmbiguityVectorInCycle();

            RemoveBiasedIntSolution(floatSolCyle, fixedIntAmbiCycles, Option.MaxAmbiDifferOfIntAndFloat);

            if (fixedIntAmbiCycles.Count < minFixedAmbiCount)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 将周转换为为米，并调整好顺序。
        /// </summary>
        /// <param name="fixedIntAmbiCycles"></param>
        /// <returns></returns>
        public WeightedVector ConvertCycleAmbiguityToMeter(WeightedVector fixedIntAmbiCycles)
        {
            if (fixedIntAmbiCycles == null) { return null; }

            var frequence = Frequence.GetFrequence(this.CurrentBasePrn, this.Option.ObsDataType, this.CurrentMaterial.ReceiverTime);
            fixedIntAmbiCycles = fixedIntAmbiCycles.SortByName(); //按照卫星顺序排序, 这样参数顺序和原浮点解一致。
            var fixedAmbiMeters = fixedIntAmbiCycles.Multiply(frequence.WaveLength); //恢复浮点数，以米为单位
            return new WeightedVector(fixedAmbiMeters, fixedIntAmbiCycles.InverseWeight);
        }
        /// <summary>
        /// 是否采用外部文件
        /// </summary>
        public bool IsOuterAmbiguityFileAvailable => (this.Option.IsUsingAmbiguityFile && AmbiguityStoarge != null);
        /// <summary>
        /// 固定相位模糊度
        /// </summary>
        /// <param name="ambiableResult"></param>
        /// <returns></returns>
        public virtual WeightedVector FixePhaseAmbiguity(IFixableParamResult ambiableResult)
        {
            //获取模糊度浮点解，单位周
            var rawFloatAmbiCycles = ambiableResult.GetFixableVectorInUnit();
            if (rawFloatAmbiCycles.Count == 0) { return new WeightedVector(); }

            //是否外部输入模糊度
            if (IsOuterAmbiguityFileAvailable)
            {
                List<RmsedNumeral> data = new List<RmsedNumeral>();
                List<string> names = new List<string>();
                foreach (var paramName in rawFloatAmbiCycles.ParamNames)
                {
                    var val = AmbiguityStoarge.Get(paramName, this.CurrentMaterial.ReceiverTime);
                    if (RmsedNumeral.IsValid(val))
                    {
                        data.Add(val);
                        names.Add(paramName);
                    }
                }
                if (data.Count > 0 || Option.IsRealTimeAmbiFixWhenOuterAmbiFileFailed)//可以指定，如果固定失败，是否进行实时估计
                {
                    return WeightedVector.Parse(data, names);
                }
            }

            WeightedVector fixedIntAmbiCycles = DoFixAmbiguity(rawFloatAmbiCycles);

            return fixedIntAmbiCycles;
        }
        /// <summary>
        /// 默认采用Lambda算法直接固定。
        /// 如果是无电离层组合，则需要分别对待，不能直接固定，需要子类进行实现，//2018.11.06，czs， hmx
        /// </summary>
        /// <param name="rawFloatAmbiCycles"></param>
        /// <returns></returns>
        protected virtual WeightedVector DoFixAmbiguity(WeightedVector rawFloatAmbiCycles)
        {
            //实时固定，采用Lambda算法，按照权逆阵排序，大的在后面，如果失败后，则删除之
            var orderedFloatAmbiCycles = rawFloatAmbiCycles.GetCovaOrdered();
            return Gnsser.LambdaAmbiguitySearcher.GetAmbiguity(orderedFloatAmbiCycles, Option.MaxRatioOfLambda, 1e-20);
        }


        /// <summary>
        /// 固定模糊度
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public WeightedVector FixePhaseAmbiguity(EpochDouFreDoubleDifferPositionResult result)
        {
            //获取模糊度浮点解，单位周
            var rawFloatAmbiCycles = result.GetWeightedAmbiguityVectorInCycle();
            //按照权逆阵排序，大的在后面，如果失败后，则删除之
            var orderedFloatAmbiCycles = rawFloatAmbiCycles.GetCovaOrdered();

            //模糊度直接用lambda算法进行固定，部分模糊度固定策略
            var fixedIntAmbiCycles = Gnsser.LambdaAmbiguitySearcher.GetAmbiguity(orderedFloatAmbiCycles, Option.MaxRatioOfLambda);

            return fixedIntAmbiCycles;
        }
        #endregion
    }

}