//2017.02.22, czs, created in hongqing, 新设计坐标转换器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Coordinates
{
   
    /// <summary>
    /// XY二维坐标转换器
    /// </summary>
    public interface IXyCoordConverter : ICoordConverter<XY>
    {
    }
    /// <summary>
    /// 一维坐标转换器
    /// </summary>
    public interface IOneDimCoordConverter : ICoordConverter<double>
    {
    }

    /// <summary>
    /// 坐标转换接口
    /// </summary>
    /// <typeparam name="TCoord"></typeparam>
    public interface ICoordConverter<TCoord>
    {
        /// <summary>
        /// 转换为新坐标
        /// </summary>
        /// <param name="oldCoord"></param>
        /// <returns></returns>
        TCoord GetNew(TCoord oldCoord);
        /// <summary>
        /// 转换为老坐标系的坐标值
        /// </summary>
        /// <param name="newCoord"></param>
        /// <returns></returns>
        TCoord GetOld(TCoord newCoord);
    }
    /// <summary>
    /// 坐标放缩，尺度变化类。
    /// </summary>
    public class CoordZoomer : IXyCoordConverter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="zoomScaleX"></param>
        /// <param name="zoomScaleY"></param>
        public CoordZoomer(double zoomScaleX, double zoomScaleY)
        {
            this.ScaleZoomerX = new ScaleZoomer(zoomScaleX);
            this.ScaleZoomerY = new ScaleZoomer(zoomScaleY);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="zoomScale"></param>
        public CoordZoomer(XY zoomScale)
        {
            this.ScaleZoomerX = new ScaleZoomer(zoomScale.X);
            this.ScaleZoomerY = new ScaleZoomer(zoomScale.Y);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oldCoord">对应点的老坐标，不可为原点</param>
        /// <param name="newCoord">对应点的新坐标，不可为原点 </param>
        public CoordZoomer(XY oldCoord, XY newCoord) : this( new XY(oldCoord.X / newCoord.X, oldCoord.Y / newCoord.Y) )
        { 
        }
 
        /// <summary>
        /// 坐标放缩尺度, X 方向。
        /// 定义为： ZoomScale =  老尺度 / 新尺度。
        /// 如，新单位为 1 cm，老单位为 1 m=100 cm，则 ZoomScale = 100
        /// </summary>
        public ScaleZoomer ScaleZoomerX { get; set; }
        /// <summary>
        /// 坐标放缩尺度, Y 方向。
        /// 定义为： ZoomScale =  老尺度 / 新尺度。
        /// 如，新单位为 1 cm，老单位为 1 m=100 cm，则 ZoomScale = 100
        /// </summary>
        public ScaleZoomer ScaleZoomerY { get; set; }
        /// <summary>
        /// 获取新坐标系中的坐标。
        /// </summary>
        /// <param name="oldCoord"></param>
        /// <returns></returns>
        public XY GetNew(XY oldCoord)
        {
            return new XY(ScaleZoomerX.GetNew(oldCoord.X), ScaleZoomerY.GetNew(oldCoord.Y));
        }
        /// <summary>
        /// 获取老坐标系中的坐标。
        /// </summary>
        /// <param name="newCoord"></param>
        /// <returns></returns>
        public XY GetOld(XY newCoord)
        {
            return new XY(ScaleZoomerX.GetOld(newCoord.X), ScaleZoomerY.GetOld(newCoord.Y));
        } 
    }

    /// <summary>
    /// 坐标放缩，尺度变化类。
    /// </summary>
    public class ScaleZoomer: IOneDimCoordConverter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="zoomScale"></param>
        public ScaleZoomer(double zoomScale)
        {
            this.ZoomScale = zoomScale;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oldVal">对应点的老坐标，不可为原点</param>
        /// <param name="newCoord">对应点的新坐标，不可为原点 </param>
        public ScaleZoomer(double  oldVal, double  newCoord)
        {
            this.ZoomScale = oldVal / newCoord;
        }
        /// <summary>
        /// 坐标放缩尺度.分别对应X和Y方向。
        /// 定义为： ZoomScale =  老尺度 / 新尺度。
        /// 如，新单位为 1 cm，老单位为 1 m=100 cm，则 ZoomScale = 100
        /// </summary>
        public double  ZoomScale { get; set; }
        /// <summary>
        /// 获取新坐标系中的坐标。
        /// </summary>
        /// <param name="oldVal"></param>
        /// <returns></returns>
        public double GetNew(double oldVal)
        {
            return oldVal * ZoomScale;
        }
        /// <summary>
        /// 获取老坐标系中的坐标。
        /// </summary>
        /// <param name="newVal"></param>
        /// <returns></returns>
        public double GetOld(double newVal)
        {
            return newVal / ZoomScale;
        }
    }
    


    /// <summary>
    /// 坐标反转类
    /// </summary>
    public class CoordInverser : IXyCoordConverter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="IsYInvert"></param>
        /// <param name="IsXInvert"></param>
        public CoordInverser(bool IsYInvert, bool IsXInvert)
        {
            this.IsYInvert = IsYInvert;
            this.IsXInvert = IsXInvert;
        }
        /// <summary>
        /// 是否反转Y轴
        /// </summary>
        public bool IsYInvert { get; set; }
        /// <summary>
        /// 是否反转X轴
        /// </summary>
        public bool IsXInvert { get; set; }
        /// <summary>
        /// 获取新坐标系中的坐标。
        /// </summary>
        /// <param name="oldCoord"></param>
        /// <returns></returns>
        public XY GetNew(XY oldCoord)
        { 
            XY　xy = null;
            if (IsXInvert)
            {
                xy = oldCoord.GetXInverted();
            }
            if (IsYInvert)
            {
                xy = oldCoord.GetYInverted();
            }
            return xy;
        }

        /// <summary>
        /// 获取老坐标系中的坐标。
        /// </summary>
        /// <param name="oldCoord"></param>
        /// <returns></returns>
        public XY GetOld(XY oldCoord)
        {
            return GetNew(oldCoord);
        }
    }
     
    /// <summary>
    /// 坐标平移类
    /// </summary>
    public class CoordTranslater : IXyCoordConverter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="TransVector">平移向量，老坐标系原点指向新坐标系原点，即新坐标系原点在老坐标系中的位置向量</param>
        public CoordTranslater(XY TransVector)
        {
            this.TransVector = TransVector;
        }
        /// <summary>
        /// 平移坐标，由旧坐标系原点指向新坐标系原点。
        /// </summary>
        public XY TransVector { get; set; }
        /// <summary>
        /// 获取新坐标系中的坐标。
        /// </summary>
        /// <param name="oldCoord"></param>
        /// <returns></returns>
        public XY GetNew(XY oldCoord)
        {
            return oldCoord - TransVector;
        }
        /// <summary>
        /// 获取老坐标系中的坐标。
        /// </summary>
        /// <param name="newCoord"></param>
        /// <returns></returns>
        public XY GetOld(XY newCoord)
        {
            return newCoord + TransVector;
        }
    }
     

}
