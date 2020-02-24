//2016.10.17, double, create in hongqing, RTCM MSM头文件

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;
using Geo.Times;
namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 169+X bit. 
    /// </summary>
    public class HeaderOfMSM : BaseRtcmMessage
    {
        public HeaderOfMSM()
        {
            // this.Length = 169+X;
            this.SatlliteMask = new List<int>();
            this.SignalMask = new List<int>();
            this.CellMask = new List<int>();
        }

        #region 属性
        /// <summary>
        /// uint12
        /// </summary>
        public uint ReferenceStationID { get; set; }
        /// <summary>
        /// uint30
        /// </summary>
        public Time EpochTime { get; set; }
        /// <summary>
        /// bit(1)
        /// </summary>
        public int MultipleMessageBit { get; set; }
        /// <summary>
        /// uint3
        /// </summary>
        public uint IODS { get; set; }
        /// <summary>
        /// Reserved  bit(7)
        /// </summary>
        public int Reserved { get; set; }
        /// <summary>
        /// uint2
        /// </summary>
        public uint ClockSteeringIndicator { get; set; }
        /// <summary>
        /// uint2
        /// </summary>
        public uint ExternalClockIndicator { get; set; }
        /// <summary>
        /// bit(1)
        /// </summary>
        public int DivergencefreeSmoothingIndicator { get; set; }
        /// <summary>
        /// bit(3)
        /// </summary>
        public int SmoothingInterval { get; set; }
        /// <summary>
        /// bit(64)
        /// </summary>
        public List<int> SatlliteMask { get; set; }
        /// <summary>
        /// bit(32)
        /// </summary>
        public List<int> SignalMask { get; set; }
        /// <summary>
        /// bit(X)
        /// </summary>
        public List<int> CellMask { get; set; }
        public int SatCount { get; set; }
        public int Nsig { get; set; }
        public int Ncell { get; set; }
        public int dow { get; set; }
        #endregion

        public static HeaderOfMSM Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static HeaderOfMSM Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            HeaderOfMSM msg = new HeaderOfMSM();

            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.ReferenceStationID = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            if (msg.MessageNumber >= 1081 && msg.MessageNumber <= 1087)
            {
                msg.dow = BitConvertUtil.GetInt(sequence.DeQueue(3));
                double secondOfDay = BitConvertUtil.GetUInt(sequence.DeQueue(27)) * 0.001;
                if (secondOfDay > 10800)
                    secondOfDay -= 10800;
                else secondOfDay += 21 * 3600;
                msg.EpochTime = new Time(Setting.ReceivingTimeOfNtripData.DateTime, secondOfDay);
            }
            else
                msg.EpochTime = new Time(new Time(Setting.ReceivingTimeOfNtripData.DateTime).GpsWeek, BitConvertUtil.GetUInt(sequence.DeQueue(30)) * 0.001);
            msg.MultipleMessageBit = BitConvertUtil.GetInt(sequence.DeQueue(1));
            msg.IODS = BitConvertUtil.GetUInt(sequence.DeQueue(3));
            msg.Reserved = BitConvertUtil.GetInt(sequence.DeQueue(7));
            msg.ClockSteeringIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(2));
            msg.ExternalClockIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(2));
            msg.DivergencefreeSmoothingIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1));
            msg.SmoothingInterval = BitConvertUtil.GetInt(sequence.DeQueue(3));
            msg.SatCount = 0;
            msg.Ncell = 0;
            msg.Nsig = 0;
            for (int i = 0; i < 64; i++)
            {
                int a = BitConvertUtil.GetInt(sequence.DeQueue(1));
                if (a == 1)
                {
                    msg.SatCount++;
                    msg.SatlliteMask.Add(i + 1);
                }
            }
            for (int i = 0; i < 32; i++)
            {
                int a = BitConvertUtil.GetInt(sequence.DeQueue(1));
                if (a == 1)
                {
                    msg.SignalMask.Add(i + 1); 
                    msg.Nsig++;
                }
            }
            int Ncell = msg.SatCount * msg.Nsig;
            for (int i = 0; i < Ncell; i++)
            {
                int a = BitConvertUtil.GetInt(sequence.DeQueue(1));
                msg.CellMask.Add(a);
                if (a == 1) msg.Ncell++;
            }
            msg.Length = 169 + msg.Ncell;
            return msg;
        }
    }
}