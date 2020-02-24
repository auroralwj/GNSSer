using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Sinex;
using Geo.Coordinates;
using System.IO;
using Geo.Referencing;
using System.IO;
using Geo.Utils;

namespace Gnsser.Winform.Other
{
    public partial class SinexCoord : Form
    {
        public SinexCoord()
        {
            InitializeComponent();
        }

        SinexFile sinexfile;
        string SavePath;
        Dictionary<string, xyzblh> sitecood;
        SortedDictionary<string, xyzblh> GnsserSitecood;
        Dictionary<string, twoxyzbl> CommonSite;
        private void button_getfilepath_Click(object sender, EventArgs e)
        {
            if(this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_sinexFile.Text = this.openFileDialog1.FileName;
            }
        }
        private class xyzblh
        {
            public XYZ truexyz;
            public double lon;
            public double lat;
        }

        private class twoxyzbl
        {
            public XYZ truexyz;
            public double truelon;
            public double truelat;

            public XYZ estxyz;
            public double estlon;
            public double estlat;
        }
        private void button_extractcoords_Click(object sender, EventArgs e)
        {
            sinexfile = SinexReader.Read(textBox_sinexFile.Text);
            List<string> sites = sinexfile.GetSiteCods();

            double[] CoordArray = sinexfile.GetEstimateVector();
            sitecood = new Dictionary<string, xyzblh>();
            for(int i =0 ;i< sites.Count;i++)
            {
                string tmpsite = sites[i];
                XYZ tmpxyz = new XYZ(CoordArray[3 * i + 0], CoordArray[3 * i + 1], CoordArray[3 * i + 2]);
                GeoCoord tmplonlat = CoordTransformer.XyzToGeoCoord(tmpxyz, Ellipsoid.WGS84);
                if(!sitecood.Keys.Contains(tmpsite))
                {
                    sitecood.Add(tmpsite, new xyzblh());
                    sitecood[tmpsite].truexyz = tmpxyz;
                    sitecood[tmpsite].lon = tmplonlat.Lon;
                    sitecood[tmpsite].lat = tmplonlat.Lat;
                }                
            }

            SavePath = this.textBox_uotputpath.Text + "\\sinexcoord" + ".txt";
                //"C:\\Users\\lilinyang\\Desktop\\sinexcoord" + ".txt";
            FileInfo aFile = new FileInfo(SavePath);
            StreamWriter SW = aFile.CreateText();
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;
            GN.NumberDecimalDigits = 6;
            foreach (var item in sitecood)
            {
                SW.Write(item.Key.ToString());
                SW.Write(" ");
                SW.Write(item.Value.truexyz.X.ToString());
                SW.Write(" ");
                SW.Write(item.Value.truexyz.Y.ToString());
                SW.Write(" ");
                SW.Write(item.Value.truexyz.Z.ToString());
                SW.Write(" ");
                SW.Write(item.Value.lon.ToString());
                SW.Write(" ");
                SW.Write(item.Value.lat.ToString());
                SW.Write("\n");
            }
            SW.Close();
        }

        private void button_outputDirec_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_uotputpath.Text = this.folderBrowserDialog1.SelectedPath;
        }

        private void button_GnsserResult_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //this.textBox_sinexFile.Text = this.openFileDialog1.FileName;
                this.textBox_GnsserFile.Text = this.openFileDialog2.FileName;
            }
        }
        public static string[] SplitByBlank(string line)
        {
            return line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
        private void button_CommonPoint_Click(object sender, EventArgs e)
        {
            //读取Gnsser结果
            bool isEnd = true;
            GnsserSitecood = new SortedDictionary<string, xyzblh>();
            using (StreamReader sr = new StreamReader(this.textBox_GnsserFile.Text))
            {
                //string line = sr.ReadLine();
                while (isEnd)
                {
                    string line = sr.ReadLine();
                    if (line == null || line == "")
                        break;
                    if (line.Length > 255)
                    {
                        throw new Exception("Line too long");
                    }
                    if (line.Length == 0)
                    {
                        isEnd = false;
                    }
                    //string[] tmp = StringUtil.SplitByBlank(line);
                    string[] tmp = SplitByBlank(line);
                    string stanam = tmp[0];
                    double X = double.Parse(tmp[1]);
                    double Y = double.Parse(tmp[2]);
                    double Z = double.Parse(tmp[3]);
                    double L = 0;
                    double B = 0;
                    if(tmp.Length > 4)
                    {
                        L = double.Parse(tmp[4]);
                        B = double.Parse(tmp[5]);
                    }                    

                    XYZ tmpxyz = new XYZ(X, Y, Z);
                    GnsserSitecood.Add(stanam, new xyzblh());
                    GnsserSitecood[stanam].truexyz = tmpxyz;
                    GnsserSitecood[stanam].lon = L;
                    GnsserSitecood[stanam].lat = B;
                }
            }

            CommonSite = new Dictionary<string, twoxyzbl>();
            foreach(var item in sitecood)
            {
                string tmpstanam = item.Key;
                foreach(var record in GnsserSitecood)
                {
                    if(record.Key == tmpstanam)
                    {

                        CommonSite.Add(tmpstanam, new twoxyzbl());
                        CommonSite[tmpstanam].truexyz = item.Value.truexyz;
                        CommonSite[tmpstanam].truelon = item.Value.lon;
                        CommonSite[tmpstanam].truelat = item.Value.lat;

                        CommonSite[tmpstanam].estxyz = record.Value.truexyz;
                        CommonSite[tmpstanam].estlon = record.Value.lon;
                        CommonSite[tmpstanam].estlat = record.Value.lat;
                    }
                }
            }

            SavePath = this.textBox_uotputpath.Text + "\\commonsaite" + ".txt";
            //"C:\\Users\\lilinyang\\Desktop\\sinexcoord" + ".txt";
            FileInfo aFile = new FileInfo(SavePath);
            StreamWriter SW = aFile.CreateText();
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;
            GN.NumberDecimalDigits = 6;
            foreach (var item in CommonSite)
            {
                SW.Write(item.Key.ToString());
                SW.Write(" ");
                SW.Write(item.Value.truexyz.X.ToString());
                SW.Write(" ");
                SW.Write(item.Value.truexyz.Y.ToString());
                SW.Write(" ");
                SW.Write(item.Value.truexyz.Z.ToString());
                SW.Write(" ");
                SW.Write(item.Value.truelon.ToString());
                SW.Write(" ");
                SW.Write(item.Value.truelat.ToString());
                SW.Write(" ");
                SW.Write(item.Value.estxyz.X.ToString());
                SW.Write(" ");
                SW.Write(item.Value.estxyz.Y.ToString());
                SW.Write(" ");
                SW.Write(item.Value.estxyz.Z.ToString());
                SW.Write(" ");
                SW.Write(item.Value.estlon.ToString());
                SW.Write(" ");
                SW.Write(item.Value.estlat.ToString());
                SW.Write("\n");
            }
            SW.Close();
        }
        
    }
}
