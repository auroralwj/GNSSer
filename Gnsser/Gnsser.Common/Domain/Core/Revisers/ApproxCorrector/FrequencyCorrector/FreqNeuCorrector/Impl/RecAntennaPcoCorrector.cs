//2014.05.22, Cui Yang, created
//2014.06.30, Cui Yang, 极潮改正的参数更新为IERS2010
//2014.08.18, czs, 将结果采用NEU坐标表示，分别对应北东天方向。
//2014.09.15, cy, 重构
//2014.10.05, czs, edit in hailutu, 将名称改为从卫星天线（SatAntenna）改为接收机天线（RecAntenna）
//2018.08.02, czs, edit in hmx, 支持天线所有指定的频率赋值，不局限于G01,G02

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Geo.IO;
using Gnsser;
using Gnsser.Domain;
using Gnsser.Data;


namespace Gnsser.Correction
{
    //接收机天线对频率观测值的改正误差改正
    /// <summary>
    /// 接收机天线平均相位中心改正(PCO->ARP)，改正数为 NEU
    /// 平均相位中心到天线几何（测量）参考点ARP。
    /// 平均相位中心认为是不变的，因此此项只需改正到测站HEN即可。
    /// 这个改正适用于所有的同一频率的卫星，因此归类到接收机频率改正,与具体的卫星无关。
    /// </summary>
    public class RecAntennaPcoCorrector : AbstractFreqBasedNeuCorrector
    {
        Log log = new Log(typeof(RecAntennaPcoCorrector));
        /// <summary>
        /// 卫星天线对频率观测值的改正误差改正，改正数为 NEU
        /// antenna excentricity，difference in phase centers
        /// </summary>
        public RecAntennaPcoCorrector()
        {
            this.Name = "测站天线平均相位中心改正(PCO->ARP)NEU";
            this.CorrectionType = CorrectionType.RecAntennaPco;
            WarnedSites = new List<string>();
        }

        /// <summary>
        /// 不重复警告
        /// </summary>
        List<string> WarnedSites { get; set; }

        /// <summary>
        /// 计算改正数
        /// </summary>
        /// <param name="input"></param>
        public override void Correct(EpochInformation input)
        {             
            if (input.EnabledSatCount == 0) { return; }
            var correction = new Dictionary<RinexSatFrequency, NEU>();

            IAntenna antenna = input.SiteInfo.Antenna;
            if (antenna == null)
            {
                if (!WarnedSites.Contains(input.SiteInfo.SiteName))
                {
                    WarnedSites.Add(input.SiteInfo.SiteName);
                    log.Warn(input.Name + "接收机天线为: " + input.SiteInfo.AntennaType + ", " + input.SiteInfo.AntennaNumber + " 没有在天线文件夹中找到该类型的天线，无法进行 PCV 改正，精度影响可达10厘米（特别是高程）！请到 https://www.ngs.noaa.gov/ANTCAL/ 下载对应天线改正信息，并追加到.atx文件中");
                }
                this.Correction = correction;
                return;
            }
           
            foreach (var item in antenna.Data)
            {
                var satFreq = item.Key;
                var pco = antenna.GetPcoValue(satFreq); //这个值不变，实际上一次赋值就可以了！！//2018.08.02， czs，in hmx
                correction.Add(satFreq, pco);
            }
            //多频的权宜之计
            if(input.SatelliteTypes.Count > 0)
            {
                correction[RinexSatFrequency.BdsA] = correction[RinexSatFrequency.GpsA];
                correction[RinexSatFrequency.BdsB] = correction[RinexSatFrequency.GpsB];
                correction[RinexSatFrequency.GalileoA] = correction[RinexSatFrequency.GpsA];
                correction[RinexSatFrequency.GalileoB] = correction[RinexSatFrequency.GpsB];
            }

            this.Correction = correction;
        }
         
    }
}
