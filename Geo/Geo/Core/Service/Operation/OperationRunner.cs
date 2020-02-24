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
    /// 数据遍历处理器
    /// </summary>  
    public class OperationRunner 
    {
        /// <summary>
        /// 数据处理链条
        /// </summary>
        public OperationRunner()
        {
            this.Precessors = new List<Operation>();
        }

        /// <summary>
        /// 处理器。访问者设计模式。
        /// </summary>
        public List<Operation> Precessors { get; set; }

        /// <summary>
        /// 遍历数据
        /// </summary> 
        public   bool Run( )
        {
            foreach (var precessor in Precessors)
            { 
                if (!precessor.Do( ))
                { 
                    //log.Debug(this.Message);
                    return false;
                    throw new Exception(precessor.StatedMessage.Message);
                }
            }
            return true;
        }


        /// <summary>
        /// 添加一个历元信息处理器。
        /// </summary>
        /// <param name="index">编号</param>
        /// <param name="processor">历元信息处理器</param>
        public void InsertProcessor(int index,Operation processor)
        {
            this.Precessors.Insert(index, processor);
        }

        /// <summary>
        /// 添加一个历元信息处理器。
        /// </summary>
        /// <param name="processor">历元信息处理器</param>
        public void AddProcessor(Operation processor)
        {
            this.Precessors.Add(processor);
        }
        /// <summary>
        /// 批量添加历元处理器
        /// </summary>
        /// <param name="processor">批量历元信息处理器</param>
        public void AddProcessor(List<Operation> processor)
        {
            this.Precessors.AddRange(processor);
        }
        /// <summary>
        /// 清除所有的处理器
        /// </summary>
        public void Clear()
        {
            this.Precessors.Clear();
        }
    } 
}
