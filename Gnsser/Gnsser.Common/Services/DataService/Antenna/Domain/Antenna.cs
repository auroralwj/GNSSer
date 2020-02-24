//2014.05.22, Cui Yang, created
//2014.08.18, Czs, edit, 代码重构,去掉了AntennaDataType枚举, 计算坐标统一为NEU
//2018.08.02, czs, edit in hmx, 检查并整理代码

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Times;
using Geo.Times; 
using Geo.Coordinates;
using Gnsser;

namespace Gnsser.Data
{
    /// <summary>
    /// 天线类。按照IGS标准构建。 
    /// </summary>
    public class Antenna : Gnsser.Data.IAntenna
    {
        Geo.IO.Log log = new Geo.IO.Log(typeof(Antenna));
        #region 构造函数
        /// <summary>
        /// 天线类默认构造函数 Default constructor
        /// </summary>
        public Antenna()
        {
            this.ValidDateFrom = Time.MinValue;
            this.ValidDateUntil = Time.MaxValue;
            this.Comments = new List<string>();
            this.Data =  new Dictionary<RinexSatFrequency, FrequencecyAntennaData>(); 
        }
         
        #endregion

        #region 核心变量
        /// <summary>
        /// 按照卫星频率存储的天线数据。
        /// </summary>
        public Dictionary<RinexSatFrequency, FrequencecyAntennaData> Data { get; private set; }
        #endregion

        #region 变量&属性
        /// <summary>
        /// 头部信息
        /// </summary>
        public AntennaHeader Header { get; set; }
        /// <summary>
        /// GNSS 系统频率
        /// </summary>
        public List<RinexSatFrequency> SatelliteFrequences { get => new List<RinexSatFrequency>(Data.Keys); }
         
        /// <summary>
        /// 代码
        /// </summary>
        public string SinexCode { get; set; }
        /// <summary>
        /// 卫星代码。 satellite obs Code
        /// </summary>
        public string SatCode { get; set; }
        /// <summary>
        /// 何年入轨，当年编号，发射编号等 COSPAR ID "YYYY-XXXA"，可选。
        /// </summary>
        public string CosparID { get; set; }

