using System;

namespace GeodeticX
{
	/// <summary>
	/// ���ڿռ�ֱ������ϵ�µ�����ֵ��ͨ����(X,Y,Z)����ʾ��
	/// </summary>
    public class SpatialRectCoordinate : Geo.Coordinates.IXYZ
	{  

        /// <summary>
        /// ������ֵ�ĳ�ʼ������
        /// </summary>
        /// <param name="X">X����</param>
        /// <param name="Y">Y����</param>
        /// <param name="Z">Z����</param>
        public SpatialRectCoordinate(double X, double Y, double Z)
		{
            this.X = X;
            this.Y = Y;
            this.Z = Z;
		}

        public double X { get; set; }
        public double Y { get; set; }
  
        public double Z { get; set; }

        /// <summary>
        /// ֵ�Ƿ�ȫΪ 0.
        /// </summary>
        public bool IsZero { get { return Z == 0 && Y==0 && Z ==0; } }
	}
}
