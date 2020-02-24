using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Geo.Times; 

namespace Gnsser.Winform
{
    public partial class BuildtBaseLineStakForm : Form
    {
        public BuildtBaseLineStakForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ID编号
        /// </summary>
        public static int BasicId = 0;
        private void button_build_Click(object sender, EventArgs e)
        {
            List<Task> tasks = new List<Task>();
            char[] spliter = new char[] { ' ', ',', '-', ':' };
            foreach (var item in this.textBox_baseLines.Lines)
            {
                Task task = new Task();
                task.Sites = new List<Site>();

                task.Id = ++BasicId;
                task.Name = "BaseLine_" + BasicId;
                task.ResultFtp = this.textBox_resultFtp.Text.Trim();
                task.Time = new Time(this.dateTimePicker1.Value);
                task.OperationName ="RNX2SNX"; 
                task.Campaign = this.textBox_campaign.Text.Trim();
                string[] sites = item.Split(spliter, StringSplitOptions.RemoveEmptyEntries);
                foreach (var site in sites)
                {
                    task.Sites.Add(new Site() { Name = site });
                }

                tasks.Add(task);
            }

            TaskMgr mgr = new TaskMgr();
            mgr.AddRange(tasks);
            mgr.Save();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            this.textBox_baseLines.Lines = File.ReadAllLines(this.textBox_baseLinePath.Text);
        }

        private void button_setBaseLinePath_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_baseLinePath.Text = openFileDialog1.FileName;
        }

        private void button_readPointSite_Click(object sender, EventArgs e)
        {
            this.textBox_baseLines.Lines = File.ReadAllLines(this.textBox_SitePPPPath.Text);
        }

        private void button_SetPointSitePath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_SitePPPPath.Text = openFileDialog1.FileName;
        }

        private void button_ppp_Click(object sender, EventArgs e)
        {
            List<Task> tasks = new List<Task>();
            char[] spliter = new char[] { ' ', ',', '-', ':' };
            foreach (var item in this.textBox_baseLines.Lines)
            {
                Task task = new Task();
                task.Sites = new List<Site>();

                task.Id = ++BasicId;
                task.Name = "PPP_" + BasicId;
                task.ResultFtp = this.textBox_resultFtp.Text.Trim();
                task.Time = new Time(this.dateTimePicker1.Value);
                task.OperationName = Interoperation.Bernese.PcfName.PPP;
                task.Campaign = this.textBox_campaign.Text.Trim();
                string[] sites = item.Split(spliter, StringSplitOptions.RemoveEmptyEntries);
                foreach (var site in sites)
                {
                    task.Sites.Add(new Site() { Name = site });
                }

                tasks.Add(task);
            }

            TaskMgr mgr = new TaskMgr();
            mgr.AddRange(tasks);
            mgr.Save();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
