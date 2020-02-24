//2014.09.04, czs, edit, 基本理清思路
//2014.12.11, czs, edit in jinxinliangmao shuangliao, 差分定位矩阵生成器
//2016.03.10, czs, edit in hongqing, 重构，准备重构定位器
//2016.04.30, czs, edit in hongqing, 重命名为 BaseGnssMatrixBuilder
//2016.04.30, czs, refactor in hongqing,  基于泛型的GNSS矩阵生成器
//2017.09.02, czs, refactor in hongqing, 重构状态转移模型

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo;
using Geo.IO;
using Gnsser.Models;
using System.Threading.Tasks;

namespace Gnsser.Service
{
    //2018.05.15, czs, create in HMX, 简易GNSS矩阵生成器
    /// <summary>
    /// 
    /// <summary>
    /// 基于泛型的GNSS矩阵生成器
    /// </summary>
    /// <typeparam name="TProduct"></typeparam>
    /// <typeparam name="TMaterial"></typeparam>
    public abstract class SimpleBaseGnssMatrixBuilder  : BaseAdjustMatrixBuilder
    {
        Log log = new Log(typeof(SimpleBaseGnssMatrixBuilder));
        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleBaseGnssMatrixBuilder(GnssProcessOption Option)
        {
            this.Option = Option;
            this.SatelliteTypes = Option.SatelliteTypes;
            this.IsSameTimeSystemInMultiGnss = Option.IsSameTimeSystemInMultiGnss;
            this.PhaseCovaProportionToRange = Option.PhaseCovaProportionToRange;
            this.SatTypeCount = SatelliteTypes.Count;
            this.IsFixingCoord = Option.IsFixingCoord;
        }
        /// <summary>
        /// 是否固定坐标
        /// </summary>
        public bool IsFixingCoord { get; set; }
        /// <summary>
        /// 指定的系统类型的数量
        /// </summary>
        public int SatTypeCount { get; set; }
        /// <summary>
        /// 系统类型。
        /// </summary>
         public List<SatelliteType> SatelliteTypes { get; set; }
        /// <summary>
        /// 多系统是否采用相同的时间基准。
        /// </summary>
        public bool IsSameTimeSystemInMultiGnss { get; set; }
        /// <summary>
        /// 相位比伪距方差的比值，通常为0.01
        /// </summary>
        public double PhaseCovaProportionToRange { get; set; }
        /// <summary>
        /// 定位选项
        /// </summary>
        public GnssProcessOption Option { get; set; }

        /// <summary>
        /// 设置原材料
        /// </summary>
        /// <param name="obj"></param>
        public abstract SimpleBaseGnssMatrixBuilder SetMaterial(Object obj);
        /// <summary>
        /// 设置原结果
        /// </summary>
        /// <param name="obj"></param>
        public abstract SimpleBaseGnssMatrixBuilder SetPreviousProduct(Object obj);

    }





