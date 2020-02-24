using System;
using System.Collections.Generic;
using System.Text;
using Geo;

namespace GeodeticX
{
    /// <summary>
    /// ��װ��������ת����ʵ����
    /// </summary>
    public class Geodetic
    {

        #region ������굽�ռ�֮������Ļ���
        /// <summary>
        /// �Ӵ�����굽�ռ�ֱ�������ת��
        /// </summary>
        /// <param name="B">���γ��</param>
        /// <param name="L">��ؾ���</param>
        /// <param name="H">��ظ�</param>
        /// <param name="X">�ռ�ֱ������X����</param>
        /// <param name="Y">�ռ�ֱ������Y����</param>
        /// <param name="Z">�ռ�ֱ������Z����</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        public static void BLH_XYZ(double B, double L, double H, out double X, out double Y, out double Z, double a, double f)
        {
            double rL, rB;                         //��γ�ȵĻ���ֵ
            double N;                              //���߳�

            double ee = (2 * f - 1) / f / f;       //��һƫ���ʵ�ƽ��

            rL = L * CoordConsts.PI / 180;
            rB = B * CoordConsts.PI / 180;

            N = a / Math.Sqrt(1 - ee * Math.Sin(rB) * Math.Sin(rB));

            X = (N + H) * Math.Cos(rB) * Math.Cos(rL);
            Y = (N + H) * Math.Cos(rB) * Math.Sin(rL);
            Z = (N * (1 - ee) + H) * Math.Sin(rB);
        }

        /// <summary>
        /// �ӿռ�ֱ�����굽��������ת�������ؽǶȵ�λ Ϊ��С��
        /// </summary>
        /// <param name="X">�ռ�ֱ������X����</param>
        /// <param name="Y">�ռ�ֱ������Y����</param>
        /// <param name="Z">�ռ�ֱ������Z����</param>
        /// <param name="B">���γ��</param>
        /// <param name="L">��ؾ���</param>
        /// <param name="H">��ظ�</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        public static void XYZ_BLH(double X, double Y, double Z, out double B, out double L, out double H, double a, double f)
        {
            double ee = (2 * f - 1) / f / f;       //��һƫ���ʵ�ƽ��

            double rL, rB;                         //��γ�ȵĻ���ֵ

            //��⾭��
            rL = Math.Atan(Y / X);            
            //�˴������Ƚ��������-180��180֮��
            if (rL < 0)
                rL += CoordConsts.PI * (Y > 0 ? 1 : 0);
            else
                rL -= CoordConsts.PI * (Y > 0 ? 0 : 1);
            
            B = 91;
            rB = Math.Atan(Z / Math.Sqrt(X * X + Y * Y));

            while (Math.Abs(rB - B) > 0.00000000001)
            {
                B = rB;
                var tanB = Math.Tan(B);
                rB = a * ee * tanB / Math.Sqrt(1 + (1 - ee) * Math.Pow(tanB, 2));
                rB = Math.Atan((Z + rB) / Math.Sqrt(X * X + Y * Y));
            }

            L = rL * 180 / CoordConsts.PI;
            B = rB * 180 / CoordConsts.PI;

            H = Math.Sqrt(X * X + Y * Y) / Math.Cos(rB) - a / Math.Sqrt(1 - ee * Math.Pow(Math.Sin(rB), 2));
        }
        #endregion

        #region ��˹ͶӰ������
        /// <summary>
        /// �Ӵ�����굽ƽ������ĸ�˹����
        /// 
        /// Ĭ�ϵ���ʹ�üٶ���������ȴ�ͶӰ
        /// </summary>
        /// <param name="B">���γ��</param>
        /// <param name="L">��ؾ���</param>
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        public static void BL_xy(double B, double L, out double x, out double y, double a, double f)
        {
            BL_xy(B, L, out x, out y, a, f, 6, true);
        }

