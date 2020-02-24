//2017.05.03, czs, create in 洪庆, 星历数据源提供适配器。
//2018.03.15, czs, edit in hmx, 重新设计和封装IGS星历获取,考虑超快星历，考虑多系统
//2018.05.02, czs, edit in hmx, 加入全局自动钟差服务

using System;
using System.IO;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data;
using System.Collections.Generic;
using Gnsser.Correction;
using Geo.Times;
using Gnsser.Times;
using Geo;
using Geo.IO;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 提供某一会话的服务。
    /// </summary>
    public abstract class AbstractServiceProvider<TService>
    {
        ILog log = new Log(typeof(AbstractServiceProvider<TService>));

        /// <summary>
        /// 构造函数.星历数据源提供适配器
        /// </summary>
        /// <param name="option"></param>
        /// <param name="processOption"></param> 
        public AbstractServiceProvider(GnsserConfig option, GnssProcessOption processOption)
        {
            this.DataSourceOption = option;
            this.ProcessOption = processOption;
        }

        #region 属性  
        /// <summary>
        /// 全局数据源选项
        /// </summary>
        public GnsserConfig DataSourceOption { get; set; }
        /// <summary>
        /// 当前会话数据处理选项
        /// </summary>
        public GnssProcessOption ProcessOption { get; set; }
        /// <summary>
        /// 当前服务
        /// </summary>
        public TService CurrentService { get; set; }
        #endregion


        /// <summary>
        /// 获取服务，根据设置自动判断。
        /// </summary> 
        /// <returns></returns>
        public abstract TService GetService();
    }     
}

