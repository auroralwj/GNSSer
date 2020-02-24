//2018.08.21, czs, create in hmx, ChartSettingOption为论文而生

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks; 
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;

namespace Geo
{

    /// <summary>
    /// 绘图设置
    /// </summary>
    public class ChartSettingOption
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ChartSettingOption(Chart chart1) : this()
        {
            var legend = chart1.Legends[0];
            var chart = chart1.ChartAreas[0];
            
            foreach (var item in chart1.Series)
            {
                var opt = new SeriesSettingOption(item.XValueType, item.YValueType)
                {
                    AxisTypeOfX = item.XAxisType,
                    AxisTypeOfY = item.YAxisType,
                    BorderWidth = item.BorderWidth,
                    MarkerSize = item.MarkerSize,
                    SeriesChartType = item.ChartType,
                    Color = item.Color
                };
                //if ( item.Color.A == 0)
                //{
                //    opt.Color = Color.Blue;
                //}
                SeriesSettingOptions[item.Name] = opt;
            }

            if (chart1.Titles.Count == 0)
            {
                Title = new EnableString("绘图");
                FontOfTitle = new FontSettingOption(new Font(FontSettingOption.DefaultFontFamily, 16), Color.Black);
                TitleDocking = Docking.Top;
            }
            else
            {
                Title = new EnableString(chart1.Titles[0].Text);
                FontOfTitle = new FontSettingOption(chart1.Titles[0].Font, chart1.Titles[0].ForeColor);
                TitleDocking = chart1.Titles[0].Docking;
            }


            //main
            AxisColor = chart.AxisX.LineColor;
            AxisWidth = new EnableInteger(chart.AxisX.LineWidth);
            ValueSpanOfX = new EnableFloatSpan(chart.AxisX.Minimum, chart.AxisX.Maximum);
            ValueSpanOfY = new EnableFloatSpan(chart.AxisY.Minimum, chart.AxisY.Maximum);
            IntervalOfX = new EnableFloat(chart.AxisX.Interval);
            IntervalOfY = new EnableFloat(chart.AxisY.Interval);
            TitleOfX = new EnableString("历元");
            TitleOfY = new EnableString("偏差(m)");
            IsShowGridX = chart.AxisX.MajorGrid.Enabled;
            IsShowGridY = chart.AxisY.MajorGrid.Enabled;
            IsStartedFromZeroOfX = chart.AxisX.IsStartedFromZero;
            IsStartedFromZeroOfY = chart.AxisY.IsStartedFromZero;
            AxisXLableFormat = new EnableString(chart.AxisX.LabelStyle.Format);
            AxisYLableFormat = new EnableString(chart.AxisY.LabelStyle.Format);
            FontOfAxiesLabel = new FontSettingOption(chart.AxisX.LabelStyle.Font, chart.AxisX.LabelStyle.ForeColor);
            FontOfXYTitle = new FontSettingOption(chart.AxisX.TitleFont, chart.AxisX.TitleForeColor);
           LogarithmBaseX = new EnableFloat( chart.AxisX.LogarithmBase, chart.AxisX.IsLogarithmic); 
            LogarithmBaseY = new EnableFloat(chart.AxisY.LogarithmBase, chart.AxisY.IsLogarithmic);

            XLabelAngle = new EnableInteger(chart.AxisX.LabelStyle.Angle, false);


            //sub
            Axis2Color = chart.AxisY2.LineColor;
            Axis2Width = new EnableInteger(chart.AxisX2.LineWidth);
            ValueSpanOfX2 = new EnableFloatSpan(chart.AxisX2.Minimum, chart.AxisX2.Maximum);
            ValueSpanOfY2 = new EnableFloatSpan(chart.AxisY2.Minimum, chart.AxisY2.Maximum);
            IntervalOfX2 = new EnableFloat(chart.AxisX2.Interval);
            IntervalOfY2 = new EnableFloat(chart.AxisY2.Interval);
            TitleOfX2 = new EnableString("历元");
            TitleOfY2 = new EnableString("偏差(m)");
            IsShowGridX2 = chart.AxisX2.MajorGrid.Enabled;
            IsShowGridY2 = chart.AxisY2.MajorGrid.Enabled;
            IsStartedFromZeroOfX2 = chart.AxisX2.IsStartedFromZero;
            IsStartedFromZeroOfY2 = chart.AxisY2.IsStartedFromZero;
            AxisX2LableFormat = new EnableString(chart.AxisX2.LabelStyle.Format);
            AxisY2LableFormat = new EnableString(chart.AxisY2.LabelStyle.Format);
            FontOfAxies2Label = new FontSettingOption(chart.AxisX2.LabelStyle.Font, chart.AxisX2.LabelStyle.ForeColor);
            FontOfXY2Title = new FontSettingOption(chart.AxisX2.TitleFont, chart.AxisX2.TitleForeColor);

            LogarithmBaseX2 = new EnableFloat(chart.AxisX2.LogarithmBase, chart.AxisX2.IsLogarithmic);
            LogarithmBaseY2 = new EnableFloat(chart.AxisY2.LogarithmBase, chart.AxisY2.IsLogarithmic); 

            //legend
            LegendTableStyle = legend.TableStyle;
            LegendStyle = legend.LegendStyle;
            LegendDocking = legend.Docking;
            LegnedX = legend.Position.X;
            LegnedY = legend.Position.Y;
            LegendSize = legend.Position.Size;
            LengendFont = new FontSettingOption(legend.Font, legend.ForeColor);
        }


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ChartSettingOption()
        {
            FontOfXYTitle = new FontSettingOption();
            FontOfXY2Title = new FontSettingOption();
            SeriesChartType = SeriesChartType.Point;
            XLabelAngle = new EnableInteger(0, false);
            this.BorderWidth = 5;
            this.MarkerSize = 5;
            LegendTableStyle = LegendTableStyle.Wide;
            this.SeriesSettingOptions = new Dictionary<string, SeriesSettingOption>();
        }
        /// <summary>
        /// 序列选项
        /// </summary>
        public Dictionary<string, SeriesSettingOption> SeriesSettingOptions { get; set; }
        /// <summary>
        /// X 标签的方向。
        /// </summary> 
        public EnableInteger XLabelAngle { get; internal set; }
        /// <summary>
        /// Y轴标题
        /// </summary>
        public EnableString TitleOfY { get; set; }
        /// <summary>
        /// X轴标题
        /// </summary>
        public EnableString TitleOfX { get; set; }
        /// <summary>
        /// XY轴字体
        /// </summary>
        public FontSettingOption FontOfXYTitle { get; set; }

