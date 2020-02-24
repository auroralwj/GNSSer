using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Gnsser.Winform.Other
{
    public partial class CombinationWL : Form
    {
        public CombinationWL()
        {
            InitializeComponent();
        }
        List<string> SatWLFiles = new List<string>();//一周的SINEX文件
        Dictionary<string, List<string>> StaWL = new Dictionary<string, List<string>>();
        private void button_selectPath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox_selectPath.Text = this.folderBrowserDialog1.SelectedPath;
            string sourseDir1 = this.textBox_selectPath.Text;
            string[] files = Directory.GetFiles(sourseDir1, "*.txt");
            for (int i = 0; i < files.Length; i++)
            {
                SatWLFiles.Add(files[i]);
            }
        }

        private void button_combination_Click(object sender, EventArgs e)
        {
            foreach (var file in SatWLFiles)
            {
                string tmpStaName = file.Substring(file.Length - 8, 4);
                List<string> tmpstaWL = new List<string>();
                using (StreamReader r = new StreamReader(file))
                {
                    string line = null;
                    int j = 0;
                    while ((line = r.ReadLine()) != null)
                    {
                        string[] tmp = SplitByBlank(line);
                        if (tmp.Length == 1)
                        {
                            tmp = SplitByExcelBlank(line);
                        }
                        tmpstaWL.Add(tmp[1]);                        
                    }
                }
                StaWL.Add(tmpStaName, tmpstaWL);
            }
            string SavePath = "C:\\Users\\lilinyang\\Desktop\\36\\WLFCB\\AllWLFCB"  + ".txt";
            FileInfo cFile = new FileInfo(SavePath);
            StreamWriter SW3 = cFile.CreateText();
            foreach (var item in StaWL)
            {
                SW3.Write(item.Key);
                SW3.Write(" ");
            }
            SW3.Write("\n");
            for (int i = 0; i < 32; i++)
            {
                foreach (var item in StaWL)
                {
                    if(i == 0)
                    {
                        SW3.Write(9999.ToString());
                        SW3.Write(" ");
                        continue;
                    }

                    if(double.Parse(item.Value[i]) == 0)
                    {
                        SW3.Write(9999.ToString());
                    }
                    else
                    {
                        SW3.Write(item.Value[i]);
                    }                    
                    SW3.Write(" ");
                }
                SW3.Write("\n");
            }            
            SW3.Close();
        }
        private static string[] SplitByBlank(string line)
        {
            return line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
        private static string[] SplitByExcelBlank(string line)
        {
            return line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
        
}
