//2016.10.28, czs,  create in hongqing, 可以并行计算的观测数据流

using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.IO;
using Geo.Referencing;
using Geo.Times;
using Geo.Utils;
using Gnsser;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using  System.Threading.Tasks;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gnsser.Winform
{
    /// <summary>
    /// 可以并行计算的观测数据流。
    /// </summary>
    public partial class ParalleledFileForm :  MultiFileStreamerForm
    {
        Log log = new Log(typeof(ParalleledFileForm));

        public ParalleledFileForm()
        {
            InitializeComponent(); 
        }

        #region 属性
         
        #endregion

        #region 方法 

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="inputPathes"></param>
        protected override void Run(string[] inputPathes)
        { 
            #region 一些设置 
            this.ProgressBar.InitProcess(inputPathes.Length);
            #endregion
            //计算
            if (ParallelConfig.EnableParallel)//并行计算
            {
                ShowInfo("开始并行计算"); 

                ParallelRunning(inputPathes);
            }
            else//串行计算
            {
                ShowInfo("开始串行计算");
                foreach (var inputPath in inputPathes)
                {
                    if (IsCancel || this.backgroundWorker1.CancellationPending) { break; }

                    log.Info("处理\t" + inputPath);
                    Run(inputPath);
                }
            }
        }
        /// <summary>
        /// 并行
        /// </summary>
        /// <param name="inputPathes"></param>
        protected void ParallelRunning(string[] inputPathes)
        {
            this.ProgressBar.InitProcess(inputPathes.Length);
            this.ProgressBar.ShowInfo("正在计算！");

            Parallel.ForEach(inputPathes, this.ParallelConfig.ParallelOptions, (inputPath, state) =>
            {
                if (IsCancel || this.backgroundWorker1.CancellationPending) { state.Stop(); }

                log.Info("处理\t" + inputPath);
                Run(inputPath);
            });
        }

        /// <summary>
        /// 执行单个,判断是否取消，更新进度条。
        /// </summary>
        /// <param name="inputPath"></param>
        protected override void Run(string inputPath)
        {
            if (this.IsCancel) { return; }

            this.ProgressBar.PerformProcessStep(); 
        }


        private void IntegralGnssFileSolveForm_Load(object sender, EventArgs e)
        { 
        }
        #endregion
    }
}