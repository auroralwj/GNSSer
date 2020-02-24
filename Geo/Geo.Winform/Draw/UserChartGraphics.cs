//2016.12.01, czs, create in hongqing, 绘制坐标
//2017.10.16, czs, edit in hongqing, 实现屏幕颜色绘制，实现BOX绘制

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Coordinates;

namespace Geo.Draw
{
    /// <summary>
    /// 绘图类。支持用户坐标和屏幕坐标绘制。
    /// 采用用户坐标（像素），左下为原点，水平为X，竖直为Y。
    /// </summary>
    public class UserChartGraphics
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ScreenCoordArea"></param>
        /// <param name="Graphics"></param>
        public UserChartGraphics(ScreenCoordConverter ScreenCoordArea, Graphics Graphics)
        {
            this.UserToScreenCoordConverter = ScreenCoordArea;
            this.Graphics = Graphics;
            this.GraphicsText = new GraphicsText();
            DefaultFont = new Font("微软雅黑", 10);
            DefaultBrush = new SolidBrush(Color.Black);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ScreenCoordArea"></param>
        public UserChartGraphics(ScreenCoordConverter ScreenCoordArea)
        {
            this.UserToScreenCoordConverter = ScreenCoordArea;
            this.GraphicsText = new GraphicsText();
            DefaultFont = new Font("宋体", 8);
            DefaultBrush = new SolidBrush(Color.Black);
            DefaultStringFormat = new StringFormat();
            DefaultStringFormat.Alignment = StringAlignment.Center;
            DefaultStringFormat.LineAlignment = StringAlignment.Center;
        }

        #region 属性
        /// <summary>
        /// 绘图
        /// </summary>
        public Graphics Graphics { get; set; }
        Graphics g { get { return Graphics; } }
        /// <summary>
        /// 用户坐标转换到屏幕坐标
        /// </summary>
        ScreenCoordConverter UserToScreenCoordConverter { get; set; }
        GraphicsText GraphicsText { get; set; }
        /// <summary>
        /// 默认字体
        /// </summary>
        public Font DefaultFont { get; set; }
        /// <summary>
        /// 默认刷子
        /// </summary>
        public Brush DefaultBrush { get; set; }
        /// <summary>
        /// 默认刷子
        /// </summary>
        public StringFormat DefaultStringFormat { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 绘制标签
        /// </summary>
        /// <param name="label"></param>
        /// <param name="pt"></param> 
        /// <param name="degree"></param>
        public void DrawLabel(string label, Point pt, float degree)
        {
            DrawLabel(label, pt, null, null, null, degree);
        }
        /// <summary>
        /// 绘制标签
        /// </summary>
        /// <param name="label"></param>
        /// <param name="pt"></param>
        /// <param name="font"></param>
        /// <param name="brush"></param>
        /// <param name="format"></param>
        /// <param name="degree"></param>
        public void DrawLabel(string label, Point pt, Font font = null, Brush brush = null, StringFormat format = null, float degree = 0)
        {
            if (font == null)
            {
                font = DefaultFont;
            }
            if (brush == null)
            {
                brush = DefaultBrush;
            }

            var screenPt = UserToScreenCoordConverter.GetScreenCoord(pt);

            //  g.DrawString(label, font, brush, screenPt);
            // 绘制围绕点旋转的文本  
            if (format == null)
            {
                format = DefaultStringFormat;
            }
            GraphicsText.Graphics = Graphics;
            GraphicsText.DrawString(label, font, brush, screenPt, format, degree);
        }
        /// <summary>
        /// 绘制带
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="yCoord"></param>
        /// <param name="xCoords"></param>
        /// <param name="height"></param>
        public void DarwBelt(Pen pen, int yCoord, int[] xCoords, int height = 5)
        {
            var y = UserToScreenCoordConverter.GetScreenY(yCoord);
            var xList = new List<int>();

            foreach (var item in xCoords)
            {
                var x = UserToScreenCoordConverter.GetScreenX(item);
                xList.Add(x);
            }
            int halfHeight = height / 2;
            foreach (var item in xCoords)
            {
                var pt = new Point(item, yCoord - halfHeight);
                var pt2 = new Point(item, yCoord + halfHeight);
                DrawLine(pen, pt, pt2);
            }
        }
        /// <summary>
        /// 绘制线条。连续绘制
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="pts"></param>
        public void DrawLine(Pen pen, params  Point[] pts)
        {
            List<Point> screentPts = new List<Point>();
            foreach (var item in pts)
            {
                screentPts.Add(UserToScreenCoordConverter.GetScreenCoord(item));
            }

            Point current = Point.Empty;

            foreach (var p in screentPts)
            {
                if (current == Point.Empty)
                {
                    current = p;
                    continue;
                }

                g.DrawLine(pen, current, p);
                current = p;
            }
        }

        /// <summary>
        /// 绘制网格
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <param name="fromPt"></param>
        /// <param name="toPt"></param>
        public void DrawGrid(Pen pen, int rowCount, int colCount, Point fromPt, Point toPt)
        {
            int minX = Math.Min(fromPt.X, toPt.X);
            int minY = Math.Min(fromPt.Y, toPt.Y);
            int maxX = Math.Max(fromPt.X, toPt.X);
            int maxY = Math.Max(fromPt.Y, toPt.Y);

            int borderHeight = Math.Abs(toPt.Y - fromPt.Y);
            int borderWidth = Math.Abs(toPt.X - fromPt.X);
            int yStep = borderHeight / rowCount;
            int xStep = borderWidth / colCount;

            for (int row = 1; row <= rowCount; row++)
            {
                int y = minY + row * yStep;
                DrawLine(pen, new Point(minX, y), new Point(maxX, y));
            }
            for (int col = 1; col <= colCount; col++)
            {
                int x = minX + col * xStep;
                DrawLine(pen, new Point(x, minY), new Point(x, maxY));
            }
        }
  
        /// <summary>
        /// 绘制盒子。
        /// </summary>
        /// <param name="box"></param>
        /// <param name="pen"></param>
        /// <param name="isContentNoboder">中间是否不绘制</param>
        public void DrawBox(Envelope box, Pen pen, bool isContentNoboder = true)
        {
            if (isContentNoboder)
            {
                var borderWidth = pen.Width;
                var halfBorder = pen.Width / 2;
                g.DrawRectangle(pen, box.LeftBottom.XRoundInt - halfBorder, box.LeftBottom.YRoundInt - halfBorder, (int)Math.Round(box.Width) + borderWidth, (int)Math.Round(box.Height) + borderWidth);
            }
            else
            {
                g.DrawRectangle(pen, box.LeftBottom.XRoundInt, box.LeftBottom.YRoundInt, (int)Math.Round(box.Width), (int)Math.Round(box.Height));
            }
        }


        /// <summary>
        /// 绘图坐标为居中部分
        /// </summary>
        /// <param name="scrPt">屏幕坐标</param>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="screenBox">绘图结果不超出此区域，屏幕坐标表示</param>
        public void DrawColorPoint(Point scrPt, int width, int height, Color color, Envelope screenBox = null)
        {
            SolidBrush brush = new SolidBrush(color);
            int halfWidth = width / 2;
            int halfHeight = height / 2;
            int x = scrPt.X - halfWidth;
            int y = scrPt.Y - halfHeight;
            if (screenBox != null)
            {
                //如果小，则会被覆盖
                if (x < screenBox.MinX) { x = (int)Math.Round(screenBox.MinX); }
                if (y < screenBox.MinY) { y = (int)Math.Round(screenBox.MinY); }
                //如果大， 只绘制一半。
                if (x + width > screenBox.MaxX) { width = (int)Math.Round(screenBox.MaxX) - x; }
                if (y + height > screenBox.MaxY) { height = (int)Math.Round(screenBox.MaxY) - y; }
            }

            g.FillRectangle(brush, x, y, width, height);
        }
        #endregion
    }
}
