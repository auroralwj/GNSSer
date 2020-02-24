//2014.09.24, czs, create, 球谐系数

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
using Geo.Times;
using Geo;
using Geo.IO;

namespace Gnsser.Data
{
    /// <summary>
    /// 球谐系数
    /// </summary>
    public class SphericalHarmonicsService : IService
    {
        Log log = new Log(typeof(SphericalHarmonicsService));

        /// <summary>
        /// 球谐系数
        /// </summary>
        /// <param name="filePath"></param>
        public SphericalHarmonicsService(string filePath) 
        { 
            var reader = new SphericalHarmonicsReader(filePath);
            this.File = reader.Read();
            Name = Path.GetFileName(filePath);
        }

        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，。
        /// </summary>
        /// <param name="filePath"></param>
        public SphericalHarmonicsService(FileOption filePath) 
        {
            var reader = new SphericalHarmonicsReader(filePath);
            this.File =  reader.Read();
            Name = Path.GetFileName(filePath.FilePath);
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public SphericalHarmonicsFile File { get; set; }  


    }
}
