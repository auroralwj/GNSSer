//2014.09.14, czs, create, 观测量，Gnsser核心模型！
//2014.10.09, czs, create in hailutu, 可以改正的对象，对象与改正数不必是同一种类型
//2014.11.19, czs, edit in namu, 名称 ICorrectableObject 改为  ICorrectable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Correction
{

    /// <summary>
    /// 基础的具有改正数的数值接口。
    /// </summary>
    public interface ICorrectableNumeral : ICorrectable<Double>
    {

    }

    /// <summary>
    /// 需要改正的值。由值和改正数都是同一种类型。
    /// </summary>
    public interface ICorrectable<TValue> : ICorrectable<TValue, TValue>
    {

    } 

    /// <summary>
    /// 最基本的，可以改正的泛型接口，对象与改正数不必是同一种类型
    /// </summary>
    public interface ICorrectable<TValue, TCorection>
    {
        /// <summary>
        /// 值
        /// </summary>
        TValue Value { get; }
        /// <summary>
        /// 改正数
        /// </summary>
        TCorection Correction { get; }

        /// <summary>
        /// 改正后的值。其类型与值的类型相同。
        /// </summary>
        TValue CorrectedValue { get; }
        /// <summary>
        /// 是否具有改正数
        /// </summary>
        bool HasCorrection { get; }
    }
}