        /// <summary>
        /// XY轴字体
        /// </summary>
        public FontSettingOption FontOfXY2Title { get; set; }
        /// <summary>
        /// 图标题字体
        /// </summary>
        public FontSettingOption FontOfTitle { get; set; }
        /// <summary>
        /// 是否启用第二轴图
        /// </summary>
        public bool EnableSubAxis { get; set; }
        /// <summary>
        /// 是否显示Y轴格网
        /// </summary>
        public bool IsShowGridX { get; internal set; }
        /// <summary>
        /// Y 轴范围
        /// </summary>
        public EnableFloatSpan ValueSpanOfY { get; internal set; }

        /// <summary>
        /// 轴宽
        /// </summary>
        public EnableInteger AxisWidth { get; internal set; }
        /// <summary>
        /// 标题
        /// </summary>
        public EnableString Title { get;  set; }

        /// <summary>
        /// 轴颜色
        /// </summary>
        public Color AxisColor { get; set; }
        /// <summary>
        /// Y轴间隔
        /// </summary>
        public EnableFloat IntervalOfY { get; internal set; }

        public EnableString AxisXLableFormat { get; set; }
        public EnableString AxisYLableFormat { get; set; }
        public EnableString AxisX2LableFormat { get; set; }
        public EnableString AxisY2LableFormat { get; set; }

