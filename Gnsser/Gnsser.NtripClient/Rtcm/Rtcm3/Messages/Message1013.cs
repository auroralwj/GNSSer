//2015.02.17, czs, create in pengzhou, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 70+29 * Nm bit.
    /// Contents of the Type 1013 Message, System Parameters
    /// </summary>
    public class Message1013: BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message1013() { Msgs = new List<MsgItem>(); } 
        /// <summary>
        /// uint12
        /// </summary>
        public uint ReferenceStationID { get; set; }
        /// <summary>
        /// uint16
        /// </summary>
        public uint MJDNumber { get; set; }
        /// <summary>
        /// uint17
        /// </summary>
        public uint SecondsofDayUTC { get; set; }
        /// <summary>
        /// uint5
        /// </summary>
        public uint NoofMessageIDAnnouncementstoFollow_Nm { get; set; }
        /// <summary>
        /// uint8
        /// </summary>
        public uint LeapSecondsGPS_UTC { get; set; }


        public   List<MsgItem> Msgs { get; set; }

        public static Message1013 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1013 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1013 msg = new Message1013();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12 
            msg.ReferenceStationID = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12 
            msg.MJDNumber = BitConvertUtil.GetUInt(sequence.DeQueue(16));//uint16 
            msg.SecondsofDayUTC = BitConvertUtil.GetUInt(sequence.DeQueue(17));//uint17 
            msg.NoofMessageIDAnnouncementstoFollow_Nm = BitConvertUtil.GetUInt(sequence.DeQueue(5));//uint5 
            msg.LeapSecondsGPS_UTC = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8 

            //29 * 
            for (int i = 0; i <msg.NoofMessageIDAnnouncementstoFollow_Nm; i++)
            {
                msg.Msgs.Add(MsgItem.Parse(sequence.DeQueue(29)));
            }
            msg.Length = 70 + (int)msg.NoofMessageIDAnnouncementstoFollow_Nm * 29;
             
            return msg;
        }

        
    }


    public class MsgItem
    {

        /// <summary>
        /// uint12
        /// </summary>
        public uint MessageID1 { get; set; }
        /// <summary>
        /// bit1
        /// </summary>
        public bool Message1SyncFlag { get; set; }
        /// <summary>
        /// uint16
        /// </summary>
        public uint Message1TransmissionInterval { get; set; }


        public static MsgItem Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);
            MsgItem msg = new Rtcm.MsgItem();
            msg.MessageID1 = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12 
            msg.Message1SyncFlag = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//uint12 
            msg.Message1TransmissionInterval = BitConvertUtil.GetUInt(sequence.DeQueue(16));//uint12 
            return msg;
        }
    }
}