//2015.02.16, czs, create in pengzhou, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 父类
    /// </summary>
    public class RtcmMessage
    {
        /// <summary>
        /// uint12 , 12
        /// </summary>
        public uint MessageNumber { get; set; }

        /// <summary>
        /// 消息编号,Uint,12
        /// </summary>
        public MessageType MessageType
        {
            get { return (MessageType)MessageNumber; }

        }
    }
}