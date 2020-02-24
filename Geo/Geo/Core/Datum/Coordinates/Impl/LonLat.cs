//2017.06.28, czs, edit in hongqing, 增加标识函数

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// Coordinate of longitude and latitude.
    /// 二维经纬度坐标。
    /// </summary>
    [Serializable]
    public class LonLat : TwoDimVector, ILonLat
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public LonLat():base(2) { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lon">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="unit">角度单位类型</param>
        public LonLat(double lon, double lat, AngleUnit unit = AngleUnit.Degree)
            : base(lon, lat)
        { 
            this.Unit = unit;
        }
        /// <summary>
        /// 角度单位类型。
        /// </summary>
        public AngleUnit Unit { get; set; }

        #region attributes 
        /// <summary>
        /// 经度
        /// </summary>
        public double Lon { get { return this[0]; } set { this[0] = value; } }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get { return this[1]; } set { this[1] = value; } }

        #endregion

        #region 对象方法
        /// <summary>
        /// 逆时针旋转。
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public  LonLat Rotate(double degree)
        {
            double rad = degree * 0.017453292519943295769236907684886;

            double lo = this.Lon * Math.Cos(rad) - this.Lat * Math.Sin(rad);
            double la = this.Lon * (Math.Sin(rad)) + this.Lat * Math.Cos(rad);
            return new LonLat(lo, la);
        }
        /// <summary>
        /// 数值是否相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            LonLat other = obj as LonLat;
            if (other == null) return false;

            return this.Lon == other.Lon && this.Lat == other.Lat;
        }
        
        public override int GetHashCode()
        {
            return (int)(Lon * 37 + Lat * 31);
        }
        /// <summary>
        /// Parse the string like (X,Y)
        /// </summary>
        /// <param name="toString"></param>
        /// <returns></returns>
        public static LonLat Parse(string toString)
        {
            toString = toString.Replace("(", "").Replace(")", "");
            string[] strs = toString.Split(new char[] { ',', ' ', '，', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            double lon = double.Parse(strs[0]);
            double lat = double.Parse(strs[1]);
            return new LonLat(lon, lat);
        }

        #endregion

        #region operator
        public static LonLat operator *(LonLat first, double num)        {            return new LonLat(first.Lon * num, first.Lat * num);        }
        public static LonLat operator /(LonLat first, double num)    {   return new LonLat(first.Lon / num, first.Lat / num);   }
        public static LonLat operator +(LonLat first, LonLat second)   { return new LonLat(first.Lon + second.Lon, first.Lat + second.Lat);  }
        public static LonLat operator -(LonLat first, LonLat second)  {    return new LonLat(first.Lon - second.Lon, first.Lat - second.Lat);    }
        #endregion

        #region 静态方法
        public static double DistanceInMeter(LonLat start, LonLat end)
        {
            return GeoDistance.GetDistanceInMeter(start, end);
            //LonLat differ = (start - end);
            //return (long)( Math.Sqrt(differ.Lat * differ.Lat + differ.Lon * differ.Lon) * 1.8 * 60); 
        }

        public static LonLat Parse(XY xy)
        {
            return new LonLat(xy.X, xy.Y);
        }
        #endregion
       
        /// <summary>
        /// 紧凑的数字，用逗号分隔。
        /// </summary>
        /// <returns></returns>
        public string ToCompactString()
        {
            return Lon.ToString("0.0000000") + "," + Lat.ToString("0.0000000");
        }
        /// <summary>
        ///字符串
        /// </summary>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public string ToDmsString(string spliter = ", ")
        {
            return new DMS(Lon, AngleUnit.Degree) + spliter + new DMS(Lat, AngleUnit.Degree);
        }
        /// <summary>
        /// 按照指定精度生成一个唯一的键
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public override string GetUniqueKey(double resolution)
        {
            double angleResolution = resolution;
            if (Unit == AngleUnit.Degree)//3600 秒/度 * 30 米/秒
            {
                angleResolution = resolution * 1e-5;
            }
            else if (Unit == AngleUnit.Radian)
            {
                angleResolution = resolution * 1e-7;
            }
            else
            {
                angleResolution = resolution * 1e-2;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(Geo.Utils.StringUtil.GetUniqueKey(Lon, angleResolution));
            sb.Append(",");
            sb.Append(Geo.Utils.StringUtil.GetUniqueKey(Lat, angleResolution));
            return sb.ToString();
        }
        /// <summary>
        /// 计算日固坐标系下的经纬度
        /// </summary>
        /// <param name="dateTimeUtc"></param>
        /// <returns></returns>
        public LonLat GetSeLonLat(DateTime dateTimeUtc)
        {
            double lon = AngularConvert.ToRad(Lon, this.Unit);
            double utRad = Geo.Times.Time. DateTimeToRad(dateTimeUtc);
            lon = lon + utRad - Math.PI;
            lon = AngularConvert.RadTo(lon, this.Unit);

            return new LonLat(lon, Lat, this.Unit);
        }

        /// <summary>
        /// 字符输出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Lon + ", " + Lat;
        }
        /// <summary>
        /// 字符输出
        /// </summary>
        /// <param name="format"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public virtual  string ToString(string format, string spliter = ", ")
        {
            return Lon.ToString(format) + spliter + Lat.ToString(format);
        }
    }
}
