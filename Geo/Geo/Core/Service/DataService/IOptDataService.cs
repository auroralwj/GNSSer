//2014.10.24, czs, create in namu shuangliao, 通用数据源服务接口

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 具有选项的通用数据源接口
    /// </summary>
    /// <typeparam name="TOption">数据服务选项</typeparam>
    /// <typeparam name="TProduct">产品</typeparam>
    public interface IOptionalService<TOption, TProduct> : IService<TProduct>
        where TOption : IOption
    {
        /// <summary>
        ///  数据源选项。
        /// </summary>
        TOption Option { get; set; }
    }

    /// <summary>
    /// 具有选项的通用数据源接口
    /// </summary>
    /// <typeparam name="TOption">数据服务选项</typeparam>
    /// <typeparam name="TProduct">产品</typeparam>
    /// <typeparam name="TTimeScope">产品</typeparam>
    public interface ITimedOptionalService<TOption, TProduct, TTimeScope> : ITimedService<TProduct, TTimeScope>
        where TOption : IOption
    {
        /// <summary>
        ///  数据源选项。
        /// </summary>
        TOption Option { get; set; }
    }

    /// <summary>
    /// 具有选项的通用数据源接口
    /// </summary>
    /// <typeparam name="TOption">数据服务选项</typeparam>
    /// <typeparam name="TProduct">产品</typeparam>
    public class OptionalService<TOption, TProduct> : AbstractService<TProduct>, IOptionalService<TOption, TProduct>  
        where TOption : IOption
    {
        /// <summary>
        ///  数据源选项。
        /// </summary>
        public TOption Option { get; set; }
    }

    /// <summary>
    /// 具有选项的通用数据源接口
    /// </summary>
    /// <typeparam name="TOption">数据服务选项</typeparam>
    /// <typeparam name="TProduct">产品</typeparam>
    /// <typeparam name="TTimeScope">产品</typeparam>
    public class TimedOptionalService<TOption, TProduct, TTimeScope>  : AbstractTimedService<TProduct, TTimeScope>, ITimedOptionalService<TOption, TProduct, TTimeScope>  
        where TOption : IOption
    {
        /// <summary>
        ///  数据源选项。
        /// </summary>
       public  TOption Option { get; set; }
    }
}
