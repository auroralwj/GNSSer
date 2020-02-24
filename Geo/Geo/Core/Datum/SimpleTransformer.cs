//2015.04.15, czs, create in numu shuangliao, ����ע��


using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{
    /// <summary>
    /// �򵥵�����ת����
    /// ���� ����ĵ�λһ�£�������ƽ�У���������෴����
    /// ��Ҫת������ƽ�ƣ������Լ����귭ת��
    /// </summary>
    public class SimpleTransformer : ICoordTransformer
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="sourseCoord"></param>
        /// <param name="targetCoord"></param>
        public SimpleTransformer(XYZ sourseCoord, XYZ targetCoord)
        {
            this.Scale = 1;
            this.DifferCoord =targetCoord - sourseCoord;
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="differCoord"></param>
        public SimpleTransformer(double scale, XYZ differCoord)
        {
            this.Scale = scale;
            this.DifferCoord = differCoord;
        }


        /// <summary>
        /// �߶�����
        /// </summary>
        public double Scale { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        public XYZ DifferCoord { get; set; }
        /// <summary>
        /// ת��
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public XYZ Trans(XYZ old)
        {
            double x = (Scale) * old.X + DifferCoord.X;
            double y = (Scale) * old.Y + DifferCoord.Y;
            double z = (Scale) * old.Z + DifferCoord.Z;
            return new XYZ(x, y, z);
        }




    }
}
