 using System;   

using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Geo.Data
{
    /// <summary>
    /// 读取器。只读取XYZ坐标。
    /// </summary>
    public class PinnacleRepFileReader : ItemReader<CoordinateRecord, PinnacleRepFile>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fileName"></param>
        public PinnacleRepFileReader(string fileName)
            : base(fileName)
        {
            this.TokenOfStartLine = "ADJUSTED COORDINATES in WGS84(XYZ)";

        }

        protected override bool ExtraCheck(string line)
        {
            if (line.Contains("--------------") || line.StartsWith("     Point") || line.StartsWith("#")) { return false; }
            //第一个是否为数字
            
            return base.ExtraCheck(line);
        }
          
        public override CoordinateRecord ParseRow(string[] items)
        {
            if (items.Length < 11) return null;

            int i = 0;
            CoordinateRecord record = new CoordinateRecord();
            //编号
            i++;
            record.Id = items[i++];
            //record.Name = record.Number;
            record.X = Double.Parse(items[i++]);
            record.Y = Double.Parse(items[i++]);
            record.Z = Double.Parse(items[i++]);
            record.MX = Double.Parse(items[i++]);
            record.MY = Double.Parse(items[i++]);
            record.MZ = Double.Parse(items[i++]); 
            record.IsKnown = ((record.MX == 0) && (record.MY == 0) && (record.MZ == 0));

            return record;
        }

        #region BLH 的解析
        protected   CoordinateRecord ParseRowBLH(string[] items)
        {
            CoordinateRecord record = new CoordinateRecord();
            record.Id = items[1];
            record.B =  Double.Parse( readB(items[2] + "°" + items[3]));
            record.L =Double.Parse( readB(items[4] + "°" + items[5]));
            record.H = Double.Parse(  items[6]);
            record.MB = Double.Parse( items[7]);
            record.ML =  Double.Parse( items[8]);
            record.MH =  Double.Parse( items[9]);
            record.IsKnown = ((record.MB == 0) && (record.ML == 0) && (record.MH == 0));
 
            return record;
        }

        private string readB(string value)
        {
            string B;
            string NS = value.Substring(value.Length - 1);
            value = value.Substring(0, value.Length - 1);
            B = DFMToDMS(value);
            if (NS == "S")
                B = "-" + B;
            return B;
        }

        private string readL(string value)
        {
            string L;
            string EW = value.Substring(value.Length - 1);
            value = value.Substring(0, value.Length - 1);
            L = DFMToDMS(value);
            if (EW == "W")
                L = "-" + L;
            return L;
        }

        //25°8′54.61340″-->250854.61340
        private string DFMToDMS(string dfm)
        {
            string du, fen, miao, leftMiao, rightMiao;
            int duIndex, fenIndex, miaoIndex, dianIndex;
            duIndex = dfm.IndexOf("°");
            fenIndex = dfm.IndexOf("'");
            miaoIndex = dfm.IndexOf("\"");
            dianIndex = dfm.IndexOf(".");
            du = dfm.Substring(0, duIndex);
            fen = dfm.Substring(duIndex + 1, fenIndex - duIndex - 1);
            fen = fen.Trim();
            fen = fen.PadLeft(2, '0');
            leftMiao = dfm.Substring(fenIndex + 1, dianIndex - fenIndex - 1);
            rightMiao = dfm.Substring(dianIndex + 1, miaoIndex - dianIndex - 1);
            leftMiao = leftMiao.Trim();
            leftMiao.PadLeft(2);
            miao = leftMiao + "." + rightMiao;
            return (du + fen + miao);
        }
        #endregion
    }
}
