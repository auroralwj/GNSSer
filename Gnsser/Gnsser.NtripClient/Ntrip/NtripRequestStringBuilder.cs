//2015.01.30, czs, create in pengzhou, Ntrip 请求字符串构建器
//2017.04.24, czs, edit in hongqing, 重命名为  NtripRequestStringBuilder

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;

namespace Gnsser.Ntrip
{
    /// <summary>
    ///  Ntrip 请求字符串构建器
    /// </summary>
    public class NtripRequestStringBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="NtripMountPoint"></param>
        /// <param name="NtripParam"></param>
        public NtripRequestStringBuilder(string NtripMountPoint , NtripOption NtripParam)
        {
            this.NtripMountPoint = NtripMountPoint;
            this.NtripParam = NtripParam;
        }
        /// <summary>
        /// 参数。
        /// </summary>
        public NtripOption NtripParam { get; set; }
        /// <summary>
        /// 挂载点，即，站点名称。
        /// </summary>
        public string NtripMountPoint { get; set; }
        /// <summary>
        /// 创建HTTP请求信息。
        /// </summary>
        /// <returns></returns>
        public string BuildRequest()
        {
            //创建请求字符串。Build request message
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("GET /" + NtripMountPoint + " HTTP/1.0");
            // sb.AppendLine("User-Agent: NTRIP  GnsserNTRIPClient/201501");
            sb.AppendLine("User-Agent: NTRIP Gnsser NTRIP Client v0.1");//NTRIP 必须写上。
            sb.AppendLine("Accept: */*");
            sb.AppendLine("Connection: close");

            if (NtripParam.Username.Length > 0)
            {
                string auth = StringUtil.ToBase64(NtripParam.Username + ":" + NtripParam.Password);
                sb.AppendLine("Authorization: Basic " + auth);
            }
            sb.AppendLine();

            string mymsg = sb.ToString();
            return mymsg;
        } 
    }
}
