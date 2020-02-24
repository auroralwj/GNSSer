//2016.01.15, czs, create in hongqing, URL工具

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
    public class JsonUtil
    {

        public static NameValueCollection ParseJson(string json_code)
        {
            NameValueCollection mc = new NameValueCollection();
            Regex regex = new Regex(@"(\s*\""?([^""]*)\""?\s*\:\s*\""?([^""]*)\""?\,?)");
            json_code = json_code.Trim();
            if (json_code.StartsWith("{"))
            {
                json_code = json_code.Substring(1, json_code.Length - 2);
            }
            foreach (Match m in regex.Matches(json_code))
            {
                mc.Add(m.Groups[2].Value, m.Groups[3].Value);
                //Response.Write(m.Groups[2].Value + "=" + m.Groups[3].Value + "<br/>");
            }
            return mc;
        }


    }
}
