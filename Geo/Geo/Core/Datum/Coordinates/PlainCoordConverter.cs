//2017.10.13, czs, create in hongqing, 线性坐标数据转换器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using Geo.Coordinates;


namespace Geo.Coordinates
{ 
    /// <summary>
    /// 二维平面坐标转换器，缩放和平移。
    /// </summary>
    public class PlainCoordConverter
    {
        /// <summary>
        /// 二维平面坐标转换器，只平移，不缩放
        /// </summary> 
        /// <param name="oldStartXY">只平移 </param>
        public PlainCoordConverter(Size oldStartXY)
        {
            XConverter = new LineCoordConverter(oldStartXY.Width);
            YConverter = new LineCoordConverter(oldStartXY.Height);
        }

        /// <summary>
        /// 二维平面坐标转换器，只平移，不缩放
        /// </summary> 
        /// <param name="oldStartXY">只平移 </param>
        public PlainCoordConverter(XY oldStartXY)
        {
            XConverter = new LineCoordConverter(oldStartXY.X);
            YConverter = new LineCoordConverter(oldStartXY.Y);
        }

        /// <summary>
        /// 二维平面坐标转换器
        /// </summary>
        /// <param name="oldSize"></param>
        /// <param name="newSize"></param>
        /// <param name="oldStartXY"></param>
        public PlainCoordConverter(XY oldSize, Size newSize, XY oldStartXY)
        {
            XConverter = new LineCoordConverter(oldSize.X, newSize.Width, oldStartXY.X);
            YConverter = new LineCoordConverter(oldSize.Y, newSize.Height, oldStartXY.Y);
        }
        /// <summary>
        /// 二维平面坐标转换器
        /// </summary>
        /// <param name="oldSize"></param>
        /// <param name="newSize"></param>
        /// <param name="oldStartXY"></param>
        public PlainCoordConverter(XY oldSize, XY newSize, XY oldStartXY)
        {
            XConverter = new LineCoordConverter(oldSize.X, newSize.X, oldStartXY.X);
            YConverter = new LineCoordConverter(oldSize.Y, newSize.Y, oldStartXY.Y);
        }
        /// <summary>
        /// 二维平面坐标转换器
        /// </summary>
        /// <param name="oldSize"></param>
        /// <param name="newSize"></param>
        /// <param name="oldStartXY"></param>
        public PlainCoordConverter(Point oldSize, Point newSize, Point oldStartXY)
        {
            XConverter = new LineCoordConverter(oldSize.X, newSize.X, oldStartXY.X);
            YConverter = new LineCoordConverter(oldSize.Y, newSize.Y, oldStartXY.Y);
        }
        /// <summary>
        /// 二维平面坐标转换器
        /// </summary>
        /// <param name="oldSize"></param>
        /// <param name="newSize"></param>
        /// <param name="oldStartXY"></param>
        public PlainCoordConverter(Size oldSize, Size newSize, Point oldStartXY)
        {
            XConverter = new LineCoordConverter(oldSize.Width, newSize.Width, oldStartXY.X);
            YConverter = new LineCoordConverter(oldSize.Height, newSize.Height, oldStartXY.Y);
        }
        /// <summary>
        /// X 轴转换器
        /// </summary>
        public LineCoordConverter XConverter { get; set; }
        /// <summary>
        /// Y轴转换器
        /// </summary>
        public LineCoordConverter YConverter { get; set; }
        /// <summary>
        /// 最大的因子
        /// </summary>
        public double MaxFactor { get { return Math.Max(this.XConverter.Factor, this.YConverter.Factor); } }


        /// <summary>
        ///转换到新的
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public XY GetNew(XY old)
        {
            var newX = XConverter.GetNew(old.X);
            var newY = YConverter.GetNew(old.Y);
            return new XY(newX, newY);
        }
        /// <summary>
        ///转换到新的
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public Point GetNewPoint(XY old)
        {
            var newX = XConverter.GetNew(old.X);
            var newY = YConverter.GetNew(old.Y);
            return new Point((int)Math.Round(newX), (int)Math.Round( newY));
        }
        /// <summary>
        /// 获取新的
        /// </summary>
        /// <param name="contentBoxUserCoord"></param>
        /// <returns></returns>
        public Envelope GetNew(Envelope contentBoxUserCoord)
        {
            var MinX = XConverter.GetNew(contentBoxUserCoord.MinX);
            var MaxX = XConverter.GetNew(contentBoxUserCoord.MaxX);
            var MinY = YConverter.GetNew(contentBoxUserCoord.MinY);
            var MaxY = YConverter.GetNew(contentBoxUserCoord.MaxY);
            return new Envelope(MinX, MinY, MaxX, MaxY);
        }
    }
     
}
