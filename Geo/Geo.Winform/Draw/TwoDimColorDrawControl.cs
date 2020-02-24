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
    /// 二维颜色绘图
    /// </summary>
    public partial class TwoDimColorDrawControl : UserControl
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TwoDimColorDrawControl()
        {
            InitializeComponent();

            ColorChartDrawer = new Geo.Draw.ColorChartDrawer();
        }

        #region  属性
        /// <summary>
        /// 绘图控件
        /// </summary>
        protected PictureBox PictureBox { get { return pictureBox1; } }
        /// <summary>
        /// 颜色绘制
        /// </summary>
        public ColorChartDrawer ColorChartDrawer { get; set; }
        /// <summary>
        /// 数据范围，以此决定颜色
        /// </summary>
        public NumerialSegment ValSpanForColor { get; set; }
        /// <summary>
        /// 数据。
        /// </summary>
        public TwoNumeralKeyAndValueDictionary TwoNumeralKeyAndValueDictionary { get; set; }
        #endregion

        private void ChartControl_SizeChanged(object sender, EventArgs e)
        {
            if (ColorChartDrawer == null) return;

            this.ColorChartDrawer.Init(this.pictureBox1.Size);
            if (TwoNumeralKeyAndValueDictionary != null)
                this.pictureBox1.Image = this.ColorChartDrawer.Draw(TwoNumeralKeyAndValueDictionary, ValSpanForColor);
            if (coords != null)
                this.pictureBox1.Image = this.ColorChartDrawer.Draw(coords);
        }

        private void 复制图像CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(pictureBox1.Image);
        }

        private void 重置RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChartControl_SizeChanged(null, null);
        }


        List<GeoCoord> coords;
        internal void Draw(List<GeoCoord> coords)
        {
            this.coords = coords;
            this.ColorChartDrawer.Init(this.pictureBox1.Size);
            if (coords != null)
            {
                this.pictureBox1.Image = this.ColorChartDrawer.Draw(coords);
            }
        }

        internal void Draw(TwoNumeralKeyAndValueDictionary table, NumerialSegment ValSpanForColor = null)
        {
            this.TwoNumeralKeyAndValueDictionary = table;
            this.ValSpanForColor = ValSpanForColor;
            this.ColorChartDrawer.Init(this.pictureBox1.Size);
            if (TwoNumeralKeyAndValueDictionary != null)
            {
                this.pictureBox1.Image = this.ColorChartDrawer.Draw(TwoNumeralKeyAndValueDictionary, ValSpanForColor);
            }
        }
    }
}