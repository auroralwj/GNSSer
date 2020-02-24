//2016.12.01, czs, create in hongqing, 绘制坐标
//2017.04.26, czs, edit in hongqing, 增加绘图接口
//2018.12.14, czs, edit in hmx, 增加可以编辑O文件

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo;
using Geo.Winform;
using Geo.Draw;
using Gnsser.Data.Rinex;
using Geo.Times;

namespace Gnsser.Winform
{
    /// <summary>
    /// 绘图
    /// </summary>
    public partial class ObsFileChartEditForm : Form
    {
        public ObsFileChartEditForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filePath"></param>
        /// <param name="isTempPath">是否是临时路径，如果是，则不进行提示</param>
        /// <param name="enableDelete"></param>
        public ObsFileChartEditForm(string filePath, bool isTempPath=false, bool isSort = false, bool isDrawAll = false, bool enableDelete = false)
        {
            InitializeComponent();
            this.isDrawAll = isDrawAll;
            this.ObsFile = RinexObsFileReader.Read(filePath);

            this.Text = ObsFile.Header.FileName;
            this.Table = ObsFile.BuildObjectTable(isDrawAll);
            if (isSort)
            {
                Table.ParamNames.Sort();
            }
            this.ObsFilePath = filePath;
            this.chartControl1.EnableDelete = enableDelete;
            this.ObsFile = ObsFile;
            if (enableDelete)
            {
                if (!isTempPath)
                {
                    Geo.Utils.FormUtil.ShowWarningMessageBox("请注意，所做修改将直接替换原文件！注意备份。\r\n" + filePath);
                }
                this.chartControl1.DataDeleting += ChartControl1_DataDeleting;
            }

            Draw(Table);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table"></param>
        /// <param name="savePath"></param>
        /// <param name="isTempPath">是否是临时路径，如果是，则不进行提示</param>
        /// <param name="enableDelete"></param>
        public ObsFileChartEditForm(RinexObsFile ObsFile, string savePath=null, bool isTempPath = false, bool isSort = false, bool isDrawAll =false,  bool enableDelete = false)
        {
            InitializeComponent();
            this.isDrawAll = isDrawAll;
            this.Text = ObsFile.Header.FileName;
            this.Table = ObsFile.BuildObjectTable(isDrawAll) ;
            if (isSort)
            {
                Table.ParamNames.Sort();
            }
            this.ObsFilePath = savePath;
            this.chartControl1.EnableDelete = enableDelete;
            this.ObsFile = ObsFile;
            if (enableDelete)
            {
                if (!isTempPath)
                {
                    Geo.Utils.FormUtil.ShowWarningMessageBox("请注意，修改数据后将保存在" + savePath +
                    "，若是原始文件，请注意备份。");
                }
                this.chartControl1.DataDeleting += ChartControl1_DataDeleting;
            }

            Draw(Table);
        }
        public bool isDrawAll { get; set; }

        /// <summary>
        /// 观测文件路径
        /// </summary>
        public string ObsFilePath { get; set; }

        public RinexObsFile ObsFile { get; set; }
        ObjectTableStorage Table { get; set; }

        private void ChartControl1_DataDeleting(Dictionary<string, IntSegment> obj)
        {
            var segment = obj.First().Value;
            var indexes = Table.GetIndexValues<Time>();
            var startEpoch = indexes[segment.Start];
            var endEpoch = indexes[segment.End];
            var timeperiod = new TimePeriod(startEpoch, endEpoch);
             
            var prns = new List<SatelliteNumber>();
            foreach (var kv in obj)
            {
                prns.Add(SatelliteNumber.Parse(kv.Key));
            }

            ObsFile.Remove(prns, timeperiod);

            RinexObsFileWriter.Write(ObsFile, ObsFilePath);

            this.ObsFile= RinexObsFileReader.Read(ObsFilePath);
            this.Table = ObsFile.BuildObjectTable(isDrawAll);

            Draw(Table);

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
