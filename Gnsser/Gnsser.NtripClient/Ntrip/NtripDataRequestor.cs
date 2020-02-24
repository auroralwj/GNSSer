//2015.01.30, czs, create in pengzhou, 对原始Socket进行简单封装，以适应Ntrip访问。
//2017.04.24, czs,edit in hongqing, 重构 预留其它网络数据方法。

 
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Text;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using Gnsser.Ntrip.Rtcm;
using Microsoft.VisualBasic.CompilerServices;
using Geo.IO;

namespace Gnsser.Ntrip
{
    /// <summary>
    /// 反馈状态
    /// </summary>
    public enum ResponseType
    {
        WaitingforNmeaGga = -1,
        Connecting = 0,
        ConnectedRequestingData = 1,
        ConnectedWaitingforData = 2,
        NtripClientIsReceivingData = 3,
        ShowInfo = 10,
        Disconnected = 100,
        GotSourceTable = 101
    }

    /// <summary>
    /// 版本
    /// </summary>
    public enum RtcmVersion
    {
        Rtcm2,
        Rtcm3,
    }

    /// <summary>
    ///  NTRIP数据请求和接收，
    ///  封装了不同的版本。
    /// </summary>
    public class NtripDataRequestor : IDisposable
    {
        Log log = new Log(typeof(NtripDataRequestor));
        /// <summary>
        /// 网络访问器，构造函数。
        /// </summary>
        /// <param name="NtripParam"></param>
        /// <param name="GPS"></param>
        public NtripDataRequestor(NtripOption NtripParam, LocalGps GPS)
        {
            this.NtripParam = NtripParam;
            this.GPS = this.GPS;//是否应该为this.GPS = GPS;
            this.DataType = Ntrip.RtcmVersion.Rtcm3;
            this.NtripRtcm3Requestor = new NtripRtcm3Requestor(NtripParam, GPS);
            this.NtripRtcm3Requestor.NtripDataStateChanged += NoticeNtripDataState;
        }
        #region 事件与属性
        public event NtripDataStateChangedEventHandler NtripDataStateChanged;

        /// <summary>
        /// 版本
        /// </summary>
        public RtcmVersion DataType { get; set; }

        public LocalGps GPS { get; set; }

        public RawTcpIpRequestor RawTcpIpVisitor;
        public NtripRtcm3Requestor NtripRtcm3Requestor { get; set; }
        public NtripOption NtripParam { get; set; } 
        private bool _IsCancelConnection { get; set; }
        /// <summary>
        /// 是否取消网络链接
        /// </summary>
        public bool IsCancel
        {
            get { return _IsCancelConnection; }
            set
            {
                _IsCancelConnection = value;
                if (RawTcpIpVisitor != null)
                {
                    RawTcpIpVisitor.IsCancelConnection = value;
                }
                if (NtripRtcm3Requestor != null)
                {
                    NtripRtcm3Requestor.IsCancel = value;
                }
            }
        } 

        #endregion

        private void NoticeNtripDataState(ResponseType Item, string Value, byte[] myBytes)
        {
            if (NtripDataStateChanged != null) NtripDataStateChanged(Item, Value, myBytes);
        }

        /// <summary>
        /// Ntrip 循环,主访问函数
        /// </summary>
        public void Request()
        {
            if (IsCancel) { log.Info("已经取消网络连接，请求无效！"); return; }
            //Pause for a bit in case we just disconnected and are now reconnecting.
            System.Threading.Thread.Sleep(1000);

            switch (this.NtripParam.ProtocolType)
            {
                case ProtocolType.RTCM3: //NTRIP Protocol
                    NtripRtcm3Requestor.Request();
                    break;
                default: //0 Raw TCP/IP Socket
                    NoticeNtripDataState(ResponseType.Connecting, "", null); //Connecting     
              
                    RawTcpIpVisitor = new RawTcpIpRequestor(NtripParam.CasterIp, NtripParam.Port);
                    RawTcpIpVisitor.NtripDataStateChanged += NtripDataStateChanged;
                    RawTcpIpVisitor.Request();
                    break;
            }
        }

        public void Dispose()
        {
            if (NtripRtcm3Requestor != null)
            {
                NtripRtcm3Requestor.Dispose();
            }
        }
    }
}