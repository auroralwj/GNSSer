//2014.10.24, czs, create in namu shuangliao, 通用数据源服务接口
//2014.10.18, czs, create in beijing, IService<TProduct> 服务的内容为产品
//2014.10.18, czs, create in beijing,IService<TProduct, TCondition> 服务的内容为产品
//2014.11.20, czs, edit in numu, 合并类型化的服务接口，都命名为 IService
//2015.05.10, czs, create in namu, 增加具有时间范围的服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;

namespace Geo
{ 
    /// <summary>
    /// 断断续续的服务，也可以是多时间段的连续的服务，如，多个文件的钟差服务。
    /// </summary>
    /// <typeparam name="TProduct">产品</typeparam>
    /// <typeparam name="TCondition">条件</typeparam>
    /// <typeparam name="TTimeScope">时间</typeparam>
    public abstract class AbstractTimedService<TProduct, TCondition, TTimeScope> :  AbstractTimedService<TProduct, TTimeScope>, ITimedService<TProduct, TCondition, TTimeScope>
    { 
        #region 方法
        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="condition">获取条件</param>
        /// <returns></returns>
        public abstract TProduct Get(TCondition condition);
        #endregion
    }

   /// <summary>
    /// 断断续续的服务，也可以是多时间段的连续的服务，如，多个文件的钟差服务。
    /// </summary>
    /// <typeparam name="TProduct">产品</typeparam>
    /// <typeparam name="TTimeScope">时间</typeparam>
    public abstract class AbstractTimedService<TProduct, TTimeScope> : AbstractService<TProduct>, ITimedService<TProduct, TTimeScope>
    {
        #region 属性
        /// <summary>
        /// 时间范围
        /// </summary>
        public TTimeScope TimePeriod { get; protected set; }

        /// <summary>
        /// 服务时间间隔
        /// </summary>
       // public int Interval { get;set; } 
        #endregion

    }
}
