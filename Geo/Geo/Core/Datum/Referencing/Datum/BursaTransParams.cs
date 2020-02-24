using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 布尔沙转换参数,具有 7 参数、 4 参数、 3 参数。
    /// </summary>
    public class BursaTransParams : IBursaTransParams, IEquatable<BursaTransParams>
    {
        private const double SEC_TO_RAD = 4.84813681109535993589914102357e-6;

        /// <summary>
        /// 初始化一个七参数。
        /// </summary>
        /// <param name="dx_m">Bursa Wolf shift in meters.</param>
        /// <param name="dy_m">Bursa Wolf shift in meters.</param>
        /// <param name="dz_m">Bursa Wolf shift in meters.</param>
        /// <param name="ex_s">Bursa Wolf rotation in arc fraction.</param>
        /// <param name="ey_s">Bursa Wolf rotation in arc fraction.</param>
        /// <param name="ez_s">Bursa Wolf rotation in arc fraction.</param>
        /// <param name="scale_ppm">Bursa Wolf scaling in parts per million.</param>
        /// <param name="areaOfUse">Area of use for this transformation</param>
        public BursaTransParams(double dx_m = 0, double dy_m = 0, double dz_m = 0, double ex_s = 0, double ey_s = 0, double ez_s = 0, double scale_ppm = 0, string areaOfUse = "")
        {
            Dx = dx_m; Dy = dy_m; Dz = dz_m;
            Ex = ex_s; Ey = ey_s; Ez = ez_s;
            Scale_ppm = scale_ppm;
            Discription = areaOfUse;
        }


        #region 七参数
        /// <summary>
        /// 平移参数，单位米。
        /// </summary>
        public double Dx { get; set; }
        /// <summary>
        /// 平移参数，单位米。
        /// </summary>
        public double Dy { get; set; }
        /// <summary>
        /// 平移参数，单位米。
        /// </summary>
        public double Dz { get; set; }
        /// <summary>
        /// 旋转参数，单位角秒，又称弧秒，是量度角度的单位，即角分的六十分之一，符号为"。
        /// 在不会引起混淆时，可简称作秒。
        /// </summary>
        public double Ex { get; set; }
        /// <summary>
        /// 旋转参数，单位角秒，又称弧秒，是量度角度的单位，即角分的六十分之一，符号为"。
        /// 在不会引起混淆时，可简称作秒。
        /// </summary>
        public double Ey { get; set; }
        /// <summary> 
        /// 旋转参数，单位角秒，又称弧秒，是量度角度的单位，即角分的六十分之一，符号为"。
        /// 在不会引起混淆时，可简称作秒。
        /// </summary>
        public double Ez { get; set; }
        /// <summary>
        /// 尺度因子，单位为百万分之一(ppm)，10^(-6)。
        /// Bursa Wolf scaling in parts per million.
        /// m = (S新 - S旧) / S旧
        /// </summary>
        public double Scale_ppm { get; set; }

        /// <summary>
        /// 尺度因子，单位米。
        /// </summary>
        public double Scale_m { get { return Scale_ppm / 1.0e+6; } set { Scale_ppm = value * 1.0e-6; } }
        /// <summary>
        /// 描述
        /// </summary>
        public string Discription { get; set; }

        #endregion


        #region IEquatable<Wgs84ConversionInfo> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as BursaTransParams);
        }

        /// <summary>
        /// Returns a hash code for the specified object
        /// </summary>
        /// <returns>A hash code for the specified object</returns>
        public override int GetHashCode()
        {
            return Dx.GetHashCode() ^ Dy.GetHashCode() ^ Dz.GetHashCode() ^
                Ex.GetHashCode() ^ Ey.GetHashCode() ^ Ez.GetHashCode() ^
                Scale_ppm.GetHashCode();
        }

        /// <summary>
        /// Checks whether the values of this instance is equal to the values of another instance.
        /// Only parameters used for coordinate system are used for comparison.
        /// Name, abbreviation, authority, alias and remarks are ignored in the comparison.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public bool Equals(BursaTransParams obj)
        {
            if (obj == null)
                return false;
            return Equals(obj, 1.0e-9);
        //    return obj.Dx == this.Dx && obj.Dy == this.Dy && obj.Dz == this.Dz &&
        //        obj.Ex == this.Ex && obj.Ey == this.Ey && obj.Ez == this.Ez && obj.Scale_ppm == this.Scale_ppm;
        }
        public bool Equals(BursaTransParams obj, double tollerence =1.0e-9 )
        {
            if (obj == null)
                return false;
            return Math.Abs(obj.Dx - this.Dx) < tollerence 
                && Math.Abs(obj.Dy - this.Dy ) < tollerence
                && Math.Abs( obj.Dz - this.Dz ) < tollerence
                && Math.Abs(obj.Ex - this.Ex ) < tollerence
                && Math.Abs( obj.Ey - this.Ey ) < tollerence
                && Math.Abs( obj.Ez - this.Ez ) < tollerence
                && Math.Abs(obj.Scale_ppm - this.Scale_ppm) < tollerence;
        }

        #endregion

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = String.Empty;

            str += "Ex_s=" + Ex.ToString("0.00000") +
                   ", Ey_s=" + Ey.ToString("0.00000") +
                   ", Ez_s=" + Ez.ToString("0.00000");
            str += ", Dx_m=" + Dx.ToString("0.00000") +
                   ", Dy_m=" + Dy.ToString("0.00000") +
                   ", Dz_m=" + Dz.ToString("0.00000");
            str += ", Scale_ppm=" + Scale_ppm.ToString("0.00000");
            return str;
        }


        /// <summary>
        /// 是否所有参数都为0
        /// </summary>
        /// <returns></returns>
        public bool IsZero
        {
            get
            {
                return !(Dx != 0 || Dy != 0 || Dz != 0 || Ex != 0 || Ey != 0 || Ez != 0 || Scale_ppm != 0);
            }
        }
        #region 常用

        public static BursaTransParams WGS72ToWGS84
        {
            get
            {
                return new BursaTransParams(0, 0, 4.5, 0, 0, 0.554,  0.219);
            }
        }
        public static BursaTransParams ED50ToWGS84
        {
            get
            {
                return new BursaTransParams(-81.0703, -89.3603, -115.7526,
                                                           -0.48488, -0.02436, -0.41321,
                                                           -0.540645); //Parameters for Denmark
            }
        }
        #endregion




        /// <summary>
        /// Affine Bursa-Wolf matrix transformation
        /// </summary>
        /// <remarks>
        /// <para>Transformation of coordinates from one geographic coordinate system into another 
        /// (also colloquially known as a "datum transformation") is usually carried out as an 
        /// implicit concatenation of three transformations:</para>
        /// <para>[geographical to geocentric >> geocentric to geocentric >> geocentric to geographic</para>
        /// <para>
        /// The middle part of the concatenated transformation, from geocentric to geocentric, is usually 
        /// described as a simplified 7-parameter Helmert transformation, expressed in matrix form with 7 
        /// parameters, in what is known as the "Bursa-Wolf" formula:<br/>
        /// <code>
        ///  S = 1 + Ppm/1000000
        ///  [ Xt ]    [     S   -Ez*S   +Ey*S   Dx ]  [ Xs ]
        ///  [ Yt ]  = [ +Ez*S       S   -Ex*S   Dy ]  [ Ys ]
        ///  [ Zt ]    [ -Ey*S   +Ex*S       S   Dz ]  [ Zs ]
        ///  [ 1  ]    [     0       0       0    1 ]  [ 1  ]
        /// </code><br/>
        /// The parameters are commonly referred to defining the transformation "from source coordinate system 
        /// to target coordinate system", whereby (XS, YS, ZS) are the coordinates of the point in the source 
        /// geocentric coordinate system and (XT, YT, ZT) are the coordinates of the point in the target 
        /// geocentric coordinate system. But that does not define the parameters uniquely; neither is the
        /// definition of the parameters implied in the formula, as is often believed. However, the 
        /// following definition, which is consistent with the "Position Vector Transformation" convention, 
        /// is common E&amp;P survey practice: 
        /// </para>	
        /// <para>(dX, dY, dZ): Translation vector, to be added to the point's position vector in the source 
        /// coordinate system in order to transform from source system to target system; also: the coordinates 
        /// of the origin of source coordinate system in the target coordinate system </para>
        /// <para>(RX, RY, RZ): Rotations to be applied to the point's vector. The sign convention is such that 
        /// a positive rotation about an axis is defined as a clockwise rotation of the position vector when 
        /// viewed from the origin of the Cartesian coordinate system in the positive direction of that axis;
        /// e.g. a positive rotation about the Z-axis only from source system to target system will result in a
        /// larger longitude value for the point in the target system. Although rotation angles may be quoted in
        /// any angular unit of measure, the formula as given here requires the angles to be provided in radians.</para>
        /// <para>: The scale correction to be made to the position vector in the source coordinate system in order 
        /// to obtain the correct scale in the target coordinate system. M = (1 + dS*10-6), whereby dS is the scale
        /// correction expressed in parts per million.</para>
        /// <para><see href="http://www.posc.org/Epicentre.2_2/DataModel/ExamplesofUsage/eu_cs35.html"/> for an explanation of the Bursa-Wolf transformation</para>
        /// </remarks>
        /// <returns></returns>
        public double[] GetAffineTransform()
        {
            double RS = 1 + Scale_ppm * 0.000001;
            return new double[7] { 
                RS,
                Ex * SEC_TO_RAD * RS,
                Ey * SEC_TO_RAD * RS,
                Ez * SEC_TO_RAD * RS,
                Dx, 
                Dy, 
                Dz };

        }
        public double[,] GetAffineMatrix()
        {
            double RS = 1 + Scale_ppm * 0.000001;
            return new double[3, 4] {
            { RS,				-Ez*SEC_TO_RAD*RS,	+Ey*SEC_TO_RAD*RS,	Dx} ,
            { Ez*SEC_TO_RAD*RS,	RS,					-Ex*SEC_TO_RAD*RS,	Dy} ,
            { -Ey*SEC_TO_RAD*RS,Ex*SEC_TO_RAD*RS,	RS,					Dz}
            };
        }

        public double[] Trans(double[] pt)
        {
           double [][]old =  Geo.Utils.MatrixUtil.Transpose( Geo.Utils.MatrixUtil.Create(pt));
           double [][] m =  Geo.Utils.MatrixUtil.Create(GetAffineMatrix());

           double[][] reslt = Geo.Utils.MatrixUtil.GetMultiply( old,m);
           return reslt[0];
        }

        /// <summary>
        /// 反相参数。
        /// </summary>
        /// <returns></returns>
        public IBursaTransParams GetInverse()
        {
            double inverScale_ppm = -1.0 * Scale_m / (Scale_m + 1) * 1.0e+6;//inverScale_ppm

          //  throw new Exception("inverScale_ppm:" + inverScale_ppm);

            return new BursaTransParams(-Dx, -Dy, -Dz, -Ex, -Ey, -Ez, inverScale_ppm, Discription + "的反参数");
        }

        /// <summary>
        /// 椭球转换
        /// </summary>
        /// <param name="xyz"></param>
        /// <returns></returns>
        public double[] Transform(double[] xyz)
        {
            double[] v = this.GetAffineTransform();
            return new double[] {
				v[0] * xyz[0] - v[3] * xyz[1] + v[2] * xyz[2] + v[4],
				v[3] * xyz[0] + v[0] * xyz[1] - v[1] * xyz[2] + v[5],
			   -v[2] * xyz[0] + v[1] * xyz[1] + v[0] * xyz[2] + v[6] };
            // m * X - Dx * Y + Ez * Z + Dx
            // m * Y + Dx* X - Ex * Z + Dy
            // m * Z  -Ez *X + Dy * Y + + Dz
        }
        /// <summary>
        /// 反向转换,尺度不变。
        /// </summary>
        /// <param name="xyz"></param>
        /// <returns></returns>
        public double[] InverseTranseform(double[] xyz)
        {
            double[] v = this.GetAffineTransform();
            return new double[] {
				v[0] * xyz[0] + v[3] * xyz[1] - v[2] * xyz[2] - v[4],
			   -v[3] * xyz[0] + v[0] * xyz[1] + v[1] * xyz[2] - v[5],
			    v[2] * xyz[0] - v[1] * xyz[1] + v[0] * xyz[2] - v[6] };
        }

    }
}
