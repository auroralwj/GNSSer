//2015.09.29, czs, create in xi'an hongqing, 数据处理引擎

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
    /// 操作。
    /// </summary>
    public abstract class Operation :  Named, Geo.IOperation
    {
        protected Log log = new Log(typeof(Operation));


        /// <summary>
        /// 信息反馈事件
        /// </summary>
        public event StatedMessageProducedEventHandler StatedMessageProduced;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Operation()
        {
            this.Name = this.GetType().Name;
        }
        /// <summary>
        /// 操作信息
        /// </summary>
        public OperationInfo OperationInfo { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public abstract Type ParamType { get; }
        /// <summary>
        /// 是否取消执行
        /// </summary>
        public bool IsCancel { get; set; }
        /// <summary>
        /// 数据处理信息。掌握处理过程。通常有的选择或错误不至于抛出异常，则采用这种方式对外通知。
        /// </summary>
        public StatedMessage StatedMessage { get; set; }
        /// <summary>
        /// 接收并检核操作信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Accept(OperationInfo info){
            this.OperationInfo = info;
            if (!String.Equals(this.Name, info.OperationName, StringComparison.OrdinalIgnoreCase))
            { 
                var Message = "操作名称不符! " + this.Name + " != " + info.OperationName;       
                OnStatedMessageProduced(   StatedMessage.GetFailed(Message)   );

                return false; 
            }
            return true;
        }
        /// <summary>
        /// 执行过程中的信息反馈
        /// </summary>
        protected virtual void OnStatedMessageProduced(StatedMessage StatedMessage)
        {
            if (StatedMessageProduced != null)
            {
                StatedMessageProduced(StatedMessage);
            }
        }  
        protected virtual void OnStatedMessageProduced()
        {
            if (StatedMessageProduced != null)
            {
                StatedMessageProduced(StatedMessage);
            }
        }



        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public abstract bool Do();

        #region override
        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Operation o = obj as Operation;
            if (o == null) { return false; }

            return Name.Equals(o.Name);
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        #endregion
    } 
     
}
