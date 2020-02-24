// 2014.09.08, czs, edit, 注意：不对钟差相对论误差进行改正，此处时间是卫星钟时间，是原始的观测数据，没有相对改正信息。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Gnsser;
using Geo.Coordinates;
using Geo.Referencing;
using Gnsser.Times;
using Geo.Algorithm;
using Geo.Times;
using Gnsser.Data.Rinex;

namespace Gnsser
{
    /// <summary>
    /// 星历插值器。目前采用切比雪夫和拉格朗日差值。
    /// 传入  EphemerisInfo 列表 也可是 Sp3Record 列表，可以从数据库或者直接从文件获取。
    /// 每次只能处理一个卫星，即PRN，否则容易引起混乱。
    /// 注意：传入的卫星列表默认为已经进行过筛选了,默认与待获取星历相近。将直接进行插值，而不再判断。
    /// </summary>
    public class OtherInterpolator
    {
         
        /// <summary>
        /// 初始化时，输入要插值的星历信息列表。
        /// 列表至少包含 2 个星历。
        /// </summary>
        /// <param name="oldInfos"></param>
        public OtherInterpolator(List<Ephemeris> oldInfos)
        {
            this.sortedRecords = oldInfos; 
        }  

        List<Ephemeris> sortedRecords { get; set; }

        #region 崔阳翻译
        XYZ satSpeed;
        XYZ satPos;
        /// <summary>
        /// 差值计算
        /// </summary>
        /// <param name="rTime"></param>
        /// <param name="staXyz"></param>
        private void SatPo(Time rTime, XYZ staXyz)
        {
            int count = sortedRecords.Count;
            double[] TimeArr = new double[count];
            double[] XArr = new double[count];
            double[] YArr = new double[count];
            double[] ZArr = new double[count];
            double[] CLKsat = new double[count];

            for (int i = 0; i < count; i++)
            {
                Ephemeris record = sortedRecords[i];
                TimeArr[i] = record.Time.SecondsOfWeek; //Y为GPS周秒。
                XArr[i] = record.XYZ.X;
                YArr[i] = record.XYZ.Y;
                ZArr[i] = record.XYZ.Z;
                CLKsat[i] = record.ClockBias;
            }

            double Time = rTime.SecondsOfWeek;//卫星发射时刻
            double startTime = TimeArr[0] - 5;
            double lastTime = TimeArr[TimeArr.Length - 1] + 5;
            double timeInterval;

            int d = 9;
            double NA = 99999;

            double x1 = 0.0, y1 = 0.0, z1 = 0.0;
            double[] x2 = new double[d]; double[] y2 = new double[d]; double[] z2 = new double[d];
            double x3 = 0.0, y3 = 0.0, z3 = 0.0;
            double x4 = 0.0, y4 = 0.0, z4 = 0.0;

            double Xt, Yt, Zt;
            double rho; double recieveTime;

            if (Time >= startTime && Time <= lastTime)
            {
                timeInterval = TimeArr[2] - TimeArr[1];
                //
                XYZ(Time, TimeArr, XArr, d, NA, ref x1, ref x2, ref x3, ref x4);
                if (x4 == 0)
                {
                    satSpeed = new XYZ(0, 0, 0);
                    return;
                }
                //拉格朗日插值
                Xt = LG(d, x1, x2, NA);
                XYZ(Time, TimeArr, YArr, d, NA, ref y1, ref y2, ref y3, ref y4);
                Yt = LG(d, y1, y2, NA);
                XYZ(Time, TimeArr, ZArr, d, NA, ref z1, ref z2, ref z3, ref z4);
                Zt = LG(d, z1, z2, NA);

                //
                satPos = new XYZ(Xt, Yt, Zt);
                rho = (satPos - staXyz).Length;

                recieveTime = rTime.SecondsOfWeek + rho / GnssConst.LIGHT_SPEED; //接受时刻
            }
            else
            {
                throw new Exception("rTime is out of range in TimeArr:SeeSatPo\n");

            }
            double t, stdx;
            for (int i = 1; i <= 3; i++)
            {
                t = recieveTime - rho / GnssConst.LIGHT_SPEED;
                stdx = ((Time - x3) / timeInterval) + 1;
                Xt = LG(d, stdx, x2, NA);
                Yt = LG(d, stdx, y2, NA);
                Zt = LG(d, stdx, z2, NA);
                satPos = new XYZ(Xt, Yt, Zt);
                rho = (satPos - staXyz).Length;
            }

            stdx = Convert.ToInt32(Math.Round(x1));
            double Dinterval;
            if (Convert.ToInt32(stdx) == 1 || Convert.ToInt32(stdx) == d)
            {
                Dinterval = timeInterval;
            }
            else
            {
                Dinterval = 2 * timeInterval;
            }
            int B, L;
            if (Convert.ToInt32(stdx) == 1)
            {
                B = 1;
            }
            else
            {
                B = Convert.ToInt32(stdx) - 1;
            }
            if (Convert.ToInt32(stdx) == d)
            {
                L = d;
            }
            else
            {
                L = Convert.ToInt32(stdx) + 1;
            }
            double Vx, Vy, Vz;
            Vx = (x2[L - 1] - x2[B - 1]) / Dinterval;
            Vy = (y2[L - 1] - y2[B - 1]) / Dinterval;
            Vz = (z2[L - 1] - z2[B - 1]) / Dinterval;

            satSpeed = new XYZ(Vx, Vy, Vz);
        }

