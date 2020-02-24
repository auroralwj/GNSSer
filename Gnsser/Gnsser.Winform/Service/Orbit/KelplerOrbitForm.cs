//2016.05.31, czs, create in hongqing, 定轨初步

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
using Geo;

namespace Gnsser.Winform
{
    public partial class KelplerOrbitForm : Form, Gnsser.Winform.IShowLayer
    {
        public event ShowLayerHandler ShowLayer;
        public KelplerOrbitForm()
        {
            InitializeComponent();
        } 

        List<MotionState> xyzs = new List<MotionState>();
        private void button_caculate_Click(object sender, EventArgs e1)
        {
            xyzs.Clear();
            double a = double.Parse(textBox_a.Text);
            double e = double.Parse(this.textBox_e.Text);
            double i0 = double.Parse(this.textBox_i.Text);
            double M = double.Parse(this.textBox_M.Text);
            double Ω = double.Parse(this.textBox_Ω.Text);
            double omiga = double.Parse(this.textBox_omiga.Text);

            double interval = double.Parse(textBox_interval.Text);
            double epochCount = double.Parse(textBox_epochCount.Text);
            CelestialMomentEllipseOrbit orbit = new CelestialMomentEllipseOrbit(omiga, i0, Ω, new MomentEllipseOrbit(M, new PlaneEllipse(a, e)));
            StringBuilder sb = new StringBuilder();
            for (double i = 0, t = 0; i < epochCount; i++,  t+= interval)
            {
                var xyz = KeplerOrbitUtil.GetSatPos(t, i0, Ω,a, e, omiga, M);
                var op = new MomentEllipseOrbit(M, t, new PlaneEllipse(a,e));
                var ceXyz = op.GetCelestialMotionState(omiga, i0, Ω);
                var differXyz = xyz - ceXyz;
                var three = orbit.GetMotionState(t);
                var differ2 = xyz - three;

                xyzs.Add(xyz);
                sb.AppendLine(xyz.ToString());
            }

            var state = KeplerOrbitUtil.GetSatPos(0, i0, Ω, a, e, omiga, M);
            var param = KeplerOrbitUtil.GetKeplerEphemerisParam(state);
            var orbit2 = new CelestialMomentEllipseOrbit(state);
           var tweo = orbit2.MotionState;
            var state2 = KeplerOrbitUtil.GetSatPos(0, param);

            var differ = state - state2;
            var dirre = state - tweo;
            int ii = 0;
            //double dt = double.Parse(this.textBox_dt.Text);

            //XYZ xyz = KeplerOrbitSolver.GetSatPos(dt, i0, Ω, Math.Sqrt(a), e, omiga, M);


            //XYZ pos = XYZ.Parse(this.textBox_a.Text);
            //XYZ speed = XYZ.Parse(this.textBox_dt.Text);
            //Time time = Time.Parse(this.dateTimePicker1.Value);

            //OrbitParamCaculator caculator = new OrbitParamCaculator();
            //this.param = caculator.CaculateOrbitParam(pos, speed, time.SecondsOfWeek);

            this.textBox_show.Text = sb.ToString();
            this.RichTextBoxControl_processInfo.Text = "计算完毕！";
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && this.xyzs.Count > 0)
            { 
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                int j = 0;
                foreach (var item in xyzs)
                {
                    GeoCoord geoCoord = CoordTransformer.XyzToGeoCoord(item.Position);
                    lonlats.Add(new AnyInfo.Geometries.Point(geoCoord, j + "" ));
                    j++;
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }

        private void button_solveOrbitParam_Click(object sender, EventArgs e)
        {
            XYZ pos = XYZ.Parse(this.textBox_pos.Text);
            XYZ vel = XYZ.Parse(this.textBox_vel.Text);
            MotionState state = new MotionState(pos, vel);
            CelestialMomentEllipseOrbit orbit = new CelestialMomentEllipseOrbit(state);
            textBox_a.Text = orbit.MomentEllipseOrbit.PlaneEllipse.a + "";
            this.textBox_e.Text = orbit.MomentEllipseOrbit.PlaneEllipse.e + "";
            this.textBox_i.Text = orbit.Inclination + "";
            this.textBox_M.Text = orbit.MomentEllipseOrbit.MeanAnomaly + "";
            this.textBox_Ω.Text = orbit.RightAscensionOfAscendingNode + "";
            this.textBox_omiga.Text = orbit.ArgumentOfPerigee + "";

            MessageBox.Show("轨道参数已更新！");
        }
    }
}
