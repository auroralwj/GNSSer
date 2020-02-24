//2016.02.09, czs, create in xi'an hongqing, 属性名称字符串

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using  Geo.Common;
using Geo.IO;

namespace Geo.IO
{ 

    /// <summary>
    /// 配置文件内容。
    /// 通常为一行一个变量和值。采用分隔符分开。
    /// </summary>
    public class NameProperty : Named
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public NameProperty(string name)
            : base(name)
        {
        }

    }

    /// <summary>
    /// 具有单位的值类型
    /// </summary>
    public class ValueProperty : NameProperty
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        public ValueProperty(string name, Unit unit):base(name)
        {
            this.Unit = unit; 
        }

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit { get; set; }
        
        #region 方法
        public override string ToString()
        {
            if (Unit != null)
            {
                return this.Name + "(" + Unit.Name + ")";
            }
            return this.Name;
        }

        public override bool Equals(object obj)
        {
            var o = (obj as ValueProperty);
            if (o == null) return false;

            return o.Name == this.Name && this.Unit.Equals(o.Unit);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Unit.GetHashCode() * 3;
        }
        #endregion 


        #region 属性
        /// <summary>
        /// 名称，与对象属性Name命名冲突，故改用NameProperty
        /// </summary>
        static public ValueProperty NameProperty
        {
            get
            {
                return new ValueProperty("Name", null);
            }
        }
        /// <summary>
        /// 名称，与对象属性Name命名冲突，故改用NameProperty
        /// </summary>
        static public ValueProperty ValueName
        {
            get
            {
                return new ValueProperty("Value", null);
            }
        }
        static public ValueProperty XMeter
        {
            get
            {
                return new ValueProperty("X", Unit.Meter);
            }
        }
        static public ValueProperty YMeter
        {
            get
            {
                return new ValueProperty("Y", Unit.Meter);
            }
        }
        static public ValueProperty ZMeter
        {
            get
            {
                return new ValueProperty("Z", Unit.Meter);
            }
        }

        static public ValueProperty HeightMeter
        {
            get
            {
                return new ValueProperty("Height", Unit.Meter);
            }
        }
        static public ValueProperty LonDegree
        {
            get
            {
                return new ValueProperty("Lon", Unit.Degree);
            }
        }
        static  public ValueProperty LatDegree
        {
            get
            {
                return new ValueProperty("Lat", Unit.Degree);
            }
        }
        static public ValueProperty LonDMS_S
        {
            get
            {
                return new ValueProperty("Lon", Unit.DMS_S);
            }
        }
        static public ValueProperty LatDMS_S
        {
            get
            {
                return new ValueProperty("Lat", Unit.DMS_S);
            }
        }
        #endregion

    }     
}
