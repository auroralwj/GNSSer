using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Coordinates;
using Geo.Times;

namespace Gnsser.Winform
{
    public partial class TropCaculateForm : Form
    {
        public TropCaculateForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            GeoCoord geoCoord = GeoCoord.Parse(this.textBox_geoCoord.Text);
            var time = this.dateTimePicker1.Value;
            var gpsTime = Time.Parse(time);
            #region 方案1 ： Neill模型
            var elevation = geoCoord.Height;
            var pTroModel = new NeillTropModel(geoCoord.Height, geoCoord.Lat, gpsTime.DayOfYear);

            var wetMap = pTroModel.Wet_Mapping_Function(elevation);
 
            var correction = GetTroposphericCorectValueWithNillModel(elevation, geoCoord, pTroModel);

            #endregion

            //#region 方案2： GMF 模型 , 崔阳， Added
           
            //double[] azel = new double[] { info.Azimuth * SunMoonPosition.D2R, info.Elevation * SunMoonPosition.D2R };

            //wetMap = 0.0;

            //double wetCorrectValue = epochSatellite.EpochInfo.ObsInfo.TropoCorrectValue;

            //correction = gTroModel.Correction(gpsTime, geoCoord, receiverPosition, azel, wetCorrectValue, ref wetMap);
            ////}
            //#endregion

            //#region 方案3： VMF 模型 ， 李林阳 added
            //      else
            //{

            //string stanam = epochSatellite.obsPath.MarkerName;

            ////double WetMap = 0.0;
            ////采用VMF1模型~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~，李林阳添加
            //this.VMF1TroModel = new VMF1TropModel(DataSourceProvider, stanam, geoCoord.Height, geoCoord.Lat * CoordConsts.DegToRadMultiplier, gpsTime);
            //wetMap = 0;
            ////采用VMF1模型，GetSatPhaseCenterCorectValue函数也要作出改变~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~，李林阳添加
            ////double correction = GetSatPhaseCenterCorectValue(elevation, geoCoord, VMF1TroModel,ref WetMap);
            //correction = VMF1TroModel.Correction(elevation, ref wetMap);

            //epochSatellite.Vmf1WetMap = wetMap;
            ////}
            //#endregion


            //epochSatellite.WetMap = wetMap;

            ShowInfo (correction);
        }

        public void ShowInfo(object msg)
        {
            this.richTextBoxControl1.Text += msg + "\r\n";
        }
            
      /// <summary>
      /// 对流层延迟改正
      /// </summary>
      /// <param name="satelliteType"></param>
      /// <param name="time"></param>
      /// <param name="satPos"></param>
      /// <param name="sunPosition"></param>
        /// <param name="satData">Name of "PRN_GPS"-like file containing satellite satData.</param>
      /// <returns></returns>
        public static double GetTroposphericCorectValueWithNillModel(double elevation, GeoCoord geoCoord, NeillTropModel pTroModel)
        {
            double tropoCorr = 0.0;
            //Compute tropospheric slant correction
            tropoCorr = pTroModel.Correction(elevation);

            if (!pTroModel.isValid())
            {
                tropoCorr = 0.0;
            }

            return tropoCorr;
        }
    }
}
