//2014.08.20，czs, 设计，改进，Gnsser核心模型！
//2014.12.01, czs, edit in liangku shangliao, 新增 CorrectableNeu，记录本历元测站改正数
//2015.05.14, czs, refacter，重构为字典管理者,采用 Builder 脱离与数据源的相关性，即实现解耦，为下一步多数据源（非RINEX）初始化做准备
//2015.05.28, czs, add in naum, 增加 EstmatedXyz 作为计算后的坐标值。
//2016.03.23, czs, edit in hongqing, 进行了简单的梳理和调整
//2016.08.19, czs, edit in huangshang 屯溪 阿拉酒店, 增加历元编号属性
//2016.10.14, double edit in hongqing 增加了适用于三频的代码
//2017.05.10, lly, edit in zz, 考虑不通过观测类型的伪距近似值
//2017.09.19, czs, edit in hongqing, 获取近似距离从EpochInfo转移到EpochSat

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;

namespace Gnsser.Domain
{
    /// <summary>
    /// 历元全信息。
    /// 一个历元测站观测到一颗卫星的数据信息，是计算数据基本的存储类，是一个线段和两个点的信息。一个指定时刻的观测信息 和 对应的星历信息。
    /// 一个该类的对象就可以进行定位。
    /// 注意：这是本程序内部核心的数据存储模型，各种数据格式都需要转换为本模型后，再开展计算工作。
    /// </summary>
    public class EpochInformation : BaseDictionary<SatelliteNumber, EpochSatellite>, IEpochInfo, IToTabRow, IEnabledMessage, ISiteSatObsInfo, IEpochObsData, IValueClone<EpochInformation>
    {
        #region 构造函数和初始化函数
        /// <summary>
        /// 构造函数，初始化基本变量。
        /// </summary>
        public EpochInformation()
        {
            this.Enabled = true;
            this.CorrectableNeu = new CorrectableNEU();
            this.NumeralCorrections = new NumeralCorrectionManager();
            this.NumeralCorrections[Gnsser.ParamNames.WetTropZpd] = 0.1;
            this.RemovedPrns = new List<SatelliteNumber>();
        }


        #endregion

        #region 属性
        /// <summary>
        /// 历元列表
        /// </summary>
        public List<Time> Epoches => new List<Time>() { ReceiverTime};
        /// <summary>
        /// 记录已经移除的卫星编号
        /// </summary>
        public List<SatelliteNumber> RemovedPrns { get; set; }
        /// <summary>
        /// 历元编号，一天之内的，通过时间和采样率计算出。
        /// </summary>
        public int EpochIndexOfDay { get { return (int)(ReceiverTime.DateTime.TimeOfDay.TotalSeconds / ObsInfo.Interval); } }

        #region 基本属性
        /// <summary>
        /// 测站相关的单值改正数或误差项，如对流层天顶距方向，电离层，钟差改正等。
        /// </summary>
        public NumeralCorrectionManager NumeralCorrections { get; set; }
        /// <summary>
        /// 测站名称 MarkerName,通常为4-8个字符的字符串
        /// </summary>
        public string SiteName { get { return SiteInfo.SiteName; } }
        /// <summary>
        /// 通常为文件名称，如 BJFS002.16O，作为不同测站之间的标识。
        /// </summary>
        public override string Name { get { return base.Name; } set { base.Name = value; } }
      /// <summary>
        /// 尝试更新观测码改正，已经包括了 C1C2 P1P2
        /// </summary>
        internal void TryUpdateObsWithCorrections()
        {
            foreach (var item in this)
            {
                item.TryUpdateObsWithCorrections();
            }
        }


        ///// <summary>
        ///// 原始历元观测数据，可以是 RINEX，RTCM3、BINEX对象等。
        ///// </summary>
        //public IEpochObsData EpochObsData { get; set; }



