using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 坐标创建工厂默认实例。
    /// </summary>
    public class CoordinateFactory :  ICoordinateFactory
    {    
        /// <summary>
        /// 实例化一个坐标工厂。
        /// </summary>
        /// <param name="reference">坐标参考系</param>
        public CoordinateFactory(ICoordinateReferenceSystem reference)
        {
            this.ReferenceSystem = reference;
        }

        /// <summary>
        /// 坐标参考系统
        /// </summary>
        public ICoordinateReferenceSystem ReferenceSystem { get; set; }
        /// <summary>
        /// 创建一个一维坐标。如高程。
        /// </summary>
        /// <param name="value">坐标轴的值</param>
        /// <param name="weight">权值</param>
        /// <returns></returns>
        public ICoordinate Create1D(Double value, Double weight = 0)
        {
            if (ReferenceSystem.CoordinateSystem.Dimension != 1) throw new ApplicationException("坐标参考系统不是 1 维的");

            Coordinate coord = new Coordinate(ReferenceSystem);
            coord[0] = value;
            coord.Weight = weight;
            return coord;
        }
        /// <summary>
        /// 创建一个一维坐标。如高程。
        /// </summary>
        /// <param name="coordinates">数组，第一个为坐标值，第二个为权值</param>
        /// <returns>一个一维坐标实例</returns>
        public ICoordinate Create1D(params Double[] coordinates)
        {
            if (coordinates.Length < 1) throw new ArgumentException("数组维数不能小于 1", "coordinates");
            double weight = 0;
            if (coordinates.Length > 1) weight = coordinates[1];
 
            return  Create1D(coordinates[0], weight);
        }
        /// <summary>
        /// 创建一个二维坐标。
        /// 数值顺序按照坐标系统定义的坐标轴的顺序赋值。一般为 x、y、z 或 lon、 lat、height。
        /// </summary>
        /// <param name="valOfAxis1">第一个坐标轴的值</param>
        /// <param name="valOfAxis2">第二个坐标轴的值</param>
        /// <param name="weight">权值</param>
        /// <returns>一个二维坐标实例</returns>
        public ICoordinate Create2D(Double valOfAxis1, Double valOfAxis2, Double weight = 0)
        {
            if (ReferenceSystem.CoordinateSystem.Dimension != 2) throw new ApplicationException("坐标参考系统不是 2 维的");

            Coordinate coord = new Coordinate(ReferenceSystem);
            coord[0] = valOfAxis1;
            coord[1] = valOfAxis2;
            coord.Weight = weight;
            return coord;

        }
        /// <summary>
        /// 创建一个二维坐标。
        /// 数值顺序按照坐标系统定义的坐标轴的顺序赋值。一般为 x、y、z 或 lon、 lat、height。
        /// </summary>
        /// <param name="coordinates">数组，第1、2个为坐标值，第3个为权值</param>
        /// <returns></returns>
        public ICoordinate Create2D(params Double[] coordinates)
        {
            if (coordinates.Length < 2) throw new ArgumentException("数组维数不能小于 2", "coordinates");
            double weight = 0;
            if (coordinates.Length > 2) weight = coordinates[2];

            return Create2D(coordinates[0], coordinates[1], weight);
        }
        /// <summary>
        /// 创建一个3维坐标。
        /// 数值顺序按照坐标系统定义的坐标轴的顺序赋值。一般为 x、y、z 或 lon、 lat、height。
        /// </summary>
        /// <param name="valOfAxis1">第1个坐标轴的值</param>
        /// <param name="valOfAxis2">第2个坐标轴的值</param>
        /// <param name="valOfAxis3">第3个坐标轴的值</param>
        /// <param name="weight">权值</param>
        /// <returns></returns>
        public ICoordinate Create3D(Double valOfAxis1, Double valOfAxis2, Double valOfAxis3, Double weight = 0)
        {
            if (ReferenceSystem.CoordinateSystem.Dimension != 3) throw new ApplicationException("坐标参考系统不是 3 维的");

            Coordinate coord = new Coordinate(ReferenceSystem);
            coord[0] = valOfAxis1;
            coord[1] = valOfAxis2;
            coord[2] = valOfAxis3;
            coord.Weight = weight;
            return coord;
        }
        /// <summary>
        /// 创建一个3维坐标。
        /// 数值顺序按照坐标系统定义的坐标轴的顺序赋值。一般为 x、y、z 或 lon、 lat、height。
        /// </summary>
        /// <param name="coordinates">数组，第1、2、3个为坐标值，第4个为权值</param>
        /// <returns></returns>
        public ICoordinate Create3D(params Double[] coordinates)
        {
            if (coordinates.Length < 3) throw new ArgumentException("数组维数不能小于 3", "coordinates");
            double weight = 0;
            if (coordinates.Length > 3) weight = coordinates[3];

            return Create3D(coordinates[0], coordinates[1], coordinates[2], weight);
        }

        /// <summary>
        /// 创建一个大地坐标
        /// </summary>
        /// <param name="lon">经度</param>
        /// <param name="lat">维度</param>
        /// <param name="height">高程</param>
        /// <param name="weight">权值</param>
        /// <returns></returns>
        public IGeodeticCoord CreateGeodeticCoord(double lon, double lat, double height, double weight = 0)
        {
            return new GeodeticCoord(ReferenceSystem, lon, lat, height, weight);
        }
        /// <summary>
        /// 创建一个经纬度坐标。
        /// </summary>
        /// <param name="lon">经度</param>
        /// <param name="lat">维度</param> 
        /// <param name="weight">权值</param>
        /// <returns></returns>
        public ILonLatCoord CreateLonLatCoord(double lon, double lat, double weight = 0)
        {
            return new LonLatCoord(ReferenceSystem, lon, lat, weight);
        }
        /// <summary>
        /// 创建XY坐标
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="weight">权值</param>
        /// <returns></returns>
        public IXyCoord CreateXyCoord(double x, double y, double weight = 0)
        {
            return new XyCoord(ReferenceSystem, x, y, weight);
        }
        /// <summary>
        /// 创建一个XYZ分量的坐标。
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="weight">权值</param>
        /// <param name="z">Z</param> 
        /// <returns></returns>
        public IXyzCoord CreateXyzCoord(double x, double y, double z, double weight = 0)
        {
            return new XyzCoord(ReferenceSystem, x, y,z, weight);
        }
    }
}
