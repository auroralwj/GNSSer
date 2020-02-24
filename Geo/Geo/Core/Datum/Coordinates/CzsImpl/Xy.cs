//2014.06.06,czs,created

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Referencing;


namespace Geo.Coordinates
{
    /// <summary>
    /// ���������� IXY  ��ʵ�֡�
    /// </summary>
    public class Xy : IXY
    {
        /// <summary>
        /// ����һ�� Xy ʵ����
        /// </summary>
        /// <param name="x">X ����</param>
        /// <param name="y">Y ����</param>
        public Xy(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        /// <summary>
        /// X ����
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y ����
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// ֵ�Ƿ�ȫΪ 0.
        /// </summary>
        public bool IsZero { get { return X == 0 && Y == 0; } }
        /// <summary>
        /// �ַ������
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X + "," + Y;
        }
        /// <summary>
        /// ��ֵ�Ƿ����
        /// </summary>
        /// <param name="obj">���Ƚ϶���</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Xy xy = obj as Xy;
            if (xy == null) return false;

            return xy.X == X && xy.Y == Y;
        }
        /// <summary>
        /// ��ϣ��
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() * 13 + Y.GetHashCode() * 37;
        }
    }
}