//2014.11.16, czs, edit in namu, ����

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;


namespace Geo.Coordinates
{
    /// <summary>
    /// X��Y������ʶ�Ķ�ά���ꡣ
    /// �ѿ������꣬ƽ�����ꡣ��õ����ꡣ
    /// </summary>
    [Serializable]
    public class XY :  Geo.Coordinates.IXY, ICloneable,IToTabRow, INumeralIndexing, IEnumerable<double> // TwoDimVector,
    {
        /// <summary>
        /// �������ʼ����ֻȡǰ������
        /// </summary>
        /// <param name="array">����</param>
        public XY(double[] array) : this(array[0], array[1]) { }

        /// <summary>
        /// Ĭ�Ϲ��캯����
        /// </summary>
        public XY()//:base(2) 
        { 
        }
        /// <summary>
        /// ���캯������XY�������ำֵ��
        /// </summary>
        /// <param name="x">X ����</param>
        /// <param name="y">Y ����</param>
        public XY(double x, double y)
        // :base(x, y)
        {
            this.X = x; this.Y = y;
        }

        #region Property
        /// <summary>
        /// X ����
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y ����
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// X��������������
        /// </summary>
        public int XInt
        {
            get { return (int)X; }
            set { X = value; }
        }
        /// <summary>
        /// Y��������������
        /// </summary>
        public int YInt
        {
            get { return (int)Y; }
            set { Y = value; }
        }
        /// <summary>
        /// X������������������
        /// </summary>
        public int XRoundInt { get { return (int)Math.Round(X); } }
        /// <summary>
        /// Y������������������
        /// </summary>
        public int YRoundInt    {  get { return (int)Math.Round(Y); }    }

        public Object Tag { get; set; }
        #endregion

        #region ����

        /// <summary>
        /// ����X�ᷭת�������ֵ��
        /// </summary>
        /// <returns></returns>
        public XY GetXInverted() { return new XY(-X, Y); }
        /// <summary>
        /// ����Y�ᷭת�������ֵ��
        /// </summary>
        /// <returns></returns>
        public XY GetYInverted() { return new XY(X, -Y); }
        /// <summary>
        /// ��ԭ��ľ���뾶��
        /// </summary>
        /// <returns></returns>
        public virtual double Radius() { return this.Norm; }

        /// <summary>
        /// ������ģ/����/����/Ԫ��ƽ���͵ĸ�
        /// </summary>
        public virtual double Norm
        {
            get
            {
                double norm = 0;
                norm += X * X;
                norm += Y * Y; 
                return Math.Sqrt(norm);
            }
        }
        /// <summary>
        /// ŷʽ���롣
        /// </summary>
        /// <param name="xy">��һ������</param>
        /// <returns></returns>
        public double Distance(XY xy) { return (this - xy).Norm; }
        public double Distance(IXY xy) { return (this - new XY( xy.X, xy.Y)).Norm; }

        /// <summary>
        /// ֵ�Ƿ�ȫΪ 0.
        /// </summary>
        public bool IsZero { get { return X == 0 && Y == 0; } }
        /// <summary>
        /// ������ת����ʱ��Ϊ����
        /// </summary>
        /// <param name="angle">��ת�Ƕ�</param>
        /// <returns></returns>
        public virtual XY Rotate(double angle, AngleUnit unit = AngleUnit.Degree)
        {
            double angleRad = angle;
            if (unit ==  AngleUnit.Degree) 
                angleRad *= CoordConsts.DegToRadMultiplier;
            return RotateRad(angleRad);
        }
        public virtual XY Rotate(TwoDimVector centerXy, double angleDeg)
        {
            double angleRad = angleDeg * CoordConsts.DegToRadMultiplier;
            return RotateRad(centerXy, angleRad);
        }
        /// <summary>
        /// ������ת����ʱ��Ϊ����
        /// </summary>
        /// <param name="angleRad"></param>
        /// <returns></returns>
        public XY RotateRad(double angleRad)
        {
            double x2 = X * Math.Cos(angleRad) + Y * Math.Sin(angleRad);
            double y2 = -X * Math.Sin(angleRad) + Y * Math.Cos(angleRad);
            return new XY(x2, y2);
        }

        /// <summary>
        /// ������ת����ʱ��Ϊ����
        /// </summary>
        /// <param name="centerXy"></param>
        /// <param name="angleRad"></param>
        /// <returns></returns>
        public XY RotateRad(TwoDimVector centerXy, double angleRad)
        {
            double differX = X - centerXy[0];
            double differY = Y - centerXy[1];

            double x2 = differX * Math.Cos(angleRad) + differY * Math.Sin(angleRad);
            double y2 = -differX * Math.Sin(angleRad) + differY * Math.Cos(angleRad);
            return new XY(x2 + centerXy[0], y2 + centerXy[1]);
        }
        /// <summary>
        /// ����ֱ������ϵ�����շ������ӣ�YΪ������XΪ������
        /// </summary>
        /// <param name="adder">������</param>
        /// <param name="direction">����</param>
        /// <returns></returns>
        public XY GetDirectionIncrease(double adder, Direction direction)
        {
            switch (direction)
            {
                case  Direction.East:
                    return this + new XY(adder, 0);
                case Direction.SouthEast:
                    return this + new XY(adder, -adder);
                case Direction.South:
                    return this + new XY(0, -adder);
                case Direction.SouthWest:
                    return this + new XY(-adder, -adder);
                case Direction.West:
                    return this + new XY(0, -adder);
                case Direction.NorthWest:
                    return this + new XY(-adder, adder);
                case Direction.North:
                    return this + new XY(0, adder);
                case Direction.NorthEast:
                    return this + new XY(adder, adder);
                default: return this;
            }
        }
        /// <summary>
        /// ����һ��ֵһ���Ķ���
        /// </summary>
        /// <returns></returns>
        public new object Clone()
        {
            return new XY(this.X, this.Y);
        }

