
using Gnsser.Times;
using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Geo;
using Geo.Times; 

namespace Gnsser
{
    /// <summary>
    /// 对流程， 伪距改正数
    /// </summary>
    public class TropoCorrection  
    {

        private static int maxPrn = 32;         //*** 38 are possible
        private static int maxTimeRow = 48;         //*** time rows (2-3 days)

        private static double LIGHT_SPEED = 299792458;        //*** exactly
        private static double EARTH_ROT_RAD_PER_SEC = 7.2921151467e-5;    //*** rad/sec
        private static double PI = 3.1415926535898e0;  //*** exactly
        private static double RAD = 180.0 / CoordConsts.PI;      //*** radians to deg

        /// <summary> NeillMF投影函数
        /// doy:年积日，lat:latitude(单位：弧度)，long:longitude（单位：弧度），h: height(单位：米），elev: elevation angle 高度角(单位：弧度）
        /// <returns></returns>
        public static double[] NeillMF(int doy, double lat, double h, double elev)
        {
            double[] hmf = new double[4];

            double day2rad = 2 * PI / 365.25;// Coefficient for changing doy to radian
            double deg2rad = PI / 180;// Coefficient for changing degrees to radians，如果是角度的话，需要用此变量更新为弧度单位
            double[] lat_hmf = new double[5] { 15 * deg2rad, 30 * deg2rad, 45 * deg2rad, 60 * deg2rad, 75 * deg2rad };//Latitude array for the hydrostatic mapping function coefficients

            double[][] abc_avg = new double[5][];//Average of coefficients a, b and c corresponding to the given latitude.
            abc_avg[0] = new double[3] { 1.2769934 * 1e-3, 2.9153695 * 1e-3, 62.610505 * 1e-3 };
            abc_avg[1] = new double[3] { 1.2683230 * 1e-3, 2.9152299 * 1e-3, 62.837393 * 1e-3 };
            abc_avg[2] = new double[3] { 1.2465397 * 1e-3, 2.9288445 * 1e-3, 63.721774 * 1e-3 };
            abc_avg[3] = new double[3] { 1.2196049 * 1e-3, 2.9022565 * 1e-3, 63.824265 * 1e-3 };
            abc_avg[4] = new double[3] { 1.2045996 * 1e-3, 2.9024912 * 1e-3, 64.258455 * 1e-3 };
            double[][] abc_amp = new double[5][];//Amplitude of coefficients a, b and c corresponding to the given latitude.
            abc_amp[0] = new double[3] { 0.0, 0.0, 0.0 };
            abc_amp[1] = new double[3] { 1.2709626 * 1e-5, 2.1414979 * 1e-5, 9.0128400 * 1e-5 };
            abc_amp[2] = new double[3] { 2.6523662 * 1e-5, 3.0160779 * 1e-5, 4.3497037 * 1e-5 };
            abc_amp[3] = new double[3] { 3.4000452 * 1e-5, 7.2562722 * 1e-5, 84.795348 * 1e-5 };
            abc_amp[4] = new double[3] { 4.1202191 * 1e-5, 11.723375 * 1e-5, 170.37206 * 1e-5 };

            //Height corrcteion
            double a_ht = 0.0000253;
            double b_ht = 0.00549;
            double c_ht = 0.00114;

            double[] lat_wmf = lat_hmf;//Wet Mapping Function
            double[][] abc_w2po = new double[5][];
            abc_w2po[0] = new double[3] { 5.8021897 * 1e-4, 1.4275268 * 1e-3, 4.3472961 * 1e-2 };
            abc_w2po[1] = new double[3] { 5.6794847 * 1e-4, 1.5138625 * 1e-3, 4.6729510 * 1e-2 };
            abc_w2po[2] = new double[3] { 5.8118019 * 1e-4, 1.4572752 * 1e-3, 4.3908931 * 1e-2 };
            abc_w2po[3] = new double[3] { 5.9727542 * 1e-4, 1.5007428 * 1e-3, 4.4626982 * 1e-2 };
            abc_w2po[4] = new double[3] { 6.1641693 * 1e-4, 1.7599082 * 1e-3, 5.4736038 * 1e-2 };

            double hs_km = h / 1000;//以km为单位
            double doy_atm = doy - 28;
            if (lat < 0)
            {
                doy_atm = doy_atm - 365.25 / 2;
                lat = Math.Abs(lat);
            }
            double doyr_atm = doy_atm * day2rad;//以弧度为单位
            double cost = Math.Cos(doyr_atm);
            double a = 0, b = 0, c = 0;
            if (lat <= lat_hmf[0])
            {
                a = abc_avg[0][0] + abc_amp[0][0] * cost;
                b = abc_avg[0][1] + abc_amp[0][1] * cost;
                c = abc_avg[0][2] + abc_amp[0][2] * cost;
            }
            for (int i = 0; i < 4; i++)
            {
                if (lat >= lat_hmf[i] && lat <= lat_hmf[i + 1])
                {
                    double dlat = (lat - lat_hmf[i]) / (lat_hmf[i + 1] - lat_hmf[i]);
                    double daavg = abc_avg[i + 1][0] - abc_avg[i][0];
                    double daamp = abc_amp[i + 1][0] - abc_amp[i][0];
                    double aavg = abc_avg[i][0] + dlat * daavg;
                    double aamp = abc_amp[i][0] + dlat * daamp;
                    a = aavg + aamp * cost;// + or - time don't sure

                    double dbavg = abc_avg[i + 1][1] - abc_avg[i][1];
                    double dbamp = abc_amp[i + 1][1] - abc_amp[i][1];
                    double bavg = abc_avg[i][1] + dlat * dbavg;
                    double bamp = abc_amp[i][1] + dlat * dbamp;
                    b = bavg + bamp * cost;

                    double dcavg = abc_avg[i + 1][2] - abc_avg[i][2];
                    double dcamp = abc_amp[i + 1][2] - abc_amp[i][2];
                    double cavg = abc_avg[i][2] + dlat * dcavg;
                    double camp = abc_amp[i][2] + dlat * dcamp;
                    c = cavg + camp * cost;/////与出处不同
                }
            }
            if (lat >= lat_hmf[4])
            {
                a = abc_avg[4][0] + abc_amp[4][0] * cost;
                b = abc_avg[4][1] + abc_amp[4][1] * cost;
                c = abc_avg[4][2] + abc_amp[4][2] * cost;
            }
            double sine = Math.Sin(elev);
            double cose = Math.Cos(elev);
            double beta = b / (sine + c);
            double gamma = a / (sine + beta);
            double topcon = 1 + (a / (1 + b / (1 + c)));
            hmf[0] = (topcon) / ((sine + gamma));//////与出处不同
            hmf[1] = -topcon * cose / ((sine + gamma) * (sine + gamma) * (1 - a / ((sine + beta) * (sine + beta) * (1 - b / (sine * c) * (sine * c)))));

            beta = b_ht / (sine + c_ht);
            gamma = a_ht / (sine + beta);
            topcon = 1 + a_ht / (1 + b_ht / (1 + c_ht));
            double ht_corr_coef = 1 / sine - (topcon) / ((sine + gamma));//////
            double ht_corr = ht_corr_coef * hs_km;
            hmf[0] = hmf[0] + ht_corr;
            double dhcc_del = -cose / (sine * sine) + topcon * cose / ((sine + gamma) * (sine + gamma) * (1 - a_ht / ((sine + beta) * (sine + beta) * (1 - b_ht / (sine * c_ht) * (sine * c_ht)))));//////出处最后两项正确！？是否是 “+”
            double dht_corr_del = dhcc_del * hs_km;
            hmf[1] = hmf[1] + dht_corr_del;
            //dht_corr_del = dhcc_del * hs_km;
            //hmf[1] = hmf[1] + dht_corr_del;


            double alat = 0, blat = 0, clat = 0;
            if (lat <= lat_wmf[0])
            {
                alat = abc_w2po[0][0];//与出处不同
                blat = abc_w2po[0][1];
                clat = abc_w2po[0][2];
            }
            for (int i = 0; i < 4; i++)
            {
                if (lat >= lat_wmf[i] && lat <= lat_wmf[i + 1])
                {
                    double dll = (lat - lat_wmf[i]) / (lat_wmf[i + 1] - lat_wmf[i]);
                    double da = abc_w2po[i + 1][0] - abc_w2po[i][0];
                    alat = abc_w2po[i][0] + dll * da;// + or - time don't sure

                    double db = abc_w2po[i + 1][1] - abc_w2po[i][1];
                    blat = abc_w2po[i][1] + dll * db;

                    double dc = abc_w2po[i + 1][2] - abc_w2po[i][2];
                    clat = abc_w2po[i][2] + dll * dc;
                }
            }
            if (lat >= lat_wmf[4])
            {
                alat = abc_w2po[4][0];
                blat = abc_w2po[4][1];
                clat = abc_w2po[4][2];
            }
            double sinelat = Math.Sin(elev);
            double coselat = Math.Cos(elev);
            double betalat = blat / (sinelat + clat);
            double gammalat = alat / (sinelat + betalat);
            double topconlat = 1 + alat / (1 + blat / (1 + clat));
            hmf[2] = (topconlat) / ((sinelat + gammalat));///////
            hmf[3] = -topconlat / ((sinelat + gammalat) * (sinelat + gammalat) * (coselat - alat / ((sinelat + betalat) * (sinelat + betalat) * coselat * (1 - blat / (sinelat * clat) * (sinelat * clat)))));/////出处最后两项正确？

            return hmf;
        }


