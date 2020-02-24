//2017.10.12, czs, create in hongqing, 二维颜色绘图

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
    /// 颜色绘图
    /// </summary>
    public class ColorChartDrawer
    {
        /// <summary>
        /// 颜色绘图
        /// </summary>
        public ColorChartDrawer()
        {        
        }

        #region  属性
        #region 坐标转换器
        /// <summary>
        /// 屏幕转换。将用户坐标转换到屏幕坐标。
        /// </summary>
        public ScreenCoordConverter UserToScreenCoordConverter { get; set; }
        /// <summary>
        /// 将用户数据转换为用户屏幕坐标。从起始点开始。因此需要加上用户坐标原点，再转换到屏幕坐标。
        /// </summary>
        public PlainCoordConverter InputToContentCoordConverter { get; set; }
        /// <summary>
        /// 内容坐标转换为用户坐标。
        /// </summary>
        public PlainCoordConverter ContentToUserCoordConverter { get; set; }
        #endregion

        /// <summary>
        /// 绘图类。采用用户坐标（像素），左下为原点，水平为X，竖直为Y。
        /// </summary>
        public UserChartGraphics ChartGraphics { get; set; }
        /// <summary>
        /// 内容边框大小。
        /// </summary>
        public Size ContentBorderSize { get; set; }
        /// <summary>
        /// 整个绘图区的大小。包含坐标轴，标签，内容区等。
        /// </summary>
        public Size ChartSize { get; set; }
        /// <summary>
        /// 绘图内容坐标原点在绘图区的坐标位置
        /// </summary>
        public Point ContentOrigin { get; set; }
        /// <summary>
        /// 内容边框宽度
        /// </summary>
        public int ContentBorderWidth { get; set; }
        /// <summary>
        /// 绘图右上距离大小
        /// </summary>
        public Size RightTopSpace { get; set; }
        /// <summary>
        /// 绘图左下距离大小
        /// </summary>
        public Size LeftDownSpace { get; set; }
        /// <summary>
        /// 绘图区大小
        /// </summary>
        public Size ContentSize { get; set; }
        /// <summary>
        /// 绘图区中心
        /// </summary>
        public Size ContentCenter { get; set; }

        /// <summary>
        /// 数据表
        /// </summary>
        public TwoNumeralKeyAndValueDictionary TableValue { get; set; }

        /// <summary>
        /// 内容盒子，坐标为屏幕坐标。
        /// </summary>
        public Envelope ContentBoxInScreenCoord { get; set; }
        /// <summary>
        /// 内容盒子，坐标为内容坐标
        /// </summary>
        public Envelope ContentBox { get; set; }
        /// <summary>
        /// 颜色生成器
        /// </summary>
        public INumeralColorBuilder ColorBuilder { get; set; }
        /// <summary>
        /// 颜色图例尺寸
        /// </summary>
        public Size SizeOfLegendColorBar { get; set; }
        /// <summary>
        /// 用户坐标下的图例起始坐标。
        /// </summary>
        public Point OriginOfLengendColorBar { get; set; }
        #endregion

        /// <summary>
        /// 设置绘图区大小，同时初始化其它可变参数。
        /// </summary>
        /// <param name="chartSize"></param>
        public void Init(Size chartSize)
        {
            RightTopSpace = new Size(80, 20);
            LeftDownSpace = new Size(60, 75);
            ContentBorderWidth = 2;
            this.ContentOrigin = new Point(LeftDownSpace.Width, LeftDownSpace.Height);
            this.ContentBorderSize = new Size(ContentBorderWidth, ContentBorderWidth);

            this.ChartSize = chartSize;
            this.ContentSize = ChartSize - LeftDownSpace - RightTopSpace;
            this.ContentCenter = new Size(ContentSize.Width / 2, ContentSize.Height / 2);
            this.SizeOfLegendColorBar = new Size(12, ToInt(ContentSize.Height * 0.6, 10));

            this.ContentToUserCoordConverter = new PlainCoordConverter(Size.Empty - this.LeftDownSpace);
            this.UserToScreenCoordConverter = new ScreenCoordConverter(this.ChartSize);
            this.ContentBox = new Envelope(0, 0, ContentSize.Width, ContentSize.Height);
            this.ContentBoxInScreenCoord = ContentToScreenCoord(ContentBox);
            this.OriginOfLengendColorBar = new Point(ContentSize.Width + 100, ToInt(ContentSize.Height * 0.2, 5) + LeftDownSpace.Height);
        }

        #region 大地坐标方式数据绘制
        /// <summary>
        /// 大地坐标方式数据绘制
        /// </summary>
        public GeoCoords GeoCoords { get; set; }
        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="geoCoords"></param>
        public Bitmap Draw(List<GeoCoord> geoCoords)
        {
            if (ChartSize.Width == 0 || ChartSize.Height == 0) { return null; }

            Bitmap bmp = new Bitmap(ChartSize.Width, ChartSize.Height); ;
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            this.UserToScreenCoordConverter = new ScreenCoordConverter(this.ChartSize);
            this.GeoCoords = new GeoCoords(geoCoords);
            this.ChartGraphics = new Geo.Draw.UserChartGraphics(UserToScreenCoordConverter, g);

            InputToContentCoordConverter = new PlainCoordConverter(this.GeoCoords.Size, this.ChartSize, GeoCoords.CoordFrom);
            //var ColorBuilder = new TwoStepColorBuilder(this.GeoCoords.HeightSpan, Color.Blue, Color.Yellow, Color.Red);
            var ColorBuilder = new ThreeStepColorBuilder(this.GeoCoords.HeightSpan, Color.Blue, Color.Cyan, Color.Yellow, Color.Red);
            int cellWidth = (int)Math.Ceiling(InputToContentCoordConverter.XConverter.Factor * GeoCoords.LonInterval);
            cellWidth = cellWidth < 1 ? 1 : cellWidth;
            int cellHeight = (int)Math.Ceiling(InputToContentCoordConverter.YConverter.Factor * GeoCoords.LatInterval);
            cellHeight = cellHeight < 1 ? 1 : cellHeight;
            foreach (var geoCoord in geoCoords)
            {
                var xy = new XY(geoCoord.Lon, geoCoord.Lat);
                var pt = InputToContentCoordConverter.GetNewPoint(xy);
                var scrPt = UserToScreenCoordConverter.GetScreenCoord(pt);

                var color = ColorBuilder.Build(geoCoord.Height);
                ChartGraphics.DrawColorPoint(scrPt, cellWidth, cellHeight, color);
            }
            return bmp;
            //    UserChartGraphics.DrawGrid(new Pen(new SolidBrush(Color.FromArgb(100, 100, 100))), 10, 10, Origin, this.ScreenCoordArea.RightTop);
        }
        #endregion

        /// <summary>
        /// 绘图。
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public Bitmap Draw(TwoNumeralKeyAndValueDictionary table, NumerialSegment valSpanForColor=null)
        {
            this.TableValue = table;
            if (table == null || table.Count == 0) return null;
            if (ChartSize.Width == 0 || ChartSize.Height == 0) { return null; }

            this.InputToContentCoordConverter = new PlainCoordConverter(table.KeySize, this.ContentSize, table.LeftDown);

            this.ColorBuilder = new ThreeStepColorBuilder(valSpanForColor??table.ValueSpan, Color.Blue, Color.Cyan, Color.Yellow, Color.Red);

            Bitmap bmp = new Bitmap(ChartSize.Width, ChartSize.Height); ;
            Graphics g = Graphics.FromImage(bmp);

            g.Clear(Color.White);
            this.ChartGraphics = new Geo.Draw.UserChartGraphics(UserToScreenCoordConverter, g);

            DrawColorTable(table);
            DrawContentBoxBorder();
            DrawYLabels();
            DrawXLables();
            DrawLegendColorBar();

            return bmp;
        }

        #region 绘图细节
        /// <summary>
        /// 绘制二维颜色图
        /// </summary>
        /// <param name="table"></param>
        public void DrawColorTable(TwoNumeralKeyAndValueDictionary table)
        {
            int cellWidth = ToInt(InputToContentCoordConverter.XConverter.Factor * table.IntervalA);
            int cellHeight = ToInt(InputToContentCoordConverter.YConverter.Factor * table.IntervalB);

            table.ForEach(new Action<double, double, double>(delegate(double x, double y, double val)
            {
                var xy = new XY(x, y);
                var scrPt = InputToScreenPt(xy);

                var color = ColorBuilder.Build(val);
                ChartGraphics.DrawColorPoint(scrPt, cellWidth, cellHeight, color, ContentBoxInScreenCoord);
            }));
        }
        /// <summary>
        /// 绘制内容边框
        /// </summary>
        private void DrawContentBoxBorder()
        {
            Pen pen = new Pen(Color.Black, ContentBorderWidth);
            ChartGraphics.DrawBox(ContentBoxInScreenCoord, pen, true);
        }

        /// <summary>
        /// 图例
        /// </summary>
        public void DrawLegendColorBar()
        {
            if (TableValue == null) { return; }
            int cellWidth = 12;
            int cellHeight = 1;

            LineCoordConverter pixeToInputValConverter = new LineCoordConverter(SizeOfLegendColorBar.Height, this.TableValue.ValueSpan.Span);
            for (int i = 0; i < SizeOfLegendColorBar.Height; i++)
            {
                var val = pixeToInputValConverter.GetNew(i) + this.TableValue.ValueSpan.Start; //像素，转换到原始数据，以获取颜色
                var color = ColorBuilder.Build(val);
                //转回平面坐标
                var pt = OriginOfLengendColorBar + new Size(0, i);
                var scrPt = UserToScreenCoordConverter.GetScreenCoord(pt);

                ChartGraphics.DrawColorPoint(scrPt, cellWidth, cellHeight, color);
            }
            //绘制文字
            ChartGraphics.DrawLabel(Geo.Utils.StringUtil.FillSpaceLeft(this.TableValue.ValueSpan.Start.ToString("0.00"), 6), OriginOfLengendColorBar - new Size(30, 20), 0);
            ChartGraphics.DrawLabel(Geo.Utils.StringUtil.FillSpaceLeft(this.TableValue.ValueSpan.End.ToString("0.00"), 6),
              OriginOfLengendColorBar + new Size(-30, SizeOfLegendColorBar.Height + 20), 0);
        }
        /// <summary>
        /// 绘制Y轴标签
        /// </summary>
        private void DrawYLabels()
        {
            if (TableValue == null) { return; }

            //绘制Y轴标签，纵轴，卫星号   

            int x = 3;
            int minYDiffer = 30;
            double prevYPixe = -minYDiffer;
            double endY = InputToContentCoordConverter.YConverter.GetNew(TableValue.KeyBSpan.End) + ContentOrigin.Y + 10;

            for (var y = this.TableValue.KeyBSpan.Start; y <= TableValue.KeyBSpan.End; y = TableValue.IntervalB + y)
            {
                var yCoord = InputToContentCoordConverter.YConverter.GetNew(y) + ContentOrigin.Y + 10;
                var pos = new Point(x, (int)yCoord);
                if ((pos.Y - prevYPixe >= minYDiffer && endY - pos.Y > minYDiffer-1) || Math.Abs(y - this.TableValue.KeyBSpan.End) < 1E-2)
                {
                    ChartGraphics.DrawLabel(Geo.Utils.StringUtil.FillSpaceLeft(y.ToString("0.00") + "°", 7), pos);

                    prevYPixe = pos.Y;
                }
            }
        }

        /// <summary>
        /// 绘制X轴标签
        /// </summary>
        private void DrawXLables()
        {
            if (TableValue == null) { return; }

            //绘制历元标签 
            double prevX = -50;
            double minXDiffer = 30;
            double endX = InputToContentCoordConverter.XConverter.GetNew(this.TableValue.KeyASpan.End) + ContentOrigin.X;

            int y = 17;

            for (var x = TableValue.KeyASpan.Start; x <= this.TableValue.KeyASpan.End; x += TableValue.IntervalA)
            {
                var xpt = InputToContentCoordConverter.XConverter.GetNew(x) + ContentOrigin.X;

                var pos = new Point((int)xpt, y);
                if (((pos.X - prevX) >= minXDiffer && endX - pos.X > minXDiffer-1) || Math.Abs(x - this.TableValue.KeyASpan.End) < 1E-2)
                {
                    prevX = pos.X;

                    var xlabel = Geo.Utils.StringUtil.FillSpaceLeft(x.ToString("0.00") + "°", 8);

                    ChartGraphics.DrawLabel(xlabel, pos, -90);
                }
            }
        }

        #region 工具细节

        /// <summary>
        /// 输入原始数据转换为屏幕坐标
        /// </summary>
        /// <param name="xy"></param>
        /// <returns></returns>
        private Point InputToScreenPt(XY xy)
        {
            var userCoord = InputToUserCoord(xy);
            var scrPt = UserToScreenCoordConverter.GetScreenCoord(userCoord);
            return scrPt;
        }
        /// <summary>
        /// 输入坐标转换为用户坐标。
        /// </summary>
        /// <param name="rawXy"></param>
        /// <returns></returns>
        private Point InputToUserCoord(XY rawXy)
        {
            var contentCoord = InputToContentCoordConverter.GetNew(rawXy);
            var userCoord = ContentToUserCoordConverter.GetNewPoint(contentCoord);
            return userCoord;
        }
        /// <summary>
        /// 内容坐标转换为屏幕坐标。
        /// </summary>
        /// <param name="xy"></param>
        /// <returns></returns>
        private Point ContentToScreenCoord(XY contentCoord)
        {
            var userCoord = ContentToUserCoordConverter.GetNewPoint(contentCoord);
            return UserToScreenCoordConverter.GetScreenCoord(userCoord);
        }
        /// <summary>
        /// 内容坐标转换为屏幕坐标。
        /// </summary>
        /// <param name="xy"></param>
        /// <returns></returns>
        private Envelope ContentToScreenCoord(Envelope contentCoord)
        {
            var userCoord = ContentToUserCoordConverter.GetNew(contentCoord);
            return UserToScreenCoordConverter.ToScreenCoord(userCoord);
        }
        /// <summary>
        /// 转换为整数，不小于指定整数。
        /// </summary>
        /// <param name="val"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        private static int ToInt(double val, int minValue = 1)
        {
            int cellLength = (int)Math.Ceiling(val);
            cellLength = cellLength < minValue ? minValue : cellLength;
            return cellLength;
        }
        #endregion
         
        #endregion 
    }


}