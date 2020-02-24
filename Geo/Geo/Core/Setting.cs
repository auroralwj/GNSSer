using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Geo
{
   /// <summary>
   /// 通用设置
   /// </summary>
    public  class Setting
    {
        #region 网站服务
        /// <summary>
        /// 网站主页
        /// </summary>
        public static string WebSiteUrl = "http://www.gnsser.com";
        /// <summary>
        /// 汇报 Bug 网址
        /// </summary>
        public static string BugReportUrl = "http://www.gnsser.com/BugReports/Create";
        /// <summary>
        /// 访问网站附带版本信息
        /// </summary>
        public static string WebSiteUrlForNewVersion = "http://www.gnsser.com?version=" + Version + "&type=" + Setting.VersionType + "&pcuser=" + System.Environment.UserName;
        /// <summary>
        /// 当前版本地址
        /// </summary>
        public static string CurrentVersionUrl = "http://www.gnsser.com/Soft/CurrentPublicVersion";
        /// <summary>
        /// 当前版本的网络路径
        /// </summary>
        public static string CurrentVersionCharacterUrl = "http://www.gnsser.com/Soft/CurrentPublicCharacter";
        #endregion
        /// <summary>
        /// GNSSer 临时目录名称
        /// </summary>
        public static string GnsserTempSubDirectoryName = "GnsserTempOutput";
        /// <summary>
        /// GNSSer工程目录
        /// </summary>
        public static string GnsserProjectDirectoryName = "GnsserProject";
        /// <summary>
        /// 版本说明路径
        /// </summary>
        public static string ImprintPath = @"Data\Documents\Imprint.txt";
        /// <summary>
        /// 帮助文档
        /// </summary>
        public static string HelpDocument = @"Data\Documents\Help.pdf";


        /// <summary>
        ///  版本
        /// </summary>
        public const  double Version = 1.40;
        /// <summary>
        /// 结束日期
        /// </summary>
        public static DateTime EndUsage = new DateTime(2025, 9, 1);
        /// <summary>
        /// 起始日期
        /// </summary>
        public static DateTime StartUsage = new DateTime(2019, 3, 1);


        /// <summary>
        /// 发现故障的技术支持地址
        /// </summary>
        public static string SupportEmail = "gnsser@163.com";
        /// <summary>
        /// 发行版本，以此进行功能控制
        /// </summary>
        //public static VersionType VersionType = VersionType.DistributionTesting;
        public static VersionType VersionType = VersionType.Development;//.BaselineNet;// ;///
                                                                     // public static VersionType VersionType = VersionType.Public;
        /// <summary>
        /// 是否启用多系统
        /// </summary>
        public static bool IsEnableMultiSystem = true;
        /// <summary>
        /// 是否启用北斗。
        /// </summary>
        public const bool IsBDsEnabled = false;
        /// <summary>
        /// 是否启用多系统。
        /// </summary>
        public static bool IsMultiSystemEnabled => VersionType == VersionType.Development || IsEnableMultiSystem;

        /// <summary>
        /// 显示关键致命重要的东西
        /// </summary>
        public static bool IsShowFatal = true;
        /// <summary>
        /// 启动日志
        /// </summary>
        public static bool EnableLog = false;
        /// <summary>
        /// 初始是否为调试状态
        /// </summary>
        public static bool IsShowDebug = false;
        /// <summary>
        /// 显示警告,必须设置显示信息才显示
        /// </summary>
        public static bool IsShowWarning = true;
        /// <summary>
        /// 是否显示执行过程中的错误,必须设置显示信息才显示
        /// </summary>
        public static bool IsShowError = true;
        /// <summary>
        /// 显示信息，显示必要的信息，包含处理中的警告和错误，同时控制日志记录。如果未来加快速度可以不显示。
        /// </summary>
        public static bool IsShowInfo = true;

        /// <summary>
        /// 是否允许网络访问
        /// </summary>
        public static bool EnableNet = true;

        /// <summary>
        /// 网络是否可以访问，包括局域网。
        /// </summary>
        public static bool IsNetEnabled { get { return EnableNet && Geo.Utils.NetUtil.IsConnected(); } }
        /// <summary>
        /// 网络是否可以访问 Internet
        /// </summary>
        public static bool IsInternetEnabled { get { return EnableNet && Geo.Utils.NetUtil.IsConnectedToInternet(); } }

        /// <summary>
        /// 初始化程序
        /// </summary>
        public static void Init()
        {
            //TryClearTempDirectory();
        }
        /// <summary>
        /// crx2rnx.exe 路径
        /// </summary>
        public static string PathOfCrx2rnx = "\"" + Path.Combine(Setting.ExeFolder, "crx2rnx.exe") + "\"";



        /// <summary>
        /// 对话框打开的表文件后缀
        /// </summary>
        public static string GnsserTextTableFileFilter = "文本表格文件(*.txt.xls)|*.txt.xls";


        /// <summary>
        /// 对话框打开的表文件后缀
        /// </summary>
        public static string TextTableFileFilter = "文本表格文件(*.txt.xls;*.csv;*.txt;*.xls)|*.txt.xls;*.csv;*.txt;*.xls|所有文件(*.*)|*.*";
        /// <summary>
        /// 表文件后缀 .txt.xls
        /// </summary>
        public static string TextTableFileExtension = ".txt.xls";
        /// <summary>
        /// All in One 文件后缀 .aio
        /// </summary>
        public static string AllInOneFileExtension = ".aio";
        /// <summary>
        /// 文本矩阵
        /// </summary>
        public static string TextMatrixFileExtension = ".mat" + TextTableFileExtension;
        /// <summary>
        /// 分组数据
        /// </summary>
        public static string TextGroupFileExtension = ".group" + TextTableFileExtension;


        /// <summary>
        /// 分组数据
        /// </summary>
        public static string TextGroupFileFilter = "GNSSer分组数值文件|*" + TextGroupFileExtension;
        /// <summary>
        /// 二进制文本矩阵
        /// </summary>
        public static string BinaryMatrixFileExtension = ".binmat";
        /// <summary>
        /// 二进制文本矩阵
        /// </summary>
        public static string BinaryMatrixFileFilter = "GNSSer二进制矩阵|*" + BinaryMatrixFileExtension;
        /// <summary>
        /// 文本矩阵
        /// </summary>
        public static string TextMatrixFileFilter = "GNSSer文本矩阵|*" + TextMatrixFileExtension;
        /// <summary>
        /// 文本矩阵方程
        /// </summary>
        public static string TextMatrixEquationFileExtension = ".equation" + AllInOneFileExtension;
        /// <summary>
        /// 二进制矩阵方程
        /// </summary>
        public static string BinaryMatrixEquationFileExtension = ".binequation";
        /// <summary>
        /// 二进制文本矩阵方程
        /// </summary>
        public static string BinaryMatrixEquationFileFilter = "GNSSer二进制矩阵方程|*" + BinaryMatrixEquationFileExtension;
        /// <summary>
        /// 文本矩阵方程
        /// </summary>
        public static string TextMatrixEquationFileFilter = "GNSSer文本矩阵方程|*" + TextMatrixEquationFileExtension +";" + "*" + TextMatrixEquationFileExtension + TextTableFileExtension;


        /// <summary>
        /// 钟跳文件后缀
        /// </summary>
        public static string ClockJumpFileExtension = ".cjump" + TextTableFileExtension;
        /// <summary>
        ///表文件默认分割
        /// </summary>
        public static string [] DefaultTextTableSpliter = new string[] { "\t"};
        /// <summary>
        /// GNSSer 观测文件格式
        /// </summary>
        public static string GnsserObsFileExtension = ".gobs";
        /// <summary>
        /// GNSSer 星历文件格式
        /// </summary>
        public static string GnsserEphFileExtension = ".geph";
        /// <summary>
        /// 卫星高度角文件过滤器
        /// </summary>
        public static string SatElevationFileExtension = ".elev" + TextTableFileExtension;
        /// <summary>
        /// 模糊度文件后缀 .amb.txt.xls
        /// </summary>
        public static string AmbiguityFileExtension = ".amb" + TextTableFileExtension;
        /// <summary>
        /// 历元Observation文件后缀.EpochObs.txt.xls
        /// </summary>
        public static string EpochObsFileExtension = ".EpochObs" + TextTableFileExtension;
        /// <summary>
        /// 历元残差文件后缀.EpochObs.txt.xls
        /// </summary>
        public static string EpochResidualFileExtension = ".EpochResidual" + TextTableFileExtension;
        /// <summary>
        /// 坐标 BaseLine 文件后缀
        /// </summary>
        public static string BaseLineFileExtension = ".BaseLine" + TextTableFileExtension;
        /// <summary>
        /// .asc
        /// </summary>
        public static string BaseLineFileOfLgoExtension = ".asc";
        /// <summary>
        /// 历元 EpochCoord 文件后缀
        /// </summary>
        public static string EpochCoordFileExtension = ".EpochCoord" + TextTableFileExtension;
        /// <summary>
        /// 测站坐标文件 SiteCoordFileExtension 文件后缀
        /// </summary>
        public static string SiteCoordFileExtension = ".SiteCoord" + TextTableFileExtension;
        /// <summary>
        /// 历元Dop文件后缀
        /// </summary>
        public static string EpochDopFileExtension = ".Dop" + TextTableFileExtension;
        /// <summary>
        /// 历元参数文件后缀
        /// </summary>
        public static string EpochParamFileExtension = ".EpochParam" + TextTableFileExtension;
        /// <summary>
        /// 参数文件后缀
        /// </summary>
        public static string ParamFileExtension = ".Param" + TextTableFileExtension;
        /// <summary>
        /// 历元RMS文件后缀
        /// </summary>
        public static string EpochParamRmsFileExtension = ".EpochParamRms" + TextTableFileExtension;
        /// <summary>
        /// 第二历元参数文件后缀
        /// </summary>
        public static string EpochSecondParamFileExtension = "2.EpochParam" + TextTableFileExtension;
        /// <summary>
        /// 第二历元RMS文件后缀
        /// </summary>
        public static string EpochSecondParamRmsFileExtension = "2.EpochParamRms" + TextTableFileExtension;
        /// <summary>
        /// 卫星高度角文件后缀
        /// </summary>
        public static string SatElevationFileFilter = "GNSSer 卫星高度角文件|*" + SatElevationFileExtension + "|" + TextTableFileFilter;

        /// <summary>
        /// 矩阵文件过滤器
        /// </summary>
        public static string MatrixTableFileFilter = TextMatrixEquationFileFilter + "|" + BinaryMatrixFileFilter + "|" + TextTableFileFilter;

        /// <summary>
        ///  矩阵方程过滤器
        /// </summary>
        public static string MatrixEquationFileFilter = TextMatrixEquationFileFilter +"|" + BinaryMatrixEquationFileFilter + "|" + TextTableFileFilter;

        /// <summary>
        /// 模糊度文件过滤器
        /// </summary>
        public static string AmbiguityFileFilter = "GNSSer 模糊度文件|*" + AmbiguityFileExtension + "|" + TextTableFileFilter;
        /// <summary>
        /// 历元坐标文件过滤器
        /// </summary>
        public static string EpochCoordFileFilter = "GNSSer 历元坐标文件|*" + EpochCoordFileExtension + "|" + TextTableFileFilter;
        /// <summary>
        /// 测站坐标文件过滤器
        /// </summary>
        public static string SiteCoordFileFilter = "GNSSer 测站坐标文件|*" + SiteCoordFileExtension + "|" + TextTableFileFilter;
        /// <summary>
        /// 基线坐标文件过滤器
        /// </summary>
        public static string BaseLineFileFilter = "GNSSer 基线坐标文件|*" + BaseLineFileExtension + "|" + TextTableFileFilter;
        public static string BaseLineFileFilterOfLgo = "LGO 基线坐标文件|*" + BaseLineFileOfLgoExtension + "|" + TextTableFileFilter;
        /// <summary>
        /// 历元参数文件过滤器
        /// </summary>
        public static string EpochParamFileFilter = "GNSSer 历元参数文件|*" + EpochParamFileExtension + "|" + TextTableFileFilter;
        /// <summary>
        ///参数文件过滤器
        /// </summary>
        public static string ParamFileFilter = "GNSSer 参数文件|*" + ParamFileExtension + "|" + TextTableFileFilter;
        
        /// <summary>
        /// 历元参数RMS文件过滤器
        /// </summary>
        public static string EpochParamRmsFileFilter = "GNSSer 历元参数RMS文件|*" + EpochParamRmsFileExtension + "|" + TextTableFileFilter;
        /// <summary>
        /// 钟跳文件后缀
        /// </summary>
        public static string ClockJumpFileFilter = "GNSSer 钟跳文件|*" + ClockJumpFileExtension + "|" + TextTableFileFilter;

        /// <summary>
        /// 对话框打开的表文件后缀
        /// </summary>
        public static string TotalTextTableFileFilter = GnsserTextTableFileFilter
            + "|" + "GNSSer FCB 文件 |*.fcb" + TextTableFileExtension
            + "|" + "GNSSer 钟跳文件 " + ClockJumpFileExtension + "|*" + ClockJumpFileExtension
            + "|" + TextTableFileFilter;
        /// <summary>
        /// 所有图像文件
        /// </summary>
        public static string ImageFilter = "所有图像文件 | *.bmp; *.pcx; *.png; *.jpg; *.gif;" +
                   "*.tif; *.ico; *.dxf; *.cgm; *.cdr; *.wmf; *.eps; *.emf|" +
                   "位图( *.bmp; *.jpg; *.png;...) | *.bmp; *.pcx; *.png; *.jpg; *.gif; *.tif; *.ico|" +
                   "矢量图( *.wmf; *.eps; *.emf;...) | *.dxf; *.cgm; *.cdr; *.wmf; *.eps; *.emf";

        /// <summary>
        /// 程序根目录
        /// </summary>
        static public string ApplicationDirectory
        { get { return AppDomain.CurrentDomain.BaseDirectory; } }
       // Path.GetDirectoryName( System.Reflection.Assembly.Load("Geo").Location); } }
                
        // System.Reflection.Assembly.GetExecutingAssembly().Location;
        // Environment.CurrentDirectory; } }
        // AppDomain.CurrentDomain.BaseDirectory; } }
        /// <summary>
        /// EXE 目录
        /// </summary>
        public static string ExeFolder { get { return Path.Combine(DataDirectory, "Exe"); } } 
        /// <summary>
        /// 数据目录
        /// </summary>
        static public string DataDirectory { get { return Path.Combine(ApplicationDirectory, "Data"); } }
        /// <summary>
        /// 临时目录，程序启动或退出时将清空
        /// </summary>
        static  public string TempDirectory { get { return Path.Combine(ApplicationDirectory, @"Temp"); } }
        /// <summary>
        /// 尝试清空临时目录
        /// </summary>
        static public void TryClearTempDirectory()
        {
             Geo.Utils.FileUtil.TryClearDirectory(TempDirectory);
        }
    }
}
