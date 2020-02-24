﻿//2014.06.24, czs, edit, 成为 GnssDataType 的遍历器
//2014.08.19, czs, edit, 格式修改，微调
//2015.05.09, czs, edit in namu, 增加移除观测类型的方法，类名 ObservationData 修改为 SatObsData 
//2018.07.18, czs, edit in HMX, 采用ObservationCode进行转换
//2018.09.08, czs, edit in hmx, 增加移除指定频率编号

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo.Utils;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// 具有时间的历元数据。
    /// 存储一个卫星的观测值记录
    /// </summary>
    public class TimedRinexSatObsData 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TimedRinexSatObsData() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="data"></param>
        public TimedRinexSatObsData(Geo.Times.Time Time , RinexSatObsData data) {
            this.Time = Time;
            this.SatObsData = data;
        }
        /// <summary>
        /// 观测记录
        /// </summary>
        public RinexSatObsData SatObsData { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public Geo.Times.Time Time { get; set; }
    }
    
    /// <summary>
    /// 存储一个卫星的观测值记录。
    /// 直接读取自RINEX文件，是原始的数据，不应该出现在计算当中。
    /// 是观测数据类型的遍历器。
    /// </summary>
    public class RinexSatObsData : IEnumerable<KeyValuePair<string, RinexObsValue>>
    {
        /// <summary>
        /// 初始化变量。
        /// </summary>
        public RinexSatObsData()
        {
            _values = new Dictionary<string, RinexObsValue>();
            RinexVersion = 3.02;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Prn"></param>
        public RinexSatObsData(SatelliteNumber Prn):this()
        {
            this.Prn = Prn;
            RinexVersion = 3.02;
        }

        /// <summary>
        /// RIENX 文件版本，或者直接引入 Header ？？？ //2018.09.25,czs, hmx
        /// </summary>
        public double RinexVersion { get; set; }
        #region 变量，属性
        /// <summary>
        /// 观测数据原始记录。
        /// </summary>
        private Dictionary<string, RinexObsValue> _values;
        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        /// <summary>
        /// 观测类型
        /// </summary>
        public List<string> ObsTypes { get { return new List<string>(_values.Keys); } }

        /// <summary>
        /// 检索器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public RinexObsValue this[string type] { get { return this._values[type]; } set { this._values[type] = value; } }
        #endregion
        /// <summary>
        /// L1 返回第一个匹配非0结果, 无则为null
        /// </summary>
        public RinexObsValue PhaseA { get { return GetRinexObsValue(FrequenceType.A, "L"); } }

        /// <summary>
        /// L2 返回第一个匹配非0结果, 无则为null
        /// </summary>
        public RinexObsValue PhaseB { get { return GetRinexObsValue(FrequenceType.B, "L"); } }

        /// <summary>
        /// P1 or C1 返回第一个匹配非0结果, 无则为null
        /// </summary>
        public RinexObsValue RangeA { get { return GetRinexObsValue(FrequenceType.A, "P","C"); } }

        /// <summary> 
        /// P2 or C2 返回第一个匹配非0结果, 无则为null
        /// </summary>
        public RinexObsValue RangeB { get { return GetRinexObsValue(FrequenceType.B, "P", "C"); } }

        public RinexObsValue FirstAvailable { get{
                if (PhaseA != null)
                {
                    return PhaseA; 
                }
                return PhaseB;
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="frequenceType"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private RinexObsValue GetRinexObsValue(FrequenceType frequenceType, params string [] prefix)
        {
            List<int> builedFreq = ObsCodeConvert.GetRinexFrequenceNumber(Prn.SatelliteType, frequenceType);
            foreach (var item in this)
            {
                foreach (var freq in builedFreq)
                {
                    foreach (var pre in prefix)
                    {
                        var keyCode = pre + freq;
                        if (item.Key.Contains(keyCode) && item.Value.Value != 0)
                        {
                            return item.Value;
                        }
                    }
                }
            }
            return null;
        }

        #region 方法
        /// <summary>
        /// 是否可以组成无电离层组合
        /// </summary>
        public bool IsIonoFreeCombinationAvaliable
        {
            get => PhaseA != null && PhaseA.Value != 0
                && PhaseB != null && PhaseB.Value != 0
                && RangeA != null && RangeA.Value != 0
                && RangeB != null && RangeB.Value != 0;
        }

        /// <summary>
        /// 移除对于双频无电离层组合多余的观测量
        /// </summary>
        public void RemoveRedundantObsForIonoFree()
        {
            List<int> freqA = ObsCodeConvert.GetRinexFrequenceNumber(Prn.SatelliteType, FrequenceType.A);
            List<int> freqB = ObsCodeConvert.GetRinexFrequenceNumber(Prn.SatelliteType, FrequenceType.B);
          
            List<int> total = new List<int>(freqA);
            total.AddRange(freqB);

            this.RemoveOtherFrequences(total);
        }


        /// <summary>
        /// 获取频率列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetFrequenceNums()
        {
            List<int> result = new List<int>();
            foreach (var item in this)
            {
                int freqNum = item.Value.ObservationCode.BandOrFrequency;
                if (!result.Contains(freqNum)) { result.Add(freqNum); }
            }
            return result;
        }

        /// <summary>
        /// 获取同一频率的观测量集合。
        /// </summary>
        /// <param name="freqNum"></param>
        /// <param name="ignoreZero">忽略0值,节约内存</param>
        /// <returns></returns>
        public List<RinexObsValue> GetObservations(int freqNum, bool ignoreZero = true)
        {
            List<RinexObsValue> result = new List<RinexObsValue>();
            foreach (var item in this)
            {
                var obs = item.Value;
                if(ignoreZero && obs.Value == 0) { continue; }

                if (obs.ObservationCode.BandOrFrequency == freqNum)
                {
                   result.Add(obs); 
                }
            }
            return result;
        }

        /// <summary>
        /// 移除指定的观测量。
        /// </summary>
        /// <param name="type"></param>
        public void Remove(string type)
        {
            if (Contains(type))
            {
                this._values.Remove(type);
            }
        } 
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="types"></param>
        public void Remove(List<string> types)
        {
            foreach (var item in types)
            {
                Remove(item);
            } 
        } 
        /// <summary>
        /// 添加一个观测类型和值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        public void Add(string type, RinexObsValue val)
        {
            if(val != null && val.Value != 0)//可以避免，重复键错误
            {
                this._values[type] = val;
            }
        }
        /// <summary>
        /// 添加一个观测类型和值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        public void Add(string type, double val)
        {
            this._values.Add(type, new RinexObsValue(val,type));
        }
        /// <summary>
        /// 顺序遍历，选择第一个非0值的代码编号.如果没有找到，则返回null。
        /// </summary>
        /// <param name="candidateCodes"></param>
        /// <returns></returns>
        public string GetFirstValuableCode(List<string> candidateCodes)
        {
            foreach (var item in candidateCodes)
            {
                if (TryGetValue(item) != 0)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 获取记录值,如果没有该记录，则返回 0 .
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns></returns>
        public double TryGetValue(string type)
        {
            var val = TryGetObsValue(type);
            if (val != null) { return val.Value; } 

            return 0; ;
        }
        /// <summary>
        /// 返回原始观测，若无返回 null
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public RinexObsValue TryGetObsValue(string type)
        {
            if (Contains(type)) { return _values[type]; }

            //有卫星，但无观测数据
            if (this.ObsTypes.Count == 0) return null;
            //若代码小于2，而数据代码为 3，则需要匹配
            if (type.Length < 3 && this.ObsTypes[0].Length == 3)
            {
                var codes = ObservationCode.GetCodesV3(type);

                foreach (var c in codes)
                {
                    if (Contains(c)) { return _values[c]; }
                }
            }

            //如果输入为 3，而数据代码为 2， 则需减少一个代码
            var code = new ObservationCode(type);
            var codeV2 = code.GetRinexCode(2.01);
            if (Contains(codeV2)) { return _values[codeV2]; }


            int i = 0;
            return null; ;
        }

        /// <summary>
        /// 移除不包括的频率编号
        /// </summary>
        /// <param name="frequenceNumToRemained"></param>
        internal void RemoveOtherFrequences(List<int> frequenceNumToRemained)
        {
            List<string> teberemoved = new List<string>();
            foreach (var item in this)
            {
                if(!frequenceNumToRemained.Contains(item.Value.ObservationCode.BandOrFrequency)) 
                { 
                   teberemoved.Add(item.Key); 
                }
            }
            this.Remove(teberemoved);
        }
        /// <summary>
        /// 移除指定频率编号
        /// </summary>
        /// <param name="frequenceNumToBeRemoved"></param>
        internal void RemoveFrequences(List<int> frequenceNumToBeRemoved)
        {
            List<string> teberemoved = new List<string>();
            foreach (var item in this)
            {
                foreach (var freqNum in frequenceNumToBeRemoved)
                {
                    if (item.Key.Contains(freqNum.ToString()))
                    {
                        teberemoved.Add(item.Key);
                        break;
                    }
                } 
            }
            this.Remove(teberemoved);
        }

        /// <summary>
        /// 获取值，若第一个非0，则直接返回。
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public double TryGetValue(List<string> types)
        {
            double val = 0;
            foreach (var type in types)
            {
                val = TryGetValue(type);
                if (val != 0) break;
            }
            return val;
        }
        /// <summary>
        /// 是否包含数据类型
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns></returns>
        public bool Contains(string type) { return _values.ContainsKey(type); }
         
        #endregion

        #region Override
        /// <summary>
        /// 遍历器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, RinexObsValue>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }
        #endregion

        /// <summary>
        /// 移除其它。
        /// </summary>
        /// <param name="list"></param>
        public void RemoveOthers(List<string> list)
        {
            foreach (var item in ObsTypes.ToArray())
            {
                if (!list.Contains(item))
                { this.Remove(item); }
            }
        }
    }
         
}
