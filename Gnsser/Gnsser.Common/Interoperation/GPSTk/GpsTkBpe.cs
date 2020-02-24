using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using System.IO;//Stream
using System.Net;//FtpWebRequest
using System.Threading;
using Geo.Common;
using Gnsser.Interoperation.Bernese;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser.Interoperation.GPSTk
{
    ///// <summary>
    ///// PCF 其名称与PCF文件夹下的PCF对应。
    ///// 可以直接 PcfName.PCF 使用。
    ///// </summary>
    //public enum PcfName
    //{
    //    PPP, RNX2SNX, BASTST, CLKDET, SUPERBPE
    //}

    ///
    /// <summary>
    ///  GPSTk BerBpe.
    /// </summary>
    public class GpsTkBpe
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GpsTkBpe()
        {
        }
        /// <summary>
        /// Bernese BerBpe.
        /// </summary>
        /// <param name="campaign">本地的工程文件名，不是路径</param>
        /// <param name="gpsTime">观测日期，年月日，YYYYMMDD</param>
        /// <param name="skip">是否忽略</param>
        public GpsTkBpe(string campaign, Time gpsTime, bool skip = false)
        {
            Init(campaign, gpsTime, skip);

            
        }

        /// <summary>
        /// 初始化工作。
        /// </summary>
        /// <param name="campaign">本地的工程文件名，不是路径</param>
        /// <param name="gpsTime">观测日期，年月日，YYYYMMDD</param>
        /// <param name="skip">是否跳过一些步骤</param>
        /// <param name="startUpPath">程序运行目录</param> 
        public void Init(string campaign, Time gpsTime, bool skip = false,string startUpPath=@"./GEN/")
        {
            this.Skip = skip;
            //GpsTkBpe.MenuPath = Path.Combine(BerGpsUerPath, @"PAN\MENU.INP");
            //GpsTkBpe.RunBpePath = Path.Combine(BerGpsUerPath, @"PAN\RUNBPE.INP");
            //GpsTkBpe.RunBpeModelPath = Path.Combine(BerGpsUerPath, @"PAN\RUNBPEMODE.INP");
            //GpsTkBpe.MenuRunPcfPath = Path.Combine(BerGpsUerPath + @"PAN\MENU_RUNPCF.INP");

          //  this.campaignDir = SaveDataDir;

            //this.cmdline = BerPath + @"MENU\menu" + " " + MenuPath + " " + MenuRunPcfPath;

            this.campaign = campaign;
            this.gpsTime = gpsTime;
            this.startUpDir = startUpPath;

        }

        /// <summary>
        /// 是否跳过一些步骤。                                                                                                                                                                                                                                                                                     
        /// </summary>
        public bool Skip { get; set; }

        ProcessRunner cmd = new ProcessRunner();
        /// <summary>
        /// cmd
        /// </summary>
        public ProcessRunner WinCmd
        {
            get { return cmd; }
            set { cmd = value; }
        }

        private string campaign;//本地的工程文件名 
        /// <summary>
        /// 本地工程所在上级目录
        /// </summary>
        public static string campaignDir;//本地工程所在上级目录
        /// <summary>
        /// 执行程序目
        /// </summary>
        public string startUpDir;//执行程序目录 /ppp.exe   /GEN/...
        /// <summary>
        /// 结果文件
        /// </summary>
        public string resultFile;
        /// <summary>
        /// 工程
        /// </summary>
        public string Campaign
        {
            get { return campaign; }
            set { campaign = value; }
        }
        private Time gpsTime;
        /// <summary>
        /// time
        /// </summary>
        public Time GpsTime
        {
            get { return gpsTime; }
            set { gpsTime = value; }
        }

        //public static string BerGpsUerPath = @"C:\GPSUSER\";
        //public static string BerPath = @"C:\BERN50\";
        //public static string BerGpsDataPath = @"C:\GPSDATA\";

        ////运行BPE，至少需要修改下面3个INP文件
        //private static string MenuPath, RunBpePath, RunBpeModelPath, MenuRunPcfPath;

        private  string cmdline ;//= BerPath + @"MENU\menu" + " " + MenuPath + " " + MenuRunPcfPath;

        static string LINE_END = "\r\n";
        static string DOUBLE_QUOTE = "\"";
        static string SPACE2 = "  ";
        static string SPACE2_DOUBLE_QUOTE = SPACE2 +  DOUBLE_QUOTE;
        static string DOUBLE_QUOTE_LINE_END = DOUBLE_QUOTE + LINE_END;
        //SCRIPT_SKIP 4
        //  "511  ADDNEQ2   R2S_FIN"
        //  "512  GPSXTR    R2S_FIN"
        //  "513  COMPAR    R2S_FIN"
        //  "514  HELMR1    R2S_FIN"
        //  ## widget = selwin; pointer = PIDLIST
        //  # SELECTED
        static string SKIP_RNX = "SCRIPT_SKIP 4" + LINE_END
            + SPACE2_DOUBLE_QUOTE + "511  ADDNEQ2   R2S_FIN" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "512  GPSXTR    R2S_FIN" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "513  COMPAR    R2S_FIN" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "514  HELMR1    R2S_FIN" + DOUBLE_QUOTE_LINE_END
                         + SPACE2 + "## widget = selwin; pointer = PIDLIST" + LINE_END
                         + SPACE2 + "# SELECTED"
            ;
                
        //SCRIPT_SKIP 0
        //  ## widget = selwin; pointer = PIDLIST
        //  # 
        static string ZERO_SKIP = "SCRIPT_SKIP 0" + LINE_END
                         + SPACE2 + "## widget = selwin; pointer = PIDLIST" + LINE_END
                         + SPACE2 + "#";
        /// <summary>
        /// PPP skip
        /// </summary>
        static string SKIP_PPP = "SCRIPT_SKIP 17" + LINE_END
            + SPACE2_DOUBLE_QUOTE + "321  CRDMERGE  PPP_AUX" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "322  ADDNEQ2   PPP_AUX" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "331  CCRNXC    PPP_AUX" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "401  PPP_PLD   PPP_GEN" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "402  NUVELO    PPP_GEN" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "403  COOVEL    PPP_GEN" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "411  CRDMERGE  PPP_VEL" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "421  CRDMERGE  PPP_CRD" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "431  ADDNEQ2   PPP_GCC" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "501  PPP_ION   PPP_ION" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "502  PPPESTAP  PPP_ION" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "503  PPPEST_P  PPP_ION" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "504  GPSXTR    PPP_ION" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "511  ADDNEQ2   PPP_ION" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "521  GPSEST    PPP_RIM" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "522  GPSXTR    PPP_RIM" + DOUBLE_QUOTE_LINE_END
            + SPACE2_DOUBLE_QUOTE + "901  PPP_SUM   PPP_GEN" + DOUBLE_QUOTE_LINE_END
                         + SPACE2 + "## widget = selwin; pointer = PIDLIST" + LINE_END
                         + SPACE2 + "# SELECTED"
            ;

        /// <summary>
        /// 
        /// </summary>
        static string SERVER_VARIABLES_PPP_0 = "SERVER_VARIABLES_0 12" + "\r\n"
              + "  \"V_A\"  \"APR\"  \"A priori information\"" + "\r\n"
              + "  \"V_B\"  \"IGS\"  \"Orbit/ERP, DCB, CLK information\"" + "\r\n"
              + "  \"V_C\"  \"PPP\"  \"CRD/TRP/TRO/SNX and CLK results\"" + "\r\n"
              + "  \"V_E\"  \"REF\"  \"CRD results referred to epoch 2000.0\"" + "\r\n"
              + "  \"V_F\"  \"ION\"  \"Station-specific ION/DCB results\"" + "\r\n"
              + "  \"V_G\"  \"RIM\"  \"Regional ION/INX/DCB results\"" + "\r\n"
              + "  \"V_STAINF\"  \"&{Campagin}\"  \"Station information file oFileName\"" + "\r\n"
              + "  \"V_PLDINF\"  \"&{Campagin}\"  \"Tectonic plate definition file oFileName\"" + "\r\n"
              + "  \"V_BLQINF\"  \"\"  \"Ocean loading correction file oFileName\"" + "\r\n"
              + "  \"V_ABBINF\"  \"&{Campagin}\"  \"Station oFileName abbreviation file oFileName\"" + "\r\n"
              + "  \"V_CRDREF\"  \"IGS_00_R\"  \"Master/reference CRD/VEL file oFileName\"" + "\r\n"
              + "  \"V_CRDMRG\"  \"IGS_00\"  \"Merged CRD/VEL file oFileName\"";
        /// <summary>
        /// 
        /// </summary>
        static string SERVER_VARIABLES_RNX_0 = "SERVER_VARIABLES_0 8" + "\r\n"
            + "  \"V_A\"  \"APR\"  \"A priori information\"" + "\r\n"
            + "  \"V_B\"  \"IGS\"  \"Orbit/ERP, DCB, ION information\"" + "\r\n"
            + "  \"V_C\"  \"P1_\"  \"Preliminary (fraction-float) result\"" + "\r\n"
            + "  \"V_E\"  \"F1_\"  \"Final (fraction-fixed) results\"" + "\r\n"
            + "  \"V_F\"  \"R1_\"  \"Size-reduced NEQ information\"" + "\r\n"
            + "  \"V_MINUS\"  \"-6\"  \"Session range begin (for COMPAR)\"" + "\r\n"
            + "  \"V_PLUS\"  \"+0\"  \"Session range end\"" + "\r\n"
            + "  \"V_CLU\"  \"3\"  \"Maximum number of files per cluster\"";
        /// <summary>
        /// PPP
        /// </summary>
        static string SERVER_VARIABLES_PPP = "SERVER_VARIABLES 12" + "\r\n"
            + "  \"V_A\" \"APR\" \"A priori information\"" + "\r\n"
            + "  \"V_B\" \"IGS\" \"Orbit/ERP, DCB, CLK information\"" + "\r\n"
            + "  \"V_C\" \"PPP\" \"CRD/TRP/TRO/SNX and CLK results\"" + "\r\n"
            + "  \"V_E\" \"REF\" \"CRD results referred to epoch 2000.0\"" + "\r\n"
            + "  \"V_F\" \"ION\" \"Station-specific ION/DCB results\"" + "\r\n"
            + "  \"V_G\" \"RIM\" \"Regional ION/INX/DCB results\"" + "\r\n"
            + "  \"V_STAINF\" \"&{Campagin}\" \"Station information file oFileName\"" + "\r\n"
            + "  \"V_PLDINF\" \"&{Campagin}\" \"Tectonic plate definition file oFileName\"" + "\r\n"
            + "  \"V_BLQINF\" \"\" \"Ocean loading correction file oFileName\"" + "\r\n"
            + "  \"V_ABBINF\" \"&{Campagin}\" \"Station oFileName abbreviation file oFileName\"" + "\r\n"
            + "  \"V_CRDREF\" \"IGS_00_R\" \"Master/reference CRD/VEL file oFileName\"" + "\r\n"
            + "  \"V_CRDMRG\" \"IGS_00\" \"Merged CRD/VEL file oFileName\"";
        static string SERVER_VARIABLES_RNX = "SERVER_VARIABLES 8" + "\r\n"
            + "  \"V_A\" \"APR\" \"A priori information\"" + "\r\n"
            + "  \"V_B\" \"IGS\" \"Orbit/ERP, DCB, ION information\"" + "\r\n"
            + "  \"V_C\" \"P1_\" \"Preliminary (fraction-float) result\"" + "\r\n"
            + "  \"V_E\" \"F1_\" \"Final (fraction-fixed) results\"" + "\r\n"
            + "  \"V_F\" \"R1_\" \"Size-reduced NEQ information\"" + "\r\n"
            + "  \"V_MINUS\" \"-6\" \"Session range begin (for COMPAR)\"" + "\r\n"
            + "  \"V_PLUS\" \"+0\" \"Session range end\"" + "\r\n"
            + "  \"V_CLU\" \"3\" \"Maximum number of files per cluster\"";

  
        /// <summary>
        /// 同步运行
        /// </summary>
        /// <param name="pcfName"></param>
        /// <returns></returns>
        public string Run(string  pcfName)
        {
            //campaignDir = Path.Combine(campaignDir, campaign);
            SetBpeEnviroment(campaign, this.gpsTime, pcfName, Skip, Path.Combine(campaignDir, campaign));
           return cmd.Run(cmdline)[0];
        }
        /// <summary>
        /// 异步运行
        /// </summary>
        /// <param name="pcfName"></param>
        public void RunAsyn(string  pcfName)
        {
            SetBpeEnviroment(campaign, this.gpsTime, pcfName, Skip, Path.Combine(campaignDir, campaign));
            cmd.RunAsyn(cmdline);

        }
                 
        /// <summary>
        /// C:\GPSDATA\EXAMPLE\SOL\F1_021430.SNX
        /// C:\GPSDATA\EXAMPLE\SOL\PPP021430.SNX
        /// </summary>
        /// <returns></returns>
        public string GetSinexPath(string  pcfName)
        {
            if (pcfName == PcfName.RNX2SNX)
            { 
                //  return campaignDir +  "\\SOL\\F1_" + gpsTime.SubYear.ToString("00") + gpsTime.DayOfYear + "0.SNX";
                return resultFile;
            }
            if (pcfName == PcfName.PPP)
            {
               // return campaignDir + "\\SOL\\PPP" + gpsTime.SubYear.ToString("00") + gpsTime.DayOfYear + "0.SNX";
                return resultFile;
            }

            return "none";
        }

        /// <summary>
        /// 获取当前运行状态，running,finished,error,等等。
        /// </summary>
        /// <returns></returns>
        public string GetBernRunningState(string pcfName)
        {
            //string strSNXPath = campaignDir + "\\BPE\\" + pcfName.ToString().Substring(0, 3) + ".RUN";
            //if (File.Exists(strSNXPath))
            //    using (StreamReader reader = new StreamReader(strSNXPath))
            //    {
            //        string line = null;
            //        while ((line = reader.ReadLine()) != null)
            //        {
            //            if (line.Trim().StartsWith("Session"))
            //            {
            //                return line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            //            }
            //        }
            //    }
            return "none";
        }


        /// <summary>
        /// 设置并运行环境所需要的配置文件
        /// 虽没有返回，但给cmdline进行赋值。
        /// </summary>
        private void SetBpeEnviroment(string campaign, Time gpsTime, string  pcfName, bool skip,string camDir)
        {
            //调用PPP，Run PPP，首先根据工程信息修改环境设置文件
            //string julianDate = gpsTime.ModifiedJulianDay.ToString("0.000000");//约旦时 
            //ModifyMenuInp(campaign, julianDate);
            //ModifyRunbpeInp(campaign, gpsTime, pcfName, skip);
            //ModifyMenuRunPcfInp();

            //生成配置文件，并生成命令语句
            if (pcfName == PcfName.PPP)
            {
                CreatPPPConfigureFile(campaign, gpsTime, skip, camDir);
            }
            if (pcfName == PcfName.RNX2SNX)
            {
                CreatBaselineConfigureFile(campaign, gpsTime, skip, camDir);
            }
        }

        #region 配置文件参数预设
        string oceanLoadingFile = "OCEAN-GOT00.dat";//file with ocean loading parameters
        string satDataFile = "PRN_GPS";//constellation satData file
        int precision = 4;//number of decimal places in output file
        double cutOffElevation = 10.0;//minimum allowed satellite elevation ,in degrees
        bool useC1 = true;//if RINEX observation files don't include P1,  //需要根据数据类型判断 
        
        //and you want to use C1 instead.instrumental errors will NOT cancel,introducing a bias that must be taken into account by other means. This bias won't be corrected in this application.
        bool checkGaps = true;// check for satData gaps bigger than 901 fraction
        int SP3GapInterval = 901;//fraction
        bool checkInterval = true;//don't allow interpolation intervals bigger than 8105 s
        int maxSP3Interval = 8105;//fraction
        //Decimation parameters. Typical values for 15-minutes-per-sample SP3 files
        int decimationInterval = 30;//like 30, 900, interval between processed satData,fraction
        int decimationTolerance = 5;//tolerance allowed for decimation,fraction
       
        
        bool filterCode = false;//It turns out that some receivers don't correct their clocks from drift."SimpleFilter" objects start to reject a lot of satellites.
        bool filterPC = true;//the "filterPC" option allows you to deactivate the "SimpleFilter" object that filters out PC, in case you need to.
        bool printModel = false;//By default, we won't print the model.

        bool useAntex = true;//this variable tells if we'll use Antex files 
        bool usePCPatterns = true;//this variable activates use of Antex patterns  
        bool useAzim = true;// this variable activates use of azimuth-dependent patterns 
        string antexFile = "igs05.atx";
        int forwardBackwardCycles = 0;//an integer < 1 means forwards processing only
        bool coordinatesAsWhiteNoise = false;//Static positioning
        bool USENEU = false;//results will be given in dLat, dLon, dH

        double eop1 = 0.075310;
        double eop2 = 0.289856;
        #endregion


        /// <summary>
        /// 建立PPP的配置文件，每个PPP任务应该是单站的，但这里读取文件夹下所有的文件，理论上应该是只有一个的。
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="gpsTime"></param> 
        /// <param name="skip"></param>
        /// <param name="camDir">工程目录</param>
        public void CreatPPPConfigureFile(string campaign, Time gpsTime, bool skip, string camDir)
        {
            //遍序文件，读取观测文件信息
            DirectoryInfo DInfo = new DirectoryInfo(camDir+"\\Orx\\");
            FileInfo[] FSInfo = DInfo.GetFiles();
            Dictionary<string, string> pathes = new Dictionary<string, string>();
            string msg = "";
            if (FSInfo.Length != 1)
            {
                throw new Exception("每个PPP任务仅对应一个测站！否则，要不建立共一的配置文件，返回一个路径，要不逐个建立，不返回路径，直接运行。");
            }
            for (int i = 0; i < FSInfo.Length; i++)
            {
                string p =camDir+"\\Orx\\"+ FSInfo[i].ToString();
                string fileName = Path.GetFileName(p);
                if (!pathes.ContainsKey(fileName)) pathes.Add(fileName, p);
                else msg += fileName + "," + "\r\n";
            }
             
            if (msg != "")
            {
                throw new Exception(msg);
            }

            //应该不用循环，PPP任务仅是包含一个站
            foreach (KeyValuePair<string, string> kv in pathes)
            {
                if ( (kv.Key) == null) continue;

                var obsFile = (new Gnsser.Data.Rinex.RinexObsFileReader(kv.Value).ReadObsFile());

                //文件的物理路径所在文件夹路径
                string paths = kv.Value;
                string tmp = paths.Substring(0, paths.LastIndexOf("\\"));
                string orfolder = paths.Substring(0, paths.LastIndexOf("\\") + 1);
                string folder = orfolder.Replace("\\", "\\\\");

                //Cui Yang 
                #region 逐站生成配置文件，运行
                //string strConfName = obsFile.Header.MarkerName.ToLower() + "-pppconf.txt";
                
                string strConfName =kv.Key.Substring(0,4).ToLower() + "-pppconf.txt";
                string strConfPath = orfolder + strConfName;//E:\\COMC\\Orx\\ahbb-pppconf.txt
                StreamWriter sw = File.CreateText(strConfPath);
                if (obsFile.StartTime != null) //判断是否
                {
                    sw.WriteLine("oceanLoadingFile   =  " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\" + oceanLoadingFile);//OCEAN-GOT00.dat
                    sw.WriteLine("satDataFile        =  " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\" + satDataFile);// PRN_GPS
                    sw.WriteLine("precision          = " + precision);//4
                    sw.WriteLine("cutOffElevation    = " + cutOffElevation);//4
                    sw.WriteLine();
                    //需要根据数据类型判断 
                    sw.WriteLine("useC1              = " + useC1);//默认没有P1观测值，则采用C1代替P1
                    sw.WriteLine();

                    sw.WriteLine("checkGaps          = " + checkGaps); 
                    sw.WriteLine("SP3GapInterval     = "+SP3GapInterval);//901
                    sw.WriteLine();
                    sw.WriteLine("checkInterval      = " + checkInterval);//.ToString().ToLower()
                    sw.WriteLine("maxSP3Interval     = " + maxSP3Interval);//8105
                    sw.WriteLine();
                    sw.WriteLine("decimationInterval = " + decimationInterval);//900
                    sw.WriteLine("decimationTolerance= " + decimationTolerance);//5
                    sw.WriteLine();
                    sw.WriteLine("filterCode         = " + filterCode);//FALSE
                    sw.WriteLine();
                    sw.WriteLine("filterPC           = " + filterPC);//true
                    sw.WriteLine();
                    sw.WriteLine("printModel         = " + printModel);//FALSE
                    sw.WriteLine();
                    sw.WriteLine("useAntex           = " + useAntex);//true
                    sw.WriteLine("usePCPatterns      = " + usePCPatterns);//true
                    sw.WriteLine("useAzim            = " + useAzim);//true
                    sw.WriteLine();

                    //
                   // sw.WriteLine("[" + obsFile.Header.MarkerName.ToUpper() + "]");//测站名称
                    sw.WriteLine("[" + kv.Key.Substring(0, 4).ToUpper() + "]");//测站名称
                    sw.WriteLine();
                    sw.WriteLine("rinexObsFile       = " + kv.Value.Replace("\\", "\\\\"));//观测文件物理路径
                    sw.WriteLine();
                    sw.WriteLine("dayOfYear          = " + obsFile.StartTime.DayOfYear.ToString());// + ", days of year for " + obsFile.FirstObsTime.Month.ToString() + "/" + obsFile.FirstObsTime.Day.ToString() + "/" + obsFile.FirstObsTime.Year.ToString()
                    sw.WriteLine();
                    string sp31; string sp32; string sp33;

                    if (obsFile.StartTime.DayOfWeek == DayOfWeek.Monday) // 周1
                    {
                        sp31 = "igs" + obsFile.StartTime.GpsWeek.ToString() + (obsFile.StartTime.DayOfWeek - 1).ToString() + ".sp3";
                        sp32 = "igs" + obsFile.StartTime.GpsWeek.ToString() + (obsFile.StartTime.DayOfWeek).ToString() + ".sp3";
                        sp33 = "igs" + obsFile.StartTime.GpsWeek.ToString() + (obsFile.StartTime.DayOfWeek + 1).ToString() + ".sp3";
                    }
                    else
                    {
                        sp31 = "igs" + obsFile.StartTime.GpsWeek.ToString() + (obsFile.StartTime.DayOfWeek - 1).ToString() + ".sp3";
                        sp32 = "igs" + obsFile.StartTime.GpsWeek.ToString() + (obsFile.StartTime.DayOfWeek).ToString() + ".sp3";
                        sp33 = "igs" + obsFile.StartTime.GpsWeek.ToString() + (obsFile.StartTime.DayOfWeek + 1).ToString() + ".sp3";
                    }
                    if (obsFile.StartTime.DayOfWeek == 0) //周日，每周的第一天
                    {
                        sp31 = "igs" + (obsFile.StartTime.GpsWeek - 1).ToString() + (obsFile.StartTime.DayOfWeek + 6).ToString() + ".sp3";
                        sp32 = "igs" + obsFile.StartTime.GpsWeek.ToString() + (obsFile.StartTime.DayOfWeek).ToString() + ".sp3";
                        sp33 = "igs" + obsFile.StartTime.GpsWeek.ToString() + (obsFile.StartTime.DayOfWeek + 1).ToString() + ".sp3";
                    }
                    if (obsFile.StartTime.DayOfWeek ==  DayOfWeek.Saturday) //周6
                    {
                        sp31 = "igs" + obsFile.StartTime.GpsWeek.ToString() + (obsFile.StartTime.DayOfWeek - 1).ToString() + ".sp3";
                        sp32 = "igs" + obsFile.StartTime.GpsWeek.ToString() + (obsFile.StartTime.DayOfWeek).ToString() + ".sp3";
                        sp33 = "igs" + (obsFile.StartTime.GpsWeek + 1).ToString() + (obsFile.StartTime.DayOfWeek - 6).ToString() + ".sp3";
                    }
                    //精密星历等信息存在工程目录下的Gen文件夹下。, SP3 ephemeris file colName =
                    sw.WriteLine("SP3List = " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\" + sp31 + " " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\" + sp32 + " " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\" + sp33);
                    sw.WriteLine();
                    //这里需要手动输入，下步需要自动读取输入！！！！！
                    sw.WriteLine("poleDisplacements         = " +eop1.ToString()+"  "+eop2.ToString() + ", arcsec");//应该自动读取s, for " + obsFile.FirstObsTime.Month.ToString() + "/" + obsFile.FirstObsTime.Day.ToString() + "/" + obsFile.FirstObsTime.Year.ToString()
                    sw.WriteLine();
  
                   
                    sw.WriteLine("nominalPosition, ECEF-IGS = " + obsFile.Header.ApproxXyz.X.ToString() + " " + obsFile.Header.ApproxXyz.Y.ToString() + " " + obsFile.Header.ApproxXyz.Z.ToString() + ", meters");
                    sw.WriteLine();

                    sw.WriteLine("# Antenna parameters");
                    //sw.WriteLine("useAntex          = " + "TRUE" + "                  # We will use Antex files");
                    //sw.WriteLine();
                    sw.WriteLine("antexFile                = " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\"+antexFile); //"                # Name of Antex file (absolute PC)"

                    string AntennaType = obsFile.Header.SiteInfo.AntennaType;
                    switch (AntennaType)
                    {
                        case "NONE":
                            sw.WriteLine("antennaModel = " + "TRM59800.00     SCIS");
                            break;
                        case "ASH700936D_M    DOME":
                            sw.WriteLine("antennaModel = " + "ASH700936D_M    NONE");
                            break;
                        case "ASH700936D_M    JPLA":
                            sw.WriteLine("antennaModel = " + "ASH700936D_M    NONE");
                            break;
                        case "ASH701945B_M    JPLA":
                            sw.WriteLine("antennaModel = " + "ASH701945B_M    NONE");
                            break;
                        case "ASH701945B_M    ENCL":
                            sw.WriteLine("antennaModel = " + "ASH701945B_M    NONE");
                            break;
                        case "ASH701945G_M    JPLA":
                            sw.WriteLine("antennaModel = " + "ASH701945G_M    NONE");
                            break;
                        case "JPSREGANT_SD_E1 NONE":
                            sw.WriteLine("antennaModel = " + "JPSREGANT_SD_E  NONE");
                            break;
                        case "JPSREGANT_DD_E2 NONE":
                            sw.WriteLine("antennaModel = " + "JPSREGANT_SD_E  NONE");
                            break;
                        case "ASH700936E      SCIS":
                            sw.WriteLine("antennaModel = " + "ASH700936E      NONE");
                            break;                     
                        case "ASH700936F_C    SNOW":
                            sw.WriteLine("antennaModel = " + "ASH700936F_C    NONE");
                            break;
                        case "TRM29659.00     DOME":
                            sw.WriteLine("antennaModel = " + "TRM29659.00     NONE");
                            break;
                        case "TRM57971.00     TZGD":
                            sw.WriteLine("antennaModel = " + "TRM57971.00     NONE");
                            break;
                        case "TRM33429.00+GP  DOME":
                            sw.WriteLine("antennaModel = " + "TRM33429.00+GP  NONE");
                            break;
                        case "SEPCHOKE_MC     NONE":
                            sw.WriteLine("antennaModel = " + "TRM33429.00+GP  NONE");
                            break;
                        case "ASH701933B_M    SCIS":
                            sw.WriteLine("antennaModel = " + "ASH701933B_M    NONE");
                            break;
                        case "TRM41249.00     DOME":
                            sw.WriteLine("antennaModel = " + "TRM41249.00     NONE");
                            break;
                        case "ASH701941.B     SNOW":
                            sw.WriteLine("antennaModel = " + "ASH701941.B     NONE");
                            break;
                        case "ASH701073.1     SNOW":
                            sw.WriteLine("antennaModel = " + "ASH701073.1     NONE");
                            break;
                        case "ASH701073.1     SCIS":
                            sw.WriteLine("antennaModel = " + "ASH701073.1     NONE");
                            break;
                        case "AOAD/M_T        DOME":
                            sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                            break;
                        case "AOAD/M_T        JPLA":
                            sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                            break;
                        case "AOAD/M_T        EMRA":
                            sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                            break;
                        case "AOAD/M_T        OSOD":
                            sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                            break;
                        case "AOAD/M_T        SCIS":
                            sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                            break;
                        case "AOAD/M_T        DUTD":
                            sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                            break;
                        case "AOAD/M_T        AUST":
                            sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                            break;
                        case "AOAD/M_B        DOME":
                            sw.WriteLine("antennaModel = " + "AOAD/M_B        NONE");
                            break;
                        case "AOAD/M_B        OSOD":
                            sw.WriteLine("antennaModel = " + "AOAD/M_B        NONE");
                            break;
                        case "ASH701933B_M    DOME":
                            sw.WriteLine("antennaModel = " + "ASH701933B_M    NONE");
                            break;
                        case "TRM59800.00     ENCL":
                            sw.WriteLine("antennaModel = " + "TRM59800.00     NONE");
                            break;
                        case "TPSCR.G3        SCPL":
                            sw.WriteLine("antennaModel = " + "TPSCR.G3        NONE");
                            break;
                        case "JPSREGANT_DD_E1 NONE":
                            sw.WriteLine("antennaModel = " + "JPSREGANT_DD_E  NONE");
                            break;
                        default:
                            sw.WriteLine("antennaModel         = " + obsFile.Header.SiteInfo.AntennaType);
                            break;

                    }
                    //if (obsFile.Header.AntennaType == "NONE")
                    //{
                    //    sw.WriteLine("antennaModel = " + "TRM59800.00     SCIS");
                    //    sw.WriteLine();
                    //}
                    
                    //else
                    //{
                    //    sw.WriteLine("antennaModel         = " + obsFile.Header.AntennaType);
                    //    sw.WriteLine();
                    //}
                    sw.WriteLine();
                    Geo.Coordinates.HEN hen = obsFile.Header.Hen;
                    sw.WriteLine("offsetARP, dH dLon dLat (UEN) = " + hen.H.ToString() + " " + hen.E.ToString() + " " + hen.N.ToString() + ", meters");
                    sw.WriteLine();
                    sw.WriteLine("forwardBackwardCycles     = " + forwardBackwardCycles );//0 + ", an integer < 1 means forwards processing only"
                    sw.WriteLine();
                    sw.WriteLine("coordinatesAsWhiteNoise   = " + coordinatesAsWhiteNoise );//FALSE + "           # Static positioning"
                    sw.WriteLine();
                    //sw.WriteLine("# The following variable, if true, sets a NEU system. If false, an XYZ system");
                    sw.WriteLine("USENEU                    = " + USENEU );//+ ", results will be given in dLat, dLon, dH"
                    sw.WriteLine();
                    sw.WriteLine("# Output");
                    sw.WriteLine("outputFile                = " + camDir.Replace("\\", "\\\\") + "\\\\Out\\\\" + kv.Key + ".out");

                    sw.WriteLine("finaloutputFile           = " + camDir.Replace("\\", "\\\\") + "\\\\Out\\\\" + "final" + kv.Key + ".out");
                    this.resultFile = camDir.Replace("\\", "\\\\") + "\\\\Out\\\\" + "final" + kv.Key + ".out";//结果文件
                    sw.WriteLine();
                    sw.WriteLine("printModel                = " + printModel);
                    if (printModel == true)
                    {
                        sw.WriteLine("modelFile             = " + camDir.Replace("\\", "\\\\") + "\\\\Out\\\\" + kv.Key + ".model");
                    }
                    sw.WriteLine();
                }
                sw.Close();

                Geo.Common.ProcessRunner cmd = new Geo.Common.ProcessRunner();
                  
                string PppPath = "\"" + startUpDir + "\\ppp.exe" + "\"";
                string param3 = " \"" + "--conffile" + "\"" + " \"" + strConfPath + "\""; //

               // string result = cmd.Run(PppPath + param3);//同步运行
              //  string result  cmd.RunAsyn(PppPath + param3);//异步运行
                this.cmdline = PppPath + param3;
                #endregion
            }
        }

        public void CreatBaselineConfigureFile(string campaign, Time gpsTime, bool skip, string camDir)
        {
           


            //遍序文件，读取观测文件信息
            DirectoryInfo DInfo = new DirectoryInfo(camDir + "\\Orx\\");
            FileInfo[] FSInfo = DInfo.GetFiles();
            Dictionary<string, string> pathes = new Dictionary<string, string>();
            string msg = "";
            if (FSInfo.Length != 2)
            {
                throw new Exception("每个单基线任务仅包含两个站！否则，要不建立共一的配置文件，返回一个路径，要不逐个建立，不返回路径，直接运行。");
            }
            string refStationPath = camDir + "\\Orx\\" + FSInfo[0].ToString();//基准站
            string rovStationPath = camDir + "\\Orx\\" + FSInfo[1].ToString();//流动站
            string refStation = Path.GetFileName(refStationPath);
            string rovStation = Path.GetFileName(rovStationPath);

            for (int i = 0; i < FSInfo.Length; i++)
            {
                string p = camDir + "\\Orx\\" + FSInfo[i].ToString();
                string fileName = Path.GetFileName(p);
                if (!pathes.ContainsKey(fileName)) pathes.Add(fileName, p);
                else msg += fileName + "," + "\r\n";
            }

            if (msg != "")
            {
                throw new Exception(msg);
            }
            //单基线任务仅包含两个站，规定读取的第一个测站为参考站，第二个测站为流动站。
            //计算输出的是两个的坐标差。
            var resObsFile =  new  Data.Rinex.RinexObsFileReader(refStationPath).ReadObsFile();
            var rovObsFile =  new Data.Rinex.RinexObsFileReader(rovStationPath).ReadObsFile();
            string pfolder = refStationPath.Substring(0, refStationPath.LastIndexOf("\\") + 1);
            string baseline = refStation.Substring(0, 2).ToUpper() + rovStation.Substring(0, 2).ToUpper();
            string strConfName = baseline + "-baselineconf.txt";
           // string strConfName = "baselineconf.txt";//命名受限于GPSTk基线读取文件没有修改。
            string strConfPath = pfolder + strConfName;//E:\\COMC\\Orx\\ahbb-pppconf.txt

            #region 查找PPP计算结果
            //
            //string startUpPath = Application.StartupPath;//可执行程序目录
            string pppPath = startUpDir + "\\pppResult.txt";
            StreamReader sr = new StreamReader(pppPath);
            string refS = refStation.Substring(0, 4).ToUpper();
            string rovS = rovStation.Substring(0, 4).ToUpper();
            double[] refSxyz  = new double[3];
            double[] rovSxyz = new double[3];
            string read = sr.ReadLine();
            while (read != null)
            {
                string[] cha = read.Split(' ');
                if (cha[0].ToUpper() == refS)
                { 
                    refSxyz[0] = Convert.ToDouble(cha[1]); refSxyz[1] = Convert.ToDouble(cha[2]); refSxyz[2] = Convert.ToDouble(cha[3]);
                }
                if (cha[0].ToUpper() == rovS)
                {
                    
                    rovSxyz[0] = Convert.ToDouble(cha[1]); rovSxyz[1] = Convert.ToDouble(cha[2]); rovSxyz[2] = Convert.ToDouble(cha[3]);
                }
                read = sr.ReadLine();
            }
            #endregion

            StreamWriter sw = File.CreateText(strConfPath);
            //Cui Yang 
            #region 逐站生成配置文件，运行
            //string strConfName = obsFile.Header.MarkerName.ToLower() + "-pppconf.txt";


            if (resObsFile.StartTime != null && rovObsFile.StartTime != null) //判断是否
            {
                sw.WriteLine("oceanLoadingFile   =  " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\" + oceanLoadingFile);//OCEAN-GOT00.dat
                sw.WriteLine("satDataFile        =  " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\" + satDataFile);// PRN_GPS
                sw.WriteLine("precision          = " + precision);//4
                sw.WriteLine("cutOffElevation    = " + cutOffElevation);//4
                sw.WriteLine();
                //需要根据数据类型判断 
                useC1 = false;
                sw.WriteLine("useC1              = " + useC1);//默认没有P1观测值，则采用C1代替P1
                sw.WriteLine();

                sw.WriteLine("checkGaps          = " + checkGaps);
                sw.WriteLine("SP3GapInterval     = " + SP3GapInterval);//901
                sw.WriteLine();
                sw.WriteLine("checkInterval      = " + checkInterval);//.ToString().ToLower()
                sw.WriteLine("maxSP3Interval     = " + maxSP3Interval);//8105
                sw.WriteLine();
                sw.WriteLine("decimationInterval = " + decimationInterval);//900
                sw.WriteLine("decimationTolerance= " + decimationTolerance);//5
                sw.WriteLine();
                sw.WriteLine("filterCode         = " + filterCode);//FALSE
                sw.WriteLine();
                sw.WriteLine("filterPC           = " + filterPC);//true
                sw.WriteLine();
                sw.WriteLine("printModel         = " + printModel);//FALSE
                sw.WriteLine();
                useAntex = true;
                sw.WriteLine("useAntex           = " + useAntex);//true
                sw.WriteLine("usePCPatterns      = " + usePCPatterns);//true
                sw.WriteLine("useAzim            = " + useAzim);//true
                sw.WriteLine();

                //
                // sw.WriteLine("[" + obsFile.Header.MarkerName.ToUpper() + "]");//测站名称
                sw.WriteLine("[" + baseline + "]");//测站名称
                sw.WriteLine();
                sw.WriteLine("refStation = " + refStation.Substring(0, 4).ToUpper());
                sw.WriteLine("rovStation = " + rovStation.Substring(0, 4).ToUpper());

                sw.WriteLine("refRinexObsFile       = " +refStationPath.Replace("\\", "\\\\"));//观测文件物理路径
                sw.WriteLine("rinexObsFile       = " + rovStationPath.Replace("\\", "\\\\"));//观测文件物理路径
                sw.WriteLine();
                sw.WriteLine("dayOfYear          = " + resObsFile.StartTime.DayOfYear.ToString());// + ", days of year for " + obsFile.FirstObsTime.Month.ToString() + "/" + obsFile.FirstObsTime.Day.ToString() + "/" + obsFile.FirstObsTime.Year.ToString()
                sw.WriteLine();
                string sp31; string sp32; string sp33;

                if (resObsFile.StartTime.DayOfWeek ==  DayOfWeek.Monday) // 周1
                {
                    sp31 = "igs" + resObsFile.StartTime.GpsWeek.ToString() + (resObsFile.StartTime.DayOfWeek - 1).ToString() + ".sp3";
                    sp32 = "igs" + resObsFile.StartTime.GpsWeek.ToString() + (resObsFile.StartTime.DayOfWeek).ToString() + ".sp3";
                    sp33 = "igs" + resObsFile.StartTime.GpsWeek.ToString() + (resObsFile.StartTime.DayOfWeek + 1).ToString() + ".sp3";
                }
                else
                {
                    sp31 = "igs" + resObsFile.StartTime.GpsWeek.ToString() + (resObsFile.StartTime.DayOfWeek - 1).ToString() + ".sp3";
                    sp32 = "igs" + resObsFile.StartTime.GpsWeek.ToString() + (resObsFile.StartTime.DayOfWeek).ToString() + ".sp3";
                    sp33 = "igs" + resObsFile.StartTime.GpsWeek.ToString() + (resObsFile.StartTime.DayOfWeek + 1).ToString() + ".sp3";
                }
                if (resObsFile.StartTime.DayOfWeek == 0) //周日，每周的第一天
                {
                    sp31 = "igs" + (resObsFile.StartTime.GpsWeek - 1).ToString() + (resObsFile.StartTime.DayOfWeek + 6).ToString() + ".sp3";
                    sp32 = "igs" + resObsFile.StartTime.GpsWeek.ToString() + (resObsFile.StartTime.DayOfWeek).ToString() + ".sp3";
                    sp33 = "igs" + resObsFile.StartTime.GpsWeek.ToString() + (resObsFile.StartTime.DayOfWeek + 1).ToString() + ".sp3";
                }
                if (resObsFile.StartTime.DayOfWeek ==  DayOfWeek.Saturday) //周日
                {
                    sp31 = "igs" + resObsFile.StartTime.GpsWeek.ToString() + (resObsFile.StartTime.DayOfWeek - 1).ToString() + ".sp3";
                    sp32 = "igs" + resObsFile.StartTime.GpsWeek.ToString() + (resObsFile.StartTime.DayOfWeek).ToString() + ".sp3";
                    sp33 = "igs" + (resObsFile.StartTime.GpsWeek + 1).ToString() + (resObsFile.StartTime.DayOfWeek - 6).ToString() + ".sp3";
                }
                //精密星历等信息存在工程目录下的Gen文件夹下。, SP3 ephemeris file colName =
                sw.WriteLine("SP3List = " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\" + sp31 + " " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\" + sp32 + " " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\" + sp33);
                sw.WriteLine();
                //这里需要手动输入，下步需要自动读取输入！！！！！
                sw.WriteLine("poleDisplacements         = " + eop1.ToString() + "  " + eop2.ToString() + ", arcsec");//应该自动读取s, for " + obsFile.FirstObsTime.Month.ToString() + "/" + obsFile.FirstObsTime.Day.ToString() + "/" + obsFile.FirstObsTime.Year.ToString()
                sw.WriteLine();

                if (refSxyz[0] == refSxyz[1] && refSxyz[1] == refSxyz[2] && refSxyz[2] == 0)
                {
                    sw.WriteLine("refNominalPosition, ECEF-IGS = " + resObsFile.Header.ApproxXyz.X.ToString() + " " + resObsFile.Header.ApproxXyz.Y.ToString() + " " + resObsFile.Header.ApproxXyz.Z.ToString() + ", meters");
                }
                else
                {
                   sw.WriteLine("refNominalPosition, ECEF-IGS = " + refSxyz[0].ToString() + " " + refSxyz[1].ToString() + " " + refSxyz[2].ToString() + ", meters"); 
                }
                if (rovSxyz[0] == rovSxyz[1] && rovSxyz[1] == rovSxyz[2] && rovSxyz[2] == 0)
                {
                    sw.WriteLine("nominalPosition, ECEF-IGS = " + rovObsFile.Header.ApproxXyz.X.ToString() + " " + rovObsFile.Header.ApproxXyz.Y.ToString() + " " + rovObsFile.Header.ApproxXyz.Z.ToString() + ", meters");
                }
                else
                {
                    sw.WriteLine("nominalPosition, ECEF-IGS = " + rovSxyz[0].ToString() + " " + rovSxyz[1].ToString() + " " + rovSxyz[2].ToString() + ", meters");
                }
                
                sw.WriteLine();

                sw.WriteLine("# Antenna parameters");
                //sw.WriteLine("useAntex          = " + "TRUE" + "                  # We will use Antex files");
                //sw.WriteLine();
                sw.WriteLine("antexFile                = " + camDir.Replace("\\", "\\\\") + "\\\\Gen\\\\" + antexFile); //"                # Name of Antex file (absolute PC)"


                string refAntennaType = resObsFile.Header.SiteInfo.AntennaType;
                string rovAntennaType = rovObsFile.Header.SiteInfo.AntennaType;
                switch (refAntennaType)
                {
                    case "NONE":
                        sw.WriteLine("refAntennaModel = " + "TRM59800.00     SCIS");
                        break;
                    case "ASH700936D_M    DOME":
                        sw.WriteLine("refAntennaModel = " + "ASH700936D_M    NONE");
                        break;
                    case "ASH700936D_M    JPLA":
                        sw.WriteLine("refAntennaModel = " + "ASH700936D_M    NONE");
                        break;
                    case "ASH701945B_M    JPLA":
                        sw.WriteLine("refAntennaModel = " + "ASH701945B_M    NONE");
                        break;
                    case "ASH701945B_M    ENCL":
                        sw.WriteLine("refAntennaModel = " + "ASH701945B_M    NONE");
                        break;
                    case "ASH701945G_M    JPLA":
                        sw.WriteLine("refAntennaModel = " + "ASH701945G_M    NONE");
                        break;
                    case "JPSREGANT_SD_E1 NONE":
                        sw.WriteLine("refAntennaModel = " + "JPSREGANT_SD_E  NONE");
                        break;
                    case "JPSREGANT_DD_E2 NONE":
                        sw.WriteLine("refAntennaModel = " + "JPSREGANT_SD_E  NONE");
                        break;
                    case "ASH700936E      SCIS":
                        sw.WriteLine("refAntennaModel = " + "ASH700936E      NONE");
                        break;
                    case "ASH700936F_C    SNOW":
                        sw.WriteLine("refAntennaModel = " + "ASH700936F_C    NONE");
                        break;
                    case "TRM29659.00     DOME":
                        sw.WriteLine("refAntennaModel = " + "TRM29659.00     NONE");
                        break;
                    case "TRM57971.00     TZGD":
                        sw.WriteLine("refAntennaModel = " + "TRM57971.00     NONE");
                        break;
                    case "TRM33429.00+GP  DOME":
                        sw.WriteLine("refAntennaModel = " + "TRM33429.00+GP  NONE");
                        break;
                    case "SEPCHOKE_MC     NONE":
                        sw.WriteLine("refAntennaModel = " + "TRM33429.00+GP  NONE");
                        break;
                    case "ASH701933B_M    SCIS":
                        sw.WriteLine("refAntennaModel = " + "ASH701933B_M    NONE");
                        break;
                    case "TRM41249.00     DOME":
                        sw.WriteLine("refAntennaModel = " + "TRM41249.00     NONE");
                        break;
                    case "ASH701941.B     SNOW":
                        sw.WriteLine("refAntennaModel = " + "ASH701941.B     NONE");
                        break;
                    case "ASH701073.1     SNOW":
                        sw.WriteLine("refAntennaModel = " + "ASH701073.1     NONE");
                        break;
                    case "ASH701073.1     SCIS":
                        sw.WriteLine("refAntennaModel = " + "ASH701073.1     NONE");
                        break;
                    case "AOAD/M_T        DOME":
                        sw.WriteLine("refAntennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        JPLA":
                        sw.WriteLine("refAntennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        EMRA":
                        sw.WriteLine("refAntennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        OSOD":
                        sw.WriteLine("refAntennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        SCIS":
                        sw.WriteLine("refAntennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        DUTD":
                        sw.WriteLine("refAntennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        AUST":
                        sw.WriteLine("refAntennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_B        DOME":
                        sw.WriteLine("refAntennaModel = " + "AOAD/M_B        NONE");
                        break;
                    case "AOAD/M_B        OSOD":
                        sw.WriteLine("refAntennaModel = " + "AOAD/M_B        NONE");
                        break;
                    case "ASH701933B_M    DOME":
                        sw.WriteLine("refAntennaModel = " + "ASH701933B_M    NONE");
                        break;
                    case "TRM59800.00     ENCL":
                        sw.WriteLine("refAntennaModel = " + "TRM59800.00     NONE");
                        break;
                    case "TPSCR.G3        SCPL":
                        sw.WriteLine("refAntennaModel = " + "TPSCR.G3        NONE");
                        break;
                    case "JPSREGANT_DD_E1 NONE":
                        sw.WriteLine("refAntennaModel = " + "JPSREGANT_DD_E  NONE");
                        break;
                    default:
                        sw.WriteLine("refAntennaModel         = " + refAntennaType);
                        break;

                }
                switch (rovAntennaType)
                {
                    case "NONE":
                        sw.WriteLine("antennaModel = " + "TRM59800.00     SCIS");
                        break;
                    case "ASH700936D_M    DOME":
                        sw.WriteLine("antennaModel = " + "ASH700936D_M    NONE");
                        break;
                    case "ASH700936D_M    JPLA":
                        sw.WriteLine("antennaModel = " + "ASH700936D_M    NONE");
                        break;
                    case "ASH701945B_M    JPLA":
                        sw.WriteLine("antennaModel = " + "ASH701945B_M    NONE");
                        break;
                    case "ASH701945B_M    ENCL":
                        sw.WriteLine("antennaModel = " + "ASH701945B_M    NONE");
                        break;
                    case "ASH701945G_M    JPLA":
                        sw.WriteLine("antennaModel = " + "ASH701945G_M    NONE");
                        break;
                    case "JPSREGANT_SD_E1 NONE":
                        sw.WriteLine("antennaModel = " + "JPSREGANT_SD_E  NONE");
                        break;
                    case "JPSREGANT_DD_E2 NONE":
                        sw.WriteLine("antennaModel = " + "JPSREGANT_SD_E  NONE");
                        break;
                    case "ASH700936E      SCIS":
                        sw.WriteLine("antennaModel = " + "ASH700936E      NONE");
                        break;
                    case "ASH700936F_C    SNOW":
                        sw.WriteLine("antennaModel = " + "ASH700936F_C    NONE");
                        break;
                    case "TRM29659.00     DOME":
                        sw.WriteLine("antennaModel = " + "TRM29659.00     NONE");
                        break;
                    case "TRM57971.00     TZGD":
                        sw.WriteLine("antennaModel = " + "TRM57971.00     NONE");
                        break;
                    case "TRM33429.00+GP  DOME":
                        sw.WriteLine("antennaModel = " + "TRM33429.00+GP  NONE");
                        break;
                    case "SEPCHOKE_MC     NONE":
                        sw.WriteLine("antennaModel = " + "TRM33429.00+GP  NONE");
                        break;
                    case "ASH701933B_M    SCIS":
                        sw.WriteLine("antennaModel = " + "ASH701933B_M    NONE");
                        break;
                    case "TRM41249.00     DOME":
                        sw.WriteLine("antennaModel = " + "TRM41249.00     NONE");
                        break;
                    case "ASH701941.B     SNOW":
                        sw.WriteLine("antennaModel = " + "ASH701941.B     NONE");
                        break;
                    case "ASH701073.1     SNOW":
                        sw.WriteLine("antennaModel = " + "ASH701073.1     NONE");
                        break;
                    case "ASH701073.1     SCIS":
                        sw.WriteLine("antennaModel = " + "ASH701073.1     NONE");
                        break;
                    case "AOAD/M_T        DOME":
                        sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        JPLA":
                        sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        EMRA":
                        sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        OSOD":
                        sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        SCIS":
                        sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        DUTD":
                        sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_T        AUST":
                        sw.WriteLine("antennaModel = " + "AOAD/M_T        NONE");
                        break;
                    case "AOAD/M_B        DOME":
                        sw.WriteLine("antennaModel = " + "AOAD/M_B        NONE");
                        break;
                    case "AOAD/M_B        OSOD":
                        sw.WriteLine("antennaModel = " + "AOAD/M_B        NONE");
                        break;
                    case "ASH701933B_M    DOME":
                        sw.WriteLine("antennaModel = " + "ASH701933B_M    NONE");
                        break;
                    case "TRM59800.00     ENCL":
                        sw.WriteLine("antennaModel = " + "TRM59800.00     NONE");
                        break;
                    case "TPSCR.G3        SCPL":
                        sw.WriteLine("antennaModel = " + "TPSCR.G3        NONE");
                        break;
                    case "JPSREGANT_DD_E1 NONE":
                        sw.WriteLine("antennaModel = " + "JPSREGANT_DD_E  NONE");
                        break; 
                    default:
                        sw.WriteLine("antennaModel         = " + rovAntennaType);
                        break;

                }
                //if (obsFile.Header.AntennaType == "NONE")
                //{
                //    sw.WriteLine("antennaModel = " + "TRM59800.00     SCIS");
                //    sw.WriteLine();
                //}

                //else
                //{
                //    sw.WriteLine("antennaModel         = " + obsFile.Header.AntennaType);
                //    sw.WriteLine();
                //}
                sw.WriteLine();
                Geo.Coordinates.HEN resHen = resObsFile.Header.Hen;
                Geo.Coordinates.HEN rpvHen = rovObsFile.Header.Hen; 
                sw.WriteLine("refOffsetARP, dH dLon dLat (UEN) = " + resHen.H.ToString() + " " + resHen.E.ToString() + " " + resHen.N.ToString() + ", meters");
                sw.WriteLine("offsetARP, dH dLon dLat (UEN) = " + rpvHen.H.ToString() + " " + rpvHen.E.ToString() + " " + rpvHen.N.ToString() + ", meters");
                sw.WriteLine();
                forwardBackwardCycles = 0;
                sw.WriteLine("forwardBackwardCycles     = " + forwardBackwardCycles);//0 + ", an integer < 1 means forwards processing only"
                sw.WriteLine();
                coordinatesAsWhiteNoise = false;
                sw.WriteLine("coordinatesAsWhiteNoise   = " + coordinatesAsWhiteNoise);//FALSE + "           # Static positioning"
                sw.WriteLine();
                //sw.WriteLine("# The following variable, if true, sets a NEU system. If false, an XYZ system");
                USENEU = false;
                sw.WriteLine("USENEU                    = " + USENEU);//+ ", results will be given in dLat, dLon, dH"
                sw.WriteLine();
                sw.WriteLine("# Output");
                sw.WriteLine("outputFile                = " + camDir.Replace("\\", "\\\\") + "\\\\Out\\\\" + refStation.Substring(0, 4).ToUpper() + rovStation.Substring(0, 4).ToUpper() + ".out");

                sw.WriteLine("finaloutputFile           = " + camDir.Replace("\\", "\\\\") + "\\\\Out\\\\" + "final" + refStation.Substring(0, 4).ToUpper() + rovStation.Substring(0, 4).ToUpper() + ".out");
                this.resultFile = camDir.Replace("\\", "\\\\") + "\\\\Out\\\\" + "final" + refStation.Substring(0, 4).ToUpper() + rovStation.Substring(0, 4).ToUpper() + ".out";//结果文件
                sw.WriteLine();
                sw.WriteLine("printModel                = " + printModel);
                if (printModel == true)
                {
                    sw.WriteLine("modelFile             = " + camDir.Replace("\\", "\\\\") + "\\\\Out\\\\" + refStation.Substring(0, 4).ToUpper() + rovStation.Substring(0, 4).ToUpper() + ".model");
                }
                sw.WriteLine();
            }
            sw.Close();

            Geo.Common.ProcessRunner cmd = new Geo.Common.ProcessRunner();

            string BaselinePath = "\"" + startUpDir + "\\SingleBaseline.exe" + "\"";
            string param3 = " \"" + "--conffile" + "\"" + " \"" + strConfPath + "\""; //

            // string result = cmd.Run(PppPath + param3);//同步运行
            //  string result  cmd.RunAsyn(PppPath + param3);//异步运行
            this.cmdline = BaselinePath + param3;
            #endregion
  
        } 
    }
}
