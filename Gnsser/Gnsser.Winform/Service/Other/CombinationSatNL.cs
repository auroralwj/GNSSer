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
    public partial class CombinationSatNL : Form
    {
        public CombinationSatNL()
        {
            InitializeComponent();
        }
        List<string> SatNLFiles = new List<string>();//一周的SINEX文件
        Dictionary<SatelliteNumber, List<StaData>> ALLSatNL = new Dictionary<SatelliteNumber, List<StaData>>();
        List<SatelliteNumber> totalsats = new List<SatelliteNumber>();
        private void button_SelectDir_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox_dir.Text = this.folderBrowserDialog1.SelectedPath;
            string sourseDir1 = this.textBox_dir.Text;
            string[] files = Directory.GetFiles(sourseDir1, "*.txt");
            for (int i = 0; i < files.Length; i++)
            {
                SatNLFiles.Add(files[i]);
            }
        }

        private void button_SelectOutputPath_Click(object sender, EventArgs e)
        {
            if(this.folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox_OutputDir.Text = this.folderBrowserDialog2.SelectedPath;
            }
        }

        private void button_extract_Click(object sender, EventArgs e)
        {
            
            for (int i = 1; i <= 32; i++)
            {
                totalsats.Add(SatelliteNumber.Parse(i.ToString()));
            }
            Dictionary<string, List<Data>> AllStaNL = new Dictionary<string, List<Data>>();

            progressBar1.Maximum = SatNLFiles.Count + totalsats.Count * 2;
            progressBar1.Minimum = 1;
            progressBar1.Step = 1;
            progressBar1.Value = progressBar1.Minimum;

            foreach(var file in SatNLFiles)
            {
                List<Data> currentStaNL = new List<Data>();
                using (StreamReader r = new StreamReader(file))
                {
                    string line = null;
                    line = r.ReadLine();                    
                    int j = 0;
                    while ((line = r.ReadLine()) != null)
                    {
                        Data data = new Data();
                        List<double> CurrentNL = new List<double>();
                        string[] tmp = SplitByBlank(line);
                        if(tmp.Length == 1)
                        {
                            tmp = SplitByExcelBlank(line);
                        }
                        data.time = Time.Parse("2013" + " " + "1" + " " + "2" + " " + tmp[1]);//日期，要更改
                        for(int i = 7 ; i< 39; i++)//前6个均和卫星无关
                        {
                            CurrentNL.Add(double.Parse(tmp[i]));
                            if(j == 1145)
                            {
                                j = 1;
                            }
                        }
                        j++;
                        data.NL = CurrentNL;
                        currentStaNL.Add(data);
                    }
                    string Staname = file.Substring(file.Length - 8, 4);
                    AllStaNL.Add(Staname, currentStaNL);
                }
                progressBar1.PerformStep();
                progressBar1.Refresh();
            }
                                       
            foreach(var sat in totalsats)
            {
                int number = sat.PRN;
                List<StaData> staNL = new List<StaData>();
                foreach (var item in AllStaNL)
                {                    
                    StaData data2 = new StaData();
                    List<double> tmpsat = new List<double>();
                    List<Time> tmptime = new List<Time>();
                    foreach (var record in item.Value)
                    {
                        tmpsat.Add(record.NL[number - 1]);
                        tmptime .Add(record.time);
                    }
                    data2.time = tmptime;
                    data2.staname = item.Key;
                    data2.SatNL = tmpsat;
                    staNL.Add(data2);
                }
                ALLSatNL.Add(sat, staNL);
                progressBar1.PerformStep();
                progressBar1.Refresh();
            }

            

            foreach (var sat in totalsats)
            {
                int number = sat.PRN;
                string SavePath = textBox_OutputDir.Text + "\\G" + "\\NL_" + sat + ".txt";
                FileInfo cFile = new FileInfo(SavePath);
                StreamWriter SW3 = cFile.CreateText();
                SW3.Write("日期");
                SW3.Write(" ");
                SW3.Write("时间");
                SW3.Write(" ");
                foreach (var item in ALLSatNL[sat])
                {
                    SW3.Write(item.staname);
                    SW3.Write(" ");
                }
                SW3.WriteLine();
                for (int i = 0; i < 2851; i++)
                {
                    Time currenttime = ALLSatNL[sat][0].time[i];
                    foreach (var item in ALLSatNL[sat])
                    {                        
                        //if (path.SatNL.Count - 1 < time)
                        //{
                        //    SW3.Write(9999999.ToString());
                        //    SW3.Write(" ");
                        //    continue;
                        //}
                        if (item.staname == "albh")
                        {
                            SW3.Write(currenttime.ToString());
                            SW3.Write(" ");
                        }                      
                        
                        if(item.time.Contains(currenttime))
                        {
                            int j = item.time.IndexOf(currenttime);//返回等于指定时间的索引值
                            if (item.SatNL[j] == 0)
                            {
                                SW3.Write(9999999.ToString());
                            }
                            else
                            {
                                SW3.Write(item.SatNL[j].ToString());//注意是j
                            }                            
                        }
                        else
                        {
                            SW3.Write(9999999.ToString());
                        }
                        
                        SW3.Write(" ");
                    }
                    SW3.WriteLine();
                }
                SW3.Close();
                progressBar1.PerformStep();
                progressBar1.Refresh();
            }
            
            FormUtil.ShowIfOpenDirMessageBox(this.textBox_OutputDir.Text);
        }
        private static string[] SplitByBlank(string line)
        {
            return line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string[] SplitByExcelBlank(string line)
        {
            return line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
        private class Data
        {
            public Time time;
            public List<double> NL;
        }

        private class StaData
        {
            public string staname;
            public List<Time> time;
            public List<double> SatNL;
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            double min = double.Parse(textBox_minvalue.Text);
            double max = double.Parse(textBox_maxvalue.Text);
            double prn = double.Parse(textBox_PRN.Text);
            foreach(var item in ALLSatNL)
            {
                if(item.Key.PRN == prn)
                {
                    foreach(var record in item.Value)
                    {
                        List<double> currentSatNL = new List<double>();
                        currentSatNL = record.SatNL;
                        for (int i = 0; i < record.SatNL.Count; i++ )
                        {
                            if (record.SatNL[i] < min || record.SatNL[i] > max)
                            {
                                record.SatNL[i] = 9999999;
                            }
                        }
                    }
                }
            }
            foreach (var sat in totalsats)
            {
                int number = sat.PRN;
                string SavePath = textBox_OutputDir.Text + "\\newG" + "\\NL_" + sat + ".txt";
                FileInfo cFile = new FileInfo(SavePath);
                StreamWriter SW3 = cFile.CreateText();
                SW3.Write("日期");
                SW3.Write(" ");
                SW3.Write("时间");
                SW3.Write(" ");
                foreach (var item in ALLSatNL[sat])
                {
                    SW3.Write(item.staname);
                    SW3.Write(" ");
                }
                SW3.WriteLine();
                for (int i = 0; i < 2851; i++)
                {
                    foreach (var item in ALLSatNL[sat])
                    {
                        if (item.SatNL.Count - 1 < i)
                        {
                            SW3.Write(9999999.ToString());
                            SW3.Write(" ");
                            continue;
                        }
                        if (item.staname == "albh")
                        {
                            SW3.Write(item.time[i].ToString());
                            SW3.Write(" ");
                        }
                        if (item.SatNL[i] == 0)
                        {
                            SW3.Write(9999999.ToString());
                        }
                        else
                        {
                            SW3.Write(item.SatNL[i].ToString());
                        }

                        SW3.Write(" ");
                    }
                    SW3.WriteLine();
                }
                SW3.Close();                
            }
        }
    }
}
