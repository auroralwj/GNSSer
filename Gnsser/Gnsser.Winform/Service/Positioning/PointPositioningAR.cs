using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser;
using Gnsser.Times;
using Geo.Coordinates;
using Gnsser.Domain;
using Gnsser.Service;

namespace Gnsser.Winform
{
    public partial class PointPositioningAR : Form
    {
        Data.Rinex.RinexObsFile obsFile;
        
        RinexFileObsDataSource observationDataSource;
        List<SatelliteType> SatelliteTypes = new List<SatelliteType>();
        PppAR pppar;
         
        /// <summary>
        /// 观测量数据来源
        /// </summary>
        public RinexFileObsDataSource dataSource { get; set; } 
        public PointPositioningAR()
        {
            InitializeComponent();
        }

        private void button_getObsPath_Click(object sender, EventArgs e)
        {
            if(openFileDialog_obs.ShowDialog()==DialogResult.OK)
            {
                textBox_obsPath.Text = openFileDialog_obs.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            obsFile = (Data.Rinex.RinexObsFile)new Data.Rinex.RinexObsFileReader(this.textBox_obsPath.Text).ReadObsFile();
               // new ObservationFlieReader(this.textBox_obsPath.Text).ReadObsFile();
            List<SatelliteNumber> prns = obsFile.GetPrns();
            List<SatelliteNumber> gpsprns = new List<SatelliteNumber>();
            foreach(var prn in prns)
            {
                if(prn.SatelliteType == SatelliteType.G)
                {
                    gpsprns.Add(prn);
                }
            }
            gpsprns.Sort();
            this.bindingSource_SDWL.DataSource = gpsprns;
            
        }
        /// <summary>
        /// 每个历元计算一次星间单差宽巷模糊度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_calWLFCBs_Click(object sender, EventArgs e)
        {
            SatelliteNumber refSat;
            refSat = (SatelliteNumber)this.bindingSource_SDWL.Current;

            //需要处理的卫星导航系统类型
            if (checkBox_BD.Checked) SatelliteTypes.Add(SatelliteType.C);
            if (checkBox_GPS.Checked) SatelliteTypes.Add(SatelliteType.G);       

            observationDataSource = new RinexFileObsDataSource(this.textBox_obsPath.Text);

            pppar = new PppAR(observationDataSource, refSat, SatelliteTypes);

            pppar.calculateMW();

            pppar.CalculateSDWLFCBs();
            #region
            //FilterEpochObservation为需要的卫星导航系统的当前离一按的观测值
            //EpochObservation FilterEpochObservation = new EpochObservation();
            //List<EpochObservation> ValidEpoch = new List<EpochObservation>();
            //foreach(EpochObservation os in obsFile)
            //{
            //    FilterEpochObservation = checkeddata(os, types);
            //    if(FilterEpochObservation.Prns.Contains(refSat))
            //    {
            //        ValidEpoch.Add(FilterEpochObservation);                    
            //    }
            //    else
            //    {
            //        break;//首次没有出现参考星就退出
            //    }
                    
            //}
            ////对没课卫星组MW组合
            //int time=0;
            //foreach(var os in ValidEpoch)
            //{
            //    foreach(var sat in os.Prns)
            //    {
            //        ObservationData currentsatdata = ValidEpoch[time].GetObservationData(sat);
            //        double P1 = currentsatdata._values[GnssDataType.P1].Value;
            //        double P2 = currentsatdata._values[GnssDataType.P2].Value;
            //        double L1 = currentsatdata._values[GnssDataType.L1].Value;
            //        double L2 = currentsatdata._values[GnssDataType.L2].Value;

            //        double MW = (L1 - L2) - ((GnssConst.GPS_L1_FREQ * P1 + GnssConst.GPS_L2_FREQ * P2) / (GnssConst.GPS_L1_FREQ + GnssConst.GPS_L2_FREQ)) * (GnssConst.GPS_L1_FREQ - GnssConst.GPS_L2_FREQ) / GnssConst.LIGHT_SPEED;
                 

            //    }
            //}
            #endregion

        }

        private void button_view_Click(object sender, EventArgs e)
        {
            //pppar.viewSDWL();
            List<DataTable> table = pppar.viewSDWL();

            this.dataGridView_SDWL.DataSource = table[0];
            bindingSource_SDWL.DataSource = this.dataGridView_SDWL.DataSource;

            this.dataGridView_UPD.DataSource = table[1];
            bindingSource_UPD.DataSource = this.dataGridView_UPD.DataSource;

        }

        private void PointPositioningAR_Load(object sender, EventArgs e)
        {
            textBox_obsPath.Text = Setting.GnsserConfig.SampleOFile;
        }

        /// <summary>
        /// 过滤掉GLONASS卫星，只保留GPS卫星
        /// 检核所有卫星的数据完整性
        /// </summary>
        /// <param name="satelliteType"></param>
        /// <returns></returns>
        //public EpochObservation checkeddata(EpochObservation currentEpochObservation, List<SatelliteType> SatelliteTypes)
        //{
        //    EpochObservation newepoc = new EpochObservation();
        //    newepoc.GpsTime = currentEpochObservation.GpsTime;
        //    newepoc.Header = currentEpochObservation.Header;
        //    //只保留GPS卫星
        //    foreach(var satelliteType in currentEpochObservation.Prns)
        //    {
        //        if (!SatelliteTypes.Contains(satelliteType.SatelliteType)) continue;

        //        ObservationData currentsatdata = currentEpochObservation.GetObservationData(satelliteType);
        //        if (currentsatdata.Contains(GnssDataType.L1)
        //            && currentsatdata.Contains(GnssDataType.L2)
        //            && currentsatdata.Contains(GnssDataType.P1)
        //            && currentsatdata.Contains(GnssDataType.P2))
        //        {
        //            newepoc._satelliteObservations.Add(satelliteType, currentsatdata);
        //            continue;
        //        }               
        //    }
        //    return newepoc;
        //}

        

    }
}
