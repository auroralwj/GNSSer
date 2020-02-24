//2014.10.29, czs, create in numu, 阿斯玛工具
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace Geo.Utils
{
    /// <summary>
    /// 双精度类型实用工具类。
    /// </summary>
    public class AsiiUtil
    {
        /// <summary>
        /// Char
        /// </summary>
        /// <param name="i">数字</param>
        /// <returns></returns>
        public static Char GetChar(int i) { return (Char)i; }
        /// <summary>
        /// 头文件开始
        /// </summary>
        public static Char StartOfHeader { get { return (Char)(1); } }
        /// <summary>
        /// 文本开始
        /// </summary>
        public static Char StartOfText { get { return (Char)(2); } }
        /// <summary>
        ///文本结束
        /// </summary>
        public static Char EndOfText { get { return (Char)(3); } }
        /// <summary>
        /// 传输结束
        /// </summary>
        public static Char EndOfTransfer { get { return (Char)(4); } }
        /// <summary>
        /// 传输块的结束
        /// </summary>
        public static Char EndOfBlock { get { return (Char)(17); } }
        public static Char DeviceContrlA { get { return (Char)(11); } }
        public static Char DeviceContrlB { get { return (Char)(12); } }
        public static Char DeviceContrlC { get { return (Char)(13); } }
        public static Char DeviceContrlD { get { return (Char)(14); } }
    }
}
