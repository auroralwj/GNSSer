using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.IO;
using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;

namespace Gnsser.Winform
{

    public partial class RinexFileNameForm : Form
    {
        public RinexFileNameForm()
        {
            InitializeComponent();
        }

        private void button_selectFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialog_obs.ShowDialog() == DialogResult.OK)
                this.textBox_fileNames.Lines = this.openFileDialog_obs.FileNames;
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            string[] fileNames = this.textBox_fileNames.Lines;
            StringBuilder sb = new StringBuilder();

            List<Data.Rinex.FileName> okList = new List<FileName>();
            List<string> badList = new List<string>();

            foreach (var item in fileNames)
            {
                try
                {
                    Data.Rinex.FileName fileName = Data.Rinex.FileName.Parse(Path.GetFileName(item)); 
                    okList.Add(fileName);
                }
                catch
                {
                    badList.Add(item);
                }
            }



            if (this.checkBox_checkName.Checked)
            {
                sb.AppendLine("------------------bad file Names------------------------------");
                foreach (var item in badList)
                {                    
                sb.AppendLine(item);
                }
            } 

            if (this.checkBox_shownameInfo.Checked)
            {
                sb.AppendLine("------------------ok file Names------------------------------");
                foreach (var item in okList)
                {
                    sb.AppendLine(item.Info);
                }
            }

            this.textBox_output.Text = sb.ToString();
        }
    }
}