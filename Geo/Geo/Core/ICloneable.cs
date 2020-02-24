//2015.10.17, create in pengzhou, 克隆泛型接口

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 克隆泛型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneable<T>
    {
        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
}
