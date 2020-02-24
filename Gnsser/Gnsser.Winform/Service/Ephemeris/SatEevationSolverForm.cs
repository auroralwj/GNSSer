//2017.02.12, czs, create in hongqing, 卫星高度角计算器。
//2017.02.16, czs, edit in hongqing, 重构优化

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Gnsser;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using Gnsser.Service;
using Geo.IO;
using Geo;
using Gnsser.Checkers;

namespace Gnsser.Winform
{
    /// <summary>
    /// 卫星高度角计算器
    /// </summary>
    public partial class SatEevationSolverForm : MultiFileStreamerForm
    {
        public SatEevationSolverForm()
        {
            InitializeComponent();
            SetEnableDetailSettingButton(false);
            SetExtendTabPageCount(1,3);
            //SetEnableMultiSysSelection(false);
            //this.SetEnableDetailSettingButton(false);
            this.Option = GnssProcessOption.GetEphemerisSourceOption(); 
        }

        protected override void Run(string[] inputPathes)
        {

            string navFile = fileOpenControl_nav.GetFilePath();


            //输出的采样间隔 
            TimeLooper looper =  timeLoopControl1.GetTimeLooper();
            var builder = new SatElevatoinTableBuilder(inputPathes,this.SatelliteTypes,  looper, namedFloatControl1AngleCut.Value);
            builder.OutputDirectory = this.OutputDirectory;
            builder.ProgressViewer = this.ProgressBar;
            if (File.Exists(navFile))
            {
                var ephService = EphemerisDataSourceFactory.Create(navFile);
                  builder.EphemerisService = ephService;
                log.Info("采用了星历服务 " + ephService);
            }
            var tableMgr = builder.Build();
            if (checkBox_enableStatistics.Checked)
            {
                int angleStep = 10;
                var table = tableMgr.First;
                #region  卫星历元数统计
                var eleTable = new ObjectTableStorage(table.Name + "_不同高度截止角的历元数量统计");
                for (int i = 0; i <= 30; i = i + angleStep)
                {
                    eleTable.NewRow();
                    eleTable.AddItem("Elevation", i);
                    var count = table.GetCount(new Func<double, bool>(val => val >= i));
                    eleTable.AddItem(count);
                    eleTable.EndRow();
                }
                #endregion

                #region 卫星颗数统计
                var rowCountDics = new Dictionary<int, Dictionary<DateTime, int>>();
                for (int i = 0; i <= 30; i = i + angleStep)
                {
                    rowCountDics[i] = table.GetEachRowCount<DateTime>(new Func<double, bool>(val => val >= i));
                }

                var satCountTable = new ObjectTableStorage(table.Name + "_可用卫星数量统计");
                var first = rowCountDics[0];
                foreach (var item in first)
                {
                    var index = item.Key;
                    satCountTable.NewRow();
                    satCountTable.AddItem("Epoch", index);
                    foreach (var rows in rowCountDics)
                    {
                        var val = rows.Value[index];
                        satCountTable.AddItem(rows.Key, val);
                    }
                    satCountTable.EndRow();
                }
                #endregion

                #region 卫星连续历元数统计
                var maxValTable = new ObjectTableStorage(table.Name + "_最大数据统计");
                var maxSequentialCountTable = new ObjectTableStorage(table.Name + "_最大连续历元数量统计");
                var minSequentialCountTable = new ObjectTableStorage(table.Name + "_最小连续历元数量统计");
                var satPeriodCountTable = new ObjectTableStorage(table.Name + "_卫星出现次数统计");
                for (int i = 0; i <= 30; i = i + angleStep)
                {
                    maxSequentialCountTable.NewRow();
                    maxSequentialCountTable.AddItem("Elevation", i);

                    minSequentialCountTable.NewRow();
                    minSequentialCountTable.AddItem("Elevation", i);

                    satPeriodCountTable.NewRow();
                    satPeriodCountTable.AddItem("Elevation", i);

                    maxValTable.NewRow();
                    maxValTable.AddItem("Elevation", i);

                    var dic = table.GetSequentialCountOfAllCol<DateTime>(new Func<double, bool>(val => val >= i));
                    foreach (var item in dic)
                    {
                        item.Value.Sort(new Comparison<Segment<DateTime>>((one, two) => (int)((two.End - two.Start) - (one.End - one.Start)).TotalSeconds));
                        //最大时段
                        var max = item.Value[0];
                        var maxSpan = (max.End - max.Start).TotalHours;
                        maxSequentialCountTable.AddItem(item.Key, maxSpan);
                        
                        //最大值
                        var maxval = table.GetMaxValue(item.Key, max.Start, max.End);
                        maxValTable.AddItem(item.Key, maxval);


                        //最小时段
                        var lastIndex = item.Value.Count - 1;
                        var min = item.Value[lastIndex];
                        var minSpan = (min.End - min.Start).TotalHours;
                        if (lastIndex == 0) { minSpan = 0; }
                        minSequentialCountTable.AddItem(item.Key, minSpan);
                        //时段次数输出
                        satPeriodCountTable.AddItem(item.Key, item.Value.Count);
                    }

                    maxValTable.EndRow();
                    maxSequentialCountTable.EndRow();
                    minSequentialCountTable.EndRow();
                    satPeriodCountTable.EndRow();
                }

                #endregion


                if (checkBox_sortPrnSatistics.Checked)
                {
                    maxValTable.SortColumns();
                    eleTable.SortColumns();
                    maxSequentialCountTable.SortColumns();
                    minSequentialCountTable.SortColumns();
                    satPeriodCountTable.SortColumns();
                }

                tableMgr.Add(maxValTable);
                tableMgr.Add(satCountTable);
                tableMgr.Add(eleTable);
                tableMgr.Add(maxSequentialCountTable);
                tableMgr.Add(minSequentialCountTable);
                tableMgr.Add(satPeriodCountTable);
            }

            BindTableA(tableMgr.First);
  
            tableMgr.WriteAllToFileAndClearBuffer(tableMgr.First);
        }
         


        private void WideLaneSolverForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl_nav.FilePath = Setting.GnsserConfig.NavPath;
          
        }

        private void fileOpenControl1_FilePathSetted(object sender, EventArgs e)
        {
            var file = fileOpenControl_inputPathes.GetFilePath("*.*O");
            if (File.Exists(file))
            {
                var oneHeader = new RinexObsFileReader(file, false).GetHeader();
                this.timeLoopControl1.TimeStepControl.SetValue(oneHeader.Interval);
                this.timeLoopControl1.TimePeriodControl.SetTimePerid(new TimePeriod(oneHeader.StartTime, oneHeader.EndTime));
            }
           
        }

        private void fileOpenControl_nav_FilePathSetted(object sender, EventArgs e)
        {
            //星历检查
            string navFile = fileOpenControl_nav.GetFilePath();
            if (File.Exists(navFile))
            {
                this.Option.EphemerisFilePath = navFile;
                this.Option.IsIndicatingEphemerisFile = true;
            }

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
         
    }

}
