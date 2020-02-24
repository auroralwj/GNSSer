//2015.01.14, czs, create in namu, 模糊度固定接口。
//2018.10.19, czs, edit in hmx， 模糊度增加RMS信息


using System;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;


namespace Gnsser
{
    /// <summary>
    /// 模糊度固定器
    /// </summary>
    public interface IAmbiguityFixer
    {

        /// <summary>
        /// 默认的RMS
        /// </summary>
        double DefaultRms { get; set; }
        /// <summary>
        /// 获取固定后的模糊度。
        /// </summary>
        /// <returns></returns>
       WeightedVector GetFixedAmbiguities(WeightedVector floatResolution);
    }

    /// <summary>
    /// 模糊度固定器
    /// </summary>
    public abstract class BaseAmbiguityFixer : IAmbiguityFixer
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public  BaseAmbiguityFixer()
        {
            DefaultRms = 1e-20;
        }
        /// <summary>
        /// 默认固定后的RMS或均方差。
        /// </summary>
        public virtual double DefaultRms { get; set; }
        /// <summary>
        /// 固定算法实现
        /// </summary>
        /// <param name="floatResolution"></param>
        /// <returns></returns>
        public abstract WeightedVector GetFixedAmbiguities(WeightedVector floatResolution);
    }
}
