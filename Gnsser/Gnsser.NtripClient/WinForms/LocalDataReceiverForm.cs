//2015.02, czs, edit ,change to C#
//2015.11.05, czs & double, edit, 增加原始数据输出
//2016.10.16, czs, create in hongqing, 新设计，多站数据接收器

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Geo;
using Geo.IO;
using System.Windows.Forms;
using Geo;
using Geo.IO;
using System.Windows.Forms;
using Geo.Coordinates;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Service;
using Geo.Times;
using Geo;
using Geo.IO;
using Geo.Winform;
using Geo.Draw;

namespace Gnsser.Ntrip.WinForms
{
    /// <summary>
    /// 多站数据接收器
    /// </summary>
    public partial class LocalDataReceiverForm : Form
    {
        Log log = new Log(typeof(LocalDataReceiverForm));
        public LocalDataReceiverForm()
        {
            InitializeComponent();
        }
        #region 属性
        LogWriter LogWriter = LogWriter.Instance;

        bool isStopLocalReading = false;
        string NtripMountPoint { get; set; }
        RtcmFileNamer RtcmFileNamer { get; set; }
        RealTimeGnssDataWriter RealTimeGnssDataWriter;

        Rtcm.Rtcm3DataReceiver Rtcm3DataReceiver;
        BinaryDataWriter BinaryDataWriter;
        long ReceivedByteCount { get; set; }
        string LocalDirectory { get { return this.directorySelectionControl1.Path; } }
        bool IsWriteToLocal { get { return checkBox_saveRawData.Checked; } }
        #endregion

        private void button_start_Click(object sender, EventArgs e)
        {
            isStopLocalReading = false;
            string filePath = this.fileOpenControl1.FilePath;
            if (!File.Exists(filePath))
            {
                MessageBox.Show("文件不存在" + filePath);
                return;
            }
            button_start.Enabled = false;
            this.backgroundWorker1.RunWorkerAsync();
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            isStopLocalReading = true;
            button_start.Enabled = true;
        }


        protected void ShowInfo(string msg) { Geo.Utils.FormUtil.InsertLineWithTimeToTextBox(this.richTextBoxControl1, msg); }
        /// <summary>
        /// 显示瞬时信息，显示完毕则丢失。
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowInstantInfo(string msg)
        {
            Geo.Utils.FormUtil.SetText(this.label_info, msg);
        }

        private void DataReceiverForm_Load(object sender, EventArgs e)
        {
            LogWriter.MsgProduced += LogWriter_MsgProduced;
        }

        void LogWriter_MsgProduced(string msg, LogType LogType, Type msgProducer) { ShowInfo(msg); }
  

        private void NTRIPCallBacktoUIThread(ResponseType Item, string Value, byte[] myBytes)
        {
            switch (Item)
            {
                case ResponseType.NtripClientIsReceivingData:

                    //写到本地接收器
                    if (IsWriteToLocal)
                    {
                        var path = Path.Combine(LocalDirectory, RtcmFileNamer.BuildRtcm3FileName());

                        Geo.Utils.FileUtil.CheckOrCreateDirectory(Path.GetDirectoryName(path));

                        if (BinaryDataWriter == null) { BinaryDataWriter = new BinaryDataWriter(path); }
                        BinaryDataWriter.Write(myBytes);
                    }

                  

                    Rtcm3DataReceiver.Receive(myBytes); 

                    if (ReceivedByteCount == 0)
                    {
                        ShowInfo("NTRIP Client is receiving data.");
                    }

                    //LogEvent("" + Value); 

                    ReceivedByteCount += myBytes.Length;
                    ShowInstantInfo("Connected, " + ReceivedByteCount + " bytes received.");

                    this.progressBarComponent1.SetCurrentPercessValue(ReceivedByteCount);
                    break;

                case ResponseType.Disconnected: //Thread commited suicide for some reason.                   

                    //lblNTRIPStatus.Text = "Disconnected, " & Value
                    ShowInfo("NTRIP 客户端已经断开, " + Value);
                    break;

            }
        }

