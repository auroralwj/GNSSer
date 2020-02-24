using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace Geo.Winform
{ 
        public partial class MyChart : Chart
        {
            public MyChart()
            {
                this.AxisViewChanged += new EventHandler<ViewEventArgs>(LinkageZoom);
            }
            private bool viewZoomAutoLinkage = false;
            //是否使用联动缩放
            public bool ViewZoomAutoLinkage
            {
                get { return viewZoomAutoLinkage; }
                set { viewZoomAutoLinkage = value; }
            }
            private double viewZoomYFix;
            //联动缩放Y轴时对最大Y和最小Y的修正。修正为YMax+Fix和YMin-Fix。
            //为确保放大后的YMax和YMix不会触及区域顶端/底端，请使用合适的Fix值适当扩大Y轴视图范围
            //例：Y轴间隔为2，使用Fix=1较为合适。
            public double ViewZoomYFix
            {
                get { return viewZoomYFix; }
                set { viewZoomYFix = value; }
            }
            private void ZoomInternal(double xStart, double xEnd, Series singleSeries)
            {
                ChartArea area = this.ChartAreas[singleSeries.ChartArea];
                //放大X轴为start到end区间的部分图表
                area.AxisX.ScaleView.Zoom(xStart, xEnd);
                if (viewZoomAutoLinkage)
                {
                    //计算此视图上所有point的Y值(每个点可能有多个Y值)，得到X值在范围内的所有点的所有Y值
                    //这里的points是List<double[]>结构
                    var points = (from p in singleSeries.Points
                                  where p.XValue <= xEnd && p.XValue >= xStart
                                  select p.YValues).ToList();
                    //展开所有Y值得到最小和最大值。这里我们使用Lambda完成嵌套的Min计算
                    //对每一个Y值集合计算最小值作为该集合的比较值，然后抽取所有集合中的最小值
                    if (points.Any())
                    {
                        double yStart = points.Min<double[]>(yValues => yValues.Min());
                        double yEnd = points.Max<double[]>(yValues => yValues.Max());
                        //放大Y轴到yStart,yEnd范围
                        area.AxisY.ScaleView.Zoom(yStart - viewZoomYFix, yEnd + viewZoomYFix);
                    }
                }
            }
            //唯一个公有方法，对指定范围的X轴区域进行缩放。关联属性会决定是否同时缩放Y轴。使用默认的series参数会对所有序列进行操作，否则只缩放指定的序列。
            public void Zoom(double xStart, double xEnd, Series series = null)
            {
                if (series == null)
                    foreach (Series s in this.Series)
                        ZoomInternal(xStart, xEnd, s);
                else if (this.Series.Contains(series))
                    ZoomInternal(xStart, xEnd, series);
                else
                    throw new Exception("This chart DO NOT contains parameter-specificed series.");
            }
            private void LinkageZoom(object sender, ViewEventArgs vea)
            {
                //如果滚动了X滚动条，应重新计算Y值范围并进行Y放大.这里不考虑第二条X轴（X2）
                if (viewZoomAutoLinkage && vea.Axis.AxisName == AxisName.X)
                {
                    double xStart = vea.ChartArea.AxisX.ScaleView.ViewMinimum;
                    double xEnd = vea.ChartArea.AxisX.ScaleView.ViewMaximum;
                    xStart = vea.NewPosition;
                    xEnd = vea.NewPosition + vea.NewSize;
                    //找出哪些序列绑定了事件参数中的轴所在的Area
                    var sList = this.Series.Where(s => s.ChartArea == vea.ChartArea.Name);
                    if (sList.Any())
                        foreach (var s in sList)
                            Zoom(xStart, xEnd, s);
                }
            }
        }
    
}
