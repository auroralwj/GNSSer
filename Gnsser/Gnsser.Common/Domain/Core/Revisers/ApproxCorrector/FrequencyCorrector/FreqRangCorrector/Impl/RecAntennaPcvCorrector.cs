//2014.05.22, Cui Yang, created
//2014.06.30, Cui Yang, 极潮改正的参数更新为IERS2010
//2014.08.18, czs, 将结果采用NEU坐标表示，分别对应北东天方向。
//2014.09.15, cy, 重构
//2014.10.05, czs, edit in hailutu, 将名称改为从卫星天线（SatAntenna）改为接收机天线（RecAntenna）
//2018.08.02, czs, edit in hmx, 支持天线所有指定的频率赋值，不局限于G01,G02
//2018.09.27, czs, edit in hmx, 去掉了RINEX编号转换统一到 ObsCodeConvert 中。


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Domain;
using Geo.IO;
using Gnsser.Data;


namespace Gnsser.Correction
{
    /// <summary>
    /// 天线相位中心变化改正，接收机天线对具体频率，具体卫星的观测值的改正。
    /// PCV Phase Center Vari
    /// </summary>
    public class RecAntennaPcvCorrector : AbstractFreqBasedRangeCorrector
    {
        Log log = new Log(typeof(RecAntennaPcvCorrector)); 
        /// <summary>
        /// 各个频率对接收机天线相位中心的改正 PCV
        /// antenna excentricity，difference in phase centers
        /// </summary>
        public RecAntennaPcvCorrector()
        {
            this.Name = "接收机端PCV距离改正";
            this.CorrectionType = CorrectionType.RecAntennaPcv;
            this.IsUseAzimuthAntenna = true;
            WarnedSites = new List<string>();
        }

        /// <summary>
        /// 是否采用方位角的天线
        ///是否采用方位角，如果没有找到，自动找没有方位角的 
        /// </summary>
        public bool IsUseAzimuthAntenna { get; set; }
        /// <summary>
        /// 不重复警告
        /// </summary>
        List<string> WarnedSites { get; set; }

        /// <summary>
        /// 改正
        /// </summary>
        /// <param name="input"></param>
        public override void Correct(EpochSatellite input)
        {
            var correction = new Dictionary<RinexSatFrequency, double>();
            this.Correction = correction;

            IAntenna antenna = input.SiteInfo.Antenna;
            if (antenna == null)
            {
                if (!WarnedSites.Contains(input.SiteInfo.SiteName))
                {
                    WarnedSites.Add(input.SiteInfo.SiteName);
                    log.Warn(input.Name + "接收机天线为: " + input.SiteInfo.AntennaType + ", " + input.SiteInfo.AntennaNumber  + " 没有在天线文件夹中找到该类型的天线，无法进行 PCV 改正，精度影响可达10厘米（特别是高程）！请到 https://www.ngs.noaa.gov/ANTCAL/ 下载对应天线改正信息，并追加到.atx文件中");
                }
                return;
            }
            List<RinexSatFrequency> frequences = input.RinexSatFrequences;

            XYZ rcvPos = input.SiteInfo.EstimatedXyz;
            if (rcvPos.Equals(XYZ.Zero))//测站未获取近似值，不改正
            {
                log.Warn(input.ReceiverTime + ", " + input.Name + ", 测站未获取近似值，不改正PCV");
                this.Correction = correction;
                return;
            }

                var elev = input.GeoElevation;
                var azim = input.Polar.Azimuth;

            foreach (var item in antenna.Data)
            {
                var satFreq = item.Key;
                double pcv = 0;
                if (IsUseAzimuthAntenna)
                {
                    pcv = antenna.GetPcvValue(satFreq, elev, azim); 
                }
                else
                {
                    pcv = antenna.GetPcvValue(satFreq, elev);
                    ///**PCV 是站星距离，不是ENU, 不用再次改正 */
                    //不信，可以试试：转换成站星方向改正等效距离算法如下：
                    //double rangeCorretion = CoordUtil.GetDirectionLength(pcv, input.Polar);
                    ////approx, 用以验证
                    //double test = pcv.U * Math.Sin(input.GeoElevation * Geo.CoordConsts.DegToRadMultiplier);
                    //double differ = rangeCorretion - test; 
                }

                correction.Add(satFreq, pcv);
            }

            //多频的权宜之计
            if (input.EpochInfo.SatelliteTypes.Count > 0)
            {
                correction[RinexSatFrequency.BdsA] = antenna.GetPcvValue(RinexSatFrequency.GpsA, elev);  
                correction[RinexSatFrequency.BdsB] = antenna.GetPcvValue(RinexSatFrequency.GpsB, elev);
                correction[RinexSatFrequency.GalileoA] = correction[RinexSatFrequency.BdsA];
                correction[RinexSatFrequency.GalileoB] = correction[RinexSatFrequency.BdsB];
            }
            this.Correction = correction;
        }

    }
}
