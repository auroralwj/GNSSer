//2019.01.15, czs, create in hmx, 对象字典

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{

    /// <summary>
    /// 对象字典
    /// </summary>
    public interface IObjectRow
    {
        /// <summary>
        /// 对象字典。
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Object> GetObjectRow();
    }

}
