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


namespace Gnsser
{
    /// <summary>
    /// 星历插值器。目前采用切比雪夫和拉格朗日差值。
    /// 传入  EphemerisInfo 列表 也可是 Sp3Record 列表，可以从数据库或者直接从文件获取。
    /// 每次只能处理一个卫星，即PRN，否则容易引起混乱。
    /// </summary>
    public class EphemerisInterpolator1
    {
        /// <summary>
        /// 初始化时，输入要插值的星历信息列表。
        /// 列表至少包含 2 个星历。
        /// </summary>
        /// <param name="oldInfos"></param>
        public EphemerisInterpolator1(List<Ephemeris> oldInfos)
        {
            this.sortedRecords = oldInfos;
            Init();
        }
        public EphemerisInterpolator1(Ephemeris[] sortedRecords)
        {
            this.sortedRecords = new List<Ephemeris>(sortedRecords);
            Init();
        }
        Ephemeris Ephemeris { get { return entities.First; } }
        public EphemerisInterpolator1(Data.Rinex.EphemerisStorage entities)
        {
            // TODO: Complete member initialization
            this.entities = entities;
            Init();
        }

        List<Ephemeris> sortedRecords;
        IGetY fitX, fitY, fitZ, fitClock;

        private void Init()
        {
            int count = entities.Count;
            double[] x = new double[count];
            double[] yX = new double[count];
            double[] yY = new double[count];
            double[] yZ = new double[count];
            double[] yClock = new double[count];

            int i = 0;
            foreach (var item in entities)
            {
                Ephemeris record = item;
                x[i] = (record.Time - entities.First.Time); //Y为GPS周秒。
                yX[i] = record.XYZ.X;
                yY[i] = record.XYZ.Y;
                yZ[i] = record.XYZ.Z;
                yClock[i] = record.ClockBias;

                i++;
            }

            //for (int i = 0; i < count; i++)
            //{
            //    Ephemeris record = sortedRecords[i];
            //    x[i] = (record.Time - sortedRecords[0].Time); //Y为GPS周秒。
            //    yX[i] = record.XYZ.X;
            //    yY[i] = record.XYZ.Y;
            //    yZ[i] = record.XYZ.Z;
            //    yClock[i] = record.ClockBias;
            //}
            int order = 10;
            order = Math.Min(order, count);

            //fitX = new PolyFit(x, yX);
            //fitY = new PolyFit(x, yY);
            //fitZ = new PolyFit(x, yZ);
            //fitClock = new PolyFit(x, yClock);
            fitX = new LagrangeInterplation(x, yX, order);
            fitY = new LagrangeInterplation(x, yY, order);
            fitZ = new LagrangeInterplation(x, yZ, order);
            fitClock = new LagrangeInterplation(x, yClock, 2);
            //fitClock = new LagrangeInterplation(x, yClock, 10);


            //fitX = new ChebyshevPolyFit(x, yX, order);
            //fitY = new ChebyshevPolyFit(x, yY, order);
            //fitZ = new ChebyshevPolyFit(x, yZ, order);
            //fitClock = new LagrangeInterplation(x, yClock, 2);
        }

