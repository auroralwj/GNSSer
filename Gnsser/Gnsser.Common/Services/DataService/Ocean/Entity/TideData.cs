//2014.05.22, Cui Yang, created

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Algorithm;
using Geo.Utils;

namespace Gnsser
{
    public class TideData
    {
        public TideData() { this.HarmonicsOfTideData = new ArrayMatrix(6, 11, 0); }
        public ArrayMatrix HarmonicsOfTideData { get; set; }
    }
}