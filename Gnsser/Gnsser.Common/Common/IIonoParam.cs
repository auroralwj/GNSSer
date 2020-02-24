//2017.09.11, edit in hongqing, 参数名称重构去掉Ion前缀


using System;



namespace Gnsser
{
    /// <summary>
    /// 电离层参数
    /// </summary>
    public interface IIonoParam
    {
        /// <summary>
        /// AlfaA0
        /// </summary>
        double AlfaA0 { get; set; }
        /// <summary>
        /// AlfaA1
        /// </summary>
        double AlfaA1 { get; set; }
        /// <summary>
        /// AlfaA2
        /// </summary>
        double AlfaA2 { get; set; }
        /// <summary>
        /// AlfaA3 
        /// </summary>
        double AlfaA3 { get; set; }
        /// <summary>
        /// BetaB0
        /// </summary>
        double BetaB0 { get; set; }
        /// <summary>
        /// BetaB1
        /// </summary>
        double BetaB1 { get; set; }
        /// <summary>
        /// BetaB2 
        /// </summary>
        double BetaB2 { get; set; }
        /// <summary>
        /// BetaB3
        /// </summary>
        double BetaB3 { get; set; }
        /// <summary>
        /// 是否具有电离层参数
        /// </summary>
        bool HasValue { get; }

    }
}
