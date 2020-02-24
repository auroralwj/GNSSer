//2014.10.24, czs, create in namu shuangliao, 通用数据源接口

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 数据源为文件。
    /// </summary> 
    public interface IFileBasedService<TProduct> : IOptionalService<FileOption, TProduct>  
    {
    }
}
