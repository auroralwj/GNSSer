//2014.06.04,czs,created

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Referencing;


namespace Geo.Coordinates
{
    /// <summary>
    /// ͨ�����ꡣ 
    /// </summary>
    [Serializable]
    public class Coordinate : Geo.Algorithm.AbstractVector, ICoordinate
    {
        /// <summary>
        /// Ĭ�Ϲ��캯������ʼ��Ϊ Empty��
        /// </summary>
        //public Coordinate():this(null) {}
        /// <summary>
        /// �ɲο�ϵͳʵ�������ꡣ
        /// </summary>
        /// <param name="referenceSystem">�ο�ϵͳ</param>
        public Coordinate(ICoordinateReferenceSystem referenceSystem, double weight = 0, CoordinateType CoordinateType = CoordinateType.Other)
        {
            this.ReferenceSystem = referenceSystem;
            this.Weight = weight;
            this.CoordDic = new Dictionary<Ordinate, double>();

            foreach (var item in ReferenceSystem.CoordinateSystem.Axes)
            {
                this.CoordDic.Add(item.Ordinate, 0);
            }
        }

        #region ����
        /// <summary>
        /// �洢������ֵ䡣
        /// </summary>
        protected Dictionary<Ordinate, Double> CoordDic { get; set; }
        /// <summary>
        /// Ȩֵ��������GeoAPI��Ӧ���� M ������
        /// </summary>
        //public double Weight { get; set; }
        /// <summary>
        /// ��ǩ�����ڴ洢һ������
        /// </summary>
        //public object Tag { get; set; }
        /// <summary>
        /// ��ȡ��������������ֵ��
        /// </summary>
        /// <param name="axisIndex">��������ţ��� 0 ��ʼ</param>
        /// <returns></returns>
        public override double this[int axisIndex]
        { 
            get { return  this[ReferenceSystem.CoordinateSystem[axisIndex].Ordinate]; }
            set { this[ReferenceSystem.CoordinateSystem[axisIndex].Ordinate] = value; } 
        }
        /// <summary>
        /// �������б�
        /// </summary>
        public List<IAxis> Axes { get { return ReferenceSystem.CoordinateSystem.Axes; } }
        /// <summary>
        /// �����ά����
        /// </summary>
        public override int Dimension { get { return CoordDic.Count; } }
        /// <summary>
        /// ����ο�ϵ��
        /// </summary>
        public ICoordinateReferenceSystem ReferenceSystem { get; set; }

         

        /// <summary>
        /// �Ƿ�Ϊ�ա���û��ָ���ο�ϵͳ����Ϊ�ա�
        /// </summary>
        public bool IsEmpty
        {
            get { return ReferenceSystem == null; }
        }

        /// <summary>
        /// ��ȡ������ָ������ֵ��
        /// </summary>
        /// <param name="ordinate"></param>
        /// <returns></returns>
        public double this[Ordinate ordinate] { get { return CoordDic[ordinate]; } set { CoordDic[ordinate] = value; } }
        #endregion

        #region ����
        /// <summary>
        /// ��¡��
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            Coordinate clone = new Coordinate(ReferenceSystem);
            foreach (var item in this.CoordDic.Keys)
            {
                clone[item] = this[item];
            }
            return clone;
        }
        /// <summary>
        /// �Ƚϡ���ԭ��ľ��롣
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is ICoordinate)
            {
                ICoordinate other = obj as ICoordinate;
                return (int)((Distance(Zero) - other.Distance(Zero)) * 1000000);
            }
            return 0;
        } 
        /// <summary>
        /// �Ƿ����ָ������
        /// </summary>
        /// <param name="ordinate"></param>
        /// <returns></returns>
        public bool ContainsOrdinate(Ordinate ordinate)
        {
            return CoordDic.ContainsKey(ordinate);
        }
        /// <summary>
        /// ��ԭ���ŷʽ���룬�뾶��
        /// </summary>
        public double Radius { get { return Distance(Zero); } }
        /// <summary>
        /// ŷʽ���롣
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double Distance(ICoordinate other)
        {
            if (!IsHomogenized(other)) throw new ArgumentException("�������Ͳ�ͬ��û�пɱ���", "other");

            double dis = 0;
            foreach (var item in CoordDic)
            {
                dis += Math.Pow(this[item.Key] - other[item.Key], 2.0);
            }

            return Math.Sqrt(dis);
        }


        /// <summary>
        /// �Ƿ�Ϊͬ�����꣬��Ҫ��ͨ���ο�ϵ�жϡ�
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsHomogenized(ICoordinate other)
        {
            //ͬΪҰ����ҲΪͬ������
            if (IsEmpty != other.IsEmpty) return false;

            if (!ReferenceSystem.Equals(other.ReferenceSystem)) return false;

            return true;
        }
        /// <summary>
        /// �ַ���
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            bool isFirst = true;
            foreach (var item in this)
            {
                if (!isFirst)  sb.Append(", ");
                sb.Append(item);
            }
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// ��ֵ�Ƿ���ȡ�
        /// </summary>
        /// <param name="other">���Ƚ϶���</param>
        /// <returns></returns>
        public bool Equals(ICoordinate other)
        {
            return Equals(other, Geo.Coordinates.Tolerance.Default);
        }
         
        /// <summary>
        /// ��ͬ�ο�ϵ�£���ֵ���Ƿ���ȡ�
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ICoordinate other, Tolerance tolerance)
        {
           //throw  new NullReferenceException("other");

            if (other == null) throw new NullReferenceException("other");

            if (!this.ReferenceSystem.Equals(other.ReferenceSystem)) return false;
            if (this.Dimension != other.Dimension) return false;
            if (this.Weight != other.Weight) return false;

            foreach (var item in this.CoordDic.Keys)
            {
                if (tolerance == null)
                {
                    if (this[item] != other[item]) 
                        return false;
                }
                else
                {
                    if (Math.Abs(this[item] - other[item]) > tolerance.Value)
                        return false;                    
                }
            }
            return true;
        }

        /// <summary>
        /// ��ͬ�ο�ϵ�£���ֵ���Ƿ���ȡ�
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Coordinate other = obj as Coordinate;
            if (other == null) return false;

            bool val = Equals(other, Geo.Coordinates.Tolerance.Default);
            return val;
        }

        /// <summary>
        /// ��ϣ����
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int code = ReferenceSystem.GetHashCode() * 13;
            foreach (var item in this.CoordDic.Values)
            {
                code += item.GetHashCode();
            }
            return code;
        }
        #region ö�����ӿ�
        /// <summary>
        /// ö�����ӿ�
        /// </summary>
        /// <returns></returns>
        public   IEnumerator<KeyValuePair<Ordinate, double>> GetEnumerator()
        {
            return CoordDic.GetEnumerator();
        }
        /// <summary>
        /// ö�����ӿ�
        /// </summary>
        /// <returns></returns>
      System.Collections.IEnumerator  System.Collections.IEnumerable.GetEnumerator()
        {
            return CoordDic.GetEnumerator();
        }
        #endregion
        #endregion

        #region ��������
        /// <summary>
        /// ԭ�㡣
        /// </summary>
        public ICoordinate Zero
        {
            get { return new Coordinate(ReferenceSystem); }
        }
        #endregion


        public override Algorithm.IVector Create(int count)
        {
            return   new  Coordinate(CoordinateReferenceSystem.Wgs84GeodeticCs);
        }

        public override Algorithm.IVector Create(double[] array)
        {
            return new Coordinate(CoordinateReferenceSystem.Wgs84GeodeticCs);
        }
       
    }
}
