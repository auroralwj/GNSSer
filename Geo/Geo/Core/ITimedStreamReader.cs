//2016.03.06, czs, create in hongqing, 时间序列数据流读取器

using System;
using System.Collections;
using System.Collections.Generic;
using Geo;
using Geo.IO;
using System.Text;
using Geo.Times;
using System.IO;

namespace Geo.IO{

    /// <summary>
    /// 时间序列数据流读取器
    /// </summary>
    /// <typeparam name="TProduct"></typeparam>
    public interface ITimedStreamReader<TProduct> : IEnumer<TProduct>
    {
        /// <summary>
        /// 尝试获取起始时间，失败则返回最大值
        /// </summary>
        /// <returns></returns>
        global::Geo.Times.Time TryGetStartTime();
        /// <summary>
        /// 尝试获取结束时间，失败则返回最小值
        /// </summary>
        /// <returns></returns>
        global::Geo.Times.Time TryGetEndTime();
    }


    /// <summary>
    ///时间序列数据流读取
    /// </summary>
    public abstract class AbstractTimedStreamReader<TProduct> : Geo.IO.ITimedStreamReader<TProduct>
    {
        protected   ILog log = Log.GetLog(typeof(AbstractTimedStreamReader<TProduct>)); 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        public AbstractTimedStreamReader(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("输入不存在：" + filePath);
            this.InputPath = filePath;
            EnumCount = int.MaxValue / 2;
            this.CurrentIndex = -1;
            StreamReader = new StreamReader(filePath, Encoding.Default);
            this.Name = Path.GetFileName(filePath);
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool  IsCancel { get; set; }
        /// <summary>
        /// 当前编号，从 0 开始。
        /// </summary>
        public int CurrentIndex { get; set; }
        /// <summary>
        /// 起始编号，从0开始。
        /// </summary>
        public int StartIndex { get; set; }
        /// <summary>
        /// 遍历数量，默认为最大值的一半。
        /// </summary>
        public int EnumCount { get; set; }
        /// <summary>
        /// 最大的循环编号
        /// </summary>
        public int MaxEnumIndex { get { return StartIndex + EnumCount; } }
        /// <summary>
        /// 设置遍历数量
        /// </summary>
        /// <param name="StartIndex"></param>
        /// <param name="EnumCount"></param>
        public void SetEnumIndex(int StartIndex, int EnumCount) { this.StartIndex = StartIndex; this.EnumCount = EnumCount; }
        /// <summary>
        /// 路径
        /// </summary>
        protected string InputPath { get; set; }
        /// <summary>
        /// 数据流读取器
        /// </summary>
        protected StreamReader StreamReader { get; set; }

        #region GetEnumerator
        /// <summary>
        /// 获取枚举器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TProduct> GetEnumerator()
        {
            return this;
        }
        /// <summary>
        /// 获取枚举器
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
        /// <summary>
        /// 从文件开始获取时间
        /// </summary>
        /// <returns></returns>
        public abstract Time TryGetStartTime();
        /// <summary>
        /// 从文件末尾获取时间
        /// </summary>
        /// <returns></returns>
        public abstract Time TryGetEndTime();
        /// <summary>
        /// 当前钟差
        /// </summary>
        public TProduct Current { get; protected set; }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (StreamReader != null)
            {
                StreamReader.Dispose();
            }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }
        /// <summary>
        /// 移动到下一个，错误则返回false
        /// </summary>
        /// <returns></returns>
        public abstract bool MoveNext(); 
        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            this.CurrentIndex = -1;
            StreamReader.BaseStream.Position = 0; 
        }
    }
}
