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
    /// 致谢
    /// </summary>
    public class FileAcknowledgement : IBlockItem
    {
        public string Name { get; set; }
        public string Val { get; set; }

        public override bool Equals(object obj)
        {
            FileAcknowledgement site = obj as FileAcknowledgement;
            return site == null ? false : Name.Equals(site.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return " " + StringUtil.FillSpace(Name, 3) + " " + StringUtil.FillSpaceLeft(Val.ToString(), 75);
        }

        public  IBlockItem Init(string line)
        {
            this.Name = line.Substring(1, 3).Trim();
            this.Val =  line.Substring(5).Trim();
            return this;
        }
    }
}