        /// <summary>
        /// �Ӵ�����굽ƽ������ĸ�˹���㣬��λ����С��
        /// 
        /// Ĭ�ϵ���ʹ�üٶ�����
        /// </summary>
        /// <param name="B">���γ��</param>
        /// <param name="L">��ؾ���</param>
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        /// <param name="beltWidth">ͶӰ�ִ��Ĵ���</param>
        public static void BL_xy(double B, double L, out double x, out double y, double a, double f, int beltWidth)
        {
            BL_xy(B, L, out x, out y, a, f, beltWidth, true);
        }

        /// <summary>
        /// �Ӵ�����굽ƽ������ĸ�˹���㣬��λ����С��
        /// 
        /// Ĭ�ϵ������ȴ�ͶӰ
        /// </summary>
        /// <param name="B">���γ��</param>
        /// <param name="L">��ؾ���</param>
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        /// <param name="assumedCoord">�Ƿ�ʹ�üٶ�����</param>
        public static void BL_xy(double B, double L, out double x, out double y, double a, double f, bool assumedCoord)
        {
            BL_xy(B, L, out x, out y, a, f, 6, assumedCoord);
        }

        /// <summary>
        /// �Ӵ�����굽ƽ������ĸ�˹���㣬��λ����С��
        /// </summary>
        /// <param name="B">���γ��</param>
        /// <param name="L">��ؾ���</param>
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        /// <param name="beltWidth">ͶӰ�ִ��Ĵ���</param>
        /// <param name="assumedCoord">�Ƿ�ʹ�üٶ�����</param>
        public static void BL_xy(double B, double L, out double x, out double y, double a, double f, int beltWidth, bool assumedCoord)
        {
            int beltNum;                           //ͶӰ�ִ��Ĵ���
            beltNum = (int)Math.Ceiling((L - (beltWidth == 3 ? 1.5 : 0)) / beltWidth);
            if (beltWidth == 3 && beltNum * 3 == L - 1.5) beltNum += 1;
            L -= beltNum * beltWidth - (beltWidth == 6 ? 3 : 0);

            Bl_xy(B, L, out x, out y, a, f, beltWidth);

            //����ɼٶ����꣬ƽ��500km��ǰ��Ӵ���
            if (assumedCoord) y += 500000 + beltNum * 1000000;
        }

        /// <summary>
        /// �Ӵ�����굽ƽ������ĸ�˹����
        /// 
        /// ָ�����������ߣ����ڽ����ڴ����㣬��ʱ�ز�ʹ�üٶ����꣬��λ����С��
        /// </summary>
        /// <param name="B">���γ��</param>
        /// <param name="L">��ؾ���</param>
        /// <param name="CenterL">����������</param>
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        /// <param name="beltWidth">ͶӰ�ִ��Ĵ���</param>
        public static void Bl_xy(double B, double dL, out double x, out double y, double a, double f, int beltWidth)
        {
            double ee = (2 * f - 1) / f / f;       //��һƫ���ʵ�ƽ��
            double ee2 = ee / (1 - ee);            //�ڶ�ƫ���ʵ�ƽ��

            double rB, tB, m;
            rB = B * CoordConsts.PI / 180;
            tB = Math.Tan(rB);
            m = Math.Cos(rB) * dL * CoordConsts.PI / 180;

            double N = a / Math.Sqrt(1 - ee * Math.Sin(rB) * Math.Sin(rB));
            double it2 = ee2 * Math.Pow(Math.Cos(rB), 2);

            x = m * m / 2 + (5 - tB * tB + 9 * it2 + 4 * it2 * it2) * Math.Pow(m, 4) / 24 + (61 - 58 * tB * tB + Math.Pow(tB, 4)) * Math.Pow(m, 6) / 720;
            x = MeridianLength(B, a, f) + N * tB * x;
            y = N * (m + (1 - tB * tB + it2) * Math.Pow(m, 3) / 6 + (5 - 18 * tB * tB + Math.Pow(tB, 4) + 14 * it2 - 58 * tB * tB * it2) * Math.Pow(m, 5) / 120);
        }

