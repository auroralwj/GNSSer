//2017.11.06, czs, added, 球谐系数


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Times;
using Geo.Utils;
using Gnsser;
using Geo;

namespace Gnsser.Data
{
    /// <summary>
    /// 球谐系数 文件
    /// </summary>
    public class SphericalHarmonicsFile : BaseDictionary<int, SphericalHarmonicsItem>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SphericalHarmonicsFile()
        { 
            this.CreateFunc = new Func<int, SphericalHarmonicsItem>(key => new SphericalHarmonicsItem(key + 1));
        }
        /// <summary>
        /// 最大的阶数。
        /// </summary>
        public int MaxDegree { get { return this.Keys.Max(); } }



    }
}