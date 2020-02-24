//2014.12.25, czs, edit, 将 IFileCollectionEphemerisService 重构为 IMultiFileEphemerisService，意为多文件数据源星历服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Service;
using Geo.Coordinates;

namespace Gnsser
{
 
    /// <summary>
    /// 文件集合数据源。争取能够合并多个星历数据源，从而提供连续的星历服务。
    /// </summary>
    public interface IMultiFileEphemerisService : IMultiSourceEphemerisService
    {

    } 

}