        /// <summary> 对流层改正：SAASTAMOINEN MODEL
        /// Saastamoinen模型
        /// TROPO: Calculation of tropospheric correction.
        /// The range correction ddr in m is to be subtracted from
        /// pseudo-ranges and carrier phases
        /// lat：测站纬度（弧度），hsta：测站高程(km),el: elevation angel of satellite(卫星高度角，单位：弧度)
        /// 
        /// <returns></returns>
        public static double tropol(double el, double hsta)
        {
            double correction = 0.0;
            double standardTemperature = 18.0; // degrees Celcius 摄氏度
            double standardPressure = 1013.0; //millibars，标准大气压强
            double standarRelativeHumidity = 50.0;//percentage,标准相对湿度
            double height = hsta * 1000;// meter

            double pressure = standardPressure * Math.Pow((1 - 2.26 / 100000 * height), 5.225);
            double temperature = standardTemperature - height * 0.0065;
            double hum = standarRelativeHumidity * Math.Exp(-6.396 / 10000 * height);
            double zenithAngle = PI / 2 - el;//弧度

            double absoluteTemperature = temperature + 273.15;// in Kevin，单位K式温度
            double waterVapourPressure = hum / 100 * Math.Exp(-37.2465 + 0.213166 * absoluteTemperature - 0.000256908 * absoluteTemperature * absoluteTemperature);

            double[] table = new double[6] { 1.156, 1.006, 0.874, 0.757, 0.654, 0.563 };

            height = hsta;// Height in Km
            if (height < 0)
            {
                height = 0;
            }
            else if (height >= 4.999)
            {
                height = 4.999;     //0<=H<5
            }
            int index = Convert.ToInt32(Math.Floor(height));
            height = height - index;
            double value = table[index + 1 - 1] + (table[index + 2 - 1] - table[index + 1 - 1]) * height;

            correction = 0.002277 * (pressure + (1225 / absoluteTemperature + 0.05) * waterVapourPressure - value * (Math.Tan(zenithAngle) * Math.Tan(zenithAngle)) / Math.Cos(zenithAngle));
            return correction;
        }

