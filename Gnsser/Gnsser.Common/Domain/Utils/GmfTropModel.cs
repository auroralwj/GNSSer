//2015.01.27, Cui Yang, created，参照GMF模型

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser
{
    /// <summary>
    /// Tropospheric model based in the GMF(Global Mapping Function) mapping functions.
    /// </summary>
    public class GmfTropModel 
    {
        

        /// <summary>
        /// Constructor to create a Neill trop model providing the position of the receiver and current time.
        /// </summary>
        /// <param name="Rp"> Receiver position. 经纬度高程格式</param>
        /// <param name="time"></param>
        public GmfTropModel()
        {
          
        }

       /// <summary>
       /// precise tropospheric model
       /// </summary>
       /// <param name="time">历元时刻</param>
        /// <param name="pos">receiver position {lat, lon, h} (rad, m)</param>
        /// <param name="azel">azimuth/elevation angle {az, el} (rad)</param>
        /// <param name="x">receiver position  XYZ (m)，这里应该是梯度向量</param>
       /// <returns></returns>
        public double Correction(Time time, GeoCoord pos, XYZ x, double[] azel, double wetCorValue, ref double wetMap)
        {
            double[] zazel = new double[] { 0.0, SunMoonPosition.PI / 2.0 };

            double zhd, m_h, m_w = 0.0, cotz, grad_n, grad_e;

            double[] res = new double[2];
            //zenith hydrostatic delay
            res = tropmodel(time, pos, zazel, 0.0);

            //mapping function
            m_h = tropmapf(time, pos, azel, ref m_w);

            ////参数中第一个数表示湿分量系数（一阶？），然后分别是二阶的，三阶的？
            //double[] dtdx = new double[] { 0, 0, 0 };

            //if (azel[1] > 0.0)
            //{
            //    /* m_w=m_0+m_0*cot(el)*(Gn*cos(az)+Ge*sin(az)): ref [6] */
            //    cotz = 1.0 / Math.Tan(azel[1]);
            //    grad_n = m_w * cotz * Math.Cos(azel[0]);
            //    grad_e = m_w * cotz * Math.Sin(azel[0]);
            //    m_w += grad_n * x[1] + grad_e * x[2];
            //    dtdx[1] = grad_n * (x[0] - zhd);
            //    dtdx[2] = grad_e * (x[0] - zhd);
            //}
            //dtdx[0] = m_w;


            //   double tropDelay = m_h * zhd + m_w * (0.1);
            // double tropDelay = m_h * zhd + m_w * (x[0] - zhd);
            //double tropDelay = m_h * zhd;// +m_w * (-zhd);//;//wetCorValue

            double tropDelay = m_h * res[0] + m_w * res[1];// +m_w * (-zhd);//;//wetCorValue
            wetMap = m_w;

            return tropDelay;
        }



        /// <summary>
        /// troposphere model
        /// compute tropospheric delay by standard atmosphere and saastamionen model
        /// </summary>
        /// <param name="time">time</param>
        /// <param name="pos">receiver position {lat, lon, h} (rad, m)</param>
        /// <param name="azel">azimuth/elevation angle {az, el} (rad)</param>
        /// <param name="humi">relative humidity</param>
        /// <returns>tropospheric delay (m)</returns>
        public double[] tropmodel(Time time, GeoCoord pos, double[] azel, double humi)
        {

            #region 旧代码,压力和温度均是定值
            //double temp0 = 15.0; //temparature at sea level
            //double hgt, pres, temp, e, z, trph, trpw;
            //if (pos.Height < -100.0 || pos.Height > 1e4) // || pos.Lon * AstronomicalFunctions.D2R <= 0)
            //{
            //    return 0.0;
            //}

            ////standard atmosphere
            //hgt = pos.Height < 0.0 ? 0.0 : pos.Height;

            //pres = 1013.25 * Math.Pow(1.0 - 2.2557e-5 * hgt, 5.2568);

            //temp = temp0 - 6.5e-3 * hgt + 273.16;

            //e = 6.108 * humi * Math.Exp((17.15 * temp - 4684.0) / (temp - 38.45));

            ////saastamoninen model
            //z = SunMoonPosition.PI / 2 - azel[1];

            //trph = 0.0022768 * pres / (1.0 - 0.00266 * Math.Cos(2.0 * pos.Lat * SunMoonPosition.DegToRad) - 0.00028 * hgt / 1e3) / Math.Cos(z);

            //trpw = 0.002277 * (1255.0 / temp + 0.05) * e / Math.Cos(z);
            #endregion

            #region 参考matlab代码
            double doy = (double)time.MJulianDays  - 44239.0 + 1 - 28;

            double[] a_geoid = new double[ ]{-5.6195e-001,-6.0794e-002,-2.0125e-001,-6.4180e-002,-3.6997e-002, 
            +1.0098e+001,+1.6436e+001,+1.4065e+001,+1.9881e+000,+6.4414e-001,
            -4.7482e+000,-3.2290e+000,+5.0652e-001,+3.8279e-001,-2.6646e-002,
            +1.7224e+000,-2.7970e-001,+6.8177e-001,-9.6658e-002,-1.5113e-002,
            +2.9206e-003,-3.4621e+000,-3.8198e-001,+3.2306e-002,+6.9915e-003,
            -2.3068e-003,-1.3548e-003,+4.7324e-006,+2.3527e+000,+1.2985e+000,
            +2.1232e-001,+2.2571e-002,-3.7855e-003,+2.9449e-005,-1.6265e-004,
            +1.1711e-007,+1.6732e+000,+1.9858e-001,+2.3975e-002,-9.0013e-004,
            -2.2475e-003,-3.3095e-005,-1.2040e-005,+2.2010e-006,-1.0083e-006,
            +8.6297e-001,+5.8231e-001,+2.0545e-002,-7.8110e-003,-1.4085e-004, 
            -8.8459e-006,+5.7256e-006,-1.5068e-006,+4.0095e-007,-2.4185e-008}; 

            double[] b_geoid = new double[]{+0.0000e+000,+0.0000e+000,-6.5993e-002,+0.0000e+000,+6.5364e-002, 
            -5.8320e+000,+0.0000e+000,+1.6961e+000,-1.3557e+000,+1.2694e+000, 
            +0.0000e+000,-2.9310e+000,+9.4805e-001,-7.6243e-002,+4.1076e-002, 
            +0.0000e+000,-5.1808e-001,-3.4583e-001,-4.3632e-002,+2.2101e-003, 
            -1.0663e-002,+0.0000e+000,+1.0927e-001,-2.9463e-001,+1.4371e-003, 
            -1.1452e-002,-2.8156e-003,-3.5330e-004,+0.0000e+000,+4.4049e-001, 
            +5.5653e-002,-2.0396e-002,-1.7312e-003,+3.5805e-005,+7.2682e-005,
            +2.2535e-006,+0.0000e+000,+1.9502e-002,+2.7919e-002,-8.1812e-003,
            +4.4540e-004,+8.8663e-005,+5.5596e-005,+2.4826e-006,+1.0279e-006,
            +0.0000e+000,+6.0529e-002,-3.5824e-002,-5.1367e-003,+3.0119e-005,
            -2.9911e-005,+1.9844e-005,-1.2349e-006,-7.6756e-009,+5.0100e-008};

            double[] ap_mean = new double[]{+1.0108e+003,+8.4886e+000,+1.4799e+000,-1.3897e+001,+3.7516e-003,
            -1.4936e-001,+1.2232e+001,-7.6615e-001,-6.7699e-002,+8.1002e-003, 
            -1.5874e+001,+3.6614e-001,-6.7807e-002,-3.6309e-003,+5.9966e-004, 
            +4.8163e+000,-3.7363e-001,-7.2071e-002,+1.9998e-003,-6.2385e-004,
            -3.7916e-004,+4.7609e+000,-3.9534e-001,+8.6667e-003,+1.1569e-002,
            +1.1441e-003,-1.4193e-004,-8.5723e-005,+6.5008e-001,-5.0889e-001,
            -1.5754e-002,-2.8305e-003,+5.7458e-004,+3.2577e-005,-9.6052e-006, 
            -2.7974e-006,+1.3530e+000,-2.7271e-001,-3.0276e-004,+3.6286e-003, 
            -2.0398e-004,+1.5846e-005,-7.7787e-006,+1.1210e-006,+9.9020e-008, 
            +5.5046e-001,-2.7312e-001,+3.2532e-003,-2.4277e-003,+1.1596e-004, 
            +2.6421e-007,-1.3263e-006,+2.7322e-007,+1.4058e-007,+4.9414e-009};

            double[] bp_mean = new double[]{+0.0000e+000,+0.0000e+000,-1.2878e+000,+0.0000e+000,+7.0444e-001, 
            +3.3222e-001,+0.0000e+000,-2.9636e-001,+7.2248e-003,+7.9655e-003, 
            +0.0000e+000,+1.0854e+000,+1.1145e-002,-3.6513e-002,+3.1527e-003, 
            +0.0000e+000,-4.8434e-001,+5.2023e-002,-1.3091e-002,+1.8515e-003, 
            +1.5422e-004,+0.0000e+000,+6.8298e-001,+2.5261e-003,-9.9703e-004, 
            -1.0829e-003,+1.7688e-004,-3.1418e-005,+0.0000e+000,-3.7018e-001, 
            +4.3234e-002,+7.2559e-003,+3.1516e-004,+2.0024e-005,-8.0581e-006, 
            -2.3653e-006,+0.0000e+000,+1.0298e-001,-1.5086e-002,+5.6186e-003, 
            +3.2613e-005,+4.0567e-005,-1.3925e-006,-3.6219e-007,-2.0176e-008, 
            +0.0000e+000,-1.8364e-001,+1.8508e-002,+7.5016e-004,-9.6139e-005, 
            -3.1995e-006,+1.3868e-007,-1.9486e-007,+3.0165e-010,-6.4376e-010}; 

            double[] ap_amp = new double[]{-1.0444e-001,+1.6618e-001,-6.3974e-002,+1.0922e+000,+5.7472e-001, 
            -3.0277e-001,-3.5087e+000,+7.1264e-003,-1.4030e-001,+3.7050e-002, 
            +4.0208e-001,-3.0431e-001,-1.3292e-001,+4.6746e-003,-1.5902e-004, 
            +2.8624e+000,-3.9315e-001,-6.4371e-002,+1.6444e-002,-2.3403e-003, 
            +4.2127e-005,+1.9945e+000,-6.0907e-001,-3.5386e-002,-1.0910e-003, 
            -1.2799e-004,+4.0970e-005,+2.2131e-005,-5.3292e-001,-2.9765e-001, 
            -3.2877e-002,+1.7691e-003,+5.9692e-005,+3.1725e-005,+2.0741e-005, 
            -3.7622e-007,+2.6372e+000,-3.1165e-001,+1.6439e-002,+2.1633e-004, 
            +1.7485e-004,+2.1587e-005,+6.1064e-006,-1.3755e-008,-7.8748e-008, 
            -5.9152e-001,-1.7676e-001,+8.1807e-003,+1.0445e-003,+2.3432e-004, 
            +9.3421e-006,+2.8104e-006,-1.5788e-007,-3.0648e-008,+2.6421e-010}; 

            double[] bp_amp = new double[]{+0.0000e+000,+0.0000e+000,+9.3340e-001,+0.0000e+000,+8.2346e-001, 
            +2.2082e-001,+0.0000e+000,+9.6177e-001,-1.5650e-002,+1.2708e-003, 
            +0.0000e+000,-3.9913e-001,+2.8020e-002,+2.8334e-002,+8.5980e-004, 
            +0.0000e+000,+3.0545e-001,-2.1691e-002,+6.4067e-004,-3.6528e-005, 
            -1.1166e-004,+0.0000e+000,-7.6974e-002,-1.8986e-002,+5.6896e-003, 
            -2.4159e-004,-2.3033e-004,-9.6783e-006,+0.0000e+000,-1.0218e-001, 
            -1.3916e-002,-4.1025e-003,-5.1340e-005,-7.0114e-005,-3.3152e-007, 
            +1.6901e-006,+0.0000e+000,-1.2422e-002,+2.5072e-003,+1.1205e-003, 
            -1.3034e-004,-2.3971e-005,-2.6622e-006,+5.7852e-007,+4.5847e-008, 
            +0.0000e+000,+4.4777e-002,-3.0421e-003,+2.6062e-005,-7.2421e-005, 
            +1.9119e-006,+3.9236e-007,+2.2390e-007,+2.9765e-009,-4.6452e-009}; 

            double[] at_mean = new double[]{+1.6257e+001,+2.1224e+000,+9.2569e-001,-2.5974e+001,+1.4510e+000,
            +9.2468e-002,-5.3192e-001,+2.1094e-001,-6.9210e-002,-3.4060e-002, 
            -4.6569e+000,+2.6385e-001,-3.6093e-002,+1.0198e-002,-1.8783e-003, 
            +7.4983e-001,+1.1741e-001,+3.9940e-002,+5.1348e-003,+5.9111e-003, 
            +8.6133e-006,+6.3057e-001,+1.5203e-001,+3.9702e-002,+4.6334e-003, 
            +2.4406e-004,+1.5189e-004,+1.9581e-007,+5.4414e-001,+3.5722e-001, 
            +5.2763e-002,+4.1147e-003,-2.7239e-004,-5.9957e-005,+1.6394e-006, 
            -7.3045e-007,-2.9394e+000,+5.5579e-002,+1.8852e-002,+3.4272e-003, 
            -2.3193e-005,-2.9349e-005,+3.6397e-007,+2.0490e-006,-6.4719e-008, 
            -5.2225e-001,+2.0799e-001,+1.3477e-003,+3.1613e-004,-2.2285e-004, 
            -1.8137e-005,-1.5177e-007,+6.1343e-007,+7.8566e-008,+1.0749e-009}; 

            double[] bt_mean = new double[]{+0.0000e+000,+0.0000e+000,+1.0210e+000,+0.0000e+000,+6.0194e-001, 
            +1.2292e-001,+0.0000e+000,-4.2184e-001,+1.8230e-001,+4.2329e-002, 
            +0.0000e+000,+9.3312e-002,+9.5346e-002,-1.9724e-003,+5.8776e-003, 
            +0.0000e+000,-2.0940e-001,+3.4199e-002,-5.7672e-003,-2.1590e-003, 
            +5.6815e-004,+0.0000e+000,+2.2858e-001,+1.2283e-002,-9.3679e-003, 
            -1.4233e-003,-1.5962e-004,+4.0160e-005,+0.0000e+000,+3.6353e-002, 
            -9.4263e-004,-3.6762e-003,+5.8608e-005,-2.6391e-005,+3.2095e-006, 
            -1.1605e-006,+0.0000e+000,+1.6306e-001,+1.3293e-002,-1.1395e-003, 
            +5.1097e-005,+3.3977e-005,+7.6449e-006,-1.7602e-007,-7.6558e-008, 
            +0.0000e+000,-4.5415e-002,-1.8027e-002,+3.6561e-004,-1.1274e-004, 
            +1.3047e-005,+2.0001e-006,-1.5152e-007,-2.7807e-008,+7.7491e-009}; 

            double[] at_amp = new double[]{-1.8654e+000,-9.0041e+000,-1.2974e-001,-3.6053e+000,+2.0284e-002, 
            +2.1872e-001,-1.3015e+000,+4.0355e-001,+2.2216e-001,-4.0605e-003, 
            +1.9623e+000,+4.2887e-001,+2.1437e-001,-1.0061e-002,-1.1368e-003, 
            -6.9235e-002,+5.6758e-001,+1.1917e-001,-7.0765e-003,+3.0017e-004, 
            +3.0601e-004,+1.6559e+000,+2.0722e-001,+6.0013e-002,+1.7023e-004, 
            -9.2424e-004,+1.1269e-005,-6.9911e-006,-2.0886e+000,-6.7879e-002, 
            -8.5922e-004,-1.6087e-003,-4.5549e-005,+3.3178e-005,-6.1715e-006, 
            -1.4446e-006,-3.7210e-001,+1.5775e-001,-1.7827e-003,-4.4396e-004, 
            +2.2844e-004,-1.1215e-005,-2.1120e-006,-9.6421e-007,-1.4170e-008, 
            +7.8720e-001,-4.4238e-002,-1.5120e-003,-9.4119e-004,+4.0645e-006, 
            -4.9253e-006,-1.8656e-006,-4.0736e-007,-4.9594e-008,+1.6134e-009}; 

            double[] bt_amp = new double[]{+0.0000e+000,+0.0000e+000,-8.9895e-001,+0.0000e+000,-1.0790e+000, 
            -1.2699e-001,+0.0000e+000,-5.9033e-001,+3.4865e-002,-3.2614e-002, 
            +0.0000e+000,-2.4310e-002,+1.5607e-002,-2.9833e-002,-5.9048e-003, 
            +0.0000e+000,+2.8383e-001,+4.0509e-002,-1.8834e-002,-1.2654e-003, 
            -1.3794e-004,+0.0000e+000,+1.3306e-001,+3.4960e-002,-3.6799e-003, 
            -3.5626e-004,+1.4814e-004,+3.7932e-006,+0.0000e+000,+2.0801e-001, 
            +6.5640e-003,-3.4893e-003,-2.7395e-004,+7.4296e-005,-7.9927e-006, 
            -1.0277e-006,+0.0000e+000,+3.6515e-002,-7.4319e-003,-6.2873e-004, 
            -8.2461e-005,+3.1095e-005,-5.3860e-007,-1.2055e-007,-1.1517e-007, 
            +0.0000e+000,+3.1404e-002,+1.5580e-002,-1.1428e-003,+3.3529e-005, 
            +1.0387e-005,-1.9378e-006,-2.7327e-007,+7.5833e-009,-9.2323e-009}; 

            //% parameter t
            //t = sin(dlat);
            double t = Math.Sin(pos.Lat * SunMoonPosition.DegToRad);
            
            //% degree n and order m
            int n = 9;
            int m = 9;

            //% determine n!  (faktorielle)  moved by 1
            //dfac(1) = 1;
            //for i = 1:(2*n + 1)
            //  dfac(i+1) = dfac(i)*i;
            //end

            double[] dfac = new double[2 * n + 2];
            dfac[0] = 1;
            for(int i = 0; i < 19; i++)
            {
                dfac[i+1] = dfac[i] * (i+1);
            }

            //% determine Legendre functions (Heiskanen and Moritz, Physical Geodesy, 1967, eq. 1-62)
            //for i = 0:n
            //    for j = 0:min(i,m)
            //        ir = floor((i - j)/2);
            //        sum = 0;
            //        for k = 0:ir
            //            sum = sum + (-1)^k*dfac(2*i - 2*k + 1)/dfac(k + 1)/dfac(i - k + 1)/dfac(i - j - 2*k + 1)*t^(i - j - 2*k);
            //        end 
            //% Legendre functions moved by 1
            //        P(i + 1,j + 1) = 1.d0/2^i*sqrt((1 - t^2)^(j))*sum;
            //    end
            //end 
             
            double[][] P = new double[n + 1][];
            for (int ii = 0; ii < n + 1;ii++ )
            {
                P[ii] = new double[n + 1];
            }
                for (int ii = 0; ii <= n; ii++)
                {
                    for (int jj = 0; jj <= Math.Min(ii, m); jj++)
                    {
                        double tmp = (ii - jj) / 2.0;
                        double ir = Math.Floor(tmp);
                        double sum = 0;
                        for (int k = 0; k <= ir; k++)
                        {
                            sum = sum + Math.Pow(-1, k) * dfac[2 * ii - 2 * k] / dfac[k] / dfac[ii - k] / dfac[ii - jj - 2 * k] * Math.Pow(t, ii - jj - 2 * k);
                        }
                        // Legendre functions moved by 1
                        P[ii][jj] = 1.0 / Math.Pow(2, ii) * Math.Sqrt(Math.Pow(1 - Math.Pow(t, 2), jj)) * sum;
                    }
                }

            //% spherical harmonics
            //i = 0;
            //for n = 0:9
            //    for m = 0:n
            //        i = i + 1;
            //        aP(i) = P(n+1,m+1)*cos(m*dlon);
            //        bP(i) = P(n+1,m+1)*sin(m*dlon);
            //    end
            //end

            double[] aP = new double[55];
            double[] bP = new double[55];
            int index = 0;
            for (int ii = 0; ii <= n; ii++)
            {
                for (int jj = 0; jj <= ii; jj++)
                {                    
                    aP[index] = P[ii][jj] * Math.Cos(jj* pos.Lon * SunMoonPosition.DegToRad);
                    bP[index] = P[ii][jj] * Math.Sin(jj* pos.Lon * SunMoonPosition.DegToRad);
                    index = index +1;
                }
            }


            //% Geoidal height
            //undu = 0.d0;
            //for i = 1:55
            //   undu = undu + (a_geoid(i)*aP(i) + b_geoid(i)*bP(i));
            //end
            double undu = 0;
            for(int i = 0; i<55;i++)
            {
                undu = undu + a_geoid[i] * aP[i] + b_geoid[i] * bP[i];
            }

            //% orthometric height
            //hort = dhgt - undu;
            double hort = pos.Height - undu;

            //% Surface pressure on the geoid
            //apm = 0.d0;
            //apa = 0.d0;
            //for i = 1:55
            //    apm = apm + (ap_mean(i)*aP(i) + bp_mean(i)*bP(i));
            //    apa = apa + (ap_amp(i) *aP(i) + bp_amp(i) *bP(i));
            //end
            //pres0  = apm + apa*cos(doy/365.25d0*2.d0*pi);

            double apm = 0.0;
            double apa = 0.0;
            for(int i = 0; i < 55; i++)
            {
                apm = apm + ap_mean[i] * aP[i] + bp_mean[i] * bP[i];
                apa = apa + ap_amp[i] * aP[i] + bp_amp[i] * bP[i];
            }
            double pres0  = apm + apa * Math.Cos(doy / 365.25 * 2.0 * Math.PI);

            //% height correction for pressure
            //pres = pres0*(1.d0-0.0000226d0*hort)^5.225d0;

            double pres = pres0 * Math.Pow(1.0 - 0.00002260 * hort, 5.225);

            //% Surface temperature on the geoid
            //atm = 0.d0;
            //ata = 0.d0;
            //for i = 1:55
            //    atm = atm + (at_mean(i)*aP(i) + bt_mean(i)*bP(i));
            //    ata = ata + (at_amp(i) *aP(i) + bt_amp(i) *bP(i));
            //end 
            //temp0 =  atm + ata*cos(doy/365.25d0*2*pi);
            double atm = 0.0;
            double ata = 0.0;
            for(int i = 0; i < 55; i++)
            {
                atm = atm + (at_mean[i] * aP[i] + bt_mean[i] * bP[i]);
                ata = ata + (at_amp[i] *aP[i] + bt_amp[i] * bP[i]);
            }
            double temp0 =  atm + ata * Math.Cos(doy / 365.25 * 2 * Math.PI);

            //% height correction for temperature
            //temp = temp0 - 0.0065d0*hort;
            double temp = temp0 - 0.0065 * hort;
            #endregion 
            //saastamoninen model

            double z = SunMoonPosition.PI / 2 - azel[1];
            double e = 0;
            if(pos.Height < 1100)
            {
                e = 11.691 * Math.Pow(1 - 0.0068 * pos.Height / 288.15, 4);
            }
            else
            {
                e = 0;
            }

            double trph = 0.0022768 * pres / (1.0 - 0.00266 * Math.Cos(2.0 * pos.Lat * SunMoonPosition.DegToRad) - 0.00028 * pos.Height / 1e3) / Math.Cos(z);

            double trpw = 0.002277 * (1255.0 / (temp + 273.15) + 0.05) * e / Math.Cos(z);
            double[] res = new double[2];
            res[0] = trph;
            res[1] = trpw;
            return res;

        }

        /// <summary>
        /// troposphere mapping function
        /// compute tropospheric mapping function by GMF or NMF or......
        /// </summary>
        /// <param name="time">time</param>
        /// <param name="pos">receiver position {lat, lon, h} (rad, m)</param>
        /// <param name="zael">azimuth/elevation angle {az, el} (rad)</param>
        /// <param name="mapfw">wet mapping function (null : not output)</param>
        /// <returns></returns>
        public double tropmapf(Time time, GeoCoord pos, double[] azel,ref double mapfw)
        {
            Time ep = new Time(2000, 1, 1, 12);

            Geoid geoid=new Geoid();

            if (pos.Height < -1000.0 || pos.Height > 20000.0)
            {
                mapfw = 0.0;
                return 0.0;
            }


            double mjd = 51544.5 + (double)(time - ep) / 86400.0;

            double lat = pos.Lat * SunMoonPosition.DegToRad;

            double lon = pos.Lon * SunMoonPosition.DegToRad;

            double hgt = pos.Height - geoid.geoidh(pos);

            double zd = SunMoonPosition.PI / 2.0 - azel[1];

            double gmfh = 0.0, mapf = 0.0;

            gmf(mjd, lat, lon, hgt, zd, ref gmfh, ref mapf);

            mapfw = mapf;

            return gmfh;

        }

        private void gmf(double dmjd, double dlat, double dlon, double dhgt, double zd, ref double gmfh, ref double gmfw)
        {


            /* System generated locals */
            int i__1, i__2, i__3, i__4;
            double d__1, d__2;

            /* Local variables */
            int i__, j, k, m, n;
            double t, ah, bh, ch, aw, bw, cw;
            double[] p = new double[100];/* was [10][10] */
            double[] ap = new double[55];
            double[] bp = new double[55];
            double[] dfac = new double[20];
            int ir;
            double c0h, aha, c10h, c11h, ahm, awa, phh, awm, doy, sum1,
                 beta, a_ht__, b_ht__, c_ht__, sine, ht_corr_coef__,
                gamma, hs_km__, topcon, ht_corr__;
            /*  This routine is part of the International Earth Rotation and */
            /*  Reference Systems Service (IERS) Conventions software satData. */

            /*  This subroutine determines the Global Mapping Functions GMF (Boehm et al. 2006). */

            /*  In general, Class 1, 2, and 3 models represent physical effects that */
            /*  act on geodetic parameters while canonical models provide lower-level */
            /*  representations or basic computations that are used by Class 1, 2, or */
            /*  3 models. */

            /*  Status: Class 1 model */

            /*     Class 1 models are those recommended to be used a priori in the */
            /*     reduction of raw space geodetic satData in order to determine */
            /*     geodetic parameter estimates. */
            /*     Class 2 models are those that eliminate an observational */
            /*     singularity and are purely conventional in nature. */
            /*     Class 3 models are those that are not required as either Class */
            /*     1 or 2. */
            /*     Canonical models are accepted as is and cannot be classified as a */
            /*     Class 1, 2, or 3 model. */

            /*  Given: */
            /*     DMJD           d      Modified Julian Date */
            /*     DLAT           d      Latitude given in radians (North Latitude) */
            /*     DLON           d      Longitude given in radians (East Longitude) */
            /*     DHGT           d      Height in meters (mean sea level) */
            /*     ZD             d      Zenith distance in radians */

            /*  Returned: */
            /*     GMFH           d      Hydrostatic mapping function (Note 1) */
            /*     GMFW           d      Wet mapping function (Note 1) */

            /*  Notes: */

            /*  1) The mapping functions are dimensionless scale factors. */

            /*  2) This is from a 9x9 Earth Gravitational Model (EGM). */

            /*  Test case: */
            /*     given input: DMJD = 55055D0 */
            /*                  DLAT = 0.6708665767D0 radians (NRAO, Green Bank, WV) */
            /*                  DLON = -1.393397187D0 radians */
            /*                  DHGT = 844.715D0 meters */
            /*                  ZD   = 1.278564131D0 radians */

            /*     expected output: GMFH = 3.425245519339138678D0 */
            /*                      GMFW = 3.449589116182419257D0 */

            /*  References: */

            /*     Boehm, J., Niell, A., Tregoning, P. and Schuh, H., (2006), */
            /*     "Global Mapping Functions (GMF): A new empirical mapping */
            /*     function based on numerical weather model satData", */
            /*     Geophy. Res. Lett., Vol. 33, L07304, doi:10.1029/2005GL025545. */

            /*     Petit, G. and Luzum, B. (eds.), IERS Conventions (2010), */
            /*     IERS Technical Note No. 36, BKG (2010) */

            /*  Revisions: */
            /*  2005 August 30 J. Boehm    Original obsCodeode */
            /*  2009 August 11 B.E. Stetzler Added header and copyright */
            /*  2009 August 12 B.E. Stetzler More modifications and defined twopi */
            /*  2009 August 12 B.E. Stetzler Provided test case */
            /*  2009 August 12 B.E. Stetzler Capitalized all variables for FORTRAN 77 */
            /*                              compatibility and corrected test case */
            /*                              latitude and longitude coordinates */
            /* ----------------------------------------------------------------------- */
            /* +--------------------------------------------------------------------- */
            /*     Reference secondOfWeek is 28 January 1980 */
            /*     This is taken from Niell (1996) to be consistent */
            /* ---------------------------------------------------------------------- */

            /* +--------------------------------------------------------------------- */
            /*     Reference secondOfWeek is 28 January 1980 */
            /*     This is taken from Niell (1996) to be consistent */
            /* ---------------------------------------------------------------------- */
            doy = dmjd - 44239.0 - 27;
            /*     Define a parameter t */
            t = Math.Sin(dlat);
            /*     Define degree n and order m EGM */
            n = 9;
            m = 9;

            /*     Determine n!  (factorial)  moved by 1 */
            dfac[0] = 1.0;
            i__1 = (n << 1) + 1;
            for (i__ = 1; i__ <= i__1; ++i__)
            {
                dfac[i__] = dfac[i__ - 1] * i__;
            }


            /*     Determine Legendre functions (Heiskanen and Moritz, */
            /*     Physical Geodesy, 1967, eq. 1-62) */
            i__1 = n;
            for (i__ = 0; i__ <= i__1; ++i__)
            {
                i__2 = Math.Min(i__, m);
                for (j = 0; j <= i__2; ++j)
                {
                    ir = (i__ - j) / 2;
                    sum1 = 0.0;
                    i__3 = ir;
                    for (k = 0; k <= i__3; ++k)
                    {
                        i__4 = i__ - j - (k << 1);
                        sum1 += Math.Pow(c_n1, k) * dfac[(i__ << 1) - (k << 1)] /
                            dfac[k] / dfac[i__ - k] / dfac[i__ - j - (k << 1)] *
                            Math.Pow(t, i__4);
                    }
                    /*         Legendre functions moved by 1 */
                    /* Computing 2nd power */
                    d__2 = t;
                    d__1 = 1 - d__2 * d__2;
                    p[i__ + 1 + (j + 1) * 10 - 11] = 1.0 / Math.Pow(c__2, i__) * Math.Sqrt(
                        Math.Pow(d__1, j)) * sum1;
                }
            }
            /*     Calculate spherical harmonics */
            i__ = 0;
            for (n = 0; n <= 9; ++n)
            {
                i__1 = n;
                for (m = 0; m <= i__1; ++m)
                {
                    ++i__;
                    ap[i__ - 1] = p[n + 1 + (m + 1) * 10 - 11] * Math.Cos(m * dlon);
                    bp[i__ - 1] = p[n + 1 + (m + 1) * 10 - 11] * Math.Sin(m * dlon);
                }
            }


            /*     Compute hydrostatic mapping function */
            bh = 0.0029;
            c0h = 0.062;
            if (dlat < 0.0)
            {
                /* SOUTHERN HEMISPHERE */
                phh = 3.1415926535897932384626433;
                c11h = 0.007;
                c10h = 0.002;
            }
            else
            {
                /* NORTHERN HEMISPHERE */
                phh = 0.0;
                c11h = 0.005;
                c10h = 0.001;
            }
            ch = c0h + ((Math.Cos(doy / 365.25 * 6.283185307179586476925287 + phh) + 1.0) *
                c11h / 2.0 + c10h) * (1.0 - Math.Cos(dlat));
            ahm = 0.0;
            aha = 0.0;
            for (i__ = 1; i__ <= 55; ++i__)
            {
                ahm += (ah_mean__[i__ - 1] * ap[i__ - 1] + bh_mean__[i__ - 1] * bp[
                    i__ - 1]) * 1e-5;
                aha += (ah_amp__[i__ - 1] * ap[i__ - 1] + bh_amp__[i__ - 1] * bp[i__
                    - 1]) * 1e-5;
            }
            ah = ahm + aha * Math.Cos(doy / 365.25 * 6.283185307179586476925287);
            sine = Math.Sin(1.5707963267948966 - zd);
            beta = bh / (sine + ch);
            gamma = ah / (sine + beta);
            topcon = ah / (bh / (ch + 1.0) + 1.0) + 1.0;
            gmfh = topcon / (sine + gamma);
            /*     Height correction for hydrostatic mapping function from Niell (1996) */
            a_ht__ = 2.53e-5;
            b_ht__ = 0.00549;
            c_ht__ = 0.00114;
            hs_km__ = dhgt / 1e3;
            beta = b_ht__ / (sine + c_ht__);
            gamma = a_ht__ / (sine + beta);
            topcon = a_ht__ / (b_ht__ / (c_ht__ + 1.0) + 10.0) + 1.0;
            ht_corr_coef__ = 1.0 / sine - topcon / (sine + gamma);
            ht_corr__ = ht_corr_coef__ * hs_km__;
            gmfh += ht_corr__;
            /*     Compute wet mapping function */
            bw = 0.00146;
            cw = 0.04391;
            awm = 0.0;
            awa = 0.0;
            for (i__ = 1; i__ <= 55; ++i__)
            {
                awm += (aw_mean__[i__ - 1] * ap[i__ - 1] + bw_mean__[i__ - 1] * bp[
                    i__ - 1]) * 1e-5;
                awa += (aw_amp__[i__ - 1] * ap[i__ - 1] + bw_amp__[i__ - 1] * bp[i__
                    - 1]) * 1e-5;
            }
            aw = awm + awa * Math.Cos(doy / 365.25 * 6.283185307179586476925287);
            beta = bw / (sine + cw);
            gamma = aw / (sine + beta);
            topcon = aw / (bw / (cw + 1.0) + 1.0) + 1.0;
            gmfw = topcon / (sine + gamma);

        }


        static int c_n1 = -1;
        static int c__2 = 2;
        /* Initialized satData */
        //[55]
        static double[] ah_mean__ = new double[]{ 125.17,0.8503,0.06936,-6.76,0.1771,0.0113,
	    0.5963,0.01808,0.002801,-0.001414,-1.212,0.093,0.003683,0.001095,
	    4.671e-5,0.3959,-0.03867,0.005413,-5.289e-4,3.229e-4,2.067e-5,0.3,
	   0.02031,0.0059,4.573e-4,-7.619e-5,2.327e-6,3.845e-6,0.1182,0.01158,
	   0.005445,6.219e-5,4.204e-6,-2.093e-6,1.54e-7,-4.28e-8,-0.4751,
	    -0.0349,0.001758,4.019e-4,-2.799e-6,-1.287e-6,5.468e-7,7.58e-8,
	    -6.3e-9,-0.116,0.008301,8.771e-4,9.955e-5,-1.718e-6,-2.012e-6,
	    1.17e-8,1.79e-8,-1.3e-9,1e-10 };
        //[55]
         static double[]  bh_mean__ =new double[] { 0.0,0.0,0.03249,0.0,0.03324,0.0185,0.0,
	    -0.1115,0.02519,0.004923,0.0,0.02737,0.01595,-7.332e-4,1.933e-4,0.0,
	    -0.04796,0.006381,-1.599e-4,-3.685e-4,1.815e-5,0.0,0.07033,0.002426,
	    -0.001111,-1.357e-4,-7.828e-6,2.547e-6,0.0,0.005779,0.003133,
	    -5.312e-4,-2.028e-5,2.323e-7,-9.1e-8,-1.65e-8,0.0,0.03688,-8.638e-4,
	    -8.514e-5,-2.828e-5,5.403e-7,4.39e-7,1.35e-8,1.8e-9,0.0,-0.02736,
	    -2.977e-4,8.113e-5,2.329e-7,8.451e-7,4.49e-8,-8.1e-9,-1.5e-9,
	    2e-10 };
         static double[] ah_amp__ = new double[]{ -0.2738,-2.837,0.01298,-0.3588,0.02413,
	    0.03427,-0.7624,0.07272,0.0216,-0.003385,0.4424,0.03722,0.02195,-0.001503,
	    2.426e-4,0.3013,0.05762,0.01019,-4.476e-4,6.79e-5,3.227e-5,0.3123,
	    -0.03535,0.00484,3.025e-6,-4.363e-5,2.854e-7,-1.286e-6,-0.6725,
	    -0.0373,8.964e-4,1.399e-4,-3.99e-6,7.431e-6,-2.796e-7,-1.601e-7,
	    0.04068,-0.01352,7.282e-4,9.594e-5,2.07e-6,-9.62e-8,-2.742e-7,
	    -6.37e-8,-6.3e-9,0.08625,-0.005971,4.705e-4,2.335e-5,4.226e-6,
	    2.475e-7,-8.85e-8,-3.6e-8,-2.9e-9,0.0 };
    static double[] bh_amp__ =new double[] { 0.0,0.0,-0.1136,0.0,-0.1868,-0.01399,0.0,
	    -0.1043,0.01175,-0.00224,0.0,-0.03222,0.01333,-0.002647,-2.316e-5,0.0,
	   0.05339,0.01107,-0.003116,-1.079e-4,-1.299e-5,0.0,0.004861,0.008891,
	    -6.448e-4,-1.279e-5,6.358e-6,-1.417e-7,0.0,0.03041,0.00115,-8.743e-4,
	    -2.781e-5,6.367e-7,-1.14e-8,-4.2e-8,0.0,-0.02982,-0.003,1.394e-5,
	    -3.29e-5,-1.705e-7,7.44e-8,2.72e-8,-6.6e-9,0.0,0.01236,-9.981e-4,
	    -3.792e-5,-1.355e-5,1.162e-6,-1.789e-7,1.47e-8,-2.4e-9,-4e-10 };
    static double[] aw_mean__ =new double[] { 56.4,1.555,-1.011,-3.975,0.03171,0.1065,
	    0.6175,0.1376,0.04229,0.003028,1.688,-0.1692,0.05478,0.02473,6.059e-4,
	    2.278,0.006614,-3.505e-4,-0.006697,8.402e-4,7.033e-4,-3.236,0.2184,
	    -0.04611,-0.01613,-0.001604,5.42e-5,7.922e-5,-0.2711,-0.4406,-0.03376,
	    -0.002801,-4.09e-4,-2.056e-5,6.894e-6,2.317e-6,1.941,-0.2562,0.01598,
	    0.005449,3.544e-4,1.148e-5,7.503e-6,-5.667e-7,-3.66e-8,0.8683,
	    -0.05931,-0.001864,-1.277e-4,2.029e-4,1.269e-5,1.629e-6,9.66e-8,
	    -1.015e-7,-5e-10 };
    static double[] bw_mean__ = new double[]{ 0.0,0.0,0.2592,0.0,0.02974,-0.5471,0.0,
	    -0.5926,-0.103,-0.01567,0.0,0.171,0.09025,0.02689,0.002243,0.0,0.3439,
	    0.02402,0.00541,0.001601,9.669e-5,0.0,0.09502,-0.03063,-0.001055,
	    -1.067e-4,-1.13e-4,2.124e-5,0.0,-0.3129,0.008463,2.253e-4,7.413e-5,
	    -9.376e-5,-1.606e-6,2.06e-6,0.0,0.2739,0.001167,-2.246e-5,-1.287e-4,
	    -2.438e-5,-7.561e-7,1.158e-6,4.95e-8,0.0,-0.1344,0.005342,3.775e-4,
	    -6.756e-5,-1.686e-6,-1.184e-6,2.768e-7,2.73e-8,5.7e-9 };
    static double[] aw_amp__ = new double[]{ 0.1023,-2.695,0.3417,-0.1405,0.3175,0.2116,
	    3.536,-0.1505,-0.0166,0.02967,0.3819,-0.1695,-0.07444,0.007409,-0.006262,
	    -1.836,-0.01759,-0.06256,-0.002371,7.947e-4,1.501e-4,-0.8603,-0.136,
	    -0.03629,-0.003706,-2.976e-4,1.857e-5,3.021e-5,2.248,-0.1178,0.01255,
	    0.001134,-2.161e-4,-5.817e-6,8.836e-7,-1.769e-7,0.7313,-0.1188,
	    0.01145,0.001011,1.083e-4,2.57e-6,-2.14e-6,-5.71e-8,2e-8,-1.632,
	    -0.006948,-0.003893,8.592e-4,7.577e-5,4.539e-6,-3.852e-7,-2.213e-7,
	    -1.37e-8,5.8e-9 };
    static double[] bw_amp__ =new double[] { 0.0,0.0,-0.08865,0.0,-0.4309,0.0634,0.0,0.1162,
	    0.06176,-0.004234,0.0,0.253,0.04017,-0.006204,0.004977,0.0,-0.1737,
	    -0.005638,1.488e-4,4.857e-4,-1.809e-4,0.0,-0.1514,-0.01685,0.005333,
	    -7.611e-5,2.394e-5,8.195e-6,0.0,0.09326,-0.01275,-3.071e-4,5.374e-5,
	    -3.391e-5,-7.436e-6,6.747e-7,0.0,-0.08637,-0.003807,-6.833e-4,
	    -3.861e-5,-2.268e-5,1.454e-6,3.86e-7,-1.068e-7,0.0,-0.02658,
	    -0.001947,7.131e-4,-3.506e-5,1.885e-7,5.792e-7,3.99e-8,2e-8,
        -5.7e-9};   
   
    }
}
