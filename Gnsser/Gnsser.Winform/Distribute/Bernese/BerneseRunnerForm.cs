using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Interoperation.Bernese;
using Geo.Times; 
namespace Gnsser.Winform
{
    public partial class BerneseRunnerForm : Form
    {
        public BerneseRunnerForm()
        {
            InitializeComponent();

            this.comboBox_solveType.DataSource = null;// Enum.GetNames(typeof(Gnsser.Interoperation.Bernese.PcfName));
           
            bpe = new BerBpe();
            bpe.WinCmd.ExitedOrDisposed += CmdHelper_ProcessExited;
        }
        Interoperation.Bernese.BerBpe bpe =null;
        private void button_run_Click(object sender, EventArgs e)
        {
            try
            {
                string campaign = this.textBox_campaign.Text;
                Time gpsTime = new Time(this.dateTimePicker_date.Value);
                string pcfName = GetPcfName();

                bpe.Init(campaign, gpsTime, checkBox_skip.Checked);

                if (this.checkBox_asyn.Checked)
                    bpe.RunAsyn(pcfName);
                else bpe.Run(pcfName);

                this.button_run.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("请确保已安装Bernese软件。" + ex.Message);
            }
        }

        void CmdHelper_ProcessExited(object sender, EventArgs e)
        {
            this.Invoke(new Action(delegate() { this.button_run.Enabled = true; }));
            ShowInfo("计算完毕！");          
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_viewState_Click(object sender, EventArgs e)
        {
            if (bpe != null)
            {
                string state = bpe.GetRunningState(GetPcfName());
                ShowInfo(state);
            }
        }

        private void ShowInfo(string state)
        {
            this.Invoke(new Action(delegate() { this.textBox_result.Text += DateTime.Now + ":" + state + "\r\n"; }));
        }

        private string GetPcfName()
        {
            return  this.comboBox_solveType.SelectedItem.ToString(); 
        }

    }
}
