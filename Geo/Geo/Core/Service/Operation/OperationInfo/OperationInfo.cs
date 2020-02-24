//2015.09.29, czs, create in xi'an hongqing,操作参数文件

using System;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Geo.Common;
using Geo.Coordinates;

namespace Geo
{
    /// <summary>
    /// 操作文件
    /// </summary>
    public class OperationInfo : RowClass, Namable
    {
        /// <summary>
        /// 默认信息
        /// </summary>
        public OperationInfo() {
            this.Depends = new List<string>();

            this.OrderedProperties = new List<string>()
            {
                Geo.Utils.ObjectUtil.GetPropertyName<OperationInfo>( m=>m.Name ),
                Geo.Utils.ObjectUtil.GetPropertyName<OperationInfo>( m=>m.OperationName ),
                Geo.Utils.ObjectUtil.GetPropertyName<OperationInfo>( m=>m.ParamFilePath ),
                Geo.Utils.ObjectUtil.GetPropertyName<OperationInfo>( m=>m.DependsString ), 

            };
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="paramFilePath"></param>
        /// <param name="Depends"></param>
        public OperationInfo(string operationName, string paramFilePath, List<string> Depends = null)
        { 
            this.OperationName = operationName;
            this.ParamFilePath = paramFilePath;
            if (Depends == null)
            { 
                this.Depends = new List<string>();
            }
            else
            {
                this.Depends = Depends;
            }
        }

        #region 核心属性
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "任务名称")]
        public string Name { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "操作名称")]
        public string OperationName { get; set; }
        /// <summary>
        /// 参数文件路径,一般用相对路径。
        /// 保存时采用相对路径，提取时采用绝对路径。
        /// </summary>
        [Display(Name = "参数文件路径")]
        public string ParamFilePath { get; set; }
        /// <summary>
        /// 依赖列表
        /// </summary>
        public List<string> Depends { get; set; }

        /// <summary>
        /// 是否具有依赖
        /// </summary>
        public bool HasDepends { get { return Depends.Count > 0; } }
        #endregion

        /// <summary>
        /// 参数类型名称
        /// </summary>
        public string ParamTypeName
        {
            get
            {
                //第二后缀,标识参数类型，这样设计免得还要建立一个操作与参数类型的关联 //czs, 2015.10.22
                return  Geo.Utils.PathUtil.GetExtension(ParamFilePath, 1).Replace(".", "");
            }
        }

        /// <summary>
        /// 依赖字符串
        /// </summary>
        [Display(Name = "依赖任务名称")]
        public string DependsString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (var item in Depends)
                {
                    if (i != 0)
                    {
                        sb.Append("-");
                    }
                    sb.Append(item);
                    i++;
                } 
                return sb.ToString();
            }
            set
            {
                string [] strs = value.Split(new char['-'],  StringSplitOptions.RemoveEmptyEntries);
                this.Depends = new List<string>();
                foreach (var item in strs)
                {
                    if (!String.IsNullOrWhiteSpace(item))
                    {
                       this.Depends.Add(item.Trim()); 
                    }
                }
            }
        }

        #region override

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //如 ",5.3" 表示分隔符为逗号，字符宽5，小数为3位
            return Name + "\t" + OperationName + "\t" + ParamFilePath +  "\t"+DependsString;// new EnumerableFormatter().Format(",", Depends, null);
        }

        /// <summary>
        /// 相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var o = obj as OperationInfo;
            if (o == null) { return false; }
            return ( o.Name == Name && o.OperationName == OperationName && o.ParamFilePath == ParamFilePath);
        }

        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hasCode = 0;
            if (Name != null)
            {
                hasCode += Name.GetHashCode();
            }
            if (OperationName != null)
            {
                hasCode += OperationName.GetHashCode();
            }
            if (ParamFilePath != null)
            {
                hasCode += ParamFilePath.GetHashCode();
            }
            return hasCode;
        }
        #endregion 
    }
}
