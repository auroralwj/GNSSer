//2014.08.20，czs, 创建 测站->卫星观测数据核心存储模型，Gnsser核心模型！
//2014.10.09, czs, edit in 海鲁吐， 增加了两个移除函数 RemoveCorrection 和  RemoveCombination
//2014.10.25, czs, eidt in numu shuanglioa, 增加改正列表，改正数实时生成，便于查看。
//2014.10.26, czs, eidt in numu, 组合值存在于观测值的组合，应该实时创建的。
//2015.05.28, czs, edit in namu, 增加 EstmatedVector
//2016.10.15, double & czs, edit in hongqing, 增加相对论时间改正数，后续可以设计测站卫星改正数集合
//2017.09.19, czs, edit in hongqing, 获取近似距离从EpochInfo转移到此,建立平差数据获取方法
//2018.06.17, czs, edit in HMX, 增加伪距权因子变量，默认为1米

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Service;
using Gnsser.Correction;
using Gnsser.Data.Rinex;
using Geo;
using Geo.Correction;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Times;

namespace Gnsser.Domain
{

    /// <summary>
    /// 包含一颗卫星和测站这个向量的数据，包括：观测数据、星历数据、卫星元数据、测站信息等，是本程序的核心存储模型。
    /// 本类由观测数据进行初始化，
    /// 再由模型进行误差改正，
    /// 最后参与计算。
    /// 大部分的前期计算工作,如：粗差探测、
    /// </summary>
    public class EpochSatellite : BaseDictionary<FrequenceType, FreqenceObservation>,
        ICommonObservationCorrection, IToTabRow, IEnabled, IValueClone<EpochSatellite>
    {
        #region 构造函数和初始化方法
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EpochSatellite() { }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="epochInfo">历元观测信息</param>
        /// <param name="prn">卫星编号</param>
        public EpochSatellite(
            EpochInformation epochInfo,
            SatelliteNumber prn
            )
            : base()
        {
            this.EpochInfo = epochInfo;
            this.SiteInfo = epochInfo ==null? null: epochInfo.SiteInfo;
            this.Enabled = true;
            this.Prn = prn;
            this.CommonCorrection = new NumerialCorrectionDic();
            this.PhaseOnlyCorrection = new NumerialCorrectionDic();
            this.RangeOnlyCorrection = new NumerialCorrectionDic();
            this.StdDevOfRange = 1;
        }

        #endregion

        #region 变量，属性  
        /// <summary>
        /// 接收时刻
        /// </summary>
        public Time ReceiverTime { get { return EpochInfo.ReceiverTime; } }
        /// <summary>
        /// 标记是否启用。如孤立观测值，或者数据不完整等。可以将其屏蔽，不参与计算。
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 接收机观测时间。具有改正数。
        /// </summary>
        public CorrectableTime Time { get { return EpochInfo.Time; } }

        /// <summary>
        /// 组合值创建器。组合值存在于观测值的组合，应该实时创建的。
        /// </summary>
        public PhaseCombinationBuilder Combinations { get { return new PhaseCombinationBuilder(this); } }


        #region 测站、卫星基本相关
        /// <summary>
        /// 观测伪距标准差，默认为1米。
        /// </summary>
        public double StdDevOfRange { get; set; }

        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; private set; }

        /// <summary>
        /// 卫星弧段编号或时段编号，通常标定卫星所在的弧段（如周跳的分段）。
        /// Satellite arc number
        /// </summary>
        public int ArcMarker { get; set; }

        #region 便捷属性
        /// <summary>
        /// 测站信息。//2015.05.14，czs, 迟早分离出去！
        /// </summary>
        public ISiteInfo SiteInfo { get; set; }
        /// <summary>
        /// 测站历元信息
        /// </summary>
        public EpochInformation EpochInfo { get; set; }
        #endregion

        /// <summary>
        /// 信号接收时刻，这是系统时间，通常是由接收机钟面时改正而来
        /// </summary>
        public Time RecevingTime { get { return Time.CorrectedValue; } }
        /// <summary>
        /// 信号发射时刻,来自星历计算后的时间
        /// </summary>
        public Time EmissionTime { get { return this.Ephemeris.Time; } }
        /// <summary>
        /// 根据伪距计算卫星信号传输时间，单位秒。有卫星钟差和相对论时间改正会更精确。
        /// </summary>
        /// <returns></returns>
        public double GetTransitTime()
        {
            var result = 0.0;
            //无伪距，或为0 
            if (AvailablePseudoRange == null || AvailablePseudoRange.Value == 0) { { var msg = Time + "," + Prn + ",没有有效伪距值，无法计算传输时间！"; log.Error(msg); return result; } }
            else if (Ephemeris == null) { log.Debug("没有星历钟差改正，计算精度有限。"); result = AvailablePseudoRange.Value / GnssConst.LIGHT_SPEED; }
            else { result = AvailablePseudoRange.Value / GnssConst.LIGHT_SPEED + this.Ephemeris.ClockBias + this.Ephemeris.RelativeCorrection; }
            if(Math.Abs(result) > 1)
            {
                log.Error("我的天，卫星距离太远了吧，到月球了？ " + result);

            }
            return result;
        }
        /// <summary>
        /// 卫星发射时刻的星历。
        /// </summary>
        public IEphemeris Ephemeris { get; set; }
        /// <summary>
        /// 是否具有星历信息。
        /// </summary>
        public bool HasEphemeris { get { return Ephemeris != null && Ephemeris.XYZ != null; } }
        /// <summary>
        /// 获取相交点、穿刺点，如电离层
        /// </summary>
        /// <param name="geoHeight">距离地球平均表面的距离 m, 默认 450 000 m</param>
        /// <returns></returns>
        public XYZ GetIntersectionXyz(double geoHeight = 450000)
        {
            var siteXyz = this.SiteInfo.EstimatedXyz;
            var satXyz = this.Ephemeris.XYZ;

            return XyzUtil.GetIntersectionXyz(siteXyz, satXyz, geoHeight);
        }

        #endregion

