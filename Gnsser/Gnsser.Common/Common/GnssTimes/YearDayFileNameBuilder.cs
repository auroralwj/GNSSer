
//2016.08.12, czs, create in fujian yong'an, 基于年和年积日的路径生成器

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
    /// 基于年和年积日的路径生成器。
    /// </summary>
    public class YearDayFileNameBuilder : AbstractBuilder<string>
    {
        public YearDayFileNameBuilder(string extension, string directory)
        {
            this.Extension = extension;
            this.Directory = directory;
        }
        /// <summary>
        /// 目录
        /// </summary>
        public string Directory { get; set; }
        /// <summary>
        /// 后缀名
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public Time Time { get; set; }

        public override string Build()
        {
            string fileName = Time.ToYearDayString();
            return Path.Combine(Directory, fileName + "." + Extension);
        }
    }

}