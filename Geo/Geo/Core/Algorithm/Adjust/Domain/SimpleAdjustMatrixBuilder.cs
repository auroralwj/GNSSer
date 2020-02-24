//2014.09.10, czs, create, 最简洁的平差矩阵生成器
//2018.03.27, czs, edit in hmx, 增大初始方差大小
//2018.04.15, czs, 增加自由项，将ObsMiusApprox改回Observation

using System;
using System.Collections.Generic; 
using System.Text; 
using Geo.Algorithm;
using Geo.Algorithm.Adjust; 
using Geo.Utils;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 最简洁的平差矩阵生成器，平差计算中需要转换的矩阵。
    /// 只需输入残差项即可。
    /// </summary>
    public abstract class SimpleAdjustMatrixBuilder : IAdjustMatrixBuilder
    {
        /// <summary>
        /// 构造函数，
        /// </summary>
        public SimpleAdjustMatrixBuilder()
        {
        }
        /// <summary>
        /// 处理过程的信息，通常是出错信息。
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// 指示是否可以平差。如果观测数据不足，或数据质量太差，则不推荐计算，以免影响结果。
        /// </summary>
        public virtual bool IsAdjustable { get { return ObsCount >= ParamCount; } }
        /// <summary>
        /// 参数名称和顺序.必须设置！！！
        /// </summary>
        public virtual List<string> ParamNames { get; set; }
        /// <summary>
        /// 第二参数名称和顺序，按需设置
        /// </summary>
        public virtual List<string> SecondParamNames { get; set; }
        /// <summary>
        /// 本历元参数是否改变
        /// </summary>
        public abstract  bool IsParamsChanged { get; }

        /// <summary>
        /// 观测量，即设计矩阵的行数。
        /// </summary>
        public virtual int ObsCount { get { return Coefficient.RowCount; } }
        /// <summary>
        /// 参数数量,根据参数名称列表确定的参数数量。
        /// </summary>
        public virtual int ParamCount { get { return this.ParamNames.Count;}}// CoeffOfDesign.ColCount; } }
        /// <summary>
        /// 是否具有第二参数
        /// </summary>
        public bool HasSecondParams { get => SecondParamNames != null; }
        /// <summary>
        /// 第二参数数量。
        /// </summary>
        public virtual int SecondParamCount { get { return SecondParamNames == null? 0:this.SecondParamNames.Count;}}
        /// <summary>
        /// 设计阵，误差方程系数阵。
        /// </summary>
        public abstract Matrix Coefficient { get; }

        /// <summary>
        /// 观测值，一般为残差，即观测值减去近似值.参数平差自由项， l = L - AX0， 观测值 减去 近似值。
        /// </summary> 
        public abstract WeightedVector Observation { get; }

        /// <summary>
        /// 自由项D，B0等等。则参数平差中，满足满足 l = L - (AX0 + D)， 默认为null
        /// </summary>
        public virtual Vector FreeVector { get { return null; } }
        /// <summary>
        /// 是否有自由项
        /// </summary>
        /// <returns></returns>
        public bool HasFreeVector { get { return FreeVector != null; } }


        #region 工具方法 


        /// <summary>
        /// 按照新的参数顺序返回加权向量。权逆阵为对称阵。
        /// </summary>
        /// <param name="newParamNames">新的参数名称列表</param>
        /// <param name="oldVector">旧的加权向量</param>
        /// <param name="initVal">新变量初始值</param>
        /// <param name="initInverseWeight">新出现的变量的初始方差，使其变大减小初始影响</param>
        /// <returns></returns>
        public static WeightedVector GetNewWeighedVectorInOrder(List<string> newParamNames,  WeightedVector oldVector, double initVal = 0, double initInverseWeight = 1E10)
        {
            List<string> oldParamNames = oldVector.ParamNames;
            IMatrix matriResult = NamedMatrix.GetSymmetricInOrder(newParamNames, oldParamNames, oldVector.InverseWeight.Array, initVal, initInverseWeight);
            Vector vector = GetNewVectorInOrder(newParamNames, oldParamNames, (Vector)oldVector);

            return new WeightedVector(vector, matriResult);
        }

        /// <summary>
        /// 按照新的参数顺序返回向量
        /// </summary>
        /// <param name="newParamNames"></param>
        /// <param name="oldParamNames"></param>
        /// <param name="oldVector"></param>
        /// <returns></returns>
        public static Vector GetNewVectorInOrder(List<string> newParamNames, List<string> oldParamNames, Vector oldVector)
        {
            Vector vector = NamedVectorConvert.ConvertNamedVector(newParamNames, oldParamNames, oldVector);
            return vector;
        }
        #endregion
    }
}
