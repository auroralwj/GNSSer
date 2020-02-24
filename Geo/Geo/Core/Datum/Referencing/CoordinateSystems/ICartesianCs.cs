using System;
namespace Geo.Referencing
{
    /// <summary>
    /// 笛卡尔坐标系。
    /// </summary>
    interface ICartesianCs :ICoordinateSystem
    {
        bool IsRightHand { get; }
    }
}
