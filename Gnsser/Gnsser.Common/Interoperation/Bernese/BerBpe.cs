using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using System.IO;//Stream
using System.Net;//FtpWebRequest
using System.Threading;
using Geo.Common;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser.Interoperation.Bernese
{
    /// <summary>
    /// PCF 其名称与PCF文件夹下的PCF对应。
    /// 可以直接 PcfName.PCF 使用。
    /// </summary>
    public class PcfName
    {
        public const string  PPP = "PPP";
        public const string RNX2SNX = "RNX2SNX";
        public const string BASTST = "BASTST";
        public const string CLKDET = "CLKDET";
        public const string SUPERBPE = "SUPERBPE"; 
    }

    ///
    /// <summary>
    ///  Bernese BerBpe.
    /// </summary>
    public class BerBpe
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BerBpe()
        {
        }
        /// <summary>
        /// Bernese BerBpe.
        /// </summary>
        /// <param name="campaign">本地的工程文件名，不是路径</param>
        /// <param name="gpsTime">观测日期，年月日，YYYYMMDD</param>
        /// <param name="skip">是否忽略</param>
        public BerBpe(string campaign, Time gpsTime, bool skip = false)
        {
            Init(campaign, gpsTime, skip);
        }

        /// <summary>
        /// 初始化工作。
        /// </summary>
        /// <param name="campaign">本地的工程文件名，不是路径</param>
        /// <param name="gpsTime">观测日期，年月日，YYYYMMDD</param>
        /// <param name="skip">是否跳过一些步骤</param>
        public void Init(string campaign, Time gpsTime, bool skip = false)
        {
            this.Skip = skip;
            BerBpe.MenuPath = Path.Combine(BerGpsUerPath, @"PAN\MENU.INP");
            BerBpe.RunBpePath = Path.Combine(BerGpsUerPath, @"PAN\RUNBPE.INP");
            BerBpe.RunBpeModelPath = Path.Combine(BerGpsUerPath, @"PAN\RUNBPEMODE.INP");
            BerBpe.MenuRunPcfPath = Path.Combine(BerGpsUerPath + @"PAN\MENU_RUNPCF.INP");

            this.cmdline = BerPath + @"MENU\menu" + " " + MenuPath + " " + MenuRunPcfPath;

            this.campaign = campaign;
            this.gpsTime = gpsTime;
        }

        /// <summary>
        /// 是否跳过一些步骤。                                                                                                                                                                                                                                                                                     
        /// </summary>
        public bool Skip { get; set; }

        ProcessRunner cmd = new ProcessRunner();
        /// <summary>
        /// CMD
        /// </summary>
        public ProcessRunner WinCmd
        {
            get { return cmd; }
            set { cmd = value; }
        }

        private string campaign;//本地的工程文件名 
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
        /// TIme
        /// </summary>
        public Time GpsTime
        {
            get { return gpsTime; }
            set { gpsTime = value; }
        }

        public static string BerGpsUerPath = @"C:\GPSUSER\";
        public static string BerPath = @"C:\BERN50\";
        public static string BerGpsDataPath = @"C:\GPSDATA\";

        //运行BPE，至少需要修改下面3个INP文件
        private static string MenuPath, RunBpePath, RunBpeModelPath, MenuRunPcfPath;

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
        static string SERVER_VARIABLES_RNX_0 = "SERVER_VARIABLES_0 8" + "\r\n"
            + "  \"V_A\"  \"APR\"  \"A priori information\"" + "\r\n"
            + "  \"V_B\"  \"IGS\"  \"Orbit/ERP, DCB, ION information\"" + "\r\n"
            + "  \"V_C\"  \"P1_\"  \"Preliminary (fraction-float) result\"" + "\r\n"
            + "  \"V_E\"  \"F1_\"  \"Final (fraction-fixed) results\"" + "\r\n"
            + "  \"V_F\"  \"R1_\"  \"Size-reduced NEQ information\"" + "\r\n"
            + "  \"V_MINUS\"  \"-6\"  \"Session range begin (for COMPAR)\"" + "\r\n"
            + "  \"V_PLUS\"  \"+0\"  \"Session range end\"" + "\r\n"
            + "  \"V_CLU\"  \"3\"  \"Maximum number of files per cluster\"";
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
            SetBpeEnviroment(campaign, this.gpsTime, pcfName, Skip);
           return cmd.Run(cmdline)[0];
        }
        /// <summary>
        /// 异步运行
        /// </summary>
        /// <param name="pcfName"></param>
        public void RunAsyn(string  pcfName)
        {
            SetBpeEnviroment(campaign, this.gpsTime, pcfName, Skip); 
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
                return BerGpsDataPath + campaign + "\\SOL\\F1_" + gpsTime.SubYear.ToString("00") + gpsTime.DayOfYear + "0.SNX";
            if (pcfName == PcfName.PPP)
                return BerGpsDataPath + campaign + "\\SOL\\PPP" + gpsTime.SubYear.ToString("00") + gpsTime.DayOfYear + "0.SNX";
            return "none";
        }

        /// <summary>
        /// 获取当前Bernese运行状态，running,finished,error,等等。
        /// </summary>
        /// <returns></returns>
        public string GetRunningState(string pcfName)
        {
            string strSNXPath =BerGpsDataPath + campaign + "\\BPE\\" + pcfName.ToString().Substring(0, 3) + ".RUN";
            if (File.Exists(strSNXPath))
                using (StreamReader reader = new StreamReader(strSNXPath))
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim().StartsWith("Session"))
                        {
                            return line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                        }
                    }
                }
            return "none";
        }


        /// <summary>
        /// 设置BPE运行环境
        /// </summary>
        private static void SetBpeEnviroment(string campaign, Time gpsTime, string pcfName, bool skip)
        {
            //调用PPP，Run PPP，首先根据工程信息修改环境设置文件
            string julianDate = gpsTime.MJulianDays.ToString("0.000000");//约旦时 
            ModifyMenuInp(campaign, julianDate);
            ModifyRunbpeInp(campaign, gpsTime, pcfName, skip);
            ModifyMenuRunPcfInp();
        }

        /// <summary>
        ///  调用Bern的BPE
        ///  1)首先，修改环境.INP文件//@"C:\GPSUSER\PAN\RUNBPE.INP"//@"C:\GPSUSER\PAN\MENU.INP"
        /// ModifyINP 为运行BPE而修改Bernese运行环境设置文件内容 *.INP，主菜单MENU.INP
        /// </summary>
        /// <param name="Campagin"></param>
        /// <param name="JulianDate"></param>
        public static void ModifyMenuInp(string Campagin, string JulianDate)
        {
            //JulianDate形式如52417.000000！
            String menuInpTxt = String.Empty;
            String[] lines = File.ReadAllLines(MenuPath); ;
            //以下只是为运行PPP而作修改，不同工程，可能还需要修改其他内容
            foreach (String line in lines)
            {
                if (line.Contains("ACTIVE_CAMPAIGN"))
                {
                    menuInpTxt += "ACTIVE_CAMPAIGN 1  " + "\"" + "${P}\\" + Campagin + "\"" + "\r\n";// \" 代表 ",\\ 代表\
                }
                else if (line.Contains("MODJULDATE"))
                {
                    menuInpTxt += "MODJULDATE 1  " + "\"" + JulianDate + "\"" + "\r\n";
                }
                else if (line.Contains("SESSION_TABLE"))
                {
                    menuInpTxt += "SESSION_TABLE 1  " + "\"" + "${P}\\" + Campagin + "\\STA\\SESSIONS.SES" + "\"" + "\r\n";// \" 代表 ",\\ 代表\
                }
                else
                {
                    menuInpTxt += line + "\r\n";
                }
            }

            File.WriteAllText(MenuPath, menuInpTxt);
        }

        
        /** ModifyINP
         * 为运行BPE而修改Bernese运行环境设置文件内容 *.INP，RUNBPE.INP
         * 
         * &{pcfName}:PPP、RNX2SNX
         * &{campagin}:EXAMPLE
         * &{Year}:2002
         * &{Session}:1430
         * &{PCFAb}:snx\ppp
         * &{SERVER_VARIABLES}:
         * &{SERVER_VARIABLES_0}:
         */
        public static void ModifyRunbpeInp(string campagin, Time gpsTime, string  pcfName, bool skip)
        {
            //year是4位，session代表第几天（如123），要在前、后面加上0，凑成4位，这里默认是四位
            string Session = Geo.Utils.StringUtil.FillZeroLeft(gpsTime.DayOfYear, 4);
            //(1)Station information fileB name
            //(2)Tectonic plate definition fileB name
            //(3)Station name abbreviation fileB name,(1)(2)(3)三个文件的前缀名字采用一样的方式！

            String runbpeInpTxt = File.ReadAllText(RunBpeModelPath);

            switch (pcfName)
            {
                case PcfName.PPP:
                    runbpeInpTxt = runbpeInpTxt.Replace("&{SERVER_VARIABLES}", SERVER_VARIABLES_PPP);
                    runbpeInpTxt = runbpeInpTxt.Replace("&{SERVER_VARIABLES_0}", SERVER_VARIABLES_PPP_0);

                    if(skip) runbpeInpTxt = runbpeInpTxt.Replace("&{SKIP}", SKIP_PPP);
                    else runbpeInpTxt = runbpeInpTxt.Replace("&{SKIP}", ZERO_SKIP);
                    break;
                case PcfName.RNX2SNX:
                    runbpeInpTxt = runbpeInpTxt.Replace("&{SERVER_VARIABLES}", SERVER_VARIABLES_RNX);
                    runbpeInpTxt = runbpeInpTxt.Replace("&{SERVER_VARIABLES_0}", SERVER_VARIABLES_RNX_0);

                    if (skip) runbpeInpTxt = runbpeInpTxt.Replace("&{SKIP}", SKIP_RNX);
                    else runbpeInpTxt = runbpeInpTxt.Replace("&{SKIP}", ZERO_SKIP);
                    break;
                default:
                    break;
            }

            runbpeInpTxt = runbpeInpTxt.Replace("&{PCFName}", pcfName.ToString())
                 .Replace("&{Campagin}", campagin)
                 .Replace("&{Year}", gpsTime.Year.ToString())
                 .Replace("&{Session}", Session)
                 .Replace("&{PCFAb}", pcfName.ToString().Substring(0, 3));

            File.WriteAllText(RunBpePath, runbpeInpTxt);
        }

        /// <summary>
        /// ModifyINP 为运行BPE而修改Bernese运行环境设置文件内容 *.INP，MENU_RUNPCF_INPE.INP
        /// </summary>
        public static void ModifyMenuRunPcfInp()
        {
            String menuRunPcfInpTxt = String.Empty;//

            String[] lines = File.ReadAllLines(MenuRunPcfPath);
            //不同工程，可能还需要修改其他内容！
            foreach (String line in lines)
            {
                if (line.Contains("RUN_BPE 1  "))
                {
                    menuRunPcfInpTxt += "RUN_BPE 1  " + "\"" + "${U}\\PAN\\RUNBPE.INP\"" + "\r\n";// \" 代表 ",\\ 代表\
                }
                else
                {
                    menuRunPcfInpTxt += line + "\r\n";
                }
            }

            File.WriteAllText(MenuRunPcfPath, menuRunPcfInpTxt);
        }

    }
}