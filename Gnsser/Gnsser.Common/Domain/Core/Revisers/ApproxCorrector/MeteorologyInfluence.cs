using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Gnsser.Times;
using System.Text;
using Geo;
using Geo.Times; 


namespace Gnsser
{
    /// <summary>
    /// 大气影响，计算对流层延迟，季节性影响等。采用的角度单位均为弧度（Rad）。
    /// met object for gps
    /// substitute Position and Rpos objects
    /// meteorology  [mi:tiəˈrɔlədʒi] n.气象学
    /// </summary>
    public class MeteorologyInfluence
    {
        /// <summary>
        /// 对流层模型改正
        /// </summary>
        /// <param name="lat_rad"></param>
        /// <param name="height"></param>
        /// <param name="verticalAngle_rad"></param>
        /// <param name="mytime"></param>
        /// <returns></returns>
        public static double TroposphereDelay(double lat_rad, double height, double verticalAngle_rad, Time mytime)
        {
            return new MeteorologyInfluence().TropDelay(lat_rad, height, verticalAngle_rad, mytime);
        } 

        //private static double RAD = 180.0 / Consts.PI;      //*** radians to deg
        private static double MDJ2000 = 51544.0;         //*** MJD of 1-jan-2000

        /// <summary>
        /// temperature in Celsius摄氏温度 [计量]
        /// </summary>
        public double Temperature_Celsius { get; set; }      
        /// <summary>
        /// pressure in mbar
        /// </summary>
        public double Pressure_Mbar { get; set; }      
        /// <summary>
        /// relative humidity 0 to 1 (not percent)
        /// </summary>
        public double RelativeHumidity { get; set; }      
        /// <summary>
        /// 干分量天顶延迟（m）
        /// dry atmosphere zenith path delay [m] 
        /// </summary>
        public double DryZenithDelay { get; set; }           
        /// <summary>
        /// 湿分类延迟（m）
        ///  wet zenith delay [m]
        /// </summary>
        public double WetZenithDelay { get; set; }         
      
        //*** compute functions
        /// <summary>
        /// compute Tropo delay in meters
        /// latitude and vert angle input in radians
        /// ortho height in meters
        /// </summary>
        /// <param name="lat_rad"></param>
        /// <param name="height"></param>
        /// <param name="verticalAngle_rad"></param>
        /// <param name="tsec"></param>
        /// <param name="mytime"></param>
        /// <returns></returns>
        public double TropDelay(double lat_rad, double height, double verticalAngle_rad, Time mytime)
        {
            //*** seasonal model for temp, pres, and r.h. 
            if ((lat_rad < -1.571) || (lat_rad > 1.571))  throw new Exception("lat error in tropdelay()"); 
            if ((height < -150.0) || (height > 5000.0))  throw new Exception("oht error in tropdelay()"); 
            if ((verticalAngle_rad < -0.1) || (verticalAngle_rad > 1.571)) throw new Exception("v-ang error in tropdelay()");  

            MetSeason(height, lat_rad, mytime);    //*** seasonal model (herring)

            //*** given temp, pres, and r.h. --> get tdelay in meters 
            if ((Temperature_Celsius < -50.0) || (Temperature_Celsius > 50.0))    throw new Exception("temp error in tropdelay()"); 
            if ((Pressure_Mbar < 500.0) || (Pressure_Mbar > 1100.0))  throw new Exception("pres error in tropdelay() pres=" + Pressure_Mbar); 
            if ((RelativeHumidity < 0.0) || (RelativeHumidity > 1.0))   throw new Exception("rh error in tropdelay()"); 

            return Tropo(lat_rad, height, verticalAngle_rad);
        }

        /// <summary>
        ///  seasonal model (TProduct. Herring, MIT)
        /// Name changed, call modified, adapted for use by Page4, J. Ray (95.07.25)
        /// height    height of site (meters) above geoid
        /// lat_rad   latitude of site (radians)
        /// t      time (weekSeconds-- jul. time, sec) (converted thru Gtime object)
        /// </summary>
        /// <param name="oht"></param>
        /// <param name="glat"></param>
        /// <param name="tsec"></param>
        /// <param name="mytime"></param>
        public void MetSeason(double height, double lat_rad,   Time gpsTime )
        {  
            double hgt, epoch, dt1, tempk;

            hgt = height / 1000.0;                       //*** funcKeyToDouble input height to km

            //mytime.jtsciv(tsec);
            //mytime.civmjd();
            //epoch = (mytime.GetModifiedJulianDay() - MDJ2000) + mytime.GetDigitaOfDay(); 

            epoch  =(double)( gpsTime.MJulianDays - (Decimal)MDJ2000);
            //*** estimate temperature -- get seasonal argument

            dt1 = (epoch / 365.25) * 2.0 * CoordConsts.PI;
            Temperature_Celsius = (-20.5 + 48.4 * Math.Cos(lat_rad) - 3.1 * hgt) +
                   (-14.3 + 3.3 * hgt) * Math.Sin(lat_rad) * Math.Cos(dt1) +
                   (-4.7 + 1.1 * hgt) * Math.Sin(lat_rad) * Math.Sin(dt1);

            //*** compute the pressure based on standard lapse rate 

            tempk = Temperature_Celsius + 273.15;                           //*** funcKeyToDouble to Kelvin
            Pressure_Mbar = 1013.25 * Math.Pow((tempk / (tempk + 6.5 * hgt)), 5.26);

            //*** relative humidity (plain default value) 
            RelativeHumidity = 0.5;
        }

