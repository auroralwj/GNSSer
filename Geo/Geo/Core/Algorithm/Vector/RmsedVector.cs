//2016.10.04, czs, create in hongqing, 具有权值的向量

using System;
using System.Text;
using System.Collections.Generic;
using Geo.Common;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Exceptions;
//using Geo.Algorithm;

namespace Geo.Algorithm
{

    /// <summary>
    /// Geo 向量，以一维列表形式实现。
    /// 是一串纯粹的数字，没有其它任何物理意义。
    /// 与矩阵相同，向量也有多种存储方式，但是主要还是一维列表比较方便,如改变向量空间的维数。
    /// </summary>
    [Serializable]
    public class  RmsedVector:Vector,  ICloneable
    { 
        #region 构造函数 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vector">一维数组</param>
        public RmsedVector(double[] vector, double [] rmses, string [] names = null)
            :base(vector, names)
        {
            this.rmsVecror = new List<double>(rmses); 
        } 
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public RmsedVector()
        {
            rmsVecror = new List<double>();
        }

        public RmsedVector(Adjust.WeightedVector weightedVector)
            : base(weightedVector.OneDimArray, weightedVector.ParamNames.ToArray())
        {
            this.rmsVecror = new List<double>( weightedVector.GetRmsVector().OneDimArray); 
        }
        #endregion

        #region 核心变量
        protected List<double> rmsVecror { get; set; }
        #endregion

        #region Vector 自我方法
        public void Add(string name, double val, double rms)
        {
            this.Add(val, name);
            this.rmsVecror.Add(rms);
        }
        public void Add(string name, RmsedNumeral val )
        {
            this.Add(val.Value, name);
            this.rmsVecror.Add(val.Rms);
        }
        /// <summary>
        /// 设置向量空间的维数。只设置增大。
        /// </summary>
        /// <param name="dimension">新维数</param>
        public void SetDimension(int dimension)
        {
            base.SetDimension(dimension);
            if (this.rmsVecror == null) { this.rmsVecror = new List<double>(dimension); }

            while (this.rmsVecror.Count < dimension) { this.rmsVecror.Add(0); }
          }


        #endregion

        #region override

        public RmsedNumeral GetRmsedNumeral(int i)
        {
            return new RmsedNumeral( Data[i], this.rmsVecror[i]); 

    }

    /// <summary>
    /// 获取指定的元素。
    /// </summary>
    /// <param name="i">行编号</param>
    /// <param name="j">列编号</param>
    /// <returns></returns>
    public NamedRmsedNumeral GetItem(int i)  
      {
            return new NamedRmsedNumeral(ParamNames[i], Data[i], this.rmsVecror[i]); 
        }

        public void SetItem(int i, NamedRmsedNumeral value) { Data[i] = value.Value; ParamNames[i] = value.Name; rmsVecror[i] = value.Rms; }


    //public override IVector Create(int count)
    //{
    //    return new RmsedVector(count);
    //}


    public override int GetHashCode() { return Data.GetHashCode(); }

        #endregion

        #region operater
        ///// <summary>
        ///// +
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static IVector operator +(Vector left, IVector right) { return left.Plus(right); }
        ///// <summary>
        ///// -
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static IVector operator -(Vector left, IVector right) { return left.Minus(right); }
        ///// <summary>
        ///// -
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static IVector operator -(Vector left) { return left.Opposite(); }
        //public static Vector operator /(Vector left, double currentVal) { return left.Divide(currentVal); }
        ///// <summary>
        ///// *
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static IVector operator *(Vector left, IVector right) { return left.Multiply(right); }
        ///// <summary>
        ///// +
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>

        //public static Vector operator +(Vector left, Vector right) { return left.Plus(right); }
        ///// <summary>
        ///// -
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static Vector operator -(Vector left, Vector right) { return left.Minus(right); }
        ///// <summary>
        ///// *
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static Vector operator *(Vector left, Vector right) { return left.Multiply(right); }
        ///// <summary>
        ///// *
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static Vector operator *(Vector left, double right) { return left.Multiply(right); }
        ///// <summary>
        ///// +
        ///// </summary>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public virtual Vector Plus(Vector right)
        //{
        //    if (this.Count != right.Count)
        //        throw new DimentionException("维数相同才可以计算！");

