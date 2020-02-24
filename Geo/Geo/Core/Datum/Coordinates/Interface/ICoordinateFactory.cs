using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 坐标创建工厂
    /// </summary>
    public interface ICoordinateFactory
    {
        /// <summary>
        /// 坐标参考系统
        /// </summary>
        ICoordinateReferenceSystem ReferenceSystem { get; set; }   
       
        /// <summary>
        /// 创建一个一维坐标。如高程。
        /// </summary>
        /// <param name="value">坐标轴的值</param>
        /// <param name="weight">权值</param>
        /// <returns></returns>
        ICoordinate Create1D(Double value, Double weight = 0);
        /// <summary>
        /// 创建一个一维坐标。如高程。
        /// </summary>
        /// <param name="coordinates">数组，第一个为坐标值，第二个为权值</param>
        /// <returns>一个一维坐标实例</returns>
        ICoordinate Create1D(params Double[] coordinates);  
        /// <summary>
        /// 创建一个二维坐标。
        /// 数值顺序按照坐标系统定义的坐标轴的顺序赋值。一般为 x、y、z 或 lon、 lat、height。
        /// </summary>
        /// <param name="valOfAxis1">第一个坐标轴的值</param>
        /// <param name="valOfAxis2">第二个坐标轴的值</param>
        /// <param name="weight">权值</param>
        /// <returns>一个二维坐标实例</returns>
        ICoordinate Create2D(Double valOfAxis1, Double valOfAxis2, Double weight=0);
        /// <summary>
        /// 创建一个二维坐标。
        /// 数值顺序按照坐标系统定义的坐标轴的顺序赋值。一般为 x、y、z 或 lon、 lat、height。
        /// </summary>
        /// <param name="coordinates">数组，第1、2个为坐标值，第3个为权值</param>
        /// <returns></returns>
        ICoordinate Create2D(params Double[] coordinates);  
        /// <summary>
        /// 创建一个3维坐标。
        /// 数值顺序按照坐标系统定义的坐标轴的顺序赋值。一般为 x、y、z 或 lon、 lat、height。
        /// </summary>
        /// <param name="valOfAxis1">第1个坐标轴的值</param>
        /// <param name="valOfAxis2">第2个坐标轴的值</param>
        /// <param name="valOfAxis3">第3个坐标轴的值</param>
        /// <param name="weight">权值</param>
        /// <returns></returns>
        ICoordinate Create3D(Double valOfAxis1, Double valOfAxis2, Double valOfAxis3, Double weight=0);
        /// <summary>
        /// 创建一个3维坐标。
        /// 数值顺序按照坐标系统定义的坐标轴的顺序赋值。一般为 x、y、z 或 lon、 lat、height。
        /// </summary>
        /// <param name="coordinates">数组，第1、2、3个为坐标值，第4个为权值</param>
        /// <returns></returns>
        ICoordinate Create3D(params Double[] coordinates);

        //IAffineTransformMatrix<DoubleComponent> CreateTransform(ICoordinate scaleVector,
        //                                                        ICoordinate rotationAxis,
        //                                                        Double rotation,
        //                                                        ICoordinate translateVector);
        //ICoordinate Homogenize(ICoordinate coordinate);
        //ICoordinate Dehomogenize(ICoordinate coordinate);
        //IPrecisionModel PrecisionModel { get; }
        //IPrecisionModel CreatePrecisionModel(PrecisionModelType type);
        //IPrecisionModel CreatePrecisionModel(Double scale);

        /// <summary>
        /// 创建一个大地坐标
        /// </summary>
        /// <param name="lon">经度</param>
        /// <param name="lat">维度</param>
        /// <param name="height">高程</param>
        /// <param name="weight">权值</param>
        /// <returns></returns>
        IGeodeticCoord CreateGeodeticCoord(double lon, double lat, double height, double weight = 0);
        /// <summary>
        /// 创建一个经纬度坐标。
        /// </summary>
        /// <param name="lon">经度</param>
        /// <param name="lat">维度</param> 
        /// <param name="weight">权值</param>
        /// <returns></returns>
        ILonLatCoord CreateLonLatCoord(double lon, double lat, double weight = 0);
        /// <summary>
        /// 创建XY坐标
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="weight">权值</param>
        /// <returns></returns>
        IXyCoord CreateXyCoord(double x, double y, double weight = 0);
        /// <summary>
        /// 创建一个XYZ分量的坐标。
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="weight">权值</param>
        /// <param name="z">Z</param> 
        /// <returns></returns>
        IXyzCoord CreateXyzCoord(double x, double y, double z, double weight = 0);
    }
}