        /// 基于泛型的GNSS矩阵生成器
        /// </summary>
        /// <typeparam name="TProduct"></typeparam>
        /// <typeparam name="TMaterial"></typeparam>
        public abstract class SimpleBaseGnssMatrixBuilder<TProduct, TMaterial> : SimpleBaseGnssMatrixBuilder
        where TProduct : AdjustmentResult
    {
        protected Log log = new Log(typeof(SimpleBaseGnssMatrixBuilder<TProduct, TMaterial>));
        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleBaseGnssMatrixBuilder(GnssProcessOption Option) : base(Option)
        {
            this.SatWeightProvider = new SatElevateWeightProvider(Option);
            SetInitApriori(Option.InitApriori, Option.IsEnableInitApriori);//只初始化一次，对第一次起作用。
        }

        /// <summary>
        /// 卫星定权
        /// </summary>
        public ISatWeightProvider SatWeightProvider { get; set; }
        /// <summary>
        /// 观测信息
        /// </summary>
        public TMaterial CurrentMaterial { get; set; }

        /// <summary>
        /// 上一次计算结果
        /// </summary>
        public TProduct PreviousProduct { get; set; }

        /// <summary>
        /// 设置原材料
        /// </summary>
        /// <param name="obj"></param>
        public override SimpleBaseGnssMatrixBuilder SetMaterial(Object obj)
        {
            this.CurrentMaterial = (TMaterial)obj;
            return this;
        }
        /// <summary>
        /// 设置原结果
        /// </summary>
        /// <param name="obj"></param>
        public override SimpleBaseGnssMatrixBuilder SetPreviousProduct(Object obj)
        {
            this.PreviousProduct = (TProduct)obj;
            return this;
        }

        #region 手动外部输入先验信息
        /// <summary>
        /// 初始先验值是否可用。
        /// </summary>
        /// <returns></returns>
        public bool IsInitAprioriAvailable
        {
            get => IsEnableInitApriori && InitApriori != null;
        }
        /// <summary>
        /// 手动指定是否启用先验信息
        /// </summary>
        public bool IsEnableInitApriori { get; set; }

        /// <summary>
        /// 手动提供初始的先验信息
        /// </summary>
        public WeightedVector InitApriori { get; private set; }
        /// <summary>
        /// 设置初始
        /// </summary>
        /// <param name="InitApriori"></param>
        /// <param name="IsEnableInitApriori"></param>
        /// <returns></returns>
        public virtual SimpleBaseGnssMatrixBuilder SetInitApriori(WeightedVector InitApriori, bool IsEnableInitApriori = true)
        {
            this.InitApriori = InitApriori;
            this.IsEnableInitApriori = IsEnableInitApriori;
            return this;
        }
        #endregion

        /// <summary>
        /// 创建先验信息.
        /// </summary> 
        public override WeightedVector AprioriParam
        {
            get
            {
                return GetApriParamVector();
            }
        }

        private WeightedVector GetApriParamVector()
        {
            WeightedVector invWeight = null;
            if (PreviousProduct == null)//第一次计算
            {
                if (this.IsInitAprioriAvailable)
                {
                    WeightedVector indiAppriori = this.InitApriori;
                    indiAppriori.ParamNames = this.ParamNames;

                    //立即释放
                    //if (PreviousProduct != null)
                    //{
                    //    this.InitApriori = null;//过河拆桥，免得后面的人掉入河流，清空
                    //    IsEnableInitApriori = false;
                    //}


                    log.Info("采用了指定的先验参数信息作为估值信息！");
                    return indiAppriori;
                }
                else //默认初始
                {
                    invWeight = CreateInitAprioriParam();
                    invWeight.ParamNames = this.ParamNames;
                }
            }
            else if (!IsParamsChanged)
            {
                invWeight = PreviousProduct.ResultMatrix.Estimated;
            }
            else
            {
                invWeight = GetNewWeighedVectorInOrder(this.ParamNames, PreviousProduct.ResultMatrix.Estimated);
            }
            //为防止病态发散，为权逆阵对角线加上改正？？？

            return invWeight;
        }

        /// <summary>
        /// 第一次参数先验值。 创建初始先验参数值和协方差阵。只会执行一次。
        /// </summary> 
        protected virtual WeightedVector CreateInitAprioriParam()
        {
            return new InitAprioriParamBuilder(this.ParamNames, this.Option).Build();
        }
    }