        //    Vector reslult = new Vector(this.Count);
        //    int length = this.Count;
        //    for (int i = 0; i < length; i++)
        //    {
        //        reslult[i] = this[i] + right[i];
        //    }
        //    return reslult;
        //}

        ///// <summary>
        ///// -
        ///// </summary>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public virtual Vector Minus(Vector right)
        //{
        //    if (this.Count != right.Count)
        //        throw new DimentionException("维数相同才可以计算！");

        //    Vector reslult = new Vector(this.Count);
        //    int length = this.Count;
        //    for (int i = 0; i < length; i++)
        //    {
        //        reslult[i] = this[i] - right[i];
        //    }
        //    return reslult;
        //}
        ///// <summary>
        ///// 相反数
        ///// </summary>
        ///// <returns></returns>
        //public virtual Vector Opposite()
        //{
        //    Vector reslult = new Vector(this.Count);
        //    int length = this.Count;
        //    for (int i = 0; i < length; i++)
        //    {
        //        reslult[i] = -this[i];
        //    }
        //    return reslult;
        //}

        ///// <summary>
        ///// Corss
        ///// </summary>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public virtual Vector Multiply(Vector right)
        //{
        //    if (this.Count != right.Count)
        //        throw new DimentionException("维数相同才可以计算！");

        //    //三维
        //    if (Count == 3)
        //    {
        //        double cp0 = this[1] * right[2] - this[2] * right[1];
        //        double cp1 = this[2] * right[0] - this[0] * right[2];
        //        double cp2 = this[0] * right[1] - this[1] * right[0];
        //        double[] cp = new double[] { cp0, cp1, cp2 };
        //        return new Vector(cp);
        //    }
        //    //  throw new NotImplementedException("暂不支持其它维数的叉乘计算" + Count);

        //    //以下有待验证
        //    Vector reslult = new Vector(this.Count);
        //    int length = this.Count;
        //    for (int i = 0; i < length; i++)
        //    {

        //        double tmp = 0;
        //        for (int j = 0; j < length; j++)
        //        {
        //            if (i == j) continue; //同轴叉乘为 0。
        //            tmp = this[i] * right[j] - right[i] * this[j];
        //        }
        //        reslult[i] = tmp;
        //    }
        //    return reslult;
        //}
        ///// <summary>
        ///// *
        ///// </summary>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public virtual Vector Multiply(double right)
        //{
        //    Vector reslult = new Vector(this.Count);
        //    int length = this.Count;
        //    for (int i = 0; i < length; i++)
        //    {
        //        reslult[i] = this[i] * right;
        //        reslult.ParamNames[i] = this.ParamNames[i];
        //    }
        //    return reslult;
        //}
        ///// <summary>
        ///// 除以
        ///// </summary>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public virtual Vector Divide(double right)
        //{
        //    Vector reslult = new Vector(this.Count);
        //    int length = this.Count;
        //    for (int i = 0; i < length; i++)
        //    {
        //        reslult[i] = this[i] / right;
        //        reslult.ParamNames[i] = this.ParamNames[i];
        //    }
        //    return reslult;
        //}
        #endregion

        ///// <summary>
        ///// 创建一个实例
        ///// </summary>
        ///// <param name="array"></param>
        ///// <returns></returns>
        //public override IVector Create(double[] array) { return new Vector(array); }
        ///// <summary>
        ///// 返回迭代
        ///// </summary>
        ///// <returns></returns>
        //public override IEnumerator<double> GetEnumerator() { return this.vector.GetEnumerator(); }

        ///// <summary>
        ///// 解析字符串，默认为 \t , 
        ///// </summary>
        ///// <param name="line"></param>
        ///// <returns></returns>
        //public static Vector Parse(string line)
        //{
        //    char[] splliters = new char[] { '\t', ',',';' };
        //    return Parse(line, splliters);
        //}

        ///// <summary>
        ///// 解析字符串，默认为
        ///// </summary>
        ///// <param name="line"></param>
        ///// <param name="splliters"></param>
        ///// <returns></returns>
        //public static Vector Parse(string line, char[] splliters)
        //{
        //    var strs = line.Split(splliters);
        //    Vector vec = new Vector(strs.Length);
        //    for (int i = 0; i < strs.Length; i++)
        //    {
        //        if (string.IsNullOrWhiteSpace(strs[i])) { continue;}
        //        vec[i] = Double.Parse(strs[i]);
        //    }

        //    return vec;
        //}

    }
}
