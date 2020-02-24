//2016.11.29, czs, edit in hongqing, 从 Geodesy 中迁移到 Geo 中。

using System;    
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Geo.Data
{
    /// <summary>
    /// Gamit Globk org 文件。
    /// 点名点号都认为相同。
    /// </summary>
    public class GamitOrgFileReader : ItemReader<GlobkOrgItem, GlobkOrgCoordFile>
    {  
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName"></param>
        public GamitOrgFileReader(string fileName) : base(fileName)
        { 
            this.TokenOfStartLine = "PARAMETER ESTIMATES FROM GLOBK Vers";
            this.TokenOfEndLine = "Eph.";
            this.StartTokenOfContentLine = "Unc.";
        }
        //Int. JIXN_GPS -2333012.42150  43333891.96480  40833375.183330   -0.03042   -0.033308   -0.009333 2014.001
        public override GlobkOrgItem ParseRow(string[] items)
        {
            GlobkOrgItem record = new GlobkOrgItem();
            record.Id = items[1].Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)[0];
            //record.Name = record.Number;
            record.X = Double.Parse( items[2]);
            record.Y =Double.Parse( items[3]);
            record.Z = Double.Parse(items[4]);

            if (items.Length >= 7)
            {
                //漂移量 mm/year
                record.Sx = Double.Parse(items[5]);
                record.Sy = Double.Parse(items[6]);
                record.Sz = Double.Parse(items[7]);
                if (items.Length >= 9)
                {
                    record.Description = items[8];//历元
                    if (items.Length >= 11)
                    {
                        record.MX = Double.Parse(items[9]);
                        record.MY = Double.Parse(items[10]);
                        record.MZ = Double.Parse(items[11]);
                        record.IsKnown = ((record.MX == 0) && (record.MY == 0) && (record.MZ == 0));
                    }
                }
            }
             
            return record;
        }
    }

}
