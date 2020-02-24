//2016.10.16, czs & double , create in hongqing, Ntrip 运行器，一个运行器可以下载一个测站的观测数据。
//2017.04.24, czs, edit in hongqing, 重构，模块化

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
using Microsoft.VisualBasic.CompilerServices;
using Gnsser.Ntrip.Rtcm;
using Geo;
using Geo.IO;
using Geo.Utils;
using Geo.Times;
using Gnsser.Data.Rinex;
//using System.Timers;

namespace Gnsser.Ntrip
{
    //2016.10.16, czs, create in hongqing, 多站 Ntrip 运行器
    /// <summary>
    /// 管理器。
    /// </summary>
    public class MultiSiteNtripRunner : BaseDictionary<string, NtripRunner>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiSiteNtripRunner() { }
        /// <summary>
        /// 初始化
        /// </summary>
        internal void Init() { foreach (var item in this) { item.Init(); } }
        /// <summary>
        /// 是否在运行。
        /// </summary>
        public bool IsRunning { get; set; }
        /// <summary>
        /// 启动
        /// </summary>
        public void Start() { foreach (var item in this) { item.Start(); } IsRunning = true; }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            foreach (var item in this)
            {
                item.Stop();
            }
            this.IsRunning = false;
        }
    }


    /// <summary>
    /// Ntrip 运行器，一个运行器可以下载一个测站的观测数据。
    /// </summary>
    public class NtripRunner : IDisposable
    { 
        Log log = new Log(typeof(NtripRunner));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public NtripRunner(NtripOption option)
        { 
            this.COMPort = new System.IO.Ports.SerialPort();
            this.COMPort.DataReceived += SerialInput;
            this.GPS = new Gnsser.Ntrip.LocalGps();
            this.Option = option; 
            this.NtripMountPoint = this.Option.PreferredMountPoint ;
            this.RtcmFileNamer = new RtcmFileNamer();
            RtcmFileNamer.NtripMountPoint = this.NtripMountPoint;

            this.IsRequiresGGA = option.IsRequiresGGA;
            this.Name = this.Option.CasterName +"->"+ this.Option.PreferredMountPoint;
            GnssSolverType = Gnsser.GnssSolverType.最简伪距定位;
        }
        
        #region 属性和参数
        public System.Threading.Thread _NtripThread;
         
        public event InfoProducedEventHandler InfoProduced;
        public event InfoProducedEventHandler InstantInfoProduced;

        public GnssSolverType GnssSolverType { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        #region 输出设备
        public RealTimeGnssDataWriter RtGnssDataWriter { get; set; }
        public Gnsser.Ntrip.LocalGps GPS { get; set; }
        public System.IO.Ports.SerialPort COMPort { get; set; }
        #endregion
        RtcmFileNamer RtcmFileNamer { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public NtripOption Option { get; set; }
        /// <summary>
        /// 起始时间utc
        /// </summary>
        public DateTime StartTimeUtc { get; set; }
         
        /// <summary>
        /// 接收到数据的总数。
        /// </summary>
        long ReceivedByteCount { get; set; }
        /// <summary>
        /// 缓存大小
        /// </summary>
        string ReceiveBuffer { get; set; }
         /// <summary>
         /// 测站，挂载点
         /// </summary>
        public string NtripMountPoint { get; set; }
        /// <summary>
        /// 格式
        /// </summary>
        public string ReceiverCorrDataType = "RTCMV3";
        /// <summary>
        /// 是否需要GGA
        /// </summary>
        public bool IsRequiresGGA { get; set; }
        /// <summary>
        /// 数据接收器
        /// </summary>
        public Rtcm.Rtcm3DataReceiver Rtcm3DataReceiver { get; set; }
        /// <summary>
        /// 网络访问器
        /// </summary>
        public NtripDataRequestor NtripDataRequestor { get; set; } 
        /// <summary>
        /// 本地写入器
        /// </summary>
        BinaryDataWriter BinaryDataWriter { get; set; } 

        /// <summary>
        /// RTCM 数据内容解析
        /// </summary>
        public RtcmDataParser RtcmDataParser { get; set; }

        public Time CurrentLocalFileTime = Time.UtcNow;

        public RealTimeGnssPositioner RealTimeGnssPositioner { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 初始化一个请求器。
        /// </summary>
        public void Init()
        {
            Setting.ReceivingTimeOfNtripData = Geo.Times.Time.UtcNow;

            //解析器 
            this.Rtcm3DataReceiver = new Rtcm.Rtcm3DataReceiver();
            this.RtcmDataParser = new RtcmDataParser(NtripMountPoint);
            this.RtGnssDataWriter = new RealTimeGnssDataWriter(this.Option.LocalDirectory, NtripMountPoint, Setting.ReceivingTimeOfNtripData);
            this.RtGnssDataWriter.BindRealTimeGnssDataProvider(this.RtcmDataParser);

            this.RtcmDataParser.EpochObservationReceived += RtcmDataParser_EpochObservationReceived;
            this.RtcmDataParser.ObsHeaderUpdated += RtcmDataParser_ObsHeaderUpdated;
            this.RtcmDataParser.EphemerisInfoReceived += RtcmDataParser_EphemerisInfoReceived;
            this.RtcmDataParser.GlonassNavRecordReceived += RtcmDataParser_GlonassNavRecordReceived;
            this.Rtcm3DataReceiver.ContentReceived += Rtcm3DataReceiver_ContentReceived;
            
            //是一个处理测试？？
            GPS.ProcessNmeaData("$GPGGA,,,,,,0,00,,,M,,M,,*66");
            GPS.GenerateGPGGAcode();
            NtripDataRequestor = new Ntrip.NtripDataRequestor(this.Option, this.GPS);
            NtripDataRequestor.NtripDataStateChanged += NtripDataStateChangedEventHander;

            RealTimeGnssPositioner = new RealTimeGnssPositioner(GnssSolverType, this.Option.LocalDirectory, NtripMountPoint, Setting.ReceivingTimeOfNtripData);
            RealTimeGnssPositioner.BindRealTimeGnssDataProvider(this.RtcmDataParser); 
        }

        #region 串口通信
        /// <summary>
        /// 串口输入。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialInput(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                ReceiveBuffer += COMPort.ReadExisting();

                if (ReceiveBuffer.IndexOf("\r\n") + 1 == 1)// == 1 为添加
                {
                    //If InStr(ReceiveBuffer, Chr(13)) Then
                    //Contains at least one carridge return
                    string[] lines = ReceiveBuffer.Split('\n');
                    //Dim lines() As String = Split(ReceiveBuffer, Chr(13))
                    for (var i = 0; i <= (lines.Length - 1) - 1; i++)
                    {
                        if (lines[(int)i].Length > 5)
                        {
                            SendSerialLineToUIThread(lines[(int)i].Trim());
                        }
                    }
                    ReceiveBuffer = lines[(lines.Length - 1)];
                }
                else
                {
                    //Data doesn't contain any line breaks
                    if (ReceiveBuffer.Length > 4000)
                    {
                        ReceiveBuffer = "";
                        SendSerialLineToUIThread("No line breaks found in data stream.");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message); 
            }
        }

        /// <summary>
        /// 发送行到串口
        /// </summary>
        /// <param name="Line"></param>
        private void SendSerialLineToUIThread(string Line)
        {
            GPS.ProcessNmeaData(Line);
        }
        #endregion

        /// <summary>
        /// 本函数会不断的执行。
        /// </summary>
        public void Start()
        {
            StartTimeUtc = DateTime.UtcNow;

            //Check the status to see if it is already connected
            if (_NtripThread != null)
            {
                if (_NtripThread.IsAlive)
                {
                    OnInfoProduced(Name + "NTRIP 已运行，正在断开.");

                    Stop();
                    return;
                }
            }

            OnInfoProduced(Name+" 启动 NTRIP 线程");
            Application.DoEvents();

            NtripDataRequestor.IsCancel = false;

            _NtripThread = new System.Threading.Thread(new System.Threading.ThreadStart(NtripLoop));
            _NtripThread.Priority = System.Threading.ThreadPriority.AboveNormal;
            _NtripThread.Start();

        }

        /// <summary>
        /// 停止接收数据
        /// </summary>
        public void Stop()
        { 
            OnInfoProduced(Name + " 正在检查并停止...");
            if (!NtripDataRequestor.IsCancel)
            {
                NtripDataRequestor.IsCancel = true;
            }
            else
            {
                OnInfoProduced(Name + " 网络访问已经断开");
            }

            if (RealTimeGnssPositioner != null && RealTimeGnssPositioner.Solver != null)
            {
                RealTimeGnssPositioner.Solver.TableTextManager.WriteAllToFileAndClearBuffer();
            }
            if (RtGnssDataWriter != null)
            {
                RtGnssDataWriter.RinexObsFileWriter.Flush();
            }

            Application.DoEvents();
            System.Threading.Thread.Sleep(1000);
            Application.DoEvents();

            //Ok, kill the thread if it is still running.
            if (_NtripThread != null && _NtripThread.IsAlive)
            {
                _NtripThread.Abort(); //Need to add the ability to truly kill a connection that is unresponsive. .Abort() doesn't seem to actually kill the thread
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();

                if (!_NtripThread.IsAlive) { OnInfoProduced(Name + " 线程停止成功。"); }
            }
            else
            {
                OnInfoProduced(Name + " 线程已经停止");
            }

            if (RtGnssDataWriter != null)
            {
                RtGnssDataWriter.Dispose();
                RtGnssDataWriter = null;
            }

            if (BinaryDataWriter != null)
            {
                BinaryDataWriter.Flush();
                BinaryDataWriter.Close();
                BinaryDataWriter = null;
            }
        }

        /// <summary>
        /// Ntrip 循环,主访问函数
        /// </summary>
        public void NtripLoop()
        {
            if (!NtripDataRequestor.IsCancel)
            {
                NtripDataRequestor.Request(); 
            }
            System.Threading.Thread.Sleep(100); 
        }
        /// <summary>
        /// 数据回调函数。与界面等外界交互的主要方法。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        /// <param name="myBytes"></param>
        private void NtripDataStateChangedEventHander(ResponseType type, string info, byte[] myBytes)
        {
            switch (type)
            {
                case ResponseType.WaitingforNmeaGga:
                    OnInfoProduced(Name + " Waiting for NMEA GGA data...");
                    break;
                case ResponseType.Connecting:
                    OnInfoProduced(Name + " NTRIP 客户端尝试连接.");
                    if (IsRequiresGGA && this.Option.UseManualGGA)
                    {
                        OnInfoProduced(Name + " NTRIP 使用模拟位置" + this.Option.ManualLat + ", " + this.Option.ManualLon);
                    }
                    break;
                case ResponseType.ConnectedRequestingData:
                    OnInfoProduced(Name + " Connected, Requesting Data...");
                    ReceivedByteCount = 0;
                    break;
                case ResponseType.ConnectedWaitingforData:
                    OnInfoProduced(Name + " Connected, Waiting for Data...");
                    break;
                case ResponseType.ShowInfo:
                    OnInfoProduced(info);
                    break;
                case ResponseType.NtripClientIsReceivingData:
                    ProcessReceivingData(myBytes);
                    break;
                case ResponseType.Disconnected: //Thread commited suicide for some reason.
                    OnInfoProduced(Name + " NTRIP 客户端已经断开, " + info);
                    Stop();
                    break;
                case ResponseType.GotSourceTable: //Got Source Table, parse it                     
                    OnInfoProduced(Name + " NTRIP 客户端下载了资源表.");
                    Stop();
                    break;
                default: break;
            }
        }
        /// <summary>
        /// 处理接收到的Ntrip数据。
        /// </summary>
        /// <param name="myBytes"></param>
        private void ProcessReceivingData(byte[] myBytes)
        { 
            //写到串口
            if (COMPort.IsOpen)
            {
                COMPort.Write(myBytes, 0, myBytes.Length);
            }
            //写到本地接收器
            if (this.Option.IsWriteToLocal)
            {
                WriteRawDataToLocal(myBytes);
            }
            //实时解析处理器
            if (Rtcm3DataReceiver != null)
            {
                Rtcm3DataReceiver.Receive(myBytes);
            }

            if (ReceivedByteCount == 0)
            {
                OnInfoProduced(Name + " NTRIP Client is receiving data.");
            }
            ReceivedByteCount += myBytes.Length;
            OnInstantInfoProduced(Name + " Connected, " + Strings.Format(ReceivedByteCount, "###,###,###,##0") + " bytes received.");

        }

        private void WriteRawDataToLocal(byte[] myBytes)
        {
            Time day = Setting.ReceivingTimeOfNtripData;//Time.UtcNow;// 
            if (BinaryDataWriter == null || CurrentLocalFileTime.DayOfYear != day.DayOfYear)
            {
                CurrentLocalFileTime = day;
                var outfileName = RtcmFileNamer.BuildRtcm3FileName();
                var path = Path.Combine(this.Option.LocalDirectory, outfileName);
                Geo.Utils.FileUtil.CheckOrCreateDirectory(Path.GetDirectoryName(path));
                BinaryDataWriter = new BinaryDataWriter(path);
            }

            BinaryDataWriter.Write(myBytes);
            BinaryDataWriter.Flush();
        }

        #region 观测文件写入文件
        void RtcmDataParser_GlonassNavRecordReceived(GlonassNavRecord obj)
        {
            //RtGnssDataWriter.WriteGlonassNavRecord(obj);
        }

        void RtcmDataParser_EphemerisInfoReceived(EphemerisParam obj)
        {
          //  RtGnssDataWriter.WriteEphemerisParam(obj);
        }

        void RtcmDataParser_ObsHeaderUpdated(RinexObsFileHeader obj)
        {
          //  RtGnssDataWriter.ReWriteHeader(obj);
        }
        /// <summary>
        /// 已接收历元数据
        /// </summary>
        /// <param name="obj"></param>
        void RtcmDataParser_EpochObservationReceived(RinexEpochObservation obj)
        {
           // RtGnssDataWriter.WriteEpochObservation(obj); 
        }
        #endregion

        void Rtcm3DataReceiver_ContentReceived(List<byte> obj)
        {
            RtcmDataParser.ParseFrame(obj);
        }

        #region 显示输出
        /// <summary>
        /// 信息输出
        /// </summary>
        /// <param name="msg"></param>
        private void OnInfoProduced(string msg) { if (InfoProduced != null) { InfoProduced(msg); } }
        private void OnInstantInfoProduced(string msg) { if (InstantInfoProduced != null) { InstantInfoProduced(msg); } }
        #endregion
         
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            //Close any open ports when the form is terminated
            if ((_NtripThread != null) && _NtripThread.IsAlive)
            {
                OnInfoProduced(Name + " 关闭 NTRIP 线程...");
                System.Threading.Thread.Sleep(10);
                Application.DoEvents();
                //Wait for the thread to notice the change and stop.
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
                if (_NtripThread.IsAlive)
                {
                    _NtripThread.Abort(); //Ok, kill the thread if it is still running.
                    System.Threading.Thread.Sleep(100);
                    Application.DoEvents();
                }

                OnInfoProduced(Name + " TRIP 线程已关闭");
                System.Threading.Thread.Sleep(10);
                Application.DoEvents();
            }


            if (COMPort.IsOpen)
            {
                OnInfoProduced(Name + " 关闭串口...");
                System.Threading.Thread.Sleep(10);
                Application.DoEvents();

                COMPort.DataReceived -= SerialInput;
                Application.DoEvents();
                System.Threading.Thread.Sleep(1000);
                //CloseMySerialPort()

                OnInfoProduced(Name + " 串口已关闭");
                System.Threading.Thread.Sleep(10);
                Application.DoEvents();
            }

            //Clean out recording queue
            GPS.WriteRecordingQueueToFile();
        }
         
        /// <summary>
        /// 关闭，释放资源
        /// </summary>
        public void Dispose()
        {
            if (RealTimeGnssPositioner != null && RealTimeGnssPositioner.Solver != null)
            {
                RealTimeGnssPositioner.Solver.TableTextManager.WriteAllToFileAndClearBuffer();
            }
            RtGnssDataWriter.Dispose();
            this.Close();
        }
        #endregion
    }
}