//2014.08.23, czs, create, 创建载波领域模型类，Gnsser核心模型！
//2014.09.07, czs, edit, 重构，将伪距等观测值移入本类中，本类存储一个载波频率的所有观测量。
//2014.11.29, czs, refactor, 增加单独的改正模型卫星频率改正模型 
//2015.05.14, czs, refactor, 实现字典管理器模式
//2018.07.18, czs, refactor in HMX, 同类型多属性观测数据支持，如C1、P1 或 C1X、C1C等

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Correction;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Correction;

namespace Gnsser.Domain
{
    //2018.08.19, czs, create in HMX, 数值克隆
    /// <summary>
    /// 数值克隆
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValueClone<T>
    {
        /// <summary>
        /// 数值克隆
        /// </summary>
        /// <returns></returns>
        T ValueClone();
    }


    //2018.07.18, czs, create in HMX, 支持不同属性的观测数据
    /// <summary>
    /// 支持不同属性的观测数据
    /// </summary>
    public class SameTypeObservations : BaseDictionary<string, Observation>, IValueClone<SameTypeObservations>, IUpdateValueWithCorrections
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="type"></param>
        public SameTypeObservations(ObservationType type)
        {
            this.ObservationType = type;
        }

        /// <summary>
        /// 观测码类型
        /// </summary>
        public ObservationType ObservationType { get; set; }
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
                    names.AddRange(item.CorrectionNames);
                }
                return names;
            }
        }

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Type: " + ObservationType + ", Count: " + this.Count + " , Detail: " + Geo.Utils.StringUtil.ToString(this.Keys);
        }
        /// <summary>
        /// 数值克隆
        /// </summary>
        /// <returns></returns>
        public SameTypeObservations ValueClone()
        {
            SameTypeObservations info = new SameTypeObservations(this.ObservationType);

            foreach (var item in this.Data)
            {
                if(item.Value is PhaseRangeObservation)
                {

                    info[item.Key] = ((PhaseRangeObservation)item.Value).ValueClone();
                }
                else
                {
                    info[item.Key] = item.Value.ValueClone();

                }
            }

            return info;
        }
        /// <summary>
        /// 更观测值，并清空改正数。
        /// </summary>
        public void UpdateValueWithCorrections()
        {
            foreach (var item in this)
            {
                item.UpdateValueWithCorrections();

            }
        }
    }




    /// <summary>
    /// 载波上的观测量，载波相位观测量类，包含指定频段载波上的所有的观测值，包含了伪距观测值和载波观测值，多普勒观测值（待实现）等。
    /// 本类是观测值类，数据一旦确定就不应该更改！
    /// </summary>
    public class FreqenceObservation : BaseDictionary<ObservationType, SameTypeObservations>, 
        ICommonObservationCorrection, IToTabRow, IEnabledMessage, IUpdateValueWithCorrections
    {
        /// <summary>
        /// 通用构造函数
        /// </summary>
        /// <param name="band">频率</param>
        /// <param name="FrequenceType">频率类型</param>
        public FreqenceObservation(Frequence band, FrequenceType FrequenceType)
        {
            this.FrequenceType = FrequenceType;
            this.Frequence = band;
            this.CommonCorrection = new NumerialCorrectionDic();
            this.PhaseOnlyCorrection = new NumerialCorrectionDic();
            this.RangeOnlyCorrection = new NumerialCorrectionDic();
            this.Enabled = true;
            this.Message = "";
        }


        #region 属性。变量      
        /// <summary>
        /// 标记是否启用。如孤立观测值，或者数据不完整等。可以将其屏蔽，不参与计算。
        /// </summary>
        public bool Enabled { get; set; }    
        /// <summary>
        /// 对象信息，如果对象停用了，一般给出其原因。
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 频率类型
        /// </summary>
        public FrequenceType FrequenceType { get; set; }

        /// <summary>
        /// 频率。
        /// </summary>
        public Frequence Frequence { get; private set; }
        /// <summary>
        /// 信号是否失锁，如载波重新计数。 根据观测信息LLI参数，指示是否具有周跳。算法有待考虑。？？？2014.09.13
        /// </summary>
        public bool IsPhaseLossedLock { get { int lli = (int)LossLockIndicator; if (lli == 1 || lli == 3 || lli == 5 || lli == 7)   return true; return false; } }
   
        /// <summary>
        /// 外部标记，是否发生了周跳
        /// </summary>
        public bool IsCycleSliped { get; set; }
        /// <summary>
        /// 多普勒频移。单位：Hz，每秒频率移动的次数。
        /// 注意：这里的方向与RINEX定义相同，如果为负数，则表示远离测站，如果为正数，则表示靠近Approach测站。
        /// </summary>
        public Observation DopplerShift { get { return Get(ObservationType.D).First; } }
        /// <summary>
        /// 根据多普勒频移计算出的卫星相对接收机的径向速度。
        /// 这是一个一维坐标，原点在测站，速度方向远离测站为正，靠近测站为负数。注意与多普勒频移的区别。 
        /// </summary>
        public double DopplerSpeed { get { return -1.0 * this.DopplerShift.CorrectedValue * this.Frequence.WaveLength; } }
        /// <summary>
        /// 载波上的伪距。默认伪距，优先返回 P 码。
        /// </summary>
        public Observation PseudoRange
        {
            get
            {
                if (!this.Contains(ObservationType.C))
                {
                    return new Observation(0, new ObservationCode(ObservationType.C, (int)FrequenceType, "C"));
                }
                Observation current = null;
                var cc = Get(ObservationType.C);
                foreach (var item in cc)
                {
                    if (item.Value != 0)//0 值就不考虑了
                    {
                        current = item;
                        if (current.GnssCodeType == GnssCodeType.P)
                        {
                            return current;
                        }
                    }
                }
                if(current == null)
                {
                    return new Observation(0, new ObservationCode(ObservationType.C, (int)FrequenceType, "C"));
                }

                return current;
            }
        }
        /// <summary>
        /// 返回所有的伪距观测值
        /// </summary>
        /// <returns></returns>
        public SameTypeObservations GetPseudoRanges()
        {
            return GetOrCreate(ObservationType.C);
        }

        /// <summary>
        /// 相位观测值等效的距离，单位：米。
        /// </summary>
        public PhaseRangeObservation PhaseRange
        {
            get
            {
                //return Get(ObservationType.L).First  as PhaseRangeObservation;
                if (!this.Contains(ObservationType.L))
                {
                    var code = new ObservationCode(ObservationType.L, (int)FrequenceType, "C");
                    return new PhaseRangeObservation(new RinexObsValue(0, code),Frequence.GpsL5);
                }

                foreach (var item in Get(ObservationType.L)) // 做 0 值判断
                {
                    var phase = item as PhaseRangeObservation;
                    if(phase == null || phase.Value == 0) { continue; }
                    return phase;
                }
                var c = new ObservationCode(ObservationType.L, (int)FrequenceType, "C");
                return new PhaseRangeObservation(new RinexObsValue(0, c), Frequence.GpsL5);
            }
        }

        /// <summary>
        /// 失锁指示.值同Rinex定义0 为OK。
        /// Loss of lock indicator (LLI). Range: 0-7   0 or blank: OK or not known     
        /// 如果值是 1，3，5，7 则表示有失锁或周跳。
        /// </summary>
        public LossLockIndicator LossLockIndicator { get { return PhaseRange.LossLockIndicator; } }

        /// <summary>
        /// 信号强度，值同Rinex定义Signal strength projected into interval 1-9:   
        /// 1: minimum possible signal strength     
        /// 5: threshold for good S/N ratio    
        /// 9: maximum possible signal strength   
        /// 0 or blank: not known, don't care   
        /// </summary>
        public int SignalStrength { get { return PhaseRange.SignalStrength; } }
        /// <summary>
        /// 近似模糊度长度。由伪距计算出。
        /// </summary>
        public double ApproxAmbiguityLength { get { return (PseudoRange.Value - PhaseRange.Value); } }
        /// <summary>
        /// 用于暂存模糊度参数。根据程序约定，自动设置值，如单位可以为米，周。可以包含其他误差，如电离层。
        /// </summary>
        public double TempAmbiguity { get; set; }
        /// <summary>
        /// 暂存模糊度和电离层之和
        /// Y 包含了当前历元的电离层和一半的模糊度距离
        /// </summary>
        public double TempAmbiguityAndIonoLength { get; set; }
        /// <summary>
        /// 临时的电离层改正
        /// </summary>
        public double TempIonoLength { get { return TempAmbiguityAndIonoLength - TempAmbiguity / 2.0; } }


        #endregion 


        #region 本频率改正数

        /// <summary>
        /// 本频率伪距的所有的改正数
        /// </summary>
        /// <returns></returns>
        public NumerialCorrectionDic GetTotalRangeCorrection()
        {
            NumerialCorrectionDic dic = GetCommonRangeCorrection();

            if (this.Contains(ObservationType.C))
            {
                //dic.AddCorrection(this.PseudoRange.Corrections);
            }

            return dic;
        }
        /// <summary>
        /// 本频率伪距的所有的改正数,距离
        /// </summary>
        /// <returns></returns>
        public NumerialCorrectionDic GetTotalPhaseCorrection()
        {
            NumerialCorrectionDic dic = GetCommonPhaseCorrection();

            if (this.Contains(ObservationType.L))
            {
                //dic.AddCorrection(this.PhaseRange.Corrections);
            }

            return dic;
        } 

        /// <summary>
        /// 本频率通用距离改正数，同时适用于载波相位和伪距，用于计算观测近似值。
        /// </summary>
        public NumerialCorrectionDic CommonCorrection { get; set; }
        /// <summary>
        /// 本频率相位距离改正数，只适用于载波相位，用于计算观测近似值。
        /// </summary>
        public NumerialCorrectionDic PhaseOnlyCorrection { get; set; }
        /// <summary>
        /// 伪距特有距离改正，如电离层改正+
        /// </summary>
        public NumerialCorrectionDic RangeOnlyCorrection { get; set; }
        /// <summary>
        /// 伪距数量
        /// </summary>
        public int PsuedoRangeCount { get { if (this.Contains(ObservationType.C)) return this[ObservationType.C].Count; return 0; } }

        public void UpdateValueWithCorrections()
        {
            foreach (var item in this)
            {
                item.UpdateValueWithCorrections();
            }
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
        /// 获取本频率与相位相关的距离改正
        /// </summary>
        /// <returns></returns>
        public NumerialCorrectionDic GetCommonPhaseCorrection()
        {
            NumerialCorrectionDic keyValues = new NumerialCorrectionDic();

            keyValues.SetCorrection(CommonCorrection.Corrections);
            keyValues.SetCorrection(PhaseOnlyCorrection.Corrections);
            //keyValues.AddCorrection("CommonRange", CommonCorrection.TotalCorrection);
            //keyValues.AddCorrection("PhaseOnly", PhaseOnlyCorrection.TotalCorrection);

            return keyValues;
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
        /// 添加相位改正。对所有的相位起作用。这里转换成相位观测值距离的改正数（Frequence.PhaseRange.Correction）。
        /// </summary>
        /// <param name="corrector">相位改正数，是相位</param>
        /// <param name="cycle">相位改正数，是相位</param>
        public void AddPhaseCyleCorrection(string corrector, double cycle)
        {
            double correction = cycle * Frequence.WaveLength / GnssConst.TWO_PI;
            AddPhaseCorrection(corrector, correction);
        }
        /// <summary>
        /// 添加周为单位的相位改正。对所有的观测值起作用。这里转换成观测值距离的改正数
        /// </summary>
        /// <param name="corrector">相位改正数，是相位</param>
        /// <param name="cycle">相位改正数，是相位</param>
        public void AddCommonCyleCorrection(string corrector, double cycle)
        {
            double correction = cycle * Frequence.WaveLength / GnssConst.TWO_PI;
            this.AddCommonCorrection(corrector, correction);
        }

        /// <summary>
        /// 添加本频率通用距离改正数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void AddCommonCorrection(string key, double val)
        {
            this.CommonCorrection.AddCorrection(key, val);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="ObservationType"></param>
        /// <param name="Observation"></param>
        public void Set(ObservationType ObservationType, Observation Observation)
        {
            var d = this.GetOrCreate(ObservationType);
            d.Add(Observation.ObservationCode.Attribute, Observation);
            
        }
        public override SameTypeObservations Create(ObservationType key)
        {
            return new SameTypeObservations(key);
        }

        /// <summary>
        /// 移除所有改正，通常是为了重新改正。
        /// </summary>
        public void ClearCorrections()
        {
            this.CommonCorrection.ClearCorrections();
            this.PhaseOnlyCorrection.ClearCorrections();
            foreach (var item in this)
            {
                foreach (var item2 in item)
                { 
                  item2.ClearCorrections();
                }
            } 
        }
        #endregion

        #region IO
        /// <summary>
        /// 改正后的字符串
        /// </summary>
        /// 
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Frequence);
            sb.Append(",");
            sb.Append(PseudoRange);
            sb.Append(",");
            sb.Append(PhaseRange);
            return sb.ToString();
           // return Frequence + ", " + PseudoRange.ToString() + ", " + PhaseRange;
        }

        /// <summary>
        /// 以制表位分割属性，利用导入到Excel中分析。
        /// </summary>
        /// <returns></returns>
        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(PseudoRange.GetTabValues());
            sb.Append("\t");
            sb.Append(PhaseRange.GetTabValues());
            if (CommonCorrection.Count > 1)
            {
                sb.Append("\t");
                sb.Append(this.CommonCorrection.GetTabValues());
            }
            if (PhaseOnlyCorrection.Count > 1)
            {
                sb.Append("\t");
                sb.Append(this.PhaseOnlyCorrection.GetTabValues());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 以制表位为分隔符的元素标题。
        /// </summary>
        /// <returns></returns>
        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Frequence.ToString() + "伪距");
            sb.Append(PseudoRange.GetTabTitles());
            sb.Append("\t");
            sb.Append(Frequence.ToString() + "载波");
            sb.Append(PhaseRange.GetTabTitles());
            if (CommonCorrection.Count > 1)
            {
                sb.Append("\t");
                sb.Append(this.CommonCorrection.GetTabTitles());
            }
            if (PhaseOnlyCorrection.Count > 1)
            {
                sb.Append("\t");
                sb.Append(this.PhaseOnlyCorrection.GetTabTitles());
            }
            return sb.ToString();
        }
        /// <summary>
        /// 数值克隆
        /// </summary>
        /// <returns></returns>
        internal FreqenceObservation DataClone()
        {
            FreqenceObservation info = new FreqenceObservation(this.Frequence,this.FrequenceType);

            foreach (var item in this.Data)
            {
                info[item.Key] = item.Value.ValueClone();
            }

            return info;
        }
        #endregion
    }
}
