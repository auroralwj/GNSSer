//2014.09.27, czs, created, ����Ȩֵ������

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Common;

namespace Geo.Coordinates
{
    /// <summary>
    /// X,Y,Z ��ά���ꡣ��ά�ռ�� 3 ��˫���ȷ�����
    /// </summary> 
    public class RmsedXYZ : RmsedValue<XYZ>
    {
        /// <summary>
        /// ��Ȩ����
        /// </summary>
        public RmsedXYZ()
            : base(new XYZ(), new XYZ())
        {
        }
        /// <summary>
        /// ��Ȩ����
        /// </summary>
        /// <param name="xyz"></param>
        public RmsedXYZ(XYZ xyz)
            : base(xyz, new XYZ())
        {
        }
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="xyz">����</param>
        /// <param name="rms">������Ϣ</param>
        public RmsedXYZ(XYZ xyz, XYZ rms)
            : base(xyz, rms)
        {

        }

        public static RmsedXYZ operator +(RmsedXYZ left, XYZ right)
        {
            return new RmsedXYZ(left.Value + right, left.Rms);
        }
        public static RmsedXYZ operator +(XYZ left, RmsedXYZ right)
        {
            return new RmsedXYZ(left + right.Value, right.Rms);
        }
        public static RmsedXYZ operator +(RmsedXYZ left, RmsedXYZ right)
        {
            return new RmsedXYZ(left.Value + right.Value,
               new XYZ(
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.X, right.Rms.X),
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.Y, right.Rms.Y),
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.Z, right.Rms.Z)));
        }
        public static RmsedXYZ operator -(RmsedXYZ left, RmsedXYZ right)
        {
            return new RmsedXYZ(left.Value - right.Value,
               new XYZ(
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.X, right.Rms.X),
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.Y, right.Rms.Y),
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.Z, right.Rms.Z)));
        }
        public static RmsedXYZ operator -(RmsedXYZ right)
        {
            return new RmsedXYZ( - right.Value, right.Rms);
        }
        /// <summary>
        /// 0
        /// </summary>
        public static RmsedXYZ Zero => new RmsedXYZ();
        /// <summary>
        /// ���ֵ
        /// </summary>
        public static RmsedXYZ MaxValue => new RmsedXYZ(XYZ.MaxValue, XYZ.MaxValue);
    }
}