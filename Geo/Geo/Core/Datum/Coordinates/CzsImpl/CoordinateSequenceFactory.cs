//2014.06.05, czs, create

using System;
using System.Collections.Generic;
using Geo.Referencing;
using System.Collections;

namespace Geo.Coordinates
{
    /// <summary>
    /// 同一参考系下的坐标序列的创建。
    /// </summary>
    public class CoordinateSequenceFactory : ICoordinateSequenceFactory
    {
        /// <summary>
        /// 创建一个坐标序列。
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public ICoordinateSequence Create(IEnumerable<ICoordinate> coordinates)
        {
            return new CoordinateSequence(coordinates);
        }
    }
}
