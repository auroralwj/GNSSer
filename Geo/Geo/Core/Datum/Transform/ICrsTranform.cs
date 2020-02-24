//2014.06.10,czs,edit

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 坐标转换接口,调用前需要指定坐标参考系。
    /// 这里采用责任链模式。
    /// </summary>
    public interface ICrsTranform : ICoordinateTranform<ICoordinate, ICoordinate>
    {
        /// <summary>
        /// 原参考系统
        /// </summary>
        ICoordinateReferenceSystem SourceCrs { get; }

        /// <summary>
        /// 目标参考系统
        /// </summary>
        ICoordinateReferenceSystem TargetCrs { get; }

        /// <summary>
        /// 得到逆向转换孪生兄弟，但有的可能不可逆。
        /// </summary>
        ICrsTranform GetInverse();

        #region 责任链
        /// <summary>
        /// 下一个转换器
        /// </summary>
        ICrsTranform Successor { set; }

        /// <summary>
        /// 匹配，是否是我的责任。
        /// </summary>
        bool IsMatched { get; }
        #endregion

    }
}
