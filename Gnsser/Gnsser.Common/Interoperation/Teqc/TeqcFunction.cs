using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Interoperation.Teqc
{
    ///// <summary>
    ///// TEQC 主要功能（三大功能）。
    ///// </summary>
    //public enum TeqcMainFunction
    //{
    //    /// <summary>
    //    /// 转换
    //    /// </summary>
    //    Translation,
    //    /// <summary>
    //    /// 编辑
    //    /// </summary>
    //    Editing,
    //    /// <summary>
    //    /// 质量检核
    //    /// </summary>
    //    QualityCheck
    //}

    /// <summary>
    /// 功能
    /// </summary>
    public enum TeqcFunction
    {
        /// <summary>
        /// 无任何操作
        /// </summary>
        None,
        /// <summary>
        /// 查看元数据。
        /// </summary>
        ViewMetadata,
        /// <summary>
        /// 质量检核
        /// </summary>
        QualityChecking,
        /// <summary>
        /// 转换
        /// </summary>
        Translation

    }
}
