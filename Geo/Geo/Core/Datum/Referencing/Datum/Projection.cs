using System;
//2014.05.24, czs, created 

using System.Collections.Generic;
using System.Linq;
using System.Text;
using  Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
    /// <summary>
    /// 投影。
    /// 存储从 geographic coordinate system 到 projected coordinate system 的转换信息，具有一些列转换参数，如椭球信息。
    /// 所有需要投影坐标转换的类都应该是本类的子类。
    /// </summary>
    public class Projection : IdentifiedObject
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="className">分类名</param>
        /// <param name="parameters">参数</param>
        /// <param name="name">名称</param>
        /// <param name="id">代码</param>
        public Projection(
            string className, 
            List<ProjectionParameter> parameters,
            string name =null,   
            string id =null)
            : base(id, name)
        {
            Parameters = parameters;
            ClassName = className;
        }

        #region Predefined projections
        #endregion

        #region Projection Members

        /// <summary>
        /// Gets the number of parameters of the projection.
        /// </summary>
        public int ParamCount
        {
            get { return Parameters.Count; }
        }

        /// <summary>
        /// Gets or sets the parameters of the projection
        /// </summary>
        public List<ProjectionParameter> Parameters { get; set; }

        /// <summary>
        /// Gets an indexed parameter of the projection.
        /// </summary>
        /// <param name="n">Index of parameter</param>
        /// <returns>n'th parameter</returns>
        public ProjectionParameter GetParameter(int n)
        {
            return Parameters[n];
        }

        /// <summary>
        /// Gets an named parameter of the projection.
        /// </summary>
        /// <remarks>The parameter name is case insensitive</remarks>
        /// <param name="name">Name of parameter</param>
        /// <returns>parameter or null if not found</returns>
        public ProjectionParameter GetParameter(string name)
        {
            foreach (ProjectionParameter par in Parameters)
                if (par.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return par;
            return null;
        }

        /// <summary>
        /// Gets the projection classification name (e.g. "Transverse_Mercator").
        /// </summary>
        public string ClassName { get; protected set; }

        /// <summary>
        /// 数值上是否相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Projection))
                return false;
            Projection proj = obj as Projection;
            if (proj.ParamCount != this.ParamCount)
                return false;
            for (int i = 0; i < Parameters.Count; i++)
            {
                ProjectionParameter param = GetParameter(proj.GetParameter(i).Name);
                if (param == null)
                    return false;
                if (param.Value != proj.GetParameter(i).Value)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 哈希数。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int code = 0;
            foreach (var item in Parameters)
            {
                code += item.GetHashCode();
            }
            return code;
        }

        #endregion
    }

    /// <summary>
    /// A named projection parameter value.
    /// </summary>
    /// <remarks>
    /// The linear units of parameters' values match the linear units of the containing 
    /// projected coordinate system. The angular units of parameter values match the 
    /// angular units of the geographic coordinate system that the projected coordinate 
    /// system is based on. (Notice that this is different from <see cref="Parameter"/>,
    /// where the units are always meters and degrees.)
    /// </remarks>
    public class ProjectionParameter : Parameter
    {
        public ProjectionParameter() { }
        /// <summary>
        /// Initializes an instance of a ProjectionParameter
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Parameter value</param>
        public ProjectionParameter(string name, double value) : base(name, value) { }
    }
    /// <summary>
    /// A named parameter value.
    /// </summary>
    public class Parameter
    {
        public Parameter() { }
        /// <summary>
        /// Creates an instance of a parameter
        /// </summary>
        /// <remarks>Units are always either meters or degrees.</remarks>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Value</param>
        public Parameter(string name, double value)
        {
            Name = name;
            Value = value;
        }
        /// <summary>
        /// Parameter name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Parameter value
        /// </summary>
        public double Value { get; set; }
    }
     

}
