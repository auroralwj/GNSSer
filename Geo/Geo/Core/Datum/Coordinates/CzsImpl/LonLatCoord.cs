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
    public class LonLatCoord : Coordinate, ILonLatCoord
    {
        /// <summary>
        /// Ĭ�Ϲ��캯������ʼ��Ϊ Coordinate.Empty��
        /// </summary>
        public LonLatCoord():this(null) {}

        
        /// <summary>
        /// ��һ�������� Lon, Lat ������ת��Ϊ Lonlat ���ꡣ
        /// </summary>
        /// <param name="coord">���� Lon, Lat������ת��</param>
        public LonLatCoord(ICoordinate coord) 
            :this(coord.ReferenceSystem, coord[Ordinate.Lon], coord[Ordinate.Lat], coord.Weight)
        {
         }
        /// <summary>
        /// �ɲο�ϵͳʵ�������ꡣ
        /// </summary>
        /// <param name="referenceSystem">�ο�ϵͳ</param>
        public LonLatCoord(ICoordinateReferenceSystem referenceSystem, double lon = 0, double lat = 0, double weight = 0)
            : base(referenceSystem, weight)
        {
            if (!ReferenceSystem.CoordinateSystem.Contains(Ordinate.Lon)
                || !ReferenceSystem.CoordinateSystem.Contains(Ordinate.Lat))
                throw new ArgumentException("�ο�ϵ��û�� Lon Lat ��", "referenceSystem");
            this.Lon = lon;
            this.Lat = lat;
        }

        /// <summary>
        /// Lon �����ֵ��
        /// </summary>
        public double Lon
        {
            get { return this[Ordinate.Lon]; }
            set { this[Ordinate.Lon] = value; }
        }

        /// <summary>
        /// Lat �����ֵ��
        /// </summary>
        public double Lat
        {
            get { return this[Ordinate.Lat]; }
            set { this[Ordinate.Lat] = value; }
        }
        /// <summary>
        /// �Ƕȵ�λ
        /// </summary>
        public AngleUnit Unit { get; set; }
    }
}
