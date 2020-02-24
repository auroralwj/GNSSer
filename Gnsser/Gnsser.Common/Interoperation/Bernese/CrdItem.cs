using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Times;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Times; 

namespace Gnsser.Interoperation.Bernese
{
    /// <summary>
    /// Bernese 文件。
    /// </summary>
    public class CrdItem //: IXYZ
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CrdItem() { }

        /// <summary>
        /// 从 Rinex Header 中创建。
        /// </summary>
        /// <param name="num"></param>
        /// <param name="name"></param>
        /// <param name="obsCodeode"></param>
        /// <param name="xyz"></param>
        public CrdItem(int num, string name, string code, XYZ xyz)
        {
            this.Num = num;
            this.Code = code;
            this.Flag = "";
            this.StationName = name;
            this.Xyz = xyz;
            this.GeoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(this.Xyz);
        }
        /// <summary>
        /// 编号
        /// </summary>

        public int Num { get; set; }
        /// <summary>
        /// 测站名
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public XYZ Xyz { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public double X { get { return Xyz.X; } }
        /// <summary>
        /// Y
        /// </summary>
        public double Y { get { return Xyz.Y; } }
        /// <summary>
        /// Z
        /// </summary>
        public double Z { get { return Xyz.Z; } }
        /// <summary>
        /// 地理坐标
        /// </summary>
        public GeoCoord GeoCoord { get; set; }
        /// <summary>
        /// 标记
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        /// 行
        /// </summary>
        /// <returns></returns>
        public string ToLine()
        {
            return ToString();
        }
        /// <summary>
        /// 到行
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string line;
            line = StringUtil.FillSpace(Num.ToString(), 3, false);
            line = StringUtil.FillSpace(line, 5);
            line += StationName + " ";
            line = StringUtil.FillSpace(line, 10);
            line += Code + " ";
            line = StringUtil.FillSpace(line, 22);

            line += StringUtil.FillSpace(Xyz.X.ToString("0.0000"), 14, false) + " ";
            line += StringUtil.FillSpace(Xyz.Y.ToString("0.0000"), 14, false) + " ";
            line += StringUtil.FillSpace(Xyz.Z.ToString("0.0000"), 14, false) + " ";

            line = StringUtil.FillSpace(line, 70);
            line += Flag;
            return line;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return StationName.GetHashCode();
        }
        /// <summary>
        /// 等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        public override bool Equals(object obj)
        {
            CrdItem o = obj as CrdItem;
            if (o == null) return false;

            return StationName.Equals(o.StationName);
        }
        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>

        public CrdItem Clone()
        {
            return new CrdItem(Num, StationName, Code, Xyz.DeepClone()) {  Flag = Flag};
        }

        /// <summary>
        /// 历元归算
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="current"></param>
        /// <param name="to"></param> 
        public static XYZ GetEpochReduction(XYZ velocity, Time current, Time to)
        {
            TimeSpan span = current.DateTime - to.DateTime;
            double differYear = span.TotalDays / 365.0;
            return GetEpochReduction(velocity, differYear);
        }
        /// <summary>
        /// 历元归算
        /// </summary> 
        /// <param name="velocity"></param>
        /// <param name="differYear"></param>
        public static XYZ GetEpochReduction(XYZ velocity, double differYear)
        {
            return  velocity * differYear;
        }

        /// <summary>
        /// NUM  STATION NAME           X (M)          Y (M)          Z (M)     FLAG
        ///  1  BRUS 13101M004     4027893.8650    307045.6950   4919475.0250 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static CrdItem ParseLine(string line)
        {
            string[] strs = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int i = 0;
            CrdItem coord = new CrdItem();

            coord.Num = int.Parse(strs[i++]);
            coord.StationName = strs[i++];
            if( line.Substring(10, 10).Trim().Length > 0) coord.Code = strs[i++];
            coord.Xyz = new XYZ(
                 double.Parse(strs[i++]),
                 double.Parse(strs[i++]),
                 double.Parse(strs[i++])
                 );
            if (strs.Length > i) coord.Flag = strs[i++];
            coord.GeoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(coord.Xyz);
            return coord;
        }
    }


}
