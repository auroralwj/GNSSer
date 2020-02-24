using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Gnsser.Times;
using Geo;
using Geo.Common;
using System.IO;

namespace Gnsser.Data
{
    /// <summary>
    /// 具体数值
    /// </summary>
    public class Vmf1Value
    {

        public Vmf1Value(string stanam, double mjd, double ah, double aw, double hzd, 
            double wzd, double meantmpKe, double pressure, double temCe, double waterPre, double ortHeight)
        {
            this.stanam = stanam;
            this.mjd = mjd;
            this.ah = ah;
            this.aw = aw;
            this.hzd = hzd;
            this.wzd = wzd;
            this.meantmpKe = meantmpKe;
            this.pressure = pressure;
            this.temCe = temCe;
            this.waterPre = waterPre;
            this.ortHeight = ortHeight;
        }
        
        /// <summary>
        /// 测站名称station name (8 characters)
        /// </summary>
        public string stanam { get; set; } 

        /// <summary>
        /// 约化儒略日，一天分为四段 .00 .25 .50和.75 modified Julian date
        /// </summary>
        public double mjd { get; set; }

        /// <summary>
        /// 干分量的系数a hydrostatic coefficient a
        /// </summary>
        public double ah { get; set; }

        /// <summary>
        /// 湿分量的系数a wet coefficient a
        /// </summary>
        public double aw{get; set;}

        /// <summary>
        /// VMF1模型的干分量延迟 hydrostatic zenith delay
        /// </summary>
        public double hzd { get; set; }

        /// <summary>
        /// VMF1模型的湿分量延迟wet zenith delay
        /// </summary>
        public double wzd { get; set; } 

        /// <summary>
        /// 卡尔文平均大气温度mean temperature of the atmosphere above the site in Kelvin
        /// </summary>
        public double meantmpKe { get; set; }

        /// <summary>
        /// 平均大气压强  pressure at the site in hPa
        /// </summary>
        public double pressure { get; set; }

        /// <summary>
        /// 以摄氏度为单位的温度 temperature at the site in Celsius
        /// </summary>
        public double temCe { get; set; }

        /// <summary>
        /// water vapour pressure at the site in hPa
        /// </summary>
        public double waterPre { get; set; }

        /// <summary>
        /// 测站正高orthometric height of the station (using geoid EGM96)
        /// </summary>
        public double ortHeight { get; set; }

    }
}
