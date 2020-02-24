//2014.10. , czs, create, 算术计算接口

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Algorithm
{
    //2016.10.21,czs, create in xian to zz train G2014, 算术操作类型
    public enum AlgorithmOperType
    {
        Plus,
        
        Add,

        Multiply,

        Devide
    }

    //2015.04.17, czs, create in namu, 提取增加一维（时间，单位等）操作数包含加减法
    /// <summary>
    /// 一维（时间，单位等）操作数包含加减法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface OneDimOperation<T>
    {
        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="right">待操作数</param>
        /// <returns>计算结果</returns>
        T Plus(T right);
        /// <summary>
        /// 减法
        /// </summary>
        /// <param name="right">待操作数</param>
        /// <returns>计算结果</returns>
        T Minus(T right);

    }



    /// <summary>
    /// 算术计算接口.加减乘除。
    /// </summary>
    /// <typeparam name="T">参与计算的类型</typeparam>
    public interface IArithmeticOperation<T> : OneDimOperation<T>
    { 
        /// <summary>
        /// 相反数
        /// </summary>
        /// <returns></returns>
        T Opposite();
        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="right">待操作数</param>
        /// <returns>计算结果</returns>
        T Multiply(T right);
        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="right">待操作数</param>
        /// <returns>计算结果</returns>
        T Multiply(double right);
        /// <summary>
        /// 除法
        /// </summary>
        /// <param name="right">待操作数</param>
        /// <returns>计算结果</returns>
        T Divide(double right);
    }
    /// <summary>
    /// 向量的计算
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IVectorOperation<T> : IArithmeticOperation<T>
    {
        /// <summary>
        /// 向量的点积/数量积
        /// </summary>
        /// <param name="right">另一个向量</param>
        /// <returns></returns>
        double Dot(T right);

        /// <summary>
        /// 向量的叉乘，结果还是向量
        /// </summary>
        /// <param name="right">另一个向量</param>
        /// <returns></returns>
        T Cross(T right);
    }

}
