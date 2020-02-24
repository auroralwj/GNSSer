//2016.08.26, czs, edit in hongqing, 数据平滑

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Utils;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;

namespace Geo.WinTools
{
    /// <summary>
    /// 残差分析。
    /// </summary>
    public class DataSmoothForm : DataProcessingForm
    {
        private TextBox textBox_windowSize;
        private Winform.Controls.ErrorRejectControl errorRejectControl1;
        private TabControl tabControl2;
        private TabPage tabPageKalman滤波;
        private TabPage tabPage滑动平均;
        private Label label2;
        private Label label1;
        /// <summary>
        /// 窗口数据
        /// </summary>
        NumeralWindowData NumeralWindowData { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataSmoothForm()
            : base()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 运行
        /// </summary>
        protected override void Run()
        {
            var type = GetDataSmoothType();
            switch (type)
            {
                case DataSmoothType.MovingAverage:
                    MovingAverage();
                    break;
                case DataSmoothType.KalmanFilter:
                    KalmanFilter();
                    break;
                default:
                    break;
            }     
        }

        private void KalmanFilter()
        {
                Adjustment = null;
            if (this.HasIndexColumn)
            {
                Dictionary<double, double> result = new Dictionary<double, double>();
                var dic = IndexedValues;
                foreach (var item in dic)
                {
                    result[item.Key] = DoLevelKalmanFilter(item.Value);
                }
                this.OutputLines = DoubleUtil.ToStringLines(result).ToArray();
                // ShowInfo("粗差：" + grossErrors.Count + " 个\r\n" + DoubleUtil.ToTableString(grossErrors));
            }
            else
            {
                var result = new List<double>();
                var dic = Values;
                foreach (var item in dic)
                {
                    result.Add( DoLevelKalmanFilter(item));
                }
                this.OutputLines = DoubleUtil.ToStringLines(result).ToArray(); 
            }
        } 

        AdjustResultMatrix Adjustment;

        /// <summary>
        /// 水平Kalman滤波
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private double DoLevelKalmanFilter( double val)
        {
            var builder = new OneDimAdjustMatrixBuilder(val, Adjustment);
            var sp = new SimpleKalmanFilter();
            Adjustment = sp.Run(builder); 
            var smoothData = Adjustment.Estimated[0];
            return smoothData;
        }

        /// <summary>
        /// 滑动平均
        /// </summary>
        private void MovingAverage()
        {
            Int32 count = Int32.Parse(this.textBox_windowSize.Text);
            NumeralWindowData = new Geo.NumeralWindowData(count);


            if (this.HasIndexColumn)
            { 
                var dic = IndexedValues; 

                Dictionary<Double, Double> result = new Dictionary<Double, Double>();
                Dictionary<Double, Double> grossErrors = new Dictionary<Double, Double>();
                foreach (var item in dic)
                {
                    if (NumeralWindowData.IsFull)
                    {
                        if (errorRejectControl1.IsEnabled)
                        {
                            var isOvered = NumeralWindowData.IsOverLimited(item.Value, errorRejectControl1.MaxLimit, errorRejectControl1.IsRelative);
                            if (isOvered)
                            {
                                grossErrors.Add(item.Key, item.Value);
                                continue;
                            }
                        }

                        result.Add(item.Key, NumeralWindowData.AverageValue);
                    }

                    NumeralWindowData.Add((int)item.Key, item.Value);
                }
                //输出
                this.OutputLines = DoubleUtil.ToStringLines(result).ToArray();
                ShowInfo("粗差：" + grossErrors.Count + " 个\r\n" + DoubleUtil.ToTableString(grossErrors));
            }
            else
            {
                var doubles = Values;// new List<Double>(Utils.DoubleUtil.ParseLines(this.InputLines));

                List<Double> result = new List<double>();
                List<Double> grossErrors = new List<double>();
                foreach (var item in doubles)
                {
                    if (NumeralWindowData.IsFull)
                    {
                        if (errorRejectControl1.IsEnabled)
                        {
                            var isOvered = NumeralWindowData.IsOverLimited(item, errorRejectControl1.MaxLimit, errorRejectControl1.IsRelative);
                            if (isOvered)
                            {
                                grossErrors.Add(item);
                                continue;
                            }
                        }

                        result.Add(NumeralWindowData.AverageValue);
                    }

                    NumeralWindowData.Add(item);
                }
                //输出
                this.OutputLines = DoubleUtil.ToStringLines(result).ToArray();
                ShowInfo("粗差：" + grossErrors.Count + " 个\r\n" + DoubleUtil.ToColumnString(grossErrors));
            }
        }

        public DataSmoothType GetDataSmoothType()
        {
            switch (this.tabControl2.SelectedTab.Text)
            {
                case "Kalman滤波":
                    return DataSmoothType.KalmanFilter;
                case "滑动平均":
                    return DataSmoothType.MovingAverage;
                default:
                    return DataSmoothType.KalmanFilter;
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_windowSize = new System.Windows.Forms.TextBox();
            this.errorRejectControl1 = new Geo.Winform.Controls.ErrorRejectControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageKalman滤波 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage滑动平均 = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageKalman滤波.SuspendLayout();
            this.tabPage滑动平均.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl2);
            this.panel1.Size = new System.Drawing.Size(198, 298);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "窗口大小(个)：";
            // 
            // textBox_windowSize
            // 
            this.textBox_windowSize.Location = new System.Drawing.Point(113, 14);
            this.textBox_windowSize.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_windowSize.Name = "textBox_windowSize";
            this.textBox_windowSize.Size = new System.Drawing.Size(35, 21);
            this.textBox_windowSize.TabIndex = 2;
            this.textBox_windowSize.Text = "20";
            // 
            // errorRejectControl1
            // 
            this.errorRejectControl1.IsEnabled = false;
            this.errorRejectControl1.Location = new System.Drawing.Point(12, 39);
            this.errorRejectControl1.Margin = new System.Windows.Forms.Padding(2);
            this.errorRejectControl1.MaxLimit = 0.1D;
            this.errorRejectControl1.Name = "errorRejectControl1";
            this.errorRejectControl1.Size = new System.Drawing.Size(171, 111);
            this.errorRejectControl1.TabIndex = 4;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageKalman滤波);
            this.tabControl2.Controls.Add(this.tabPage滑动平均);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(198, 298);
            this.tabControl2.TabIndex = 5;
            // 
            // tabPageKalman滤波
            // 
            this.tabPageKalman滤波.Controls.Add(this.label2);
            this.tabPageKalman滤波.Location = new System.Drawing.Point(4, 22);
            this.tabPageKalman滤波.Name = "tabPageKalman滤波";
            this.tabPageKalman滤波.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageKalman滤波.Size = new System.Drawing.Size(190, 272);
            this.tabPageKalman滤波.TabIndex = 0;
            this.tabPageKalman滤波.Text = "Kalman滤波";
            this.tabPageKalman滤波.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "一维Kalman滤波";
            // 
            // tabPage滑动平均
            // 
            this.tabPage滑动平均.Controls.Add(this.errorRejectControl1);
            this.tabPage滑动平均.Controls.Add(this.textBox_windowSize);
            this.tabPage滑动平均.Controls.Add(this.label1);
            this.tabPage滑动平均.Location = new System.Drawing.Point(4, 22);
            this.tabPage滑动平均.Name = "tabPage滑动平均";
            this.tabPage滑动平均.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage滑动平均.Size = new System.Drawing.Size(190, 260);
            this.tabPage滑动平均.TabIndex = 1;
            this.tabPage滑动平均.Text = "滑动平均";
            this.tabPage滑动平均.UseVisualStyleBackColor = true;
            // 
            // DataSmoothForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(511, 386);
            this.InputLines = new string[0];
            this.Name = "DataSmoothForm";
            this.OutputLines = new string[0];
            this.Text = "数据平滑";
            this.panel1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPageKalman滤波.ResumeLayout(false);
            this.tabPageKalman滤波.PerformLayout();
            this.tabPage滑动平均.ResumeLayout(false);
            this.tabPage滑动平均.PerformLayout();
            this.ResumeLayout(false);

        }
    }

    /// <summary>
    /// 数据平滑类型
    /// </summary>
    public enum DataSmoothType
    {
        /// <summary>
        /// Kalman滤波
        /// </summary>
        KalmanFilter,
        /// <summary>
        /// 滑动平均
        /// </summary>
        MovingAverage,
    }
}