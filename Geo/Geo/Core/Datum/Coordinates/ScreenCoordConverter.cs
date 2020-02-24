//2016.12.01, czs, create in hongqing, 绘制坐标
//2017.10.17, czs, edit in hongqing, 增加BOX转换。

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
    /// 屏幕区域坐标 转换器。负责用户坐标与屏幕坐标的转换。
    /// </summary>
    public class ScreenCoordConverter
    {
        /// <summary>
        /// 屏幕区域坐标 转换器。负责用户坐标与屏幕坐标的转换。
        /// </summary>
        /// <param name="chartSize"></param>
        public ScreenCoordConverter(Size chartSize)
        {
            this.ChartSize = chartSize;
        }

        #region 属性
        /// <summary>
        /// 左下角坐标。
        /// </summary>
        public Point LeftDown { get { return new Point(0, 0); } }
        /// <summary>
        /// 右上角坐标。
        /// </summary>
        public Point LeftTop { get { return new Point(0, ChartSize.Height); } }
        /// <summary>
        /// 左上角坐标。
        /// </summary>
        public Point RightTop { get { return new Point(ChartSize.Width, ChartSize.Height); } }
        /// <summary>
        /// 右下角坐标。
        /// </summary>
        public Point RightDown { get { return new Point(ChartSize.Width, 0); } }

        /// <summary>
        /// 绘图区大小，画板、控件尺寸，如果超出，就不用绘图了。
        /// </summary>
        public Size ChartSize { get; set; }
        #endregion

        /// <summary>
        /// 获取框框坐标
        /// </summary>
        /// <returns></returns>
        public Point[] GetBorderCoords()
        {
            var pts = new List<Point>(){
               LeftDown ,
                LeftTop, 
                RightTop,
                RightDown,
                LeftDown};
            return pts.ToArray();
        }

        #region  转换


        /// <summary>
        /// 获取新的
        /// </summary>
        /// <param name="contentBoxUserCoord"></param>
        /// <returns></returns>
        public Envelope ToScreenCoord(Envelope contentBoxUserCoord)
        {
            var leftDown = contentBoxUserCoord.LeftBottom;
            var rightTop = contentBoxUserCoord.RightTop;

            return new Envelope(GetScreenXy(leftDown), GetScreenXy(rightTop));
        }

        /// <summary>
        /// 从用户坐标，以左下角为原点的坐标（横轴为X，右手系），
        /// 转换到屏幕坐标（以左上角为原点，横轴为X，左手系）。
        /// 转换方法，X 轴不变。Y轴平移并反向。
        /// </summary>
        /// <param name="userCoord">以左下角为原点的坐标（横轴为X，右手系）</param> 
        /// <returns></returns>
        public Point GetScreenCoord(Point userCoord)
        {
            int x = userCoord.X;
            var userY = userCoord.Y;
            int y = GetScreenY(userY);

            return new Point(x, y);
        }
        /// <summary>
        /// 从用户坐标，以左下角为原点的坐标（横轴为X，右手系），
        /// 转换到屏幕坐标（以左上角为原点，横轴为X，左手系）。
        /// 转换方法，X 轴不变。Y轴平移并反向。
        /// </summary>
        /// <param name="userCoord">以左下角为原点的坐标（横轴为X，右手系）</param> 
        /// <returns></returns>
        public Point GetScreenCoord(XY userCoord)
        {
            int x = userCoord.XRoundInt;
            var userY = userCoord.YRoundInt;
            int y = GetScreenY(userY);

            return new Point(x, y);
        }
        /// <summary>
        /// 从用户坐标，以左下角为原点的坐标（横轴为X，右手系），
        /// 转换到屏幕坐标（以左上角为原点，横轴为X，左手系）。
        /// 转换方法，X 轴不变。Y轴平移并反向。
        /// </summary>
        /// <param name="userCoord">以左下角为原点的坐标（横轴为X，右手系）</param> 
        /// <returns></returns>
        public XY GetScreenXy(XY userCoord)
        {
            int x = userCoord.XRoundInt;
            var userY = userCoord.YRoundInt;
            int y = GetScreenY(userY);

            return new XY(x, y);
        }
        /// <summary>
        /// 获取屏幕Y坐标
        /// </summary>
        /// <param name="userY"></param>
        /// <returns></returns>
        public int GetScreenY(int userY)
        {

            int height = ChartSize.Height;
            int y = -(userY - height);
            return y;
        }
        /// <summary>
        /// 获取屏幕 X 坐标。
        /// </summary>
        /// <param name="userX"></param>
        /// <returns></returns>
        public int GetScreenX(int userX)
        {
            return userX;
        }
        /// <summary>
        /// 获取屏幕线
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public Point[] GetScreenLine(Point[] pts)
        {
            int length = pts.Length;
            Point[] screenLine = new Point[pts.Length];
            for (int i = 0; i < length; i++)
            {
                screenLine[i] = GetScreenCoord(pts[i]);
            }
            return screenLine;
        }
        /// <summary>
        /// 计算用户坐标
        /// </summary>
        /// <param name="screenPt"></param>
        /// <returns></returns>
        public Point GetUserCoord(Point screenPt)
        {
            int width = ChartSize.Width;
            int height = ChartSize.Height;

            int x = screenPt.X;
            int y = (height - screenPt.Y);

            return new Point(x, y);
        }
        #endregion
    }
}
