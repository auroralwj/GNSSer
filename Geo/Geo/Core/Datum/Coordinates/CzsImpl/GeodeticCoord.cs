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
    public class GeodeticCoord : LonLatCoord, IGeodeticCoord
    {
        /// <summary>
        /// Ĭ�Ϲ��캯������ʼ��Ϊ Coordinate.Empty��
        /// </summary>
        public GeodeticCoord():this(null) {}

        /// <summary>
        /// ��һ�������� Lon, Lat, height������ת��Ϊ������ꡣ
        /// </summary>
        /// <param name="coord">���� Lon, Lat, height������ת��</param>
        public GeodeticCoord(ICoordinate coord) 
            :this(coord.ReferenceSystem, coord[Ordinate.Lon], coord[Ordinate.Lat], coord[Ordinate.Height], coord.Weight)
        {
            //if (!coord.ContainsOrdinate(Ordinate.Lon)
            //   || !coord.ContainsOrdinate(Ordinate.Lat)
            //   || !coord.ContainsOrdinate(Ordinate.Height))
            //    throw new ArgumentException("���Ǵ������", "coord");

        }
        /// <summary>
        /// �ɲο�ϵͳʵ�������ꡣ
        /// </summary>
        /// <param name="referenceSystem">�ο�ϵͳ</param>
        public GeodeticCoord(ICoordinateReferenceSystem referenceSystem, double lon = 0, double lat = 0, double height = 0,double weight = 0)
            : base(referenceSystem, lon, lat, weight)
        {
            if (!ReferenceSystem.CoordinateSystem.Contains(Ordinate.Height) )
                throw new ArgumentException("�ο�ϵ��û�� Height ��", "referenceSystem");
            this.Height = height;
        }

        /// <summary>
        /// Lon �����ֵ��
        /// </summary>
        public double Height
        {
            get { return this[Ordinate.Height]; }
            set { this[Ordinate.Height] = value; }
        } 
    }
}
