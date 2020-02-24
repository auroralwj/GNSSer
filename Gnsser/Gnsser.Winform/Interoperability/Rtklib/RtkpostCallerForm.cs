using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Geo.IO;
using System.Windows.Forms;
using Gnsser.Interoperation;

namespace Gnsser.Winform
{
    public partial class RtkpostCallerForm : Form
    {
        public RtkpostCallerForm()
        {
            InitializeComponent();
            this.RtkExe = new RtkpostExecuter(Setting.GnsserConfig.RtklibPostExe);

            this.textBox_obs.Text = Setting.GnsserConfig.SampleOFileA;
            this.textBox_nav.Text = Setting.GnsserConfig.SampleNFile; 
        }

        /// <summary>
        /// RTK 执行器
        /// </summary>
        RtkpostExecuter RtkExe { get; set; }


        private void button_obsPathSet_Click(object sender, EventArgs e) { if (openFileDialog_obs.ShowDialog() == System.Windows.Forms.DialogResult.OK) { this.textBox_obs.Text = openFileDialog_obs.FileName; }  }

        private void button_setNavPath_Click(object sender, EventArgs e) { if (openFileDialog_nav.ShowDialog() == System.Windows.Forms.DialogResult.OK) { this.textBox_nav.Text = openFileDialog_nav.FileName; } }

        private void button_caculate_Click(object sender, EventArgs e)
        {
            RtkpostOption option = new RtkpostOption()
            {
                RtklibType = Interoperation.RtklibType.Post
            };

            option.AddRoverObsFilePath(this.textBox_obs.Text);
            option.NavigationPath = this.textBox_nav.Text;
            option.ConfigPath = Setting.GnsserConfig.RtklibPostConfig;

            if(File.Exists(this.textBox_sp3.Text))  option.Sp3Path = this.textBox_sp3.Text;
            if(File.Exists(this.textBox_clk.Text))   option.ClkPath = this.textBox_clk.Text;

            List<string> result = RtkExe.Run(option);

            this.richTextBoxControl1.Text = result[0];
            this.richTextBoxControl2.Text = result[result.Count - 1];
        }

        private void button_readConfig_Click(object sender, EventArgs e)
        {
            Geo.WinTools.ConfigFileEditForm form = new Geo.WinTools.ConfigFileEditForm(Setting.GnsserConfig.RtklibPostConfig);
            form.ShowDialog();
        }

        private void button_setclk_Click(object sender, EventArgs e) { if (openFileDialog_clk.ShowDialog() == System.Windows.Forms.DialogResult.OK) { this.textBox_clk.Text = openFileDialog_clk.FileName; } }


        private void button_setsp3_Click(object sender, EventArgs e) { if (openFileDialog_nav.ShowDialog() == System.Windows.Forms.DialogResult.OK) { this.textBox_sp3.Text = openFileDialog_nav.FileName; } }

    }
}