        /// <summary> 对流层改正：SAASTAMOINEN MODEL
        /// Saastamoinen模型
        /// TROPO: Calculation of tropospheric correction.
        /// The range correction ddr in m is to be subtracted from
        /// pseudo-ranges and carrier phases
        /// lat：测站纬度（弧度），hsta：测站高程(km),el: elevation angel of satellite(卫星高度角，单位：弧度)
        /// 
        /// <returns></returns>
        public static double tropol1(double lat, double hsta)
        {
            double correction = 0.0;
            double standardPressure = 1013.25; //millibars，标准大气压强

            double height = hsta * 1000;// meter

            double pressure = standardPressure * Math.Pow((1 - 2.26 / 100000 * height), 5.225);

            double g = 1 - 0.00266 * Math.Cos(2 * lat) - 0.00028 * hsta;

            correction = 0.002277 * pressure / g;
            return correction;
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
        public static double MetSeason(double height, double lat_rad, Time gpsTime)
        {
            double hgt, epoch, dt1, tempk;

            hgt = height / 1000.0;                       //*** funcKeyToDouble input height to km

            //mytime.jtsciv(tsec);
            //mytime.civmjd();
            //epoch = (mytime.GetModifiedJulianDay() - MDJ2000) + mytime.GetDigitaOfDay(); 
            double MDJ2000 = 51544.0;
            epoch = (double)(gpsTime.MJulianDays - (Decimal)MDJ2000);
            //*** estimate temperature -- get seasonal argument

            dt1 = (epoch / 365.25) * 2.0 * CoordConsts.PI;
            double Temperature_Celsius = (-20.5 + 48.4 * Math.Cos(lat_rad) - 3.1 * hgt) +
                   (-14.3 + 3.3 * hgt) * Math.Sin(lat_rad) * Math.Cos(dt1) +
                   (-4.7 + 1.1 * hgt) * Math.Sin(lat_rad) * Math.Sin(dt1);

            //*** compute the pressure based on standard lapse rate 

            tempk = Temperature_Celsius + 273.15;                           //*** funcKeyToDouble to Kelvin
            double Pressure_Mbar = 1013.25 * Math.Pow((tempk / (tempk + 6.5 * hgt)), 5.26);

            //*** relative humidity (plain default value) 
            double RelativeHumidity = 0.5;


            double fgrav, tk, pp;
            //*** saastamoinen's function for gravity
            fgrav = 1.0 - 0.00266 * Math.Cos(2.0 * lat_rad) - 0.00028e-3 * height;

            //*** water vapor partial pressure in mbars   (proportional to rh)
            tk = Temperature_Celsius + 273.15;
            pp = RelativeHumidity * 6.11 * Math.Pow((tk / 273.15), -5.3) * Math.Exp(25.2 * Temperature_Celsius / tk);

            //*** zenith delays
            double WetZenithDelay = 0.0022768 * (1255.0 / tk + 0.05) * pp / fgrav;
            double DryZenithDelay = 0.0022768 * Pressure_Mbar / fgrav;

            return DryZenithDelay;
        }


        /// <summary> 获取湿分量的系数
        ///对流层延迟，第一个是干分量的延迟量，第二个值是湿分量的映射函数系数
        /// 高精度的对流层改正
        /// 对流层延迟由干、湿分量组成，常用天顶方向的干、湿分量和相应的映射函数表示：detDtrop=detDdry*Mdry(E)+detDwet*Mwet(E)
        /// 采用Saastamoinen模型改正对流层延迟干分量，将湿延迟分量作为未知参数进行估计,计算湿分量的系数
        /// 对流层映射函数采用NMF映射函数
        /// lat：测站纬度（弧度），h：测站高程(km),P:测站大气压强(mbar)，TProduct:测站温度(k),e：大气中的水汽压(mbar)
        /// </summary>
        /// <returns></returns>
        public static double[] GetDryTropCorrectValue(Time gpsTime, XYZ satPos, XYZ receiverXyz)
        {
            Polar p = CoordTransformer.XyzToGeoPolar(satPos, receiverXyz);
            Geo.Coordinates.GeoCoord geoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(receiverXyz, Geo.Coordinates.AngleUnit.Radian);
            //double troCorect = MeteorologyInfluence.TroposphereDelay(geoCoord.Lat, geoCoord.Height, info.ElevatAngle, gpsTime)

            //double SS = Math.Asin(1);
            //double SSS = Math.Asin(0);

            double[] NMF = NeillMF(gpsTime.DayOfYear, geoCoord.Lat, geoCoord.Height, p.Elevation);

            double zpd = tropol(90, geoCoord.Height / 1000d);
            double zpd1 = tropol(geoCoord.Lat, geoCoord.Height / 1000d);
            double zpd11 = tropol1(geoCoord.Lat, geoCoord.Height / 1000.0);

            double dryZpd = MetSeason(geoCoord.Height, geoCoord.Lat, gpsTime);

            double TropE = dryZpd * NMF[0];//干分量改正

            double[] DryWet = new double[2];
            DryWet[0] = TropE;

            double wetCo = NMF[2];//湿分量系数
            DryWet[1] = wetCo;

            //return wetCo;
            return DryWet;
        }
    }
}
