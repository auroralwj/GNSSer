//2014.05.22， Cui Yang, 新增了一些方法

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// X,Y,Z 三维坐标。三维空间的 3 个双精度分量。
    /// 也是向量。
    /// </summary>
    [Serializable]
    public class XYZ : XY, IXYZ, IEquatable<XYZ>, IObjectRow
    {
        #region 构造函数
        /// <summary>
        /// 以一个二维坐标初始化
        /// </summary>
        /// <param name="xy"></param>
        public XYZ(XY xy) : base(xy.X, xy.Y) { }//this.SetDimension(3); } 

        /// <summary>
        /// 最常用的构造函数，分别初始化三个分量。
        /// </summary>
        /// <param name="x">X 坐标分量</param>
        /// <param name="y">Y 坐标分量</param>
        /// <param name="z">Z 坐标分量</param>
        public XYZ(double x =0, double y =0, double z = 0)
            : base(x, y)
        { 
           // this.SetDimension(3);
            this.Z = z;
        }
        /// <summary>
        /// 以一个三个元素的一维数组初始化
        /// </summary>
        /// <param name="array">三个元素的一维数组</param>
        public XYZ(double[] array)
            : this(array[0], array[1], array[2]) { }

        #endregion 
         
        /// <summary>
        /// Multiply each component in the vector by a given factor.
        /// </summary>
        /// <param name="factor">The factor.</param>
        public void Mul(double factor)
        {
            X *= factor;
            Y *= factor;
            Z *= factor;
        }
        /// <summary>
        /// 与一点的欧式距离
        /// </summary>
        /// <param name="xyz"></param>
        /// <returns></returns>
        public double Distance(XYZ xyz)
        {
          //  if (Dimension == 2) return base.Distance(xyz);
            double dx = xyz.X - X;
            double dy = xyz.Y - Y;
            double dz = xyz.Z - Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        /// <summary>
        /// 距离
        /// </summary>
        /// <param name="xyz"></param>
        /// <returns></returns>
        public double Distance(IXYZ xyz)
        {
           // if (Dimension == 2) return base.Distance(xyz);
            double dx = xyz.X - X;
            double dy = xyz.Y - Y;
            double dz = xyz.Z - Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override double this[int i] {
            get { if (i == 2) return Z; return base[i]; }
            set { if (i == 2)   Z = value; else  base[i] = value; }
        }
        /// <summary>
        /// 3
        /// </summary>
        public override int Count { get { return 3; } }
        /// <summary>
        /// 是否都小
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool IsHasSmallerThan(double val)
        {
            return X < val || Y < val || Z < val;
        }
        /// <summary>
        /// 是否有效
        /// </summary>
        public override bool IsValid { get { return Geo.Utils.DoubleUtil.IsValid(Z) && base.IsValid; } }
        /// <summary>
        /// 向量的模/范数/长度/元素平方和的根
        /// </summary>
        public override double Norm
        {
            get
            {
                double norm = 0;
                norm += X * X;
                norm += Y * Y;
                norm += Z * Z;
                return Math.Sqrt(norm);
            }
        }
        /// <summary>
        /// 返回轻量级
        /// </summary>
        public Xyz Xyz => new Xyz(X, Y, Z);
        /// <summary>
        /// Calculates the magnitude of the vector.
        /// 长度，标量
        /// </summary>
        /// <returns>The vector magnitude.</returns>
        public double Magnitude()
        {
            return this.Norm;
        }
        /// <summary>
        /// 计算两个矢量的角度。 单位默认为度
        /// </summary>
        /// <param name="vec">The second vector.</param>
        /// <param name="unit">单位</param>
        /// <returns>
        /// The angle between the two vectors.
        /// </returns>
        public double Angle(XYZ vec, AngleUnit unit = AngleUnit.Degree)
        {
            double val = Math.Acos(Dot(vec) / (Magnitude() * vec.Magnitude()));
            if (unit == AngleUnit.Degree)
                val *= AngularConvert.RadToDegMultiplier;
            return val;
        }

        /// <summary>
        /// Rotates the XYZ coordinates around the X-axis.
        /// </summary>
        public XYZ RotateX(double radians)
        {
            XY xy = new XY(this.Y, this.Z);
            var newXY = xy.RotateRad(radians);
            var newXyz = new XYZ(X, newXY.X, newXY.Y);

            return newXyz;
        }

        /// <summary>
        /// Rotates the XYZ coordinates around the Y-axis.
        /// </summary>
        public XYZ RotateY(double radians)
        {
            XY xy = new XY(this.Z, this.X);
            var newXY = xy.RotateRad(radians);
            var newXyz = new XYZ(newXY.Y, Y, newXY.X);

            double x = (Math.Cos(radians) * X) + (Math.Sin(radians) * Z);
            double z = (-Math.Sin(radians) * X) + (Math.Cos(radians) * Z);
            return new XYZ(x, Y, z);
        }

        /// <summary>
        /// Rotates the XYZ coordinates around the Z-axis.
        /// </summary>
        public XYZ RotateZ(double radians)
        {
            XY xy = new XY(this.X, this.Y);
            var newXY = xy.RotateRad(radians);
            var newXyz = new XYZ(newXY.X, newXY.Y, this.Z);
            return newXyz;
        }

        /// <summary>
        /// 计算旋转角度，单位弧度
        /// </summary>
        /// <param name="oldXyz"></param>
        /// <param name="newXyz"></param>
        /// <returns></returns>
        public static double GetRotateXAngle(XYZ oldXyz, XYZ newXyz)
        {
            return GetIncludedAngle(new XY(oldXyz.Y, oldXyz.Z), new XY(newXyz.Y, newXyz.Z));
        }
        /// <summary>
        /// 计算旋转角度，单位弧度
        /// </summary>
        /// <param name="oldXyz"></param>
        /// <param name="newXyz"></param>
        /// <returns></returns>
        public static double GetRotateYAngle(XYZ oldXyz, XYZ newXyz)
        {
            return GetIncludedAngle(new XY(oldXyz.Z, oldXyz.X), new XY(newXyz.Z, newXyz.X));
        }
        /// <summary>
        /// 计算旋转角度，单位弧度
        /// </summary>
        /// <param name="oldXyz"></param>
        /// <param name="newXyz"></param>
        /// <returns></returns>
        public static double GetRotateZAngle(XYZ oldXyz, XYZ newXyz)
        {
            return GetIncludedAngle(oldXyz, newXyz);
        }
        /// <summary>
        /// 计算两个向量夹角
        /// </summary>
        /// <param name="vector"></param> 
        /// <returns></returns>
        public  double GetIncludedAngle(XYZ vector) { return GetIncludedAngle(this, vector); }
        /// <summary>
        /// 计算两个向量夹角
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        public static double GetIncludedAngle(XYZ vectorA, XYZ vectorB)
        {
            double  cosAngle = vectorA.Dot(vectorB) / (vectorA.Length * vectorB.Length);
            return Math.Acos(cosAngle);
        }
        #region 坐标旋转
        /// <summary>
        /// 坐标旋转。
        /// </summary>
        /// <param name="angle">在三个轴的旋转角度</param>
        /// <returns></returns>
        public XYZ Rotate(XYZ angle, AngleUnit angleUnit= AngleUnit.Degree)
        {
            double alphaX = angle.X;
            double alphaY = angle.Y;
            double alphaZ = angle.Z;

            return Rotate(alphaX, alphaY, alphaZ, angleUnit);
        }

        /// <summary>
        /// 坐标旋转。
        /// </summary>
        /// <param name="alphaX">绕 X 轴旋转的角度</param>
        /// <param name="alphaY">绕 Y 轴旋转的角度</param>
        /// <param name="alphaZ">绕 Z 轴旋转的角度</param>
        /// <returns></returns>
        public XYZ Rotate(double alphaX, double alphaY, double alphaZ, AngleUnit angleUnit = AngleUnit.Degree)
        {
            if (angleUnit == AngleUnit.Degree)
            {
                alphaX *= CoordConsts.DegToRadMultiplier;
                alphaY *= CoordConsts.DegToRadMultiplier;
                alphaZ *= CoordConsts.DegToRadMultiplier;
            }
            double x, y, z;
            // 在处理三维坐标旋转时，使用标准的数学公式是没有问题的。
            // 可是把二维坐标旋转调用三次，也可以实现三维坐标的旋转，并且有易读易懂，処理速度快的优点。
            //Z Axis Rotation
            double x2 = X * Math.Cos(alphaZ) + Y * Math.Sin(alphaZ);
            double y2 = -X * Math.Sin(alphaZ) + Y * Math.Cos(alphaZ);
            double z2 = Z;

            //Y Axis Rotation
            double z3 = z2 * Math.Cos(alphaY)+ x2 * Math.Sin(alphaY);
            double x3 = -z2 * Math.Sin(alphaY) + x2 * Math.Cos(alphaY);
            double y3 = y2;

            //X Axis Rotation
            y = y3 * Math.Cos(alphaX) + z3 * Math.Sin(alphaX);
            z = -y3 * Math.Sin(alphaX) + z3 * Math.Cos(alphaX);
            x = x3;
            return new XYZ(x, y, z);
        }
        #endregion

        #region operator
        /// <summary>
        /// 坐标缩放
        /// </summary>
        /// <param name="first"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static XYZ operator *(XYZ first, double num)
        {
            return new XYZ(first.X * num, first.Y * num, first.Z * num);
        }
        public static XYZ operator *(double num, XYZ first)
        {
            return new XYZ(first.X * num, first.Y * num, first.Z * num);
        }
        public static XYZ operator -(XYZ first)
        {
            return new XYZ(-first.X, -first.Y, -first.Z);
        }
        /// <summary>
        /// 坐标缩放
        /// </summary>
        /// <param name="first"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static XYZ operator /(XYZ first, double num)
        {
            return new XYZ(first.X / num, first.Y / num, first.Z / num);
        }
        /// <summary>
        /// 坐标平移
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static XYZ operator +(XYZ first, XYZ second)
        {
            return new XYZ(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
        }
        /// <summary>
        /// 坐标平移。
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static XYZ operator -(XYZ first, XYZ second)
        {
            return new XYZ(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
        }
        /// <summary>
        /// 叉乘
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static XYZ operator *(XYZ first, XYZ second)
        {
            return first.Cross(second);
        }
        #endregion
        
        #region property
        /// <summary>
        /// Z 分量
        /// </summary>
        public double Z { get; set; }

        #endregion

        #region equals

        /// <summary>
        /// 三个数字相等则为True。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            XYZ pt = obj as XYZ;
            if (pt == null) return false;

            return Equals(pt);
        }
        /// <summary>
        /// 相等否
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool Equals(XYZ pt)
        {
            if (X == pt.X && Y == pt.Y && pt.Z.Equals(Z))
                return true;

            return false;
        }
        /// <summary>
        /// 在容忍的范围内是否相等
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public bool Equals(XYZ pt, double tolerance)
        { 
            if (Math.Abs(X - pt.X) < tolerance
                && Math.Abs(Y - pt.Y) < tolerance
                && Math.Abs(Z - pt.Z) < tolerance)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return (int)(X + Y + Z);
        }
        #endregion

        /// <summary>
        /// 测站
        /// </summary>
        public string Site { get; set; }// 
         /// <summary>
         /// 深度克隆
         /// </summary>
         /// <returns></returns>
        public XYZ DeepClone()
        {
            return new XYZ(X, Y, Z);
        }
         
        /// <summary>
        /// 向量点乘
        /// returns the dot product of the two vectors
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public double Dot(XYZ right)
        {
            return base.Dot(right); 
        }

        /// <summary>
        /// 向量叉乘。
        /// retuns v1 x v2 , vector cross product
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public XYZ Cross(XYZ right)
        {
            double cp0 = this.Y * right.Z - this.Z * right.Y;
            double cp1 = this.Z * right.X - this.X * right.Z;
            double cp2 = this.X * right.Y - this.Y * right.X;
            XYZ cp = new XYZ(cp0, cp1, cp2);
            return cp;
        }

        const double MinDiffer =0.000000000000001;

        /// <summary>
        /// 单位向量
        /// </summary>
        /// <returns></returns>
        public XYZ UnitVector()
        { 
            double mag = this.Length;
            if (mag <= MinDiffer)   throw new Exception(" 长度太小了， Divide by Zero Error"); 
            XYZ retArg = new XYZ(this.X / mag, this.Y / mag, this.Z / mag);
            return retArg;
        }
        
        /// <summary>
        /// 点的融合。加权平均。
        /// </summary>
        /// <param name="pt"></param>
        public XYZ Amalgamation(XYZ pt)
        {
            double x = (X * Weight + pt.X * pt.Weight) / (Weight + pt.Weight);
            double y = (Y * Weight + pt.Y * pt.Weight) / (Weight + pt.Weight);
            double weight = (Weight + pt.Weight) / 2;
            XYZ coord = new XYZ(x, y);
            coord.Weight = weight;
            return coord;
        }


        #region distance
       
     

        /// <summary>
        /// 该点到任意两点线段的距离。
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public double Distance(XYZ pt1, XYZ pt2)
        {
            //首先判断是否在两点连线的垂直区域内。如果不是则直接返与最近的一个断点的距离，如果是则计算垂直距离。
            double differX = pt2.X - pt1.X;
            double differY = pt2.Y - pt1.Y;
            if (differY == 0)//与X轴平行
            {
                //直接比较 X 值 
                XYZ minPt;
                XYZ maxPt;
                if (pt1.X < pt2.X)
                {
                    minPt = pt1;
                    maxPt = pt2;
                }
                else
                {
                    minPt = pt2;
                    maxPt = pt1;
                }

                if (this.X < pt1.X)
                {
                    return this.Distance(pt1);
                }
                else if (this.X > pt2.X)
                {
                    return this.Distance(pt2);
                }
                else//在两点中间
                {
                    return Math.Abs(this.Y - pt1.Y);
                }
            }
            else
            {
                //回结果:
                //     角 θ，以弧度为单位，满足 -π≤θ≤π，且 tan(θ) = y / x，其中 (x, y) 是笛卡儿平面中的点。
                //请看下面： 
                //如果 (x, y) 在第 1 象限，则   0  < θ < π/2。
                //如果 (x, y) 在第 2 象限，则 π/2 < θ ≤π。
                //如果 (x, y) 在第 3 象限，则  -π < θ < -π/2。
                //如果 (x, y) 在第 4 象限，则-π/2 < θ < 0。
                double angle = Math.Atan2(differY, differX);
                //垂直角的斜率
                double vAngle = angle + CoordConsts.PI / 2.0;
                //直线 y = kx + b, b = y-  kx;
                double k = Math.Tan(vAngle);

                double b1 = pt1.Y - k * pt1.X;
                double b2 = pt2.Y - k * pt2.X;
                double bme = Y - k * X;

                XYZ minPt;
                XYZ maxPt;
                double minB, maxB;
                if (b1 < b2)
                {
                    minPt = pt1;
                    maxPt = pt2;
                    minB = b1;
                    maxB = b2;
                }
                else
                {
                    minPt = pt2;
                    maxPt = pt1;
                    minB = b2;
                    maxB = b1;
                }

                if (bme < minB)
                {
                    return this.Distance(minPt);
                }
                else if (bme > maxB)
                {
                    return this.Distance(maxPt);
                }
                else//在两点中间,
                {
                    return DistanceOfBeeline(this, pt1, pt2);
                }
            }
        }
        /// <summary>
        /// 与直线的距离（不是线段）。
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static double DistanceOfBeeline(XYZ xyz, XYZ pt1, XYZ pt2)
        {
            double tempUpper = Math.Abs((xyz.X - pt1.X) * (pt2.Y - pt1.Y) - (xyz.Y - pt1.Y) * (pt2.X - pt1.X));
            double tempDown = Math.Sqrt((pt1.X - pt2.X) * (pt1.X - pt2.X) + (pt1.Y - pt2.Y) * (pt1.Y - pt2.Y));
            if (tempDown == 0) return 0;
            return tempUpper / tempDown;
        }
        /// <summary>
        /// 两点的欧氏距离。
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        //public double Distance(Coordinate coord)
        //{
        //    if (Dimension == 2)
        //    {
        //        double dis = Math.Sqrt((X - coord.X) * (X - coord.X) + (Y - coord.Y) * (Y - coord.Y));
        //        return dis;
        //    }
        //    else
        //        return Math.Sqrt((X - coord.X) * (X - coord.X) + (Y - coord.Y) * (Y - coord.Y) + (Z - coord.Z) * (Z - coord.Z));
        //}
        #endregion

        /// <summary>
        /// 点在直线上的垂点。
        /// </summary>
        /// <param name="one"></param>
        /// <param name="another"></param>
        /// <param name="outer"></param>
        /// <returns></returns>
        public static XYZ GetVerticalPoint(XYZ one, XYZ another, XYZ outer)
        {
            //首先判断是否在两点连线的垂直区域内。如果不是则直接返与最近的一个断点的距离，如果是则计算垂直距离。
            double differX = another.X - one.X;
            double differY = another.Y - one.Y;
            if (differY == 0)//与X轴平行
            {
                return new XYZ(outer.X, one.Y);
            }
            else if (differX == 0)//与Y轴平行
            {
                return new XYZ(one.X, outer.Y);
            }
            else
            {
                double kOrigin = differY / differX;
                double bOrigin = one.Y - kOrigin * one.X;

                double angle = Math.Atan2(differY, differX);
                //垂直角的斜率
                double vAngle = angle + CoordConsts.PI / 2.0;
                //直线 y = kx + b, b = y-  kx;
                double k = Math.Tan(vAngle);
                double bme = outer.Y - k * outer.X;

                //
                double y = (bOrigin + bme * (kOrigin * kOrigin)) / (1 + kOrigin * kOrigin);
                double x = (y - bOrigin) / kOrigin;
                return new XYZ(x, y);
            }
        }
        public XYZ Cos { get { return new XYZ(CosX, CosY, CosZ); } }
        /// <summary>
        /// X / Length
        /// </summary>
        public double CosX { get { return X / this.Length; } }
        /// <summary>
        /// Y / Length
        /// </summary>
        public double CosY { get { return Y / this.Length; } }
        /// <summary>
        /// Z / Length
        /// </summary>
        public double CosZ { get { return Z / this.Length; } }
        /// <summary>
        /// 长度
        /// </summary>
        public double Length { get { return this.Norm;} }

        /// <summary>
        /// 按权拟合。
        /// </summary>
        /// <param name="xyzA"></param>
        /// <param name="weightA"></param>
        /// <param name="xyzB"></param>
        /// <param name="weightB"></param>
        /// <returns></returns>
        public static XYZ GetXYZ(XYZ xyzA, double weightA, XYZ xyzB, double weightB)
        {
            return new XYZ()
            {
                X = (xyzA.X * weightA + xyzB.X * weightB) / (weightA + weightB),
                Y = (xyzA.Y * weightA + xyzB.Y * weightB) / (weightA + weightB),
                Z = (xyzA.Z * weightA + xyzB.Z * weightB) / (weightA + weightB)
            };
        }
        /// <summary>
        /// 对象行
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetObjectRow()
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("X", X);
            row.Add("Y", Y);
            row.Add("Z", Z);
            return row;
        }
        /// <summary>
        /// new XYZ(X, Y, Z);
        /// </summary>
        /// <returns></returns>
        public XYZ DeepCone()
        {
            return new XYZ(X, Y, Z);
        }
        public LonLat GetLonLat() { return new LonLat(X, Y); }

        #region equals
        //public override bool Equals(object obj)
        //{
        //    if (!(obj is Coordinate)) return false;
        //    //if (base.Equals(obj)) return true;

        //    Coordinate pt = (Coordinate)obj;
        //    if (X == pt.X && Y == pt.Y && pt.Z.Equals(Z))
        //        return true;
        //    return false;
        //}

        //public override int GetHashCode()
        //{
        //    return (int)(X + Y + Z);
        //}
        #endregion

        /// <summary>
        /// 解析三维浮点数数组
        /// </summary>
        /// <param name="array">三维浮点数数组</param>
        /// <returns></returns>
        public static XYZ Parse(IEnumerable<double> array, int formIndex = 0)
        {
            XYZ xyz = new XYZ();
            int i = formIndex;
            foreach (var item in array)
            {
                if (i == formIndex) { xyz.X = item; }
                if (i == formIndex + 1) { xyz.Y = item; }
                if (i == formIndex + 2) { xyz.Z = item; break; }
                i++;
            }
            return xyz;
        }
        /// <summary>
        /// 解析三维浮点数数组
        /// </summary>
        /// <param name="array">三维浮点数数组</param>
        /// <returns></returns>
        public static XYZ Parse(double[] array) { return new XYZ(array[0], array[1], array[2]); }
        /// <summary>
        /// 解析字符串，可以解析空格、分号、换行符、回车符、Tab为分隔符的字符串
        /// </summary>
        /// <param name="toString"></param>
        /// <returns></returns>
        public static new XYZ Parse(string toString) { return Parse(toString, new char[] { ',', ';',' ','\t','\n','\r'});  }

        /// <summary>
        /// (x,y) (x,y,z) (x y z) x y z
        /// </summary>
        /// <param name="toString"></param>
        /// <param name="separaters"></param>
        /// <returns></returns>
        public static XYZ Parse(string toString, char[] separaters)
        { 
            toString = toString.Replace("(", "").Replace(")", "");
            string[] strs = toString.Split(separaters, StringSplitOptions.RemoveEmptyEntries);
            return Parse(strs);
        }
        /// <summary>
        /// 解析字符串数组，支持二维和三维
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static XYZ Parse(string[] strs)
        {
            double lon = double.Parse(strs[0]);
            double lat = double.Parse(strs[1]);
            double z = 0;
            if (strs.Length > 2) z = double.Parse(strs[2]);
            return new XYZ(lon, lat, z);
        }
        /// <summary>
        /// 解析坐标组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="outerSeparaters">坐标之间的分隔号</param>
        /// <param name="innerSeparaters">坐标内部分隔号</param>
        /// <returns></returns>
        public static List<XYZ> ParseList(string str, char[] outerSeparaters, char[] innerSeparaters)
        {
            List<XYZ> list = new List<XYZ>();
            string[] strs = str.Split(outerSeparaters, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in strs)
            {
                list.Add(Parse(s));
            }
            return list;
        }
        /// <summary>
        /// 转换为数组
        /// </summary>
        /// <returns></returns>
        public double[] ToArray() { return new double[] { X, Y, Z }; }
        /// <summary>
        /// 解析坐标组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="outerSeparater">坐标之间的分隔号</param>
        /// <param name="innerSeparater">坐标内部分隔号</param>
        /// <returns></returns>
        public static List<XYZ> ParseList(string str, char outerSeparater, char innerSeparater)
        {
            return ParseList(str, new char[] { outerSeparater }, new char[] { innerSeparater });
        }
        /// <summary>
        /// "X\tY\tZ"
        /// </summary>
        /// <returns></returns>
        public override string GetTabTitles()
        {
            return "X\tY\tZ";
        }

        /// <summary>
        /// Z.ZZZZ X.XXXX Y.YYYY
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static XYZ ParseSnx(string str)
        {
            string[] strs = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new XYZ(double.Parse(strs[1]), double.Parse(strs[2]), double.Parse(strs[0]));
        }
        /// <summary>
        /// Z.ZZZZ X.XXXX Y.YYYY
        /// </summary>
        /// <returns></returns>
        public string ToSnxString()
        {
            return Z.ToString("0.0000") + " " + X.ToString("0.0000") + " " + Y.ToString("0.0000");
        }

        #region 坐标的旋转
        /// <summary>
        /// 围绕 X 坐标的旋转，X坐标不变。Computes rotation about axis X
        /// </summary>
        /// <param name="Triple"></param>
        /// <param name="alphaX">angle to rotate, in degrees</param>
        /// <returns></returns>
        public static XYZ RotateX(XYZ Triple, double alphaX, AngleUnit unit = AngleUnit.Degree)
        {
            //Deg to  Rad
            if (unit == AngleUnit.Degree)
            {
                alphaX *= CoordConsts.DegToRadMultiplier;
            }            
           

            double x, y, z;
            // 在处理三维坐标旋转时，使用标准的数学公式是没有问题的。
            // 可是把二维坐标旋转调用三次，也可以实现三维坐标的旋转，并且有易读易懂，処理速度快的优点。        
            //X Axis Rotation
            x = Triple.X;
            y = Triple.Y * Math.Cos(alphaX) + Triple.Z * Math.Sin(alphaX);
            z = -Triple.Y * Math.Sin(alphaX) + Triple.Z * Math.Cos(alphaX);
            return new XYZ(x, y, z);
        }

        /// <summary>
        /// 围绕 Y 坐标的旋转，Y坐标不变。Computes rotation about axis Y
        /// </summary>
        /// <param name="Triple"></param>
        /// <param name="alphaY">angle to rotate, in degrees</param>
        /// <returns></returns>
        public static XYZ RotateY(XYZ Triple, double alphaY, AngleUnit unit = AngleUnit.Degree)
        {
            //Deg to  Rad
            if (unit == AngleUnit.Degree)
            {
                alphaY *= CoordConsts.DegToRadMultiplier;
            }            

            double x, y, z;
            // 在处理三维坐标旋转时，使用标准的数学公式是没有问题的。
            // 可是把二维坐标旋转调用三次，也可以实现三维坐标的旋转，并且有易读易懂，処理速度快的优点。        
            //X Axis Rotation
            x = Triple.X * Math.Cos(alphaY) - Math.Sin(alphaY) * Triple.Z;
            y = Triple.Y;
            z = Triple.X * Math.Sin(alphaY) + Triple.Z * Math.Cos(alphaY);
            return new XYZ(x, y, z);
        }
        /// <summary>
        /// 围绕 Z 坐标的旋转，Z坐标不变。Computes rotation about axis Z
        /// </summary>
        /// <param name="Triple"></param>
        /// <param name="alphaZ">angle to rotate, in degrees</param>
        /// <returns></returns>
        public static XYZ RotateZ(XYZ Triple, double alphaZ, AngleUnit unit = AngleUnit.Degree)
        {
            //Deg to  Rad
            if (unit == AngleUnit.Degree)
            {
                alphaZ *= CoordConsts.DegToRadMultiplier;
            }

            double x, y, z;
            // 在处理三维坐标旋转时，使用标准的数学公式是没有问题的。
            // 可是把二维坐标旋转调用三次，也可以实现三维坐标的旋转，并且有易读易懂，処理速度快的优点。        
            //X Axis Rotation
            x = Triple.X * Math.Cos(alphaZ) + Math.Sin(alphaZ) * Triple.Y;
            y = -Math.Sin(alphaZ) * Triple.X + Math.Cos(alphaZ) * Triple.Y;
            z = Triple.Z;
            return new XYZ(x, y, z);
        }
        #endregion
        /// <summary>
        /// 是否在原点。
        /// </summary>
        public bool IsZero { get { return this.Equals(Zero); } }
         static XYZ zero = new XYZ();
        /// <summary>
        /// （0,0,0）
        /// </summary>
        public static XYZ Zero { get { return zero; } }
        /// <summary>
        /// 最大值
        /// </summary>
        public static XYZ MaxValue => new XYZ(double.MaxValue, double.MaxValue, double.MaxValue);

        /// <summary>
        /// 字符输出
        /// </summary>
        /// <param name="format"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public override string ToString(string format, string spliter = ", ")
        {
            return X.ToString(format) + spliter + Y.ToString(format) + spliter + Z.ToString(format);
        }
        /// <summary>
        /// 字符串。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.Count; i++)
            {
                if (i != 0) sb.Append(", "); 
                sb.Append(String.Format(new NumeralFormatProvider(), "{0:6.4}", this[i]));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string GetTabValues()
        {
            return X + "\t" +Y + "\t" + Z;
        } 
        /// <summary>
        /// 遍历
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<double> GetEnumerator()
        {
            return (new List<double> { X, Y, Z }).GetEnumerator();
        }
        /// <summary>
        /// 坐标是否为 0，或null。
        /// </summary>
        /// <param name="xyz"></param>
        /// <returns></returns>
        static public bool IsZeroOrEmpty(XYZ xyz)
        {
            if (xyz == null) { return true; }
            return (xyz.IsZero);
        }
        /// <summary>
        /// 坐标是否有效, 
        /// </summary>
        /// <param name="xyz"></param>
        /// <returns></returns>
        static public bool IsValueValid(XYZ xyz)
        {
            if (xyz == null) { return false; }
            return DoubleUtil.IsValid(xyz.X) &&  DoubleUtil.IsValid(xyz.Y) &&  DoubleUtil.IsValid(xyz.Z);
        }
        /// <summary>
        /// RMS 相加
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static XYZ RmsPlus(XYZ left, XYZ right)
        {
            var x = DoubleUtil.RmsPlus(left.X, right.X);
            var y = DoubleUtil.RmsPlus(left.Y, right.Y);
            var z = DoubleUtil.RmsPlus(left.Z, right.Z);
            return new XYZ(x, y, z);
        }

        /// <summary>
        /// 比较圆心距离。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int CompareTo(object obj)
        {
            if (obj is XYZ)
            {
                XYZ xy = obj as XYZ;
                return this.Length.CompareTo(xy.Length);
            }
            throw new ArgumentException("输入参数应该为XYZ类型");
        }
    }
}