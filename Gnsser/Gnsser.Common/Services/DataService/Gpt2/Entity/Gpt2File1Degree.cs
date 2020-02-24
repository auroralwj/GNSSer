﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Algorithm;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Geo;
using Geo.Common;
using Gnsser.Times;
using Geo.Times;

namespace Gnsser.Data
{
    public class Gpt2File1Degree : IEnumerable<Gpt2Value1Degree>
    {
                /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="VMF1Infos"></param>
        public Gpt2File1Degree(List<Gpt2Value1Degree> Gpt2Infos)
        {
            this.Gpt2Infos = Gpt2Infos;
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get { return Gpt2Infos.Count; } }

        /// <summary>
        /// 数据列表
        /// </summary>   

        public List<Gpt2Value1Degree> Gpt2Infos { get; set; }

        /// <summary>
        /// 清除所有
        /// </summary>
        public void Clear()
        {
            Gpt2Infos.Clear();
        }

        public Gpt2Res1Degree GetGridInfo(Time time, GeoCoord geoCoord)
        {
            return GetGridInfo_1Degree(time, geoCoord.Lat, geoCoord.Lon, geoCoord.Height);
        }       

        /// <summary>
        /// 获取四个格网点,并进行内插
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public Gpt2Res1Degree GetGridInfo_1Degree(Time time, double lat, double lon, double height)
        {
            lat = lat * SunMoonPosition.DegToRad;
            lon = lon * SunMoonPosition.DegToRad;

            List<Gpt2Value1Degree> StaInfos = new List<Gpt2Value1Degree>();
            Gpt2Res1Degree gpt2Res = new Gpt2Res1Degree(time, lat, lon, height, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            //VMF1Value satInfo = VMF1Infos.Find(m => m.stanam.Equals(staname));
            double mjd = (double)time.MJulianDays;

            // change the reference epoch to January 1 2000
            double dmjd1 = mjd - 51544.5;

            // mean gravity in m/s**2
            double gm = 9.80665;
            // molar mass of dry air in kg/mol 28.965*10^-3
            double dMtr = 0.028965;
            // universal gas constant in J/K/mol
            double Rg = 8.3143;
            double cosfy = Math.Cos(dmjd1 / 365.25 * 2 * Math.PI);
            double coshy = Math.Cos(dmjd1 / 365.25 * 4 * Math.PI);
            double sinfy = Math.Sin(dmjd1 / 365.25 * 2 * Math.PI);
            double sinhy = Math.Sin(dmjd1 / 365.25 * 4 * Math.PI);

            //only positive longitude in degrees
            double plon = 0;
            if (lon < 0)
                plon = (lon + 2 * Math.PI) * 180 / Math.PI;
            else
                plon = lon * 180 / Math.PI;
            // transform to polar distance in degrees
            double ppod = (-lat + Math.PI / 2) * 180 / Math.PI;

            //find the index (line in the grid file) of the nearest point
            double ipod = Math.Floor((ppod + 1));
            double ilon = Math.Floor((plon + 1));

            // normalized (to one) differences, can be positive or negative
            double diffpod = (ppod - (ipod - 0.5));
            double difflon = (plon - (ilon - 0.5));

            if (ipod == 181) ipod = 180;
            if (ilon == 361) ipod = 1;
            if (ilon == 0)   ipod = 360;

            // get the number of the corresponding line
            int[] indx = new int[4]; indx[0] = 0; indx[1] = 0; indx[2] = 0; indx[3] = 0;
            indx[0] = (int)((ipod - 1) * 360 + ilon) - 1; //注意一定要减1，c#从0开始

            // near the poles: nearest neighbour interpolation, otherwise: bilinear
            double bilinear = 0;
            if (ppod > 0.5 && ppod < 179.5) bilinear = 1;

            //case of nearest neighbourhood

            if (bilinear == 0)
            {
                int ix = indx[0];
                //transforming ellipsoidial height to orthometric height
                double undu = Gpt2Infos[ix].undu;
                double hgt = height - undu;

                //pressure, temperature at the heigtht of the grid
                double T0 = Gpt2Infos[ix].T.a0 + Gpt2Infos[ix].T.A1 * cosfy + Gpt2Infos[ix].T.B1 * sinfy + Gpt2Infos[ix].T.A2 * coshy + Gpt2Infos[ix].T.B2 * sinhy;
                double p0 = Gpt2Infos[ix].pre.a0 + Gpt2Infos[ix].pre.A1 * cosfy + Gpt2Infos[ix].pre.B1 * sinfy + Gpt2Infos[ix].pre.A2 * coshy + Gpt2Infos[ix].pre.B2 * sinhy;
                //specific humidity
                double Q = Gpt2Infos[ix].Q.a0 + Gpt2Infos[ix].Q.A1 * cosfy + Gpt2Infos[ix].Q.B1 * sinfy + Gpt2Infos[ix].Q.A2 * coshy + Gpt2Infos[ix].Q.B2 * sinhy;
                //lapse rate of the temperature
                double dT = Gpt2Infos[ix].dT.a0 + Gpt2Infos[ix].dT.A1 * cosfy + Gpt2Infos[ix].dT.B1 * sinfy + Gpt2Infos[ix].dT.A2 * coshy + Gpt2Infos[ix].dT.B2 * sinhy;

                //station height - grid height
                double redh = hgt - Gpt2Infos[ix].Hs;

                //temperature at station height in Celsius
                double T = T0 + dT * redh - 273.15;

                //temperature lapse rate in degrees / km
                dT = dT * 1000;

                //virtual temperature in Kelvin
                double Tv = T0 * (1 + 0.6077 * Q);
                double c = gm * dMtr / (Rg * Tv);

                //pressure in hPa
                double p = (p0 * Math.Exp(-c * redh)) / 100;

                //hydrostatic coefficient ah 
                double ah = Gpt2Infos[ix].ah.a0 + Gpt2Infos[ix].ah.A1 * cosfy + Gpt2Infos[ix].ah.B1 * sinfy + Gpt2Infos[ix].ah.A2 * coshy + Gpt2Infos[ix].ah.B2 * sinhy;
                //wet coefficient aw
                double aw = Gpt2Infos[ix].aw.a0 + Gpt2Infos[ix].aw.A1 * cosfy + Gpt2Infos[ix].aw.B1 * sinfy + Gpt2Infos[ix].aw.A2 * coshy + Gpt2Infos[ix].aw.B2 * sinhy;

                //water vapour decrease factor la - added by GP
                double la = Gpt2Infos[ix].la.a0 + Gpt2Infos[ix].la.A1 * cosfy + Gpt2Infos[ix].la.B1 * sinfy + Gpt2Infos[ix].la.A2 * coshy + Gpt2Infos[ix].la.B2 * sinhy;

                //mean temperature of the water vapor Tm - added by GP
                double Tm = Gpt2Infos[ix].Tm.a0 + Gpt2Infos[ix].Tm.A1 * cosfy + Gpt2Infos[ix].Tm.B1 * sinfy + Gpt2Infos[ix].Tm.A2 * coshy + Gpt2Infos[ix].Tm.B2 * sinhy;

                //water vapor pressure in hPa - changed by GP
                double e0 = (Q * p0 / (0.622 + 0.378 * Q)) / 100;//on the grid
                //double e = (Q * p) / (0.622 + 0.378 * Q);
                double e = e0 * Math.Pow(100 * p / p0, la + 1);// (100 * p / p0) ^ (la + 1);

                gpt2Res.p = p; gpt2Res.T = T; gpt2Res.dT = dT; gpt2Res.e = e; gpt2Res.ah = ah; gpt2Res.aw = aw; gpt2Res.undu = undu; gpt2Res.la = la; gpt2Res.Tm = Tm;

            }
            else //bilinear interpolation
            {
                double ipod1 = ipod + Math.Sign(diffpod);
                double ilon1 = ilon + Math.Sign(difflon);
                if (ilon1 == 361) ilon1 = 1;
                if (ilon1 == 0) ilon1 = 360;
                //get the number of the line changed for the 1 degree grid (GP)
                indx[1] = (int)((ipod1 - 1) * 360 + ilon) - 1;  // along same longitude //注意一定要减1，c#从0开始
                indx[2] = (int)((ipod - 1) * 360 + ilon1) - 1; // along same polar distance//注意一定要减1，c#从0开始
                indx[3] = (int)((ipod1 - 1) * 360 + ilon1) - 1; // diagonal//注意一定要减1，c#从0开始

                double[] undul = new double[4];
                double[] Ql = new double[4];
                double[] dTl = new double[4];
                double[] Tl = new double[4];
                double[] pl = new double[4];
                double[] ahl = new double[4];
                double[] awl = new double[4];
                double[] lal = new double[4];
                double[] Tml = new double[4];
                double[] el  = new double[4];

                for (int i = 0; i < 4; i++)
                {
                    //% transforming ellipsoidial height to orthometric height:
                    // Hortho = -N + Hell
                    undul[i] = Gpt2Infos[indx[i]].undu;
                    double hgt = height - undul[i];

                    //pressure, temperature at the heigtht of the grid
                    double T0 = Gpt2Infos[indx[i]].T.a0 + Gpt2Infos[indx[i]].T.A1 * cosfy + Gpt2Infos[indx[i]].T.B1 * sinfy + Gpt2Infos[indx[i]].T.A2 * coshy + Gpt2Infos[indx[i]].T.B2 * sinhy;
                    double p0 = Gpt2Infos[indx[i]].pre.a0 + Gpt2Infos[indx[i]].pre.A1 * cosfy + Gpt2Infos[indx[i]].pre.B1 * sinfy + Gpt2Infos[indx[i]].pre.A2 * coshy + Gpt2Infos[indx[i]].pre.B2 * sinhy;
                    //specific humidity
                    Ql[i] = Gpt2Infos[indx[i]].Q.a0 + Gpt2Infos[indx[i]].Q.A1 * cosfy + Gpt2Infos[indx[i]].Q.B1 * sinfy + Gpt2Infos[indx[i]].Q.A2 * coshy + Gpt2Infos[indx[i]].Q.B2 * sinhy;

                    //reduction = stationheight - gridheight
                    double Hs1 = Gpt2Infos[indx[i]].Hs;
                    double redh = hgt - Hs1;

                    //lapse rate of the temperature in degree / m
                    dTl[i] = Gpt2Infos[indx[i]].dT.a0 + Gpt2Infos[indx[i]].dT.A1 * cosfy + Gpt2Infos[indx[i]].dT.B1 * sinfy + Gpt2Infos[indx[i]].dT.A2 * coshy + Gpt2Infos[indx[i]].dT.B2 * sinhy;

                    //temperature reduction to station height
                    Tl[i] = T0 + dTl[i] * redh - 273.15;

                    //virtual temperature
                    double Tv = T0 * (1 + 0.6077 * Ql[i]);
                    double c = gm * dMtr / (Rg * Tv);

                    //pressure in hPa
                    pl[i] = (p0 * Math.Exp(-c * redh)) / 100;

                    // hydrostatic coefficient ah
                    ahl[i] = Gpt2Infos[indx[i]].ah.a0 + Gpt2Infos[indx[i]].ah.A1 * cosfy + Gpt2Infos[indx[i]].ah.B1 * sinfy + Gpt2Infos[indx[i]].ah.A2 * coshy + Gpt2Infos[indx[i]].ah.B2 * sinhy;

                    //wet coefficient aw
                    awl[i] = Gpt2Infos[indx[i]].aw.a0 + Gpt2Infos[indx[i]].aw.A1 * cosfy + Gpt2Infos[indx[i]].aw.B1 * sinfy + Gpt2Infos[indx[i]].aw.A2 * coshy + Gpt2Infos[indx[i]].aw.B2 * sinhy;

                    //water vapor decrease factor la - added by GP
                    lal[i] = Gpt2Infos[indx[i]].la.a0 +Gpt2Infos[indx[i]].la.A1 * cosfy + Gpt2Infos[indx[i]].la.B1 * sinfy + Gpt2Infos[indx[i]].la.A2 * coshy + Gpt2Infos[indx[i]].la.B2 * sinhy;

                    //mean temperature of the water vapor Tm - added by GP
                    Tml[i] = Gpt2Infos[indx[i]].Tm.a0 +Gpt2Infos[indx[i]].Tm.A1 * cosfy + Gpt2Infos[indx[i]].Tm.B1 * sinfy + Gpt2Infos[indx[i]].Tm.A2 * coshy + Gpt2Infos[indx[i]].Tm.B2 * sinhy;
                    // water vapor pressure in hPa - changed by GP
                    double e0 = Ql[i] * p0 / (0.622 + 0.378 * Ql[i]) / 100; // on the grid
                    el[i] = e0 * Math.Pow(100 * pl[i] / p0, lal[i] + 1);  //(100 * pl[i] / p0) ^ (lal[i] + 1);  // on the station height - (14) Askne and Nordius, 1987
                }

                double dnpod1 = Math.Abs(diffpod); // distance nearer point
                double dnpod2 = 1 - dnpod1;   // distance to distant point
                double dnlon1 = Math.Abs(difflon);
                double dnlon2 = 1 - dnlon1;

                //pressure
                double R1 = dnpod2 * pl[0] + dnpod1 * pl[1];
                double R2 = dnpod2 * pl[2] + dnpod1 * pl[3];
                gpt2Res.p = dnlon2 * R1 + dnlon1 * R2;

                //temperature
                R1 = dnpod2 * Tl[0] + dnpod1 * Tl[1];
                R2 = dnpod2 * Tl[2] + dnpod1 * Tl[3];
                gpt2Res.T = dnlon2 * R1 + dnlon1 * R2;

                //temperature in degree per km
                R1 = dnpod2 * dTl[0] + dnpod1 * dTl[1];
                R2 = dnpod2 * dTl[2] + dnpod1 * dTl[3];
                gpt2Res.dT = (dnlon2 * R1 + dnlon1 * R2) * 1000;

                //humidity
                R1 = dnpod2 * el[0] + dnpod1 * el[1];
                R2 = dnpod2 * el[2] + dnpod1 * el[3];
                gpt2Res.e = dnlon2 * R1 + dnlon1 * R2;

                //hydrostatic
                R1 = dnpod2 * ahl[0] + dnpod1 * ahl[1];
                R2 = dnpod2 * ahl[2] + dnpod1 * ahl[3];
                gpt2Res.ah = dnlon2 * R1 + dnlon1 * R2;

                //wet
                R1 = dnpod2 * awl[0] + dnpod1 * awl[1];
                R2 = dnpod2 * awl[2] + dnpod1 * awl[3];
                gpt2Res.aw = dnlon2 * R1 + dnlon1 * R2;

                //undulation
                R1 = dnpod2 * undul[0] + dnpod1 * undul[1];
                R2 = dnpod2 * undul[2] + dnpod1 * undul[3];
                gpt2Res.undu = dnlon2 * R1 + dnlon1 * R2;

                //water vapor decrease factor la - added by GP
                R1 = dnpod2 * lal[0] + dnpod1 * lal[1];
                R2 = dnpod2 * lal[2] + dnpod1 * lal[3];
                gpt2Res.la = dnlon2 * R1 + dnlon1 * R2;

                //mean temperature of the water vapor Tm - added by GP
                R1 = dnpod2 * Tml[0] + dnpod1 * Tml[1];
                R2 = dnpod2 * Tml[2] + dnpod1 * Tml[3];
                gpt2Res.Tm = dnlon2 * R1 + dnlon1 * R2;                

            }
            return gpt2Res;
        }
        #region override
        public IEnumerator<Gpt2Value1Degree> GetEnumerator()
        {
            return Gpt2Infos.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Gpt2Infos.GetEnumerator();
        }
        #endregion
    }

}