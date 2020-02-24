//2015.01.30, czs, create in pengzhou, 对原始Socket进行简单封装，以适应Ntrip访问。
//2017.04.24, czs, edit in hongqing, 重构
 
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
    /// Ntrip 反馈的文本。
    /// </summary>
    public class NtripResponseText
    {
        public static string SOURCETABLE_200_OK = "SOURCETABLE 200 OK";
        public static string ENDSOURCETABLE = "ENDSOURCETABLE";
        public static string _401_Unauthorized = "401 Unauthorized";
        public static string ICY_200_OK = "ICY 200 OK";
    }



    /// <summary>
    /// Ntrip Rtcm 3.X 版本的数据访问和接收。
    /// 包括数据内容，数据表，请求失败等。
    /// </summary>
    public class NtripRtcm3Requestor :IDisposable, Geo.ICancelAbale
    {
        Log log = new Log(typeof(NtripRtcm3Requestor)  );
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="GPS"></param>
        public NtripRtcm3Requestor(NtripOption Option, LocalGps GPS)
        {
            this.Option = Option;
            this.GPS = this.GPS;
            this.NtripMountPoint = Option.PreferredMountPoint;
            this.RtcmVersion = Ntrip.RtcmVersion.Rtcm3;
            sourcetablePath = Application.StartupPath + "\\sourcetable.dat";

            NetDataReceiver = new NtripDataTransceiver(Option.CasterIp, Option.Port);
            NetDataReceiver.DataReceived += NetDataReceiver_DataReceived;

        }
        #region 属性
        /// <summary>
        /// 是否取消连接或请求
        /// </summary>
        public bool IsCancel { get { return NetDataReceiver.IsCancel; } set { NetDataReceiver.IsCancel = value; } }
        /// <summary>
        /// 通知进度，并传回数据。
        /// </summary>
        public event NtripDataStateChangedEventHandler NtripDataStateChanged;
        public string sourcetablePath { get; set; }
        /// <summary>
        /// RTCM 版本
        /// </summary>
        public RtcmVersion RtcmVersion { get; set; }
        /// <summary>
        /// 本地GPS对象，用于获取本地信息。
        /// </summary>
        public LocalGps GPS { get; set; }

        /// <summary>
        /// 参数。
        /// </summary>
        public NtripOption Option { get; set; }
        /// <summary>
        /// 最近使用的GGA。
        /// </summary>
        public string MostRecentGGA = "";  
        /// <summary>
        /// 挂载点，测站。
        /// </summary>
        public string NtripMountPoint = "WUH20";

        NtripDataTransceiver NetDataReceiver { get; set; }
        #endregion

        #region 方法
        private void NoticeNtripDataState(ResponseType Item, string Value, byte[] myBytes)
        {
            if (NtripDataStateChanged != null) NtripDataStateChanged(Item, Value, myBytes);
        }

        /// <summary>
        /// 网络请求。
        /// </summary>
        public void Request()
        { 
          //Is GGA satData required
            if (this.Option.IsRequiresGGA && !this.Option.UseManualGGA) 
            { 
                if (!CheckAndLoopWaitGga())   {    return;   }
            }
            //try
            //{
                NoticeNtripDataState(ResponseType.Connecting, "", null); //Connecting
                if (!NetDataReceiver.Connect())
                {
                    log.Error("无法连接网络。" + Option.CasterIp + ":" + Option.Port);
                    return;
                }
                NoticeNtripDataState(ResponseType.ConnectedRequestingData, "", null); //Connected

                //请求数据 
                string reqStr = new NtripRequestStringBuilder(NtripMountPoint, Option).BuildRequest();
                NetDataReceiver.SendRequest(reqStr);

                System.Threading.Thread.Sleep(100);

                string responseData = NetDataReceiver.GetFirstResponse();

                //判断首次是否成功。
                if (ProcessResponse(responseData))
                {
                    //获取数据，如果失败了，则继续循环。
                    while (!NetDataReceiver.ReceiveStream())
                    {
                        if (IsCancel) { return; }
                        System.Threading.Thread.Sleep(1000);
                        if (IsCancel) { return; }

                        log.Info(NtripMountPoint + "-连接已断开，将重新请求。");
                        this.Request();
                    }
                } 
                NetDataReceiver.Disconnect(); 

            //}
            //catch (Exception ex)
            //{
            //    log.Error(NtripMountPoint + "-连接失败 。 等待3分钟后重试" + ex.Message);

            //    if (IsCancel) { return; }
            //    System.Threading.Thread.Sleep( 3 * 60 * 1000);
            //    if (IsCancel) { return; }

            //    while (!Geo.Utils.NetUtil.IsConnectedToInternet())
            //    {
            //        if (IsCancel) { return; }
            //        System.Threading.Thread.Sleep(3 * 60 * 1000);
            //    }
                 
            //    this.Request();
            //}
        }


        /// <summary>
        /// 处理套接字的响应。如果解析成功，则返回true。继续解析，否则返回false，则可以关闭之。
        /// </summary> 
        /// <param name="responseData"></param>
        private bool ProcessResponse(string responseData)
        {
            //1.返回资源表
            if (responseData.Contains(NtripResponseText.SOURCETABLE_200_OK))
            {
                ProcessSourceTable(responseData);
                return true;
            }//2.返回未授权提示
            else if (responseData.Contains(NtripResponseText._401_Unauthorized))
            {
                ProcessLoginFalure(); 
            }//3.读取数据内容
            else if (responseData.Contains(NtripResponseText.ICY_200_OK))
            {
                NoticeNtripDataState(ResponseType.ConnectedWaitingforData, "连接成功！即将返回数据", null); //ICY 200 OK, Waiting for satData
                return true;
            }
            else//4.没有响应直接关闭
            { 
                NoticeNtripDataState(ResponseType.Disconnected, "No Response.", null);
                throw new Exception("网络访问失败"); 
            }
            return false;
        }

        int count = 0;
        void NetDataReceiver_DataReceived(byte[] data)
        {
            //接收数据如实上报前台处理,并更新进度条
            NoticeNtripDataState(ResponseType.NtripClientIsReceivingData, null, data);
            count++;

            if (this.Option.IsRequiresGGA && count % 10 == 0)//有的需要手动发送GGA 
            { RequestWithGga(); }
        }


        /// <summary>
        /// 检查GGA参数是否设置，如果没有设置，则等待设置，直到外部取消。
        ///   //This is a thread-local option that can get set to false later if only need to send GGA once.
        ///This sub gets called on a new thread, it send/receives satData, waits 100ms, then loops.
        /// </summary>
        /// <returns></returns>
        private bool CheckAndLoopWaitGga()
        {
            bool result = false;
            if (string.IsNullOrEmpty(MostRecentGGA)) //Has GGA satData been received
            {
                NoticeNtripDataState(ResponseType.WaitingforNmeaGga, "等待GGA数据...", null); //Waiting for GGA satData
                int i = 0;
                while (true && i < int.MaxValue)
                {
                    i++;

                    if (!(MostRecentGGA == ""))
                    {
                        result = true;
                        break;
                    }
                    if (IsCancel) //Flag changed, kill the thread
                    {
                        result = false;
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
            return result;
        } 

        /// <summary>
        /// 附加GGA的请求
        /// </summary>
        private void RequestWithGga()
        {
            string TheGGA = "";
            if (this.Option.UseManualGGA)
            {
                TheGGA = GPS.GenerateGPGGAcode(); //This function runs in the NTRIP thread.
                //NeedsToSendGGA = False 'Only needs to be once when using a manual GGA
            }
            else
            {
                TheGGA = MostRecentGGA;
            }
            this.NetDataReceiver.SendRequest(TheGGA + "\r\n"); 
        }
        /// <summary>
        /// 数据源表处理。获取并写入到 sourcetable.dat 中。
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        private string ProcessSourceTable(string responseData)
        { 
            responseData = this.NetDataReceiver.GetTextUntil(responseData, "ENDSOURCETABLE");

          
            File.WriteAllText(sourcetablePath, responseData);

            NoticeNtripDataState(ResponseType.GotSourceTable, responseData, null); //Send on sourcetable for parsing

            NoticeNtripDataState(ResponseType.Disconnected, "Downloaded Source Table", null); 
            return responseData;
        }

        private void ProcessLoginFalure()
        {
            NoticeNtripDataState(ResponseType.Disconnected, "用户名或密码无效.", null); 
        }

        public void Dispose()
        {
            NetDataReceiver.Dispose();
        }
        #endregion
    } 
}