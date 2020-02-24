using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Gnsser.Times;
using Geo.Times; 
using Geo.Utils;

namespace Gnsser.Data.Sinex
{

    /// <summary>
    /// 文件信息。
    /// +INPUT/HISTORY
    /// *_Version_ Cre __Creation__ Own _Data_start_ _Data_end___ TProduct Param S Type_
    /// +GLK 1.06 SIO 12:365:84606 SIO 12:365:00000 12:365:86370 P 00732 2 S E A
    /// </summary>
    public class InputFile : IBlockItem
    {
        public string CreationAgencyCode { get; set; }
        public Time CreationTime { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }

        public override bool Equals(object obj)
        {
            InputFile site = obj as InputFile;
            return site == null ? false : FileName.Equals(site.FileName);
        }

        public override int GetHashCode()
        {
            return FileName.GetHashCode();
        }
        public override string ToString()
        {
            string line = "";
            line += " " + StringUtil.FillSpace(CreationAgencyCode, 3);
            line += " " + CreationTime.ToYdsString();
            line += " " + StringUtil.FillSpace(FileName, 29);
            line += " " + StringUtil.FillSpace(FileDescription, 32);

            return line;
        }


        public static InputFile ParseLine(string line)
        {
            InputFile b = new InputFile();
            b.CreationAgencyCode = line.Substring(1, 3);
            b.CreationTime = Time.ParseYds(line.Substring(5, 12));
            b.FileName = line.Substring(18, 29);
            b.FileDescription = line.Substring(48, 32);

            return b;
        }

        public  IBlockItem Init(string line)
        {
            this.CreationAgencyCode = line.Substring(1, 3);
            this.CreationTime = Time.ParseYds(line.Substring(5, 12));
            this.FileName = line.Substring(18, 29);
            this.FileDescription = line.Substring(48);

            return this;
        }
    }

}