        public bool IsShowGridY { get; internal set; }
        public bool EnableLegnedSize { get; internal set; }
        public SeriesChartType SeriesChartType { get; internal set; }
        public bool IsEnableSeriesChartType { get; internal set; }

        public int MarkerSize { get; set; }
        public int BorderWidth { get; set; }
        public LegendTableStyle LegendTableStyle { get; internal set; }
        public float LegnedX { get; internal set; }
        public float LegnedY { get; internal set; }
        public bool EnableLegendPostion { get; internal set; }
        public EnableFloatSpan ValueSpanOfX { get; internal set; }
        public EnableFloat IntervalOfX { get; internal set; }
        public LegendStyle LegendStyle { get; internal set; }
        public Docking LegendDocking { get; internal set; }
        public SizeF LegendSize { get; set; }
        public FontSettingOption LengendFont { get; internal set; }
        public bool IsStartedFromZeroOfX { get; internal set; }
        public bool IsStartedFromZeroOfY { get; internal set; }
        public Color Axis2Color { get; internal set; }
        public EnableInteger Axis2Width { get; internal set; }
        public EnableFloatSpan ValueSpanOfX2 { get; internal set; }
        public EnableFloatSpan ValueSpanOfY2 { get; internal set; }
        public EnableFloat IntervalOfX2 { get; internal set; }
        public EnableFloat IntervalOfY2 { get; internal set; }
        public EnableString TitleOfX2 { get; internal set; }
        public EnableString TitleOfY2 { get; internal set; }
        public bool IsShowGridX2 { get; internal set; }
        public bool IsShowGridY2 { get; internal set; }
        public bool IsStartedFromZeroOfY2 { get; internal set; }
        public bool IsStartedFromZeroOfX2 { get; internal set; }
        public FontSettingOption FontOfAxiesLabel { get; internal set; }
        public FontSettingOption FontOfAxies2Label { get;   set; }
        public Docking TitleDocking { get;   set; }


        public EnableFloat LogarithmBaseX { get; set; }
        public EnableFloat LogarithmBaseY { get; set; }
        public EnableFloat LogarithmBaseX2 { get; set; }
        public EnableFloat LogarithmBaseY2 { get; set; }
         

        /// <summary>
        /// 应用样式到绘图
        /// </summary>
        /// <param name="chart1"></param>
        internal void ApplyOptionFormat(Chart chart1)
        {
            ApplyOptionFormat(chart1, this);
        }

