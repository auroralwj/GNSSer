//2015.01.10, czs, create in namu,  数据遍历器  StreamDataWalker
//2016.05.02, czs, edit in hongqing, 数据矫正管理器

using System;
using System.Collections.Generic;
using System.Text;
using System.IO; 
using Geo; 
using Geo.Utils;
using Geo.Common; 

namespace Geo
{
   /// <summary>
    /// 数据矫正管理器
    /// </summary> 
    /// <typeparam name="TMaster">待处理(主人，访问者)类型</typeparam>
    public class ReviserManager<TMaster> : Reviser<TMaster>
    {
        /// <summary>
        /// 数据处理链条
        /// </summary>
        public ReviserManager()
        {
            this.Name = typeof(TMaster).Name + " 矫正管理器";
            this.Precessors = new List<Geo.IReviser<TMaster>>();
            IsBreakWhenFailed = true;
        }
        /// <summary>
        /// 当前出现一个false返回，是否断开。
        /// </summary>
        public bool IsBreakWhenFailed { get; set; }

        /// <summary>
        /// 处理器。访问者设计模式。
        /// </summary>
        public List<IReviser<TMaster>> Precessors { get; set; }

        /// <summary>
        /// 获取修正器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetReviser<T>() where T:IReviser<TMaster>
        {
            foreach (var item in Precessors)
            {
                if (item is T) { return (T)item; }
            }
            return default(T);
        }

        /// <summary>
        /// 遍历数据
        /// </summary> 
        public override bool Revise(ref TMaster data)
        {
            if (data == null) return true;

            foreach (var precessor in Precessors)
            {
                var obj = data;
                precessor.Buffers = this.Buffers;
                if (!precessor.Revise(ref obj) && IsBreakWhenFailed)
                {
                    this.Message = precessor.Message;
                    log.Warn("校正中断于 " + precessor.Name + " " + precessor.GetType() + ", " +this.Message);
                    return false;
                    throw new Exception(precessor.Message);
                } 
            }
            return true;
        }


        /// <summary>
        /// 添加一个历元信息处理器。
        /// </summary>
        /// <param name="index">编号</param>
        /// <param name="processor">历元信息处理器</param>
        public void InsertProcessor(int index, IReviser<TMaster> processor)
        {
            this.Precessors.Insert(index, processor);
        }

        /// <summary>
        /// 添加一个历元信息处理器。
        /// </summary>
        /// <param name="processor">历元信息处理器</param>
        public void AddProcessor(IReviser<TMaster> processor)
        {
            this.Precessors.Add(processor);
        }
        /// <summary>
        /// 批量添加历元处理器
        /// </summary>
        /// <param name="processor">批量历元信息处理器</param>
        public void AddProcessor(IEnumerable<IReviser<TMaster>> processor)
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

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + ", Count： " + Precessors.Count );
            if (this.Precessors.Count > 0)
            {
                sb.AppendLine(", { ");
                int i = 1;
                foreach (var p in Precessors)
                {
                    if (i != 1) { sb.AppendLine(","); }
                    sb.Append(p.ToString());

                    i++;
                }
                sb.Append("}");
            }
            return sb.ToString();
        }
    }
   /// <summary>
    /// 数据遍历处理器
    /// </summary>
    /// <typeparam name="TMaster">待处理(主人，访问者)类型</typeparam>
    public class TwinsReviserManager<TMaster> : TwinsReviser<TMaster>
    {
        /// <summary>
        /// 数据处理链条
        /// </summary>
        public TwinsReviserManager()
        {
            this.Precessors = new List<Geo.ITwinsReviser<TMaster>>();
        }

        /// <summary>
        /// 处理器。访问者设计模式。
        /// </summary>
        public List<ITwinsReviser<TMaster>> Precessors { get; set; }

        /// <summary>
        /// 遍历数据
        /// </summary> 
        public override bool Revise(ref TMaster data, ref TMaster dataB)
        {
            foreach (var precessor in Precessors)
            {
                var obj = data;
                var objB = dataB;
                if (!precessor.Revise(ref obj,ref objB ))
                {
                    this.Message = precessor.Message;
                    log.Debug(this.Message);
                    return false;
                    throw new Exception(precessor.Message);
                }
            }
            return true;
        }


        /// <summary>
        /// 添加一个历元信息处理器。
        /// </summary>
        /// <param name="index">编号</param>
        /// <param name="processor">历元信息处理器</param>
        public void InsertProcessor(int index, ITwinsReviser<TMaster> processor)
        {
            this.Precessors.Insert(index, processor);
        }

        /// <summary>
        /// 添加一个历元信息处理器。
        /// </summary>
        /// <param name="processor">历元信息处理器</param>
        public void AddProcessor(ITwinsReviser<TMaster> processor)
        {
            this.Precessors.Add(processor);
        }
        /// <summary>
        /// 批量添加历元处理器
        /// </summary>
        /// <param name="processor">批量历元信息处理器</param>
        public void AddProcessor(List<ITwinsReviser<TMaster>> processor)
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

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        { 
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + ", Count： " + Precessors.Count);
            if (this.Precessors.Count > 0)
            {
                sb.AppendLine(", { ");
                int i = 1;
                foreach (var p in Precessors)
                {
                    if (i != 1) { sb.AppendLine(","); }
                    sb.Append(p.ToString());

                    i++;
                }
                sb.Append("}");
            }
            return sb.ToString();
        }
    }
     
}