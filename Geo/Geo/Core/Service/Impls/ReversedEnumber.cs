//2017.09.02, czs, create in hongqing, 实现反向数据流

using System;
using System.IO;
using System.Collections.Generic;
using Geo.IO;
using Geo.Common;

namespace Geo
{
    /// <summary>
    /// 实现反向数据流
    /// </summary>
    /// <typeparam name="TMaterial"></typeparam>
    public class ReversedEnumber<TMaterial> : AbstractEnumer<TMaterial>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="Stream"></param>
        public ReversedEnumber(IEnumer<TMaterial> Stream)
        {
            this.DataSource = Stream;
            this.Name = "Reversed_" + Stream.Name;
            Init();
        }
        /// <summary>
        /// 数据
        /// </summary>
        public List<TMaterial> Data { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public IEnumer<TMaterial> DataSource { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            CurrentIndex = 0;
            Data = new List<TMaterial>();
            while (this.DataSource.MoveNext())
            {
                if (this.DataSource.Current == null) { continue; }
                Data.Insert(0, this.DataSource.Current);
            }
        }
        /// <summary>
        /// 向前一步
        /// </summary>
        /// <returns></returns>
        public override bool MoveNext()
        {
            bool isOk = base.MoveNext();
            if (!isOk) { return isOk; }
            if (CurrentIndex <= Data.Count)
            { this.Current = Data[CurrentIndex - 1]; return true; }
            else { return false; }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose() { if (DataSource != null) { DataSource.Dispose(); this.Data.Clear(); DataSource = null; } }
        /// <summary>
        /// 重置数据
        /// </summary>
        public override void Reset() { DataSource.Reset(); }
    }

}