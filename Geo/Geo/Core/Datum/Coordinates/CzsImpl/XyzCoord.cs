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
    public class XyzCoord : XyCoord, IXyzCoord
    { 
        /// <summary>
        /// Ĭ�Ϲ��캯������ʼ��Ϊ Coordinate.Empty��
        /// </summary>
        public XyzCoord():this(null) {}
        /// <summary>
        /// ����������ת��ΪXYZ���ꡣ
        /// </summary>
        /// <param name="coord">��������</param>
        public XyzCoord(ICoordinate coord) : 
            this(coord.ReferenceSystem, coord[Ordinate.X], coord[Ordinate.Y], coord[Ordinate.Z], coord.Weight)
        {

        }
        /// <summary>
        /// �ɲο�ϵͳʵ�������ꡣ
        /// </summary>
        /// <param name="referenceSystem">�ο�ϵͳ</param>
        public XyzCoord(ICoordinateReferenceSystem referenceSystem, double x = 0, double y = 0, double z = 0, double weight = 0)
            : base(referenceSystem,x,y, weight)
        {
            if (!ReferenceSystem.CoordinateSystem.Contains(Ordinate.Z))
                throw new ArgumentException("�ο�ϵ��û�� Z ��", "referenceSystem");
            this.Z = z;
        }

        /// <summary>
        /// Z �����ֵ��
        /// </summary>
        public double Z
        {
            get { return this[Ordinate.Z]; }
            set { this[Ordinate.Z] = value; }
        }

        //public override string ToString()
        //{
        //    return "(" + X + "," + Y + "," + "," + Z + ")";
        //}
    }
}
