using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;

namespace Gnsser.Interoperation.Bernese
{
    /// <summary>
    /// Bernese VelItem 文件。
    /// </summary>
    public class VelItem //: IXYZ
    {
        /// <summary>
        /// 从 1 开始的编号
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 测站名称，如 ALGO
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 测站代码， 如 40104M002
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 速度 ，单位为 米/年。
        /// </summary>
        public XYZ Vxyz { get; set; }
        /// <summary>
        /// 标签，如 IG00
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        /// 板块， 如 NOAM
        /// </summary>
        public string Plate { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public double X { get { return Vxyz.X; } }
        /// <summary>
        /// Y
        /// </summary>
        public double Y { get { return Vxyz.Y; } }
        /// <summary>
        /// Z
        /// </summary>
        public double Z { get { return Vxyz.Z; } }
        /// <summary>
        /// 行
        /// </summary>
        /// <returns></returns>
        public string ToLine()
        {
            string line;
            line = StringUtil.FillSpace(Num.ToString(), 3, false);
            line = StringUtil.FillSpace(line, 5);
            line += StationName + " ";
            line = StringUtil.FillSpace(line, 10);
            line += Code + " ";
            line = StringUtil.FillSpace(line, 22);

            line += StringUtil.FillSpace(Vxyz.X.ToString("0.0000"), 14, false) + " ";
            line += StringUtil.FillSpace(Vxyz.Y.ToString("0.0000"), 14, false) + " ";
            line += StringUtil.FillSpace(Vxyz.Z.ToString("0.0000"), 14, false) + " ";

            line = StringUtil.FillSpace(line, 70);
            line += Flag;
            line = StringUtil.FillSpace(line, 75);
            line += Plate;

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
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            VelItem o = obj as VelItem;
            if (o == null) return false;

            return StationName.Equals(o.StationName);
        }

        /// <summary>
        /// NUM  STATION NAME           VX (M/Y)       VY (M/Y)       VZ (M/Y)  FLAG   PLATE
        /// 1  ALGO 40104M002          -0.0154        -0.0054         0.0043    IG00 NOAM
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static VelItem ParseLine(string line)
        {
            string[] strs = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int i = 0;
            VelItem coord = new VelItem()
            {
                Num = int.Parse(strs[i++]),
                StationName = strs[i++],
                Code = strs[i++],
                Vxyz = new XYZ(
                    double.Parse(strs[i++]),
                    double.Parse(strs[i++]),
                    double.Parse(strs[i++])
                    )
            };
            if (strs.Length > i) coord.Flag = strs[i++];
            if (strs.Length > i) coord.Plate = strs[i++];
            return coord;
        }


    }
}
