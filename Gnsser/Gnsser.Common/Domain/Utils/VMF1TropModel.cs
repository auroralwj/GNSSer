//2014.05.22, lly, created
//2015.05.28, lly, edit ,第96行改为（少了一个时间）： this.StaVmf1Value = this.DataSourceProvider.Vmf1DataService.Get(stanam, VMF1gpstime);

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Times;
using Gnsser.Data;
using Geo.Times;

namespace Gnsser
{
    /// <summary>
    /// Tropospheric model based in the VMF1 mapping functions.
    /// </summary>
    public class VMF1TropModel
    {
        double VMF1Height = 0.0;
        double VMF1Lat = 0.0;  //degree
        Time VMF1gpstime;
        string stanam;
        bool ValidHeight = false;
        bool ValidLat = false;
        bool ValidStaNam = false;
        bool Valid; //true only if current model parameters are valid
        // private bool Valid = false;

        bool Validgpstime = true;
        DataSourceContext DataSourceProvider;
        List<Vmf1Value> StaVmf1Value = new List<Vmf1Value>();
        /// <summary>
        /// Default constructor
        /// </summary>
        public VMF1TropModel()
        {
            ValidHeight = false;
            ValidLat = false;
            Validgpstime = false;
        }
        public VMF1TropModel(string StaNam, double ht, double lat, Time gpstime)
        {
            SetStationName(StaNam);
            SetReceiverHeight(ht);
            SetReceiverLatitude(lat);
            SetGpsTime(gpstime);
            StaVmf1Value = null;
        }
        /// <summary>
        /// Constructor to create a Neill trop model providing the height of
        /// the receiver above mean sea level (as defined by ellipsoid model), its latitude and the secondOfWeek of year.
        /// </summary>
        /// <param name="ht">Height of the receiver above mean sea level, in meters.</param>
        /// <param name="lat"> Latitude of receiver, in degrees.</param>
        /// <param name="doy">Day of year.</param>
        public VMF1TropModel(DataSourceContext DataSouceProvider, string StaNam, double ht, double lat, Time gpstime)
        {
            SetStationName(StaNam);
            SetReceiverHeight(ht);
            SetReceiverLatitude(lat);
            SetGpsTime(gpstime);
            StaVmf1Value = null;
            this.DataSourceProvider = DataSouceProvider;
        }

        public bool isValid() { return Valid; }

        /// <summary>
        /// Compute and return the full tropospheric delay. 
        /// The receiver height, latitude and Day oy Year must has been set before using the appropriate constructor or the provided methods.
        /// </summary>
        /// <param name="elevation"> Elevation of satellite as seen at receiver,in degrees</param>
        /// <returns></returns>
        public double Correction(double elevation, ref double wetmap)
        {
            if (!Valid)
            {
                if (!ValidLat)
                {
                    throw new Exception("Invalid Neill trop model: Rx Latitude");
                }

                if (!ValidHeight)
                {
                    throw new Exception("Invalid Neill trop model: Rx Height");
                }

                if (!Validgpstime)
                {
                    throw new Exception("Invalid Neill trop model: day of year");
                }
            }

            this.StaVmf1Value = this.DataSourceProvider.Vmf1DataService.Get(stanam, VMF1gpstime);

            //this.StaVmf1Value = this.DataSourceProvider.Vmf1DataService.Get(stanam);
            //VMF1Value satInfo = VMF1Infos.Find(m => m.stanam.Equals(staname));
            // Neill mapping functions work down to 3 degrees of elevation
            if (elevation < 3.0)
            {
                return 0.0;
            }

            double map_dry = Dry_Mapping_Function(elevation);
            double map_wet = Wet_Mapping_Function(elevation);

            wetmap = map_wet;
            // Compute tropospheric delay

            double tropDelay = (Dry_Zenith_Delay() * map_dry) + (Wet_Zenith_Delay() * map_wet);
            return tropDelay;
        }

