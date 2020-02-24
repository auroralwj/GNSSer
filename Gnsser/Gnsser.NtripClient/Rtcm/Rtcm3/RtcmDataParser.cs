//2015.02.12, czs, create in pengzhou, BitConvertUtil
//2016.01.23, czs & double, edit in hongqing, 修复编码问题，1019和1004解析正确！移动工具方法到Utils中
//2016.10.16&17,double, edit in hongqing, 正确解析SSR信息，并输出到文件，梳理了代码
//2016.10.18, double, edit in hongqing, 初步实现了解析了GPS和BDS的MSM文件，并输出到O 文件，还需要进一步调试。GLONASS的MSM还需要进一步进行研究。
//2017.04.24, czs, edit in hongqing, 重构将文件输出提出本类


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;
using System.IO;
using Geo.Times;
using Geo.IO;
using Gnsser.Data.Rinex;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// RTCM 数据解析。
    /// </summary>
    public class RtcmDataParser : RealTimeGnssDataProvider
    {
        ILog log = new Log(typeof(RtcmDataParser));

        #region 初始化，构造函数
        /// <summary>
        /// 解析器构造函数
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="version"></param>
        public RtcmDataParser(string siteName,  double version = 3.02)
        {
            this.SiteName = siteName;
            this.Rtcm3Converter = new Rtcm3Converter();
            this.ObsHeader = CreateOFileHeader(version);
            OnObsHeaderCreated(ObsHeader);
            this.BinStringSequence = new StringSequence();
            this.EphMessageConverter = new RtcmEphMessageConverter();
        }
        #endregion


        #region 常用属性
        RtcmSSRMessageConverter SSRMessageConverter { get; set; }
        /// <summary>
        /// RTCM3 转换器
        /// </summary>
        Rtcm3Converter Rtcm3Converter { get; set; }
        /// <summary>
        /// 头部观测信息
        /// </summary>
        public RinexObsFileHeader ObsHeader { get; set; }
        
        RtcmEphMessageConverter EphMessageConverter { get; set; }
        /// <summary>
        /// 二进制字符串
        /// </summary>
        StringSequence BinStringSequence { get; set; }
        public HeaderOfMSM HeaderOfMSM { get; set; }

        #endregion

        /// <summary>
        /// 解析一个帧
        /// </summary>
        /// <param name="satData"></param>
        public void ParseFrame(List<byte> data)
        {
            var binStr = BitConvertUtil.GetBinString(data);
            BinStringSequence.EnQuence(binStr);

            //首先获取并判断消息类型
            int msgTypeNum = 0;
            while (BinStringSequence.Capacity > 0 && msgTypeNum < 200)
            {
                int msgNumber = BitConvertUtil.GetInt(BinStringSequence.GetQueue(12));

                if (msgNumber == 0) break;

                //log.Debug("正在解析：" + msgNumber); 

                ParseOneMessage(msgNumber, data);
                msgTypeNum++;
            }
            BinStringSequence.Clear();
        }
        /// <summary>
        /// 解析一个信息。
        /// </summary>
        /// <param name="satData">传入的数据，包含一条完整帧的数据。</param>
        /// <param name="msgNumber">信息编号</param>
        private void ParseOneMessage(int msgNumber, List<byte> data)
        {
            #region 1001 - 1004 GPS 观测量相关
            if (msgNumber >= 1001 && msgNumber <= 1004)
            {
                var gpsHeaderData = BinStringSequence.DeQueue(64);
                GpsMessageHeader RtcmHeader = GpsMessageHeader.Parse(gpsHeaderData);
                EpochMessage EpochMessage = new EpochMessage { Header = new NormalHeader(RtcmHeader) };
                //本地时间同步 
                Setting.ReceivingTimeOfNtripData = EpochMessage.Header.Time;
                for (int i = 0; i < RtcmHeader.MessageCount; i++)
                {
                    switch (msgNumber)
                    {
                        case 1001:
                            Message1001 Message1001 = Message1001.Parse(BinStringSequence.DeQueue(58));
                            break;
                        case 1002:
                            Message1002 Message1002 = Message1002.Parse(BinStringSequence.DeQueue(74));
                            break;
                        case 1003:
                            Message1003 Message1003 = Message1003.Parse(BinStringSequence.DeQueue(101));
                            break;
                        case 1004:
                            Message1004 Message1004 = Message1004.Parse(BinStringSequence.DeQueue(125));

                            NormalMessage1004 msg = new NormalMessage1004(RtcmHeader, Message1004);
                            #region 测试用，可删除
                            //name = Path.Combine(LocalDirectory, @"Rtcm3\Raw\Mormal_1004_" + msg.Prn + ".xls");
                            //Geo.Utils.FileUtil.CheckOrCreateDirectory(System.IO.Path.GetDirectoryName(name));
                            //if (!System.IO.File.Exists(name))
                            //{
                            //    System.IO.File.AppendAllText(name, msg.GetTabTitles() + "\r\n");
                            //}

                            //System.IO.File.AppendAllText(name, msg.GetTabValues() + "\r\n");
                            #endregion
                            EpochMessage.Add(msg.Prn, msg);
                            break;
                    }
                }
                var EpochObservation = Rtcm3Converter.GetEpochObservation(EpochMessage);
                EpochObservation.Header = ObsHeader;
                EpochObservation.Name = SiteName;
                OnEpochObservationReceived(EpochObservation);
            }
            #endregion

            #region 1005-1013,1033,天线相关
            int parsedLen = 0;
            //Antenna Description Messages
            switch (msgNumber)
            {
                case 1005:
                    Message1005 Message1005 = Message1005.Parse(BinStringSequence.DeQueue(152));
                    break;
                case 1006://测站参考点信息
                    Message1006 Message1006 = Message1006.Parse(BinStringSequence.DeQueue(168));
                    UpdateOFileHeader(Message1006);
                    break;
                case 1007:
                    Message1007 Message1007 = Message1007.Parse(data);
                    parsedLen = 40 + (int)Message1007.DescriptorCounterN * 8;
                    BinStringSequence.DeQueue(parsedLen);
                    break;
                case 1008:
                    Message1008 Message1008 = Message1008.Parse(data);
                    parsedLen = 48 + (int)Message1008.DescriptorCounterN * 8 + (int)Message1008.SerialNumberCounterM * 8;
                    BinStringSequence.DeQueue(parsedLen);
                    break;
                case 1013:
                    Message1013 Message1013 = Message1013.Parse(data);
                    parsedLen = 70 + (int)Message1013.NoofMessageIDAnnouncementstoFollow_Nm * 29;
                    BinStringSequence.DeQueue(parsedLen);
                    break;

                case 1033://天线信息
                    Message1033 Message1033 = Message1033.Parse(data);
                    UpdateOFileHeader(Message1033);
                    parsedLen = Message1033.Length;
                    //parsedLen = 72 +
                    //    (Message1033.AntennaDescriptorCounterN
                    //    + Message1033.AntennaSerialNumberCounterM
                    //    + Message1033.ReceiverTypeDescriptorCounterI
                    //    + Message1033.ReceiverFirmwareVersionCounterJ
                    //    + Message1033.ReceiverSerialNumberCounterK
                    //    )
                    //    * 8;
                    BinStringSequence.DeQueue(parsedLen);
                    break;
            }
            #endregion

            #region 1009 - 1012 Glonass 观测量相关
            //Glonass 
            if (msgNumber >= 1009 && msgNumber <= 1012)
            {
                var gpsHeaderData = BinStringSequence.DeQueue(61);
                GlonassMessageHeader RtcmHeader = GlonassMessageHeader.Parse(gpsHeaderData);
                GlonassEpochMessage EpochMessage = new GlonassEpochMessage { Header = new GlonassNormalHeader(RtcmHeader) };
                //本地时间同步 
                Setting.ReceivingTimeOfNtripData = EpochMessage.Header.Time;
                for (int i = 0; i < RtcmHeader.MessageCount; i++)
                {
                    switch (msgNumber)
                    {
                        case 1009:
                            Message1009 Message1009 = Message1009.Parse(BinStringSequence.DeQueue(64));
                            break;
                        case 1010:
                            Message1010 Message1010 = Message1010.Parse(BinStringSequence.DeQueue(79));
                            break;
                        case 1011:
                            Message1011 Message1011 = Message1011.Parse(BinStringSequence.DeQueue(107));
                            break;
                        case 1012:
                            Message1012 Message1012 = Message1012.Parse(BinStringSequence.DeQueue(130));

                            GlonassNormalMessage1012 msg = new GlonassNormalMessage1012(RtcmHeader, Message1012);
                            EpochMessage.Add(msg.Prn, msg);
                            break;

                        default: break;
                    }
                }

                var EpochObservation = Rtcm3Converter.GetEpochObservation(EpochMessage);
                EpochObservation.Header = ObsHeader;
                EpochObservation.Name = SiteName;
                OnEpochObservationReceived(EpochObservation);
            }
            #endregion

            #region 1019、1020、1044-1046 各系统星历数据
            #region message1019  GPS 星历数据
            if (msgNumber == 1019)
            {
                Message1019 msg = Message1019.Parse(BinStringSequence.DeQueue(488));
                var EphemerisObs = EphMessageConverter.GetEphemerisParam(msg);
                OnEphemerisInfoReceived(EphemerisObs);
            }

            #endregion

            #region message1020  GLONASS 星历数据
            if (msgNumber == 1020)
            {
                Message1020 msg = Message1020.Parse(BinStringSequence.DeQueue(360));
                RtcmEphMessageConverter EphMessageConverter = new RtcmEphMessageConverter();
                var EphemerisObs = EphMessageConverter.GlonassNavRecord(msg);
                OnGlonassNavRecordReceived(EphemerisObs);
            }
            #endregion
            #region message63  BDS 星历数据
            if (msgNumber == 63)
            {
                Message63 msg = Message63.Parse(BinStringSequence.DeQueue(511));
                var EphemerisObs = EphMessageConverter.GetEphemerisParam(msg);

                OnEphemerisInfoReceived(EphemerisObs);
            }

            #endregion
            #region message1044  QZSS 星历数据
            if (msgNumber == 1044)
            {
                Message1044 msg = Message1044.Parse(BinStringSequence.DeQueue(485));
                var EphemerisObs = EphMessageConverter.GetEphemerisParam(msg);

                OnEphemerisInfoReceived(EphemerisObs);
            }

            #endregion
            #region message1045  Galileo 星历数据
            if (msgNumber == 1045)
            {
                Message1045 msg = Message1045.Parse(BinStringSequence.DeQueue(496));
                var EphemerisObs = EphMessageConverter.GetEphemerisParam(msg);

                OnEphemerisInfoReceived(EphemerisObs);
            }

            #endregion

            #endregion

            #region message1057 - 1062 GPS SSR

            SSRMessageConverter = new RtcmSSRMessageConverter();
            if (msgNumber == 1057)//SSR GPS Orbit Correction
            {
                var SSRGpsOrbitHeaderData = BinStringSequence.DeQueue(68);
                SSRGpsHeader68 SSRGpsOrbitHeader = SSRGpsHeader68.Parse(SSRGpsOrbitHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRGpsOrbitHeader.NoofSatellite; i++)
                {
                    var Sp3Record = new Ephemeris();
                    Message1057 Message1057 = Message1057.Parse(BinStringSequence.DeQueue(135));
                    Sp3Record = SSRMessageConverter.GetSp3Param(Message1057, SSRGpsOrbitHeader);
                    Sp3Section.Add(Sp3Record.Prn, Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }
            if (msgNumber == 1058)
            {
                var SSRGpsClockCorrectionHeaderData = BinStringSequence.DeQueue(67);
                SSRGpsHeader67 SSRGpsClockCorrectionHeader = SSRGpsHeader67.Parse(SSRGpsClockCorrectionHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRGpsClockCorrectionHeader.NoofSatellite; i++)
                {
                    Message1058 Message1058 = Message1058.Parse(BinStringSequence.DeQueue(76));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1058, SSRGpsClockCorrectionHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }
            //if (Sp3Section.Count != 0 && msgNumber == 1058)

            if (msgNumber == 1059)
            {
                var SSRGpsSatelliteCodeBiasHeaderData = BinStringSequence.DeQueue(67);
                SSRGpsHeader67 SSRGpsSatelliteCodeBiasHeader = SSRGpsHeader67.Parse(SSRGpsSatelliteCodeBiasHeaderData);
                Message1059 Message1059 = Message1059.Parse(BinStringSequence.DeQueue(30));
            }
            if (msgNumber == 1060)
            {
                var SSRGpsCombinedHeaderData = BinStringSequence.DeQueue(68);
                SSRGpsHeader68 SSRGpsCombinedHeader = SSRGpsHeader68.Parse(SSRGpsCombinedHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRGpsCombinedHeader.NoofSatellite; i++)
                {
                    Message1060 Message1060 = Message1060.Parse(BinStringSequence.DeQueue(205));
                    NormalMessage1060 msg = new NormalMessage1060(Message1060);

                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1060, SSRGpsCombinedHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }
            if (msgNumber == 1061)
            {
                var SSRGpsURAHeaderData = BinStringSequence.DeQueue(67);
                SSRGpsHeader67 SSRGpsURAHeader = SSRGpsHeader67.Parse(SSRGpsURAHeaderData);
                Message1061 Message1061 = Message1061.Parse(BinStringSequence.DeQueue(12));
            }
            if (msgNumber == 1062)
            {
                var SSRGpsHighRateClockHeaderData = BinStringSequence.DeQueue(67);
                SSRGpsHeader67 SSRGpsHighRateClockHeader = SSRGpsHeader67.Parse(SSRGpsHighRateClockHeaderData);
                Message1062 Message1062 = Message1062.Parse(BinStringSequence.DeQueue(28));
            }

            #endregion

            #region message1063 - 1068 GLONASS SSR

            if (msgNumber == 1063)//SSR GLONASS Orbit Correction
            {
                var SSRGlonassOrbitHeaderData = BinStringSequence.DeQueue(65);
                SSRGlonassHeader65 SSRGlonassOrbitHeader = SSRGlonassHeader65.Parse(SSRGlonassOrbitHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRGlonassOrbitHeader.NoofSatellite; i++)
                {
                    Message1063 Message1063 = Message1063.Parse(BinStringSequence.DeQueue(134));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1063, SSRGlonassOrbitHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                } 
                //OnSSRSp3RecordReceived(Sp3Section);
            }
            if (msgNumber == 1064)
            {
                var SSRGlonassClockCorrectionHeaderData = BinStringSequence.DeQueue(64);
                SSRGlonassHeader64 SSRGlonassClockCorrectionHeader = SSRGlonassHeader64.Parse(SSRGlonassClockCorrectionHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRGlonassClockCorrectionHeader.NoofSatellite; i++)
                {
                    Message1064 Message1064 = Message1064.Parse(BinStringSequence.DeQueue(75));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1064, SSRGlonassClockCorrectionHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                //OnSSRSp3RecordReceived(Sp3Section);
            }
            if (msgNumber == 1065)
            {
                var SSRGlonassSatelliteCodeBiasHeaderData = BinStringSequence.DeQueue(64);
                SSRGlonassHeader64 SSRGlonassSatelliteCodeBiasHeader = SSRGlonassHeader64.Parse(SSRGlonassSatelliteCodeBiasHeaderData);
                for (int i = 0; i < SSRGlonassSatelliteCodeBiasHeader.NoofSatellite; i++)
                {
                    Message1065 Message1065 = Message1065.Parse(BinStringSequence.DeQueue(30));
                }
            }
            if (msgNumber == 1066)
            {
                var SSRGlonassCombinedHeaderData = BinStringSequence.DeQueue(65);
                SSRGlonassHeader65 SSRGlonassCombinedHeader = SSRGlonassHeader65.Parse(SSRGlonassCombinedHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRGlonassCombinedHeader.NoofSatellite; i++)
                {
                    Message1066 Message1066 = Message1066.Parse(BinStringSequence.DeQueue(204));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1066, SSRGlonassCombinedHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                //OnSSRSp3RecordReceived(Sp3Section);

            }
            if (msgNumber == 1067)
            {
                var SSRGlonassURAHeaderData = BinStringSequence.DeQueue(64);
                SSRGlonassHeader64 SSRGlonassURAHeader = SSRGlonassHeader64.Parse(SSRGlonassURAHeaderData);
                for (int i = 0; i < SSRGlonassURAHeader.NoofSatellite; i++)
                {
                    Message1067 Message1067 = Message1067.Parse(BinStringSequence.DeQueue(11));
                }
            }
            if (msgNumber == 1068)
            {
                var SSRGlonassHighRateClockHeaderData = BinStringSequence.DeQueue(64);
                SSRGlonassHeader64 SSRGlonassHighRateClockHeader = SSRGlonassHeader64.Parse(SSRGlonassHighRateClockHeaderData);
                for (int i = 0; i < SSRGlonassHighRateClockHeader.NoofSatellite; i++)
                {
                    Message1068 Message1068 = Message1068.Parse(BinStringSequence.DeQueue(27));
                }
            }

            #endregion

            #region message1258 - 1263 BeiDou SSR

            //var Sp3Record = new Sp3Record();
            SSRMessageConverter = new RtcmSSRMessageConverter();
            if (msgNumber == 1258)//SSR BeiDou Orbit Correction
            {
                var SSRBeiDouOrbitHeaderData = BinStringSequence.DeQueue(68);
                SSRBeiDouHeader68 SSRBeiDouOrbitHeader = SSRBeiDouHeader68.Parse(SSRBeiDouOrbitHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRBeiDouOrbitHeader.NoofSatellite; i++)
                {
                    Message1258 Message1258 = Message1258.Parse(BinStringSequence.DeQueue(161));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1258, SSRBeiDouOrbitHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }
            if (msgNumber == 1259)
            {
                var SSRBeiDouClockCorrectionHeaderData = BinStringSequence.DeQueue(67);
                SSRBeiDouHeader67 SSRBeiDouClockCorrectionHeader = SSRBeiDouHeader67.Parse(SSRBeiDouClockCorrectionHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRBeiDouClockCorrectionHeader.NoofSatellite; i++)
                {
                    Message1259 Message1259 = Message1259.Parse(BinStringSequence.DeQueue(76));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1259, SSRBeiDouClockCorrectionHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }
            //if (Sp3Section.Count != 0 && msgNumber == 1058)

            if (msgNumber == 1260)
            {
                var SSRBeiDouSatelliteCodeBiasHeaderData = BinStringSequence.DeQueue(67);
                SSRBeiDouHeader67 SSRBeiDouSatelliteCodeBiasHeader = SSRBeiDouHeader67.Parse(SSRBeiDouSatelliteCodeBiasHeaderData);
                Message1260 Message1260 = Message1260.Parse(BinStringSequence.DeQueue(30));
            }
            if (msgNumber == 1261)
            {
                var SSRBeiDouCombinedHeaderData = BinStringSequence.DeQueue(68);
                SSRBeiDouHeader68 SSRBeiDouCombinedHeader = SSRBeiDouHeader68.Parse(SSRBeiDouCombinedHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                SSRMessage SSRMessage = new SSRMessage { };
                for (int i = 0; i < SSRBeiDouCombinedHeader.NoofSatellite; i++)
                {
                    Message1261 Message1261 = Message1261.Parse(BinStringSequence.DeQueue(231));
                    NormalMessage1261 msg = new NormalMessage1261(Message1261);

                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1261, SSRBeiDouCombinedHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }
            if (msgNumber == 1262)
            {
                var SSRBeiDouURAHeaderData = BinStringSequence.DeQueue(67);
                SSRBeiDouHeader67 SSRBeiDouURAHeader = SSRBeiDouHeader67.Parse(SSRBeiDouURAHeaderData);
                Message1262 Message1262 = Message1262.Parse(BinStringSequence.DeQueue(12));
            }
            if (msgNumber == 1263)
            {
                var SSRBeiDouHighRateClockHeaderData = BinStringSequence.DeQueue(67);
                SSRBeiDouHeader67 SSRBeiDouHighRateClockHeader = SSRBeiDouHeader67.Parse(SSRBeiDouHighRateClockHeaderData);
                Message1263 Message1263 = Message1263.Parse(BinStringSequence.DeQueue(28));
            }

            #endregion

            #region message1240 - 1245 Galileo SSR

            //var Sp3Record = new Sp3Record();
            SSRMessageConverter = new RtcmSSRMessageConverter();
            if (msgNumber == 1240)//SSR Galileo Orbit Correction
            {
                var SSRGalileoOrbitHeaderData = BinStringSequence.DeQueue(68);
                SSRGalileoHeader68 SSRGalileoOrbitHeader = SSRGalileoHeader68.Parse(SSRGalileoOrbitHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRGalileoOrbitHeader.NoofSatellite; i++)
                {
                    Message1240 Message1240 = Message1240.Parse(BinStringSequence.DeQueue(137));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1240, SSRGalileoOrbitHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);

            }
            if (msgNumber == 1241)
            {
                var SSRGalileoClockCorrectionHeaderData = BinStringSequence.DeQueue(67);
                SSRGalileoHeader67 SSRGalileoClockCorrectionHeader = SSRGalileoHeader67.Parse(SSRGalileoClockCorrectionHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRGalileoClockCorrectionHeader.NoofSatellite; i++)
                {

                    Message1241 Message1241 = Message1241.Parse(BinStringSequence.DeQueue(76));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1241, SSRGalileoClockCorrectionHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }
            if (msgNumber == 1242)
            {
                var SSRGalileoSatelliteCodeBiasHeaderData = BinStringSequence.DeQueue(67);
                SSRGalileoHeader67 SSRGalileoSatelliteCodeBiasHeader = SSRGalileoHeader67.Parse(SSRGalileoSatelliteCodeBiasHeaderData);
                for (int i = 0; i < SSRGalileoSatelliteCodeBiasHeader.NoofSatellite; i++)
                {

                }
                Message1242 Message1242 = Message1242.Parse(BinStringSequence.DeQueue(30));
            }
            if (msgNumber == 1243)
            {
                var SSRGalileoCombinedHeaderData = BinStringSequence.DeQueue(68);
                SSRGalileoHeader68 SSRGalileoCombinedHeader = SSRGalileoHeader68.Parse(SSRGalileoCombinedHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                SSRMessage SSRMessage = new SSRMessage { };
                for (int i = 0; i < SSRGalileoCombinedHeader.NoofSatellite; i++)
                {
                    Message1243 Message1243 = Message1243.Parse(BinStringSequence.DeQueue(207));
                    NormalMessage1243 msg = new NormalMessage1243(Message1243);

                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1243, SSRGalileoCombinedHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }
            if (msgNumber == 1244)
            {
                var SSRGalileoURAHeaderData = BinStringSequence.DeQueue(67);
                SSRGalileoHeader67 SSRGalileoURAHeader = SSRGalileoHeader67.Parse(SSRGalileoURAHeaderData);
                Message1244 Message1244 = Message1244.Parse(BinStringSequence.DeQueue(12));
            }
            if (msgNumber == 1245)
            {
                var SSRGalileoHighRateClockHeaderData = BinStringSequence.DeQueue(67);
                SSRGalileoHeader67 SSRGalileoHighRateClockHeader = SSRGalileoHeader67.Parse(SSRGalileoHighRateClockHeaderData);
                Message1245 Message1245 = Message1245.Parse(BinStringSequence.DeQueue(28));
            }

            #endregion

            #region message1246 - 1251 QZSS SSR


            SSRMessageConverter = new RtcmSSRMessageConverter();
            if (msgNumber == 1246)//SSR QZSS Orbit Correction
            {
                var SSRQZSSOrbitHeaderData = BinStringSequence.DeQueue(68);
                SSRQZSSHeader68 SSRQZSSOrbitHeader = SSRQZSSHeader68.Parse(SSRQZSSOrbitHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRQZSSOrbitHeader.NoofSatellite; i++)
                {
                    Message1246 Message1246 = Message1246.Parse(BinStringSequence.DeQueue(133));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1246, SSRQZSSOrbitHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);

            }
            if (msgNumber == 1247)
            {
                var SSRQZSSClockCorrectionHeaderData = BinStringSequence.DeQueue(67);
                SSRQZSSHeader67 SSRQZSSClockCorrectionHeader = SSRQZSSHeader67.Parse(SSRQZSSClockCorrectionHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRQZSSClockCorrectionHeader.NoofSatellite; i++)
                {
                    Message1247 Message1247 = Message1247.Parse(BinStringSequence.DeQueue(74));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1247, SSRQZSSClockCorrectionHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }

            if (msgNumber == 1248)
            {
                var SSRQZSSSatelliteCodeBiasHeaderData = BinStringSequence.DeQueue(67);
                SSRQZSSHeader67 SSRQZSSSatelliteCodeBiasHeader = SSRQZSSHeader67.Parse(SSRQZSSSatelliteCodeBiasHeaderData);
                Message1248 Message1248 = Message1248.Parse(BinStringSequence.DeQueue(30));
            }
            if (msgNumber == 1249)
            {
                var SSRQZSSCombinedHeaderData = BinStringSequence.DeQueue(68);
                SSRQZSSHeader68 SSRQZSSCombinedHeader = SSRQZSSHeader68.Parse(SSRQZSSCombinedHeaderData);
                SSRMessage SSRMessage = new SSRMessage { };
                for (int i = 0; i < SSRQZSSCombinedHeader.NoofSatellite; i++)
                {
                    Message1249 Message1249 = Message1249.Parse(BinStringSequence.DeQueue(203));
                    NormalMessage1249 msg = new NormalMessage1249(Message1249);

                }
            }
            if (msgNumber == 1250)
            {
                var SSRQZSSURAHeaderData = BinStringSequence.DeQueue(67);
                SSRQZSSHeader67 SSRQZSSURAHeader = SSRQZSSHeader67.Parse(SSRQZSSURAHeaderData);
                Message1250 Message1250 = Message1250.Parse(BinStringSequence.DeQueue(10));
            }
            if (msgNumber == 1251)
            {
                var SSRQZSSHighRateClockHeaderData = BinStringSequence.DeQueue(67);
                SSRQZSSHeader67 SSRQZSSHighRateClockHeader = SSRQZSSHeader67.Parse(SSRQZSSHighRateClockHeaderData);
                Message1251 Message1251 = Message1251.Parse(BinStringSequence.DeQueue(26));
            }

            #endregion

            #region message1252 - 1257 SBS SSR

            SSRMessageConverter = new RtcmSSRMessageConverter();
            if (msgNumber == 1252)//SSR SBS Orbit Correction
            {
                var SSRSBSOrbitHeaderData = BinStringSequence.DeQueue(68);
                SSRSBSHeader68 SSRSBSOrbitHeader = SSRSBSHeader68.Parse(SSRSBSOrbitHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRSBSOrbitHeader.NoofSatellite; i++)
                {
                    Message1252 Message1252 = Message1252.Parse(BinStringSequence.DeQueue(160));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1252, SSRSBSOrbitHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }
            if (msgNumber == 1253)
            {
                var SSRSBSClockCorrectionHeaderData = BinStringSequence.DeQueue(67);
                SSRSBSHeader67 SSRSBSClockCorrectionHeader = SSRSBSHeader67.Parse(SSRSBSClockCorrectionHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                for (int i = 0; i < SSRSBSClockCorrectionHeader.NoofSatellite; i++)
                {
                    Message1253 Message1253 = Message1253.Parse(BinStringSequence.DeQueue(76));
                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1253, SSRSBSClockCorrectionHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }
            //if (Sp3Section.Count != 0 && msgNumber == 1058)

            if (msgNumber == 1254)
            {
                var SSRSBSSatelliteCodeBiasHeaderData = BinStringSequence.DeQueue(67);
                SSRSBSHeader67 SSRSBSSatelliteCodeBiasHeader = SSRSBSHeader67.Parse(SSRSBSSatelliteCodeBiasHeaderData);
                Message1254 Message1254 = Message1254.Parse(BinStringSequence.DeQueue(30));
            }
            if (msgNumber == 1255)
            {
                var SSRSBSCombinedHeaderData = BinStringSequence.DeQueue(68);
                SSRSBSHeader68 SSRSBSCombinedHeader = SSRSBSHeader68.Parse(SSRSBSCombinedHeaderData);
                Sp3Section Sp3Section = new Sp3Section();
                SSRMessage SSRMessage = new SSRMessage() { };
                for (int i = 0; i < SSRSBSCombinedHeader.NoofSatellite; i++)
                {
                    Message1255 Message1255 = Message1255.Parse(BinStringSequence.DeQueue(230));
                    NormalMessage1255 msg = new NormalMessage1255(Message1255);

                    var Sp3Record = SSRMessageConverter.GetSp3Param(Message1255, SSRSBSCombinedHeader);
                    Sp3Section.Add(Sp3Record.Prn, (Ephemeris)Sp3Record);
                    Sp3Section.Time = Sp3Record.Time;
                }
                OnSSRSp3RecordReceived(Sp3Section);
            }
            if (msgNumber == 1256)
            {
                var SSRSBSURAHeaderData = BinStringSequence.DeQueue(67);
                SSRSBSHeader67 SSRSBSURAHeader = SSRSBSHeader67.Parse(SSRSBSURAHeaderData);
                Message1256 Message1256 = Message1256.Parse(BinStringSequence.DeQueue(12));
            }
            if (msgNumber == 1257)
            {
                var SSRSBSHighRateClockHeaderData = BinStringSequence.DeQueue(67);
                SSRSBSHeader67 SSRSBSHighRateClockHeader = SSRSBSHeader67.Parse(SSRSBSHighRateClockHeaderData);
                Message1257 Message1257 = Message1257.Parse(BinStringSequence.DeQueue(28));
            }
            #endregion

            #region MSM

            if (msgNumber >= 1071 && msgNumber <= 1077)//(msgNumber >= 1071 && msgNumber <= 1127)
            {
                RinexEpochObservation RinexEpochObservation = null;
                RtcmMsmMessageConverter RtcmMsmMessageConverter = new Ntrip.RtcmMsmMessageConverter(ObsHeader);
                HeaderOfMSM headerOfMsm = HeaderOfMSM.Parse(data);
                HeaderOfMSM = headerOfMsm;
                BinStringSequence.DeQueue(headerOfMsm.Length);
                int msmtype = msgNumber % 10;
                int Nsat = headerOfMsm.SatCount;
                int Ncell = headerOfMsm.Ncell;
                switch (msmtype)
                {
                    case 1:
                        MSM1 MSM1 = MSM1.Parse(BinStringSequence.DeQueue(Nsat * 10 + Ncell * 15), Nsat, Ncell);
                        RinexEpochObservation = RtcmMsmMessageConverter.GetRinexEpochObservation(MSM1, headerOfMsm);
                        break;
                    case 2:
                        MSM2 MSM2 = MSM2.Parse(BinStringSequence.DeQueue(Nsat * 10 + Ncell * 27), Nsat, Ncell);
                        RinexEpochObservation = RtcmMsmMessageConverter.GetRinexEpochObservation(MSM2, headerOfMsm);
                        break;
                    case 3:
                        MSM3 MSM3 = MSM3.Parse(BinStringSequence.DeQueue(Nsat * 10 + Ncell * 42), Nsat, Ncell);
                        RinexEpochObservation = RtcmMsmMessageConverter.GetRinexEpochObservation(MSM3, headerOfMsm);
                        break;
                    case 4:
                        MSM4 MSM4 = MSM4.Parse(BinStringSequence.DeQueue(Nsat * 18 + Ncell * 48), Nsat, Ncell);
                        RinexEpochObservation = RtcmMsmMessageConverter.GetRinexEpochObservation(MSM4, headerOfMsm);
                        break;
                    case 5:
                        MSM5 MSM5 = MSM5.Parse(BinStringSequence.DeQueue(Nsat * 36 + Ncell * 63), Nsat, Ncell);
                        RinexEpochObservation = RtcmMsmMessageConverter.GetRinexEpochObservation(MSM5, headerOfMsm);
                        break;
                    case 6:
                        MSM6 MSM6 = MSM6.Parse(BinStringSequence.DeQueue(Nsat * 18 + Ncell * 65), Nsat, Ncell);
                        RinexEpochObservation = RtcmMsmMessageConverter.GetRinexEpochObservation(MSM6, headerOfMsm);
                        break;
                    case 7:
                        MSM7 MSM7 = MSM7.Parse(BinStringSequence.DeQueue(Nsat * 36 + Ncell * 80), Nsat, Ncell);
                        RinexEpochObservation = RtcmMsmMessageConverter.GetRinexEpochObservation(MSM7, headerOfMsm);
                        break;
                    default:
                        break;
                }
                RinexEpochObservation.Name = this.SiteName;
                RinexEpochObservation.Header = ObsHeader;
                OnEpochObservationReceived(RinexEpochObservation);
            }
            #endregion
        }

        #region 观测头部信息的创建与更新。
        /// <summary>
        /// 接收机与天线信息
        /// </summary>
        /// <param name="Msg1033"></param>
        private void UpdateOFileHeader(Message1033 Msg1033)
        {
            bool isChanged = false;

            if (ObsHeader.SiteInfo.ReceiverNumber != Msg1033.ReceiverSerialNumber) { ObsHeader.SiteInfo.ReceiverNumber = Msg1033.ReceiverSerialNumber; isChanged = true; }
            if (ObsHeader.SiteInfo.ReceiverType != Msg1033.ReceiverTypeDescriptor) { ObsHeader.SiteInfo.ReceiverType = Msg1033.ReceiverTypeDescriptor; isChanged = true; }
            if (ObsHeader.SiteInfo.ReceiverVersion != Msg1033.ReceiverFirmwareVersion) { ObsHeader.SiteInfo.ReceiverVersion = Msg1033.ReceiverFirmwareVersion; isChanged = true; }
            if (ObsHeader.SiteInfo.AntennaNumber != Msg1033.AntennaSerialNumber) { ObsHeader.SiteInfo.AntennaNumber = Msg1033.AntennaSerialNumber; isChanged = true; }// "Unknown";
            if (ObsHeader.SiteInfo.AntennaType != Msg1033.AntennaDescriptor) { ObsHeader.SiteInfo.AntennaType = Msg1033.AntennaDescriptor; isChanged = true; }//.AntennaSerialNumber;// "Unknown";
            //header.AntDeltaXyz = new Geo.Coordinates.XYZ();

            if (isChanged) { OnObsHeaderUpdated(ObsHeader); }
        }
        /// <summary>
        /// 更新测站坐标
        /// </summary>
        /// <param name="Msg1006"></param>
        private void UpdateOFileHeader(Message1006 Msg1006)
        {
            ObsHeader.Hen.H = Msg1006.AntennaHeight;
            UpdateOFileHeader((Message1005)Msg1006);
        }

        /// <summary>
        /// 更新测站坐标
        /// </summary>
        /// <param name="Msg1005"></param>
        private void UpdateOFileHeader(Message1005 Msg1005)
        {
            var xyz = new Geo.Coordinates.XYZ(Msg1005.AntennaReferencePointECEF_X, Msg1005.AntennaReferencePointECEF_Y, Msg1005.AntennaReferencePointECEF_Z);
            if (ObsHeader.ApproxXyz.Equals(xyz)) { return; }
            ObsHeader.ApproxXyz = xyz;
            OnObsHeaderUpdated(ObsHeader);
        }
        #endregion

    }
}