        /// <summary>
        /// GNSS 测站固定信息，包括接收机和天线信息。
        /// </summary>
        public ISiteInfo SiteInfo { get; set; }
        /// <summary>
        /// 快速获取整个观测信息。
        /// </summary>
        public IObsInfo ObsInfo { get; set; }
        /// <summary>
        /// 历元标记，对应于 RINEX EpochFlag
        /// 历元标志：0表示正常，1表示在前一历元与当前历元之间发生了电源故障，>1为事件标志
        /// If EVENT FLAG record (epoch flag > 1):         
        /// - Event flag:                                
        /// 2: start moving antenna                     
        /// 3: new site occupation (end of kinem. satData) (at least MARKER NAME record follows)   
        /// 4: header information follows             
        /// 5: external event (epoch is significant,same time frame as observation time tags)
        /// 6: cycle slip records follow to optionally  report detected and repaired cycle slips (same format as OBSERVATIONS records;  
        ///slip instead of observation; LLI and signal strength blank)  
        /// </summary>
        public EpochState EpochState { get; set; }
        /// <summary>
        /// 支持的系统
        /// </summary>
        public List<SatelliteType> SatelliteTypes
        {
            get
            {
                return (from prn in TotalPrns select prn.SatelliteType).Distinct().ToList();
            }
        }
        /// <summary>
        /// 测站的本地NEU坐标。通常由各种改正数改正而来。
        /// 具有时效性，每个历元都是一个独立的对象。
        /// </summary>
        public CorrectableNEU CorrectableNeu { get; private set; }
        /// <summary>
        /// 估值坐标加上NEU改正
        /// </summary>
        /// <returns></returns>
        public XYZ CorrecedXYZ
        {
            get=> Geo.Coordinates.CoordTransformer.EnuToXyz( new ENU( CorrectableNeu.CorrectedValue), this.SiteInfo.EstimatedXyz);
        }

        /// <summary>
        /// 通过数字编号来获取测站卫星向量。
        /// </summary>
        /// <param name="i">编号</param>
        /// <returns></returns>
        public EpochSatellite this[int i] { get { return this[TotalPrns[i]]; } }

        #region 时间
        /// <summary>
        /// 历元观测时间，接收机钟面时，原始观测时间，没有经过改正。
        /// 虽然本时间不够精确，但是我是一成不变的，这是我的优势，哈哈哈哈！！！
        /// </summary>
        public Time ReceiverTime { get { return Time.Value; } }
        /// <summary>
        /// 接收机钟改正后的历元。更加精确，但是改正一次就变化一次，不可以用作字典关键字Key。
        /// </summary>
        public Time CorrectedTime { get { return Time.CorrectedValue; } }
        /// <summary>
        /// 接收机观测时间。具有改正数。
        /// </summary>
        public CorrectableTime Time { get; set; }
        #endregion

        #region 卫星编号
        /// <summary>
        /// 具有周跳标记的卫星列表。
        /// </summary>
        public List<SatelliteNumber> GetCycleSlipedPrns(bool enabledSatOnly = false)
        {
            var prns = new List<SatelliteNumber>();
            foreach (var item in this.GetData())
            {
                if (enabledSatOnly && !item.Value.Enabled) { continue; }

                if (item.Value.IsRecordedCycleSlipe()) { prns.Add(item.Key); }
            }
            return prns;
        }
        /// <summary>
        /// 本历元所观测到的所有的卫星编号列表。
        /// </summary>
        public List<SatelliteNumber> TotalPrns { get { return (Keys); } }
        /// <summary>
        /// 可用卫星编号列表
        /// </summary>
        public List<SatelliteNumber> EnabledPrns { get=> (from sat in EnabledSats select sat.Prn).ToList(); }// TotalPrns.FindAll(m => this[m].Enabled); } }
        /// <summary>
        /// 可用卫星编号列表
        /// </summary>
        public List<SatelliteType> EnabledSatelliteTypes
        {
            get
            {
                List<SatelliteType> list = new List<SatelliteType>();

                foreach (var item in this)
                {
                    if (!list.Contains(item.Prn.SatelliteType))
                    {
                        list.Add(item.Prn.SatelliteType);
                    }
                }
                return list;
            }
        }
        /// <summary>
        /// 不可用卫星编号列表
        /// </summary>
        public List<SatelliteNumber> DisabledPrns { get { return TotalPrns.FindAll(m => !this[m].Enabled); } }
        /// <summary>
        /// 不稳定的卫星，通常为具有周跳的卫星。
        /// </summary>
        public List<SatelliteNumber> UnstablePrns
        {
            get
            {
                List<SatelliteNumber> unstablePrns = new List<SatelliteNumber>();
                foreach (var item in this)
                {
                    if (item.IsUnstable)
                    {
                        unstablePrns.Add(item.Prn);
                    }
                }
                return unstablePrns;
            }
        }
        /// <summary>
        /// 所有的卫星数量。
        /// </summary>
        public int TotalSatCount { get { return TotalPrns.Count; } }
        /// <summary>
        /// 可用的卫星数量
        /// </summary>
        public int EnabledSatCount { get { return EnabledSats.Count; } }
        /// <summary>
        /// 返回所有未禁用的卫星
        /// </summary>
        public List<EpochSatellite> EnabledSats { get { return new List<EpochSatellite>(this.Values.FindAll(m => m.Enabled)); } }
        /// <summary>
        /// 具有周跳的卫星编号
        /// </summary>
        public List<SatelliteNumber> SlipedPrns
        {
            get
            {
                List<SatelliteNumber> satwithcs = new List<SatelliteNumber>();
                foreach (var item in this)
                {
                    if (item.IsUnstable)
                    {
                        satwithcs.Add(item.Prn);
                    }
                }
                return satwithcs;
            }
        }
        /// <summary>
        /// 所有卫星星历集合，方便查看
        /// </summary>
        public List<IEphemeris> TotalEphemerises
        {
            get
            {
                List<IEphemeris> list = new List<IEphemeris>();

                foreach (var item in this)
                {
                    list.Add(item.Ephemeris);
                }
                return list;
            }
        }
        #endregion