        /// <summary>
        /// 获取插值后的 EphemerisInfo。
        /// 注意：不对钟差相对论误差进行改正，此处时间是卫星钟时间，没有相对信息。2014.09.08
        /// 失败，如超过时间，则返回null
        /// 由于一般没有卫星速度的数据，必须通过插值坐标计算速度。2015.03.
        /// 如果有速度信息，以下的方法就不合适了。
        /// </summary> 
        /// <param name="gpsTime">时间</param>
        /// <returns></returns>
        public Ephemeris GetEphemerisInfo(Time gpsTime)
        {




            SatelliteNumber PRN = Ephemeris.Prn;
            Time min = Ephemeris.Time;
            //Time max = sortedRecords[sortedRecords.Count - 1].Time;
            //double span = (double)(max - min);
            double gpsSecond = (gpsTime - min); //减去序列中第一个数
            ////老是在这里出问题，宽限一点呢？？16分钟
            //double expand = 60 * 16;
            //if (gpsSecond + expand < 0 || gpsSecond - expand > span)
            //{
            //    //return null;
            //    throw new ArgumentException("历元在给定的时间段外，不可进行插值。");
            //}
            //计算位置
            double x = 0, y = 0, z = 0; double dtime = 0;
            double dx = 0, dy = 0, dz = 0; double ddtime = 0;


            int count = entities.Count;
            int order = 10;
            order = Math.Min(order, count);

            //czs
         //   List<int> indexes = GetNearstIndexes(sortedRecords, gpsSecond, order);
            //cy
            //List<int> indexes = GetNearstIndexes(sortedRecords, gpsTime, order);





            double[] t = new double[order];


            double[] tList = new double[order + 1];

            double[] xList = new double[order + 1];
            double[] yList = new double[order + 1];
            double[] zList = new double[order + 1];
            double[] clockList = new double[order + 1];
            
            int i = 0;
            foreach (var item in this.entities)
            {
                Ephemeris record = item;

                t[i] =  (record.Time - gpsTime); //秒。


                #region 方案1：顾及地球自转效应
                //correction for earth rotation
                double sinl = Math.Sin(SunMoonPosition.OMGE * t[i]);
                double cosl = Math.Cos(SunMoonPosition.OMGE * t[i]);

                xList[i] = cosl * record.XYZ.X - sinl * record.XYZ.Y;
                yList[i] = sinl * record.XYZ.X + cosl * record.XYZ.Y;
                zList[i] = record.XYZ.Z;
                #endregion


                #region 方案2：不顾及地球自转效应

                //xList[time] = record.XYZ.X;
                //yList[time] = record.XYZ.Y;
                //zList[time] = record.XYZ.Z;

                #endregion

                //tList[i] = (record.Time - sortedRecords[0].Time); //秒。
              //  clockList[i] = record.ClockBias;

                i++;
            }


            //Lagrange(tList, xList, gpsSecond, ref x, ref dx);

            //Lagrange(tList, yList, gpsSecond, ref y, ref dy);

            //Lagrange(tList, zList, gpsSecond, ref z, ref dz);

            //Lagrange(tList, clockList, gpsSecond, ref dtime, ref ddtime);

            //int Nhalf = (int)(order / 2) - 1;
            //int Nmatch = indexes[Nhalf];



            x = fitX.GetY(gpsSecond);
            y = fitY.GetY(gpsSecond);
            z = fitZ.GetY(gpsSecond);
            dtime = fitClock.GetY(gpsSecond);

            XYZ SatXyz = new XYZ(x, y, z);


            //精度赋值 
            //XYZ SatXyzSig = sortedRecords[Nmatch].XyzSdev;
            //double clockSig = sortedRecords[Nmatch].ClockSdev;
            //XYZ SatXyzDotSig = new Geo.Coordinates.XYZ(0);

            //计算速度
            //double nextSecond = gpsSecond + 0.001;
            //XYZ xyzNext = new XYZ(fitX.GetY(nextSecond), fitY.GetY(nextSecond), fitZ.GetY(nextSecond));
            //XYZ speed = (xyzNext-xyz) / 0.001;


            XYZ SatSpeed = new XYZ(dx, dy, dz);

            //Add relativity correction to dtime 
            //This only for consistency with GPSEphemerisInter
            //dtr=-2*dot(R,V)/(C*C)

            //dtime +=EphemerisUtil.GetRelativeCorrection(SatXyz, SatSpeed);

            return new Ephemeris()
            {
                Prn = PRN,
                XYZ = SatXyz, 
                //XyzSdev=SatXyzSig,
                // Clock = fitClock.GetY(gpsSecond),
                ClockBias = dtime,
                //ClockSdev = clockSig,
                ClockDrift = ddtime,
                Time = gpsTime,
                XyzDot = SatSpeed,
                //XyzDotSdev=SatXyzDotSig
            };
        }


