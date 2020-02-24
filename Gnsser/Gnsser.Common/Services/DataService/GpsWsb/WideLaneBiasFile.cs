//2018.09.06, czs, create in hmx, 文件，存储所有，快速响应

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust; 
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using Gnsser;
using Gnsser.Checkers;
using Geo.Referencing;
using Geo.Utils; 

namespace Gnsser
{
    /// <summary>
    /// 文件，存储所有，快速响应。 
    /// </summary>
    public class WideLaneBiasFile  : BaseDictionary<Time, WideLaneBiasItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WideLaneBiasFile()
        { 
        }  



    }
}