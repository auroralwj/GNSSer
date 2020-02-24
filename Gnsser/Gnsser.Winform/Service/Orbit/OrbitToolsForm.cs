//2014.09.28, czs, create in hailutu, 定轨初步

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Geo.Common;
using Gnsser.Times;
using Geo.Coordinates;
using Gnsser.Data;
using AnyInfo.Layers;
using AnyInfo;

namespace Gnsser.Winform
{
    public partial class OrbitToolsForm : Form, Gnsser.Winform.IShowLayer
    {
        public event ShowLayerHandler ShowLayer;
        public OrbitToolsForm()
        {
            InitializeComponent();
        }
        OrbitParam param;
        private void button_caculate_Click(object sender, EventArgs e)
        {
            Double radius = Double.Parse(this.textBox_radius.Text);
            Double distance = Double.Parse(this.textBox_distance.Text);
            Double dTime = distance / GnssConst.LIGHT_SPEED;

            double myConst = Math.Sqrt(GnssSystem.Gps.Ellipsoid.GM / radius) / GnssConst.LIGHT_SPEED;
            double ds = distance * myConst;

            this.textBox_show.Text ="常数：" +myConst + ",时间偏差：" + dTime + ", 卫星偏差：" + ds.ToString();
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && this.param != null)
            {
                double time = 0;
                OrbitParamCaculator caculator = new OrbitParamCaculator();
                List<XYZ> xyzs = new List<XYZ>();
                for (int i = 0; i < 500; i++)
                {
                    time = i * 10000;
                    XYZ xyz = caculator.GetSatPos(param, time);
                    xyzs.Add(xyz);
                }

                 
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                int j = 0;
                foreach (var item in xyzs)
                {
                    GeoCoord geoCoord = CoordTransformer.XyzToGeoCoord(item);
                    lonlats.Add(new AnyInfo.Geometries.Point(geoCoord, j + "" ));
                    j++;
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }
    }
}
