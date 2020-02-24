using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Sinex;
using Gnsser.Service;
using Geo.WinUtils;
using Geo.Utils;
using Geo.Coordinates;
using AnyInfo;
using Geo.Algorithm.Adjust;
using Cygeo.Adjustment.EMST;

namespace Gnsser.Winform
{
    public partial class SelectedBestBaseline : Form
    {
        public SelectedBestBaseline()
        {
            InitializeComponent();
        }

        private void button_GetPppPathes_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) this.textBox_PathesOfPppFile.Lines = this.openFileDialog1.FileNames;
        }

        private void button_GetCoordSnsPathes_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) this.textBox_pathesOfCoordFile.Lines = this.openFileDialog1.FileNames;
        }


        List<AnyInfo.Geometries.Point> points = new List<AnyInfo.Geometries.Point>();

        private void button_PppBestBaseline_Click(object sender, EventArgs e)
        {

            //---------------------------------------------------数据读取//---------------------------------------------------
           
            //读取PPP结果
            string[] pppfilePathes = this.textBox_PathesOfPppFile.Lines;
            List<SinexFile> pppsinexFiles = SinexFile.Read(pppfilePathes);
            List<string> strSite = new List<string>();
            int m = pppfilePathes.Length;
            double[][] pppXyz = new double[m * 3][];

            int count = 0;
            foreach (var file in pppsinexFiles)
            {
                List<SinexSiteDetail> sites = file.GetSinexSites();


                foreach (var item2 in sites)
                {
                    if (!strSite.Contains(item2.Name.ToUpper()))
                    {
                        strSite.Add(item2.Name.ToUpper());
                    }
                    else
                    {
                        throw new Exception("测站信息重复！");
                    }

                }
                double[][] xyz = file.GetEstimateMatrix();
                double[][] xyzCov = file.GetEstimateCovaMatrix();
                double std = file.GetStatistic().VarianceFactor;
                pppXyz[3 * count + 0] = new double[1]; pppXyz[3 * count + 0][0] = xyz[0][0];
                pppXyz[3 * count + 1] = new double[1]; pppXyz[3 * count + 1][0] = xyz[1][0];
                pppXyz[3 * count + 2] = new double[1]; pppXyz[3 * count + 2][0] = xyz[2][0];



                count += 1;
            }


            List<Delaunay.deXYZ> ListXyz = new List<Delaunay.deXYZ>();

            for (int i = 0; i < strSite.Count; i++)
            {
                Delaunay.deXYZ xyz = new Delaunay.deXYZ();
                xyz.X = pppXyz[3 * i + 0][0];
                xyz.Y = pppXyz[3 * i + 1][0];
                xyz.Z = pppXyz[3 * i + 2][0];
                ListXyz.Add(xyz);
            }


            Delaunay delaunay = new Delaunay(ListXyz);

            MiniSpanTreeKruskal MiniSpanTreeKruskal = new Cygeo.Adjustment.EMST.MiniSpanTreeKruskal(delaunay);


            List<string> BenDir = new List<string>();
            List<string> EndDir = new List<string>();
            List<double> LengthBaseline = new List<double>();

            foreach (var item in MiniSpanTreeKruskal.min_ed)
            {
                BenDir.Add(strSite[item.vs]);  //起点
                EndDir.Add(strSite[item.ve]); //终点

                LengthBaseline.Add(item.weight);
            }









            //---------------------------------------------------------结果输出--------------------------------------------
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("最短基线及其长度");

            for (int i = 0; i < strSite.Count; i++)
            {
                sb.AppendLine(strSite[i]);
            }


            for (int i = 0; i < BenDir.Count; i++)
            {
                sb.AppendLine(BenDir[i] + "-" + EndDir[i] + " " + LengthBaseline[i].ToString());
            }



            this.textBox_info.Text = sb.ToString();

            //----------------------------------------------------地图准备//----------------------------------------------------
            points.Clear();
            //GeoCoord c1 = CoordTransformer.XyzToGeoCoord(aprioriA);
            //GeoCoord c2 = CoordTransformer.XyzToGeoCoord(aprioriB);
            //GeoCoord c3 = CoordTransformer.XyzToGeoCoord(estA);
            //GeoCoord c4 = CoordTransformer.XyzToGeoCoord(estB);
            //points.Add(new AnyInfo.Geometries.Point(c1.Lon, c1.Lat, "aprA"));
            //points.Add(new AnyInfo.Geometries.Point(c2.Lon, c2.Lat, "aprB"));
            //points.Add(new AnyInfo.Geometries.Point(c3.Lon, c3.Lat, "estA"));
            //points.Add(new AnyInfo.Geometries.Point(c4.Lon, c4.Lat, "estB")); 

        }
    }
}

