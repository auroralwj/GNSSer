// 2014.09.19, czs, create, in hailutu, 数据源统一设计

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates; 
using Gnsser.Service; 
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Geo;

namespace Gnsser.Data
{
    /// <summary>
    /// 海洋潮汐数据源。
    /// </summary>
    public class OceanLoadingHarmonicsService : GnssFileService<Geo.Algorithm.IMatrix>, IService<Geo.Algorithm.IMatrix, string>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="OceanLoadingFile">海洋潮汐文件</param>
        public OceanLoadingHarmonicsService(FileOption OceanLoadingFile)
            : base(OceanLoadingFile)
        {
            blqData = new BLQDataReader(OceanLoadingFile.FilePath);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="OceanLoadingFile">海洋潮汐文件</param>
        public OceanLoadingHarmonicsService(string OceanLoadingFile)
            : base(OceanLoadingFile)
        {
            blqData = new BLQDataReader(OceanLoadingFile);
        }

        BLQDataReader blqData;

        /// <summary>
        /// 返回矩阵
        /// </summary>
        /// <param name="markerName"></param>
        /// <returns></returns>
        public Geo.Algorithm.IMatrix Get(string markerName)
        {
            ArrayMatrix Harmonics = null; 
            if (blqData.OceanTidesData.ContainsKey(markerName))
            {
                //Get harmonics satData from file
                Harmonics = blqData.GetTideHarmonics(markerName);
            }
            else
            {
                Harmonics = new Geo.Algorithm.ArrayMatrix(6, 11, 0.0);
            }
            return Harmonics;
        }
    }
}