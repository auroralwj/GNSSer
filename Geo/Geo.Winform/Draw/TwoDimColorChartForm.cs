//2017.10.14, czs, create in hongqing, 二维色彩绘图

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Coordinates;

namespace Geo.Draw
{
    /// <summary>
    /// 二维色彩绘图
    /// </summary>
    public partial class TwoDimColorChartForm : Form
    {
        /// <summary>
        /// 二维色彩绘图
        /// </summary>
        public TwoDimColorChartForm()
        {
            InitializeComponent();
        } 
         
        /// <summary>
        /// 二维色彩绘图
        /// </summary>
        /// <param name="table"></param>
        /// <param name="ValSpanForColor"></param>
        public TwoDimColorChartForm(TwoNumeralKeyAndValueDictionary table, NumerialSegment ValSpanForColor= null)
        {
            InitializeComponent();
            this.table = table;

            Draw(table, ValSpanForColor);
        }

        /// <summary>
        /// 二维色彩绘图
        /// </summary>
        /// <param name="coords"></param>
        public TwoDimColorChartForm(List<GeoCoord> coords)
        {
            InitializeComponent();
            this.Coords = coords;
            this.chartControl1.Draw(coords);
        }

        TwoNumeralKeyAndValueDictionary table;
        List<GeoCoord> Coords; 

        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="table"></param>
        /// <param name="ValSpanForColor"></param>
        public void Draw(TwoNumeralKeyAndValueDictionary table,NumerialSegment ValSpanForColor = null)
        { 
            this.chartControl1.Draw(table,ValSpanForColor);
        } 
         
    }
}