        #region 传输过程相关，伪距、载波等基本观测量
        /// <summary>
        /// 求平均后的卫星相对接收机的速度， 单位：米/秒。相向为负，反向为正。注意：与多普勒频率大小相反。
        /// </summary>
        public double AverageDopplorSpeed
        {
            get
            {
                int count = 0;
                double val = 0;
                foreach (var item in this.Values)
                {
                    //保证不为空，且被赋过值，才能参与计算
                    if (item.DopplerShift != null && item.DopplerSpeed != 0)
                    {
                        val += item.DopplerSpeed;
                        count++;
                    }
                }
                return val / count;
            }
        }
        /// <summary>
        /// 频率数量
        /// </summary>
        public int FrequencyCount { get { return this.Count; } }
        /// <summary>
        /// 卫星大地高度角，度。
        /// </summary>
        public double GeoElevation { get => Polar.Elevation; }
        /// <summary>
        /// 卫星球坐标高度角，度。
        /// </summary>
        public double SphereElevation { get => SpherePolar.Elevation; }
        /// <summary>
        /// 站星球面极坐标（基于球面）。单位：度。
        /// </summary>
        public Polar SpherePolar { get { return CoordTransformer.XyzToSpherePolar(Ephemeris.XYZ, this.SiteInfo.EstimatedXyz, AngleUnit.Degree); } }
  

