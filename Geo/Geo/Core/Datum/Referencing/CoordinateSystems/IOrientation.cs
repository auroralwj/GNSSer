using System;
namespace Geo.Referencing
{
    /// <summary>
    /// 指向。如坐标轴的指向。
    /// </summary>
    public  interface IOrientation
    {
        /// <summary>
        /// 方向名称或描述。有的时候枚举无法列出，如Other，就需要用此属性描述。
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 方向枚举。
        /// </summary>
        Direction Direction { get; set; }
    }
}
