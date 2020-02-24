//2015.11.12, czs, create in xian hongqing, 可并行计算的参数接口

using System;

namespace Geo.IO
{
    /// <summary>
    /// 可并行计算的参数接口
    /// </summary>
    public interface IParallelableParam
    {
        /// <summary>
        /// 是否采用并行计算
        /// </summary>
        bool IsParallel { get; set; }
        /// <summary>
        /// 并行粒度
        /// </summary>
        int ParallelProcessCount { get; set; }
    }

    /// <summary>
    /// 可并行计算的参数默认实现
    /// </summary>
    public class ParallelableParam : IParallelableParam
    {
        /// <summary>
        /// 是否采用并行计算
        /// </summary>
        public bool IsParallel { get; set; }
        /// <summary>
        /// 并行粒度
        /// </summary>
        public int ParallelProcessCount { get; set; }
    }
}
