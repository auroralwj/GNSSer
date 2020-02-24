//2018.03.27, czs, create in hmx, 具有权值的数据向量,同时含有一个改正数。 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 具有权值的数据向量,同时含有一个改正数。 
    /// 适用于具有近似值或改正的变量。
    /// </summary>
    public class CorrectableWeightedVector : Correction.AbstractCorrectable<WeightedVector, Vector>, IReadable//, IDisposable, IWeightedVector
    {

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Value">具有误差（权阵)的值</param>
        public CorrectableWeightedVector(WeightedVector Value) : base(Value)
        {
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Value">具有误差（权阵)的值</param>
        /// <param name="vector">没有误差的向量</param>
        public CorrectableWeightedVector(WeightedVector Value, Vector vector) : base(Value, vector)
        {
        }

        /// <summary>
        /// 改正后的值
        /// </summary>
        public override WeightedVector CorrectedValue { get { if (Correction != null) { return Value + Correction; } return Value; } }


        /// <summary>
        /// 提供可读String类型返回
        /// </summary>
        /// <returns></returns>
        public virtual string ToReadableText(string splitter = ",")
        {
            //StringBuilder sb = new StringBuilder();
            //var vecStr = base.ToReadableText();
            //var matStr = new Matrix(this.InverseWeight).ToReadableText(splitter);

            //sb.Append(vecStr);
            //sb.Append(matStr);

            return CorrectedValue.ToReadableText(splitter);
        }
    }

}