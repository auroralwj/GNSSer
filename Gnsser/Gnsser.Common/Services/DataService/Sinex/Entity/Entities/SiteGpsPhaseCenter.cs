//2017.06.14, czs, edit in hongqing, 增加字符串非零判断

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
    /// GPS相位中心信息。
    /// </summary>
    public class SiteGpsPhaseCenter :  IBlockItem
    {
        public string AntennaType { get; set; }
        public string AntennaSerialNumber { get; set; }
        public Geo.Coordinates.HEN UneL1 { get; set; }
        public Geo.Coordinates.HEN UneL2 { get; set; }
        public string AntennaCalibrationModel { get; set; }

        public override bool Equals(object obj)
        {
            SiteGpsPhaseCenter site = obj as SiteGpsPhaseCenter;
            return site == null ? false : AntennaType.Equals(site.AntennaType);
        }

        public override int GetHashCode()
        {
            return AntennaType.GetHashCode();
        }
        public override string ToString()
        {
            string line =
               " " + StringUtil.FillSpaceLeft(AntennaType, 20)
            + " " + StringUtil.FillSpaceLeft(AntennaSerialNumber, 5)
            + " " + UneL1.ToRnxString()
            + " " + UneL2.ToRnxString()
            + " " + StringUtil.FillSpaceLeft(AntennaCalibrationModel, 10)
            ;

            return line;
        }
        
        public  IBlockItem Init(string line)
        { 
            this.AntennaType = line.Substring(1, 20);
            this.AntennaSerialNumber = line.Substring(22, 5);
 
            var item = Geo.Utils.StringUtil.SubString(line, 28, 41);
            if (!String.IsNullOrWhiteSpace(item))
            {
                string[] strs = item.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int i = 0;
                this.UneL1 = new HEN(double.Parse(strs[i++]), double.Parse(strs[i++]), double.Parse(strs[i++]));
                this.UneL2 = new HEN(double.Parse(strs[i++]), double.Parse(strs[i++]), double.Parse(strs[i++]));
                this.AntennaCalibrationModel = Geo.Utils.StringUtil.SubString(line, 69, 10);
            } 

            return this;
        } 
    }

}
