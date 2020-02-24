using System;
using Geo;

namespace Gnsser
{


    /// <summary>
    /// 接口方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObsFileProcessStreamer<T> : IBasicProcess, IDisposable where T : ISiteSatObsInfo
    {
        /// <summary>
        /// 运行后激发
        /// </summary>
        void PostRun();
        /// <summary>
        /// 预先处理
        /// </summary>
        /// <param name="mEpochInfo"></param>
        /// <returns></returns>
        Geo.LoopControlType PreProcess(T mEpochInfo);
        /// <summary>
        /// 处理历元
        /// </summary>
        /// <param name="mEpochInfo"></param>
        void Process(T mEpochInfo);

        /// <summary>
        /// 数据检核和检校
        /// </summary>
        /// <param name="mEpochInfo"></param>
        /// <returns></returns>
        Geo.LoopControlType ProducingRevise(T mEpochInfo);
    }
}