    /// <summary>
    /// 基于泛型的GNSS矩阵生成器
    /// </summary>
    /// <typeparam name="TMaterial"></typeparam>
    /// <typeparam name="TProduct"></typeparam>
    public abstract class BaseGnssMatrixBuilder<TProduct, TMaterial> : SimpleBaseGnssMatrixBuilder<TProduct, TMaterial>
        where TMaterial : ISiteSatObsInfo
        where TProduct : BaseGnssResult
    {
        Log log = new Log(typeof(BaseGnssMatrixBuilder<TProduct, TMaterial>));

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseGnssMatrixBuilder(GnssProcessOption Option):base(Option)
        {
            this.Option = Option;
            this.ParamStateTransferModelManager = new ParamStateTransferModelManager(Option);
            this.ParamPipleManager = new NumeralWindowDataManager(10);
            this.BaseSiteName = Option.IndicatedBaseSiteName;
            this.BaseSatType = Option.SatelliteTypes[0];
        }
        /// <summary>
        /// 参数 随机模型管理器
        /// </summary>
        public ParamStateTransferModelManager ParamStateTransferModelManager { get; set; }

        public virtual int BaseClockCount { get; set; }
        #region 基本属性

        /// <summary>
        /// 上一个原料，存在于上一个计算结果中，若是第一个，则为NULL
        /// </summary>
        public virtual TMaterial PrevMaterial { get { if (PreviousProduct == default(TProduct)) return default(TMaterial); return (TMaterial)(PreviousProduct.Material); } }
        /// <summary>
        /// 参数是否改变，默认通过可用卫星进行判断。
        /// </summary>
        public override bool IsParamsChanged
        {
            get
            {
                if (PreviousProduct != null)
                {
                    if (PreviousProduct is BaseGnssResult)
                    {
                        var preResult = PreviousProduct as BaseGnssResult;
                        return !ListUtil.IsEqual(this.ParamNames, preResult.ParamNames);
                    }
                }

                return true;
            }
        }
        /// <summary>
        /// 卫星数量
        /// </summary>
        public int EnabledSatCount { get; set; }
        /// <summary>
        /// 启用的卫星
        /// </summary>
        public List<SatelliteNumber> EnabledPrns { get; set; }
        /// <summary>
        /// 直接获取 GnssParamNameBuilder
        /// </summary>
        public GnssParamNameBuilder GnssParamNameBuilder { get { return (GnssParamNameBuilder)ParamNameBuilder; } }

        /// <summary>
        /// 第二参数名称生成器
        /// </summary>
        public virtual GnssParamNameBuilder SecondParamNameBuilder { get; set; }


        /// <summary>
        /// 用于管理数据发生跳变的参数，加大他们的状态转移噪声。
        /// </summary>
        public NumeralWindowDataManager ParamPipleManager { get; set; }
         
        
    /// <summary>
    /// 基准卫星类型
    /// </summary>
    public SatelliteType BaseSatType { get; set; }
    /// <summary>
    /// 常常需要基准卫星。
    /// 基础卫星编号。都以此卫星做差分。
    /// </summary>
    public SatelliteNumber CurrentBasePrn { get; private set; }
        /// <summary>
        /// 基准测站，用于网解
        /// </summary>
    public string BaseSiteName { get;

            protected set; }
        /// <summary>
        /// 相对上一历元，基准卫星是否稳定，以确定状态转移模型和观测噪声。
        /// 当发生周跳时，也可以设置true，采用它来重新定权。
        /// </summary>
        public bool IsBaseSatUnstable { get; set; }
        /// <summary>
        /// 基准卫星变化了
        /// </summary>
        public event Action<SatelliteNumber, SatelliteNumber> BaseSatChangedEnventHandler;
        /// <summary>
        /// 基准测站变化处理事件
        /// </summary>
        public event Action<string, string> BaseSiteChangedEnventHandler;
        /// <summary>
        /// 测站改变了
        /// </summary>
        /// <param name="newSiteName"></param>
        /// <param name="oldSiteName"></param>
        protected virtual void OnBaseSiteChanged(string newSiteName, string oldSiteName)
        {
            BaseSiteChangedEnventHandler?.Invoke(newSiteName, oldSiteName);
        }
        /// <summary>
        /// 基准卫星变化了
        /// </summary>
        /// <param name="newPrn"></param>
        /// <param name="oldPrn"></param>
        protected virtual void OnBaseSatChanged(SatelliteNumber newPrn, SatelliteNumber oldPrn)
        {
            BaseSatChangedEnventHandler?.Invoke(newPrn, oldPrn);
        }
        #endregion


        #region  获取与设置相位系数
        protected double CoeefOfPhase { get; set; }
        protected bool IsCoeefOfPhaseSetted { get; set; }
        protected double CoeefOfPhaseL2 { get; set; }
        protected bool IsCoeefOfPhaseSettedL2 { get; set; }
        /// <summary>
        /// 相位模糊度系数，可以为米和周，按照设置文件决定。
        /// </summary>
        /// <returns></returns>
        protected virtual double CheckAndGetCoeefOfPhase()
        {
            if (IsCoeefOfPhaseSetted)
            {
                return CoeefOfPhase;
            }

            CoeefOfPhase = 1;
            if (!Option.IsPhaseInMetterOrCycle)
            {   //获取当前单频的信号频率,若是无电离层猪组合，则采用L1的频率
                Frequence Frequence = Gnsser.Frequence.GetFrequence(this.CurrentBasePrn, this.Option.ObsDataType, this.CurrentMaterial.ReceiverTime);
                CoeefOfPhase = Frequence.WaveLength;

                IsCoeefOfPhaseSetted = true;
            }

            return CoeefOfPhase;
        }
        /// <summary>
        ///第二频率 相位模糊度系数 ，可以为米和周，按照设置文件决定。
        /// </summary>
        /// <returns></returns>
        protected virtual double CheckAndGetCoeefOfPhaseL2()
        {
            if (IsCoeefOfPhaseSettedL2)
            {
                return CoeefOfPhaseL2;
            }

            CoeefOfPhaseL2 = 1;
            if (!Option.IsPhaseInMetterOrCycle)
            {   //获取当前单频的信号频率,若是无电离层猪组合，则采用L1的频率
                Frequence Frequence = Gnsser.Frequence.GetFrequence(this.CurrentBasePrn, ObsPhaseType.L2, this.CurrentMaterial.ReceiverTime);
                CoeefOfPhaseL2 = Frequence.WaveLength;

                IsCoeefOfPhaseSettedL2 = true;
            }

            return CoeefOfPhaseL2;
        }
        #endregion


        #region smart api
        /// <summary>
        /// 设置基础卫星
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public BaseGnssMatrixBuilder<TProduct, TMaterial> SetBasePrn(SatelliteNumber prn)
        {
            if (prn != CurrentBasePrn)
            {
                IsBaseSatUnstable = true;
                OnBaseSatChanged(prn, CurrentBasePrn);
                this.CurrentBasePrn = prn;
            }
            return this;
        }
        /// <summary>
        /// 设置基础测站
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public BaseGnssMatrixBuilder<TProduct, TMaterial> SetBaseSiteName(string siteName)
        {
            if (siteName != BaseSiteName)
            {
                OnBaseSiteChanged(siteName, BaseSiteName);
                BaseSiteName = siteName;
            }
            return this;
        }
        /// <summary>
        /// 设置基础卫星类型
        /// </summary>
        /// <param name="satType"></param>
        /// <returns></returns>
        public BaseGnssMatrixBuilder<TProduct, TMaterial> SetBaseSatType(SatelliteType satType)
        {
            this.BaseSatType = satType; 
            return this;
        }
        /// <summary>
        /// 设置历元信息
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <returns></returns>
        public BaseGnssMatrixBuilder<TProduct, TMaterial> SetMaterial(TMaterial epochInfo)
        {
            this.CurrentMaterial = epochInfo;
            this.EnabledPrns = epochInfo.EnabledPrns;
            this.EnabledSatCount = this.EnabledPrns.Count;

            return this;
        }
        /// <summary>
        /// 设置上一个定位结果
        /// </summary>
        /// <param name="previousResult"></param>
        /// <returns></returns>
        public BaseGnssMatrixBuilder<TProduct, TMaterial> SetPreviousProduct(TProduct previousResult)
        {
            this.PreviousProduct = previousResult; return this;
        }
        #endregion

        #region 方法

        /// <summary>
        /// 显示调用构建器
        /// </summary>
        public override void Build()
        {
            this.ParamNames = BuildParamNames();
          
            //判断基准星是否稳定
            if (!IsBaseSatUnstable && this.CurrentMaterial != null && this.CurrentMaterial.HasCycleSlip(this.CurrentBasePrn))
            {
                IsBaseSatUnstable = true;
            } 

            //检查并更新状态转移模型
            UpdateStateTransferModels();

            Check();

        }
        /// <summary>
        /// 简单的检查
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            if (HasApprox)
            {
                if (this.ParamCount != ApproxParam.Count)
                {
                    throw new Exception("近似参数数量不匹配！");
                }
            }
            if (HasApriori)
            {
                if (this.ParamNames.Count != AprioriParam.Count)
                {
                    throw new Exception("先验数量不匹配！");
                }
            }

            return false;
        }

