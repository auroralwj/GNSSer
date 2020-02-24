//2015.09.30, czs, create in K879西安到宝鸡列车上, 具有参数文件的操作

using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo.IO;

namespace Geo
{


    /// <summary>
    /// 基于参数(文件)的操作。具有特定的目录。
    /// </summary>
    public abstract class ParamBasedOperation<TParam> : Operation
        where TParam : IOrderedProperty
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ParamBasedOperation() : base()
        {
        }
        /// <summary>
        /// 当前参数
        /// </summary>
        public TParam CurrentParam { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public override Type ParamType { get { return typeof(TParam);} }
        //Param
        /// <summary>
        /// 接收，东西。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Accept(OperationInfo info)
        {
            var accpted = base.Accept(info);
            if (accpted)
            {
                //检查文件存在性
                if (!File.Exists(info.ParamFilePath))
                {
                    this.StatedMessage = StatedMessage.Faild;
                    this.StatedMessage.Message = "参数文件不存在！" + info.ParamFilePath;
                    return false;
                }
            }

            return accpted;
        }
    } 
}
