//2016.02.01, czs, edit in hongqing, 修正估值坐标
//2017.06.03, czs, edit in hongqing, 修正大地坐标解析。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;


namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// 测站标识。
    /// </summary>
    public class SiteId : IBlockItem
    {
        /// <summary>
        /// 默认构造函数，除了初始化一个对象，什么也不干。
        /// </summary>
        public SiteId() { }

        public SiteId(
            string siteCode,
            string UniqueMonumentId,
            GeoCoord aprioriCoord,
            string StationDescription = "",
            string ptCode = "A",
            string ObservationCode = "P"
            )
        {
            this.GeoCoord = aprioriCoord;
            this.SiteCode = siteCode;
            this.UniqueMonumentId = UniqueMonumentId;
            this.ObservationCode = ObservationCode;
            this.StationDescription = StationDescription;
            this.ApproximateLongitude = GeoCoord.Lon;
            this.ApproximateLatitude = GeoCoord.Lat;
            this.ApproximateHeight = GeoCoord.Height;
        }

        public string SiteCode { get; set; }
        public string PointCode { get; set; }
        public string UniqueMonumentId { get; set; }
        public string ObservationCode { get; set; }
        public string StationDescription { get; set; }
        /// <summary>
        /// 先验经度，单位度。
        /// </summary>
        public double ApproximateLongitude { get; set; }
        /// <summary>
        /// 先验纬度，单位度。
        /// </summary>
        public double ApproximateLatitude { get; set; }
        /// <summary>
        /// 先验高度，单位米。
        /// </summary>
        public double ApproximateHeight { get; set; }
        /// <summary>
        /// 先验大地坐标。
        /// </summary>
        public GeoCoord GeoCoord { get; set; }

        public override bool Equals(object obj)
        {
            SiteId site = obj as SiteId;
            return site == null ? false : SiteCode.Equals(site.SiteCode);
        }

        public override int GetHashCode()
        {
            return SiteCode.GetHashCode();
        }
        /// <summary>
        /// *CODE PT __DOMES__ TProduct _STATION DESCRIPTION__ APPROX_LON_ APPROX_LAT_ _APP_H_
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string line =
              " " + StringUtil.FillSpaceLeft(SiteCode, 4)
            + " " + StringUtil.FillSpaceLeft(PointCode, 2)
            + " " + StringUtil.FillSpaceLeft(UniqueMonumentId, 9)
            + " " + ObservationCode
            + " " + StringUtil.FillSpace(StationDescription, 22)
            + "" + ToSnxString(new DMS(ApproximateLongitude))
            + "" +  ToSnxString(new DMS(ApproximateLatitude)) 
            + " " + StringUtil.FillSpaceLeft(ApproximateHeight.ToString("0.0"), 7);

            return line;
        }


        public  IBlockItem Init(string line)
        { 
            this.SiteCode = line.Substring(1, 4);
            this.PointCode = line.Substring(6, 2).Trim();
            this.UniqueMonumentId = line.Substring(9, 9).Trim();
            this.ObservationCode = line.Substring(19, 1);
            this.StationDescription = line.Substring(21, 22);

            // 8 45 45.4
            this.ApproximateLongitude = PareSnx(line.Substring(43, 12)).Degrees;             
            this.ApproximateLatitude = PareSnx(line.Substring(55, 12)).Degrees;            
            //this.ApproximateLongitude = PareSnx(line.Substring(67, 12)).Degrees;
            //this.ApproximateLatitude = PareSnx(line.Substring(79, 12)).Degrees;
            this.ApproximateHeight =  Geo.Utils.StringUtil.ParseDouble(line, 68, 7);
            this.GeoCoord = new Geo.Coordinates.GeoCoord(this.ApproximateLongitude, this.ApproximateLatitude, this.ApproximateHeight);


            return this;
        }

        /// <summary>
        /// 355 30 12.3.
        /// 0 -40 12.2
        /// </summary>
        /// <returns></returns>
        public static string ToSnxString(DMS dms)
        {
            string mark = "";
            if (!dms.IsPlus) mark = "-";
            string degStr = StringUtil.FillSpaceLeft(To180MinusPlus(dms.Degree) + "", 3);
            if (dms.Degree == 0)
            {
                if (dms.Minute == 0)
                {
                    return "    "
                         + "   "
                         + StringUtil.FillSpaceLeft(mark + dms.Second.ToString("0.0"), 5);
                }
                return "    "
                    + StringUtil.FillSpaceLeft(mark + dms.Minute.ToString(), 3)
                    + StringUtil.FillSpaceLeft(dms.Second.ToString("0.0"), 5);
            }

            return StringUtil.FillSpaceLeft(mark + To180MinusPlus( dms.Degree), 4)
                + StringUtil.FillSpaceLeft(dms.Minute.ToString(), 3)
                + StringUtil.FillSpaceLeft(dms.Second.ToString("0.0"), 5);
        }

        public static int To180MinusPlus(int degree)
        {
            while (degree > 180)
            {
                degree -= 360;
            }
            while (degree < -180)
            {
                degree += 360;
            }
            return degree;
        }

        /// <summary>
        /// 注意判断度为0，分为0 时的正负号问题。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static DMS PareSnx(string line){
            bool IsPlus = true;
            //line = line.TrimStart();
            //if (line.StartsWith("-"))
            //{
            //    IsPlus = false;
            //}

            //line = line.Substring(1);

            line = line.TrimStart();
            int aa = line.IndexOf('-');
            string[] strs =null;
            if (line.StartsWith("0") && line.IndexOf('-') == 1)
            {
                string blank = " ";
                line.Insert(1, blank);
                //strs.Add("0");
                var strs1 = line.Substring(1,line.Length-1).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int length = strs1.Length + 1;
                strs=new string[length];
                strs[0] = "0";
                int i = 1;
                foreach (var item in strs1)
                {
                    strs[i] = item;
                    i++;
                }
            }

            else
            {
               strs = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                
            }
            
            int deg = 0;
            int min = 0;
            double sec = 0;
            if (strs.Length > 2)
            {
                int i = 0;
                deg = int.Parse(strs[i++]);
                min = int.Parse(strs[i++]);
                sec = double.Parse(strs[i++]);
            }
            else if (strs.Length == 2)
            {
                int i = 0;
                min = int.Parse(strs[i++]);
                sec = double.Parse(strs[i++]);
            }
            else if (strs.Length == 1)
            {
                int i = 0; 
                sec = double.Parse(strs[i++]);
            } 

            if (deg == 0)
            {
                if (min == 0) IsPlus = sec > 0;
                else IsPlus = min > 0;
            }
            else IsPlus = deg > 0;

            return new DMS(deg, min, sec, IsPlus);
        }
    }

   
}
