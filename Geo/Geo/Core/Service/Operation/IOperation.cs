//2015.09.29, czs, create in xi'an hongqing, 数据处理引擎

using System;
using Geo.IO;


namespace Geo
{
    /// <summary>
    /// 操作接口
    /// </summary>
    public  interface IOperation : Namable, ICancelAbale
    {
        /// <summary>
        /// 处理过程中的信息。
        /// </summary>
        event StatedMessageProducedEventHandler StatedMessageProduced;

        /// <summary>
        /// 输入信息
        /// </summary>
        OperationInfo OperationInfo { get; }
        /// <summary>
        /// 参数类型
        /// </summary>
        Type ParamType { get; }
        /// <summary>
        /// 过程与结果描述信息
        /// </summary>
         StatedMessage StatedMessage { get; }
        /// <summary>
        /// 接收，并检查输入。若不通过检查，则返回false。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool Accept(OperationInfo info);

        /// <summary>
        /// 执行。若遇到不可原谅因素，则返回false。
        /// </summary>
        /// <returns></returns>
        bool Do();

    }
}
