using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;

namespace Geo.Winform
{
    public partial class SysTitleSettingForm : Form
    {
        public SysTitleSettingForm()
        {
            InitializeComponent();
            ConfigurationManager.RefreshSection("appSettings");
              this.textBox_title.Text = System.Configuration. ConfigurationManager.AppSettings["Title"];
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //Save to 
            config.AppSettings.Settings["Title"].Value=  this.textBox_title.Text.Trim();
            config.Save(ConfigurationSaveMode.Modified);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