        /// <summary>
        ///   compute troposphere delay (saastamoinen with herring mapping) 
        /// verticalAngle_rad      -- vertical angle (radians)
        /// lat_rad    -- geodetic latitude (radians)
        /// height    -- orthometric height of rcvr. (meters)
        /// </summary>
        /// <param name="lat_rad"></param>
        /// <param name="height"></param>
        /// <param name="verticalAngle_rad"></param>
        /// <returns></returns>
        public double Tropo(double lat_rad, double height, double verticalAngle_rad)
        { 
            double hs, sine, cgla, ts10;
            double ah, bh, ch, mh, aw, bw, cw, mw;

            //*** zenith delay section

            Saast(lat_rad, height);              //*** saastamoinen,

            //*** mapping function section
            //*** herring: 1992, refraction of transatmospheric signals in geodesy
            //*** note: mapping fcns derived: lat 27N-65N, and ht. 0-1.6 km

            hs = height / 1000.0;       //*** hs -- orthometric ht of rcvr.(kilometers)
            sine = Math.Sin(verticalAngle_rad);
            cgla = Math.Cos(lat_rad);
            ts10 = Temperature_Celsius - 10.0;        //*** offset to surface temperature (Celsius)

            //*** hydrostatic mapping fcn  (herring eq. 8)  (RMS ~ 3-10 mm)

            ah = (1.2330 + 0.0139 * cgla - 0.0209 * hs + 0.00215 * ts10) * 1.0e-3;
            bh = (3.1612 - 0.1600 * cgla - 0.0331 * hs + 0.00206 * ts10) * 1.0e-3;
            ch = (71.244 - 4.2930 * cgla - 0.1490 * hs - 0.00210 * ts10) * 1.0e-3;

            mh = (1.0 + ah / (1.0 + bh / (1.0 + ch))) /
                 (sine + ah / (sine + bh / (sine + ch)));

            //*** wet mapping fcn  (herring eq. 9)

            aw = (0.583 - 0.011 * cgla - 0.052 * hs + 0.0014 * ts10) * 1.0e-3;
            bw = (1.402 - 0.102 * cgla - 0.101 * hs + 0.0020 * ts10) * 1.0e-3;
            cw = (45.85 - 1.910 * cgla - 1.290 * hs + 0.0150 * ts10) * 1.0e-3;

            mw = (1.0 + aw / (1.0 + bw / (1.0 + cw))) /  (sine + aw / (sine + bw / (sine + cw)));

            //*** total atmospheric delay  (herring eq 3)

            return DryZenithDelay * mh + WetZenithDelay * mw;
        }

        /// <summary> 
        ///  compute wet and dry zenith delay using the saastamoinen formula
        ///* input
        ///* lat   site latitude       [radians]
        ///* oht   orthometric height  [m]
        ///* bp    barometric pressure [millibars]
        ///* tc    temperature         [centigrade]
        ///* rh    relative humidity   [0.0 --> 1.0]
        ///*
        ///* output
        ///* dryzen  dry atmosphere zenith path delay [m]
        ///* wetzen  wet zenith delay [m]
        ///*
        ///*  87-02-15, written by:   j. l. davis
        ///*  91-01-09, mss, standard header.  remove vlbi specific obsCodeode
        ///*  96-04-11, dgm, combined wet and dry, and deleted derivative stuff
        ///*  00-05-24, dgm, java conversion 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="height"></param>
        public void Saast(double lat_rad, double height)
        { 
            double fgrav, tk, pp;
            //*** saastamoinen's function for gravity
            fgrav = 1.0 - 0.00266 * Math.Cos(2.0 * lat_rad) - 0.00028e-3 * height;

            //*** water vapor partial pressure in mbars   (proportional to rh)
            tk = Temperature_Celsius + 273.15;
            pp = RelativeHumidity * 6.11 * Math.Pow((tk / 273.15), -5.3) * Math.Exp(25.2 * Temperature_Celsius / tk);

            //*** zenith delays
            WetZenithDelay = 0.0022768 * (1255.0 / tk + 0.05) * pp / fgrav;
            DryZenithDelay = 0.0022768 * Pressure_Mbar / fgrav;
        }

        /// <summary>
        /// compute pressure vs. temperature
        /// ICAN atmosphere, (eq.176) Handbook of Met. (Berry, Bollay, Beers)
        ///                (pg. 374)   (1945, McGraw Hill)
        /// z in meters
        /// info,p0 in millibars
        /// </summary>
        /// <param name="z"></param>
        /// <param name="p0"></param>
        /// <returns></returns>
        public double Adalap(double z, double p0)
        { 
            double p;
            p0 = 1013.2;                          //***debug -- check this (overwrite?)
            p = p0 * Math.Pow((1.0 - 0.0065 * z / 288.0), 5.2568);
            return p;
        }
    }//***endclass Met








