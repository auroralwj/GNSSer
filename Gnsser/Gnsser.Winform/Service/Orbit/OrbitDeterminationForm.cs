//2014.09.28, czs, create in hailutu, 定轨初步

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Times; 
using Geo.Common;
using Gnsser.Times;
using Geo.Coordinates;
using Gnsser.Data;
using AnyInfo.Layers;
using AnyInfo;

namespace Gnsser.Winform
{
    public partial class OrbitDeterminationForm : Form, Gnsser.Winform.IShowLayer
    {
        public event ShowLayerHandler ShowLayer;
        public OrbitDeterminationForm()
        {
            InitializeComponent();
        }
        OrbitParam param;
        private void button_caculate_Click(object sender, EventArgs e)
        {
            XYZ pos = XYZ.Parse(this.textBox_pos.Text);
            XYZ speed = XYZ.Parse(this.textBox_speed.Text);
            Time time = Time.Parse(this.dateTimePicker1.Value);

            OrbitParamCaculator caculator = new OrbitParamCaculator();
            this.param = caculator.CaculateOrbitParam(pos, speed, time.SecondsOfWeek);

            this.textBox_show.Text = param.ToString();
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
