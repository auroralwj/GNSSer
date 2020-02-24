//2015.03.29, czs, create in hailutu, 封装NTrip一些常见应用.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace Gnsser.Ntrip
{
    /// <summary>
    ///  封装NTrip一些常见应用。
    /// </summary>
   public  class NtripUtil
    {
       /// <summary>
        /// 直接从网络获取数据源表
       /// </summary>
       /// <param name="host">主机</param>
       /// <param name="port">端口</param>
       /// <returns></returns>
       public static SourceTable GetSourceTableFromNet(string host = "http://www.igs-ip.net", int port = 2101)
       {
           var tableStr = GetSourceTableStringFromNet(host, port);
           return  SourceTable.Parse(tableStr);
       }

       /// <summary>
       /// 直接从网络获取数据源表，返回字符串。
       /// </summary>
       /// <param name="host"></param>
       /// <param name="port"></param>
       /// <returns></returns>
       public static string GetSourceTableStringFromNet(string host = "http://www.igs-ip.net", int port = 2101)
       { 
           string url = host + ":" + port;

           WebClient client = new WebClient();
           var tableStr = client.DownloadString(url);
           return tableStr;
       }
    }
}