        /// <summary>
        /// antenna calibration agency
        /// </summary>
        public string Agency { get; set; }
        /// <summary>
        ///  校准的天线数量，一类型天线可能有多个。number of individual antennas calibrated
        /// </summary>
        public string NumOfAntennas { get; set; }
        /// <summary>
        /// 校准日期。calibration date
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 天线类型,一般都是大写。
        /// </summary> 
        public string AntennaType { get; set; }
        /// <summary>
        /// 天线罩
        /// </summary>
        public string Radome { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public string SerialOrSatFreqCode { get; set; }
        /// <summary>
        /// 校准方法
        /// </summary>
        public string CalibrationMethod { get; set; }
         
        /// <summary>
        /// 方位角的增量。Increment of the azimuth
        /// </summary>
        public double DeltaAzimuth { get; set; }
        /// <summary>
        /// 起始天顶角的值。 Initial zenith grid value
        /// </summary>
        public double ZenithStart { get; set; }
        /// <summary>
        /// 终点天顶角。 Final zenith grid value
        /// </summary>
        public double ZenithEnd { get; set; }
        /// <summary>
        /// 天顶角增量。 Increment of the zenith
        /// </summary>
        public double DeltaZenith { get; set; }
        /// <summary>
        /// 频率的数量。 Number of frequencies
        /// </summary>
        public int NumOfFrequencies { get; set; }
        /// <summary>
        /// 有效起始日期
        /// </summary>
        public Time ValidDateFrom { get; set; }
        /// <summary>
        /// 有效截止日期。
        /// </summary>
        public Time ValidDateUntil { get; set; }
        /// <summary>
        /// 有效时段
        /// </summary>
        public TimePeriod TimePeriod { get => new TimePeriod(ValidDateFrom, ValidDateUntil); }
        /// <summary>
        /// 注释。
        /// </summary>
        public List<string> Comments { get; set; }
   
        /// <summary>
        /// 相位偏心表频率的数量。
        /// Get aboutSize of antenna phase center eccentricities map.
        /// </summary>
        public int DataCount { get { return Data.Count; } }

        // Returns if this object is valid. The validity criteria is to
        // have a non-empty 'antennaData' map AND a non-empty 'antennaEccMap'.
        /// <summary>
        /// 是否有效。是否包含数据。
        /// </summary>
        public bool IsValid
        {
            get { return DataCount > 0; }
        }


        #endregion

        #region 对象方法
        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        public bool Contains(RinexSatFrequency freq)
        {
            return ShowedNoPcvFreqs.Contains(freq) || !(Data.ContainsKey(freq));
        }

        #region 插值计算核心方法
        /// <summary>
        /// 获取天线中心相位的改正量。不依赖方位角， 单位：度。如果没有找到，则返回 0.
        /// Get antenna phase center variation. Use this method when you
        /// don't have azimuth dependent phase center patterns.
        /// This method returns a Triple, in UEN system, with the elevation-dependent phase center variation.
        /// </summary>
        /// <param name="freq">GNSS系统和对应的频率 </param>
        /// <param name="elevation">高度角 elevation Elevation (degrees)</param>
        /// @warning The phase center variation Triple is in UEN system.
        /// <returns></returns>
        public double GetPcvValue(RinexSatFrequency freq, double elevation)
        {
            //the angle should be measured respect to zenith
            double zenith = 90.0 - elevation;

           if(!CheckZenith(zenith)) { return 0; }          

            if (Data.ContainsKey(freq))
            {
                // Return result. Only the "Up" component is important.
                double up = LinearInterpol(Data[freq].NonAzimuthData, zenith); 
                return up;
            }
            else if (!ShowedNoPcvFreqs.Contains(freq))//只显示一次避免反复显示。
            {
                ShowedNoPcvFreqs.Add(freq);
                log.Warn(this.ToString() + ", " + freq.SatelliteType + " 的 " + freq.RinexCarrierNumber + " 频率的天线数据获取失败！");
            }
            return 0; 
        }

        /// <summary>
        /// 获取某方向到天线平均相位中心的改正量PCV。如果没有找到具有方位角的数据，则从不带方位角的数据中计算。
        /// 如果都没有找到，则返回null.
        /// Get antenna phase center variation.
        /// This method returns a Triple, in UEN system, with the elevation and azimuth-dependent phase center variation.
        /// </summary>
        /// <param name="freq">频率。Frequency</param>
        /// <param name="elevation">高度角，单位：度。Elevation (degrees)</param>
        /// <param name="azimuth">方位角，单位：度。Azimuth (degrees)</param>
        /// @warning The phase center variation Triple is in UEN system.
        /// <returns></returns>
        public double GetPcvValue(RinexSatFrequency freq, double elevation, double azimuth)
        {
            //需要将高度角转换为天顶角。the angle should be measured respect to zenith
            double zenith = 90.0 - elevation;

            if (!CheckZenith(zenith)) { return 0; }

            //将方位角归算到0-360之间
            azimuth = RecudeToRange(azimuth, 0, 360);

            //Look for this frequency in pcDic
            if (Data.ContainsKey(freq))
            {
                Dictionary<double, List<double>> aziData = Data[freq].AzimuthDependentData;
                //获取方位角所在的区间
                double lowerAzimuth = Math.Floor(azimuth / DeltaAzimuth) * DeltaAzimuth;
                double upperAzimuth = lowerAzimuth + DeltaAzimuth;
                // Look for satData vectors，
                bool isContainLower = aziData.ContainsKey(lowerAzimuth);
                bool isContainUpper = aziData.ContainsKey(upperAzimuth);
                // 距离最小刻度的小数部分,且归算到编号角度
                double fractionalAzimuth = (azimuth - lowerAzimuth) / (upperAzimuth - lowerAzimuth);

                if (fractionalAzimuth == 0.0)//刚好在数刻度上，具有现成的数据。
                {
                    if (isContainLower)// Check if there is satData for 'lowerAzimuth'
                    { 
                        double up = LinearInterpol(aziData[lowerAzimuth], zenith); 
                        return up;
                    }
                    log.Warn("没有发现该方位角的数据：" + lowerAzimuth);
                    return 0;
                }
                else if (isContainLower && isContainUpper) // 插值计算。 We have to interpolate
                { 
                    //Find values corresponding to both azimuths
                    double val1 = LinearInterpol(aziData[lowerAzimuth], zenith);
                    double val2 = LinearInterpol(aziData[upperAzimuth], zenith);
                    //Return result
                    double up = val1 + (val2 - val1) * fractionalAzimuth;
                    //var check = Geo.Utils.DoubleUtil.Interpolate(zenithAngle, lowerAzimuth, upperAzimuth, val1, val2); 
                    return up;
                }
            }
   
            return GetPcvValue(freq, elevation);
        }
        #endregion

        #region 常见方法 
        List<RinexSatFrequency> ShowedNoPcoFreqs = new List<RinexSatFrequency>();
        List<RinexSatFrequency> ShowedNoPcvFreqs = new List<RinexSatFrequency>();
        /// <summary>
        /// 如果失败则返回 NEU 0.
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        public NEU GetPcoValue(RinexSatFrequency freq)
        {
            if (Data.ContainsKey(freq))
            {
                return Data[freq].PhaseCenterEccentricities;
            }
            else if(!ShowedNoPcoFreqs.Contains(freq))//只显示一次避免反复显示。
            {
                ShowedNoPcoFreqs.Add(freq);
                log.Warn(this.ToString() + ", " + freq + ", 没有获取到！");
            }
            return NEU.Zero;// new NEU();
        }

        /// <summary>
        /// 添加一个注释。
        /// Add antenna comments.
        /// </summary>
        /// <param name="comments"> Antenna comments line</param>
        /// <returns></returns>
        public void AddComment(string comments)  { Comments.Add(comments); }

        /// <summary>
        /// 添加或设置天线的相位中心偏差。单位：米。
        /// Add antenna phase center ecccentricities, in METERS.
        /// </summary>
        /// <param name="freq"> Frequency.</param>
        /// <param name="trEcc">Eccentricity Triple, in METERS.</param>
        /// <returns></returns>
        public void SetAntennaEcc(RinexSatFrequency freq, NEU trEcc)
        {
            if (!Data.ContainsKey(freq)) Data[freq] = new FrequencecyAntennaData();
            Data[freq].PhaseCenterEccentricities = trEcc;
        }

        /// <summary>
        /// 添加天线偏心差 PCO RMS。单位：米。
        /// Add antenna phase center RMS eccentricities, in METERS.
        /// </summary>
        /// <param name="freq">频率。Frequency.</param>
        /// <param name="ecc">NEU方向的均方根。North eccentricity RMS component, in METERS.</param> 
        /// <returns></returns>
        public void SetAntennaRmsEcc(RinexSatFrequency freq, NEU ecc)
        {                     
            Data[freq].PhaseCenterEccentricitiesRms = ecc;  // Get Triple into eccentricities map
        }

        /// <summary>
        /// 添加不依赖方位角的天线偏心差，单位：米。
        /// Add antenna non-azimuth dependent pattern, in METERS.
        /// </summary>
        /// <param name="freq">Frequency</param>
        /// <param name="pcVec">  Vector of phase centers, in METERS.</param>
        /// <returns></returns>
        public void SetAntennaNoAziPattern(RinexSatFrequency freq, List<double> pcVec)
        {
            Data[freq].NonAzimuthData = pcVec;
        }

        /// <summary>
        ///添加依赖方位角的天线偏心差，单位：米。
        /// Add antenna azimuth dependent pattern, in METERS.
        /// </summary>
        /// <param name="freq">Frequency</param>
        /// <param name="azi">Azimuth</param>
        /// <param name="pcVec">Vector of phase centers, in METERS.</param>
        /// <returns></returns>
        public void SetAntennaPattern(RinexSatFrequency freq, double azi, List<double> pcVec)
        {
             Data[freq].AzimuthDependentData.Add(azi, pcVec);
        }

        /// <summary>
        /// 添加不依赖方位角的天线偏心中误差，单位：米。
        /// Add antenna non-azimuth dependent RMS, in METERS.
        /// </summary>
        /// <param name="freq">Frequency</param>
        /// <param name="pcRMS"> Vector of phase centers RMS, in METERS.</param>
        /// <returns></returns>
        public void SetAntennaNoAziRms(RinexSatFrequency freq, List<double> pcRMS)
        {
            Data[freq].NonAzimuthDataRms = pcRMS;
        }

        /// <summary>
        /// 添加依赖方位角的天线偏心中误差，单位：米。
        /// Add antenna azimuth dependent RMS, in METERS.
        /// </summary>
        /// <param name="freq">Frequency</param>
        /// <param name="azimuth">Azimuth</param>
        /// <param name="pcRmsVector">Vector of phase centers RMS, in METERS.</param>
        /// <returns></returns>
        public void SetAntennaPatternRms(RinexSatFrequency freq, double azimuth, List<double> pcRmsVector)
        {
            Data[freq].AzimuthDependentDataRms[azimuth] = pcRmsVector;
        }

        #endregion

        #region override
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Type:" + AntennaType);
            sb.Append(",Serial:" + SerialOrSatFreqCode);
            sb.Append(",Radome:" + Radome);
    //        sb.Append(",SinexCode:" + SinexCode);
            return sb.ToString();
        }
        /// <summary>
        /// 详细信息
        /// </summary>
        /// <returns></returns>
        public string ToDetailString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SinexCode:" + SinexCode);
            sb.Append(",satellite obsCodeode:" + SatCode);
            sb.Append(",CosparID:" + CosparID);
            sb.Append(",Agency:" + Agency);
            sb.Append(",NumAntennas:" + NumOfAntennas);
            sb.Append(",Date:" + Date);
            sb.Append(",AntennaType:" + AntennaType);
            sb.Append(",AntennaRadome:" + Radome);
            sb.Append(",AntennaSerial:" + SerialOrSatFreqCode);
            sb.Append(",CalibrationMethod:" + CalibrationMethod);
            sb.Append(",DeltaAzimuth:" + DeltaAzimuth);
            sb.Append(",Zen1:" + ZenithStart);
            sb.Append(",Zen2:" + ZenithEnd);
            sb.Append(",Dzen:" + DeltaZenith);
            sb.Append(",NumOfFrequencies:" + NumOfFrequencies);
            sb.Append(",ValidDateFrom:" + ValidDateFrom);
            sb.Append(",ValidDateUntil:" + ValidDateUntil);
            sb.Append(",AntennaEccDicSize:" + DataCount);
            sb.Append(",IsValid:" + IsValid);
            return sb.ToString();
        }
        #endregion

