//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{
    /// <summary>
    /// 基准。一组计量参照标准，如时间基准、位置基准、质量基准等。
    /// </summary>
    public class Datum : IdentifiedObject, IDatum
    {
        /// <summary>
        /// 创建一个默认实例。
        /// </summary>
          public Datum(){}
        /// <summary>
        /// 创建一个实例。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
          public Datum(string name = null, string id = null)
              : this( DatumType.Other, id, name)
          {
          }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
          public Datum(DatumType type, string name = null, string id = null)
              : base(id, name)
          {
          }
        /// <summary>
		/// 基准类型
		/// </summary>
        public DatumType DatumType { get; set; }

        /// <summary>
        /// 哈希数。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return DatumType.GetHashCode();
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Datum))
                return false;
            Datum datum = (obj as Datum);
            if ((datum.DatumType != Referencing.DatumType.Other) && datum.DatumType == this.DatumType)
                return true;
            return base.Equals(datum);
        }        /// <summary>
        /// 默认以逗号隔开的字符串。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return  Name;
        }

    }
}