        /// <summary>
        /// ƽ�����꣨��Ȼ�����ٶ����꣩���������ĸ�˹���㣬��λ����С��
        /// 
        /// Ĭ��ʹ�����ȴ�
        /// </summary>
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="B">���γ��</param>
        /// <param name="L">��ؾ���</param>
        /// <param name="beltNum">ͶӰ�ִ��Ĵ���</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        public static void xy_BL(double x, double y, out double B, out double L, int beltNum,
            double a = Geo.Referencing.Ellipsoid.SemiMajorAxisOfCGCS2000, double f = Geo.Referencing.Ellipsoid.InverseFlatOfCGCS2000)
        {
            xy_BL(x, y, out B, out L, 6, beltNum, a, f);
        }

        /// <summary>
        /// ƽ�����꣨��Ȼ�����ٶ����꣩���������ĸ�˹���㣬��λ����С��
        /// </summary>
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="B">���γ��</param>
        /// <param name="L">��ؾ���</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        /// <param name="beltNum">�ִ����</param>
        /// <param name="beltWidth">ͶӰ�ִ��Ĵ���</param>
        public static void xy_BL(double x, double y, out double B, out double L, int beltWidth, int beltNum = 0,
            double a=Geo.Referencing.Ellipsoid.SemiMajorAxisOfCGCS2000, double f=Geo.Referencing.Ellipsoid.InverseFlatOfCGCS2000)
        {
            //���Ϊ�ٶ����꣬ת��Ϊ��Ȼ���� 
            if (y > 1000000)
            {
                beltNum = (int)Math.Ceiling(y / 1000000) - 1;
                y -= 1000000 * beltNum + 500000;
            }

            //���γ���뾭��
            xy_Bl(x, y, out B, out L, a, f, beltWidth);

            //��⾭��
            L += beltWidth * beltNum - ((beltWidth == 6) ? 3 : 0);
        }

        /// <summary>
        /// ƽ�����꣨��Ȼ���꣩���������ĸ�˹���㣬��λ����С��
        /// </summary>
        /// <param name="x">ƽ������</param>
        /// <param name="y">ƽ�����</param>
        /// <param name="B">���γ��</param>
        /// <param name="l">���Ȳ�</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        /// <param name="beltWidth">ͶӰ�ִ��Ĵ���</param>
        private static void xy_Bl(double x, double y, out double B, out double l, double a, double f, int beltWidth)
        {
            if (y > 1000000)
            {
                throw new Exception("�������ʹ���Ӧʹ����Ȼ����");
            }

            double ee = (2 * f - 1) / f / f;       //��һƫ���ʵ�ƽ��
            double ee2 = ee / (1 - ee);            //�ڶ�ƫ���ʵ�ƽ��

            double cA, cB, cC, cD, cE;

            cA = 1 + 3 * ee / 4 + 45 * ee * ee / 64 + 175 * Math.Pow(ee, 3) / 256 + 11025 * Math.Pow(ee, 4) / 16384;
            cB = 3 * ee / 4 + 15 * ee * ee / 16 + 525 * Math.Pow(ee, 3) / 512 + 2205 * Math.Pow(ee, 4) / 2048;
            cC = 15 * ee * ee / 64 + 105 * Math.Pow(ee, 3) / 256 + 2205 * Math.Pow(ee, 4) / 4096;
            cD = 35 * Math.Pow(ee, 3) / 512 + 315 * Math.Pow(ee, 4) / 2048;
            cE = 315 * Math.Pow(ee, 4) / 131072;

            double Bf = x / (a * (1 - ee) * cA);

            do
            {
                B = Bf;
                Bf = (x + a * (1 - ee) * (cB * Math.Sin(2 * Bf) / 2 - cC * Math.Sin(4 * Bf) / 4 + cD * Math.Sin(6 * Bf) / 6) - cE * Math.Sin(8 * Bf) / 8) / (a * (1 - ee) * cA);
            }
            while (Math.Abs(B - Bf) > 0.00000000001);

            double N = a / Math.Sqrt(1 - ee * Math.Pow(Math.Sin(Bf), 2));
            double V2 = 1 + ee2 * Math.Pow(Math.Cos(Bf), 2);
            double it2 = ee2 * Math.Pow(Math.Cos(Bf), 2);
            double tB2 = Math.Pow(Math.Tan(Bf), 2);

            B = Bf - V2 * Math.Tan(Bf) / 2 * (Math.Pow(y / N, 2) - (5 + 3 * tB2 + it2 - 9 * it2 * tB2) * Math.Pow(y / N, 4) / 12 + (61 + 90 * tB2 + 45 * tB2 * tB2) * Math.Pow(y / N, 6) / 360);
            l = (y / N - (1 + 2 * tB2 + it2) * Math.Pow(y / N, 3) / 6 + (5 + 28 * tB2 + 24 * tB2 * tB2 + 6 * it2 + 8 * it2 * tB2) * Math.Pow(y / N, 5) / 120) / Math.Cos(Bf);

            B = B * 180 / CoordConsts.PI;
            l = l * 180 / CoordConsts.PI;
        }