        #endregion

        #region IOrderedProperty 接口
        /// <summary>
        /// 排好序的属性名称
        /// </summary>
        public List<string> OrderedProperties { get; protected set; }
        /// <summary>
        /// 排好序的属性名称
        /// </summary>
        public List<ValueProperty> Properties { get; protected set; }
        #endregion

        #region EnabledMessage接口
        /// <summary>
        /// 对象信息，如果对象停用了，一般给出其原因。
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 是否可用，是否启用。
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// MW 值
        /// </summary>
        public Dictionary<SatelliteNumber, double> MWs
        {
            get
            {
                Dictionary<SatelliteNumber, double> mws = new Dictionary<SatelliteNumber, double>();
                foreach (var item in this)
                {
                    if (item.Enabled)
                        mws.Add(item.Prn, item.Combinations.MwPhaseCombinationValue);
                }
                return mws;
            }
        }
        #endregion
        #endregion

        #region 方法
        /// <summary>
        /// 是否包含指定系统
        /// </summary>
        /// <param name="SatelliteType"></param>
        /// <returns></returns>
        public bool Contains(SatelliteType SatelliteType) { return SatelliteTypes.Contains(SatelliteType); }
        /// <summary>
        /// 观测数据（接收机），或则某一卫星，是否标记发生了周跳或信号失锁。
        /// </summary>
        /// <returns></returns>
        public bool IsRecordedCycleSlipe()
        {
            bool isRecordedCycleSlipe = this.EpochState != EpochState.Ok;
            if (isRecordedCycleSlipe) return true;

            foreach (var item in this)
            {
                if (item.IsRecordedCycleSlipe()) return true;
            }
            return false;
        }

        #region  用于平差计算
        /// <summary>
        ///  返回的是残差值, 观测伪距向量，包含了观测值和近似值，可以直接用于计算，也可以拆分后计算。
        /// </summary>
        /// <param name="obsDataType">数据类型</param>
        /// <param name="isPhase">是否为载波</param>
        /// <param name="enabledSatOnly">是否只采用未禁用的卫星</param>
        /// <returns></returns>
        public AdjustVector GetAdjustVector(SatObsDataType obsDataType, bool enabledSatOnly = true)
        {
            var app = GetApproxRangeVector(obsDataType, enabledSatOnly);
            var obs = GetObsVector(obsDataType, enabledSatOnly);

            return new AdjustVector(obs, app);
        }
        /// <summary>
        /// 获取近似距离向量
        /// </summary>
        /// <param name="obsDataType"></param>
        /// <param name="enabledSatOnly"></param>
        /// <returns></returns>
        public Vector GetApproxRangeVector(SatObsDataType obsDataType, bool enabledSatOnly = true)
        {
            int count = enabledSatOnly ? EnabledSatCount : TotalSatCount;
            Vector app = new Vector(count);
            int i = 0;

            #region switch 结构
            foreach (var sat in this.Values)
            {
                if (enabledSatOnly && !sat.Enabled) continue;

                var val = sat.GetApproxRange(obsDataType);
                app[i] = val;
                app.ParamNames[i] = sat.Prn.ToString();
                i++;
            }
            #endregion
            return app;
        }


