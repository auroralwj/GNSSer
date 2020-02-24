//2014.10.29, czs, create in numu, 二进制分隔符

using System;
using Geo.Algorithm;
//using Geo.Common;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 二进制分隔符
    /// </summary>
    public  class BinarySpliter
    {
        /// <summary>
        /// 二进制文件的开始
        /// </summary>
        public static Char StartOfFile { get { return AsiiUtil.GetChar(1); } }

        /// <summary>
        /// 二进制文件的开始
        /// </summary>
        public static Char EndOfFile { get { return AsiiUtil.GetChar(2); } }
        /// <summary>
        /// 行参数的开始
        /// </summary>
        public static Char StartOfRowParams { get { return AsiiUtil.GetChar(3);  } }
        /// <summary>
        /// 行参数的结束
        /// </summary>
        public static Char EndOfRowParams { get { return AsiiUtil.GetChar(4); } }
        /// <summary>
        /// 列参数的开始
        /// </summary>
        public static Char StartOfColParams { get { return AsiiUtil.GetChar(5);  } }
        /// <summary>
        /// 列参数的结束
        /// </summary>
        public static Char EndOfColParams { get { return AsiiUtil.GetChar(6); } }
        /// <summary>
        /// 头部信息的开始
        /// </summary>
        public static Char StartOfHeader { get { return AsiiUtil.GetChar(7); ; } }
        /// <summary>
        /// 头部信息的结束
        /// </summary>
        public static Char EndOfHeader { get { return AsiiUtil.GetChar(8); } }
        /// <summary>
        /// 主题内容部分的开始
        /// </summary>
        public static Char StartOfContent { get { return AsiiUtil.GetChar(9);  } }
        /// <summary>
        /// 主题内容部分的结束
        /// </summary>
        public static Char EndOfContent { get { return AsiiUtil.GetChar(10); } }



    }
}
