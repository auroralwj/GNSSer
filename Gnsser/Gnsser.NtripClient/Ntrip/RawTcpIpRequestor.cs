//2015.01.30, czs, create in pengzhou, 对原始Socket进行简单封装，以适应Ntrip访问。

 
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

namespace Gnsser.Ntrip
{
    
   public delegate void  NtripDataStateChangedEventHandler(ResponseType Item, string Value, byte[] myBytes);
     
    /// <summary>
    /// TCP IP
    /// 直接访问。
    /// </summary>
    public class RawTcpIpRequestor
    {
        public event NtripDataStateChangedEventHandler NtripDataStateChanged;

        public RawTcpIpRequestor(string Host, int Port)
        {
            this.Host = Host;
            this.Port = Port;
        }

        /// <summary>
        /// 主机，Ip。
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 是否取消连接
        /// </summary>
        public bool IsCancelConnection { get; set; }


        private void NoticeNtripDataState(ResponseType Item, string Value, byte[] myBytes)
        {
            if (NtripDataStateChanged != null) NtripDataStateChanged(Item, Value, myBytes);
        }

        public void Request()
        {
            //Connect to server
            System.Net.Sockets.Socket sckt = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            try
            {
                sckt.Connect(Host, Port);
            }
            catch (Exception ex)
            {
                NoticeNtripDataState(ResponseType.Disconnected, "服务器未响应。" + ex.Message, null);
            }

            NoticeNtripDataState(ResponseType.ConnectedRequestingData, "", null); //Connected

            int DataNotReceivedFor_1 = 0;
            while (true)
            {
                int DataLength = sckt.Available;
                if (DataLength == 0)
                {
                    DataNotReceivedFor_1++;
                    if (DataNotReceivedFor_1 > 300)
                    {
                        //Data not received for 30 fraction. Terminate the connection.
                        NoticeNtripDataState(ResponseType.Disconnected, "连接超时.", null);
                    }
                }
                else
                {
                    DataNotReceivedFor_1 = 0;
                    byte[] InBytes = new byte[DataLength];
                    sckt.Receive(InBytes, DataLength, System.Net.Sockets.SocketFlags.None);
                    NoticeNtripDataState(ResponseType.NtripClientIsReceivingData, null, InBytes);
                }

                //外部是否指定停止接受数据。
                if (IsCancelConnection) //Flag changed, kill the thread
                {
                    sckt.Disconnect(false);
                    NoticeNtripDataState(ResponseType.Disconnected, "", null);
                }
                System.Threading.Thread.Sleep(100);
            }
        }



    }


}