//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Correction
{ 
    /// <summary>
    /// 改正器链，用于作用在改正数载体上。
    /// </summary>
    /// <typeparam name="TCorrection">改正数类型</typeparam>
    /// <typeparam name="TInput">改正时需要输入的对象类型</typeparam>
    public interface ICorrectorChian<TCorrection, TInput> :
        ICorrector<TCorrection, TInput>,
        IEnumerable<ICorrector<TCorrection, TInput>>
    {   
        /// <summary>
        /// 改正器。
        /// </summary>
        List<ICorrector<TCorrection, TInput>> Correctors { get; set; }

        /// <summary>
        /// 改正数字典，记录改正数细节，便于调试。
        /// </summary>
        Dictionary<string, TCorrection> Corrections { get; }
        
        /// <summary>
        /// 通过这种方法添加责任链的后继者。
        /// </summary>
        /// <param name="correctorNode">改正器</param>
        /// <returns></returns>
        CorrectorChain<TCorrection, TInput> Add(ICorrector<TCorrection, TInput> correctorNode);
    }     
}