        /// <summary>
        /// ����ͶӰ�����ڴ�����
        /// </summary>
        /// <param name="x0">��ͶӰ����x����</param>
        /// <param name="y0">��ͶӰ����y����</param>
        /// <param name="x1">��ͶӰ����x����</param>
        /// <param name="y1">��ͶӰ����y����</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�����ı��ʵ���</param>
        /// <param name="beltWidth">����</param>
        public static void PointToWestBelt(double x0, double y0, out double x1, out double y1, double a, double f, int beltWidth)
        {
            int beltNum = 0;
            if (y0 > 1000000)
            {
                beltNum = (int)Math.Ceiling(y0 / 1000000) - 1;
                y0 -= 1000000 * beltNum + 500000;
            }

            double B, l;
            xy_Bl(x0, y0, out B, out l, a, f, beltWidth);
            Bl_xy(B, l + beltWidth, out x1, out y1, a, f, beltWidth);

            y1 += (beltNum - 1) * 1000000 + 500000;
        }

        /// <summary>
        /// ��ͶӰ�����ڴ�����
        /// </summary>
        /// <param name="x0">��ͶӰ����x����</param>
        /// <param name="y0">��ͶӰ����y����</param>
        /// <param name="x1">��ͶӰ����x����</param>
        /// <param name="y1">��ͶӰ����y����</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�����ı��ʵ���</param>
        /// <param name="beltWidth">����</param>
        public static void PointToEastBelt(double x0, double y0, out double x1, out double y1, double a, double f, int beltWidth)
        {
            int beltNum = 0;
            if (y0 > 1000000)
            {
                beltNum = (int)Math.Ceiling(y0 / 1000000) - 1;
                y0 -= 1000000 * beltNum + 500000;
            }

            double B, l;
            xy_Bl(x0, y0, out B, out l, a, f, beltWidth);
            Bl_xy(B, l - beltWidth, out x1, out y1, a, f, beltWidth);

            y1 += (beltNum + 1) * 1000000 + 500000;
        }

