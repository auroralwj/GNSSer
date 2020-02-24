
//2017.05.10, lly, add in zz,GT2 模型

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Data;

namespace Gnsser
{
    public class Gpt2TropModel
    {
        DataSourceContext DataSourceProvider;
        public Gpt2TropModel()
        {

        }
        public Gpt2TropModel(DataSourceContext DataSourceProvider)
        {
            this.DataSourceProvider = DataSourceProvider;
        }
        public double Correction(Time time, GeoCoord pos, XYZ x, double[] azel, double wetCorValue, ref double wetMap)
        {   //对流层延迟+湿延迟投影函数
            double[] res = new double[2];

            //res = tropmodel(time, pos, azel, 0.0);

            //double[] res2 = new double[2];

            if (this.DataSourceProvider.gpt2DataService1Degree != null)
            {
                res = tropmodel1Degree(time, pos, azel, 0.0);

            }
            else if (this.DataSourceProvider.gpt2DataService != null)
            {
                res = tropmodel(time, pos, azel, 0.0);
            }


            wetMap = res[1];

            return res[0];
        }

        public double[] tropmodel(Time time, GeoCoord pos, double[] azel, double humi)
        {
            //Gpt2DataService gpt2DataService = new Data.Gpt2DataService();
            // Gpt2Res gpt2Res = this.DataSourceProvider.gpt2DataService.Acquire(time, pos.Lat, pos.Lon, pos.Height);
            Gpt2Res gpt2Res = this.DataSourceProvider.gpt2DataService.Acquire(time, pos);

            double dlat = pos.Lat * SunMoonPosition.DegToRad; ;
            double mjd = (double)time.MJulianDays;

            double doy = mjd - 44239 + 1 - 28;
            double bh = 0.0029;
            double c0h = 0.062;

            double phh = 0; double c11h = 0; double c10h = 0;
            if (dlat < 0)
            {
                phh = Math.PI;
                c11h = 0.007;
                c10h = 0.002;
            }
            else
            {
                phh = 0;
                c11h = 0.005;
                c10h = 0.001;
            }
            double ch = c0h + ((Math.Cos(doy / 365.25 * 2 * Math.PI + phh) + 1) * c11h / 2 + c10h) * (1 - Math.Cos(dlat));

            double sine = Math.Sin(azel[1]);
            double beta = bh / (sine + ch);
            double gamma = gpt2Res.ah / (sine + beta);
            double topcon = (1.0 + gpt2Res.ah / (1.0 + bh / (1.0 + ch)));
            double vmf1h = topcon / (sine + gamma);

            //2017.12.1 lly加入 参考vmf1_ht.m
            // C  height correction for hydrotatic part [Niell, 1996]     
            double a_ht = 2.53d - 5;
            double b_ht = 5.49d - 3;
            double c_ht = 1.14d - 3;
            double hs_km = pos.Height / 1000.0;
            double beta_ht = b_ht / (sine + c_ht);
            double gamma_ht = a_ht / (sine + beta_ht);
            double topcon_ht = (1.0 + a_ht / (1.0 + b_ht / (1.0 + c_ht)));
            double ht_corr_coef = 1.0 / sine - topcon_ht / (sine + gamma_ht);
            double ht_corr = ht_corr_coef * hs_km;
            //改正
            vmf1h = vmf1h + ht_corr;


            double bw = 0.00146;
            double cw = 0.04391;
            beta = bw / (sine + cw);
            gamma = gpt2Res.aw / (sine + beta);
            topcon = (1.0 + gpt2Res.aw / (1.0 + bw / (1.0 + cw)));
            double vmf1w = topcon / (sine + gamma);

            double hgt = pos.Height < 0.0 ? 0.0 : pos.Height;
            //Saas模型
            double trph = 0.0022768 * gpt2Res.p / (1.0 - 0.00266 * Math.Cos(2.0 * pos.Lat * SunMoonPosition.DegToRad) - 0.00028 * hgt / 1e3);

            double trpw = 0.002277 * (1255.0 / (gpt2Res.T + 273.15) + 0.05) * gpt2Res.e;

            double[] res = new double[2];
            res[0] = trph * vmf1h + trpw * vmf1w;
            res[1] = vmf1w;
            return res;

        }
        public double[] tropmodel1Degree(Time time, GeoCoord pos, double[] azel, double humi)
        {
            //Gpt2DataService gpt2DataService = new Data.Gpt2DataService();
            // Gpt2Res gpt2Res = this.DataSourceProvider.gpt2DataService.Acquire(time, pos.Lat, pos.Lon, pos.Height);
            Gpt2Res1Degree gpt2Res1Degree = this.DataSourceProvider.gpt2DataService1Degree.Acquire(time, pos);

            double dlat = pos.Lat * SunMoonPosition.DegToRad; ;
            double mjd = (double)time.MJulianDays;

            double doy = mjd - 44239 + 1 - 28;
            double bh = 0.0029;
            double c0h = 0.062;

            double phh = 0; double c11h = 0; double c10h = 0;
            if (dlat < 0)
            {
                phh = Math.PI;
                c11h = 0.007;
                c10h = 0.002;
            }
            else
            {
                phh = 0;
                c11h = 0.005;
                c10h = 0.001;
            }
            double ch = c0h + ((Math.Cos(doy / 365.25 * 2 * Math.PI + phh) + 1) * c11h / 2 + c10h) * (1 - Math.Cos(dlat));

            double sine = Math.Sin(azel[1]);
            double beta = bh / (sine + ch);
            double gamma = gpt2Res1Degree.ah / (sine + beta);
            double topcon = (1.0 + gpt2Res1Degree.ah / (1.0 + bh / (1.0 + ch)));
            double vmf1h = topcon / (sine + gamma);

            //2017.12.1 lly加入 参考vmf1_ht.m
            // C  height correction for hydrotatic part [Niell, 1996]     
            double a_ht = 2.53e-5;
            double b_ht = 5.49e-3;
            double c_ht = 1.14e-3;
            double hs_km = pos.Height / 1000.0;
            double beta_ht = b_ht / (sine + c_ht);
            double gamma_ht = a_ht / (sine + beta_ht);
            double topcon_ht = (1.0 + a_ht / (1.0 + b_ht / (1.0 + c_ht)));
            double ht_corr_coef = 1.0 / sine - topcon_ht / (sine + gamma_ht);
            double ht_corr = ht_corr_coef * hs_km;
            //改正
            vmf1h = vmf1h + ht_corr;

            double bw = 0.00146;
            double cw = 0.04391;
            beta = bw / (sine + cw);
            gamma = gpt2Res1Degree.aw / (sine + beta);
            topcon = (1.0 + gpt2Res1Degree.aw / (1.0 + bw / (1.0 + cw)));
            double vmf1w = topcon / (sine + gamma);

            double hgt = pos.Height < 0.0 ? 0.0 : pos.Height;
            //Saas模型
            double trph = 0.0022768 * gpt2Res1Degree.p / (1.0 - 0.00266 * Math.Cos(2.0 * pos.Lat * SunMoonPosition.DegToRad) - 0.00028 * hgt / 1e3);

            double trpw = 0.002277 * (1255.0 / (gpt2Res1Degree.T + 273.15) + 0.05) * gpt2Res1Degree.e;

            //Askne & Nordius

            double k1 = 77.604; double k2 = 64.79; double k3 = 3776 * Math.Pow(10, 5);
            double k2_ = 16.52;
            double Rd = 8.314 / 28.9644;
            // mean gravity in m/s**2
            double gm = 9.80665;
            double trpw2 = (k2_ + k3 / gpt2Res1Degree.Tm) * Rd * gpt2Res1Degree.e * Math.Pow(10, -6) / ((gpt2Res1Degree.la + 1) * gm);

            double[] res = new double[2];
            res[0] = trph * vmf1h + trpw2 * vmf1w;
            res[1] = vmf1w;
            //res[2] = trpw;
            return res;

        }
    }
}
