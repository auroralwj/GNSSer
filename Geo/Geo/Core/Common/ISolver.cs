using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Common
{
    /// <summary>
    /// 算法计算器。
    /// </summary>
    public interface ISolver
    {
        /// <summary>
        /// 执行计算。
        /// </summary>
        void Solve();
    }

    /// <summary>
    /// 一个抽象的算法计算器实现
    /// </summary>
    public abstract class AbstractSolver : Named, ISolver
    {
        /// <summary>
        /// 执行计算。
        /// </summary>
        public abstract void Solve();

    }
}
