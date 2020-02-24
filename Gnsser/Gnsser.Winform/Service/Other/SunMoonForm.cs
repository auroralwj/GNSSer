using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using Geo.Times; 
using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;
using Geo.Referencing;
using AnyInfo;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using System.Windows.Forms.DataVisualization.Charting; 

namespace Gnsser.Winform
{
    public partial class SunMoonForm : Form , Gnsser.Winform.IShowLayer
    {
         
        public event ShowLayerHandler ShowLayer; 

        public SunMoonForm()
        {
            InitializeComponent();
        }
        GeoCoord moonGeo;
        GeoCoord sunGeo;
        private void button1_Click(object sender, EventArgs e)
        {
            Time time = new Time(this.dateTimePicker1.Value);

            MoonPosition moon = new MoonPosition();
            XYZ moonPos = moon.GetPosition(time);
            moonGeo =  CoordTransformer.XyzToGeoCoord(moonPos);

            StringBuilder moonSb = new StringBuilder();
            moonSb.AppendLine("XYZ:" + moonPos.ToString()  );
            moonSb.AppendLine("GeoCoord:" + moonGeo.ToString()  );
            this.textBox_moon.Text = moonSb.ToString();


            SunPosition sun = new SunPosition();
            XYZ sunPos = sun.GetPosition(time);

            sunGeo = CoordTransformer.XyzToGeoCoord(sunPos);

            StringBuilder sunSb = new StringBuilder();
            sunSb.AppendLine("XYZ:" + sunPos.ToString());
            sunSb.AppendLine("GeoCoord:" + sunGeo.ToString());
            this.textBox_sun.Text = sunSb.ToString(); 
        }

        private void button_showMap_Click(object sender, EventArgs e)
        {
            if (moonGeo != null && sunGeo != null && ShowLayer != null)
            {
                AnyInfo.Geometries.Point sunpt = new AnyInfo.Geometries.Point(sunGeo, "1", "太阳");
                AnyInfo.Geometries.Point moonpt = new AnyInfo.Geometries.Point(moonGeo, "2", "月亮");


                List< AnyInfo.Geometries.Point > lonLats = new List< AnyInfo.Geometries.Point >(){
                    sunpt, moonpt
                }; 

                Layer layer = LayerFactory.CreatePointLayer(lonLats);
                ShowLayer(layer);
            }
        }
    }
}