        /// <summary>
        /// 获取指定数组中与 X 最相近的数组编号。 
        /// </summary>
        /// <param name="XArray">递增或递减数组</param>
        /// <param name="xValue"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<int> GetNearstIndexes(List<Ephemeris> sortedRecords, double xValue, int order = 10)
        {
            int count = sortedRecords.Count;
            double[] XArray = new double[count];
            //double[] yX = new double[count];
            //double[] yY = new double[count];
            //double[] yZ = new double[count];
            //double[] yClock = new double[count];

            for (int i = 0; i < count; i++)
            {
                Ephemeris record = sortedRecords[i];
                XArray[i] =  (record.Time - sortedRecords[0].Time); //Y为GPS周秒。
                //yX[time] = record.XYZ.X;
                //yY[time] = record.XYZ.Y;
                //yZ[time] = record.XYZ.Z;
                //yClock[time] = record.Offset;
            }
            //int order = 10;
            order = Math.Min(order, count);



            List<int> indexes = new List<int>();
            //如果数量大于数组数量，则返回全部
            if (order >= XArray.Length)
            {
                for (int i = 0; i < order; i++)
                {
                    indexes.Add(i);
                }
                return indexes;
            }
            //找到离X最小的编号
            int halfCount = order / 2;
            int index = 0;
            double min = double.MaxValue;
            for (int i = 0; i < XArray.Length; i++)
            {
                double diff = Math.Abs(XArray[i] - xValue);
                // if (diff == 0) return YArray[time];
                if (diff < min)
                {
                    min = diff;
                    index = i;
                }
            }
            //在数组的头部
            if (index - halfCount <= 0) for (int i = 0; i < order; i++) indexes.Add(i);
            //尾部
            else if (index + halfCount >= XArray.Length - 1) for (int i = 0, j = XArray.Length - 1; i < order; i++, j--) indexes.Insert(0, j);
            //中间
            else for (int i = 0; i < order; i++) indexes.Add(index - halfCount + i);

            if (indexes[0] < 0) throw new Exception("出错了");

            return indexes;
        }

        /// <summary>
        /// 获取指定数组中与 X 最相近的数组编号。 
        /// </summary>
        /// <param name="XArray">递增或递减数组</param>
        /// <param name="xValue"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<int> GetNearstIndexes(List<Ephemeris> sortedRecords, Time xValue, int order = 10)
        {
            int count = sortedRecords.Count;
            
            order = Math.Min(order, count);

            int i,j,k = 0;
            List<int> indexes = new List<int>();
            //如果数量大于数组数量，则返回全部
            if (order >= count)
            {
                for (i = 0; i < order; i++)
                {
                    indexes.Add(i);
                }
                return indexes;
            }


            //找到离X最近的编号 binary search
            int index = 0;
           
            for ( i = 0, j = count - 1; i < j; )
            {
                k = (i + j) / 2;
                if ( (sortedRecords[k].Time - xValue) < 0.0)
                { i = k + 1; }
                else { j = k; }
            }

            index = i <= 0 ? 0 : i - 1;

            //polynomial interpolation for orbit
            i = index - (order + 1) / 2;

            if (i < 0) i = 0; else if ((i + order) >= count) i = count - order - 1;
           
            for (j = 0; j <= order; j++)
            {
                indexes.Add(i + j);
            }

            return indexes;
        }



        /// <summary>
        /// 拉格朗日插值 崔阳
        /// </summary>
        /// <param name="xs"></param>
        /// <param name="ys"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dydx"></param>
        /// <returns></returns>
        public static void Lagrange(double[] xs, double[] ys, double x, ref double y, ref double dydx)
        {
            if (xs.Length != ys.Length) throw new ArgumentException("数组大小必须一致.");


            int N = xs.Length;
            int M = (N * (N + 1)) / 2;
            double[] P = new double[N];
            double[] Q = new double[M];
            double[] D = new double[N];
            for (int i = 0; i < N; i++) { P[i] = 1.0; D[i] = 1.0; }
            for (int i = 0; i < M; i++) { Q[i] = 1.0; }

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i != j)
                    {
                        P[i] *= x - xs[j];
                        D[i] *= xs[i] - xs[j];
                        if (i < j)
                        {
                            for (int k = 0; k < N; k++)
                            {
                                if (k == i || k == j) continue;
                                Q[i + (j * (j + 1)) / 2] *= x - xs[k];
                            }
                        }
                    }
                }
            }

            y = 0.0;
            dydx = 0.0;
            for (int i = 0; i < N; i++)
            {
                y += ys[i] * (P[i] / D[i]);
                double S = 0;
                for (int k = 0; k < N; k++) if (i != k)
                    {
                        if (k < i) S += Q[k + (i * (i + 1)) / 2] / D[i];
                        else S += Q[i + (k * (k + 1)) / 2] / D[i];
                    }
                dydx += ys[i] * S;
            }
        }

        #region 崔阳翻译
        XYZ satSpeed;
        XYZ satPos;
        private Data.Rinex.EphemerisStorage entities;
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
