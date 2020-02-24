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
    /// 注释
    /// +INPUT/HISTORY
    /// *_Version_ Cre __Creation__ Own _Data_start_ _Data_end___ TProduct Param S Type_
    /// +GLK 1.06 SIO 12:365:84606 SIO 12:365:00000 12:365:86370 P 00732 2 S E A
    /// </summary>
    public class FileComment : IBlockItem
    {
        public string Comment { get; set; }

        public override bool Equals(object obj)
        {
            FileComment site = obj as FileComment;
            return site == null ? false : Comment.Equals(site.Comment);
        }

        public override int GetHashCode()
        {
            return Comment.GetHashCode();
        }
        public override string ToString()
        {
            string line = "";
            line += " " + StringUtil.FillSpace(Comment, 79);
            return line;
        }


        public  IBlockItem Init(string line)
        {
            if (String.IsNullOrWhiteSpace(line)) return this;
            this.Comment = Geo.Utils.StringUtil.SubString(line, 1, 79); 
            return this;
        }
    }

}
