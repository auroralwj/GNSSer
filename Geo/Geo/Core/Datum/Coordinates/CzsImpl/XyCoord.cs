//2014.06.06,czs,created

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Referencing;


namespace Geo.Coordinates
{
    /// <summary>
    /// ���� X Y ���������ꡣ
    /// </summary>
    [Serializable]
    public class XyCoord : Coordinate, IXyCoord
    {
        /// <summary>
        /// Ĭ�Ϲ��캯������ʼ��Ϊ Coordinate.Empty��
        /// </summary>
        public XyCoord():this(null) {}
         
        /// <summary>
        /// ��һ�������� X, Y ������ת��Ϊ XY ���ꡣ
        /// </summary>
        /// <param name="coord">���� Lon, Lat������ת��</param>
        public XyCoord(ICoordinate coord) 
            :this(coord.ReferenceSystem, coord[Ordinate.X], coord[Ordinate.Y], coord.Weight)
        {
         }
        /// <summary>
        /// �ɲο�ϵͳʵ�������ꡣ
        /// </summary>
        /// <param name="referenceSystem">�ο�ϵͳ</param>
        public XyCoord(ICoordinateReferenceSystem referenceSystem, double x= 0, double y = 0,double weight=0):base(referenceSystem, weight)
        {
            if (!ReferenceSystem.CoordinateSystem.Contains(Ordinate.X)
                || !ReferenceSystem.CoordinateSystem.Contains(Ordinate.Y))
                throw new ArgumentException("�ο�ϵ��û�� X Y ��", "referenceSystem");
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// X �����ֵ��
        /// </summary>
        public double X
        {
            get { return this[Ordinate.X]; }
            set { this[Ordinate.X] = value; }
        }

        /// <summary>
        /// Y �����ֵ��
        /// </summary>
        public double Y
        {
            get { return this[Ordinate.Y]; }
            set { this[Ordinate.Y] = value; }
        }

        /// <summary>
        /// ֵ�Ƿ�ȫΪ 0.
        /// </summary>
        public bool IsZero { get { return X == 0 && Y == 0; } }
    }
}