    ///// <summary>
    ///// 高精度的对流层改正
    ///// 对流层延迟由干、湿分量组成，常用天顶方向的干、湿分量和相应的映射函数表示：detDtrop=detDdry*Mdry(E)+detDwet*Mwet(E)
    ///// 采用Saastamoinen模型改正对流层延迟干分量，将湿延迟分量作为未知参数进行估计
    ///// 对流层映射函数采用GMF映射函数
    ///// lat：测站纬度（弧度），h：测站高程(km),P:测站大气压强(mbar)，TProduct:测站温度(k),e：大气中的水汽压(mbar)
    ///// <returns></returns>
    //public static double GetSaastamoinenDryTropCorrectValue(Time gpsTime, XYZ receiverVector, XYZ receiverXyz)
    //{
    //    Polar info = CoordTransformer.GetLocalPolar(receiverVector, receiverXyz);
    //    Geo.Coordinates.GeoCoord geoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(receiverXyz, Geo.Coordinates.AngelUnit.Radian);
    //    //double troCorect = MeteorologyInfluence.TroposphereDelay(geoCoord.Lat, geoCoord.Height, info.ElevatAngle, gpsTime)
    //    double f = SaastamoinenF(geoCoord.Lat, geoCoord.Height * 10e-3);
    //    double P = 1013.25;//这里默认采用标准大气压强1.013*10e5Pa=1013.25hPa=1.01325bar
    //    double detDdry = 0.0022768 * P / f;
    //    return detDdry;
    //}


    ///// <summary>
    ///// Saastamoinen模型的辅助函数
    ///// lat：测站纬度（弧度），h：测站高程(km)
    ///// <returns></returns>
    //private static double SaastamoinenF(double lat, double h)
    //{
    //    double f = 1 - 0.00266 * Math.Cos(2 * lat) - 0.00028 * h;
    //    return f;
    //}


    ///// <summary>
    ///// GMF映射函数
    ///// 干分量延迟映射函数和湿分量延迟映射函数
    ///// fai：测站纬度（弧度）,h：测站高程(km)，E：高度角
    ///// <returns></returns>
    //private static double GMF(double fai,double h,double E,Time gpsTime)
    //{
    //    double adry = 0.0, awet = 0.0;
    //    double bdry = 0.0029, bwet = 0.000146;
    //    double cdry = 0.0, cwet = 0.04391;

    //    double phh, c11h, c10h, doy;
    //    if (fai >= 0)//测站处于北半球
    //    {
    //        phh = 0.0;
    //        c11h = 0.005;
    //        c10h = 0.001;
    //        doy = gpsTime.DayOfYear;
    //    }
    //    else//南半球
    //    {
    //        phh = PI;
    //        c11h = 0.007;
    //        c10h = 0.002;
    //        doy = gpsTime.DayOfYear - 180;//年纪日，南北半球季节差异，南半球去掉180（阮的论文）
    //    }
    //    double aht = 2.53 * 10e-5;
    //    double bht = 5.49 * 10e-3;
    //    double cht = 1.14 * 10e-3;
    //    cdry = 0.062 + ((Math.Cos((doy - 28) * 2 * PI / 365.25 + phh) + 1) * c11h / 2 + c10h) * (1 - Math.Cos(fai));
    //    //九阶球谐级数形式表示模型常数
    //    double api = 0.0, bpi = 0.0;
    //    for (int n = 0; n <= 9; n++)
    //    {
    //        for (int m = 0; m <= n; m++)
    //        {
    //            double Pnm = sPhereSeries(n, m, fai);
    //            api += Pnm * Math.Cos(m * fai);
    //            bpi += Pnm * Math.Sin(m * fai);
    //        }
    //    }
    //    //adry,awet
    //    for (int time = 1; time <= 55; time++)
    //    { }
    //}

    ///// <summary>
    ///// 球谐函数P(n+1,m+1)
    ///// <returns></returns>
    //private static double sPhereSeries(int n, int m, double fai)
    //{
    //    int s = ((n - m) / 2);
    //    double count = 0.0;

    //    for (int k = 0; k <= s; k++)
    //    {
    //        double sinfai = Math.Sin(Math.Pow(fai, n - m - 2 * k));
    //        count += Math.Pow(-1, k) * multplicative(2 * n - 2 * m + 1) * sinfai / (multplicative(k + 1) * multplicative(n - k + 1) * multplicative(n - m - 2 * k + 1));
    //    }
    //    double P = Math.Pow(2, -n) * Math.Pow((1 - Math.Sin(fai) * Math.Sin(fai)), m / 2) * count;
    //    return P;
    //}

    //private static int multplicative(int m)
    //{
    //    int s = 1;
    //    for (int time = 1; time <= m; time++)
    //    {
    //        s *= time;
    //    }
    //    return s;
    //}


   
}