        /// <summary>
        /// ���ȴ������ȴ�֮�������ת��
        /// </summary>
        /// <param name="x0">ԭʼx����</param>
        /// <param name="y0">ԭʼy����</param>
        /// <param name="x1">Ŀ��x����</param>
        /// <param name="y1">Ŀ��y����</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        /// <param name="beltWidth">Ŀ����������˲���ͬʱҲָ����ԭʼ����Ĵ���</param>
        public static void PointBeltWidthChanged(double x0, double y0, out double x1, out double y1, double a, double f, int originalBeltWidth)
        {
            double B, l;

            //���Ϊ�ٶ����꣬ת��Ϊ��Ȼ����
            int beltNum = 0;
            if (y0 > 1000000)
            {
                beltNum = (int)Math.Ceiling(y0 / 1000000) - 1;
                y0 -= 1000000 * beltNum + 500000;
            }
            
            //���
            xy_Bl(x0, y0, out B, out l, a, f, originalBeltWidth);

            //���ԭʼ����Ϊ���ȴ�ͶӰ
            if (originalBeltWidth == 6)
            {
                //���������������
                if (Math.Abs(l) <= 1.5)
                {
                    x1 = x0;
                    y1 = y0;
                    if (beltNum > 0)
                    {
                        beltNum = beltNum * 2 - 1;
                        y1 += 1000000 * beltNum + 500000;
                    }
                    return;
                }
                //����ڷִ�������Ե
                else if (l < -1.5)
                {
                    Bl_xy(B, 3 + l, out x1, out y1, a, f, 3);
                    if (beltNum > 0)
                    {
                        beltNum = (beltNum - 1) * 2;
                        y1 += 1000000 * beltNum + 500000;
                    }
                    return;
                }
                //����ڷִ��Ķ���Ե
                else
                {
                    Bl_xy(B, 3 - l, out x1, out y1, a, f, 3);
                    if (beltNum > 0)
                    {
                        beltNum = (beltNum + 1) * 2;
                        y1 += 1000000 * beltNum + 500000;
                    }
                    return;
                }
            }
            //���ԭʼ����Ϊ���ȴ�ͶӰ
            else if (originalBeltWidth == 3)
            { 
                int rem, newBeltNum;
                newBeltNum = Math.DivRem(beltNum, 2, out rem) + 1;


                //����������ȴ������������������������غ�
                if (rem == 1)
                {
                    x1 = x0;
                    y1 = y0;
                    if (beltNum > 0)
                        y1 += 1000000 * newBeltNum + 500000;
                    return;
                }
                else if (l > 0)
                {
                    Bl_xy(B, l - 3, out x1, out y1, a, f, 6);
                    if (beltNum > 0)
                        y1 += 1000000 * newBeltNum + 500000;
                    return;
                }
                else
                {
                    Bl_xy(B, l + 3, out x1, out y1, a, f, 6);
                    if (beltNum > 0)
                    {
                        newBeltNum -= 1;
                        y1 += 1000000 * newBeltNum + 500000;
                    }
                    return;
                }
            }
            else
                throw new Exception("�����ͶӰ����");
        }
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
        public static SpatialRectCoordinate CoordinateTransform(Geo.Coordinates.IXYZ old, TransformParameters p)
        {
            double X, Y, Z;
            double rou = 206264.80624709635515647335733078;// 3600 * 180 / Consts.PI;

            X = p.Dx + old.X * (1 + p.m) + (old.Y * p.Ez - old.Z * p.Ey) / rou;
            Y = p.Dy + old.Y * (1 + p.m) + (old.Z * p.Ex - old.X * p.Ez) / rou;
            Z = p.Dz + old.Z * (1 + p.m) + (old.X * p.Ey - old.Y * p.Ex) / rou;

            return new SpatialRectCoordinate(X, Y, Z);
        }

        /// <summary>
        /// ����ɯ�߲���ģ������ת�����Ӵ�����굽�������
        /// </summary>
        /// <param name="pntBLH"></param>
        /// <param name="eOld"></param>
        /// <param name="eNew"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static GeodeticCoordinate CoordinateTransform(GeodeticCoordinate pntBLH, ReferenceEllipsoid eOld, ReferenceEllipsoid eNew, TransformParameters param)
        {
            double X, Y, Z;
            BLH_XYZ(pntBLH.B, pntBLH.L, pntBLH.H, out X, out Y, out Z, eOld.a, eOld.f);

            SpatialRectCoordinate s = CoordinateTransform(new SpatialRectCoordinate(X, Y, Z), param);

            double B, L, H;
            XYZ_BLH(s.X, s.Y, s.Z, out B, out L, out H, eNew.a, eNew.f);
            return new GeodeticCoordinate(B, L, H);
        }

