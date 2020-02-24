//2014.05.22�� Cui Yang, ������һЩ����

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// X,Y,Z ��ά���ꡣ��ά�ռ�� 3 ��˫���ȷ�����
    /// Ҳ��������
    /// </summary>
    [Serializable]
    public class XYZ : XY, IXYZ, IEquatable<XYZ>, IObjectRow
    {
        #region ���캯��
        /// <summary>
        /// ��һ����ά�����ʼ��
        /// </summary>
        /// <param name="xy"></param>
        public XYZ(XY xy) : base(xy.X, xy.Y) { }//this.SetDimension(3); } 

        /// <summary>
        /// ��õĹ��캯�����ֱ��ʼ������������
        /// </summary>
        /// <param name="x">X �������</param>
        /// <param name="y">Y �������</param>
        /// <param name="z">Z �������</param>
        public XYZ(double x =0, double y =0, double z = 0)
            : base(x, y)
        { 
           // this.SetDimension(3);
            this.Z = z;
        }
        /// <summary>
        /// ��һ������Ԫ�ص�һά�����ʼ��
        /// </summary>
        /// <param name="array">����Ԫ�ص�һά����</param>
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
        /// ��һ���ŷʽ����
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
        /// ����
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
        /// ����
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
        /// �Ƿ�С
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool IsHasSmallerThan(double val)
        {
            return X < val || Y < val || Z < val;
        }
        /// <summary>
        /// �Ƿ���Ч
        /// </summary>
        public override bool IsValid { get { return Geo.Utils.DoubleUtil.IsValid(Z) && base.IsValid; } }
        /// <summary>
        /// ������ģ/����/����/Ԫ��ƽ���͵ĸ�
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
        /// ����������
        /// </summary>
        public Xyz Xyz => new Xyz(X, Y, Z);
        /// <summary>
        /// Calculates the magnitude of the vector.
        /// ���ȣ�����
        /// </summary>
        /// <returns>The vector magnitude.</returns>
        public double Magnitude()
        {
            return this.Norm;
        }
        /// <summary>
        /// ��������ʸ���ĽǶȡ� ��λĬ��Ϊ��
        /// </summary>
        /// <param name="vec">The second vector.</param>
        /// <param name="unit">��λ</param>
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
        /// ������ת�Ƕȣ���λ����
        /// </summary>
        /// <param name="oldXyz"></param>
        /// <param name="newXyz"></param>
        /// <returns></returns>
        public static double GetRotateXAngle(XYZ oldXyz, XYZ newXyz)
        {
            return GetIncludedAngle(new XY(oldXyz.Y, oldXyz.Z), new XY(newXyz.Y, newXyz.Z));
        }
        /// <summary>
        /// ������ת�Ƕȣ���λ����
        /// </summary>
        /// <param name="oldXyz"></param>
        /// <param name="newXyz"></param>
        /// <returns></returns>
        public static double GetRotateYAngle(XYZ oldXyz, XYZ newXyz)
        {
            return GetIncludedAngle(new XY(oldXyz.Z, oldXyz.X), new XY(newXyz.Z, newXyz.X));
        }
        /// <summary>
        /// ������ת�Ƕȣ���λ����
        /// </summary>
        /// <param name="oldXyz"></param>
        /// <param name="newXyz"></param>
        /// <returns></returns>
        public static double GetRotateZAngle(XYZ oldXyz, XYZ newXyz)
        {
            return GetIncludedAngle(oldXyz, newXyz);
        }
        /// <summary>
        /// �������������н�
        /// </summary>
        /// <param name="vector"></param> 
        /// <returns></returns>
        public  double GetIncludedAngle(XYZ vector) { return GetIncludedAngle(this, vector); }
        /// <summary>
        /// �������������н�
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        public static double GetIncludedAngle(XYZ vectorA, XYZ vectorB)
        {
            double  cosAngle = vectorA.Dot(vectorB) / (vectorA.Length * vectorB.Length);
            return Math.Acos(cosAngle);
        }
        #region ������ת
        /// <summary>
        /// ������ת��
        /// </summary>
        /// <param name="angle">�����������ת�Ƕ�</param>
        /// <returns></returns>
        public XYZ Rotate(XYZ angle, AngleUnit angleUnit= AngleUnit.Degree)
        {
            double alphaX = angle.X;
            double alphaY = angle.Y;
            double alphaZ = angle.Z;

            return Rotate(alphaX, alphaY, alphaZ, angleUnit);
        }

        /// <summary>
        /// ������ת��
        /// </summary>
        /// <param name="alphaX">�� X ����ת�ĽǶ�</param>
        /// <param name="alphaY">�� Y ����ת�ĽǶ�</param>
        /// <param name="alphaZ">�� Z ����ת�ĽǶ�</param>
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
            // �ڴ�����ά������תʱ��ʹ�ñ�׼����ѧ��ʽ��û������ġ�
            // ���ǰѶ�ά������ת�������Σ�Ҳ����ʵ����ά�������ת���������׶��׶����I���ٶȿ���ŵ㡣
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
        /// ��������
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
        /// ��������
        /// </summary>
        /// <param name="first"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static XYZ operator /(XYZ first, double num)
        {
            return new XYZ(first.X / num, first.Y / num, first.Z / num);
        }
        /// <summary>
        /// ����ƽ��
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static XYZ operator +(XYZ first, XYZ second)
        {
            return new XYZ(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
        }
        /// <summary>
        /// ����ƽ�ơ�
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static XYZ operator -(XYZ first, XYZ second)
        {
            return new XYZ(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
        }
        /// <summary>
        /// ���
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
        /// Z ����
        /// </summary>
        public double Z { get; set; }

        #endregion

        #region equals

        /// <summary>
        /// �������������ΪTrue��
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
        /// ��ȷ�
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
        /// �����̵ķ�Χ���Ƿ����
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
        /// ��վ
        /// </summary>
        public string Site { get; set; }// 
         /// <summary>
         /// ��ȿ�¡
         /// </summary>
         /// <returns></returns>
        public XYZ DeepClone()
        {
            return new XYZ(X, Y, Z);
        }
         
        /// <summary>
        /// �������
        /// returns the dot product of the two vectors
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public double Dot(XYZ right)
        {
            return base.Dot(right); 
        }

        /// <summary>
        /// ������ˡ�
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
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        public XYZ UnitVector()
        { 
            double mag = this.Length;
            if (mag <= MinDiffer)   throw new Exception(" ����̫С�ˣ� Divide by Zero Error"); 
            XYZ retArg = new XYZ(this.X / mag, this.Y / mag, this.Z / mag);
            return retArg;
        }
        
        /// <summary>
        /// ����ںϡ���Ȩƽ����
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
        /// �õ㵽���������߶εľ��롣
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public double Distance(XYZ pt1, XYZ pt2)
        {
            //�����ж��Ƿ����������ߵĴ�ֱ�����ڡ����������ֱ�ӷ��������һ���ϵ�ľ��룬���������㴹ֱ���롣
            double differX = pt2.X - pt1.X;
            double differY = pt2.Y - pt1.Y;
            if (differY == 0)//��X��ƽ��
            {
                //ֱ�ӱȽ� X ֵ 
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
                else//�������м�
                {
                    return Math.Abs(this.Y - pt1.Y);
                }
            }
            else
            {
                //�ؽ��:
                //     �� �ȣ��Ի���Ϊ��λ������ -�СܦȡܦУ��� tan(��) = y / x������ (x, y) �ǵѿ���ƽ���еĵ㡣
                //�뿴���棺 
                //��� (x, y) �ڵ� 1 ���ޣ���   0  < �� < ��/2��
                //��� (x, y) �ڵ� 2 ���ޣ��� ��/2 < �� �ܦС�
                //��� (x, y) �ڵ� 3 ���ޣ���  -�� < �� < -��/2��
                //��� (x, y) �ڵ� 4 ���ޣ���-��/2 < �� < 0��
                double angle = Math.Atan2(differY, differX);
                //��ֱ�ǵ�б��
                double vAngle = angle + CoordConsts.PI / 2.0;
                //ֱ�� y = kx + b, b = y-  kx;
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
                else//�������м�,
                {
                    return DistanceOfBeeline(this, pt1, pt2);
                }
            }
        }
        /// <summary>
        /// ��ֱ�ߵľ��루�����߶Σ���
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
        /// �����ŷ�Ͼ��롣
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
        /// ����ֱ���ϵĴ��㡣
        /// </summary>
        /// <param name="one"></param>
        /// <param name="another"></param>
        /// <param name="outer"></param>
        /// <returns></returns>
        public static XYZ GetVerticalPoint(XYZ one, XYZ another, XYZ outer)
        {
            //�����ж��Ƿ����������ߵĴ�ֱ�����ڡ����������ֱ�ӷ��������һ���ϵ�ľ��룬���������㴹ֱ���롣
            double differX = another.X - one.X;
            double differY = another.Y - one.Y;
            if (differY == 0)//��X��ƽ��
            {
                return new XYZ(outer.X, one.Y);
            }
            else if (differX == 0)//��Y��ƽ��
            {
                return new XYZ(one.X, outer.Y);
            }
            else
            {
                double kOrigin = differY / differX;
                double bOrigin = one.Y - kOrigin * one.X;

                double angle = Math.Atan2(differY, differX);
                //��ֱ�ǵ�б��
                double vAngle = angle + CoordConsts.PI / 2.0;
                //ֱ�� y = kx + b, b = y-  kx;
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
        /// ����
        /// </summary>
        public double Length { get { return this.Norm;} }

        /// <summary>
        /// ��Ȩ��ϡ�
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
        /// ������
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
        /// ������ά����������
        /// </summary>
        /// <param name="array">��ά����������</param>
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
        /// ������ά����������
        /// </summary>
        /// <param name="array">��ά����������</param>
        /// <returns></returns>
        public static XYZ Parse(double[] array) { return new XYZ(array[0], array[1], array[2]); }
        /// <summary>
        /// �����ַ��������Խ����ո񡢷ֺš����з����س�����TabΪ�ָ������ַ���
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
        /// �����ַ������飬֧�ֶ�ά����ά
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
        /// ����������
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="outerSeparaters">����֮��ķָ���</param>
        /// <param name="innerSeparaters">�����ڲ��ָ���</param>
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
        /// ת��Ϊ����
        /// </summary>
        /// <returns></returns>
        public double[] ToArray() { return new double[] { X, Y, Z }; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="outerSeparater">����֮��ķָ���</param>
        /// <param name="innerSeparater">�����ڲ��ָ���</param>
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

        #region �������ת
        /// <summary>
        /// Χ�� X �������ת��X���겻�䡣Computes rotation about axis X
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
            // �ڴ�����ά������תʱ��ʹ�ñ�׼����ѧ��ʽ��û������ġ�
            // ���ǰѶ�ά������ת�������Σ�Ҳ����ʵ����ά�������ת���������׶��׶����I���ٶȿ���ŵ㡣        
            //X Axis Rotation
            x = Triple.X;
            y = Triple.Y * Math.Cos(alphaX) + Triple.Z * Math.Sin(alphaX);
            z = -Triple.Y * Math.Sin(alphaX) + Triple.Z * Math.Cos(alphaX);
            return new XYZ(x, y, z);
        }

        /// <summary>
        /// Χ�� Y �������ת��Y���겻�䡣Computes rotation about axis Y
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
            // �ڴ�����ά������תʱ��ʹ�ñ�׼����ѧ��ʽ��û������ġ�
            // ���ǰѶ�ά������ת�������Σ�Ҳ����ʵ����ά�������ת���������׶��׶����I���ٶȿ���ŵ㡣        
            //X Axis Rotation
            x = Triple.X * Math.Cos(alphaY) - Math.Sin(alphaY) * Triple.Z;
            y = Triple.Y;
            z = Triple.X * Math.Sin(alphaY) + Triple.Z * Math.Cos(alphaY);
            return new XYZ(x, y, z);
        }
        /// <summary>
        /// Χ�� Z �������ת��Z���겻�䡣Computes rotation about axis Z
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
            // �ڴ�����ά������תʱ��ʹ�ñ�׼����ѧ��ʽ��û������ġ�
            // ���ǰѶ�ά������ת�������Σ�Ҳ����ʵ����ά�������ת���������׶��׶����I���ٶȿ���ŵ㡣        
            //X Axis Rotation
            x = Triple.X * Math.Cos(alphaZ) + Math.Sin(alphaZ) * Triple.Y;
            y = -Math.Sin(alphaZ) * Triple.X + Math.Cos(alphaZ) * Triple.Y;
            z = Triple.Z;
            return new XYZ(x, y, z);
        }
        #endregion
        /// <summary>
        /// �Ƿ���ԭ�㡣
        /// </summary>
        public bool IsZero { get { return this.Equals(Zero); } }
         static XYZ zero = new XYZ();
        /// <summary>
        /// ��0,0,0��
        /// </summary>
        public static XYZ Zero { get { return zero; } }
        /// <summary>
        /// ���ֵ
        /// </summary>
        public static XYZ MaxValue => new XYZ(double.MaxValue, double.MaxValue, double.MaxValue);

        /// <summary>
        /// �ַ����
        /// </summary>
        /// <param name="format"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public override string ToString(string format, string spliter = ", ")
        {
            return X.ToString(format) + spliter + Y.ToString(format) + spliter + Z.ToString(format);
        }
        /// <summary>
        /// �ַ�����
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
        /// ��ʾ
        /// </summary>
        /// <returns></returns>
        public override string GetTabValues()
        {
            return X + "\t" +Y + "\t" + Z;
        } 
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<double> GetEnumerator()
        {
            return (new List<double> { X, Y, Z }).GetEnumerator();
        }
        /// <summary>
        /// �����Ƿ�Ϊ 0����null��
        /// </summary>
        /// <param name="xyz"></param>
        /// <returns></returns>
        static public bool IsZeroOrEmpty(XYZ xyz)
        {
            if (xyz == null) { return true; }
            return (xyz.IsZero);
        }
        /// <summary>
        /// �����Ƿ���Ч, 
        /// </summary>
        /// <param name="xyz"></param>
        /// <returns></returns>
        static public bool IsValueValid(XYZ xyz)
        {
            if (xyz == null) { return false; }
            return DoubleUtil.IsValid(xyz.X) &&  DoubleUtil.IsValid(xyz.Y) &&  DoubleUtil.IsValid(xyz.Z);
        }
        /// <summary>
        /// RMS ���
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
        /// �Ƚ�Բ�ľ��롣
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
            throw new ArgumentException("�������Ӧ��ΪXYZ����");
        }
    }
}