        /// <summary>
        /// Returns Standardzied Time & extracts PoArr for Lagrange Interpolation (LG) and time at the start point LG
        /// </summary>
        /// <param name="timeX">GPS time epoch at which you need to interpolate</param>
        /// <param name="TimeArr"> GPS time array corresponding to array of PoArr</param>
        /// <param name="PoArr">GPS coordinate X, Y or Z corresponding to TimeArr</param>
        /// <param name="d"> number of points used for the Lagrange interpolation</param>
        /// <param name="NA"></param>
        /// <param name="stdX"></param>
        /// <param name="XtractPoArr"></param>
        /// <param name="TimeArr"></param>
        /// <param name="index"></param>
        private void XYZ(double timeX, double[] TimeArr, double[] PoArr, int d, double NA, ref double stdX, ref double[] XtractPoArr, ref double TimeArrs, ref double index)
        {
            double rowTimeArr = TimeArr.Length;
            double rowPoArr = PoArr.Length;

            if (rowTimeArr != rowPoArr)
            {
                throw new Exception("'Err(TimeArr, PoArr)\n");
            }
            else if (d > rowTimeArr)
            {
                throw new Exception("Not enough data using d\n");
            }
            double timeInterval = TimeArr[2] - TimeArr[1];
            int PositionTimeArr = Convert.ToInt32(Math.Ceiling((timeX - TimeArr[0]) / timeInterval) + 1);

            d = 9;//9阶的LG插值
            XtractPoArr = new double[d];
            if (PoArr[PositionTimeArr - 1] == NA)
            {
                stdX = 0;
                for (int i = 0; i < d; i++)
                { XtractPoArr[i] = 0; }
                TimeArrs = 0;
                index = 0;
                return;
            }

            if (PositionTimeArr < Convert.ToInt32(Math.Ceiling(Convert.ToDouble(d) / 2)))
            {
                for (int i = 0; i < d; i++)
                {
                    XtractPoArr[i] = PoArr[i];
                }
                stdX = ((timeX - TimeArr[0]) / timeInterval) + 1;
                TimeArrs = TimeArr[0];
                index = 1;
                return;
            }
            else
            {
                //
                double NearEnd = rowTimeArr - PositionTimeArr;
                int StartPt;
                if (NearEnd < Math.Floor(Convert.ToDouble(d) / 2))
                {
                    StartPt = Convert.ToInt32(rowTimeArr - d + 1);
                    for (int i = 0; i < d; i++)
                    {
                        XtractPoArr[i] = PoArr[StartPt + i - 1];
                    }
                }
                else
                {
                    for (int i = 0; i < d; i++)
                    {
                        int ptr = Convert.ToInt32(PositionTimeArr - Math.Floor(Convert.ToDouble(d) / 2) + i - 1);
                        XtractPoArr[i] = PoArr[ptr];
                    }
                    StartPt = Convert.ToInt32(PositionTimeArr - Math.Floor(Convert.ToDouble(d) / 2));
                }
                stdX = ((timeX - TimeArr[StartPt - 1]) / timeInterval) + 1;
                TimeArrs = TimeArr[StartPt - 1];
                index = 2;
                return;

            }

        }

