using System;

namespace GeodeticX
{
	/// <summary>
	/// 点在空间直角坐标系下的坐标值，通常以(X,Y,Z)来表示。
	/// </summary>
    public class SpatialRectCoordinate : Geo.Coordinates.IXYZ
	{  

        /// <summary>
        /// 带坐标值的初始化函数
        /// </summary>
        /// <param name="X">X分量</param>
        /// <param name="Y">Y分量</param>
        /// <param name="Z">Z分量</param>
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
        /// 值是否全为 0.
        /// </summary>
        public bool IsZero { get { return Z == 0 && Y==0 && Z ==0; } }
	}
}
