//2019.01.12, czs, edit in hmx, ��˹ת���ع�������ƽ������ת��

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    

    /// <summary>
    /// ��ز������㡣
    /// </summary>
    public class GeodeticUtils
    { 

        #region ������굽�ռ�֮������Ļ���
        public static double [] XyzToGeodeticCoord(IXYZ xyz, Geo.Referencing.IEllipsoid ellipsiod = null)
        {
            double lon, lat, height;
            if(ellipsiod == null)
            {
                ellipsiod = Geo.Referencing.Ellipsoid.CGCS2000;
            }
            XyzToGeodeticCoord(xyz.X, xyz.Y, xyz.Z,out lon, out lat, out height, ellipsiod.SemiMajorAxis, ellipsiod.InverseFlattening);
            return new double[] { lon, lat, height };
        }

        /// <summary>
        /// �Ӵ�����굽�ռ�ֱ�������ת��
        /// </summary>
        /// <param name="lat_deg">���γ��</param>
        /// <param name="lon_deg">��ؾ���</param>
        /// <param name="height">��ظ�</param>
        /// <param name="X">�ռ�ֱ������X����</param>
        /// <param name="Y">�ռ�ֱ������Y����</param>
        /// <param name="Z">�ռ�ֱ������Z����</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        public static void GeodeticToXyzCoord( double lon_deg, double lat_deg, double height, out double X, out double Y, out double Z, double a, double f)
        {
            double rL, rB;                         //��γ�ȵĻ���ֵ
            double N;                              //���߳�

            double ee = (2 * f - 1) / f / f;       //��һƫ���ʵ�ƽ��

            rL = lon_deg * CoordConsts.PI / 180;
            rB = lat_deg * CoordConsts.PI / 180;

            N = a / Math.Sqrt(1 - ee * Math.Sin(rB) * Math.Sin(rB));

            X = (N + height) * Math.Cos(rB) * Math.Cos(rL);
            Y = (N + height) * Math.Cos(rB) * Math.Sin(rL);
            Z = (N * (1 - ee) + height) * Math.Sin(rB);
        }

        /// <summary>
        /// �ӿռ�ֱ�����굽��������ת��
        /// </summary>
        /// <param name="X">�ռ�ֱ������X����</param>
        /// <param name="Y">�ռ�ֱ������Y����</param>
        /// <param name="Z">�ռ�ֱ������Z����</param>
        /// <param name="lat_deg">���γ��</param>
        /// <param name="lon_deg">��ؾ���</param>
        /// <param name="height">��ظ�</param>
        /// <param name="majorRadius">�ο����򳤰���</param>
        /// <param name="inverseFlat">�ο�������ʵ���</param>
        public static void XyzToGeodeticCoord(double X, double Y, double Z, out double lon_deg, out double lat_deg,  out double height, double majorRadius, double inverseFlat)
        {
            double ee = (2 * inverseFlat - 1) / inverseFlat / inverseFlat;       //��һƫ���ʵ�ƽ��

            double rL, rB;                         //��γ�ȵĻ���ֵ

            //��⾭��
            rL = Math.Atan(Y / X);            
            //�˴������Ƚ��������-180��180֮��
            if (rL < 0)
                rL += CoordConsts.PI * (Y > 0 ? 1 : 0);
            else
                rL -= CoordConsts.PI * (Y > 0 ? 0 : 1);
            
            lat_deg = 91;
            rB = Math.Atan(Z / Math.Sqrt(X * X + Y * Y));

            while (Math.Abs(rB - lat_deg) > 0.00000000001)
            {
                lat_deg = rB;
                rB = majorRadius * ee * Math.Tan(lat_deg) / Math.Sqrt(1 + (1 - ee) * Math.Pow(Math.Tan(lat_deg), 2));
                rB = Math.Atan((Z + rB) / Math.Sqrt(X * X + Y * Y));
            }

            lon_deg = rL * 180 / CoordConsts.PI;
            lat_deg = rB * 180 / CoordConsts.PI;

            height = Math.Sqrt(X * X + Y * Y) / Math.Cos(rB) - majorRadius / Math.Sqrt(1 - ee * Math.Pow(Math.Sin(rB), 2));
        }
        #endregion

        #region ��˹ͶӰ������

        /// <summary>
        /// �Ӵ�����굽ƽ������ĸ�˹����
        /// </summary>
        /// <param name="lat_deg">���γ��</param>
        /// <param name="lon_deg">��ؾ���</param>
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="majorRadius">�ο����򳤰���</param>
        /// <param name="inverseFlat">�ο�������ʵ���</param>
        /// <param name="beltWidth">ͶӰ�ִ��Ĵ���</param>
        /// <param name="orignalLon">����������</param>
        /// <param name="YConst">����Y�ӳ���</param>
        /// <param name="aveGeoHeight">�Ϸ��Ϳ�ɵ�����������򷨣���ظ�</param>
        /// <param name="IsIndicateOriginLon">�Ƿ�ָ��</param>
        /// <param name="isWithBeltNum">�Ƿ�ʹ�üٶ�����</param>
        public static void LonLatToGaussXy(double lon_deg, double lat_deg, double aveGeoHeight,
            out double x, out double y, ref double orignalLon, bool IsIndicateOriginLon,
            int beltWidth = 6, double YConst = 500000, bool isWithBeltNum = true,
            double majorRadius = Referencing.Ellipsoid.SemiMajorAxisOfCGCS2000, double inverseFlat = Referencing.Ellipsoid.InverseFlatOfCGCS2000
            )
        { 
            //�Ϸ��Ϳ�ɵ������������
            majorRadius += aveGeoHeight;

            //ͶӰ�ִ��Ĵ���
            if (!IsIndicateOriginLon)
            {
                orignalLon = GetOrigalLonDegFromAnyLon(lon_deg, beltWidth);
            }
            var beltNum = GetBeltNum(lon_deg, beltWidth);

            double differLonGeg = lon_deg - orignalLon;

            LonLatToGaussXy(differLonGeg, lat_deg, out x, out y, majorRadius, inverseFlat);

            //����ɼٶ����꣬ƽ��500km��ǰ��Ӵ���
            y += YConst;
            if (isWithBeltNum) { y += beltNum * 1000000; }
        }


        /// <summary>
        /// ��˹���굽��γ��,��λ����С��
        /// </summary>
        /// <param name="xy"></param> 
        /// <param name="YConst ">Y����</param>
        /// <param name="majorRadius">�ο����򳤰���</param>
        /// <param name="inversFlat">�ο�������ʵ���</param> 
        /// <param name="aveGeoHeight">ͶӰ���ظ�</param> 
        /// <param name="originLon_deg">���������ߣ���С��</param>
        /// <returns></returns>
        public static LonLat GaussXyToLonLat(XY xy, double aveGeoHeight, double originLon_deg, double YConst = 500000,
            double majorRadius = Ellipsoid.SemiMajorAxisOfCGCS2000, double inversFlat = Ellipsoid.InverseFlatOfCGCS2000)
        {
            double lon_deg, lat_deg;
            GaussXyToLonLat(xy.X, xy.Y, aveGeoHeight, out lon_deg, out lat_deg, originLon_deg, YConst, majorRadius, inversFlat );
            return new LonLat(lon_deg, lat_deg);
        }
        /// <summary>
        /// ƽ�����꣨��Ȼ�����ٶ����꣩���������ĸ�˹���㣬�Զ��ж��Ƿ���зִ��š�
        /// </summary>
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="lat_deg">���γ��</param>
        /// <param name="lon_deg">��ؾ���</param>
        /// <param name="YConst ">Y����</param>
        /// <param name="majorRadius">�ο����򳤰���</param>
        /// <param name="aveGeoHeight">ͶӰ���ظ�</param> 
        /// <param name="inversFlat">�ο�������ʵ���</param> 
        /// <param name="originLon_deg">���������ߣ���С��</param>
        public static void GaussXyToLonLat(double x, double y, double aveGeoHeight, out double lon_deg,out double lat_deg, double originLon_deg, 
            double YConst=500000, double majorRadius=Ellipsoid.SemiMajorAxisOfCGCS2000, double inversFlat = Ellipsoid.InverseFlatOfCGCS2000)
        {
            double beltNum = 0;
            //Ĭ�ϲ�����������ģ��
            majorRadius += aveGeoHeight;

            if (y > 1000000)//������ţ����Ϊ�ٶ����꣬ת��Ϊ��Ȼ����
            {
                beltNum = (int)(y % 1000000);
                y = 1000000 * beltNum;//����Yֵ
            }
            //��ȥ�������ָ�ԭʼ��ֵ
            y -= YConst;

            //���γ���뾭��
            GaussXyToDifferLonLat(x, y, out lon_deg, out lat_deg,  majorRadius, inversFlat);

            //��⾭��
            lon_deg += originLon_deg; 
        }

        #region �����㷨
        /// <summary>
        /// ���㾭�Ȳ�
        /// </summary>
        /// <param name="lon_deg"></param>
        /// <param name="beltNum"></param>
        /// <param name="beltWidth"></param>
        /// <returns></returns>
        public static double GetDifferLonDeg(double lon_deg, int beltNum, int beltWidth)
        {
            return lon_deg - beltNum * beltWidth - (beltWidth == 6 ? 3 : 0);
        }
        /// <summary>
        /// �����˹ͶӰ�ִ���
        /// </summary>
        /// <param name="lonDeg"></param>
        /// <param name="beltWidth">3, 6</param>
        /// <returns></returns>
        static public int GetBeltNum(double lonDeg, int beltWidth)
        {
            if (beltWidth == 6)
            {
                return (int)((lonDeg + 6) / 6);
            }
            if (beltWidth == 3)
            {
                return (int)((lonDeg + 1.3) / 3);
            }
            throw new Exception("��ֻ֧�� 3 / 6 �ֶȴ� " + beltWidth);
        }
        /// <summary>
        /// ֱ�Ӽ�������������
        /// </summary>
        /// <param name="lonDeg"></param>
        /// <param name="beltWidth"></param>
        /// <returns></returns>
        static public double GetOrigalLonDegFromAnyLon(double lonDeg, int beltWidth)
        {
            var beltNum = GetBeltNum(lonDeg, beltWidth);
            return GetOrigalLonDeg(beltNum, beltWidth);
        }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="beltNum"></param>
        /// <param name="beltWidth"></param>
        /// <returns></returns>
        public static double GetOrigalLonDeg(int beltNum, int beltWidth)
        {
            if (beltWidth == 6)
            {
                return  (beltNum * 6) - 3;
            }
            if (beltWidth == 3)
            {
                return beltNum * 3;
            }
            throw new Exception("��ֻ֧�� 3 / 6 �ֶȴ� " + beltWidth); 
        } 

        /// <summary>
        /// �Ӵ�����굽ƽ������ĸ�˹����
        /// 
        /// ָ�����������ߣ����ڽ����ڴ����㣬��ʱ�ز�ʹ�üٶ�����
        /// </summary>
        /// <param name="lat_deg">���γ��</param>
        /// <param name="differLonDeg">��ؾ���</param> 
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="majorRadius">�ο����򳤰���,��������ֱ���޸�ֵ</param>
        /// <param name="inverseFlat">�ο�������ʵ���</param>
        public static void LonLatToGaussXy(double differLonDeg, double lat_deg, out double x, out double y, double majorRadius, double inverseFlat)
        {
            double ee = (2 * inverseFlat - 1) / inverseFlat / inverseFlat;       //��һƫ���ʵ�ƽ��
            double ee2 = ee / (1 - ee);            //�ڶ�ƫ���ʵ�ƽ��

            double rB, tB, m;
            rB = lat_deg * CoordConsts.PI / 180;
            tB = Math.Tan(rB);
            m = Math.Cos(rB) * differLonDeg * CoordConsts.PI / 180;

            double N = majorRadius / Math.Sqrt(1 - ee * Math.Sin(rB) * Math.Sin(rB));
            double it2 = ee2 * Math.Pow(Math.Cos(rB), 2);

            x = m * m / 2 + (5 - tB * tB + 9 * it2 + 4 * it2 * it2) * Math.Pow(m, 4) / 24 + (61 - 58 * tB * tB + Math.Pow(tB, 4)) * Math.Pow(m, 6) / 720;
            x = MeridianLength(lat_deg, majorRadius, inverseFlat) + N * tB * x;
            y = N * (m + (1 - tB * tB + it2) * Math.Pow(m, 3) / 6 + (5 - 18 * tB * tB + Math.Pow(tB, 4) + 14 * it2 - 58 * tB * tB * it2) * Math.Pow(m, 5) / 120);
        }
        /// <summary>
        /// ƽ�����꣨��Ȼ���꣩���������ƫ����������ߣ��ĸ�˹����
        /// </summary>
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="lat_deg">���γ��</param>
        /// <param name="lon_deg">���Ȳ�</param>
        /// <param name="majorRadius">�ο����򳤰���</param>
        /// <param name="inverseFlat">�ο�������ʵ���</param>
        private static void GaussXyToDifferLonLat(double x, double y, out double lon_deg,  out double lat_deg,
            double majorRadius= Ellipsoid.SemiMajorAxisOfCGCS2000, double inverseFlat= Ellipsoid.InverseFlatOfCGCS2000)
        {
            if (y > 1000000)
            {
                throw new Exception("�������ʹ���Ӧʹ����Ȼ����");
            }

            double ee = (2 * inverseFlat - 1) / inverseFlat / inverseFlat;       //��һƫ���ʵ�ƽ��
            double ee2 = ee / (1 - ee);            //�ڶ�ƫ���ʵ�ƽ��

            double cA, cB, cC, cD, cE;

            cA = 1 + 3 * ee / 4 + 45 * ee * ee / 64 + 175 * Math.Pow(ee, 3) / 256 + 11025 * Math.Pow(ee, 4) / 16384;
            cB = 3 * ee / 4 + 15 * ee * ee / 16 + 525 * Math.Pow(ee, 3) / 512 + 2205 * Math.Pow(ee, 4) / 2048;
            cC = 15 * ee * ee / 64 + 105 * Math.Pow(ee, 3) / 256 + 2205 * Math.Pow(ee, 4) / 4096;
            cD = 35 * Math.Pow(ee, 3) / 512 + 315 * Math.Pow(ee, 4) / 2048;
            cE = 315 * Math.Pow(ee, 4) / 131072;

            double Bf = x / (majorRadius * (1 - ee) * cA);

            do
            {
                lat_deg = Bf;
                Bf = (x + majorRadius * (1 - ee) * (cB * Math.Sin(2 * Bf) / 2 - cC * Math.Sin(4 * Bf) / 4 + cD * Math.Sin(6 * Bf) / 6) - cE * Math.Sin(8 * Bf) / 8) / (majorRadius * (1 - ee) * cA);
            }
            while (Math.Abs(lat_deg - Bf) > 0.00000000001);

            double N = majorRadius / Math.Sqrt(1 - ee * Math.Pow(Math.Sin(Bf), 2));
            double V2 = 1 + ee2 * Math.Pow(Math.Cos(Bf), 2);
            double it2 = ee2 * Math.Pow(Math.Cos(Bf), 2);
            double tB2 = Math.Pow(Math.Tan(Bf), 2);

            lat_deg = Bf - V2 * Math.Tan(Bf) / 2 * (Math.Pow(y / N, 2) - (5 + 3 * tB2 + it2 - 9 * it2 * tB2) * Math.Pow(y / N, 4) / 12 + (61 + 90 * tB2 + 45 * tB2 * tB2) * Math.Pow(y / N, 6) / 360);
            lon_deg = (y / N - (1 + 2 * tB2 + it2) * Math.Pow(y / N, 3) / 6 + (5 + 28 * tB2 + 24 * tB2 * tB2 + 6 * it2 + 8 * it2 * tB2) * Math.Pow(y / N, 5) / 120) / Math.Cos(Bf);

            lat_deg = lat_deg * 180 / CoordConsts.PI;
            lon_deg = lon_deg * 180 / CoordConsts.PI;
        }
        #endregion
          
        #endregion

        #region ����ת��
         



        /// <summary>
        /// ����ɯ�߲���ģ������ת�����ӿռ�ֱ�����굽�ռ�ֱ������
        /// </summary>
        /// <param name="old"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        ///����ת��ʱ����תXYZ��Ȼ���ٽ�XYZת����BLH��xy
        /// WGS-84 --> BJZ54
        ///-15.415, 157.025, 94.74,   0.312, 0.08,  0.102, -1.465e-6        /*ȫ��*/
        ///-14.756, 145.798, 100.886, 0.618, 0.255, 0.302, -0.439e-6        /*���Ͼֲ�*/
        ///  BJZ54 --> DXZ88
        ///16.5,    -152.9,  -91.8,   -0.226, -0.003, 0.0, 1.22e-6
        public static IXYZ BursaTransform(IXYZ old, IBursaTransParams p)
        {
            //double[] result = info.Transform(new double[] { old.X, old.Y, old.Z });
           // return XYZ.Parse(result);

            double X, Y, Z; 
            double scale = (1 + p.Scale_m);
            X = p.Dx + old.X * scale + (old.Y * p.Ez - old.Z * p.Ey) * SEC_TO_RAD;
            Y = p.Dy + old.Y * scale + (old.Z * p.Ex - old.X * p.Ez) * SEC_TO_RAD;
            Z = p.Dz + old.Z * scale + (old.X * p.Ey - old.Y * p.Ex) * SEC_TO_RAD;

            return new Xyz(X, Y, Z);
        }
        private const double SEC_TO_RAD = 4.84813681109535993589914102357e-6;       
        /// <summary>
        /// ���ò���ɯ����ת��ģ�����ת������
        /// </summary>
        /// <param name="oldPoint">������ϵ�µ�����꼯��</param>
        /// <param name="newPoint">������ϵ�µ�����꼯��</param>
        /// <param name="weight">Ȩ��</param>
        /// <param name="paraCount">ת����������������Ϊ3��4��7��</param>
        /// <returns></returns>
        public static BursaTransParams BursaParamsSolve(
                                                            Xyz[] oldPoint,
                                                            Xyz[] newPoint,
                                                            double[] weight = null,
                                                            int paraCount = 7)
        {
            if (newPoint.Length != oldPoint.Length) throw new ArgumentException("�¾���������Ӧ��һ�£�");

            int pntCount = newPoint.Length;

            if (weight == null)
            {
                weight = new double[pntCount * 3];
                for (int i = 0; i < pntCount * 3; i++)
                {
                    weight[i] = 1;
                }
            }

            #region ������
            //360 For I = 0 To NN2
            //361 For J = 0 To N
            //365 A#(I, J) = 0#
            //366 Next J: Next I
            //370 For I = 0 To NN2 Step 3
            //375 J = I + 1: K = I + 2
            //376 Q = Int(I / 3)
            //378 A#(I, 0) = 1#: A#(J, 1) = 1#: A#(K, 2) = 1#
            //379 If NA = 3 Then GoTo 385
            //380 If NA = 4 Then GoTo 384
            //381 A#(J, 3) = ZH#(2, Q) * 0.000004848: A#(K, 3) = -ZH#(1, Q) * 0.000004848
            //382 A#(I, 4) = -A#(J, 3): A#(K, 4) = ZH#(0, Q) * 0.000004848
            //383 A#(I, 5) = -A#(K, 3): A#(J, 5) = -A#(K, 4)
            //384 A#(I, N) = ZH#(0, Q) * 0.000001: A#(J, N) = ZH#(1, Q) * 0.000001: A#(K, N) = ZH#(2, Q) * 0.000001
            //385 Next I
            double[,] A = new double[pntCount * 3, paraCount];
            for (int i = 0; i < pntCount; i++)
            {
                A[i * 3 + 0, 0] = 1;
                A[i * 3 + 1, 1] = 1;
                A[i * 3 + 2, 2] = 1;

                if (paraCount == 7)
                {
                    double rou = 3600 * 180 * 1 / CoordConsts.PI;
                    A[i * 3 + 1, 3] = oldPoint[i].Z / rou;
                    A[i * 3 + 2, 3] = -oldPoint[i].Y / rou;
                    A[i * 3 + 2, 4] = oldPoint[i].X / rou;

                    A[i * 3 + 0, 4] = -A[i * 3 + 1, 3];
                    A[i * 3 + 0, 5] = -A[i * 3 + 2, 3];
                    A[i * 3 + 1, 5] = -A[i * 3 + 2, 4];
                }

                if (paraCount == 7 || paraCount == 4)
                {
                    A[i * 3 + 0, paraCount - 1] = oldPoint[i].X * 0.000001;
                    A[i * 3 + 1, paraCount - 1] = oldPoint[i].Y * 0.000001;
                    A[i * 3 + 2, paraCount - 1] = oldPoint[i].Z * 0.000001;
                }
            }

            //395 For I = 1 To NN2 + 1
            //400 J = Int((I - 1) / 3)
            //405 L#(I - 1) = ZH#(I - 3 * J - 1, J) - ZH#(I - 3 * J - 1, J + NN)
            //410 Next I
            double[] L = new double[pntCount * 3];
            for (int i = 0; i < pntCount; i++)
            {
                L[i * 3 + 0] = oldPoint[i].X - newPoint[i].X;
                L[i * 3 + 1] = oldPoint[i].Y - newPoint[i].Y;
                L[i * 3 + 2] = oldPoint[i].Z - newPoint[i].Z;
            }
            #endregion

            #region ������
            //415 For I = 0 To N
            //420 For K = 0 To N
            //425 S1# = 0#
            //430 For J = 0 To NN2
            //431 AW# = A#(J, K)
            //435 S1# = A#(J, I) * AW# * PZ#(J + 1) + S1#
            //440 Next J
            //445 B#(I, K) = S1#
            //450 Next K
            //455 S2# = 0#
            //460 For J = 0 To NN2
            //464 K = Int(J / 3)
            //465 S2# = -A#(J, I) * L#(J) * PZ#(J + 1) + S2#
            //470 Next J
            //475 B#(I, N + 1) = S2#
            //480 Next I
            double[,] B = new double[paraCount, paraCount + 1];
            for (int i = 0; i < paraCount; i++)
            {
                for (int k = 0; k < paraCount; k++)
                {
                    B[i, k] = 0;
                    for (int j = 0; j < pntCount * 3; j++)
                    {
                        B[i, k] += A[j, i] * A[j, k] * weight[j];
                    }
                }

                B[i, paraCount] = 0;
                for (int j = 0; j < pntCount * 3; j++)
                {
                    B[i, paraCount] -= A[j, i] * L[j] * weight[j];
                }
            }
            #endregion

            #region �ⷽ��
            double[,] QQ = new double[paraCount, paraCount];
            //800 For I = 0 To N
            //801 For J = 0 To N
            //802 QQ#(I, J) = 0#: Next J
            //803 QQ#(I, I) = 1#: Next I
            for (int i = 0; i < paraCount; i++)
            {
                QQ[i, i] = 1;
            }

            //805 For I = 0 To N
            //810 If I = N Then GoTo 920
            //820 AM# = Abs(B#(I, I))
            //830 IM = I
            //840 For J = I + 1 To N
            //850 AA# = Abs(B#(J, I))
            //860 If AM# < AA# Then AM# = AA#: IM = J
            //870 Next J
            //880 If I = IM Then GoTo 920
            //890 For J = 0 To N + 1
            //900 AA# = B#(I, J): B#(I, J) = B#(IM, J): B#(IM, J) = AA#
            //910 Next J
            //911 For J = 0 To N
            //912 AA# = QQ#(I, J): QQ#(I, J) = QQ#(IM, J): QQ#(IM, J) = AA#
            //913 Next J
            //920 P# = B#(I, I)
            //930 For J = I + 1 To N + 1
            //940 B#(I, J) = B#(I, J) / P#
            //950 Next J
            //951 For J = 0 To N
            //952 QQ#(I, J) = QQ#(I, J) / P#
            //953 Next J
            //960 For J = 0 To N
            //970 If B#(J, I) = 0 Or J = I Then GoTo 993
            //980 P# = B#(J, I)
            //981 For K = I + 1 To N + 1
            //982 B#(J, K) = B#(J, K) - P# * B#(I, K)
            //983 Next K
            //984 For K = 0 To N
            //985 QQ#(J, K) = QQ#(J, K) - P# * QQ#(I, K)
            //992 Next K
            //993 Next J
            //994 Next I
            for (int i = 0; i < paraCount; i++)
            {
                if (i != paraCount - 1)
                {
                    double max = Math.Abs(B[i, i]);
                    int mark = i;
                    for (int j = i + 1; j < paraCount; j++)
                    {
                        if (max < Math.Abs(B[j, i]))
                        {
                            max = Math.Abs(B[j, i]);
                            mark = j;
                        }
                    }

                    if (i != mark)
                    {
                        for (int j = 0; j <= paraCount; j++)
                        {
                            double temp = B[i, j];
                            B[i, j] = B[mark, j];
                            B[mark, j] = temp;
                        }

                        for (int j = 0; j < paraCount; j++)
                        {
                            double temp = QQ[i, j];
                            QQ[i, j] = QQ[mark, j];
                            QQ[mark, j] = temp;
                        }
                    }
                }

                for (int j = i + 1; j <= paraCount; j++)
                {
                    B[i, j] /= B[i, i];
                }

                for (int j = 0; j < paraCount; j++)
                {
                    QQ[i, j] /= B[i, i];
                }

                for (int j = 0; j < paraCount; j++)
                {
                    if (B[i, j] != 0 && i != j)
                    {
                        for (int k = i + 1; k <= paraCount; k++)
                        {
                            B[j, k] -= B[j, i] * B[i, k];
                        }

                        for (int k = 0; k < paraCount; k++)
                        {
                            QQ[j, k] -= B[j, i] * QQ[i, k];
                        }
                    }
                }
            }

            //486 M# = 0#
            //490 For I = 0 To NN2
            //495 For J = 0 To N
            //500 L#(I) = A#(I, J) * B#(J, NA) + L#(I)
            //505 Next J
            //510 M# = L#(I) * L#(I) * PZ#(I + 1) + M#
            //515 Next I
            double M = 0;
            for (int i = 0; i < pntCount * 3; i++)
            {
                for (int j = 0; j < paraCount; j++)
                {
                    L[i] += A[i, j] * B[j, paraCount];
                }
                M += L[i] * L[i] * weight[i];
            }

            //516 M# = Sqr(M# / (NN2 + 1 - NA))
            //517 For I = 0 To N
            //518 For J = 0 To N
            //519 QQ#(I, J) = QQ#(I, J) * M# * M#
            //520 Next J: Next I
            for (int i = 0; i < paraCount; i++)
                for (int j = 0; j < paraCount; j++)
                    QQ[i, j] *= M / (pntCount * 3 - paraCount);
            #endregion

            switch (paraCount)
            {
                case 3:
                    return new BursaTransParams(B[0, paraCount], B[1, paraCount], B[2, paraCount]);

                case 4:
                    return new BursaTransParams(B[0, paraCount], B[1, paraCount], B[2, paraCount], B[3, paraCount]);

                case 7:
                    return new BursaTransParams(B[0, paraCount], B[1, paraCount], B[2, paraCount], B[3, paraCount],
                                                 B[4, paraCount], B[5, paraCount], B[6, paraCount]);

                default:
                    return null;
            }
        }
        #endregion

        #region ����������
        /// <summary>
        /// �����������������
        /// </summary>
        /// <param name="latA_deg">��֪��γ��</param>
        /// <param name="lonA_deg">��֪�㾭��</param>
        /// <param name="geodeticLine">����߳�</param>
        /// <param name="azimuth_deg">��ط�λ��</param>
        /// <param name="latB_deg">�����γ��</param>
        /// <param name="lonB_deg">����㾭��</param>
        /// <param name="antAzimuth_deg">��ط���λ��</param>
        /// <param name="majorRadius">�ο����򳤰���</param>
        /// <param name="inverseFlat">�ο�������ʵ���</param>
        public static void BesselPoint( 
            double lonA_deg,
            double latA_deg,
            double azimuth_deg,
            double geodeticLine,
            double majorRadius, 
            double inverseFlat,
            out double lonB_deg,
            out double latB_deg,
            out double antAzimuth_deg)
        {
            double ee = (2 * inverseFlat - 1) / inverseFlat / inverseFlat;                                           //��һƫ���ʵ�ƽ��
            double ee2 = (2 * inverseFlat - 1) / (inverseFlat - 1) / (inverseFlat - 1);                              //�ڶ�ƫ���ʵ�ƽ��

            latA_deg = latA_deg * CoordConsts.PI / 180;
            lonA_deg = lonA_deg * CoordConsts.PI / 180;
            azimuth_deg = azimuth_deg * CoordConsts.PI / 180;

            double u1 = Math.Atan(Math.Sqrt(1 - ee) * Math.Tan(latA_deg));                     //u1 is (-90, 90)
            double m = Math.Cos(u1) * Math.Sin(azimuth_deg);                                      //sin(m)
            m = Math.Atan(m / Math.Sqrt(1 - m * m));                                     //m is (-90, 90)
            if (m < 0) m += 2 * CoordConsts.PI;

            double M = Math.Atan(Math.Tan(u1) / Math.Cos(azimuth_deg));                          //M is (-90, 90)
            if (M < 0) M += CoordConsts.PI;                                                    //change M to (0, 180)

            //compute cofficients
            double KK = ee2 * Math.Pow(Math.Cos(m), 2);                                   
            double alpha = Math.Sqrt(1 + ee2) * (1 - KK / 4 + 7 * KK * KK / 64 - 15 * Math.Pow(KK, 3) / 256) / majorRadius;
            double beta = KK / 4 - KK * KK / 8 + 37 * Math.Pow(KK, 3) / 512;
            double gama = KK * KK * (1 - KK) / 128;

            //loop for compute sigma
            double sigma, temp;
            sigma = alpha * geodeticLine;
            temp = 0;
            while (Math.Abs(temp - sigma) > 0.00001 / 206265)                         //����0.00001��
            {
                temp = sigma;
                sigma = alpha * geodeticLine + beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) + gama * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma);
            }

            antAzimuth_deg = Math.Atan(Math.Tan(m) / Math.Cos(M + sigma));                         //A2 is (-90, 90)
            if (antAzimuth_deg < 0) antAzimuth_deg += CoordConsts.PI;
            if (azimuth_deg < CoordConsts.PI) antAzimuth_deg += CoordConsts.PI;

            double u2 = Math.Atan(-Math.Cos(antAzimuth_deg) * Math.Tan(M + sigma));                //u2 is (-90, 90)

            double lamda1;
            lamda1 = Math.Atan(Math.Sin(u1) * Math.Tan(azimuth_deg));                           //lamda1
            if (lamda1 < 0) lamda1 += CoordConsts.PI;
            if (m > CoordConsts.PI) lamda1 += CoordConsts.PI;

            double lamda2;                                                             //lamda2
            lamda2 = Math.Atan(Math.Sin(u2) * Math.Tan(antAzimuth_deg));
            if (lamda2 < 0) lamda2 += CoordConsts.PI;
            if (m > CoordConsts.PI)
            {
                if (M + sigma < CoordConsts.PI) lamda2 += CoordConsts.PI;
            }
            else
            {
                if (M + sigma > CoordConsts.PI) lamda2 += CoordConsts.PI;
            }

            KK = ee * Math.Pow(Math.Cos(m), 2);
            alpha = ee / 2 + ee * ee / 8 + Math.Pow(ee, 3) / 16 - ee * (1 + ee) * KK / 16 + 3 * ee * KK * KK / 128;
            beta = ee * (1 + ee) * KK / 16 - ee * KK * KK / 32;
            gama = ee * KK * KK / 256;

            lonB_deg = lonA_deg + lamda2 - lamda1 - Math.Sin(m) * (alpha * sigma + beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) + gama * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma));
            if (lonB_deg < 0) lonB_deg += 2 * CoordConsts.PI;

            latB_deg = Math.Atan(Math.Sqrt(1 + ee2) * Math.Tan(u2)) * 180 / CoordConsts.PI;          //B in (-90, 90)
            lonB_deg = lonB_deg * 180 / CoordConsts.PI;
            antAzimuth_deg = antAzimuth_deg * 180 / CoordConsts.PI;
        }
        /// <summary>
        /// ���㱴������ط�λ��
        /// </summary>
        /// <param name="startXyz"></param>
        /// <param name="toXyz"></param>
        /// <returns></returns>
        public static double BesselAzimuthAngle(XYZ startXyz, XYZ toXyz, double majorRadius = Ellipsoid.SemiMajorAxisOfCGCS2000, double inverseFlat = Ellipsoid.InverseFlatOfCGCS2000)
        {
            var geoCoordA = XyzToGeodeticCoord(startXyz);
            var geoCoordB = XyzToGeodeticCoord(toXyz);

            return BesselAzimuthAngle(geoCoordA[0], geoCoordA[1], geoCoordB[0], geoCoordB[1], majorRadius, inverseFlat);
        }
        /// <summary>
        /// ���㱴������ط�λ��
        /// </summary>
        /// <param name="startXyz"></param>
        /// <param name="toXyz"></param>
        /// <returns></returns>
        public static double BesselAzimuthAngle(Xyz startXyz, Xyz toXyz, double majorRadius = Ellipsoid.SemiMajorAxisOfCGCS2000, double inverseFlat = Ellipsoid.InverseFlatOfCGCS2000)
        {
            var geoCoordA = XyzToGeodeticCoord(startXyz);
            var geoCoordB = XyzToGeodeticCoord(toXyz);

            return BesselAzimuthAngle(geoCoordA[0], geoCoordA[1], geoCoordB[0], geoCoordB[1], majorRadius, inverseFlat);
        }
        /// <summary>
        /// ���㱴������ط�λ��
        /// </summary>
        /// <param name="startXyz"></param>
        /// <param name="toXyz"></param>
        /// <returns></returns>
        public static double BesselAzimuthAngle(LonLat startXyz, LonLat toXyz, double majorRadius = Ellipsoid.SemiMajorAxisOfCGCS2000, double inverseFlat = Ellipsoid.InverseFlatOfCGCS2000)
        {  
            return BesselAzimuthAngle(startXyz.Lon, startXyz.Lat, toXyz.Lon, toXyz.Lat, majorRadius, inverseFlat);
        }

        /// <summary>
        /// ����������������ط�λ��
        /// </summary>
        /// <param name="latA_deg">���γ��</param>
        /// <param name="lonA_deg">��㾭��</param>
        /// <param name="latB_deg">ĩ��γ��</param>
        /// <param name="lonB_deg">ĩ�㾭��</param>
        /// <param name="majorRadius">�ο����򳤰���</param>
        /// <param name="inverseFlat">�ο�������ʵ���</param>
        /// <returns>�����Ĵ�ط�λ��</returns>
        public static double BesselAzimuthAngle(double lonA_deg, double latA_deg,  double lonB_deg,double latB_deg, double majorRadius = Ellipsoid.SemiMajorAxisOfCGCS2000, double inverseFlat = Ellipsoid.InverseFlatOfCGCS2000)
        {
            if (Math.Abs(lonA_deg - lonB_deg) < 0.00001 / 206265) return CoordConsts.PI;                  //�������0.00001��

            double ee = (2 * inverseFlat - 1) / inverseFlat / inverseFlat;                                           //��һƫ���ʵ�ƽ��

            double u1 = Math.Atan(Math.Sqrt(1 - ee) * Math.Tan(latA_deg * CoordConsts.PI / 180));
            double u2 = Math.Atan(Math.Sqrt(1 - ee) * Math.Tan(latB_deg * CoordConsts.PI / 180));
            double dL = (lonB_deg - lonA_deg) * CoordConsts.PI / 180;

            double sigma = Math.Sin(u1) * Math.Sin(u2) + Math.Cos(u1) * Math.Cos(u2) * Math.Cos(dL);     //Cos(sigma)
            sigma = Math.Atan(Math.Sqrt(1 - sigma * sigma) / sigma);
            if (sigma < 0) sigma += CoordConsts.PI;

            double m = Math.Cos(u1) * Math.Cos(u2) * Math.Sin(dL) / Math.Sin(sigma);                     //Sin(m)
            m = Math.Atan(m / Math.Sqrt(1 - m * m));

            double KK, alpha, beta, gama;
            double temp = dL;
            double lamda = dL + 0.003351 * sigma * Math.Sin(m);
            double M = CoordConsts.PI / 2;

            while (Math.Abs(lamda - temp) > 0.00001 / 206265)                                           //����0.00001��
            {
                temp = lamda;

                KK = ee * Math.Pow(Math.Cos(m), 2);
                alpha = ee / 2 + ee * ee / 8 + Math.Pow(ee, 3) / 16 - ee * (1 + ee) * KK / 16 + 3 * ee * KK * KK / 128;
                beta = ee * (1 + ee) * KK / 16 - ee * KK * KK / 32;
                gama = ee * KK * KK / 256;

                lamda = dL + Math.Sin(m) * (alpha * sigma + beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) + gama * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma));

                sigma = Math.Sin(u1) * Math.Sin(u2) + Math.Cos(u1) * Math.Cos(u2) * Math.Cos(lamda);     //Cos(sigma)
                sigma = Math.Atan(Math.Sqrt(1 - sigma * sigma) / sigma);
                if (sigma < 0) sigma += CoordConsts.PI;

                m = Math.Cos(u1) * Math.Cos(u2) * Math.Sin(lamda) / Math.Sin(sigma);                     //Sin(m)
                m = Math.Atan(m / Math.Sqrt(1 - m * m));

                double tanA = Math.Sin(lamda) / (Math.Cos(u1) * Math.Tan(u2) - Math.Sin(u1) * Math.Cos(lamda));
                
                M = Math.Atan(Math.Sin(u1) * tanA / Math.Sin(m));
                if (M < 0) M += CoordConsts.PI;
            }
            
            double A = Math.Atan(Math.Sin(lamda) / (Math.Cos(u1) * Math.Tan(u2) - Math.Sin(u1) * Math.Cos(lamda)));
            if (A < 0) A += CoordConsts.PI;
            if (m < 0) A += CoordConsts.PI;

            return  A * 180 / CoordConsts.PI;
        }

        /// <summary>
        /// �ɴ������������߳�,��������ƽ����ظ߼������߳���
        /// </summary>
        /// <param name="longlatADeg"></param>
        /// <param name="longlatBDeg"></param>
        /// <param name="majorRadius"></param>
        /// <param name="inverseFlat"></param>
        /// <returns></returns>
        public static double BesselGeodeticLine(GeoCoord longlatADeg, GeoCoord longlatBDeg, double majorRadius = Ellipsoid.SemiMajorAxisOfCGCS2000, double inverseFlat = Ellipsoid.InverseFlatOfCGCS2000)
        {
            double aveGeoHeight = (longlatBDeg.Height + longlatADeg.Height) / 2.0;
            return BesselGeodeticLine(longlatADeg.Lon, longlatADeg.Lat, longlatBDeg.Lon, longlatBDeg.Lat, majorRadius + aveGeoHeight, inverseFlat);
        }


        /// <summary>
        /// �ɴ������������߳�
        /// </summary>
        /// <param name="longlatADeg"></param>
        /// <param name="longlatBDeg"></param>
        /// <param name="aveGeoHeight">ƽ����ظ�</param>
        /// <param name="majorRadius"></param>
        /// <param name="inverseFlat"></param>
        /// <returns></returns>
        public static double BesselGeodeticLine(LonLat longlatADeg, LonLat longlatBDeg, double aveGeoHeight=0, double majorRadius = Ellipsoid.SemiMajorAxisOfCGCS2000, double inverseFlat = Ellipsoid.InverseFlatOfCGCS2000)
        {
            return BesselGeodeticLine(longlatADeg.Lon, longlatADeg.Lat, longlatBDeg.Lon, longlatBDeg.Lat, majorRadius + aveGeoHeight, inverseFlat);
        } 
        /// <summary>
        /// �ɴ������������߳�
        /// </summary>
        /// <param name="latA_deg">���γ��</param>
        /// <param name="lonA_deg">��㾭��</param>
        /// <param name="latB_deg">ĩ��γ��</param>
        /// <param name="lonB_deg">ĩ�㾭��</param>
        /// <param name="majorRadius">�ο����򳤰���</param>
        /// <param name="inverseFlat">�ο�������ʵ���</param>
        /// <returns>�����Ĵ���߳�</returns>
        public static double BesselGeodeticLine(double lonA_deg, double latA_deg,  double lonB_deg,  double latB_deg, double majorRadius = Ellipsoid.SemiMajorAxisOfCGCS2000, double inverseFlat = Ellipsoid.InverseFlatOfCGCS2000)
        {
            if (Math.Abs(lonA_deg - lonB_deg) < 0.00001 / 206265)                                  //�������0.00001��
            {
                return Math.Abs(MeridianLength(latA_deg, majorRadius, inverseFlat) - MeridianLength(latB_deg, majorRadius, inverseFlat));
            }

            double ee = (2 * inverseFlat - 1) / inverseFlat / inverseFlat;                                           //��һƫ���ʵ�ƽ��
            double ee2 = (2 * inverseFlat - 1) / (inverseFlat - 1) / (inverseFlat - 1);                              //�ڶ�ƫ���ʵ�ƽ��

            double u1 = Math.Atan(Math.Sqrt(1 - ee) * Math.Tan(latA_deg * CoordConsts.PI / 180));
            double u2 = Math.Atan(Math.Sqrt(1 - ee) * Math.Tan(latB_deg * CoordConsts.PI / 180));
            double dL = (lonB_deg - lonA_deg) * CoordConsts.PI / 180;

            double sigma = Math.Sin(u1) * Math.Sin(u2) + Math.Cos(u1) * Math.Cos(u2) * Math.Cos(dL);     //Cos(sigma)
            sigma = Math.Atan(Math.Sqrt(1 - sigma * sigma) / sigma);
            if (sigma < 0) sigma += CoordConsts.PI;

            double m = Math.Cos(u1) * Math.Cos(u2) * Math.Sin(dL) / Math.Sin(sigma);                     //Sin(m)
            m = Math.Atan(m / Math.Sqrt(1 - m * m));

            double KK, alpha, beta, gama;
            double temp = dL;
            double lamda = dL + 0.003351 * sigma * Math.Sin(m);
            double M = CoordConsts.PI / 2;

            while (Math.Abs(lamda - temp) > 0.00001 / 206265)                                           //����0.00001��
            {
                temp = lamda;

                KK = ee * Math.Pow(Math.Cos(m), 2);
                alpha = ee / 2 + ee * ee / 8 + Math.Pow(ee, 3) / 16 - ee * (1 + ee) * KK / 16 + 3 * ee * KK * KK / 128;
                beta = ee * (1 + ee) * KK / 16 - ee * KK * KK / 32;
                gama = ee * KK * KK / 256;

                lamda = dL + Math.Sin(m) * (alpha * sigma + beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) + gama * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma));

                sigma = Math.Sin(u1) * Math.Sin(u2) + Math.Cos(u1) * Math.Cos(u2) * Math.Cos(lamda);     //Cos(sigma)
                sigma = Math.Atan(Math.Sqrt(1 - sigma * sigma) / sigma);
                if (sigma < 0) sigma += CoordConsts.PI;

                m = Math.Cos(u1) * Math.Cos(u2) * Math.Sin(lamda) / Math.Sin(sigma);                     //Sin(m)
                m = Math.Atan(m / Math.Sqrt(1 - m * m));

                double tanA = Math.Sin(lamda) / (Math.Cos(u1) * Math.Tan(u2) - Math.Sin(u1) * Math.Cos(lamda));

                M = Math.Atan(Math.Sin(u1) * tanA / Math.Sin(m));
                if (M < 0) M += CoordConsts.PI;
            }

            KK = ee2 * Math.Pow(Math.Cos(m), 2);
            alpha = Math.Sqrt(1 + ee2) * (1 - KK / 4 + 7 * KK * KK / 64 - 15 * Math.Pow(KK, 3) / 256) / majorRadius;
            beta = KK / 4 - KK * KK / 8 + 37 * Math.Pow(KK, 3) / 512;
            gama = KK * KK * (1 - KK) / 128;

            return (sigma - beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) - gama * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma)) / alpha;
        }

        /// <summary>
        /// ��ط�λ������������Ŀռ�ֱ��������ⷽλ�ǡ�
        /// �����������д���֤���о����� 2019.01.07�� czs , hmx
        /// </summary>
        /// <param name="fromPoint">���ռ�ֱ������</param>
        /// <param name="toPoint">ĩ��ռ�ֱ������</param>
        /// <param name="majorRadius">�ο����򳤰���</param>
        /// <param name="inverseFlat">�ο�������ʵ���</param>
        /// <returns>�����Ĵ�ط�λ��</returns>
        public static double AzimuthAngle(Xyz fromPoint, Xyz toPoint, double majorRadius=Ellipsoid.SemiMajorAxisOfCGCS2000, double inverseFlat= Ellipsoid.InverseFlatOfCGCS2000)
        {
            double B, L, H;
            XyzToGeodeticCoord(fromPoint.X, fromPoint.Y, fromPoint.Z, out B, out L, out H, majorRadius, inverseFlat);
            B = B * CoordConsts.PI / 180;
            L = L * CoordConsts.PI / 180;

            double Dx, Dy, Dz;
            Dx = toPoint.X - fromPoint.X;
            Dy = toPoint.Y - fromPoint.Y;
            Dz = toPoint.Z - fromPoint.Z;

            double tempX, tempY;
            tempX = -Math.Sin(B) * Math.Cos(L) * Dx - Math.Sin(B) * Math.Sin(L) * Dy + Math.Cos(B) * Dz;
            tempY = -Math.Sin(L) * Dx + Math.Cos(L) * Dy;

            double A = Math.Atan(tempY / tempX);
            if (tempX > 0 && tempY < 0) A += 2 * CoordConsts.PI;
            if (tempX < 0) A += CoordConsts.PI;

            return A * 180 / CoordConsts.PI;
        }
        #endregion

        /// <summary>
        /// ��γ����������߻���
        /// </summary>
        /// <param name="lat_deg">γ��</param>
        /// <param name="majorRadius">������</param>
        /// <param name="inverseFlat">���ʵ���</param>
        /// <returns>�����߻���</returns>
        public static double MeridianLength(double lat_deg, double majorRadius, double inverseFlat)
        {
            double ee = (2 * inverseFlat - 1) / inverseFlat / inverseFlat;       //��һƫ���ʵ�ƽ��            
            double rB = lat_deg * CoordConsts.PI / 180;         //����ת��Ϊ����

            //�����߻�����ʽ��ϵ��
            double cA, cB, cC, cD, cE;
            cA = 1 + 3 * ee / 4 + 45 * Math.Pow(ee, 2) / 64 + 175 * Math.Pow(ee, 3) / 256 + 11025 * Math.Pow(ee, 4) / 16384;
            cB = 3 * ee / 4 + 15 * Math.Pow(ee, 2) / 16 + 525 * Math.Pow(ee, 3) / 512 + 2205 * Math.Pow(ee, 4) / 2048;
            cC = 15 * Math.Pow(ee, 2) / 64 + 105 * Math.Pow(ee, 3) / 256 + 2205 * Math.Pow(ee, 4) / 4096;
            cD = 35 * Math.Pow(ee, 3) / 512 + 315 * Math.Pow(ee, 4) / 2048;
            cE = 315 * Math.Pow(ee, 4) / 131072;

            //�����߻���
            return majorRadius * (1 - ee) * (cA * rB - cB * Math.Sin(2 * rB) / 2 + cC * Math.Sin(4 * rB) / 4 - cD * Math.Sin(6 * rB) / 6 + cE * Math.Sin(8 * rB) / 8);
        }
    }
}