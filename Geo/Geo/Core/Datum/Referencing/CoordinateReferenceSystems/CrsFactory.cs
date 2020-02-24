//2014.06.12, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 坐标参考系统工厂。坐标参考系统，通常由坐标系统和坐标基准构成。
    /// </summary>
    public class CrsFactory : ICrsFactory
    {

        /// <summary>
        /// 创建一个坐标参考系统工厂
        /// </summary>
        public CrsFactory() { }

        /// <summary>
        /// 创建一个坐标参考系。
        /// </summary>
        /// <param name="coordinateSystem">坐标系统</param>
        /// <param name="datum">基准</param>
        /// <returns></returns>
        public ICoordinateReferenceSystem Create(ICoordinateSystem coordinateSystem, IDatum datum)
        {
            return new CoordinateReferenceSystem(coordinateSystem, datum);
        }
             
    }
}