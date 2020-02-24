using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// 定义的符号
    /// </summary>
    public class CharDefinition
    {
        // Character Definition
        //No other character is allowed at the beginning of coeffOfParams line!
        public const string HEADER_TRAILER = "%"; //Header and trailer line,
        public const string COMMENT = "*"; //Comment line within the header and trailer line,
        public const string TITLE_START = "+";//Title at the end of coeffOfParams fileRefferblock
        public const string TITLE_END = "-";// Title at the end of coeffOfParams fileRefferblock
        public const string DATA_LINE = " ";//Data line within coeffOfParams fileRefferblock
    }

}
