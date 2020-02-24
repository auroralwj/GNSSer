
//2014.11.19, czs, edit in namu, 提取 VectorMatrix， IWeightedVector

using System;
namespace Geo.Algorithm
{
    /// <summary>
    /// 具有权值的数据向量。也是一个单列矩阵。
    /// 其权值采用 InverseWeight 属性专门存储。
    /// 平差计算中，数据向量值与数值的协方差是平差的基本单元。
    /// </summary>
  public  interface IWeightedVector
    {
      /// <summary>
      /// 获取协方差
      /// </summary>
      /// <param name="varianceFactor">协方差因子，单位权方差</param>
      /// <returns></returns>
       Matrix GetCovariance(double varianceFactor);
      /// <summary>
      /// 权逆阵
      /// </summary>
       Matrix InverseWeight { get; }
      /// <summary>
      /// 是否具有权值
      /// </summary>
        bool IsWeighted { get; }
      /// <summary>
      /// 设置权矩阵
      /// </summary>
      /// <param name="inverseWeight"></param>
        void SetInverseWeight(global::Geo.Algorithm.Matrix inverseWeight);
      /// <summary>
      /// 输出格式化文本
      /// </summary>
      /// <returns></returns>
        string ToFormatedText();
      /// <summary>
      /// 获取权矩阵。
      /// </summary>
       Matrix Weights { get; }
    }
}
