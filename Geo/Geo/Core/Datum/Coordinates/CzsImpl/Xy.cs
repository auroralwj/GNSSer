//2014.06.06,czs,created

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Referencing;


namespace Geo.Coordinates
{
    /// <summary>
    /// 最轻量级的 IXY  的实现。
    /// </summary>
    public class Xy : IXY
    {
        /// <summary>
        /// 创建一个 Xy 实例。
        /// </summary>
        /// <param name="x">X 分量</param>
        /// <param name="y">Y 分量</param>
        public Xy(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        /// <summary>
        /// X 分量
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y 分量
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// 值是否全为 0.
        /// </summary>
        public bool IsZero { get { return X == 0 && Y == 0; } }
        /// <summary>
        /// 字符串表达
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X + "," + Y;
        }
        /// <summary>
        /// 数值是否相等
        /// </summary>
        /// <param name="obj">待比较对象</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Xy xy = obj as Xy;
            if (xy == null) return false;

            return xy.X == X && xy.Y == Y;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() * 13 + Y.GetHashCode() * 37;
        }
    }
}