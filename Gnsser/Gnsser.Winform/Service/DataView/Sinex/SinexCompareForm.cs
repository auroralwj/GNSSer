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

namespace Gnsser.Winform
{
    public partial class SinexCompareForm : Form, IShowLayer
    {
        public SinexCompareForm()
        {
            InitializeComponent();
        }

        public event ShowLayerHandler ShowLayer;
        SinexFile fileA;
        SinexFile fileB;

        private void button_getPath_Click(object sender, EventArgs e) { if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)   this.textBox_PathA.Text = this.openFileDialog1.FileName; }
        private void button_getPathB_Click(object sender, EventArgs e) { if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) this.textBox_PathB.Text = this.openFileDialog1.FileName; }
        private void button_readB_Click(object sender, EventArgs e) { Read(); }

        private void Read()
        {
            string pathA = this.textBox_PathA.Text;
            if (!File.Exists(pathA))
            {
                FormUtil.ShowFileNotExistBox(pathA);
                return;
            }
            string pathB = this.textBox_PathB.Text;
            if (!File.Exists(pathB))
            {
                FormUtil.ShowFileNotExistBox(pathB);
                return;
            }

            fileA = SinexReader.Read(pathA);
            this.textBox_sinexA.Text = fileA.ToString();
            List<SinexSiteDetail> listA = fileA.GetSinexSites();
            this.bindingSourceA.DataSource = listA;

            fileB = SinexReader.Read(pathB);
            this.textBox_sinexB.Text = fileB.ToString();
            List<SinexSiteDetail> listB = fileB.GetSinexSites();
            this.bindingSourceB.DataSource = listB;


            this.bindingSourceC.DataSource = SinexSiteDetail.Compare(listA, listB);
        }


        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && fileA != null)
            {
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                List<SolutionEstimateGroup> group = SolutionEstimateGroup.GetGroups(fileA.SolutionEstimateBlock.Items);

                foreach (SolutionEstimateGroup g in group)
                {
                    lonlats.Add(new AnyInfo.Geometries.Point(g.GeoCoord, g.Items[0].SiteCode));
                }


                //foreach (SiteId text in fileA.SiteIdBlock.Items)
                //{
                //    lonlats.Add(new AnyInfo.Geometries.Point(text.GeoCoord, text.SiteCode));
                //}
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }



    }
}