        /// <summary>
        /// 站星大地极坐标（基于椭球面）。单位：度。
        /// </summary>
        public Polar Polar { get { return CoordTransformer.XyzToGeoPolar(Ephemeris.XYZ, this.SiteInfo.EstimatedXyz, AngleUnit.Degree); } }
        /// <summary>
        /// 是否具有可用的伪距
        /// </summary>
        public bool HasAvailablePseudoRange
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.PseudoRange.Value != 0) { return true; }
                }
                return false;
            }
        }
        /// <summary>
        /// 返回一个可用的伪距，如果没有，则返回 0 。
        /// </summary>
        public Observation AvailablePseudoRange
        {
            get
            {
                foreach (var item in this)
                {
                    if (item != null && item.Contains(ObservationType.C) && item.PseudoRange.Value != 0) { return item.PseudoRange; }
                }
                return null;
            }
        }


        /// <summary>
        /// 载波 A 的观测量
        /// </summary>
        public FreqenceObservation FirstAvailable { get { foreach (var item in this) { return item; } return null; } }
        /// <summary>
        /// 载波 A 的观测量
        /// </summary>
        public FreqenceObservation FrequenceA { get { return Get(FrequenceType.A); } }
        /// <summary>
        /// 载波 B 的观测量
        /// </summary>
        public FreqenceObservation FrequenceB { get { return Get(FrequenceType.B); } }
        /// <summary>
        /// 载波 C 的观测量。如 GPS 的 L5，北斗的第三频率。
        /// </summary>
        public FreqenceObservation FrequenceC { get { return Get(FrequenceType.C); } }
        /// <summary>
        /// 卫星频率类型。
        /// </summary>
        public List<FrequenceType> FrequenceTypes { get { return (Keys); } }
        
        private List<RinexSatFrequency> _rinexSatFrequences;
        /// <summary>
        /// RINEX格式的卫星频率列表。一个频率可能对应多个RINEX编号。如北斗C1->C1,C2
        /// </summary>
        public List<RinexSatFrequency> RinexSatFrequences
        {
            get
            {
                //若有，则直接返回
                if(_rinexSatFrequences != null && !IsChanged) { return _rinexSatFrequences; }

                IsChanged = false;

                //否则初始化
                _rinexSatFrequences = new List<RinexSatFrequency>();
                var dic = ObsCodeConvert.GetRinexFreqIndexDic(this.Prn.SatelliteType);
                List<FrequenceType> frs = FrequenceTypes;
                foreach (var fr in frs)
                {
                    if (dic.ContainsKey(fr))//不包含则不支持
                    {
                        var rinexList = dic[fr];
                        foreach (var rinexNum in rinexList)
                        {
                            RinexSatFrequency sf = new RinexSatFrequency(Prn, rinexNum);
                            if (!_rinexSatFrequences.Contains(sf))
                            {
                                _rinexSatFrequences.Add(sf);
                            }
                        }

                    }
                }
                return _rinexSatFrequences;
            }
        }
        /// <summary>
        /// 通过载波作差获取的电离层距离（含可以视为常数的硬件延迟和模糊度的倍数（GPS为1.54倍））
        /// </summary>
        public double IonoLenOfL1ByDifferL1L2
        {
            get => (this.FrequenceA.PhaseRange.Value - this.FrequenceB.PhaseRange.Value) * Frequence.GetIonoAndDcbOfL1CoeffL1L2(this.Prn.SatelliteType);
        }
        /// <summary>
        /// 通过载波作差获取的电离层距离（含可以视为常数的硬件延迟和模糊度的倍数（GPS为2.54倍））
        /// </summary>
        public double IonoLenOfL2ByDifferL1L2
        {
            get => (this.FrequenceA.PhaseRange.Value - this.FrequenceB.PhaseRange.Value) * Frequence.GetIonoAndDcbOfL2CoeffL1L2(this.Prn.SatelliteType);
        }
        /// <summary>
        /// 通过载波作差获取的电离层距离（含可以视为常数的硬件延迟和模糊度的倍数（GPS为1.54或2.54倍））
        /// </summary>
        /// <param name="FrequenceType"></param>
        /// <returns></returns>
        public double GetIonoLenByDifferPhase(FrequenceType FrequenceType)
        {
            if (FrequenceType == FrequenceType.A)
            {
                return IonoLenOfL1ByDifferL1L2;
            }

            if (FrequenceType == FrequenceType.B)
            {
                return IonoLenOfL2ByDifferL1L2;
            }
            log.Warn("不支持 " + FrequenceType + " 的电离层计算！");
            return 0;
        }

        /// <summary>
        /// 更新改正数， 如果有C1C2 的DCB改正，则会改正观测码 为 P1 P2
        /// </summary>
        public void TryUpdateObsWithCorrections()
        {
            TryUpdateC1C2ToP1P2IfDcbCorrected();

            foreach (var freq in this)
            {
                freq.UpdateValueWithCorrections();
            } 
        }

        /// <summary>
        /// 更新C1C2,并将改正数直接改正到原始数据上，同时移除P1C1等标记。
        /// </summary>
        public void TryUpdateC1C2ToP1P2IfDcbCorrected()
        {
            var correctoinNames = this.ObsCorrectionNames;
            if (correctoinNames.Contains(CorrectionNames.DcbP1C1) && !HasP1 && this.Contains(FrequenceType.A))
            {
                var range = this[FrequenceType.A].PseudoRange;
                range.Value += range.Corrections[CorrectionNames.DcbP1C1.ToString()];
                range.Corrections.Remove(CorrectionNames.DcbP1C1.ToString());

                var code = range.ObservationCode;
                ObservationCode.ChagngeCaToP(ref code);
            }
            if (correctoinNames.Contains(CorrectionNames.DcbP2C2) && !HasP2 && this.Contains(FrequenceType.B))
            {
                var range = this[FrequenceType.B].PseudoRange; 
                range.Value += range.Corrections[CorrectionNames.DcbP2C2.ToString()];
                range.Corrections.Remove(CorrectionNames.DcbP2C2.ToString());

                var code = range.ObservationCode;
                ObservationCode.ChagngeCaToP(ref code);
            } 
        }

        /// <summary>
        /// 是否具有P1码
        /// </summary>
        public bool HasP1 { get { return HasCodeP(FrequenceType.A); } }
        /// <summary>
        /// 是否具有P1码
        /// </summary>
        public bool HasP2 { get { return HasCodeP(FrequenceType.B); } }
        /// <summary>
        /// 是否具有精码
        /// </summary>
        /// <param name="frequenceType"></param>
        /// <returns></returns>
        public bool HasCodeP(FrequenceType frequenceType)
        {
            if (this.Contains(frequenceType))
            {
                var obs = this[frequenceType];
                if (obs.Contains(ObservationType.C))
                {
                    var ranges = obs[ObservationType.C];
                    foreach (var range in ranges)
                    {
                        if (range.ObservationCode.Attribute == ObservationCode.DefaultAttributeOfP)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 获取指定频率伪距数量。
        /// </summary>
        /// <param name="frequenceType"></param>
        /// <returns></returns>
        public int GetPsuedoRangeCount(FrequenceType frequenceType = FrequenceType.A)
        {
            if (this.Contains(FrequenceType.A))
            {
                return  this[FrequenceType.A].PsuedoRangeCount;
            }
            return 0;
        }


        #endregion

        #region  方法与检索器

        /// <summary>
        /// 获取某一频率的近似模糊度距离。
        /// </summary>
        /// <param name="frequencyType"></param>
        /// <param name="isTryIonoFree"></param>
        /// <returns></returns>
        public double GetApproxAmbiguityDistance(FrequenceType frequencyType, bool isTryIonoFree = true)
        {
            double phaseRange = Get(frequencyType).PhaseRange.CorrectedValue;
            double ambiguityRange = 0;
            double approxPhaseRange = 0;
            if (this.Count == 2 && isTryIonoFree)//双频则用无电离层组合
            {
                approxPhaseRange =  this.GetApproxRange(SatObsDataType.IonoFreeRange);
            }
            else
            {
                approxPhaseRange = this.GetApproxPseudoRange(frequencyType).CorrectedValue;
            } 
             ambiguityRange = approxPhaseRange - phaseRange - this.Time.Correction * GnssConst.LIGHT_SPEED;

            return ambiguityRange;
        }

        /// <summary>
        /// 是否包含指定的卫星类型和频率
        /// </summary>
        /// <param name="satFreq">卫星类型和频率</param>
        /// <returns></returns>
        public bool Contains(RinexSatFrequency satFreq) {return this.RinexSatFrequences.Contains(satFreq); }// return Prn.SatelliteType == satFreq.SatelliteType && this.Contains(satFreq.FrequenceType); }
        /// <summary>
        /// 获取数值。
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public DetailedCorrectableNumeral this[SatObsDataType dataType] { get { return this.GetDataValue(dataType); } }

        #region 改正数         
        /// <summary>
        /// 通用模型距离改正，同时适用于伪距和载波，如卫星钟差改正、对流层改正等。
        /// </summary>
        public NumerialCorrectionDic CommonCorrection { get; set; }
        /// <summary>
        /// 相位距离改正，如电离层改正-
        /// </summary>
        public NumerialCorrectionDic PhaseOnlyCorrection { get; set; }
        /// <summary>
        /// 伪距特有距离改正，如电离层改正+
        /// </summary>
        public NumerialCorrectionDic RangeOnlyCorrection { get; set; }

        /// <summary>
        /// 获取站星层次的通用载波距离改正
        /// </summary>
        /// <returns></returns>
        public NumerialCorrectionDic GetCommonPhaseCorrection()
        {
            NumerialCorrectionDic dic = new NumerialCorrectionDic();
            dic.SetCorrection(CommonCorrection.Corrections);
            dic.SetCorrection(PhaseOnlyCorrection.Corrections);
            return dic;
        }
        /// <summary>
        /// 获取站星层次的通用伪距距离改正
        /// </summary>
        /// <returns></returns>
        public NumerialCorrectionDic GetCommonRangeCorrection()
        {
            NumerialCorrectionDic dic = new NumerialCorrectionDic();
            dic.SetCorrection(CommonCorrection.Corrections);
            dic.SetCorrection(RangeOnlyCorrection.Corrections);
            return dic;
        }

        /// <summary>
        /// 添加相位改正。对所有的相位起作用。这里转换成相位观测值距离的改正数（Frequence.PhaseRange.Correction）。
        /// </summary>
        /// <param name="corrector">相位改正数，是相位</param>
        /// <param name="phaseCorrection">相位改正数，是相位</param>
        public void AddPhaseCyleCorrection(string corrector, double phaseCorrection)
        {
            foreach (var item in this.Values)
            { 
                item.AddPhaseCyleCorrection(corrector, phaseCorrection);
            }
        }
        /// <summary>
        /// 添加周为单位的相位改正。对所有的观测值起作用。这里转换成观测值距离的改正数
        /// </summary>
        /// <param name="corrector">相位改正数，是相位</param>
        /// <param name="cycle">相位改正数，是相位</param>
        public void AddCommonCyleCorrection(string corrector, double cycle)
        {
            foreach (var item in this.Values)
            {
                item.AddCommonCyleCorrection(corrector, cycle);
            }
        }

        /// <summary>
        /// 添加本对象通用距离改正数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void AddCommonCorrection(string key, double val)
        {
            this.CommonCorrection.AddCorrection(key, val);
        }
        /// <summary>
        /// 添加本对象通用距离改正数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetCommonCorrection(string key, double val)
        {
            this.CommonCorrection.SetCorrection(key, val);
        }
        /// <summary>
        /// 添加伪距距离改正
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void AddRangeCorrection(string key, double val)
        {
            this.RangeOnlyCorrection.AddCorrection(key, val);
        }
        /// <summary>
        /// 添加相位距离改正
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void AddPhaseCorrection(string key, double val)
        {
            this.PhaseOnlyCorrection.AddCorrection(key, val);
        }
        /// <summary>
        /// 移除所有的改正数，包括公共的和频率私有的。
        /// 通常是为了重新计算。
        /// </summary>
        public void ClearCorrections()
        {
            this.CommonCorrection.ClearCorrections();
            foreach (var item in this) item.ClearCorrections();
        }
        #endregion

        /// <summary>
        /// 估计测站钟差对距离的影响.此处认为距离受卫星和接收机钟差影响
        /// </summary>
        /// <param name="revPos">接收机位置</param>
        /// <returns></returns>
        public double GetApproxReceiverTimeErrorDistance(XYZ revPos)
        {
            double distance = (Ephemeris.XYZ - revPos).Length - GnssConst.LIGHT_SPEED * Ephemeris.ClockBias;
            return FrequenceA.PseudoRange.Value - distance;
        }

        #endregion


        #region 常用组合与改正值
        /// <summary>
        /// 是否可以做电离层组合
        /// </summary>
        public bool IsIonoFreeAvailable
        {
            get => this.Count >= 2 && this.Contains(FrequenceType.A) && this.Contains(FrequenceType.B)
                    && this.FrequenceA.PhaseRange.Value != 0 && this.FrequenceA.PhaseRange.Value != 0
                    && this.FrequenceB.PhaseRange.Value != 0 && this.FrequenceB.PhaseRange.Value != 0;
        }

        /// <summary>
        /// 观测值改正名称
        /// </summary>
        public List<CorrectionNames> ObsCorrectionNames
        {
            get
            {
                List<CorrectionNames> names = new List<CorrectionNames>();
                foreach (var item in this)
                {
                    names.AddRange(item.ObsCorrectionNames);
                }
                return names;
            }
        }

        /// <summary>
        /// 获取伪距数据量
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public DetailedCorrectableNumeral GetPseudoRange(RangeType dataType)
        {
            switch (dataType)
            {
                case RangeType.RangeA: return FrequenceA.PseudoRange;
                case RangeType.RangeB: return FrequenceB.PseudoRange;
                case RangeType.RangeC: return FrequenceC.PseudoRange;
                case RangeType.IonoFreeRangeOfAB: return Combinations.GetIonoFreeRange(FreqCombinationType.AB,true);
                case RangeType.IonoFreeRangeOfBC: return Combinations.GetIonoFreeRange(FreqCombinationType.BC, true);
                case RangeType.IonoFreeRangeOfAC: return Combinations.GetIonoFreeRange(FreqCombinationType.AC, true);
                default: return FrequenceA.PseudoRange;
            }
        }
        /// <summary>
        /// 获取伪距数据量
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public DetailedCorrectableNumeral GetPhaseRange(RangeType dataType)
        {
            switch (dataType)
            {
                case RangeType.RangeA: return FrequenceA.PhaseRange;
                case RangeType.RangeB: return FrequenceB.PhaseRange;
                case RangeType.RangeC: return FrequenceC.PhaseRange;
                case RangeType.IonoFreeRangeOfAB: return Combinations.GetIonoFreeRange(FreqCombinationType.AB, false);
                case RangeType.IonoFreeRangeOfBC: return Combinations.GetIonoFreeRange(FreqCombinationType.BC, false);
                case RangeType.IonoFreeRangeOfAC: return Combinations.GetIonoFreeRange(FreqCombinationType.AC, false);
                default: return FrequenceA.PseudoRange;
            }
        }
        /// <summary>
        /// 获取指定了数值。
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <returns></returns>
        public DetailedCorrectableNumeral GetDataValue(SatObsDataType dataType)
        {
            switch (dataType)
            {
                case SatObsDataType.IonoFreeRange: return Combinations.IonoFreeRange;
                case SatObsDataType.IonoFreePhaseRange: return Combinations.IonoFreePhaseRange;
                case SatObsDataType.AlignedIonoFreePhaseRange: return new DetailedCorrectableNumeral(AlignedIonoFreePhaseRange);
                //case EpochSatDataType.AlignedIonoFreePhaseRange: return  CombinationBuilder.IonoFreePhaseRange;
                //case SatObsDataType.AlignedIonoFreePhaseRangeTriFrequency: return new DetailedCorrectableNumeral(AlignedIonoFreePhaseRangeThreeFrequency);
                case SatObsDataType.IonoFreeRangeOfTriFreq: return Combinations.IonoFreeRangeThreeFrequency;
                case SatObsDataType.IonoFreePhaseRangeOfTriFreq: return Combinations.IonoFreePhaseRangeThreeFrequency;
                case SatObsDataType.AlignedIonoFreePhaseRangeOfTriFreq: return new DetailedCorrectableNumeral(AlignedIonoFreePhaseRangeOfTriFreq);
                case SatObsDataType.PseudoRangeA: return this.FrequenceA.PseudoRange;
                case SatObsDataType.PhaseA: return new DetailedCorrectableNumeral(this.FrequenceA.PhaseRange.RawPhaseValue);
                case SatObsDataType.PhaseRangeA: return this.FrequenceA.PhaseRange;
                case SatObsDataType.PseudoRangeB: return this.FrequenceB.PseudoRange;
                case SatObsDataType.PhaseB: return new DetailedCorrectableNumeral(this.FrequenceB.PhaseRange.RawPhaseValue);
                case SatObsDataType.PhaseRangeB: return this.FrequenceB.PhaseRange;
                case SatObsDataType.PseudoRangeC: return this.FrequenceC.PseudoRange;
                case SatObsDataType.PhaseC: return new DetailedCorrectableNumeral(this.FrequenceC.PhaseRange.RawPhaseValue);
                case SatObsDataType.PhaseRangeC: return this.FrequenceC.PhaseRange;
                case SatObsDataType.MwCombination: return Combinations.MwRangeCombination;
                case SatObsDataType.LiCombination: return Combinations.LiPhaseComb;
                default: break;
            }
            return DetailedCorrectableNumeral.Zero;
        }
        /// <summary>
        /// 获取残差。观测值减去近似值。
        /// </summary>
        /// <param name="obsType">观测值类型</param>
        /// <param name="approxType">近似值类型</param>
        /// <returns></returns>
        public double GetResidual(SatObsDataType obsType, SatApproxDataType approxType) { return GetDataValue(obsType).CorrectedValue - GetDataValue(approxType).CorrectedValue; }
        /// <summary>
        /// 获取指定了数值。
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <returns></returns>
        public DetailedCorrectableNumeral GetDataValue(SatApproxDataType dataType)
        {
            switch (dataType)
            {
                case SatApproxDataType.IonoFreeApproxPseudoRange: return this.IonoFreeApproxPseudoRange;
                case SatApproxDataType.IonoFreeApproxPhaseRange: return IonoFreeApproxPhaseRange;
                case SatApproxDataType.ApproxPseudoRangeA: return this.ApproxPseudoRangeA;
                case SatApproxDataType.ApproxPhaseRangeA: return ApproxPhaseRangeA;
                case SatApproxDataType.ApproxPseudoRangeB: return this.ApproxPseudoRangeB;
                case SatApproxDataType.ApproxPhaseRangeB: return ApproxPhaseRangeB;
                case SatApproxDataType.ApproxPseudoRangeC: return this.ApproxPseudoRangeC;
                case SatApproxDataType.ApproxPhaseRangeC: return ApproxPhaseRangeC;
                case SatApproxDataType.ApproxPseudoRangeOfTriFreq: return this.ApproxPseudoRangeOfTriFreq;
                case SatApproxDataType.ApproxPhaseRangeOfTriFreq: return ApproxPhaseRangeOfTriFreq;
                default: break;
            }
            return DetailedCorrectableNumeral.Zero;
        }
        /// <summary>
        /// 钟跳改正
        /// </summary>
        /// <param name="clockJumpCorretion">单位：秒</param>
        /// <param name="clockJumpState"></param>
        public void CorrectClockJump(double clockJumpCorretion, ClockJumpState clockJumpState)
        {

            if (clockJumpCorretion == 0) { return; }
            var range = clockJumpCorretion * GnssConst.LIGHT_SPEED; //跳了的，应该减去，此处统一用加，所以乘以 -1.
            var IsRepairRange = clockJumpState == ClockJumpState.ClockJumped || clockJumpState == ClockJumpState.ClockJumpedRangeOnly;
            var IsRepairPhase = clockJumpState == ClockJumpState.ClockJumped || clockJumpState == ClockJumpState.ClockJumpedPhaseOnly;

            foreach (var freqObs in this)
            {
                foreach (var obs in freqObs)
                {
                    foreach (var o in obs)
                    {
                        if (o.ObservationCode.ObservationType == ObservationType.C && IsRepairRange)
                        {
                            o.Value += range;
                        }

                        if (o.ObservationCode.ObservationType == ObservationType.L && IsRepairPhase)
                        {
                            var phase = o as PhaseRangeObservation;
                            phase.AddPhaseRange(range);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 数值克隆
        /// </summary>
        /// <returns></returns>
        public EpochSatellite ValueClone()
        {
            EpochSatellite info = new EpochSatellite();

            foreach (var item in this.Data)
            {
                info[item.Key] = item.Value.DataClone();
            }

            return info;
        }

        /// <summary>
        /// 获取残差。观测值减去近似值。
        /// </summary>
        /// <param name="obsType">观测值类型</param>
        /// <param name="approxType">近似值类型</param>
        /// <returns></returns>
        //public double GetResidual(SatObsDataType obsType, SatApproxOfTriFreqDataType approxType) { return GetDataValue(obsType).CorrectedValue - GetDataValue(approxType).CorrectedValue; }
        /// <summary>
        /// 获取指定了数值。
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <returns></returns>
        //public DetailedCorrectableNumeral GetDataValue(SatApproxDataType dataType)
        //{
        //    switch (dataType)
        //    {
        //        case SatApproxDataType.ApproxPseudoRangeOfTriFreq: return this.ApproxPseudoRangeOfTriFreq;
        //        case SatApproxDataType.ApproxPhaseRangeOfTriFreq: return ApproxPhaseRangeOfTriFreq;
        //        default: break;
        //    }
        //    return DetailedCorrectableNumeral.Zero;
        //}
        /// <summary>
        /// 测站到卫星估值向量,由测站概略值坐标计算而出。
        /// </summary>
        public XYZ ApproxVector { get { return Ephemeris.XYZ - SiteInfo.ApproxXyz; } }
        /// <summary>
        /// 测站到卫星估值向估计量,由测站估值坐标计算而出。
        /// </summary>
        public XYZ EstmatedVector { get { return Ephemeris.XYZ - SiteInfo.EstimatedXyz; } }
        
        #region  一些便利的近似组合观测值
        /// <summary>
        /// 通过卫星和测站估值坐标计算出的距离结果,以及所有改正数的集合。
        /// 各个频率的改正结果，也应该加在这上面。
        /// </summary>
        public DetailedCorrectableNumeral IonoFreeApproxPseudoRange
        {
            get
            {
                DetailedCorrectableNumeral data = new DetailedCorrectableNumeral(EstmatedVector.Norm);
                data.SetCorrection(this.GetCommonRangeCorrection().Corrections);
                double rangeACor = this.FrequenceA.GetTotalRangeCorrection().TotalCorrection;
                double rangeBCor = this.FrequenceB.GetTotalRangeCorrection().TotalCorrection;
             
                //   无电离层组合
                double comCor = PhaseCombinationBuilder.GetIonoFreeRangeValue(rangeACor, rangeBCor, this.Prn, this.ReceiverTime);

                data.SetCorrection("频率AB无电离层组合改正", comCor);
                return data;
            }
        }

        /// <summary>
        /// 通过卫星和测站估值坐标计算出的距离结果,以及所有改正数的集合。
        /// 各个频率的改正结果，也应该加在这上面。
        /// </summary>
        public DetailedCorrectableNumeral IonoFreeApproxPhaseRange
        {
            get
            {
                DetailedCorrectableNumeral data = new DetailedCorrectableNumeral(EstmatedVector.Norm);
                data.SetCorrection(this.GetCommonPhaseCorrection().Corrections);
                double rangeACor = this.FrequenceA.GetTotalPhaseCorrection().TotalCorrection;
                double rangeBCor = this.FrequenceB.GetTotalPhaseCorrection().TotalCorrection;

                double comCor = PhaseCombinationBuilder.GetIonoFreeRangeValue(rangeACor, rangeBCor, this.Prn, this.ReceiverTime);
                data.SetCorrection("频率AB无电离层组合改正", comCor);
                return data;
            }
        }

        /// <summary>
        /// 遍历，返回一个可用的近似伪距
        /// </summary>
        public DetailedCorrectableNumeral AvailableApproxPseudoRange
        {
            get
            {
                DetailedCorrectableNumeral data = new DetailedCorrectableNumeral(EstmatedVector.Norm);
                data.SetCorrection(this.GetCommonRangeCorrection().Corrections);
                foreach (var item in this)
                {
                    if (item != null && item.Contains(ObservationType.C))
                    {
                        data.SetCorrection(item.GetTotalRangeCorrection().Corrections);
                        break;
                    }
                }
                return data;
            }
        }

        /// <summary>
        /// 频率A的近似伪距值
        /// </summary>
        public DetailedCorrectableNumeral ApproxPseudoRangeA { get => GetApproxPseudoRange(FrequenceType.A); }
        /// <summary>
        /// 频率B的近似伪距值
        /// </summary>
        public DetailedCorrectableNumeral ApproxPseudoRangeB { get => GetApproxPseudoRange(FrequenceType.B); }
        /// <summary>
        /// 频率C 的近似伪距值
        /// </summary>
        public DetailedCorrectableNumeral ApproxPseudoRangeC  { get =>  GetApproxPseudoRange(FrequenceType.C);  }
        
        /// <summary>
        /// 基于频率A的伪距，增加了部分关于频率A的改正
        /// </summary>
        public DetailedCorrectableNumeral ApproxPhaseRangeA { get => GetApproxPhaseRange(FrequenceType.A); }
        /// <summary>
        /// 基于频率B的伪距，增加了部分关于频率B的改正
        /// </summary>
        public DetailedCorrectableNumeral ApproxPhaseRangeB { get => GetApproxPhaseRange(FrequenceType.B); }
        /// <summary>
        /// 基于频率C的伪距，增加了部分关于频率C的改正
        /// </summary>
        public DetailedCorrectableNumeral ApproxPhaseRangeC { get => GetApproxPhaseRange(FrequenceType.C); }
        /// <summary>
        /// 获取某一频率的近似伪距，包括改正数。
        /// 注意：这里通常不包括接收机钟差改正。
        /// </summary>
        /// <param name="frequenceType"></param>
        /// <returns></returns>
        public DetailedCorrectableNumeral GetApproxPseudoRange(FrequenceType frequenceType)
        {
            DetailedCorrectableNumeral data = new DetailedCorrectableNumeral(EstmatedVector.Norm);
            data.SetCorrection(this.GetCommonRangeCorrection().Corrections);
            data.SetCorrection(this[frequenceType].GetTotalRangeCorrection().Corrections);
            return data;

        }
        /// <summary>
        /// 获取某一频率的伪距，及其所有改正数。
        /// </summary>
        /// <param name="frequenceType"></param>
        /// <returns></returns>
        public DetailedCorrectableNumeral GetApproxPhaseRange(FrequenceType frequenceType)
        {
            DetailedCorrectableNumeral data = new DetailedCorrectableNumeral(EstmatedVector.Norm);
            data.SetCorrection(this.GetCommonPhaseCorrection().Corrections);
            data.SetCorrection(this[frequenceType].GetTotalPhaseCorrection().Corrections);
            return data;

        }
        /// <summary>
        /// 通过卫星和测站估值坐标计算出的距离结果,以及所有改正数的集合。
        /// 各个频率的改正结果，也应该加在这上面。
        /// </summary>
        public DetailedCorrectableNumeral ApproxPseudoRangeOfTriFreq
        {
            get
            {
                DetailedCorrectableNumeral data = new DetailedCorrectableNumeral(EstmatedVector.Norm);
                data.SetCorrection(this.GetCommonRangeCorrection().Corrections);
                double rangeACor = this.FrequenceA.GetTotalRangeCorrection().TotalCorrection;
                double rangeBCor = this.FrequenceB.GetTotalRangeCorrection().TotalCorrection;
                double rangeCCor = this.FrequenceC.GetTotalRangeCorrection().TotalCorrection;
                //   无电离层组合
                double comCor = PhaseCombinationBuilder.GetIonoFreeComValue(rangeACor, rangeBCor, rangeCCor, this);

                data.SetCorrection("频率ABC无电离层组合改正", comCor);
                return data;
            }
        }

        /// <summary>
        /// 通过卫星和测站估值坐标计算出的距离结果,以及所有改正数的集合。
        /// 各个频率的改正结果，也应该加在这上面。
        /// </summary>
        public DetailedCorrectableNumeral ApproxPhaseRangeOfTriFreq
        {
            get
            {
                DetailedCorrectableNumeral data = new DetailedCorrectableNumeral(EstmatedVector.Norm);
                data.SetCorrection(this.GetCommonPhaseCorrection().Corrections);
                double rangeACor = this.FrequenceA.GetTotalPhaseCorrection().TotalCorrection ;
                double rangeBCor = this.FrequenceB.GetTotalPhaseCorrection().TotalCorrection;
                double rangeCCor = this.FrequenceC.GetTotalPhaseCorrection().TotalCorrection;

                double comCor = PhaseCombinationBuilder.GetIonoFreeComValue(rangeACor, rangeBCor, rangeCCor, this);
                data.SetCorrection("频率ABC无电离层组合改正", comCor);
                return data;
            }
        }
        /// <summary>
        /// 对其后的相位观测量。改正后的相位值。是组合观测值和改正值之和
        /// </summary>
        public double AlignedIonoFreePhaseRange
        {
            get
            {
                //对齐周跳改正偏移。 模糊度准确才能保证计算的准确性！！！！！
                double Offset = this.Combinations.IonoFreePhaseRange.Frequence.GetDistance(AmbiguityOfIonoFreePhase);
                double correctedPhase = this.Combinations.IonoFreePhaseRange.CorrectedValue + Offset;
                return correctedPhase;
            }
        }
        /// <summary>
        /// 对其的三频无电离层组合
        /// </summary>
        public double AlignedIonoFreePhaseRangeOfTriFreq
        {
            get
            {
                //对齐周跳改正偏移。 模糊度准确才能保证计算的准确性！！！！！
                double Offset = this.Combinations.IonoFreePhaseRangeThreeFrequency.Frequence.GetDistance(AmbiguityOfIonoFreePhaseOfTriFreq);
                double correctedPhase = this.Combinations.IonoFreePhaseRangeThreeFrequency.CorrectedValue + Offset;
                return correctedPhase;
            }
        }

        #endregion


        /// <summary>
        /// 相位模糊度。检查并获取相位组合值与伪距对齐的模糊度。模糊度准确才能保证计算的准确性!!!!！！！！！
        /// </summary> 
        public long AmbiguityOfIonoFreePhase { get; set; }
        /// <summary>
        /// 相位模糊度。检查并获取相位组合值与伪距对齐的模糊度。模糊度准确才能保证计算的准确性!!!!！！！！！
        /// </summary> 
        public long AmbiguityOfIonoFreePhaseOfTriFreq { get; set; }
        /// <summary>
        /// 当前数值是否稳定标记，如是否具有周跳。模糊度是否已经固定。而非采用伪距改正的值。
        /// </summary>
        public bool IsUnstable { get; set; }

        /// <summary>
        /// 设置所有频率的周跳情况,设置频率的 IsCycleSliped 属性。
        /// </summary>
        /// <param name="trueOrFalse"></param>
        public void SetCycleSlip(bool trueOrFalse)
        {
            IsUnstable = trueOrFalse;
            foreach (var item in this) { item.IsCycleSliped = trueOrFalse; }
        }
        #endregion

        #endregion

        /// <summary>
        /// 观测数据（接收机）某一频率是否标记发生了周跳或信号失锁。
        /// </summary>
        /// <returns></returns>
        public bool IsRecordedCycleSlipe()
        {
            foreach (var item in this)
            {
                if (item.IsPhaseLossedLock) { return true; }
            }
            return false;
        }

        #region IO
        /// <summary>
        /// 简要字符描述
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Prn + ":" + this.RecevingTime + ", " + "载波频率数:" + this.FrequencyCount;
        }
        /// <summary>
        /// 表格的题目
        /// </summary>
        /// <returns></returns>
        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Ephemeris.GetTabTitles());

            sb.Append("\t");
            sb.Append("周跳否");
            sb.Append("\t");
            sb.Append("无电离层组合模糊度");

            sb.Append("\t");
            sb.Append(this.CommonCorrection.GetTabTitles().Replace("距离改正", ""));

            //每一个频率 
            foreach (var item in this.Values)
            {
                sb.Append("\t");
                sb.Append(item.GetTabTitles());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 表格的行
        /// </summary>
        /// <returns></returns>
        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Ephemeris.GetTabValues());

            sb.Append("\t");
            sb.Append(this.IsUnstable);
            sb.Append("\t");
            sb.Append(this.AmbiguityOfIonoFreePhase);

            sb.Append("\t");
            sb.Append(CommonCorrection.GetTabValues());

            //每一个频率 
            foreach (var item in this)
            {
                sb.Append("\t");
                sb.Append(item.GetTabValues());
            }
            return sb.ToString();
        }
        #endregion

        #region new adding
        /// <summary>
        /// 对流层湿延迟——系数，中对流层矫正器的时候赋值。
        /// </summary>
        public double WetMap { get; set; }

        /// <summary>
        /// 当前历元的对流层天顶延迟，直接计算而出，可以用于无对流参数的伪距定位。
        /// </summary>
        public double AppriorTropDelay { get; set; }

        /// <summary>
        /// 当前历元的天顶对流层湿延迟
        /// </summary>
        public double AppriorWetDelay { get; set; }
        /// <summary>
        /// 天顶方向映射函数
        /// </summary>
        public double WetMap_ZTD;

        /// <summary>
        /// VMF1的湿分量映射函数
        /// </summary>
        public double Vmf1WetMap { get; set; }
        /// <summary>
        /// DCB-P1C1
        /// </summary>
        public double DcbP1C1 { get; set; }
        #endregion
        /// <summary>
        /// 平差观测值获取
        /// </summary>
        /// <param name="obsDataType"></param>
        /// <returns></returns>
        public double GetAdjustValue(SatObsDataType obsDataType)
        {
            var obs = GetObsValue(obsDataType);
            var approx = GetApproxRange(obsDataType);
            var obsMinusApprox = obs - approx;
            return obsMinusApprox;
        }
        /// <summary>
        /// 观测值
        /// </summary>
        /// <param name="obsDataType"></param>
        /// <returns></returns>
        public double GetObsValue(SatObsDataType obsDataType)
        {
            return this[obsDataType].CorrectedValue;
        }



        /// <summary>
        /// 获取近似距离
        /// </summary>
        /// <param name="obsDataType"></param>
        /// <returns></returns>
        internal double GetApproxRange(SatObsDataType obsDataType)
        {
            double val = 0;
            switch (obsDataType)
            {
                case SatObsDataType.PhaseA:
                    val = this.ApproxPhaseRangeA.CorrectedValue;
                    break;
                case SatObsDataType.PhaseB:
                    val = this.ApproxPhaseRangeB.CorrectedValue;
                    break;

                case SatObsDataType.IonoFreeRange:
                    val = this.IonoFreeApproxPseudoRange.CorrectedValue;
                    break;
                case SatObsDataType.IonoFreePhaseRange:
                    val = this.IonoFreeApproxPhaseRange.CorrectedValue;
                    break;
                case SatObsDataType.AlignedIonoFreePhaseRange:
                    val = this.IonoFreeApproxPhaseRange.CorrectedValue;
                    break;
                case SatObsDataType.PseudoRangeA:
                    val = this.ApproxPseudoRangeA.CorrectedValue;
                    break;
                case SatObsDataType.PseudoRangeB:
                    val = this.ApproxPseudoRangeB.CorrectedValue;
                    break;
                case SatObsDataType.PseudoRangeC:
                    val = this.ApproxPseudoRangeC.CorrectedValue;
                    break;
                case SatObsDataType.PhaseRangeA:
                    val = this.ApproxPhaseRangeA.CorrectedValue;
                    break;
                case SatObsDataType.PhaseRangeB:
                    val = this.ApproxPhaseRangeB.CorrectedValue;
                    break;
                case SatObsDataType.PhaseRangeC:
                    val = this.ApproxPhaseRangeC.CorrectedValue;
                    break;
                default:
                    val = this.ApproxPseudoRangeA.CorrectedValue;
                    break;
            }
            return val;
        }

        /// <summary>
        /// 原始载波距离减去站星距离
        /// </summary>
        /// <param name="frequenceType"></param>
        /// <returns></returns>
        public double GetRawPhaseRangeResidual(FrequenceType frequenceType)
        {
            return this[frequenceType].PhaseRange.Value- this.EstmatedVector.Length;
        }

        /// <summary>
        /// 原始伪距距离减去站星距离
        /// </summary>
        /// <param name="frequenceType"></param>
        /// <returns></returns>
        public double GetRawRangeResidual(FrequenceType frequenceType)
        {
            return this[frequenceType].PseudoRange.Value- this.EstmatedVector.Length;
        }
        /// <summary>
        /// MW值，仅对C1改正
        /// </summary>
        public double MwCycle
        {
            get
            {
                //EpochSatellite epochSat = EpochSat;
                var f1 = FrequenceA.Frequence.Value;
                var f2 = FrequenceB.Frequence.Value;


                double L1 = this.FrequenceA.PhaseRange.Value;
                double L2 = this.FrequenceB.PhaseRange.Value;
                //用原始的P1观测值
                double P1 = this.FrequenceA.PseudoRange.Value;
                //对CA码，改P1-C1
                if (this.FrequenceA.PseudoRange.GnssCodeType == GnssCodeType.CA)
                {
                    P1 = P1 + this.DcbP1C1;
                }
                //用原始的P2观测值
                double P2 = this.FrequenceB.PseudoRange.Value;

                double value = GetMwValue(f1, f2, L1, L2, P1, P2) / this.Combinations.MwRangeCombination.Frequence.WaveLength;
                return value;

                //double freqVal = f1 - f2; //此处采用宽项的频率，以周为单位推导波长和频率

                //Frequence freqence = new Frequence("MW_" + epochSat.Prn.SatelliteType, freqVal);
                //return new PhaseCombination(value, freqence);
            }

        }
        public static double GetMwValue(double f1, double f2, double L1, double L2, double P1, double P2)
        {
            double e = f1 / (f1 - f2);
            double f = f2 / (f1 - f2);
            double c = f1 / (f1 + f2);
            double d = f2 / (f1 + f2);

            double value =
                e * L1
              - f * L2

              - c * P1
              - d * P2;
            return value;
        }

        /// <summary>
        /// mp1，多路径指标
        /// </summary>
        public double Mp1
        {
            get
            {
                var f1 = FrequenceA.Frequence.Value;
                var f2 = FrequenceB.Frequence.Value;


                double L1 = this.FrequenceA.PhaseRange.Value;
                double L2 = this.FrequenceB.PhaseRange.Value;
                //用原始的P1观测值
                double P1 = this.FrequenceA.PseudoRange.Value;
                //用原始的P2观测值
                double P2 = this.FrequenceB.PseudoRange.Value;
                double alfa = f1 * f1 / (f2 * f2);
                double mp1 = P1 - (1 + 2 / (alfa - 1)) * L1 + (2 / (alfa - 1)) * L2;           
                return mp1;
            }
        }
        /// <summary>
        /// mp2，多路径指标
        /// </summary>
        public double Mp2
        {
            get
            {
                var f1 = FrequenceA.Frequence.Value;
                var f2 = FrequenceB.Frequence.Value;


                double L1 = this.FrequenceA.PhaseRange.Value;
                double L2 = this.FrequenceB.PhaseRange.Value;
                //用原始的P1观测值
                double P1 = this.FrequenceA.PseudoRange.Value;
                //用原始的P2观测值
                double P2 = this.FrequenceB.PseudoRange.Value;
                double alfa = f1 * f1 / (f2 * f2);
                double mp2 = P2 - (2 * alfa / (alfa - 1)) * L1 + (2 * alfa / (alfa - 1) - 1) * L2;
                return mp2;
            }
        }

        /// <summary>
        ///获取这颗卫星的观测类型
        /// </summary>
        /// <returns></returns>
        public List<ObservationCode> GetObservationCodes()
        {
            List<ObservationCode> list = new List<ObservationCode>();
            foreach (var freqObs in this)
            {
                foreach (var sameType in freqObs)
                {
                    foreach (var item in sameType)
                    {
                        list.Add(item.ObservationCode);
                    }
                }
            }

            return list;
        }
    }
}