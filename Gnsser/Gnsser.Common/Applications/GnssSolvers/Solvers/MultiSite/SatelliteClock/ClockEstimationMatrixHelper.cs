//2016.10.21, double, edit in hongqing, 修改矩阵

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Geo.Algorithm;

namespace Gnsser
{ 
    /// <summary>
    /// 钟差估计辅助类
    /// </summary>
    public class ClockEstimationMatrixHelper
    {
        /// <summary>
        /// 构建一个首次方差参数顺序： dtr trop dts N
        /// </summary>
        /// <param name="ParamCount"></param>
        /// <param name="SiteCount"></param>
        /// <param name="SatCount"></param>
        /// <returns></returns>
        public static WeightedVector GetInitAprioriParam(int ParamCount, int SiteCount, int SatCount)
        {

            DiagonalMatrix initCova = new DiagonalMatrix(ParamCount);
            //Fill the initialErrorCovariance matrix

            //First, the receiver clock 
            for (int i = 0; i < SiteCount; i++)
                initCova[i] = 9.0e10;    //(300 km) ^*2
            //Second, the zenital wet tropospheric delay
            for (int i = SiteCount; i < 2 * SiteCount; i++)
                initCova[i] = 0.25;    //(0.5 m) ^*2

            //Third, the satellite clock
            for (int i = 2 * SiteCount; i < 2 * SiteCount + SatCount; i++)
                initCova[i] = 9.0e10;    //(300 km) ^*2

            //Finally, the phase biases
            for (int i = 2 * SiteCount + SatCount; i < ParamCount; i++)
            {
                initCova[i] = 4.0e14;   //(20000 km) ^*2
            }
            return new WeightedVector(new Vector(ParamCount), initCova);
        }

        /// <summary>
        /// 无电离层历元差分钟差估计先验信息  dtr trop dts
        /// </summary>
        /// <param name="ParamCount"></param>
        /// <param name="SiteCount"></param>
        /// <param name="SatCount"></param>
        /// <returns></returns>
        public static WeightedVector GetInitEpochDifferAprioriParam(int ParamCount, int SiteCount, int SatCount)
        {

            DiagonalMatrix initCova = new DiagonalMatrix(ParamCount);

            //Fill the initialErrorCovariance matrix

            //First, the receiver clock 
            for (int i = 0; i < SiteCount; i++)
                initCova[i] = 9.0e10 * 2;    //(300 km) ^*2
            //Second, the zenital wet tropospheric delay
            for (int i = SiteCount; i < 2 * SiteCount; i++)
                initCova[i] = 0.25 * 2;    //(0.5 m) ^*2

            //Third, the satellite clock
            for (int i = 2 * SiteCount; i < ParamCount; i++)
                initCova[i] = 9.0e10 * 2;    //(300 km) ^*2

            return new WeightedVector(new Vector(ParamCount), initCova);
        }
         
    }

}