        /// <summary>
        /// ����ת��
        /// </summary>
        /// <param name="xyzOld">������</param>
        /// <param name="xyzNew">������</param>
        /// <param name="param">ת������</param>
        public static void CoordinateTransform(double[] xyzOld, double[] xyzNew, TransformParameters param)
        {
            if (xyzNew.Length != 3 || xyzOld.Length != 3)
            {
                throw new Exception("����ά�����󣬱���Ϊ3");
            }
            SpatialRectCoordinate p1 = new SpatialRectCoordinate(xyzOld[0], xyzOld[1], xyzOld[2]);
            SpatialRectCoordinate p2 = CoordinateTransform(p1, param);
            xyzNew[0] = p2.X;
            xyzNew[1] = p2.Y;
            xyzNew[2] = p2.Z;
        }
        
        /// <summary>
        /// ���ò���ɯ����ת��ģ�����ת������
        /// </summary>
        /// <param name="oldPoint">������ϵ�µ�����꼯��</param>
        /// <param name="newPoint">������ϵ�µ�����꼯��</param>
        /// <param name="weight">Ȩ��</param>
        /// <param name="paraCount">ת����������������Ϊ3��4��7��</param>
        /// <returns></returns>
        public static TransformParameters ParameterCompute(SpatialRectCoordinate[] oldPoint, SpatialRectCoordinate[] newPoint, 
                                                           double[] weight = null,int paraCount=7)
        {
            if (newPoint.Length != oldPoint.Length)  throw new ArgumentException("�¾���������Ӧ��һ�£�");

            int pntCount = newPoint.Length;

            if(weight ==null)
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
                    return new TransformParameters(B[0, paraCount], B[1, paraCount], B[2, paraCount]);

                case 4:
                    return new TransformParameters(B[0, paraCount], B[1, paraCount], B[2, paraCount], B[3, paraCount]);

                case 7:
                    return new TransformParameters(B[0, paraCount], B[1, paraCount], B[2, paraCount], B[3, paraCount], 
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
        /// <param name="B1">��֪��γ��</param>
        /// <param name="L1">��֪�㾭��</param>
        /// <param name="S">����߳�</param>
        /// <param name="A1">��ط�λ��</param>
        /// <param name="B2">�����γ��</param>
        /// <param name="L2">����㾭��</param>
        /// <param name="A2">��ط���λ��</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        public static void Bessel_PntSA_Pnt(double B1, double L1, double S, double A1, out double B2, out double L2, out double A2, double a, double f)
        {
            double ee = (2 * f - 1) / f / f;                                           //��һƫ���ʵ�ƽ��
            double ee2 = (2 * f - 1) / (f - 1) / (f - 1);                              //�ڶ�ƫ���ʵ�ƽ��

            B1 = B1 * CoordConsts.PI / 180;
            L1 = L1 * CoordConsts.PI / 180;
            A1 = A1 * CoordConsts.PI / 180;

            double u1 = Math.Atan(Math.Sqrt(1 - ee) * Math.Tan(B1));                     //u1 is (-90, 90)
            double m = Math.Cos(u1) * Math.Sin(A1);                                      //sin(m)
            m = Math.Atan(m / Math.Sqrt(1 - m * m));                                     //m is (-90, 90)
            if (m < 0) m += 2 * CoordConsts.PI;

            double M = Math.Atan(Math.Tan(u1) / Math.Cos(A1));                          //M is (-90, 90)
            if (M < 0) M += CoordConsts.PI;                                                    //change M to (0, 180)

            //compute cofficients
            double KK = ee2 * Math.Pow(Math.Cos(m), 2);                                   
            double alpha = Math.Sqrt(1 + ee2) * (1 - KK / 4 + 7 * KK * KK / 64 - 15 * Math.Pow(KK, 3) / 256) / a;
            double beta = KK / 4 - KK * KK / 8 + 37 * Math.Pow(KK, 3) / 512;
            double gama = KK * KK * (1 - KK) / 128;

            //loop for compute sigma
            double sigma, temp;
            sigma = alpha * S;
            temp = 0;
            while (Math.Abs(temp - sigma) > 0.00001 / 206265)                         //����0.00001��
            {
                temp = sigma;
                sigma = alpha * S + beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) + gama * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma);
            }

            A2 = Math.Atan(Math.Tan(m) / Math.Cos(M + sigma));                         //A2 is (-90, 90)
            if (A2 < 0) A2 += CoordConsts.PI;
            if (A1 < CoordConsts.PI) A2 += CoordConsts.PI;

            double u2 = Math.Atan(-Math.Cos(A2) * Math.Tan(M + sigma));                //u2 is (-90, 90)

            double lamda1;
            lamda1 = Math.Atan(Math.Sin(u1) * Math.Tan(A1));                           //lamda1
            if (lamda1 < 0) lamda1 += CoordConsts.PI;
            if (m > CoordConsts.PI) lamda1 += CoordConsts.PI;

            double lamda2;                                                             //lamda2
            lamda2 = Math.Atan(Math.Sin(u2) * Math.Tan(A2));
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

            L2 = L1 + lamda2 - lamda1 - Math.Sin(m) * (alpha * sigma + beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) + gama * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma));
            if (L2 < 0) L2 += 2 * CoordConsts.PI;

            B2 = Math.Atan(Math.Sqrt(1 + ee2) * Math.Tan(u2)) * 180 / CoordConsts.PI;          //B in (-90, 90)
            L2 = L2 * 180 / CoordConsts.PI;
            A2 = A2 * 180 / CoordConsts.PI;
        }

        /// <summary>
        /// ����������������ط�λ��
        /// </summary>
        /// <param name="B1">���γ��</param>
        /// <param name="L1">��㾭��</param>
        /// <param name="B2">ĩ��γ��</param>
        /// <param name="L2">ĩ�㾭��</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        /// <returns>�����Ĵ�ط�λ��</returns>
        public static double Bessel_BL_A(double B1, double L1, double B2, double L2, double a, double f)
        {
            if (Math.Abs(L1 - L2) < 0.00001 / 206265) return CoordConsts.PI;                  //�������0.00001��

            double ee = (2 * f - 1) / f / f;                                           //��һƫ���ʵ�ƽ��

            double u1 = Math.Atan(Math.Sqrt(1 - ee) * Math.Tan(B1 * CoordConsts.PI / 180));
            double u2 = Math.Atan(Math.Sqrt(1 - ee) * Math.Tan(B2 * CoordConsts.PI / 180));
            double dL = (L2 - L1) * CoordConsts.PI / 180;

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
        /// �ɴ������������߳�
        /// </summary>
        /// <param name="B1">���γ��</param>
        /// <param name="L1">��㾭��</param>
        /// <param name="B2">ĩ��γ��</param>
        /// <param name="L2">ĩ�㾭��</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        /// <returns>�����Ĵ���߳�</returns>
        public static double Bessel_BL_S(double B1, double L1, double B2, double L2, double a, double f)
        {
            if (Math.Abs(L1 - L2) < 0.00001 / 206265)                                  //�������0.00001��
            {
                return Math.Abs(MeridianLength(B1, a, f) - MeridianLength(B2, a, f));
            }

            double ee = (2 * f - 1) / f / f;                                           //��һƫ���ʵ�ƽ��
            double ee2 = (2 * f - 1) / (f - 1) / (f - 1);                              //�ڶ�ƫ���ʵ�ƽ��

            double u1 = Math.Atan(Math.Sqrt(1 - ee) * Math.Tan(B1 * CoordConsts.PI / 180));
            double u2 = Math.Atan(Math.Sqrt(1 - ee) * Math.Tan(B2 * CoordConsts.PI / 180));
            double dL = (L2 - L1) * CoordConsts.PI / 180;

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
            alpha = Math.Sqrt(1 + ee2) * (1 - KK / 4 + 7 * KK * KK / 64 - 15 * Math.Pow(KK, 3) / 256) / a;
            beta = KK / 4 - KK * KK / 8 + 37 * Math.Pow(KK, 3) / 512;
            gama = KK * KK * (1 - KK) / 128;

            return (sigma - beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) - gama * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma)) / alpha;
        }

        /// <summary>
        /// ������ط�λ������������Ŀռ�ֱ��������ⷽλ��
        /// </summary>
        /// <param name="pntA">���ռ�ֱ������</param>
        /// <param name="pntB">ĩ��ռ�ֱ������</param>
        /// <param name="a">�ο����򳤰���</param>
        /// <param name="f">�ο�������ʵ���</param>
        /// <returns>�����Ĵ�ط�λ��</returns>
        public static double Missile_XYZ_A(SpatialRectCoordinate pntA, SpatialRectCoordinate pntB, double a, double f)
        {
            double B, L, H;
            XYZ_BLH(pntA.X, pntA.Y, pntA.Z, out B, out L, out H, a, f);
            B = B * CoordConsts.PI / 180;
            L = L * CoordConsts.PI / 180;

            double Dx, Dy, Dz;
            Dx = pntB.X - pntA.X;
            Dy = pntB.Y - pntA.Y;
            Dz = pntB.Z - pntA.Z;

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
        /// <param name="B">γ��</param>
        /// <param name="a">������</param>
        /// <param name="f">���ʵ���</param>
        /// <returns>�����߻���</returns>
        public static double MeridianLength(double B, double a, double f)
        {
            double ee = (2 * f - 1) / f / f;       //��һƫ���ʵ�ƽ��            
            double rB = B * CoordConsts.PI / 180;         //����ת��Ϊ����

            //�����߻�����ʽ��ϵ��
            double cA, cB, cC, cD, cE;
            cA = 1 + 3 * ee / 4 + 45 * Math.Pow(ee, 2) / 64 + 175 * Math.Pow(ee, 3) / 256 + 11025 * Math.Pow(ee, 4) / 16384;
            cB = 3 * ee / 4 + 15 * Math.Pow(ee, 2) / 16 + 525 * Math.Pow(ee, 3) / 512 + 2205 * Math.Pow(ee, 4) / 2048;
            cC = 15 * Math.Pow(ee, 2) / 64 + 105 * Math.Pow(ee, 3) / 256 + 2205 * Math.Pow(ee, 4) / 4096;
            cD = 35 * Math.Pow(ee, 3) / 512 + 315 * Math.Pow(ee, 4) / 2048;
            cE = 315 * Math.Pow(ee, 4) / 131072;

            //�����߻���
            return a * (1 - ee) * (cA * rB - cB * Math.Sin(2 * rB) / 2 + cC * Math.Sin(4 * rB) / 4 - cD * Math.Sin(6 * rB) / 6 + cE * Math.Sin(8 * rB) / 8);
        }

        public static double DMSToDegree(double dms)
        {
            double d, m, s;

            d = (int)(dms / 10000);
            m = (int)((dms - d * 10000) / 100);
            s = dms - d * 10000 - m * 100;

            return d + m / 60 + s / 3600.0;
        }

        public static double DegreeToDMS(double degree)
        {
            double d, m, s;
            d = (int)degree;
            m = (int)((degree - d) * 60);
            s = ((degree - d) * 60 - m) * 60;

            return d * 10000 + m * 100 + s;
        }
    }
}