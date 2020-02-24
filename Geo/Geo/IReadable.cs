//2018.03.31, czs, create in hmx, 具有可读性的，提供可读String类型返回

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{
    /// <summary>
    /// 具有可读性的，提供可读String类型返回
    /// </summary>
    public  interface IReadable
    {
        /// <summary>
        /// 提供可读String类型返回
        /// </summary>
        /// <param name="splitter"></param>
        /// <returns></returns>
        string ToReadableText(string splitter = ",");

    }
}