        // Compute and return the zenith delay for the dry component of
        // the troposphere.  
        public double Dry_Zenith_Delay()
        {
            if (!Valid)
            {
                throw new Exception("Invalid model");
            }
            double Time = VMF1gpstime.TickTime.SecondsOfDay / (24.0 * 3600);// (double)((VMF1gpstime.Hour + VMF1gpstime.Minute / 60 + VMF1gpstime.Calendar.Second / 3600) / 24);
            double DryZenithDelay = 0.0;
            #region
            //algo站 没有收敛
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 2.2234;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 2.2371;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 2.2480;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 2.2433;
            //}
            //ajac站 没有收敛
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 2.3117;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 2.3078;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 2.3059;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 2.3058;
            //}
            ////ohi3站 没有收敛
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 2.2837;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 2.2773;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 2.2720;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 2.2664;
            //}
            //ADIS站
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 1.7386;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 1.7425;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 1.7347;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 1.7418;
            //}
            ////aira站 没有收敛
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 2.2464;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 2.2466;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 2.2438;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 2.2482;
            //}
            ////albh站 没有收敛
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 2.3270;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 2.3343;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 2.3346;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 2.3377;
            //}
            ////albh站 没有收敛
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 2.1572;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 2.1490;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 2.1524;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 2.1536;
            //}
            ////alrt站 没有收敛
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 2.2928;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 2.2999;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 2.3026;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 2.2977;
            //}
            ////ankr站 没有收敛
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 2.0814;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 2.0802;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 2.0807;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 2.0814;
            //}
            //amc2站 没有收敛
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 1.8272;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 1.8309;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 1.8323;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 1.8372;
            //}
            //antc
            //这个即是从VMF1文件中读取的，对应为第五列~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 2.1296;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 2.1227;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 2.1218;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 2.1180;
            //}
            ////bako
            //if (Time <= 0.25)
            //{
            //    DryZenithDelay = 2.2638;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    DryZenithDelay = 2.2608;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    DryZenithDelay = 2.2633;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    DryZenithDelay = 2.2623;
            //}
            #endregion
            if (Time <= 0.25)
            {
                //DryZenithDelay = 2.3117;
                DryZenithDelay = StaVmf1Value[0].hzd;
            }
            else if (Time > 0.25 && Time <= 0.50)
            {
                //DryZenithDelay = 2.3078;
                DryZenithDelay = StaVmf1Value[1].hzd;
            }
            else if (Time > 0.50 && Time <= 0.75)
            {
                //DryZenithDelay = 2.3059;
                DryZenithDelay = StaVmf1Value[2].hzd;
            }
            else if (Time > 0.75 && Time < 1.00)
            {
                //DryZenithDelay = 2.3058;
                DryZenithDelay = StaVmf1Value[3].hzd;
            }
            return DryZenithDelay;
        }

        // Compute and return the zenith delay for the wet component of
        // the troposphere.
        public double Wet_Zenith_Delay()
        {
            if (!Valid)
            {
                throw new Exception("Invalid model");
            }
            double Time = VMF1gpstime.TickTime.SecondsOfDay / (24.0 * 3600);// (double)((VMF1gpstime.Calendar.Hour + VMF1gpstime.Calendar.Minute / 60 + VMF1gpstime.Calendar.Second / 3600) / 24);
            double WetZenithDelay = 0.0;
            #region
            //algo站 没有收敛
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.0299;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay = 0.0138;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay = 0.0150;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay = 0.0163;
            //}
            ////algo站 没有收敛
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.0689;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay = 0.0721;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay = 0.0812;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay = 0.1250;
            //}
            ////ohi3站 没有收敛
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.0581;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay = 0.0644;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay = 0.0815;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay = 0.0809;
            //}
            //ADIS站
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.0412;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay =  0.0312;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay =  0.0289;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay =  0.0445;
            //}

            ////aira站 没有收敛
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.0722;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay = 0.0641;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay = 0.0510;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay = 0.0737;
            //}

            ////albh站 没有收敛
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.0845;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay = 0.0611;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay = 0.0476;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay = 0.0530;
            //}
            ////alic站 没有收敛
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.1202;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay = 0.1078;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay = 0.0949;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay = 0.0702;
            //}
            ////alrt站 没有收敛
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.0091;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay = 0.0100;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay = 0.0137;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay = 0.0163;
            //}
            ////ankr站 没有收敛
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.0552;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay = 0.0577;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay = 0.0558;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay = 0.0534;
            //}
            ////amc2站
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.0297;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay = 0.0288;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay = 0.0280;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay = 0.0277;
            //}
            //antc站
            //这个即是从VMF1文件中读取的，对应第六列
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.0587;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay = 0.0435;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay = 0.0364;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay = 0.0599;
            //}
            ////bako站
            //if (Time <= 0.25)
            //{
            //    WetZenithDelay = 0.3271;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    WetZenithDelay = 0.3657;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    WetZenithDelay = 0.3678;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    WetZenithDelay = 0.3563;
            //}
            #endregion
            //ajac站 没有收敛
            if (Time <= 0.25)
            {
                //WetZenithDelay = 0.0689;
                WetZenithDelay = StaVmf1Value[0].wzd;
            }
            else if (Time > 0.25 && Time <= 0.50)
            {
                //WetZenithDelay = 0.0721;
                WetZenithDelay = StaVmf1Value[1].wzd;
            }
            else if (Time > 0.50 && Time <= 0.75)
            {
                //WetZenithDelay = 0.0812;
                WetZenithDelay = StaVmf1Value[2].wzd;
            }
            else if (Time > 0.75 && Time < 1.00)
            {
                //WetZenithDelay = 0.1250;
                WetZenithDelay = StaVmf1Value[3].wzd;
            }
            return WetZenithDelay;
        }

