//2016.02.23, czs, edit in hongqing, 测量值总伴随着误差，单值，双值或三值

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{ 

    /// <summary>
    /// 具有误差的数值接口
    /// </summary>
    public interface ISurveyValue : INumeralValue
    {
        /// <summary>
        /// 标准差。中误差。
        /// </summary>
        double? Error { get; set; }
    }

    /// <summary>
    /// 具有2个测量值接口
    /// </summary>
    public interface ISurveyTwoValue : ISurveyValue
    {
        /// <summary>
        /// 测量B值
        /// </summary>
        double ValueB { get; set; }
        /// <summary>
        /// 标准差B。中误差B。
        /// </summary>
        double? ErrorB { get; set; }
    }

    /// <summary>
    ///具有3个测量值接口
    /// </summary>
    public interface ISurveyTriValue : ISurveyTwoValue
    {
        /// <summary>
        /// 测量C值
        /// </summary>
        double ValueC { get; set; }
        /// <summary>
        /// 标准差C。中误差C。
        /// </summary>
        double? ErrorC { get; set; }
    } 

}
