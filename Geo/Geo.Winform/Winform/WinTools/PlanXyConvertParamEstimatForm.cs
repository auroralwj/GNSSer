//2019.01.12, czs, create in hmx, 平面坐标转换参数估计

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Common;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Times;


namespace Geo.WinTools
{
    /// <summary>
    /// 平面坐标转换参数估计
    /// </summary>
    public partial class PlanXyConvertParamEstimatForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PlanXyConvertParamEstimatForm()
        {
            InitializeComponent();
          //  this.namedXyControl1.SetValue("-79.6622372344136, 20.0414167642593");
        }
        private void GeoXyzConvertForm_Load(object sender, EventArgs e)
        {
        }
        bool IsOutSplitByTab => checkBox_IsOutSplitByTab.Checked;
        /// <summary>
        /// 参数
        /// </summary>
        /// <returns></returns>
        public PlainXyConvertParam GetParam()
        {
            return plainXyTransParamControl1.GetValue(); 
        }
        /// <summary>
        /// 参数
        /// </summary>
        /// <returns></returns>
        public void SetParam(PlainXyConvertParam  convertParam)
        {
            plainXyTransParamControl1.SetValue(convertParam);  
        }

        private void button_oldToNew_Click(object sender, EventArgs e)
        {
            try
            {
                var olds = GetOlds();
                PlanXyConverter planXyConverter = new PlanXyConverter(GetParam());

                var news = planXyConverter.Convert(olds);

                SetNews(news); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_newToOld_Click(object sender, EventArgs e)
        {
            try
            { 
                var news = GetNews();
                PlanXyConverter planXyConverter = new PlanXyConverter(GetParam());

                var old = planXyConverter.Convert(news);

                SetOlds(old);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
         

        private void button_solveParams_Click(object sender, EventArgs e)
        {
            var olds = GetOlds();
            var news = GetNews();

            var planXyConvertParamEstimator = new PlainXyConvertParamEstimator(olds, news);

            var param = planXyConvertParamEstimator.Estimate();
            this.SetParam(param);
        }
        private void button_exchange_Click(object sender, EventArgs e)
        {
            var temp = this.textBox_news.Text;
            this.textBox_news.Text = this.textBox_olds.Text;
            this.textBox_olds.Text = temp;
        }

        #region 界面交互方法
        private List<XY> GetOlds()
        {
            return GetXys(textBox_olds);
        }
        private List<XY> GetNews()
        {
            return GetXys(textBox_news);
        }
        private List<XY> GetXys(TextBoxBase TextBoxBase)
        {
            List<XY> xys = new List<XY>();
            foreach (var item in TextBoxBase.Lines)
            {
                if (item == null || String.IsNullOrWhiteSpace(item.Trim())) { continue; }
                xys.Add(XY.Parse(item));
            }
            return xys;
        }
        public void SetOlds(List<XY> xys)
        {
            SetCoords(xys, this.textBox_olds);
        }
        public void SetNews(List<XY> xys)
        {
            SetCoords(xys, this.textBox_news);
        }
        public void SetCoords(List<XY> xys, TextBoxBase TextBoxBase)
        {
            StringBuilder sb = new StringBuilder();
            var spliter = IsOutSplitByTab ? "\t" : ", ";
            foreach (var item in xys)
            {
                sb.AppendLine(item.ToString("0.000000", spliter));
            }
            TextBoxBase.Text = sb.ToString();
        }
        #endregion

    }
}