        #endregion

        #region 静态工具方法和算法细节

        /// <summary>
        /// 检查天顶距离大小范围
        /// </summary>
        /// <param name="zenith">单位：度</param>
        /// <returns></returns>
        private bool CheckZenith(double zenith)
        {
            if ((zenith < ZenithStart) || (zenith > ZenithEnd))
            {
                log.Warn("天线高度角超过允许的范围(" + ZenithStart + ", " + ZenithEnd + "), " + zenith + ", 取消天线改正！");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 将数据归算到一个范围内。
        /// </summary>
        /// <param name="val">待归算数据</param>
        /// <param name="minInclude">最小值（含）</param>
        /// <param name="maxExcludeAndCycle">最大值（不含），周期</param>
        /// <returns></returns>
        private static double RecudeToRange(double val, double minInclude, double maxExcludeAndCycle)
        {
            while (val < minInclude) { val += maxExcludeAndCycle; }
            while (val >= maxExcludeAndCycle) { val -= maxExcludeAndCycle; }
            return val;
        }

        /// <summary>
        /// 线性插值。
        /// </summary>
        /// <param name="dataList"> 按照间隔存放的数据。</param>
        /// <param name="zenithAngle">天顶距离，单位:度</param> 
        /// <returns></returns>
        private double LinearInterpol(List<double> dataList, double zenithAngle)
        {
            //Get the normalized angle'normalizedAngle' is a value corresponding to the original angle divided by the angle interval.
            double normalizedAngle = ((zenithAngle - ZenithStart) / DeltaZenith);
            //计算整数部分
            int index = Convert.ToInt32(Math.Floor(normalizedAngle));// Get the index value 'normalizedAngle' is equivalent to

            int i = (int)normalizedAngle;
            if (i != index)
            {
                index = i;
                if (i < 0) return dataList[0];
                if (i >= 18) return dataList[18];
            }

            double fraction = normalizedAngle - index;// Find the fraction from 'index'           
            if (fraction == 0.0) return dataList[index]; // Check if 'normalizedAngle' is exactly a value in the map
            else
            {
                // In this case, we have to interpolate
                double val1 = dataList[index];
                double val2 = dataList[index + 1];
                // Return result
                var result = (val1 + (val2 - val1) * fraction);
                //var check = Geo.Utils.DoubleUtil.Interpolate(normalizedAngle, index, index + 1, val1, val2);
                return result;
            }
        }
        #endregion
    }
}
