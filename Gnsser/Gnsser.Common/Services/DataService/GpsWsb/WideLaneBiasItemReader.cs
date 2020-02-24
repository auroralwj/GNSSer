
//2016.08.21, czs, create in fujian yong'an, 法国宽项读取器

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
    /// 法国宽项读取器。 
    /// ftp://ftpsedr.cls.fr/pub/igsac/Wide_lane_GPS_satellite_biais.wsb
    /// </summary>
    public class WideLaneBiasItemReader : LineFileReader<WideLaneBiasItem>
    { 
          /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public WideLaneBiasItemReader(string gofFilePath, string metaFilePath = null) : base(gofFilePath, metaFilePath)
        {
            ItemSpliters = new string[] { " " };
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="Gmetadata"></param>
        public WideLaneBiasItemReader(string gofFilePath, Gmetadata Gmetadata)
            : base(gofFilePath, Gmetadata)
        {
            ItemSpliters = new string[] { " " };
        }

        public override WideLaneBiasItem Parse(string[] items)
        {
            WideLaneBiasItem item = new WideLaneBiasItem();
            int i =0;
            Time time = new Time(int.Parse( items [i++]), int.Parse( items [i++]),int.Parse( items [i++]), int.Parse( items [i++]));
            item.Time = time;
            for (int j = 4; j < items.Length; j++)
            {
                var prn = new SatelliteNumber(j - 3, SatelliteType.G);
                var val = Double.Parse(items[i++]);

                if (val == 0) {
                    val = Double.NaN;
                }

                item.Data.Add(prn,val);
            }
            return item;
        } 
    }

}