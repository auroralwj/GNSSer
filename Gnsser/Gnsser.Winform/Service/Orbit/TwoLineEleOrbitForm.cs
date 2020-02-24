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
using Gnsser.Orbits;


namespace Gnsser.Winform
{
    public partial class TwoLineEleOrbitForm : Form, Gnsser.Winform.IShowLayer
    {
        public event ShowLayerHandler ShowLayer;
        public TwoLineEleOrbitForm()
        {
            InitializeComponent();
        } 

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && this.lonlats != null)
            {

                if (ShowLayer != null)
                {
                    Layer layer = LayerFactory.CreatePointLayer(lonlats);
                    ShowLayer(layer);
                }
           
            }
        }
        List<AnyInfo.Geometries.Point> lonlats = null;

        private void button_solve_Click(object sender, EventArgs e)
        {
            TwoLineElement tle = GetTwoLineElement();
            Gnsser.Orbits.Orbit orbit = new Gnsser.Orbits.Orbit(tle); 

            double intervalMin = Double.Parse(this.textBox_intervalMin.Text);
            double count = Int32.Parse(this.textBox_count.Text);
            List<TimedMotionState> sateStates = new List<TimedMotionState>();
            for (int i = 0; i < count; i++)
            {
                double time = i * intervalMin;
                TimedMotionState eciSDP4 = orbit.PositionEci(time);
                sateStates.Add(eciSDP4); 
            }

            if (sateStates.Count == 0) return;
             
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(orbit.ToString());
            sb.AppendLine(tle.ToString());
            foreach (var item in sateStates)
            { 
                sb.AppendLine(item.ToString());
            }

            this.textBox_show.Text = sb.ToString(); 

            int j = 0;
            lonlats = new List<AnyInfo.Geometries.Point>();
            foreach (var item in sateStates)
            {
                GeoCoord geoCoord = CoordTransformer.XyzToGeoCoord(item.Position *1000, AngleUnit.Degree);
                lonlats.Add(new AnyInfo.Geometries.Point(geoCoord, j + "", item.Date.ToTime().ToString("hh:mm:ss")));
                j++;
            }
        }

        private TwoLineElement GetTwoLineElement()
        {
            string str = this.textBox_input.Text; 
            TwoLineElement tle = new TwoLineElement(str);
            return tle;
        }

        private void button_radarCaculate_Click(object sender, EventArgs e)
        { 
            bool isGeoCoord = this.radioButton_geoCoord.Checked; 

            GeoCoord siteCoord = null;
            if (isGeoCoord)
            {
                siteCoord = GeoCoord.Parse(this.textBox_coord.Text);
                siteCoord.Unit = AngleUnit.Degree;
            }
            else
            {
                XYZ xyz = XYZ.Parse(this.textBox_coord.Text);
                siteCoord = CoordTransformer.XyzToGeoCoord(xyz);
            }

            TwoLineElement tle = GetTwoLineElement();
            Gnsser.Orbits.Orbit orbit = new Gnsser.Orbits.Orbit(tle);

            double intervalMin = Double.Parse(this.textBox_intervalMin.Text);
            double count = Int32.Parse(this.textBox_count.Text); 
             
            lonlats = new List<AnyInfo.Geometries.Point>();
            lonlats.Add(new AnyInfo.Geometries.Point(siteCoord, "SitePoint"));
            for (int i = 0; i < count; i++)
            {
                double time = i * intervalMin;
                TimedMotionState eciSDP4 = orbit.PositionEci(time);
                TopoCoord topoLook = OrbitUtils.GetSatTopoCoord(eciSDP4, siteCoord);

                if (topoLook.Elevation > 0)
                {
                    GeoCoord geoCoord = CoordTransformer.XyzToGeoCoord(eciSDP4.Position * 1000, AngleUnit.Degree);
                    lonlats.Add(new AnyInfo.Geometries.Point(geoCoord, i + ""));
                }
            }

        }
    }
}
