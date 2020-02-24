using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Gnsser.Data.Sinex;

using Geo.Utils;
using Geo.Coordinates;
using AnyInfo;
using Gnsser.Service; 

namespace Gnsser.Winform
{
    /// <summary>
    /// Gnss网平差计算
    /// </summary>
    public partial class GnssNetworkAdjustment: Form
    {
        /// <summary>
        /// 显示图层
        /// </summary>
        public event ShowLayerHandler showPointLayer;
        
        public GnssNetworkAdjustment()
        {
            InitializeComponent();
        }


        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_Path.Lines = this.openFileDialog1.FileNames;
            }
        }

        private void button_readAdjust_Click(object sender, EventArgs e) 
        {         
            Read();
        }

        private void Read()
        {
            string strKnownPoints = this.textBox_KnowPoints.Text;
            if (strKnownPoints == null) { throw new Exception("请输入固定点点号！"); }

            string[] strKPoints = strKnownPoints.Split(';');
            string[] filePaths = this.textBox_Path.Lines;
           
            double k0 = Convert.ToDouble(this.textBox_k0.Text);
            double k1 = Convert.ToDouble(this.textBox_k1.Text);
            double eps = Convert.ToDouble(this.textBox_eps.Text);
        //    string[] filePathes = this.textBox_Path.Lines;

         //   List<SinexFile> sinexFiles = SinexFile.Read(filePaths);
          //  SinexNetworksAdjust sinexNetAdjust = new SinexNetworksAdjust(filePaths, strKPoints, k0, k1, eps);
            double rate = 0.05;
            SinexNetworksAdjust sinexNetAdjust = new SinexNetworksAdjust(filePaths, strKPoints, k0, k1, eps, rate);

            // SinexNetworkAjustment sna = new SinexNetworkAjustment(filePaths, strKPoints,1);//
        }

        List<GeoCoord> GeoCoords;

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
        //    if (showPointLayer != null && GeoCoords != null)
        //    {
        //        List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
        //        foreach (GeoCoord g in GeoCoords)
        //        {
        //            lonlats.Add(new AnyInfo.Geometries.Point(g, "1"));
        //        }
        //        Layer layer = LayerFactory.CreatePointLayer(lonlats);
        //        showPointLayer(layer);
        //    }
        }

        private void button_saveTofile_Click(object sender, EventArgs e)
        {
        //    if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        File.WriteAllText(saveFileDialog1.FileName, this.textBox_info.Text);

        //        FormUtil.ShowIfOpenDirMessageBox(saveFileDialog1.FileName);
        //    }
        }

        private void button_LSAdjust_Click(object sender, EventArgs e)
        {
            string strKnownPoints = this.textBox_KnowPoints.Text;
            if (strKnownPoints == null) { throw new Exception("请输入固定点点号！"); }

            string[] strKPoints = strKnownPoints.Split(';');
            string[] filePaths = this.textBox_Path.Lines;
            //    string[] filePathes = this.textBox_Path.Lines;

            //   List<SinexFile> sinexFiles = SinexFile.Read(filePaths);
            SinexNetworksAdjust sinexNetAdjust = new SinexNetworksAdjust(filePaths, strKPoints);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strKnownPoints = this.textBox_KnowPoints.Text;
            if (strKnownPoints == null) { throw new Exception("请输入固定点点号！"); }

            string[] strKPoints = strKnownPoints.Split(';');
            string[] filePaths = this.textBox_Path.Lines;

            double k0 = Convert.ToDouble(this.textBox_k0.Text);
            double k1 = Convert.ToDouble(this.textBox_k1.Text);
            double eps = Convert.ToDouble(this.textBox_eps.Text);
            //    string[] filePathes = this.textBox_Path.Lines;

            //   List<SinexFile> sinexFiles = SinexFile.Read(filePaths);
            //  SinexNetworksAdjust sinexNetAdjust = new SinexNetworksAdjust(filePaths, strKPoints, k0, k1, eps);
            double rate = 0.02;
            SinexNetworksAdjust sinexNetAdjust = new SinexNetworksAdjust(filePaths, strKPoints, k0, k1, eps, rate);

            // SinexNetworkAjustment sna = new SinexNetworkAjustment(filePaths, strKPoints,1);//

        }

        private void button_FreeCoGPS_Click(object sender, EventArgs e)
        {
            string[] filePaths = this.textBox_Path.Lines;
         
            SinexNetworksAdjust sinexNetAdjust = new SinexNetworksAdjust(filePaths);
        }

        //private void button_getPath_Click(object sender, EventArgs e)
        //{
        //    if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        this.textBox_Path.Lines = this.openFileDialog1.FileNames;
        //    }
        //}

        //private void button_read_Click(object sender, EventArgs e)
        //{
        //    ////读取已知点点号
        //    //string strKPointLine = this.textBox_KnowPoints.Text;
        //    //if (strKPointLine == null)
        //    //{ throw new Exception("已知点个数不能为零！"); }
        //    //string[] strKPoints = strKPointLine.Split(';');
        //    //string[] filePathes = this.textBox_Path.Lines;

        //    //GPSSinexNetsAdjust GpsNetsAdj = new GPSSinexNetsAdjust(filePathes, strKPoints);


        //    ////Read();

        //    //List<SinexFile> sinexFiles = SinexFile.Read(filePathes);

        //    //SinexFile current = null;
        //    //foreach (var path in sinexFiles)
        //    //{
        //    //    if (current == null) current = path;
        //    //    else
        //    //    {
        //    //        SubNetsUnion just = new SubNetsUnion(current, path);
        //    //        GeoCoords = just.GetGeoCoords();

        //    //        current = just.ResultSinexFile;
        //    //    }
        //    //}
        //    //this.textBox_info.Text = current.ToString();//.GetResult();
        //}

        //private void button_saveTofile_Click(object sender, EventArgs e)
        //{
        //    if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        File.WriteAllText(saveFileDialog1.FileName, this.textBox_info.Text);

        //        FormUtil.ShowIfOpenDirMessageBox(saveFileDialog1.FileName);
        //    }
        //}

        //private void button_showOnMap_Click(object sender, EventArgs e)
        //{
        //    if (showPointLayer != null && GeoCoords != null)
        //    {
        //        List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
        //        foreach (GeoCoord g in GeoCoords)
        //        {
        //            lonlats.Add(new AnyInfo.Geometries.Point(g, "1"));
        //        }
        //        Layer layer = LayerFactory.CreatePointLayer(lonlats);
        //        showPointLayer(layer);
        //    }
        //}        

    }
}
