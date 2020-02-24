//2018.11.09, czs, create in HMX, 一条基线的平差结果
//2018.11.10, czs, create in hmx, 增加基线组合类
//2018.11.30, czs, create in hmx, 实现IToTabRow接口，用于规范输出,合并定义新的 BaseLineNet

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using System.Linq;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;
using Geo;
using AnyInfo.Graphs.Structure;
using Geo.Times;
using AnyInfo.Graphs;

namespace Gnsser
{ 
    /// <summary>
    /// 一个测站多个坐标
    /// </summary>
    public class SiteCoordsManager:BaseDictionary<String, SiteCoords>
    {
        public SiteCoordsManager()
        {

        }

        public override SiteCoords Create(string key)
        {
            return new SiteCoords() { Name = key };
        }

    }
    /// <summary>
    /// 测站坐标一个测站多个坐标
    /// </summary>
    public class SiteCoords
    {
        /// <summary>
        /// 测站坐标
        /// </summary>
        public SiteCoords()
        {
            this.Coords = new List<XYZ>();
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 坐标序列
        /// </summary>
        public List<XYZ> Coords { get; set; }
        /// <summary>
        /// 第一坐标
        /// </summary>
        public XYZ Coord { get => Coords[0]; }

        /// <summary>
        /// 添加一个坐标
        /// </summary>
        /// <param name="xyz"></param>
        public void Add(XYZ xyz) { this.Coords.Add(xyz); }


        /// <summary>
        /// 坐标是否相同
        /// </summary>
        /// <returns></returns>
        public bool IsCoordSame()
        {
            var baseXyz = Coords[0];
            double tolerance = 1e-6;
            foreach (var item in Coords)
            {
                if (!baseXyz.Equals(item, tolerance))
                {
                    
                    return false;
                }
            }
            return true;
        }
    }
}