        /// <summary>
        /// Compute and return the mapping function for dry component of the troposphere.
        /// </summary>
        /// <param name="elevation"> Elevation of satellite as seen at receiver, in degrees</param>
        /// <returns></returns>
        public double Dry_Mapping_Function(double elevation)
        {
            if (!Valid)
            {
                if (!ValidLat)
                {
                    throw new Exception("Invalid Neill trop model: Rx Latitude");
                }

                if (!ValidHeight)
                {
                    throw new Exception("Invalid Neill trop model: Rx Height");
                }

                if (!Validgpstime)
                {
                    throw new Exception("Invalid Neill trop model: day of year");
                }
            }

            //用VMF1模型试验
            double Time = VMF1gpstime.TickTime.SecondsOfDay / (24.0 * 3600);// (double)((VMF1gpstime.Calendar.Hour + VMF1gpstime.Calendar.Minute / 60 + VMF1gpstime.Calendar.Second / 3600) / 24);
            double ah = 0.0;
            #region
            //algo 该站没有收敛
            //if (Time <= 0.25)
            //{
            //    ah = 0.00120578 + Time / 0.25 * (0.0011977 - 0.00120578);
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    ah = 0.00119777 + (Time - 0.25) / 0.25 * (0.00119374 - 0.00119777);
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    ah = 0.00119374 + (Time - 0.50) / 0.25 * (0.00119251 - 0.00119374);
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    ah = 0.00119251 + (Time - 0.75) / 0.25 * (0.00119350 - 0.00119251);
            //}

            ////ajac，精度提高了不少
            //if (Time <= 0.25)
            //{
            //    ah = 0.00123463 + Time / 0.25 * (0.00123326 - 0.00123463);
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    ah = 0.00123326 + (Time - 0.25) / 0.25 * (0.00123385 - 0.001123326);
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    ah = 0.00123385 + (Time - 0.50) / 0.25 * (0.00123132 - 0.00123385);
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    ah = 0.00123132 + (Time - 0.75) / 0.25 * (0.00122992 - 0.00123132);
            //}

            //wtza，中间好，两头差
            //if (Time <= 0.25)
            //{
            //    ah = 0.00121524 + Time / 0.25 * (0.00121428 - 0.00121524);
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    ah = 0.00121428 + (Time - 0.25) / 0.25 * (0.00121258 - 0.00121428);
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    ah = 0.00121258 + (Time - 0.50) / 0.25 * (0.00120855 - 0.00121258);
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    ah = 0.00120855 + (Time - 0.75) / 0.25 * (0.00120504 - 0.001230855);
            //}

            ////ohi3
            //if (Time <= 0.25)
            //{
            //    //ah = 0.00122798 + Time / 0.25 * (0.00122753 - 0.00122798);
            //    ah = 0.00122798;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    //ah = 0.00122753 + (Time - 0.25) / 0.25 * (0.00122599 - 0.00122753);
            //    ah = 0.00122753;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    //ah = 0.00122599 + (Time - 0.50) / 0.25 * (0.00122368 - 0.00122599);
            //    ah = 0.00122599;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    //ah = 0.00122368 + (Time - 0.75) / 0.25 * (0.00122222 - 0.001230855);
            //    ah = 0.00122368;
            //}

            //sulp
            //if (Time <= 0.25)
            //{
            //    ah = 0.00121704;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    ah = 0.00121755;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    ah = 0.001221913;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    ah = 0.00122305;
            //}

            //AADIS站
            //if (Time <= 0.25)
            //{
            //    ah = 0.00122442 + Time / 0.25 * (0.00122440 - 0.00122442);
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    ah = 0.00122440 + (Time - 0.25) / 0.25 * (0.00122748 - 0.00122440);
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    ah = 0.00122748 + (Time - 0.5) / 0.25 * (0.00122616 - 0.00122748);
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    ah = 0.00122616 + (Time - 0.75) / 0.25 * (0.00122528 - 0.00122616);
            //}

            ////ohi3
            //if (Time <= 0.25)
            //{
            //    //ah = 0.00122798 + Time / 0.25 * (0.00122753 - 0.00122798);
            //    ah = 0.00123934;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    //ah = 0.00122753 + (Time - 0.25) / 0.25 * (0.00122599 - 0.00122753);
            //    ah = 0.00123938;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    //ah = 0.00122599 + (Time - 0.50) / 0.25 * (0.00122368 - 0.00122599);
            //    ah = 0.00124052;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    //ah = 0.00122368 + (Time - 0.75) / 0.25 * (0.00122222 - 0.001230855);
            //    ah = 0.00124023;
            //}
            //albh
            //if (Time <= 0.25)
            //{
            //    //ah = 0.00122798 + Time / 0.25 * (0.00122753 - 0.00122798);
            //    ah = 0.00122448;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    //ah = 0.00122753 + (Time - 0.25) / 0.25 * (0.00122599 - 0.00122753);
            //    ah = 0.00122780;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    //ah = 0.00122599 + (Time - 0.50) / 0.25 * (0.00122368 - 0.00122599);
            //    ah = 0.00123084;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    //ah = 0.00122368 + (Time - 0.75) / 0.25 * (0.00122222 - 0.001230855);
            //    ah = 0.00123225;
            //}
            ////alic
            //if (Time <= 0.25)
            //{
            //    ah = 0.00126748;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    ah = 0.00127249;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    ah = 0.0012725;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    ah = 0.00127028;
            //}
            ////alrt
            //if (Time <= 0.25)
            //{
            //    ah = 0.00116149;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    ah = 0.00116157;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    ah = 0.00116323;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    ah = 0.00116357;
            //}
            ////ankr
            //if (Time <= 0.25)
            //{
            //    ah = 0.00120839;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    ah = 0.00120714;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    ah = 0.00120706;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    ah = 0.00120711;
            //}
            ////amc2
            //if (Time <= 0.25)
            //{
            //    ah = 0.00117859;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    ah = 0.00117743;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    ah = 0.00117728;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    ah = 0.00117604;
            //}
            //antc
            //这个即是从VMF1文件中读取的，对应为第三列~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //if (Time <= 0.25)
            //{
            //    ah = 0.00124653;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    ah = 0.00124975;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    ah = 0.00125005;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    ah = 0.00125257;
            //}
            //bako
            //if (Time <= 0.25)
            //{
            //    ah = 0.00127629;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    ah = 0.00127603;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    ah = 0.00127607;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    ah = 0.00127573;
            //}
            #endregion
            //ajac，精度提高了不少
            if (Time <= 0.25)
            {
                //ah = 0.00123463 + Time / 0.25 * (0.00123326 - 0.00123463);
                ah = StaVmf1Value[0].ah;
            }
            else if (Time > 0.25 && Time <= 0.50)
            {
                //ah = 0.00123326 + (Time - 0.25) / 0.25 * (0.00123385 - 0.001123326);
                ah = StaVmf1Value[1].ah;
            }
            else if (Time > 0.50 && Time <= 0.75)
            {
                //ah = 0.00123385 + (Time - 0.50) / 0.25 * (0.00123132 - 0.00123385);
                ah = StaVmf1Value[2].ah;
            }
            else if (Time > 0.75 && Time < 1.00)
            {
                //ah = 0.00123132 + (Time - 0.75) / 0.25 * (0.00122992 - 0.00123132);
                ah = StaVmf1Value[3].ah;
            }
            int dmjd = (int)VMF1gpstime.MJulianDays; //2013年1月1日对应的约化儒略日,这个地方要测试

            double dlat = VMF1Lat;//测站纬度

            double ht = VMF1Height;//正高

            double zd = Math.PI / 2 - elevation / 180 * Math.PI;//天顶距

            int doy = dmjd - 44239 + 1 - 28;
            double bh = 0.0029;
            double c0h = 0.062;

            double phh = 0.0;
            double c11h = 0.0;
            double c10h = 0.0;
            if (dlat < 0) //南半球
            {
                phh = Math.PI;
                c11h = 0.007;
                c10h = 0.002;
            }
            else //北半球
            {
                phh = 0.0;
                c11h = 0.005;
                c10h = 0.001;
            }
            double ch = c0h + ((Math.Cos(doy / 365.25 * 2.0 * Math.PI + phh) + 1) * c11h / 2 + c10h) * (1 - Math.Cos(dlat));
            double sine = Math.Sin(Math.PI / 2 - zd);
            double beta = bh / (sine + ch);
            double gamma = ah / (sine + beta);
            double topcon = (1.0 + ah / (1.0 + bh / (1.0 + ch)));
            double vmf1h = topcon / (sine + gamma);

            //height correction for hydrotatic part [Niell, 1996] 
            double a_ht = 2.53 * Math.Pow(10, -5);
            double b_ht = 5.49 * Math.Pow(10, -3);
            double c_ht = 1.14 * Math.Pow(10, -3);
            double hs_km = ht / 1000.0;
            beta = b_ht / (sine + c_ht);
            gamma = a_ht / (sine + beta);
            topcon = (1.0 + a_ht / (1.0 + b_ht / (1.0 + c_ht)));
            double ht_corr_coef = 1.0 / sine - topcon / (sine + gamma);
            double ht_corr = ht_corr_coef * hs_km;
            vmf1h = vmf1h + ht_corr;

            return vmf1h;
        }

