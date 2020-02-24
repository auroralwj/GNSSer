//2016.05.14, czs, create in hongqing, 重写 高次差探测周跳法
//2016.08.04, czs, edit in fujian yongan, 修正

using System; 
using System.Collections.Generic;
using System.Linq; 
using System.Text; 
using Geo; 

namespace Geo
{
    /// <summary>
    /// 窗口数据管理器
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class NumeralWindowDataManager<TKey> : BaseDictionary<TKey, NumeralWindowData>
    { /// <summary>
      /// 构造函数
      /// </summary>
      /// <param name="windowSize"></param>
      /// <param name="MaxBreakCount"></param>
        public NumeralWindowDataManager(int windowSize, int MaxBreakCount = 3)
        {
            this.WindowSize = windowSize;
            this.MaxBreakCount = MaxBreakCount;
        }

        #region 属性 
        /// <summary>
        /// 窗口大小
        /// </summary>
        public int WindowSize { get; set; }
        /// <summary>
        /// 大于此，则分段
        /// </summary>
        public int MaxBreakCount { get; set; }
        #endregion

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override NumeralWindowData Create(TKey key)
        {
            return new NumeralWindowData(WindowSize) { MaxBreakCount = MaxBreakCount };
        }
    }

    /// <summary>
    /// 窗口数据管理器
    /// </summary>
    public class NumeralWindowDataManager : NumeralWindowDataManager<string>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="windowSize"></param>
        public NumeralWindowDataManager(int windowSize, int MaxBreakCount = 3) : base(windowSize, MaxBreakCount)
        {  
        }

    }
}