        /// <summary>
        ///  观测向量或由观测值组合的向量.对伪距观测值加DCB改正，对载波相位观测值加天线相位缠绕
        /// </summary>
        /// <param name="obsDataType">数据类型</param> 
        /// <param name="enabledSatOnly">是否只采用未禁用的卫星</param>
        /// <returns></returns>
        public Vector GetObsVector(SatObsDataType obsDataType, bool enabledSatOnly = true)
        {
            int count = enabledSatOnly ? EnabledSatCount : TotalSatCount;
            Vector obs = new Vector(count);
            int i = 0;
            foreach (var sat in this.Values)
            {
                if (enabledSatOnly && !sat.Enabled) continue;

                //观测值，或组合值
                var obj = sat[obsDataType];
                obs[i] = obj.CorrectedValue;
                obs.ParamNames[i] = sat.Prn + "";

                i++;
            }
            return obs;
        }


        #endregion

        #region 常用方法
        /// <summary>
        /// 如果有该卫星，就如实返回，如果没有则返回false。
        /// </summary>
        /// <param name="prn">卫星编号</param>
        /// <returns></returns>
        public bool HasCycleSlip(SatelliteNumber prn) { if (Contains(prn)) { return this[prn].IsUnstable; } return false; }

        /// <summary>
        /// 清除所有改正数。
        /// </summary>
        public void ClearCorrections()
        {
            //this.Time.Correction = 0;
            this.CorrectableNeu.ClearCorrections();
            foreach (var item in this.Values)
            {
                item.ClearCorrections();
            }
        }
        /// <summary>
        /// 获取高度角最大的卫星编号
        /// </summary>
        /// <param name="isEnabledOnly"></param>
        /// <returns></returns>
        public SatelliteNumber GetMaxElevationPrn(bool isEnabledOnly = true)
        {
            var prns = isEnabledOnly ? this.EnabledPrns : this.TotalPrns;

            var maxSat = this[0];
            foreach (var sat in this)
            {
                if (maxSat.Polar.Elevation < sat.Polar.Elevation)
                {
                    maxSat = sat;
                }
            }

            return maxSat.Prn;
        }
        /// <summary>
        /// 获取高度角最大的卫星编号列表
        /// </summary>
        /// <param name="isEnabledOnly"></param>
        /// <returns></returns>
        public List<SatelliteNumber> GetMaxElevationPrns(bool isEnabledOnly = true)
        {
            List<SatelliteNumber> okPrns = isEnabledOnly ? this.EnabledPrns : this.TotalPrns;

            okPrns.Sort(delegate (SatelliteNumber a, SatelliteNumber b)
            {
                var differ = this[b].Polar.Elevation - this[a].Polar.Elevation;
                return (int)(differ * 360000);
            });
            return okPrns;
        }

        #endregion

