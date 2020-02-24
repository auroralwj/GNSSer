//2014.06.06,czs,created

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Referencing;


namespace Geo.Coordinates
{
    /// <summary>
    /// 最轻量级的 IXYZ  的实现。
    /// </summary>
    public class Xyz :Xy, IXYZ
    {
        public Xyz(double x, double y, double z)
            :base(x,y)
        {
            this.Z = z;
        }
        /// <summary>
        /// Z 分量
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// 值是否全为 0.
        /// </summary>
        public bool IsZero { get { return Z == 0 && base.IsZero; } }
        /// <summary>
        /// 字符串表达
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X + "," + Y + "," + Z;
        }
        /// <summary>
        /// 数值是否相等
        /// </summary>
        /// <param name="obj">待比较对象</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Xyz xyz = obj as Xyz;
            if (xyz == null) return false;

            return base.Equals(xyz) && Z== xyz.Z;
        }
        /// <summary>
        ///  哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() + Z.GetHashCode() * 3;
        }
    }
}