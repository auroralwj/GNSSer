//2014.10.25, czs, create in namu shuangliao, 具有改正数的坐标

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;
using Geo.Correction;
using Gnsser.Times;
using Geo.Coordinates; 
using Geo;

namespace Gnsser.Correction
{

    /// <summary>
    /// 具有改正数的坐标。
    /// </summary> 
    public class CorrectableXYZ : AbstractDetailedCorrectable<XYZ, XYZ>, IDetailedCorrectable<XYZ, XYZ>
    {
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="val">改正值</param> 
        public CorrectableXYZ(XYZ val):base(val)
        { 
        } 

        public override XYZ TotalCorrection
        {
            get
            {
                XYZ xyz = XYZ.Zero;
                foreach (var item in this.Corrections)
                {
                    xyz += item.Value;
                }
                return xyz;
            }
        }

        public override XYZ CorrectedValue { get { return Value + Correction; } }
    } 
}