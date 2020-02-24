//2019.03.09, czs, create in hongqing, WeightedXyz

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Algorithm;

namespace Geo.Coordinates
{
    /// <summary>
    /// º”»® XYZ
    /// </summary>
    public class CovaedXyz : CovaedValue<XYZ, Matrix>
    {
        public CovaedXyz()
        {

        }

        public CovaedXyz(XYZ Value, Matrix Cova) : base(Value, Cova)
        {
        }
    }
}