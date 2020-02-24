//2014.10.06， czs, create in hailutu, 配置通用接口

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo
{ 
    /// <summary>
    /// 网络资源配置，需要至少提供一个网络路径。
    /// </summary>
    public class NetOption : BaseOption, INetOption
    {
        /// <summary>
        /// 构造函数。输入名称。
        /// </summary>
        /// <param name="Url">路径</param>
        /// <param name="name">名称</param>
        public NetOption(string Url, string name = "单网络文件配置")
            : base(name)
        {
            this.Urls = new List<string>();
            this.Urls.Add(Url);
        }
        /// <summary>
        /// 构造函数。输入名称。
        /// </summary>
        /// <param name="Urls">集合路径</param>
        /// <param name="name">名称</param>
        public NetOption(string[] Urls, string name = "集合网络文件配置")
            : base(name)
        {
            this.Urls = new List<string>(Urls);
        }
        /// <summary>
        /// 文件路径集合
        /// </summary>
        public List<String> Urls { get; set; }
        /// <summary>
        /// 第一个文件路径
        /// </summary>
        public string Url { get { return Urls[0]; } }
    }
}