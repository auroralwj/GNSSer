//2014.09.17, czs, create, 测试

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Domain;

namespace Gnsser.Correction
{
    /// <summary>
    ///固定模糊度的改正。假定给定的测站坐标非常精确，采用此坐标作为真值来固定模糊度。
    /// </summary>
    public class AmbiguityFixedCorrector : AbstractRangeCorrector
    {
        /// <summary>
        /// 构造函数.固定模糊度的改正
        /// </summary> 
        public AmbiguityFixedCorrector()
        {
            this.Name = "固定模糊度的改正";
        }

        public override void Correct(EpochSatellite epochSatellite)
        {
            XYZ receiverXyz = epochSatellite.SiteInfo.EstimatedXyz;
            XYZ satXyz = epochSatellite.Ephemeris.XYZ;

            XYZ ray = satXyz - receiverXyz;


            double correction = ray.Radius();
            this.Correction = ( correction);
            
        } 
    }
}
