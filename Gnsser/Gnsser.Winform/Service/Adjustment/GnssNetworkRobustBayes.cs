using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Service;

namespace Gnsser.Winform.Service.Adjustment
{
    public partial class GnssNetworkRobustBayes : Form
    {
        public GnssNetworkRobustBayes()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 读取周解文件,在抗差贝叶斯估计中,周解文件有两个(上一周的周解文件和这一周的周解文件-坐标真值);在最小二乘平差中,周解文件只有一个，即是这一周的周解文件-坐标真值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_getfilepath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_WeekSolutionFilepath.Lines = this.openFileDialog1.FileNames;
            }
        }

        /// <summary>
        /// 读取日解文件,在抗差贝叶斯估计中,周解和日解文件要相邻(上一周的周解文件和这一周的7个日解文件);在最小二乘平差中,周解文件是坐标的真值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_getrijiefilepath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_DailySolutionFilepath.Lines = this.openFileDialog2.FileNames;
            }
        }
        /// <summary>
        /// 最小二乘平差
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_LSAdjustment_Click(object sender, EventArgs e)
        {
            string strKnown_Points = this.textBox_knownpoint.Text;
            if (strKnown_Points == null)
            {
                throw new Exception("请输入固定点号!");
            }

            string[] Known_Points = strKnown_Points.Split(';');//分隔字符
            string[] WS_FilePath = this.textBox_WeekSolutionFilepath.Lines;
            string[] DS_FilePath = this.textBox_DailySolutionFilepath.Lines;
            //double k0 = double.Parse(this.textBox_k0.Text);
            //double k1 = double.Parse(this.textBox_k1.Text);
            //double eps = double.Parse(this.textBox_eps.Text);

            //最小二乘平差
            //输入文件:日解文件
            //平差类型：最小二乘平差
            //输出：跟踪站平差后的坐标
            SinexNetWorkAdjustmentLLY sinex_network_adjustment = new SinexNetWorkAdjustmentLLY(WS_FilePath, DS_FilePath, Known_Points);
        }

        private void button_Robust_Click(object sender, EventArgs e)
        {
            string strKnown_Points = this.textBox_knownpoint.Text;
            if (strKnown_Points == null)
            {
                throw new Exception("请输入固定点号!");
            }
            string[] Known_Points = strKnown_Points.Split(';');

            string[] WS_FilePath = this.textBox_WeekSolutionFilepath.Lines;
            string[] DS_FilePath = this.textBox_DailySolutionFilepath.Lines;
            string[] Priori_WS_FilePath = this.textBox_PrioriWeekSolutionFilePath.Lines;

            double k0 = double.Parse(this.textBox_k0.Text);
            double k1 = double.Parse(this.textBox_k1.Text);
            double eps = double.Parse(this.textBox_eps.Text);

            if ( k0 > k1 && k0 <0 && eps <= 0)
            {
                throw new Exception("抗差条件不足!");
            }           

            //加入粗差率,即是在多少基线里面加入粗差
            double rate = 0.01;
            int time = DS_FilePath.Length;

            SinexNetWorkAdjustmentLLY sinex_network_adjustment = new SinexNetWorkAdjustmentLLY(WS_FilePath, DS_FilePath, Priori_WS_FilePath, Known_Points, k0, k1, eps, rate, time);
        }

        /// <summary>
        /// 贝叶斯估计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Bayes_Click(object sender, EventArgs e)
        {
            string strKnown_Points = this.textBox_knownpoint.Text;
            if (strKnown_Points == null)
            {
                throw new Exception("请输入固定点号!");
            }

            string[] Known_Points = strKnown_Points.Split(';');
            string[] WS_FilePath = this.textBox_WeekSolutionFilepath.Lines;
            string[] DS_FilePath = this.textBox_DailySolutionFilepath.Lines;
            string[] Priori_WS_FilePath = this.textBox_PrioriWeekSolutionFilePath.Lines;

            SinexNetWorkAdjustmentLLY sinex_network_adjustment = new SinexNetWorkAdjustmentLLY(WS_FilePath, DS_FilePath, Priori_WS_FilePath, Known_Points);
        }

        private void button_getprioriweeksolutionfile_Click(object sender, EventArgs e)
        {
            if(this.openFileDialog3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_PrioriWeekSolutionFilePath.Lines = this.openFileDialog3.FileNames;
            }
        }

        private void button_robustbayes_Click(object sender, EventArgs e)
        {
            string strKnown_Points = this.textBox_knownpoint.Text;
            if (strKnown_Points == null)
            {
                throw new Exception("请输入固定点号!");
            }
            string[] Known_Points = strKnown_Points.Split(';');

            string[] WS_FilePath = this.textBox_WeekSolutionFilepath.Lines;
            string[] DS_FilePath = this.textBox_DailySolutionFilepath.Lines;
            string[] Priori_WS_FilePath = this.textBox_PrioriWeekSolutionFilePath.Lines;

            double k0 = double.Parse(this.textBox_k0.Text);
            double k1 = double.Parse(this.textBox_k1.Text);
            double eps = double.Parse(this.textBox_eps.Text);

            if (k0 > k1 && k0 < 0 && eps <= 0)
            {
                throw new Exception("抗差条件不足!");
            }

            //加入粗差率,即是在多少基线里面加入粗差
            double rate = 0.01;
            int time = DS_FilePath.Length;

            SinexNetWorkAdjustmentLLY sinex_network_adjustment = new SinexNetWorkAdjustmentLLY(WS_FilePath, DS_FilePath, Priori_WS_FilePath, Known_Points, k0, k1, eps, rate);
        }
    }
}
