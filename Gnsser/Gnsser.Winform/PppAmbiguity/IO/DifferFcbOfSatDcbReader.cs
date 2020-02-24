
//2016.08.11, czs, create in fujian yong'an, 基于无电离层组合星间单差模糊度的宽窄项

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using Gnsser;
using Geo;
using Gnsser.Data;
using Gnsser.Checkers;
using Geo.Referencing;
using Geo.Utils;
using System.IO;

namespace Gnsser
{
    /// <summary>
    /// 基于无电离层组合星间单差模糊度的宽窄项。 
    /// </summary>
    public class DifferFcbOfSatDcbReader : LineFileReader<DifferFcbOfSatDcbItem>
    { 
          /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public DifferFcbOfSatDcbReader(string gofFilePath, string metaFilePath = null) : base(gofFilePath, metaFilePath)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="Gmetadata"></param>
        public DifferFcbOfSatDcbReader(string gofFilePath, Gmetadata Gmetadata)
            : base(gofFilePath, Gmetadata)
        {

        }
        /// <summary>
        /// 解析行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public override DifferFcbOfSatDcbItem ParseLine(string line)
        {
            return DifferFcbOfSatDcbItem.Parse(line);
        } 
    }

}