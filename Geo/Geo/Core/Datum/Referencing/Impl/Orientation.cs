//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{
    /// <summary>
    /// 指向，坐标轴的指向。
    /// </summary>
    public class Orientation : IOrientation
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Direction">方向枚举</param>
        /// <param name="name">方向名称或描述</param>
        public Orientation(Direction Direction, string name = null)
        {
            this.Name = name;
            this.Direction = Direction;
        }

        /// <summary>
        /// 方向名称或描述。有的时候枚举无法列出，如Other，就需要用此属性描述。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public Direction Direction { get; set; }
        /// <summary>
        /// 同时比较名称和方向
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Orientation o = obj as Orientation;

            if (o == null) return false;

            return Name == o.Name && Direction == o.Direction; 
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode() + Direction.GetHashCode() * 13;
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name  + ":" + Direction;
        }
    }

}