        /// <summary>
        /// 创建参数名称
        /// </summary>
        public virtual List<string> BuildParamNames()
        {
            if (SecondParamNameBuilder != null)
            {
                this.SecondParamNames = BuildParamNames(SecondParamNameBuilder);
            }

            this.ParamNames = BuildParamNames(GnssParamNameBuilder); 
            return ParamNames;
        }
        /// <summary>
        /// 生存参数名称
        /// </summary>
        /// <param name="nameBuilder"></param>
        /// <returns></returns>
        public List<String> BuildParamNames(GnssParamNameBuilder nameBuilder)
        {
            nameBuilder.SetMaterial(this.CurrentMaterial)
                .SetEpoches(this.CurrentMaterial.Epoches)
                .SetBaseSiteName(BaseSiteName)
                .SetBasePrn(this.CurrentBasePrn)
                .SetPrns(this.EnabledPrns)
                .SetSatelliteTypes(this.CurrentMaterial.SatelliteTypes);
            return nameBuilder.Build();
        }

        #region 参数转移模型 的更新与设置

        /// <summary>
        /// 检查并更新参数状态转移模型
        /// </summary> 
        public virtual void UpdateStateTransferModels()
        {
            //确保以下正确
            //  var key = this.ParamStateTransferModelManager.GetOrCreate(keyPrev);
        }

        #endregion

