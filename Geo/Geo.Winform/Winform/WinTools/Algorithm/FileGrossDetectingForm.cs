﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.WinTools
{
    /// <summary>
    /// 文件粗差探测
    /// </summary>
    public class FileGrossDetectingForm : FileExecutingForm 
    {
        private Label label1;
        private TextBox textBox_count;
    /// <summary>
    /// 默认构造函数
    /// </summary>
        public FileGrossDetectingForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public override void Run(string input, string output)
        {
            int count = Int32.Parse(this.textBox_count.Text);

            Geo.Algorithm.Adjust.GrossErrorFinder finder = new Geo.Algorithm.Adjust.GrossErrorFinder(count);


            List<List<Double>> inputCols = this.GetInputDoubleCols();
            List<Dictionary<int, double>> resultCols = new List<Dictionary<int, double>>();
            foreach (var doubles in inputCols)
            {
                Dictionary<int, double> grosses = finder.Find(doubles.ToArray());

                //  List<Double> result = GetMovingAverage(doubles, count);
                resultCols.Add(grosses);
            }

            //写入
            this.TryDeleteOutputFile();

            foreach (var row in resultCols)
            {
                foreach (var item in row)
                {
                    this.AppendLineToOutFile(item.Key + "\t" + item.Value);
                }
            }
            Utils.FormUtil.ShowOkAndOpenFile(this.OutputPath);
        }
        /// <summary>
        /// 滑动平均
        /// </summary>
        /// <param name="doubles">数组</param>
        /// <param name="count">过滤次数</param>
        /// <returns></returns>
        private static List<double> GetMovingAverage(List<Double> doubles, int count)
        {
            List<Double> result = new List<double>();

            //  ChebyshevPolyFit inter = new ChebyshevPolyFit(doubles.ToArray(), 10);
            MovingAverageInterpolater inter = new MovingAverageInterpolater(doubles.ToArray(), count);

            for (int i = 0; i < doubles.Count; i++)
            {
                result.Add(inter.GetY(i));
            }
            return result;
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_count = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(115, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "探测次数：";
            // 
            // textBox_count
            // 
            this.textBox_count.Location = new System.Drawing.Point(203, 159);
            this.textBox_count.Name = "textBox_count";
            this.textBox_count.Size = new System.Drawing.Size(100, 25);
            this.textBox_count.TabIndex = 4;
            this.textBox_count.Text = "3";
            // 
            // GrossDetectingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.ClientSize = new System.Drawing.Size(656, 224);
            this.Controls.Add(this.textBox_count);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "GrossDetectingForm";
            this.Text = "粗差探测";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textBox_count, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    } 






}
