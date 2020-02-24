//2016.12.01, czs, create in hongqing, 绘制坐标
//2017.04.26, czs, edit in hongqing, 增加绘图接口
//2018.12.27, czs, edit in ryd, 增加时段绘制构造函数

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Times;

namespace Geo.Draw
{
    /// <summary>
    /// 绘图
    /// </summary>
    public partial class EpochChartForm : Form
    {
        public EpochChartForm()
        {
            InitializeComponent();
        }
        /// <summary>
        ///绘制时段图
        /// </summary>
        /// <param name="timeperiods"></param>
        /// <param name="name"></param>
        public EpochChartForm(Dictionary<string, List<TimePeriod>> timeperiods, string name = "时段图")
        {
            InitializeComponent();

            Init(timeperiods, name);
        }
        /// <summary>
        ///绘制时段图
        /// </summary>
        /// <param name="timeperiods"></param>
        /// <param name="name"></param>
        public EpochChartForm(Dictionary<string, TimePeriod> timeperiods, string name= "时段图")//: this()
        {
            InitializeComponent();
            Dictionary<string, List<TimePeriod>> timeps = new Dictionary<string, List<TimePeriod>>();
            foreach (var item in timeperiods)
            {
                timeps[item.Key] = new List<TimePeriod>() { item.Value };
            }
            Init(timeps, name); 
        }
        #region 时段初始化
        private void Init(Dictionary<string, List<TimePeriod>> timeperiods, string name)
        {
            ObjectTableStorage table = EpochChartControl.BuildEpochTable(timeperiods, name);
            this.Text = table.Name;
            Draw(table);
        }
      
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table"></param>
        /// <param name="obsFilePath"></param>
        /// <param name="enableDelete"></param>
        public EpochChartForm(ObjectTableStorage table, string obsFilePath=null, bool enableDelete = false)
        {
            InitializeComponent();
            this.Text = table.Name;
            this.ObsFilePath = obsFilePath;
            this.chartControl1.EnableDelete = enableDelete;
            if (enableDelete)
            {
                this.chartControl1.DataDeleting += ChartControl1_DataDeleting;
            }

            Draw(table);
        }
        /// <summary>
        /// 观测文件路径
        /// </summary>
        public string ObsFilePath { get; set; }


        private void ChartControl1_DataDeleting(Dictionary<string, IntSegment> obj)
        {
            

        }

        public EpochChartForm(DataTable table)
        {
            InitializeComponent();

            Draw(table);
        }
        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="table"></param>
        public void Draw(ObjectTableStorage table)
        {
            this.chartControl1.SetTable(table);
            this.chartControl1.Draw();
        }
        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="table"></param>
        public void Draw(DataTable table)
        {
            this.chartControl1.SetTable(table);
            this.chartControl1.Draw();
        }
         
    }
}
