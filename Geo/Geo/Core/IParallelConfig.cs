//2018.03.22, czs, edit in hmx, 增加默认

using System;

namespace Geo
{
    /// <summary>
    /// 并行计算接口
    /// </summary>
     public  interface IParallelConfig
    {
         /// <summary>
         /// 是否启用并行计算
         /// </summary>
        bool EnableParallel { get; set; }
         /// <summary>
         /// 并行计算选项
         /// </summary>
        System.Threading.Tasks.ParallelOptions ParallelOptions { get; }
    }
    /// <summary>
    /// 并行技术控制
    /// </summary>
     public class ParallelConfig : IParallelConfig
     {
         /// <summary>
         /// 默认构造函数
         /// </summary>
         public ParallelConfig()
         {
             EnableParallel = true;
             this.ParallelOptions = new System.Threading.Tasks.ParallelOptions();
             ParallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;
         }

         /// <summary>
         /// 指名参数的构造函数。
         /// </summary>
         /// <param name="MaxDegreeOfParallelism"></param>
         /// <param name="enabled"></param>
         public ParallelConfig(int MaxDegreeOfParallelism , bool enabled = true)
         {
             EnableParallel = enabled;
             this.ParallelOptions = new System.Threading.Tasks.ParallelOptions();
             ParallelOptions.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
         }

         /// <summary>
         /// 是否启用并行计算
         /// </summary>
         public bool EnableParallel { get; set; }
         /// <summary>
         /// 并行计算选项
         /// </summary>
         public System.Threading.Tasks.ParallelOptions ParallelOptions { get; set; }
        /// <summary>
        /// 默认
        /// </summary>
        public static ParallelConfig Default { get { return new ParallelConfig() { EnableParallel = true, ParallelOptions  = new System.Threading.Tasks.ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount } }; } }
    }
}