        /// <summary>
        /// 应用样式到绘图
        /// </summary>
        /// <param name="chart1"></param>
        /// <param name="Option"></param>
        public static void ApplyOptionFormat(Chart chart1, ChartSettingOption Option)
        {
            chart1.BeginInit();
            var chart = chart1.ChartAreas[0];

            chart.AxisX.IsLogarithmic = Option.LogarithmBaseX.Enabled;
            chart.AxisX.LogarithmBase = Option.LogarithmBaseX.Value;
            chart.AxisY.IsLogarithmic = Option.LogarithmBaseY.Enabled;
            chart.AxisY.LogarithmBase = Option.LogarithmBaseY.Value;

            chart.AxisX2.IsLogarithmic = Option.LogarithmBaseX2.Enabled;
            chart.AxisX2.LogarithmBase = Option.LogarithmBaseX2.Value;
            chart.AxisY2.IsLogarithmic = Option.LogarithmBaseY2.Enabled;
            chart.AxisY2.LogarithmBase = Option.LogarithmBaseY2.Value;
             

            if (Option.Title.Enabled)
            {
                Title title = new Title(Option.Title.Value, Option.TitleDocking);
                if(chart1.Titles.Count == 0)
                {
                    chart1.Titles.Add(title);
                }
                else
                {
                    chart1.Titles[0] =(title);
                }

                if (Option.FontOfXYTitle != null)
                {
                    title.Font = Option.FontOfXYTitle.Font;
                    title.ForeColor =  Option.FontOfXYTitle.Color;
                }
                title.ForeColor = Option.FontOfTitle.Color;
                title.Font = Option.FontOfTitle.Font;
            }

            //主轴
            if (Option.TitleOfX.Enabled)
            {
                chart.AxisX.Title = Option.TitleOfX.Value;
                if (Option.FontOfXYTitle != null)
                {
                    chart.AxisX.TitleFont = Option.FontOfXYTitle.Font;
                    chart.AxisX.TitleForeColor = Option.FontOfXYTitle.Color;
                }
            }
            if (Option.TitleOfY.Enabled)
            {
                chart.AxisY.Title = Option.TitleOfY.Value;
                if (Option.FontOfXYTitle != null)
                {
                    chart.AxisY.TitleFont = Option.FontOfXYTitle.Font;
                    chart.AxisY.TitleForeColor = Option.FontOfXYTitle.Color;
                }
            }

            if (Option.XLabelAngle.Enabled)
            {
                chart.AxisX.LabelStyle.Angle = Option.XLabelAngle.Value;
            }

            chart.AxisX.MajorGrid.Enabled = Option.IsShowGridX;
            chart.AxisY.MajorGrid.Enabled = Option.IsShowGridY;

            if (Option.IntervalOfY.Enabled)
            {
                chart.AxisY.Interval = Option.IntervalOfY.Value;
            }
            if (Option.ValueSpanOfY.Enabled)
            {
                chart.AxisY.Maximum = Option.ValueSpanOfY.Value.End;
                chart.AxisY.Minimum = Option.ValueSpanOfY.Value.Start;
                chart.AxisY.ScaleView.ZoomReset(); 
            }

            if (Option.IntervalOfX.Enabled)
            {
                chart.AxisX.Interval = Option.IntervalOfX.Value;
            }
            if (Option.ValueSpanOfX.Enabled)
            {
                chart.AxisX.Maximum = Option.ValueSpanOfX.Value.End;
                chart.AxisX.Minimum = Option.ValueSpanOfX.Value.Start;
                chart.AxisX.ScaleView.ZoomReset();
            }
            if (Option.AxisWidth.Enabled)
            {
                chart.AxisX.LineWidth = Option.AxisWidth.Value;
                chart.AxisX.LineColor = Option.AxisColor;
                chart.AxisY.LineWidth = Option.AxisWidth.Value;
                chart.AxisY.LineColor = Option.AxisColor;
            }
            chart.AxisX.IsStartedFromZero = Option.IsStartedFromZeroOfX;
            chart.AxisY.IsStartedFromZero = Option.IsStartedFromZeroOfY;

            if (Option.AxisXLableFormat.Enabled)
            {
                chart.AxisX.LabelStyle.Format = Option.AxisXLableFormat.Value;
            }

            if (Option.AxisYLableFormat.Enabled)
            {
                chart.AxisY.LabelStyle.Format = Option.AxisYLableFormat.Value;
            }
            chart.AxisX.LabelStyle.Font = Option.FontOfAxiesLabel.Font;
            chart.AxisY.LabelStyle.Font = Option.FontOfAxiesLabel.Font;
            chart.AxisX.LabelStyle.ForeColor = Option.FontOfAxiesLabel.Color;
            chart.AxisY.LabelStyle.ForeColor = Option.FontOfAxiesLabel.Color;


            //副轴
            if (Option.EnableSubAxis)
            {
                if (Option.TitleOfX2.Enabled)
                {
                    chart.AxisX2.Title = Option.TitleOfX2.Value;
                    if (Option.FontOfXY2Title != null)
                    {
                        chart.AxisX2.TitleFont = Option.FontOfXY2Title.Font;
                        chart.AxisX2.TitleForeColor = Option.FontOfXY2Title.Color;
                    }
                }
                if (Option.TitleOfY2.Enabled)
                {
                    chart.AxisY2.Title = Option.TitleOfY2.Value;
                    if (Option.FontOfXY2Title != null)
                    {
                        chart.AxisY2.TitleFont = Option.FontOfXY2Title.Font;
                        chart.AxisY2.TitleForeColor = Option.FontOfXY2Title.Color;
                    }
                }

                chart.AxisX2.MajorGrid.Enabled = Option.IsShowGridX2;
                chart.AxisY2.MajorGrid.Enabled = Option.IsShowGridY2;

                if (Option.IntervalOfY2.Enabled)
                {
                    chart.AxisY2.Interval = Option.IntervalOfY2.Value;
                }
                if (Option.ValueSpanOfY2.Enabled)
                {
                    chart.AxisY2.Maximum = Option.ValueSpanOfY2.Value.End;
                    chart.AxisY2.Minimum = Option.ValueSpanOfY2.Value.Start;
                }

                if (Option.IntervalOfX2.Enabled)
                {
                    chart.AxisX2.Interval = Option.IntervalOfX2.Value;
                }
                if (Option.ValueSpanOfX2.Enabled)
                {
                    chart.AxisX.Maximum = Option.ValueSpanOfX2.Value.End;
                    chart.AxisX.Minimum = Option.ValueSpanOfX2.Value.Start;
                }
                if (Option.Axis2Width.Enabled)
                {
                    chart.AxisX2.LineWidth = Option.Axis2Width.Value;
                    chart.AxisX2.LineColor = Option.Axis2Color;
                    chart.AxisY2.LineWidth = Option.Axis2Width.Value;
                    chart.AxisY2.LineColor = Option.Axis2Color;
                }
                chart.AxisX2.IsStartedFromZero = Option.IsStartedFromZeroOfX2;
                chart.AxisY2.IsStartedFromZero = Option.IsStartedFromZeroOfY2;

                if (Option.AxisX2LableFormat.Enabled)
                {
                    chart.AxisX2.LabelStyle.Format = Option.AxisX2LableFormat.Value;
                }

                if (Option.AxisY2LableFormat.Enabled)
                {
                    chart.AxisY2.LabelStyle.Format = Option.AxisY2LableFormat.Value;
                } 

                chart.AxisX2.LabelStyle.Font = Option.FontOfAxies2Label.Font;
                chart.AxisY2.LabelStyle.Font = Option.FontOfAxies2Label.Font;
                chart.AxisX2.LabelStyle.ForeColor = Option.FontOfAxies2Label.Color;
                chart.AxisY2.LabelStyle.ForeColor = Option.FontOfAxies2Label.Color;
            }

            //图列
            var legend = chart1.Legends[0];
            legend.TableStyle = Option.LegendTableStyle;
            legend.LegendStyle = Option.LegendStyle;
            legend.Docking = Option.LegendDocking;
            if (Option.EnableLegnedSize)
            {
                legend.Position.Width = Option.LegendSize.Width;
                legend.Position.Height = Option.LegendSize.Height;
            }
            if (Option.EnableLegendPostion)
            {
                legend.Position.X = Option.LegnedX;
                legend.Position.Y = Option.LegnedY;

            }

            legend.Font = Option.LengendFont.Font;
            legend.ForeColor = Option.LengendFont.Color;

            //各个序列设置

            //各序列，先统一设定一次
            if (Option.IsEnableSeriesChartType)
            {
                foreach (var item in chart1.Series)
                {
                    item.ChartType = Option.SeriesChartType;
                    item.MarkerSize = Option.MarkerSize;
                    item.BorderWidth = Option.BorderWidth;
                }
            }
            //如果有变化，则继续设置
            foreach (var item in chart1.Series)
            {
                if (Option.SeriesSettingOptions.ContainsKey(item.Name))
                {
                    var opt = Option.SeriesSettingOptions[item.Name];
                    item.MarkerSize = opt.MarkerSize;
                    item.Color = opt.Color;
                    item.ChartType = opt.SeriesChartType;
                    item.BorderWidth = opt.BorderWidth;
                    item.XValueType = opt.XValueType;
                    item.YValueType = opt.YValueType;
                    item.XAxisType = opt.AxisTypeOfX;
                    item.YAxisType = opt.AxisTypeOfY;
                }
            }

            chart1.EndInit();
        }
    }
}
