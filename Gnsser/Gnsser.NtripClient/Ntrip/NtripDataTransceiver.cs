//2015.02.17, czs, create in pengzhou, 网络数据接收器，封装了一个Socket。
//2016.01.24, czs, edit in hongqing, 修正接收数据报错，未接收到数据可以等待,重命名为 NtripDataTransceiver
//2017.04.24, czs,edit in hongqing, 重构，增加Connect()方法，判断成功与否

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
    /// 二进制Ntrip数据接收委托
    /// </summary>
    /// <param name="satData"></param>
    public delegate void DataReceivedEventHandler(byte [] data);
    
    /// <summary>
    ///二进制Ntrip数据接收器， 网络数据接收器，封装了一个Socket。
    /// </summary>
    public class NtripDataTransceiver: IDisposable
    {
        ILog log = new Log(typeof(NtripDataTransceiver));

        /// <summary>
        /// 构造函数，初始化，并不连接
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public NtripDataTransceiver(string host, int port)
        {
            this.Host = host;
            this.Port = port;
            this.TimeOutSeconds = 60 * 5; 
        }

        #region 属性、事件
        /// <summary>
        /// 数据接收事件
        /// </summary>
        public event DataReceivedEventHandler DataReceived;
        /// <summary>
        /// 主机名称
        /// </summary>
        public String Host { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 是否取消连接
        /// </summary>
        public bool IsCancel { get; set; }
        /// <summary>
        /// 链接超时时间。
        /// </summary>
        public int TimeOutSeconds { get; set; } 
        /// <summary>
        /// 主要的网络请求工具。
        /// </summary>
        private System.Net.Sockets.Socket Socket { get; set; }
        /// <summary>
        ///  获取已经从网络接收且可供读取的数据量。字节数量。
        /// </summary>
        public int SocketAvailable { get { return Socket.Available; } }
        #endregion 

        #region 方法
        /// <summary>
        /// 网络连接，Socket
        /// </summary>
        public bool Connect()
        {
            this.Socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            try
            {
                Socket.Connect(Host, Port);
                log.Info("远程连接成功！" + Host + ":" + Port);
                return true;
            }
            catch (Exception ex)
            {
                //Socket = null;
                log.Error("网络链接失败：" + ex.Message);
                return false;
                throw ex;
            }
        }
        
        /// <summary>
        /// 由生成的请求字符串，通过Socket请求网络数据。
        /// </summary>
        /// <param name="requestString"></param>
        public void SendRequest(string requestString)
        {
            //Send request
            byte[] data = System.Text.Encoding.ASCII.GetBytes(requestString);
            Socket.Send(data);
        }

 
        /// <summary>
        /// 解析第一次响应
        /// </summary> 
        /// <returns></returns>
        public string GetFirstResponse()
        {
            //解析头部信息，Wait for response 
            var timeCount = TimeOutSeconds;
            for (var i = 0; i <= timeCount; i++) //Wait 30 fraction for a response
            {
                System.Threading.Thread.Sleep(300);//0.1s
                string responseData = ReceiveSocketString();
                if (responseData.Length > 0)
                {
                    return responseData; 
                }
            }
            return "";
        }
        /// <summary>
        /// 不断的数据接收，时间通过事件DataReceived传出。
        /// </summary>
        /// <returns>如果连接失败，则返回false</returns>
        public bool ReceiveStream()
        { 
            int DataNotReceivedFor = 0;

            while (true)
            {
                if (IsCancel) //Flag changed, kill the thread
                {
                    Disconnect();
                    break;
                }

                int DataLength = Socket.Available;
                if (DataLength == 0)//数据接收为 0 
                {
                    DataNotReceivedFor++;
                    if (DataNotReceivedFor > TimeOutSeconds)//Data not received for 30 fraction. Terminate the connection.
                    {
                        //log.Info( TimeOutSeconds + " 秒，无数据。等待 5 分钟再试。");

                        //等待传输 0.1 秒
                        //System.Threading.Thread.Sleep(5* 60 * 1000);
                        DataNotReceivedFor = 0;

                        return false;
                    }
                }
                else//具有数据，正式处理了
                {
                    DataNotReceivedFor = 0;
                    byte[] inBytes = ReceiveRawData();
                } 


                //等待传输 0.1 秒
                System.Threading.Thread.Sleep(200);
            }

            return true;
        }

        /// <summary>
        /// 接收的原始二进制数据，接收前先等待0.1秒。
        /// </summary>
        /// <returns></returns>
        private byte[] ReceiveRawData()
        {
            //等待传输 0.1 秒
            System.Threading.Thread.Sleep(100);

            int dataLength = Socket.Available;
            byte[] inBytes = null;
            if (dataLength > 0)
            {
                inBytes = new byte[dataLength];
                Socket.Receive(inBytes, dataLength, System.Net.Sockets.SocketFlags.None);

                //接收数据如实上报前台处理,更新进度条
                if (DataReceived != null) { DataReceived(inBytes); }
            }
            return inBytes;
        }
        /// <summary>
        /// 从socket获取数据，并解析为字符串。
        /// </summary>
        /// <returns></returns>
        private string ReceiveSocketString()
        {
            string responseData = "";
            byte[] inBytes = ReceiveRawData();
            if (inBytes == null)
            {
                log.Debug("没有接收到 RTCM 数据");
                return ""; 
            }
            responseData = System.Text.Encoding.ASCII.GetString(inBytes, 0, inBytes.Length);
            return responseData;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (Socket.Connected) { Socket.Disconnect(false); }
            //等待 0.1 秒
            System.Threading.Thread.Sleep(100);

            if (Socket.Connected) { log.Error("Socket网络断开失败"); }
            else { log.Info("Socket网络成功断开"); }
        }

        /// <summary>
        /// 从网络读取文本，直到指定的标识结束。
        /// </summary>
        /// <param name="initText">初始字符串。</param>
        /// <param name="endMarker"></param>
        /// <returns></returns>
        public string GetTextUntil(string initText, string endMarker = "ENDSOURCETABLE")
        {
            //Start of ObsDataSource table was downloaded. Check for more satData.
            for (var i = 0; i <= TimeOutSeconds; i++) //Wait another 10 fraction for ObsDataSource table
            {
                System.Threading.Thread.Sleep(100);
                initText += ReceiveSocketString();
                //已经读取完毕
                if (initText.Contains(endMarker))
                {
                    break;
                }
            }
            return initText;
        }

        public void Dispose()
        {
            if (Socket != null)
            {
                Disconnect();
            }

        }
        #endregion
    }
}