        /// <summary>
        /// Compute and return the mapping function for wet component of the troposphere.
        /// </summary>
        /// <param name="elevation"> Elevation of satellite as seen at receiver, in degrees</param>
        /// <returns></returns>
        public double Wet_Mapping_Function(double elevation)
        {
            if (!Valid)
            {
                if (!ValidLat)
                {
                    throw new Exception("Invalid Neill trop model: Rx Latitude");
                }

                if (!ValidHeight)
                {
                    throw new Exception("Invalid Neill trop model: Rx Height");
                }

                if (!Validgpstime)
                {
                    throw new Exception("Invalid Neill trop model: day of year");
                }
            }

            double zd = Math.PI / 2 - elevation / 180 * Math.PI;//天顶距
            double sine = Math.Sin(Math.PI / 2 - zd);
            double Time = VMF1gpstime.TickTime.SecondsOfDay / (24.0 * 3600);//(double)((VMF1gpstime.Calendar.Hour + VMF1gpstime.Calendar.Minute / 60 + VMF1gpstime.Calendar.Second / 3600) / 24);
            double aw = 0.0;
            #region
            ////ALGO站
            //if (Time <= 0.25)
            //{
            //    aw = 0.00047040 + Time / 0.25 * (0.00047945 - 0.00047040);
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00047945 + (Time - 0.25) / 0.25 * (0.00056550 - 0.00047945);
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00056550 + (Time - 0.5) / 0.25 * (0.00055101 - 0.00056550);
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00055101 + (Time - 0.75) / 0.25 * (0.00053924 - 0.00055101);
            //}
            //ajac站
            //if (Time <= 0.25)
            //{
            //    aw = 0.00048931 + Time / 0.25 * (0.00048242 - 0.00048931);
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00048242 + (Time - 0.25) / 0.25 * (0.00052885 - 0.00048242);
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00052885 + (Time - 0.5) / 0.25 * (0.00056990 - 0.00052885);
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00056990 + (Time - 0.75) / 0.25 * (0.00057057 - 0.00056990);
            //}
            ////ohi3站
            //if (Time <= 0.25)
            //{
            //    aw = 0.00059881;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00060027;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00060933;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00054278;
            //}
            //ADIS站
            //if (Time <= 0.25)
            //{
            //    aw = 0.00037846 + Time / 0.25 * (0.00038266 - 0.00037846);
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00038266 + (Time - 0.25) / 0.25 * (0.00039034 - 0.00038266);
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00039034 + (Time - 0.5) / 0.25 * (0.00038590 - 0.00039034);
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00038590 + (Time - 0.75) / 0.25 * (0.00037967 - 0.00122616);
            //}
            ////aira站
            //if (Time <= 0.25)
            //{
            //    aw = 0.00044187;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00043510;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00044983;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00046206;
            //}

            ////albh站
            //if (Time <= 0.25)
            //{
            //    aw = 0.00058374;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00052999;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00049587;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00053396;
            //}
            ////alic站
            //if (Time <= 0.25)
            //{
            //    aw = 0.00060577;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00060076;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00059510;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00062564;
            //}
            ////alrt站
            //if (Time <= 0.25)
            //{
            //    aw = 0.00057729;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00061985;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00064366;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00065823;
            //}
            ////ankr站
            //if (Time <= 0.25)
            //{
            //    aw = 0.00050510;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00050560;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00049303;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00048352;
            //}
            ////amc2站
            //if (Time <= 0.25)
            //{
            //    aw = 0.00048367;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00047389;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00047598;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00046897;
            //}
            //antc站
            //这个即是从VMF1文件中读取的，对应为第四列~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //if (Time <= 0.25)
            //{
            //    aw = 0.00043763;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00047989;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00048236;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00046783;
            //}
            //antc站
            //这个即是从VMF1文件中读取的，对应
            //if (Time <= 0.25)
            //{
            //    aw = 0.00064655;
            //}
            //else if (Time > 0.25 && Time <= 0.50)
            //{
            //    aw = 0.00065400;
            //}
            //else if (Time > 0.50 && Time <= 0.75)
            //{
            //    aw = 0.00067795;
            //}
            //else if (Time > 0.75 && Time < 1.00)
            //{
            //    aw = 0.00067451;
            //}
            #endregion
            //ajac站
            if (Time <= 0.25)
            {
                //aw = 0.00048931 + Time / 0.25 * (0.00048242 - 0.00048931);
                aw = StaVmf1Value[0].aw;
            }
            else if (Time > 0.25 && Time <= 0.50)
            {
                //aw = 0.00048242 + (Time - 0.25) / 0.25 * (0.00052885 - 0.00048242);
                aw = StaVmf1Value[1].aw;
            }
            else if (Time > 0.50 && Time <= 0.75)
            {
                //aw = 0.00052885 + (Time - 0.5) / 0.25 * (0.00056990 - 0.00052885);
                aw = StaVmf1Value[2].aw;
            }
            else if (Time > 0.75 && Time < 1.00)
            {
                //aw = 0.00056990 + (Time - 0.75) / 0.25 * (0.00057057 - 0.00056990);
                aw = StaVmf1Value[3].aw;
            }
            double bw = 0.00146;
            double cw = 0.04391;
            double beta = bw / (sine + cw);
            double gamma = aw / (sine + beta);
            double topcon = (1.0 + aw / (1.0 + bw / (1.0 + cw)));
            double vmf1w = topcon / (sine + gamma);

            return vmf1w;
        }

