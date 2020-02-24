using System;

namespace GeodeticX
{
    /// <summary>
    /// 天文坐标
    /// </summary>
    public class AstronomicCoordinate
    {
        /// <summary>
        /// 天文纬度 (－90～90)
        /// </summary>
        private double latitude;
        /// <summary>
        /// 天文经度 (-180～180)
        /// </summary>
        private double longitude;
        /// <summary>
        /// 天文方位角
        /// </summary>
        private double azimuth;

        /// <summary>
        /// 是否设定了坐标值的标记
        /// </summary>
        private bool blIsNull;
        private bool aIsNull;

        public AstronomicCoordinate()
        {
            blIsNull = true;
            aIsNull = true;
        }

        /// <summary>
        /// 带坐标值的初始化函数
        /// </summary>
        /// <param name="λ">天文经度</param>
        /// <param name="φ">天文纬度</param>
        public AstronomicCoordinate(double λ, double φ)
        {
            bool bIsNull = true, lIsNull = true;

            //判断纬度的值域范围
            if (λ >= -180 && λ <= 180)
            {
                longitude = λ;
                lIsNull = false;
            }
            else
                throw new Exception("天文经度值超限，应该在-180到180之间");

            //判断经度的值域范围
            if (φ >= -90 && φ <= 90)
            {
                latitude = φ;
                bIsNull = false;
            }
            else
                throw new Exception("天文纬度值超限，应该在-90到90之间");

            if (!bIsNull && !lIsNull) 
                blIsNull = false;
            aIsNull = true;
        }

        /// <summary>
        /// 带坐标值的初始化函数
        /// </summary>
        /// <param name="λ">纬度</param>
        /// <param name="φ">经度</param>
        /// <param name="A">大地方位角</param>
        public AstronomicCoordinate(double λ, double φ, double A)
            : this(λ, φ)
        {
            if (A >= 0 && A < 360)
            {
                azimuth = A;
                aIsNull = false;
            }
            else
                throw new Exception("天文方位角超限，应该在0到360度之间");
        }

        /// <summary>
        /// 获取/设置天文经度
        /// </summary>
        public double λ
        {
            get
            {
                if (!blIsNull)
                    return longitude;
                else
                    throw new Exception("天文经度为空");
            }
        }

        /// <summary>
        /// 获取/设置天文纬度
        /// </summary>
        public double φ
        {
            get
            {
                if (!blIsNull)
                    return latitude;
                else
                    throw new Exception("天文纬度为空");
            }
        }

        /// <summary>
        /// 获取/设置大地方位角
        /// </summary>
        public double A
        {
            get
            {
                if (!aIsNull)
                    return azimuth;
                else
                    throw new Exception("大地方位角为空");
            }
            set
            {
                if (value >= 0 && value < 360)
                {
                    azimuth = value;
                    aIsNull = false;
                }
                else
                    throw new Exception("天文方位角超限，应该在0到360度之间");
            }
        }

        public void SetCoordinate(double λ, double φ)
        {
            bool bIsNull = true, lIsNull = true;

            //判断纬度的值域范围
            if (λ >= -180 && λ <= 180)
            {
                longitude = λ;
                lIsNull = false;
            }
            else
                throw new Exception("天文经度值超限，应该在-180到180之间");

            //判断经度的值域范围
            if (φ >= -90 && φ <= 90)
            {
                latitude = φ;
                bIsNull = false;
            }
            else
                throw new Exception("天文纬度值超限，应该在-90到90之间");

            if (!bIsNull && !lIsNull)
                blIsNull = false;
            aIsNull = true;
        }

        public void SetCoordinate(double λ, double φ, double A)
        {
            SetCoordinate(λ, φ);

            if (A >= 0 && A < 360)
            {
                azimuth = A;
                aIsNull = false;
            }
            else
                throw new Exception("天文方位角超限，应该在0到360度之间");
        }
    }
}
