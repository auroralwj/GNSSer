//2015.09.30, czs, create in xi'an hongqing, 数据处理引擎

using System;
using Geo.IO;
using Geo.Utils;
using Geo.IO;
using Geo;
using Geo.Times;
//using Geo.IO;


namespace Geo
{
    /// <summary>
    /// 操作注册管理中心。操作首先在此注册后，才可以使用。
    /// </summary>
    public class OperationManager<TOperation> : BaseDictionary<String, TOperation>
        where TOperation : IOperation
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OperationManager()
            : base("操作注册管理中心")
        {

        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="operation"></param>
        public void Regist(TOperation operation)
        {
            this.Add(operation.Name, operation);
        }
         
    }
    /// <summary>
    /// 操作注册管理中心。操作首先在此注册后，才可以使用。
    /// </summary>
    public class OperationManager : OperationManager<IOperation>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OperationManager() 
        {

        }
         
    }
}