        /// <summary>
        /// Define the receiver height; this is required before calling correction() or any of the zenith_delay routines.
        /// </summary>
        /// <param name="ht"> Height of the receiver above mean sea level,in meters.</param>
        public void SetReceiverHeight(double ht)
        {
            VMF1Height = ht;
            ValidHeight = true;
            // Change the value of field "valid" if everything is already set
            Valid = ValidHeight && ValidLat && Validgpstime;
        }

        /// <summary>
        /// Define the receiver latitude; this is required before calling correction() or any of the zenith_delay routines.
        /// </summary>
        /// <param name="lat"> Latitude of receiver, in degrees.</param>
        public void SetReceiverLatitude(double lat)
        {
            VMF1Lat = lat;
            ValidLat = true;
            // Change the value of field "valid" if everything is already set
            Valid = ValidHeight && ValidLat && Validgpstime;
        }

        /// <summary>
        /// Set the time when tropospheric correction will be computed for,in days of the year.
        /// </summary>
        /// <param name="doy">Day of the year</param>
        public void SetGpsTime(Time gpstime)
        {
            if ((gpstime.DayOfYear >= 1) && (gpstime.DayOfYear <= 366))
            {
                Validgpstime = true;
            }
            else
            {
                Validgpstime = false;
            }
            VMF1gpstime = gpstime;
            // Change the value of field "valid" if everything is already set
            Valid = ValidHeight && ValidLat && Validgpstime;
        }

        public void SetStationName(string StaNam)
        {
            if (StaNam != null)
            {
                ValidStaNam = true;
            }
            else
            {
                ValidStaNam = false;
            }
            this.stanam = StaNam;
            // Change the value of field "valid" if everything is already set
            Valid = ValidHeight && ValidLat && Validgpstime && ValidStaNam;
        }
    }
}
