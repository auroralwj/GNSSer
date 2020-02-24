//2015.02.07, czs, create in pengzhou, 字节转运站，用于接收网络数据。
//2015.10.18, czs, extracted in pengzhou, 提取队列，也可以用系统自带Queue

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{
    /// <summary>
    /// 被挤出队列啦
    /// </summary>
    /// <param name="queue"></param>
    public delegate void DequeueEventHandler<T>(List<T> queue);

    /// <summary>
    /// 队列，也可以用系统自带 Queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseQueue<T>
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Capacity"></param>
        public BaseQueue(int Capacity = 40, bool AutoDequeue = false) 
        {
            this.Capacity = Capacity; 
            this.Queue = new List<T>(Capacity);
            this.AutoDequeue = AutoDequeue;
        }
        #region 属性、事件
        #region 核心存储
        /// <summary>
        /// 数据存储。
        /// </summary>
        public List<T> Queue { get; set; }
        #endregion
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 自动出队。即如果入队数量超过了容量，则自动出队，同时激发 Dequeuing 事件。
        /// </summary>
        public bool AutoDequeue { get; set; }
        /// <summary>
        /// 即将出队。
        /// </summary>
        public event DequeueEventHandler<T> Dequeuing;

        /// <summary>
        /// 缓存大小,单位为个数。
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// 库存实际数量。
        /// </summary>
        public int Count { get { return Queue.Count; } }

        #endregion

        #region 方法 
        /// <summary>
        /// 插入数组到队列
        /// </summary>
        /// <param name="index">编号</param>
        /// <param name="bts">字节数组</param>
        public void Insert(int index, T[] bts)
        {
            Queue.InsertRange(index, bts);
            CheckOrDequeue();
        }

        /// <summary>
        /// 入队,接收数据。
        /// </summary>
        /// <param name="bts">字节数组</param>
        public void Enqueue(T[] bts)
        {
            this.Queue.AddRange(bts);
            CheckOrDequeue();
        }
        /// <summary>
        /// 入队,接收数据。
        /// </summary>
        /// <param name="t"></param>
        public void Enqueue(T  t)
        {
            this.Queue.Add(t);
            CheckOrDequeue();
        }

        /// <summary>
        ///出队， 将先入的推出来。从低字节开始提取。
        /// </summary>
        public List<T> Dequeue(int count = Int32.MaxValue)
        {
            int len = Math.Min(Queue.Count, count);
            var result = Queue.GetRange(0, len);
            Queue.RemoveRange(0, len);

            DoDequeuing(result);

            return result;
        }
        /// <summary>
        /// 出队事件
        /// </summary>
        /// <param name="result"></param>
        protected void DoDequeuing(List<T> result)
        {
            if (Dequeuing != null)
            {
                Dequeuing(result);
            }
        }
        /// <summary>
        /// 清空队列
        /// </summary>
        public void Clear() { this.Queue.Clear(); }

        private void CheckOrDequeue()
        {
            var extra = Count - Capacity;
            if (AutoDequeue && extra > 0) { Dequeue(extra); }
        }

        #endregion

    } 
}