        /// <summary>
        /// �Ƚ�Բ�ľ��롣
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int CompareTo(object obj)
        {
            if (obj is XY)
            {
                XY xy = obj as XY;
                return Radius().CompareTo(xy.Radius());
            }
            throw new ArgumentException("�������Ӧ��ΪXY����");
        }

        /// <summary>
        /// ����ڱ�����Ϊԭ������ޡ��� 1 ��ʼ��
        /// </summary>
        /// <param name="other">��һ������</param>
        /// <returns></returns>
        //public int GetQuadrant(XY other)
        //{
        //    return (other - this).Quadrant;
        //}
      
        #endregion

        #region operator
        public static XY operator *(XY first, double num)   {   return new XY(first.X * num, first.Y * num);    }
        public static XY operator /(XY first, double num)   {  return new XY(first.X / num, first.Y / num);   }
        public static XY operator +(XY first, XY second)   {   return new XY(first.X + second.X, first.Y + second.Y);   }
        public static XY operator -(XY first, XY second)   {   return new XY(first.X - second.X, first.Y - second.Y); }
        #endregion
        /// <summary>
        /// �������������нǣ�����ת�Ƕȣ���λ����
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        public static double GetIncludedAngle(XY vectorA, XY vectorB)
        {
            double cosAngle = vectorA.Dot(vectorB) / (vectorB.Radius() * vectorA.Radius());
            double cosAngle2 = (vectorB.X * vectorA.X + vectorB.Y * vectorA.Y ) / (vectorB.X * vectorB.X + vectorB.Y * vectorB.Y);
            var angle = Math.Acos(cosAngle);

            return angle;
            double sinAngle = (vectorB.Y * vectorA.X - vectorB.X * vectorA.Y) / (vectorB.X * vectorB.X + vectorB.Y * vectorB.Y);
            var angle2 = Math.Asin(sinAngle);

            return angle2;
        }
        #region overrides
        public override bool Equals(object obj)
        {
            XY xy = obj as XY;
            if (xy == null) return false;

            if (xy.X == X && xy.Y == Y) return true;

            return false;
        }
        public override int GetHashCode()
        {
            return (int)(X * 37 + Y * 31);
        }
        /// <summary>
        /// Parse the string like (X,Y)
        /// </summary>
        /// <param name="toString"></param>
        /// <returns></returns>
        public static XY Parse(string toString)
        {
            toString = toString.Replace("(", "").Replace(")", "");
            string[] strs = toString.Split(new char[]{' ',',','\t'}, StringSplitOptions.RemoveEmptyEntries);
            double lon = double.Parse(strs[0]);
            double lat = double.Parse(strs[1]);
            return new XY(lon, lat);
        }
        #endregion
        /// <summary>
        /// �����ĵ��/������
        /// </summary>
        /// <param name="right">��һ������</param>
        public double Dot(XY right)
        {
            double reslult = 0;
            int length = this.Count;
            for (int i = 0; i < length; i++)
            {
                reslult += this[i] * right[i];
            }
            return reslult;
        }
        public virtual double this[int i]
        {
            get { if (i == 0) return X; return Y; }
            set { if (i == 0)   X = value; else  Y = value; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public virtual int Count { get { return 2; } }

        public double Weight { get; set; }
        /// <summary>
        /// �ַ���
        /// </summary>
        /// <param name="format"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public virtual string ToString(string format, string spliter = ", ")
        {
            return X .ToString(format)+ spliter + Y.ToString(format);
        }
        /// <summary>
        /// �ַ���
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X + ", " +  Y;
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < this.Count; i++)
            //{
            //    if (i != 0) sb.Append(", ");
            ////    sb.Append(String.Format(new NumeralFormatProvider(), "{0:6.4}", this[i]));
            ////}
            //return sb.ToString();
        }
        /// <summary>
        /// "X\tY\tZ"
        /// </summary>
        /// <returns></returns>
        public virtual string GetTabTitles()
        {
            return "X\tY";
        }

        public virtual bool IsValid { get { return Geo.Utils.DoubleUtil.IsValid(X) && Geo.Utils.DoubleUtil.IsValid(Y); } }


        public virtual string GetTabValues()
        {
            return X+"\t"+Y;
        }

        public virtual string GetUniqueKey(double resolution=1e-3)
        {
            return Geo.Utils.StringUtil.GetUniqueKey(this, resolution);

        }

        public virtual IEnumerator<double> GetEnumerator()
        {
            return (new List<double> { X, Y }).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
