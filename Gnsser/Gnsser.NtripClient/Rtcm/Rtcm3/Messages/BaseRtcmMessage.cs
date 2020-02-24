//2015.05.06, czs, create in namu, RTCM 消息基类。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// RTCM 消息基类。
    /// </summary>
    public class BaseRtcmMessage
    {
        /// <summary>
        /// 消息长度。单位bit.
        /// </summary>
        public int Length { get; protected set; }

        /// <summary>
        /// 数据信息编号，如1001，通常为 uint12
        /// </summary>
        public uint MessageNumber { get; set; }
    }
}