//2014.08.19, czs, create, 单独的接收机天线改正，标石到天线参考点（ARP）的矢量

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Domain;


namespace Gnsser.Correction
{
    /// <summary>
    /// 接收机天线改正，标石到天线参考点（ARP）的矢量,无载波频率相关
    /// 天线相位中心偏差（PCO，Phase Center Offset）
    /// </summary>
    public class RecAntennaArpCorrector : AbstractEpochNeuReviser
    {

        /// <summary>
        /// 接收机天线改正，标石到天线参考点（ARP）的矢量
        /// </summary>
        public RecAntennaArpCorrector()
        {
            this.Name = "测站对接收机天线HEN改正";
            this.CorrectionType = CorrectionType.RecAntennaArp;
        }

        /// <summary>
        /// 接收机天线改正
        /// </summary>
        /// <param name="epochInformation"></param>
        public override void Correct(EpochInformation epochInformation)
        {
            HEN offsetARPHEN = epochInformation.SiteInfo.Hen;

            if (offsetARPHEN == null)
                return;

            // Vector from monument to ARP([UEN])，标石到天线参考点（ARP）的矢量。
            NEU offsetARP = new NEU(offsetARPHEN.N, offsetARPHEN.E, offsetARPHEN.H);
            NEU correction = offsetARP;

            this.Correction =   correction;
            //this.Correction = NEU.Zero;// correction;
        } 
         
    }
}