        /// <summary>
        ///  Lagrange(LG) Interpolation Polynomial
        /// </summary>
        /// <param name="d">number of satData points used in LG</param>
        /// <param name="stdT">standardized time; computed in subroutine X</param>
        /// <param name="Fx">tabulated satellite ephemeris & clock; vector of aboutSize d</param>
        /// <param name="NA"></param>
        /// <returns></returns>
        private double LG(int d, double stdT, double[] Fx, double NA)
        {
            double LLG;
            if (Fx.Length == 1)
            {
                LLG = NA;
                return LLG;
            }
            else if (d < 2)
            {
                throw new Exception("Err: Degree of interpolation must more than 2.\n");
            }
            else if (Fx.Length != d)
            { throw new Exception("Err: Fx must has the same point as d.\n"); }

            int numNA = 0;
            int rowFx = Fx.Length;
            for (int s = 0; s < rowFx; s++)
            {
                if (Fx[s] == NA)
                {
                    numNA += 1;
                }
                //
            }
            if (numNA > Math.Ceiling(0.3 * rowFx))
            {
                LLG = NA;
                return LLG;
            }
            double[] LLGG = new double[d];
            double[] nLG = new double[d];
            double[] dLG = new double[d];
            for (int i = 0; i < d; i++)
            {
                LLGG[i] = 0;
                nLG[i] = 1;
            }
            dLG = prodd(d, Fx, NA);

            double[] x = new double[d];
            x[0] = 1;
            for (int i = 0; i < d - 1; i++)
            {
                x[i + 1] = x[i] + 1;
            }
            for (int i = 0; i < d; i++)
            {
                if (Fx[i] != NA)
                {
                    for (int j = 0; j < d; j++)
                    {
                        if (Fx[j] != NA && i != j)
                        {
                            nLG[i] = nLG[i] * (stdT - x[j]);
                        }
                    }
                    LLGG[i] = nLG[i] / dLG[i];
                }
            }
            LLG = 0.0;
            for (int i = 0; i < d; i++)
            {
                LLG += LLGG[i] * Fx[i];
            }
            return LLG;
        }

        /// <summary>
        /// calculate LG Denomenator
        /// </summary>
        /// <param name="d">number of satData points used in LG</param>
        /// <param name="Fx">tabulated satellite ephemeris & clock, vector of aboutSize d</param>
        /// <param name="NA"></param>
        /// <returns></returns>
        private double[] prodd(int d, double[] Fx, double NA)
        {
            double[] x = new double[d];
            double[] L = new double[d];
            x[0] = 1;
            L[0] = 1;

            for (int a = 0; a < d - 1; a++)
            {
                x[a + 1] = x[a] + 1;
                L[a + 1] = 1;
            }
            for (int i = 0; i < d; i++)
            {
                if (Fx[i] != NA)
                {
                    for (int j = 0; j < d; j++)
                    {
                        if (j != i && Fx[j] != NA)
                        {
                            L[i] = L[i] * (x[i] - x[j]);
                        }
                    }
                }
            }
            return L;
        }

        #endregion
    }
}