        #region 整理、删除、同步卫星和观测数据
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="prns"></param>
        internal void Enable(List<SatelliteNumber> prns)
        {
            foreach (var prn in this.DisabledPrns)
            {
                if (prns.Contains(prn)) { this[prn].Enabled = true; }
            }
        }
        /// <summary>
        /// 禁用未指定卫星的观测数据。
        /// </summary>
        /// <param name="enabledPrns">启用的集合</param>
        /// <returns></returns>
        public void DisableOthers(List<SatelliteNumber> enabledPrns)
        {
            var find = this.EnabledPrns.FindAll(m => !enabledPrns.Contains(m));

            foreach (var item in find) { this[item].Enabled = false; }
            log.Debug(String.Format(new EnumerableFormatProvider(), this.Name + ", " + this.ReceiverTime.ToShortTimeString() + ", Disabled:{0}", enabledPrns));
        }
        /// <summary>
        /// 禁用指定卫星的观测数据。
        /// </summary>
        /// <param name="tobeDisabledPrns">待删除的卫星集合</param>
        /// <returns></returns>
        public void Disable(List<SatelliteNumber> tobeDisabledPrns)
        {
            foreach (var item in tobeDisabledPrns)
            { if (this.Contains(item)) this[item].Enabled = false; }

            log.Debug(String.Format(new EnumerableFormatProvider(), this.Name + ", " + this.ReceiverTime.ToShortTimeString() + ",  Disabled:{0}", tobeDisabledPrns));
        }
        /// <summary>
        /// 移除指定的卫星
        /// </summary>
        /// <param name="tobeRemovedPrns"></param>
        public void Remove(List<SatelliteNumber> tobeRemovedPrns, bool isShowReason = true, string info = "未说明原因")
        {
            if (tobeRemovedPrns == null || tobeRemovedPrns.Count == 0) { return; }

            foreach (var prn in tobeRemovedPrns) { this.Remove(prn); }
            if (isShowReason)
            {
                log.Debug(this.Name + ", " + this.ReceiverTime.ToShortTimeString() + ",删除了 " + Geo.Utils.EnumerableUtil.ToString(tobeRemovedPrns) + ", " + info);
            }
        }
        /// <summary>
        /// 移除不可以做无电离层组合的卫星
        /// </summary>
        public void RemoveIonoFreeUnavailable()
        {
            List<SatelliteNumber> tobeRemovedPrns = new List<SatelliteNumber>();
            foreach (var sat in this)
            {
                if (!sat.IsIonoFreeAvailable)
                {
                    tobeRemovedPrns.Add(sat.Prn);
                }
            }
            Remove(tobeRemovedPrns, true, "无法做无电离层组合");
        }
        /// <summary>
        /// 移除,并且说明移除的原因
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="info"></param>
        public virtual void Remove(SatelliteNumber prn, string info)
        {
            Remove(prn);
            log.Debug(this.Name + ", " + this.ReceiverTime.ToShortTimeString() + ",删除了 " + prn + ", " + info);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="prn"></param>
        public override void Remove(SatelliteNumber prn)
        {
            base.Remove(prn);
            if (!RemovedPrns.Contains(prn)) { RemovedPrns.Add(prn); }
        }
        /// <summary>
        /// 移除未包含的卫星系统
        /// </summary>
        /// <param name="types"></param>
        public void RemoveOtherGnssSystem(SatelliteType types) { RemoveOtherGnssSystem(new List<SatelliteType>() { types });  }
        /// <summary>
        /// 移除未包含的卫星系统
        /// </summary>
        /// <param name="types"></param>
        public void RemoveOtherGnssSystem(List<SatelliteType> types)
        {
            List<SatelliteNumber> tobeRemoved = new List<SatelliteNumber>();
            foreach (var item in this)
            {
                if (!types.Contains(item.Prn.SatelliteType))
                {
                    tobeRemoved.Add(item.Prn);
                }
            }
            this.Remove(tobeRemoved, true, "移除非包含系统：" + Geo.Utils.EnumerableUtil.ToString(types));
        }

        public void RemoveUnStableMarkers()
        {
            foreach (var item in this)
            {
                item.IsUnstable = false;
            }
        }
        /// <summary>
        /// 删除没有星历的观测信息。
        /// </summary>
        public void DisableNoEphemeris()
        {
            //确保观测卫星中的卫星 与 星历数据源的卫星对应。
            List<SatelliteNumber> prnsToDelete = new List<SatelliteNumber>();

            foreach (var item in this.Values)
            {
                if (!item.HasEphemeris)
                    prnsToDelete.Add(item.Prn);
            }
            if (prnsToDelete.Count > 0)
            {
                Disable(prnsToDelete);
            }
        }


        /// <summary>
        /// 删除没有星历的观测信息。
        /// </summary>
        public void RemoveNoEphemeris()
        {
            //确保观测卫星中的卫星 与 星历数据源的卫星对应。
            List<SatelliteNumber> prnsToDelete = new List<SatelliteNumber>();

            foreach (var item in this.Values)
            {
                if (!item.HasEphemeris)
                    prnsToDelete.Add(item.Prn);
            }
            if (prnsToDelete.Count > 0)
            {
                log.Debug(Name + " " + ReceiverTime + ", 删除没有星历的观测量。");
                Remove(prnsToDelete);
            }
        }

        /// <summary>
        /// 给指定卫星添加停用标记，而其余的标记为启用。这种卫星将不参与计算。
        /// </summary>
        /// <param name="tobeDisables">待停用卫星</param>
        public void DisableAndEnableOthers(List<SatelliteNumber> tobeDisables)
        {
            if (tobeDisables == null) tobeDisables = new List<SatelliteNumber>();

            foreach (var item in this)
            {
                if (tobeDisables.Contains(item.Prn)) item.Enabled = false;
                else item.Enabled = true;
            }
        }

        #endregion
        /// <summary>
        /// 所有数据内容都克隆一个
        /// </summary>
        /// <returns></returns>
        public EpochInformation ValueClone()
        {
            EpochInformation info = new EpochInformation();
            info.SiteInfo = this.SiteInfo;
            info.Name = this.Name;
            info.Time = this.Time;
            foreach (var item in this)
            {
                var sat = item.ValueClone();
                sat.EpochInfo = info;
                info[item.Prn] = sat;
            }
            return info;
        }
        /// <summary>
        /// 改正钟跳，只改正数值，不改正钟。
        /// </summary>
        /// <param name="ClockJumpCorretion"></param>
        /// <param name="ClockJumpState"></param>
        public void CorrectClockJump(double ClockJumpCorretion, ClockJumpState ClockJumpState)
        { 
            if(ClockJumpCorretion == 0.0) { return; }

            foreach (var sat in this)
            {
                sat.CorrectClockJump(ClockJumpCorretion, ClockJumpState);
            }

            this.EpochState = EpochState.Ok;
        }

        #endregion

        #region static
        /// <summary>
        /// 快速生成
        /// </summary>
        /// <param name="epochObs"></param>
        /// <param name="SatelliteTypes"></param>
        /// <returns></returns>
        public static EpochInformation Parse(RinexEpochObservation epochObs, List<SatelliteType> SatelliteTypes)
        {
            RinexEpochInfoBuilder buiderA = new RinexEpochInfoBuilder(SatelliteTypes);

            EpochInformation einfo = buiderA.Build(epochObs);
            //einfo.EpochObsData = epochObs;

            return einfo;
        }

        #endregion

        #region IO
        /// <summary>
        /// 打印输出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in SatelliteTypes)
            {
                sb.Append(GnssSystem.GetGnssType(item));
                sb.Append(",");
            }
            return Name + ", " + this.ReceiverTime.ToString()+ ", " + EpochState + ", GNSS:" + sb.ToString()
                + ",EnabledSatCount:" + this.EnabledSatCount
                + ":" + String.Format(new EnumerableFormatProvider(), "{0:,}", EnabledPrns)
                + ",TotalSatCount:" + this.TotalSatCount + ", Coord(" + SiteInfo.ApproxXyz + ")";
        }
        /// <summary>
        /// 简短显示历元信息，包括名称和时间
        /// </summary>
        /// <returns></returns>
        public string ToShortString() { return Name + ", " + this.ReceiverTime.ToString(); }
        /// <summary>
        /// 获取表格分开的标题
        /// </summary>
        /// <returns></returns>
        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.SiteInfo.GetTabTitles());

