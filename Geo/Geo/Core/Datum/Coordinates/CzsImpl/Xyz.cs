//2014.06.06,czs,created

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Referencing;


namespace Geo.Coordinates
{
    /// <summary>
    /// ���������� IXYZ  ��ʵ�֡�
    /// </summary>
    public class Xyz :Xy, IXYZ
    {
        public Xyz(double x, double y, double z)
            :base(x,y)
        {
            this.Z = z;
        }
        /// <summary>
        /// Z ����
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// ֵ�Ƿ�ȫΪ 0.
        /// </summary>
        public bool IsZero { get { return Z == 0 && base.IsZero; } }
        /// <summary>
        /// �ַ������
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X + "," + Y + "," + Z;
        }
        /// <summary>
        /// ��ֵ�Ƿ����
        /// </summary>
        /// <param name="obj">���Ƚ϶���</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Xyz xyz = obj as Xyz;
            if (xyz == null) return false;

            return base.Equals(xyz) && Z== xyz.Z;
        }
        /// <summary>
        ///  ��ϣ��
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() + Z.GetHashCode() * 3;
        }
    }
}