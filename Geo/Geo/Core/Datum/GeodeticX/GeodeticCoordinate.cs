using System;

namespace GeodeticX
{
	/// <summary>
	/// 点的大地坐标
	/// </summary>
	public class GeodeticCoordinate
	{
        /// <summary>
        /// 经度 (-180～180)
        /// </summary>
        private double longitude;

        /// <summary>
        /// 纬度 (－90～90)
        /// </summary>
        private double latitude;

        /// <summary>
        /// 大地高
        /// </summary>
        private double geoidHeight;

        /// <summary>
        /// 是否设定了坐标值的标记
        /// </summary>
        private bool blIsNull;
        private bool hIsNull;

        public GeodeticCoordinate()
        {
            blIsNull = true;
            hIsNull = true;
        }

        /// <summary>
        /// 带坐标值的初始化函数
        /// </summary>
        /// <param name="B">纬度</param>
        /// <param name="L">经度</param>
        public GeodeticCoordinate(double B, double L)
        {
            bool bIsNull = true, lIsNull = true;

            //判断纬度的值域范围
            if (Math.Abs(B) <= 90)
            {
                latitude = B;
                bIsNull = false;
            }
            else
                throw new Exception("纬度值超限，应该在-90到90之间");

            //判断经度的值域范围
            if (Math.Abs(L) <= 180)
            {
                longitude = L;
                lIsNull = false;
            }
            else
                throw new Exception("经度值超限，应该在-180到180之间");

            if (!bIsNull && !lIsNull) 
                blIsNull = false;
            hIsNull = true;
        }

        /// <summary>
        /// 带坐标值的初始化函数
        /// </summary>
        /// <param name="B">纬度</param>
        /// <param name="L">经度</param>
        /// <param name="H">大地高</param>
        public GeodeticCoordinate(double B, double L, double H)
            : this(B, L)
        {
            geoidHeight = H;
            hIsNull = false;
        }

        /// <summary>
        /// 获取/设置纬度
        /// </summary>
        public double B
        {
            get
            {
                if (!blIsNull)
                    return latitude;
                else
                    throw new Exception("纬度为空");
            }
        }

        /// <summary>
        /// 获取/设置经度
        /// </summary>
        public double L
        {
            get
            {
                if (!blIsNull)
                    return longitude;
                else
                    throw new Exception("经度为空");
            }
        }

        /// <summary>
        /// 获取/设置大地高
        /// </summary>
        public double H
        {
            get
            {
                if (!hIsNull)
                    return geoidHeight;
                else
                    throw new Exception("大地高为空");
            }
            set
            {
                geoidHeight = value;
                hIsNull = false;
            }
        }

        public void SetCoordinate(double B, double L)
        {
            bool bIsNull = true, lIsNull = true;

            //判断纬度的值域范围
            if (Math.Abs(B) > 90)
            {
                latitude = B;
                bIsNull = false;
            }
            else
                throw new Exception("纬度值超限，应该在-90到90之间");

            //判断经度的值域范围
            if (Math.Abs(L) > 180)
            {
                longitude = L;
                lIsNull = false;
            }
            else
                throw new Exception("经度值超限，应该在-180到180之间");

            if (!bIsNull && !lIsNull)
                blIsNull = false;
            hIsNull = true;
        }

        public void SetCoordinate(double B, double L, double H)
        {
            SetCoordinate(B, L);

            geoidHeight = H;
            hIsNull = false;
        }

        public override string ToString()
        {
            return string.Empty;// this.ToString("DMS");
        }
	}
}
