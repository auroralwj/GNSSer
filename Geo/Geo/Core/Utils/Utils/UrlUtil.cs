//2016.01.11, czs, create in hongqing, URL工具

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Geo.Utils
{
    /// <summary>
    /// URL工具
    /// </summary>
    public  class UrlUtil
    {
        /// <summary>
        /// 解析参数为字典
        /// </summary>
        /// <param name="urlParams"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseParams(string urlParams)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string[] pairs = urlParams.Split('&');
            foreach (var item in pairs)
            {
                string[] par = item.Split('=');
                if (par.Length != 2) 
                    continue;
                var name = par[0];
                var value = par[1];

                dic[name] = value;
            }

            return dic;
        }

        public static NameValueCollection ParseUrlParameters(string str_params)
        {
            NameValueCollection nc = new NameValueCollection();
            foreach (string p in str_params.Split('&'))
            {
                string[] p_s = p.Split('=');
                nc.Add(p_s[0], p_s[1]);
            }
            return nc;
        }
    }
}
