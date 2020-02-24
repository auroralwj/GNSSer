using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    ///<summary>
    ///坐标序列工厂。
    ///</summary>
    public interface ICoordinateSequenceFactory
    {
        /// <summary>
        /// 创建一个坐标序列。
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        ICoordinateSequence Create(IEnumerable<ICoordinate> coordinates);

    }
}