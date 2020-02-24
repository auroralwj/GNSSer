//2014.09.14, czs, create, 改正器顶层接口

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Common;

namespace Geo.Correction
{

    /// <summary>
    /// 改正器.改正器的执行方法为：计算改正值，而不直接改变数值。
    /// </summary>
    /// <typeparam name="TCorrection">改正数类型</typeparam>
    /// <typeparam name="TInput">改正时需要输入的对象类型</typeparam>
    public interface ICorrector<TCorrection, TInput> : ICorrector<TCorrection>
    {
        /// <summary>
        /// 执行改正。
        /// </summary>
        void Correct(TInput input);
    }

    /// <summary>
    /// 改正器.改正器的执行方法为：计算改正值，而不直接改变数值。
    /// </summary>
    /// <typeparam name="TCorrection">改正数类型</typeparam>
    public interface ICorrector<TCorrection> : ICorrector
    {
        /// <summary>
        /// 改正数
        /// </summary>
        TCorrection Correction { get; }
    }

    /// <summary>
    /// 改正器顶层接口
    /// </summary>
    public interface ICorrector 
    {
        /// <summary>
        /// 改正器的名称，便于调试查看。
        /// </summary>
        string Name { get; }

      
    } 
     
}
