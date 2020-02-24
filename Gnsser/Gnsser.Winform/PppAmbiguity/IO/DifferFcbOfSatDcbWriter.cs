
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
    public class DifferFcbOfSatDcbWriter : LineFileWriter<DifferFcbOfSatDcbItem>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public DifferFcbOfSatDcbWriter(string gofFilePath, FileMode FileMode = FileMode.Create)
            : base(gofFilePath, "", Encoding.ASCII, FileMode)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public DifferFcbOfSatDcbWriter(string gofFilePath, string metaFilePath, FileMode FileMode = FileMode.Create)
            : base(gofFilePath, metaFilePath, Encoding.ASCII, FileMode)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="Gmetadata"></param>
        public DifferFcbOfSatDcbWriter(string gofFilePath, Gmetadata Gmetadata, FileMode FileMode = FileMode.Create)
            : base(gofFilePath, Gmetadata, Encoding.ASCII, FileMode)
        { 
        }
        /// <summary>
        /// 构造函数，以数据流初始化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="Gmetadata"></param>
        public DifferFcbOfSatDcbWriter(Stream stream, Gmetadata Gmetadata)
            : base(stream, Gmetadata)
        { 
        }

        public override string EntityToLine(DifferFcbOfSatDcbItem obj)
        {
            return obj.ToLineString();// base.EntityToLine(obj);
        }
          
    }

}