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
using Gnsser.Data;

namespace Gnsser.Winform
{

    public partial class AntennaViewForm : Form
    {
        public AntennaViewForm()
        {
            InitializeComponent();
        }

        private void button_selectFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialog_atx.ShowDialog() == DialogResult.OK)
                this.textBox_fileNames.Text = this.openFileDialog_atx.FileName;
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            string fileName = this.textBox_fileNames.Text;

            AntennaFile file = AntexReader.ReadFile(fileName);
          //  AntexReader obsFile = new AntexReader(fileName);
           

            StringBuilder sb = new StringBuilder();   
            foreach (var item in file.Antennas)
            {
                sb.AppendLine(item.ToString());
            } 

            this.textBox_output.Text = sb.ToString();
        }

        private void AntennaViewForm_Load(object sender, EventArgs e)
        {
            this.textBox_fileNames.Text = Application.StartupPath + System.Configuration.ConfigurationManager.AppSettings["AntennaFile"];
        }
    }
}