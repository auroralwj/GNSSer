//2015.09.29, czs, create in xi'an hongqing, Gnsser文件命名规则

using System;
using System.Text;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;


namespace Geo.IO
{
    /// <summary>
    /// Gnsser 文件类型
    /// </summary>
    public enum GnsserFileType
    {
        /// <summary>
        /// 默认什么都没有
        /// </summary>
        None,
        /// <summary>
        /// 配置文件
        /// </summary>
        Conf,
        /// <summary>
        /// 元数据文件
        /// </summary>
        Gmeta,
        /// <summary>
        /// 数据文件
        /// </summary>
        Data,
        /// <summary>
        /// 数据引擎文件
        /// </summary>
        Gpe,
        /// <summary>
        /// 操作文件
        /// </summary>
        Oper,
        /// <summary>
        /// 参数文件，实质是数据文件
        /// </summary>
        Param,
        /// <summary>
        /// 执行脚本文件
        /// </summary>
        GScript
    }



    /// <summary>
    /// Gnsser文件命名规则。
    /// 提供文件名称和类型，生成完整了文件名称。
    /// </summary>
    public class GnsserFileNamer : AbstractBuilder<string, string>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SelfExtension">自定义后缀名</param>
        /// <param name="basicFileType">基础文件类型</param>
        /// <param name="classifyType">分类文件类型</param>
        public GnsserFileNamer( string SelfExtension, GnsserFileType basicFileType = GnsserFileType.Data, GnsserFileType classifyType = GnsserFileType.None)
        {
            this.SelfExtension = SelfExtension;
            this.BasicFileType = basicFileType;
            this.ClassifyType = classifyType;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SelfExtension">自定义后缀名</param>
        /// <param name="basicFileType">基础文件类型</param>
        /// <param name="classifyType">分类文件类型</param>
        public GnsserFileNamer(GnsserFileType basicFileType = GnsserFileType.Data, string SelfExtension = null, GnsserFileType classifyType = GnsserFileType.None)
        {
            this.SelfExtension = SelfExtension;
            this.BasicFileType = basicFileType;
            this.ClassifyType = classifyType;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 文件自有后缀。如 .xyz，.blh 。
        /// 若非空，则为最后的后缀。
        /// </summary>
        public string SelfExtension { get; set; }
        /// <summary>
        /// 基础文件类型.如 Data
        /// 为倒数第二后缀。
        /// </summary>
         public GnsserFileType BasicFileType{get;set;}
        /// <summary>
         /// 分类文件类型. 如 Oper。
         /// 为倒数第三后缀。
        /// </summary>
         public GnsserFileType ClassifyType { get; set; }
        #endregion
         
        /// <summary>
        /// 构建。
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
         public override string Build(string filePathWithoutExtension)
         {
             var extension = "";
             if (!String.IsNullOrWhiteSpace(SelfExtension))
             {
                 extension = SelfExtension;
             }
             if (BasicFileType != GnsserFileType.None && BasicFileType != GnsserFileType.Data)
             {
                 extension = "." + BasicFileType.ToString() + extension;
             }
             if (ClassifyType != GnsserFileType.None)
             {
                 extension = "." + ClassifyType.ToString() + extension;
             }
             return filePathWithoutExtension + extension;
         }

        #region 默认命名器
        /// <summary>
        /// 数据文件命名器
        /// </summary>
         //public static  GnsserFileNamer GnsserFileNamer
         //{
         //    get
         //    {
         //        return new GnsserFileNamer( GnsserFileType.Data,)
         //    }
         //}

        #endregion
    }
}