            sb.Append("\t");
            sb.Append("时间" + this.Time.GetTabTitles());

            sb.Append("\t");
            sb.Append("本地坐标" + this.CorrectableNeu.GetTabTitles());

            return sb.ToString();
        }
        /// <summary>
        /// 获取表格分开的值
        /// </summary>
        /// <returns></returns>
        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.SiteInfo.GetTabValues());

            sb.Append("\t");
            sb.Append(this.Time.GetTabValues());

            sb.Append("\t");
            sb.Append(this.CorrectableNeu.GetTabValues());

            return sb.ToString();
        }

        /// <summary>
        /// 移除0值卫星。
        /// </summary>
        /// <param name="isRemoveZeroRange"></param>
        /// <param name="isRemoveZeroPhase"></param>
        /// <param name="freqCount"></param>
        public void RemoveZeroObsSat(bool isRemoveZeroRange = true, bool isRemoveZeroPhase = true, int freqCount = 2)
        {
            List<SatelliteNumber> zeroSats = new List<SatelliteNumber>();
            foreach (var sat in this)
            {
                int index = 0;
                foreach (var freq in sat)
                {
                    if (index >= freqCount) { continue; }

                    if (isRemoveZeroRange && freq.PseudoRange.Value == 0)
                    {
                        zeroSats.Add(sat.Prn);
                        continue;
                    }
                    if (isRemoveZeroPhase && freq.PhaseRange.Value == 0)
                    {
                        zeroSats.Add(sat.Prn);
                        continue;
                    }
                    index++;
                }
            }

            this.Remove(zeroSats, true, "观测值为 0 的卫星");
        }
        /// <summary>
        /// 具有星历的卫星列表
        /// </summary>
        /// <returns></returns>
        public List<EpochSatellite> GetEpochSatWithEphemeris()
        {
            List<EpochSatellite> sats = new List<EpochSatellite>();
            foreach (var item in this)
            {
                if (item.HasEphemeris) { sats.Add(item); }
            }
            return sats;
        }
        #endregion



    }
}