        /// <summary>
        ///  创建状态转移矩阵和噪声,注意：大多数状态转移模型为两个对角线矩阵
        /// </summary>
        public override WeightedMatrix Transfer
        {
            get
            {
                double[] trans;
                double[] model;

                trans = new double[ParamCount];
                model = new double[ParamCount];

                //x y z clk 系统时间差
                for (int i = 0; i < ParamCount; i++)
                {
                    var key = ParamNames[i];
                    var stateTransfer = this.ParamStateTransferModelManager.GetOrCreate(key);
                    stateTransfer.Init(this.CurrentMaterial); 

                    if (stateTransfer is Gnsser.Models.DueSatPhaseAmbiguityMode)
                    {
                        ((Gnsser.Models.DueSatPhaseAmbiguityMode)stateTransfer).SetBasePrn(this.CurrentBasePrn);
                    }

                    var tran = stateTransfer.GetTrans();
                    var vari = stateTransfer.GetNoiceVariance();

                    //只负责静态转移的数
                    if (Option.IsPromoteTransWhenResultValueBreak && tran == 1 && !Gnsser.ParamNames.IsDxyz(key))
                    {
                        var windowData = ParamPipleManager.GetOrCreate(key);
                        var lastEst = this.AprioriParam.Get(key);
                        if (RmsedNumeral.IsValid(lastEst))
                        {
                            int aveCount = 10;
                            var isOk = windowData.AverageCheckAddOrClear(lastEst.Value, aveCount);
                            if (!isOk)
                            {
                                var info = this.PrevMaterial.Name + ", "
                                    + this.PrevMaterial.ReceiverTime + ", "
                                    + key + ": "
                                    + lastEst.Value.ToString("G4").PadLeft(7, ' ') + ", 超限 转移阵重置， 前" + aveCount + " 个均值 " + windowData.LastAverage.ToString("G4");
                                tran = 0;
                                vari = 1e8;
                                log.Info(info);
                            }
                        }
                        //新出现的参数，及时升噪
                        if (PreviousProduct != null && !PreviousProduct.ParamNames.Contains(key) && vari < 1)
                        {
                            var info = this.CurrentMaterial.Name + ", " + this.CurrentMaterial.ReceiverTime + ", 上一历元 "
                                   + this.PrevMaterial.ReceiverTime + " 不包括 "
                                   + key + " 为其转移模型升噪 ";
                            log.Info(info);
                            tran = 0;
                            vari = 1e10;
                        }
                    }

                    trans[i] = tran;
                    model[i] = vari;
                }

                return new WeightedMatrix(new DiagonalMatrix(trans), new DiagonalMatrix(model));
            }
        }


        /// <summary>
        ///  创建状态转移矩阵和噪声,注意：大多数状态转移模型为两个对角线矩阵
        /// </summary>
        public override WeightedMatrix SecondTransfer
        {
            get
            {
                if (!HasSecondParams) { return null; }

                double[] trans;
                double[] model;

                trans = new double[SecondParamCount];
                model = new double[SecondParamCount];

                //x y z clk 系统时间差
                for (int i = 0; i < SecondParamCount; i++)
                {
                    var key = SecondParamNames[i];
                    var stateTransfer = this.ParamStateTransferModelManager.GetOrCreate(key);
                    stateTransfer.Init(this.CurrentMaterial);

                    if (stateTransfer is Gnsser.Models.DueSatPhaseAmbiguityMode)
                    {
                        ((Gnsser.Models.DueSatPhaseAmbiguityMode)stateTransfer).SetBasePrn(this.CurrentBasePrn);
                    }

                    var tran = stateTransfer.GetTrans();
                    var vari = stateTransfer.GetNoiceVariance(); 
                    trans[i] = tran;
                    model[i] = vari;
                }

                return new WeightedMatrix(new DiagonalMatrix(trans), new DiagonalMatrix(model));
            }
        }


        #endregion


        /// <summary>
        /// P2 的电离层系数
        /// </summary>
        /// <returns></returns>
        protected double GetIonoCoeOfP2(SatelliteType SatelliteType)
        {
            double IonoCoeOfP2 = 0;
            switch (SatelliteType)
            {
                case SatelliteType.G:
                    IonoCoeOfP2 = GnssConst.CoeOfGPSIono;
                    break;
                case SatelliteType.E:
                    IonoCoeOfP2 = GnssConst.CoeOfGalileoIono;
                    break;
                case SatelliteType.C:
                    IonoCoeOfP2 = GnssConst.CoeOfBDIono;
                    break;
            }

            return IonoCoeOfP2;
        }
    }

}