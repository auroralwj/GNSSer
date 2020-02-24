using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Gnsser.Times;
using Gnsser;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using Geo.Algorithm;

namespace Gnsser.Winform.Other
{
    public partial class DeleteData : Form
    {
        public DeleteData()
        {
            InitializeComponent();
        }

        private void button_selectFile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_satNLFile.Text = this.openFileDialog1.FileName;
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            List<StaData> stadata = new List<StaData>();
            using (StreamReader r = new StreamReader(this.textBox_satNLFile.Text))
            {
                string line = null;
                line = r.ReadLine();
                int j = 0;
                while ((line = r.ReadLine()) != null)
                {
                    StaData currentdata = new StaData();
                    List<double> CurrentNL = new List<double>();
                    string[] tmp = SplitByBlank(line);
                    if (tmp.Length == 1)
                    {
                        tmp = SplitByExcelBlank(line);
                    }
                    foreach (var item in tmp)
                    {

                    }

                }

            }
        }
        private class StaData
        {
            public string staname;
            public List<Time> time;
            public List<double> SatNL;
        }
        private static string[] SplitByExcelBlank(string line)
        {
            return line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
        private static string[] SplitByBlank(string line)
        {
            return line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