        void Rtcm3DataReceiver_ContentReceived(List<byte> obj)
        {
            RtcmDataParser.ParseFrame(obj);
        }
        Rtcm.RtcmDataParser RtcmDataParser { get; set; }
        RealTimeGnssPositioner RealTimeGnssPositioner{ get; set; }

         

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        { 

            log.Info("启动本地Ntrip数据传输");
            ReceivedByteCount = 0;
            string filePath = this.fileOpenControl1.FilePath;
            var fileName = Path.GetFileName(filePath);

            FileInfo info = new FileInfo(filePath);
            progressBarComponent1.InitProcess(info.Length);

            RtcmFileNamer = RtcmFileNamer.Parse(fileName);
            this.NtripMountPoint = RtcmFileNamer.NtripMountPoint;
            RealTimeGnssDataWriter = new Ntrip.RealTimeGnssDataWriter(this.LocalDirectory, NtripMountPoint, RtcmFileNamer.Time);
            //解析器 
            Rtcm3DataReceiver = new Rtcm.Rtcm3DataReceiver();
            RtcmDataParser = new Rtcm.RtcmDataParser(NtripMountPoint); 
            RealTimeGnssDataWriter.BindRealTimeGnssDataProvider(RtcmDataParser);

            IEphemerisService indicatedEph = null;
            if (System.IO.File.Exists(fileOpenControl_nav.FilePath))
            {
                indicatedEph = EphemerisDataSourceFactory.Create(fileOpenControl_nav.FilePath);
            }
            var type = GnssSolverTypeHelper.GetGnssSolverType(this.singleSiteGnssSolverTypeSelectionControl1.CurrentdType);

            RealTimeGnssPositioner = new RealTimeGnssPositioner(type, this.LocalDirectory, NtripMountPoint, RtcmFileNamer.Time);
            RealTimeGnssPositioner.BindRealTimeGnssDataProvider(this.RtcmDataParser);

            RealTimeGnssPositioner.EphemerisService = indicatedEph; 

            Rtcm3DataReceiver.ContentReceived += Rtcm3DataReceiver_ContentReceived;


            BinaryReader reader = new BinaryReader(new FileStream(filePath, FileMode.Open, FileAccess.Read), Encoding.ASCII);
            int readCount = 100;
            List<Byte> list = new List<byte>();
            reader.BaseStream.Position = 0;
            while (!isStopLocalReading && reader.PeekChar() != -1)
            {
                var bt = reader.ReadByte();
                list.Add(bt);
                if (list.Count >= readCount)
                {
                    this.NTRIPCallBacktoUIThread(ResponseType.NtripClientIsReceivingData, "正在传送本地数据", list.ToArray());

                    list.Clear();

                    System.Threading.Thread.Sleep(3);
                }
            }

            if (RealTimeGnssPositioner != null && RealTimeGnssPositioner.Solver != null)
            {
                RealTimeGnssPositioner.Solver.TableTextManager.WriteAllToFileAndClearBuffer();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (RealTimeGnssDataWriter != null)
            {
                RealTimeGnssDataWriter.Dispose();
            }

            string filePath = this.fileOpenControl1.FilePath; 
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(LocalDirectory);
          
            log.Info("本地数据传输完毕");
            button_stop_Click(null, null);
            progressBarComponent1.Full();
        }

        private void LocalDataReceiverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("请留步，后台没有运行完毕。是否一定要退出？") == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    LogWriter.MsgProduced -= LogWriter_MsgProduced;
                }
            }
            else
            {
                //IsClosed = true;
                LogWriter.MsgProduced -= LogWriter_MsgProduced;
            }
        }

        private void button_drawDxyz_Click(object sender, EventArgs e)
        {
            if (RealTimeGnssPositioner == null) { return; }

            var table = RealTimeGnssPositioner.Solver.TableTextManager.First;
            this.paramVectorRenderControl1.SetTableTextStorage(table);// (names);
            this.paramVectorRenderControl1.DrawParamLines();   
         //   paramVectorRenderControl1.DrawTable(table);
            //ChartForm form = new ChartForm(table);
            //form.Show();
        }

    }


   
}