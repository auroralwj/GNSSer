using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Algorithm.Adjust;
using Algorithm.Adjust;
using Gnsser.Data.Sinex;
using Geo.Algorithm;
//using Geo.Algorithm.CyMatrix;
using System.Diagnostics;//Added for the stopwatch
using System.Threading.Tasks;

namespace Gnsser.Service
{
    /// <summary>
    /// 平差,包括最小二乘平差和抗差估计，以及抗差贝叶斯估计
    /// 观测值是基于sinex文件报讯的基线向量文件,类似于近代平差课程中的基线向量文件
    /// </summary>
    public class SinexNetWorkAdjustmentLLY
    {
        #region
        AdjustBasicInformation BasicInformation = new AdjustBasicInformation();

        /// <summary>
        /// 基线观测向量
        /// 存储所有的观测向量,每个基线向量三个分量构成一个XYZ
        /// </summary>
        public List<XYZ> BaseLineList = new List<XYZ>();

        /// <summary>
        /// 点坐标近似值
        /// 坐标数组，三个坐标构成一个XYZ，顺序与点号一一对应
        /// </summary>
        public List<XYZ> PointsAppXYZ = new List<XYZ>();

        /// <summary>
        /// 点坐标真值
        /// 坐标数组，三个坐标构成一个XYZ，顺序与点号一一对应
        /// </summary>
        public List<XYZ> PointsEstimateXYZ = new List<XYZ>();

        /// <summary>
        /// 是否是已知点
        /// </summary>
        public bool[] IsKnownPoint { get; set; }

        //每个文件单独存储基线向量、协方差矩阵、基线向量点号
        /// <summary>
        /// 基线向量，3m*1一维的矩阵块列，m=总点数减1，即为独立的基线向量数
        /// </summary>
        public List<double[][]> BaseLineBlockList = new List<double[][]>();

        /// <summary>
        /// 基线向量协方差矩阵,3m*3m
        /// </summary>
        public List<double[][]> BaseLineCovBlockList = new List<double[][]>();

        /// <summary>
        /// 基线向量点号，m*2维的矩阵列块，两列矩阵，首列是起点点号的序列，第二列是终点点号的序列
        /// </summary>
        public List<double[][]> BaseLineNameBlockList = new List<double[][]>();

        /// <summary>
        /// ?????????不知道
        /// </summary>
        List<SinexFile> SinexFiles = null;
        List<SinexFile> PriorSinexFile = null;
        #endregion

        /// <summary>
        /// 最小二乘平差
        /// </summary>
        /// <param name="WS_FilePath">周解文件,数量:一个</param>
        /// <param name="DS_FilePath">日解文件</param>
        /// <param name="KnownPoint">已知点</param>
        public SinexNetWorkAdjustmentLLY(string[] WS_FilePath, string[] DS_FilePath, string[] KnownPoint)
        {

            for (int i = 0; i < KnownPoint.Length; i++)
            {
                BasicInformation.KnownPointName.Add(KnownPoint[i]);//先读取已知跟踪站
            }

            SinexFiles = SinexReader.Read(DS_FilePath);//把日解文件读进来
            BasicInformation.FileNumber = SinexFiles.Count;
            ConFirmPointBasicInformation(SinexFiles);//获取点基本信息

            //建立基线
            List<int[]> TotalBeginOfBaselineList = new List<int[]>();//起点点号的索引,长度是总点数减1
            List<int[]> TotalEndOfBaselineList = new List<int[]>();//终点点号的索引,长度是总点数减1
            double[][] bList = new double[BasicInformation.FileNumber][];//基线向量的坐标差,（总点数-1）*3长度1302
            double[][] pList = new double[BasicInformation.FileNumber][];//L的协方差矩阵

            //建立基线信息:基线向量坐标差,bList基线向量的协方差矩阵pList,基线向量的起始点TotalBeginOfBaselineList和TotalEndOfBaselineList
            BuildCleanBaselineInformation(ref TotalBeginOfBaselineList, ref TotalEndOfBaselineList, ref bList, ref pList);

            //string WeekSolution_Path = "G:\\VS_program\\Geo2014.05.12.17.19\\Gnsser\\Gnsser.Winform\\bin\\Debug\\Data\\GNSS\\分区平差\\1721\\igs13P1721.snx";
            string WeekSolution_Path = WS_FilePath[0];

            //获得公共点坐标的估值,该估值是1721周周解的坐标估值
            List<XYZ> PubEst = GetPubPointEstXYZ(WeekSolution_Path);

            //PointsAppXYZ是加入了随机数之后的坐标
            double[] temXYZ = new double[PointsAppXYZ.Count * 3];//435*3
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                temXYZ[3 * i + 0] = PointsAppXYZ[i].X;
                temXYZ[3 * i + 1] = PointsAppXYZ[i].Y;
                temXYZ[3 * i + 2] = PointsAppXYZ[i].Z;
            }

            var SW_LS = Stopwatch.StartNew();//对新的 Stopwatch 实例进行初始化，将运行时间属性设置为零，然后开始测量运行时间。

            //最小二乘解
            RobustBayesAdjustment LSAdjustment = new RobustBayesAdjustment(BasicInformation, temXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            LSAdjustment.LeastSquare();

            double[] LS_Est = LSAdjustment.PointsAppXYZ;
            double Sigma_LS = LSAdjustment.sigma;
            string Time_LS = SW_LS.ElapsedMilliseconds.ToString();

            //输出结果
            #region
            //设置保存路径
            string SavePath = "E:\\VS_Pro\\Test\\Robust_Bayes\\1721\\result\\LS_Result" + ".txt";

            FileInfo aFile = new FileInfo(SavePath);
            StreamWriter SW = aFile.CreateText();//FileInfo的CreateText方法创建写入新文本文件的 StreamWriter

            //CultureInfo:提供有关特定区域性的信息（对于非托管代码开发，则称为“区域设置”）。 这些信息包括区域性的名称、书写系统、使用的日历以及对日期和排序字符串的格式化设置。
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;// System.Globalization.NumberFormatInfo根据区域性定义如何设置数值格式以及如何显示数值
            GN.NumberDecimalDigits = 6; //小数点后6位
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                XYZ LSXYZ = new XYZ(PubEst[i].X - LS_Est[3 * i + 0], PubEst[i].Y - LS_Est[3 * i + 1], PubEst[i].Z - LS_Est[3 * i + 2]);
                SW.Write(LSXYZ.X.ToString("N",GN));
                SW.Write(" ");
                SW.Write(LSXYZ.Y.ToString("N",GN));
                SW.Write(" ");
                SW.Write(LSXYZ.Z.ToString("N",GN));
                SW.Write("\n");
            }
            SW.Write(Time_LS);
            SW.Write("\n");
            SW.Write(Sigma_LS);
            SW.Close();
            #endregion
            
        }
        /// <summary>
        /// 抗差贝叶斯估计
        /// </summary>
        /// <param name="WS_FilePath"></param>
        /// <param name="DS_FilePath"></param>
        /// <param name="KnownPoint"></param>
        /// <param name="k0"></param>
        /// <param name="k1"></param>
        /// <param name="eps"></param>
        public SinexNetWorkAdjustmentLLY(string[] WS_FilePath, string[] DS_FilePath, string[] Priori_WS_FilePath, string[] strKnownPoint, double k0, double k1, double eps, double rate)
        {
            //获取已知点号信息
            for (int i = 0; i < strKnownPoint.Length; i++)
            {
                BasicInformation.KnownPointName.Add(strKnownPoint[i]);
            }
            List<string> tmpFilePath = new List<string>();
            for (int i = 0; i < DS_FilePath.Length; i++)
            {
                tmpFilePath.Add(DS_FilePath[i]);
            }
            for (int j = 0; j < Priori_WS_FilePath.Length; j++)
            {
                tmpFilePath.Add(Priori_WS_FilePath[0]);//默认是7个日解文件,再把上一周的周解文件加进来
            }
            string[] TmpFilePath = new string[tmpFilePath.Count];
            for (int k = 0; k < tmpFilePath.Count; k++)
            {
                TmpFilePath[k] = tmpFilePath[k];
            }

            SinexFiles = SinexReader.Read(TmpFilePath);//读取文件,每循环一次，读取的文件增加一次

            BasicInformation.FileNumber = SinexFiles.Count;

            //获取点基本信息,主要是多个snx文件的公共信息
            ConFirmPointBasicInformation(SinexFiles);
            BasicInformation.TotalBaselineNumber = BasicInformation.TotalBaselineNumber - (BasicInformation.TotalPointNumber - 1);//总的基线向量个数                  
            BasicInformation.FileNumber = BasicInformation.FileNumber - 1;//Sinex文件总数

            //建立基线信息：纯净的数据
            List<int[]> TotalBeginOfBaselineList = new List<int[]>();
            List<int[]> TotalEndOfBaselineList = new List<int[]>();
            double[][] bList = new double[BasicInformation.FileNumber][];
            double[][] pList = new double[BasicInformation.FileNumber][];

            BuildBaselineInformation(ref TotalBeginOfBaselineList, ref TotalEndOfBaselineList, ref bList, ref pList);

            //建立先验信息,先验协方差矩阵和先验坐标值
            double[] Priori_X = new double[3 * BasicInformation.TotalPointNumber];
            double[] Priori_CovX = new double[3 * BasicInformation.TotalPointNumber * (3 * BasicInformation.TotalPointNumber + 1) / 2];
            PriorSinexFile = SinexReader.Read(Priori_WS_FilePath);
            BuildPriorInformation(ref Priori_CovX, ref Priori_X);//正确

            //对观测值加粗差,即对bList基线向量坐标差修改
            double sigma = 0.002;//中误差
            AddGrossError(ref bList, rate, sigma);

            List<XYZ> PubEst = GetPubPointEstXYZ(WS_FilePath[0]);//这个originfile应该是周解文件,不能改变吧？？？？？

            double[] tmpXYZ = new double[PointsAppXYZ.Count * 3];
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                tmpXYZ[3 * i + 0] = PointsAppXYZ[i].X;
                tmpXYZ[3 * i + 1] = PointsAppXYZ[i].Y;
                tmpXYZ[3 * i + 2] = PointsAppXYZ[i].Z;
            }

            var SW_BA = Stopwatch.StartNew();

            //贝叶斯的解
            RobustBayesAdjustment BAAdjustment1 = new RobustBayesAdjustment(BasicInformation, tmpXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            BAAdjustment1.BayesEstimate();
            double[] BA_Est = BAAdjustment1.PointsAppXYZ;
            double SigmaBA = BAAdjustment1.sigma;
            string TimeBA = SW_BA.ElapsedMilliseconds.ToString();

            //并行贝叶斯的解
            var SW_Parallel_BA = Stopwatch.StartNew();
            RobustBayesAdjustment BAAdjustment2 = new RobustBayesAdjustment(BasicInformation, tmpXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            BAAdjustment2.Parallel_BayesEstimate();
            double[] Parallel_BA_Est = BAAdjustment2.PointsAppXYZ;
            double Parallel_SigmaBA = BAAdjustment2.sigma;
            string Parallel_TimeBA = SW_Parallel_BA.ElapsedMilliseconds.ToString();

            //抗差贝叶斯解t=1
            var SW_ROBA1 = Stopwatch.StartNew();
            RobustBayesAdjustment ROBAAdjustment1 = new RobustBayesAdjustment(BasicInformation, tmpXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            ROBAAdjustment1.RobustBayes(FactorYype.IGG3, eps, k0, k1, 1);
            double[] ROBA_Est1 = ROBAAdjustment1.PointsAppXYZ;
            double SigmaROBA1 = ROBAAdjustment1.sigma;
            string TimeROBA1 = SW_ROBA1.ElapsedMilliseconds.ToString();

            ////抗差贝叶斯解t=2
            //var SW_ROBA2 = Stopwatch.StartNew();
            //RobustBayesAdjustment ROBAAdjustment2 = new RobustBayesAdjustment(BasicInformation, tmpXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            //ROBAAdjustment2.RobustBayes(FactorYype.IGG3, eps, k0, k1, 2);  //(FactorYype.IGG3, eps, k0, k1, 2);
            //double[] ROBA_Est2 = ROBAAdjustment2.PointsAppXYZ;
            //double SigmaROBA2 = ROBAAdjustment2.sigma;
            //string TimeROBA2 = SW_ROBA2.ElapsedMilliseconds.ToString();

            ////抗差贝叶斯解t=3
            //var SW_ROBA3 = Stopwatch.StartNew();
            //RobustBayesAdjustment ROBAAdjustment3 = new RobustBayesAdjustment(BasicInformation, tmpXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            //ROBAAdjustment3.RobustBayes(FactorYype.IGG3, eps, k0, k1, 3);
            //double[] ROBA_Est3 = ROBAAdjustment3.PointsAppXYZ;
            //double SigmaROBA3 = ROBAAdjustment3.sigma;
            //string TimeROBA3 = SW_ROBA3.ElapsedMilliseconds.ToString();

            //并行抗差贝叶斯解t=1
            var SW_Parallel_ROBA1 = Stopwatch.StartNew();
            RobustBayesAdjustment Parallel_ROBAAdjustment1 = new RobustBayesAdjustment(BasicInformation, tmpXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            Parallel_ROBAAdjustment1.Parallel_RobustBayes(FactorYype.IGG3, eps, k0, k1, 1);
            double[] Parallel_ROBA_Est1 = Parallel_ROBAAdjustment1.PointsAppXYZ;
            double Parallel_SigmaROBA1 = Parallel_ROBAAdjustment1.sigma;
            string Parallel_TimeROBA1 = SW_Parallel_ROBA1.ElapsedMilliseconds.ToString();

            //并行抗差贝叶斯解t=2
            //var SW_Parallel_ROBA2 = Stopwatch.StartNew();
            //RobustBayesAdjustment Parallel_ROBAAdjustment2 = new RobustBayesAdjustment(BasicInformation, tmpXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            //Parallel_ROBAAdjustment2.Parallel_RobustBayes(FactorYype.IGG3, eps, k0, k1, 2);
            //double[] Parallel_ROBA_Est2 = Parallel_ROBAAdjustment2.PointsAppXYZ;
            //double Parallel_SigmaROBA2 = Parallel_ROBAAdjustment2.sigma;
            //string Parallel_TimeROBA2 = SW_Parallel_ROBA2.ElapsedMilliseconds.ToString();

            ////并行抗差贝叶斯解t=3
            //var SW_Parallel_ROBA3 = Stopwatch.StartNew();
            //RobustBayesAdjustment Parallel_ROBAAdjustment3 = new RobustBayesAdjustment(BasicInformation, tmpXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            //Parallel_ROBAAdjustment3.Parallel_RobustBayes(FactorYype.IGG3, eps, k0, k1, 3);
            //double[] Parallel_ROBA_Est3 = Parallel_ROBAAdjustment3.PointsAppXYZ;
            //double Parallel_SigmaROBA3 = Parallel_ROBAAdjustment3.sigma;
            //string Parallel_TimeROBA3 = SW_Parallel_ROBA3.ElapsedMilliseconds.ToString();

            //结果文件输出
            #region
            string SavePath = "E:\\VS_Pro\\Test\\Robust_Bayes\\1721\\result\\RobustBayes_Result" + "(" + k0 + " " + k1 + ")" + ".txt";
            FileInfo aFile = new FileInfo(SavePath);
            StreamWriter SW = aFile.CreateText();
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;
            GN.NumberDecimalDigits = 6;

            SW.WriteLine(BasicInformation.TotalPointNumber.ToString());
            SW.WriteLine("Bayes中误差 = " + SigmaBA.ToString("N", GN));
            SW.WriteLine("Bayes串行估计时间 = " + TimeBA);
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                XYZ BA_XYZ = new XYZ(PubEst[i].X - BA_Est[3 * i + 0], PubEst[i].Y - BA_Est[3 * i + 1], PubEst[i].Z - BA_Est[3 * i + 2]);

                SW.Write(BA_XYZ.X.ToString("N", GN));
                SW.Write(" ");
                SW.Write(BA_XYZ.Y.ToString("N", GN));
                SW.Write(" ");
                SW.Write(BA_XYZ.Z.ToString("N", GN));
                SW.Write("\n");
            }

            SW.WriteLine(BasicInformation.TotalPointNumber.ToString());
            SW.WriteLine("并行Bayes中误差 = " + Parallel_SigmaBA.ToString("N", GN));
            SW.WriteLine("Bayes并行估计时间 = " + Parallel_TimeBA);
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                XYZ BA_XYZ = new XYZ(PubEst[i].X - Parallel_BA_Est[3 * i + 0], PubEst[i].Y - Parallel_BA_Est[3 * i + 1], PubEst[i].Z - Parallel_BA_Est[3 * i + 2]);

                SW.Write(BA_XYZ.X.ToString("N", GN));
                SW.Write(" ");
                SW.Write(BA_XYZ.Y.ToString("N", GN));
                SW.Write(" ");
                SW.Write(BA_XYZ.Z.ToString("N", GN));
                SW.Write("\n");
            }

            SW.WriteLine("抗差中误差(t=1) = " + SigmaROBA1.ToString("N", GN));
            SW.WriteLine("抗差估计时间(t=1) = " + TimeROBA1);
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                XYZ RO_XYZ = new XYZ(PubEst[i].X - ROBA_Est1[3 * i + 0], PubEst[i].Y - ROBA_Est1[3 * i + 1], PubEst[i].Z - ROBA_Est1[3 * i + 2]);

                SW.Write(RO_XYZ.X.ToString("N", GN));
                SW.Write(" ");
                SW.Write(RO_XYZ.Y.ToString("N", GN));
                SW.Write(" ");
                SW.Write(RO_XYZ.Z.ToString("N", GN));
                SW.Write("\n");
            }

            //SW.WriteLine("抗差中误差(t=2) = " + SigmaROBA2.ToString("N", GN));
            //SW.WriteLine("抗差估计时间(t=2) = " + TimeROBA2);
            //for (int time = 0; time < PointsAppXYZ.Count; time++)
            //{
            //    XYZ RO_XYZ = new XYZ(PubEst[time].X - ROBA_Est2[3 * time + 0], PubEst[time].Y - ROBA_Est2[3 * time + 1], PubEst[time].Z - ROBA_Est2[3 * time + 2]);

            //    SW.Write(RO_XYZ.X.ToString("N", GN));
            //    SW.Write(" ");
            //    SW.Write(RO_XYZ.Y.ToString("N", GN));
            //    SW.Write(" ");
            //    SW.Write(RO_XYZ.Z.ToString("N", GN));
            //    SW.Write("\n");
            //}

            //SW.WriteLine("抗差中误差(t=3) = " + SigmaROBA3.ToString("N", GN));
            //SW.WriteLine("抗差估计时间(t=3) = " + TimeROBA3);
            //for (int time = 0; time < PointsAppXYZ.Count; time++)
            //{
            //    XYZ RO_XYZ = new XYZ(PubEst[time].X - ROBA_Est3[3 * time + 0], PubEst[time].Y - ROBA_Est3[3 * time + 1], PubEst[time].Z - ROBA_Est3[3 * time + 2]);

            //    SW.Write(RO_XYZ.X.ToString("N", GN));
            //    SW.Write(" ");
            //    SW.Write(RO_XYZ.Y.ToString("N", GN));
            //    SW.Write(" ");
            //    SW.Write(RO_XYZ.Z.ToString("N", GN));
            //    SW.Write("\n");
            //}

            SW.WriteLine("并行抗差中误差(t=1) = " + Parallel_SigmaROBA1.ToString("N", GN));
            SW.WriteLine("并行抗差估计时间(t=1) = " + Parallel_TimeROBA1);
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                XYZ RO_XYZ = new XYZ(PubEst[i].X - Parallel_ROBA_Est1[3 * i + 0], PubEst[i].Y - Parallel_ROBA_Est1[3 * i + 1], PubEst[i].Z - Parallel_ROBA_Est1[3 * i + 2]);

                SW.Write(RO_XYZ.X.ToString("N", GN));
                SW.Write(" ");
                SW.Write(RO_XYZ.Y.ToString("N", GN));
                SW.Write(" ");
                SW.Write(RO_XYZ.Z.ToString("N", GN));
                SW.Write("\n");
            }

            //SW.WriteLine("并行抗差中误差(t=2) = " + Parallel_SigmaROBA2.ToString("N", GN));
            //SW.WriteLine("并行抗差估计时间(t=2) = " + Parallel_TimeROBA2);
            //for (int time = 0; time < PointsAppXYZ.Count; time++)
            //{
            //    XYZ RO_XYZ = new XYZ(PubEst[time].X - Parallel_ROBA_Est2[3 * time + 0], PubEst[time].Y - Parallel_ROBA_Est2[3 * time + 1], PubEst[time].Z - Parallel_ROBA_Est2[3 * time + 2]);

            //    SW.Write(RO_XYZ.X.ToString("N", GN));
            //    SW.Write(" ");
            //    SW.Write(RO_XYZ.Y.ToString("N", GN));
            //    SW.Write(" ");
            //    SW.Write(RO_XYZ.Z.ToString("N", GN));
            //    SW.Write("\n");
            //}

            //SW.WriteLine("并行抗差中误差(t=3) = " + Parallel_SigmaROBA3.ToString("N", GN));
            //SW.WriteLine("并行抗差估计时间(t=3) = " + Parallel_TimeROBA3);
            //for (int time = 0; time < PointsAppXYZ.Count; time++)
            //{
            //    XYZ RO_XYZ = new XYZ(PubEst[time].X - Parallel_ROBA_Est3[3 * time + 0], PubEst[time].Y - Parallel_ROBA_Est3[3 * time + 1], PubEst[time].Z - Parallel_ROBA_Est3[3 * time + 2]);

            //    SW.Write(RO_XYZ.X.ToString("N", GN));
            //    SW.Write(" ");
            //    SW.Write(RO_XYZ.Y.ToString("N", GN));
            //    SW.Write(" ");
            //    SW.Write(RO_XYZ.Z.ToString("N", GN));
            //    SW.Write("\n");
            //}
            SW.Close();
            #endregion
            //int m =  DS_FilePath.Length;//只对日解文件进行抗差估计,包含多个日解文件
            ////string[] WholeFilepath = WS_FilePath + DS_FilePath;
            ////不同规模,每个文件代表一个规模
            //for (int time = 0; time < m; time++)
            //{
            //    BaseLineList = new List<XYZ>();
            //    PointsAppXYZ = new List<XYZ>();
            //    PointsEstimateXYZ = new List<XYZ>();
            //    BaseLineBlockList = new List<double[][]>();
            //    BaseLineCovBlockList = new List<double[][]>();
            //    BaseLineNameBlockList = new List<double[][]>();
            //    BasicInformation = new Geo.Algorithm.Adjust.AdjustBasicInformation();
            //    SinexFiles = null;
            //    //SinexNetWorkAdjustmentLLY(WS_FilePath, DS_FilePath, Priori_WS_FilePath, strKnownPoint, k0, k1, eps, rate, time);//只处理了日解文件
            //}
        }
        /// <summary>
        /// 抗差估计
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <param name="strKnownPoint"></param>
        /// <param name="sk0"></param>
        /// <param name="sk1"></param>
        /// <param name="eps"></param>
        /// <param name="rate">粗差率</param>
        /// <param name="time"></param>
        public SinexNetWorkAdjustmentLLY(string[] WS_FilePath, string[] DS_FilePath, string[] Priori_WS_FilePath, string[] strKnownPoint, double k0, double k1, double eps, double rate, int Time)
        {
            
            //string OriginFile = sFilePath;
            
            //从此实例检索子字符串。 子字符串从指定的字符位置开始且具有指定的长度。应该是把点名提取出来，只有snx文件点名
            //string strFileName = DS_FilePath[0].Substring(WS_FilePath[0].LastIndexOf('\\'), DS_FilePath[0].Length - DS_FilePath[0].LastIndexOf("\\"));//?????

            //int m = 7; //模拟计算7天
            //
            string[] FilePath = new string[Time];
            for (int i = 0; i < Time; i++)
            {
                FilePath[i] = DS_FilePath[i];
            }

            //获取已知点号信息
            for (int i = 0; i < strKnownPoint.Length; i++)
            {
                BasicInformation.KnownPointName.Add(strKnownPoint[i]);
            }


            //List<string> tmpFilePath = new List<string>();
            //for (int time = 0; time < FilePath.Length; time++)
            //{
            //    tmpFilePath.Add(FilePath[time]);
            //}
            //for (int j = 0; j < Priori_WS_FilePath.Length; j++)
            //{
            //    tmpFilePath.Add(Priori_WS_FilePath[j]);//默认是7个日解文件,再把上一周的周解文件加进来
            //}
            //string[] TmpFilePath = new string[tmpFilePath.Count];
            //for (int k = 0; k < tmpFilePath.Count; k++)
            //{
            //    TmpFilePath[k] = tmpFilePath[k];
            //}

            SinexFiles = SinexReader.Read(FilePath);//读取文件,每循环一次，读取的文件增加一次

            BasicInformation.FileNumber = SinexFiles.Count;

            //获取点基本信息,主要是多个snx文件的公共信息
            ConFirmPointBasicInformation(SinexFiles);
            //BasicInformation.TotalBaselineNumber = BasicInformation.TotalBaselineNumber - (BasicInformation.TotalPointNumber - 1);//总的基线向量个数                  
            //BasicInformation.FileNumber = BasicInformation.FileNumber - 1;//Sinex文件总数

            //建立基线信息：纯净的数据
            List<int[]> TotalBeginOfBaselineList = new List<int[]>();
            List<int[]> TotalEndOfBaselineList = new List<int[]>();
            double[][] bList = new double[BasicInformation.FileNumber][];
            double[][] pList = new double[BasicInformation.FileNumber][];

            BuildBaselineInformation(ref TotalBeginOfBaselineList, ref TotalEndOfBaselineList, ref bList, ref pList);

            //对观测值加粗差,即对bList基线向量坐标差修改
            double sigma = 0.002;//中误差
            AddGrossError(ref bList, rate, sigma);

            List<XYZ> PubEst = GetPubPointEstXYZ(WS_FilePath[0]);//这个originfile应该是周解文件,不能改变吧？？？？？

            double[] tmpXYZ = new double[PointsAppXYZ.Count * 3];
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                tmpXYZ[3 * i + 0] = PointsAppXYZ[i].X;
                tmpXYZ[3 * i + 1] = PointsAppXYZ[i].Y;
                tmpXYZ[3 * i + 2] = PointsAppXYZ[i].Z;
            }

            var SW_LS = Stopwatch.StartNew();

            //最小二乘解
            RobustBayesAdjustment LSAdjustment = new RobustBayesAdjustment(BasicInformation, tmpXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            LSAdjustment.LeastSquare();
            double[] LS_Est = LSAdjustment.PointsAppXYZ;
            double SigmaLS = LSAdjustment.sigma;
            string TimeLS = SW_LS.ElapsedMilliseconds.ToString();

            //抗差解
            var SW_RO1 = Stopwatch.StartNew();
            RobustBayesAdjustment ROAdjustment1 = new RobustBayesAdjustment(BasicInformation, tmpXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            ROAdjustment1.Robust(FactorYype.IGG3, eps, k0, k1, 2);
            double[] RO_Est1 = ROAdjustment1.PointsAppXYZ;
            double SigmaRO1 = ROAdjustment1.sigma;
            string TimeRO1 = SW_RO1.ElapsedMilliseconds.ToString();

            //抗差解
            var SW_RO2 = Stopwatch.StartNew();
            RobustBayesAdjustment ROAdjustment2 = new RobustBayesAdjustment(BasicInformation, tmpXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList);
            ROAdjustment2.Robust(FactorYype.IGG3, eps, k0, k1, 1);
            double[] RO_Est2 = ROAdjustment2.PointsAppXYZ;
            double SigmaRO2 = ROAdjustment2.sigma;
            string TimeRO2 = SW_RO2.ElapsedMilliseconds.ToString();
            
            //结果文件输出
            #region
            //string SavePath = "D:\\Test\\" + strFileName + Time.ToString() + "-" + rate.ToString() + "-Restult" + ".txt";
            string SavePath = "E:\\VS_Pro\\Test\\Robust_Bayes\\1721\\result\\Robust_Result" + "(" + k0 + "-" + k1 + " " + eps + ")" + ".txt";
            FileInfo aFile = new FileInfo(SavePath);
            StreamWriter SW = aFile.CreateText();
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;
            GN.NumberDecimalDigits = 6;

            SW.WriteLine(BasicInformation.TotalPointNumber.ToString());
            SW.WriteLine("LS中误差 = " + SigmaLS.ToString("N", GN));
            SW.WriteLine("LS串行估计时间 = " + TimeLS);
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                XYZ LS_XYZ = new XYZ(PubEst[i].X - LS_Est[3 * i + 0], PubEst[i].Y - LS_Est[3 * i + 1], PubEst[i].Z - LS_Est[3 * i + 2]);

                SW.Write(LS_XYZ.X.ToString("N", GN));
                SW.Write(" ");
                SW.Write(LS_XYZ.Y.ToString("N", GN));
                SW.Write(" ");
                SW.Write(LS_XYZ.Z.ToString("N", GN));
                SW.Write("\n");
            }

            SW.WriteLine("抗差1中误差 = " + SigmaRO1.ToString("N", GN));
            SW.WriteLine("抗差1估计时间 = " + TimeRO1);
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                XYZ RO_XYZ = new XYZ(PubEst[i].X - RO_Est1[3 * i + 0], PubEst[i].Y - RO_Est1[3 * i + 1], PubEst[i].Z - RO_Est1[3 * i + 2]);

                SW.Write(RO_XYZ.X.ToString("N", GN));
                SW.Write(" ");
                SW.Write(RO_XYZ.Y.ToString("N", GN));
                SW.Write(" ");
                SW.Write(RO_XYZ.Z.ToString("N", GN));
                SW.Write("\n");
            }

            SW.WriteLine("抗差2中误差 = " + SigmaRO2.ToString("N", GN));
            SW.WriteLine("抗差2估计时间 = " + TimeRO2);
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                XYZ RO_XYZ = new XYZ(PubEst[i].X - RO_Est2[3 * i + 0], PubEst[i].Y - RO_Est2[3 * i + 1], PubEst[i].Z - RO_Est2[3 * i + 2]);

                SW.Write(RO_XYZ.X.ToString("N", GN));
                SW.Write(" ");
                SW.Write(RO_XYZ.Y.ToString("N", GN));
                SW.Write(" ");
                SW.Write(RO_XYZ.Z.ToString("N", GN));
                SW.Write("\n");
            }
            SW.Close();
            #endregion
        }

        /// <summary>
        /// 贝叶斯估计,要读取先验信息
        /// </summary>
        /// <param name="WS_FilePath">周解文件</param>
        /// <param name="DS_FilePath">日解文件</param>
        /// <param name="Priori_WS_FilePath">先验信息,即上周的周解文件</param>
        /// <param name="KnownPoint"></param>
        public SinexNetWorkAdjustmentLLY(string[] WS_FilePath, string[] DS_FilePath, string[] Priori_WS_FilePath, string[] KnownPoint)
        {
            for (int i = 0; i < KnownPoint.Length; i++)
            {
                BasicInformation.KnownPointName.Add(KnownPoint[i]);//先读取已知跟踪站
            }

            List<string> tmpFilePath = new List<string>();
            for (int i = 0; i < DS_FilePath.Length; i++)
            {
                tmpFilePath.Add(DS_FilePath[i]);
            }
            for (int j = 0; j < Priori_WS_FilePath.Length;j++ )
            {
                tmpFilePath.Add(Priori_WS_FilePath[0]);//默认是7个日解文件,再把上一周的周解文件加进来
            }
            string[] TmpFilePath = new string[tmpFilePath.Count];
            for (int k = 0; k < tmpFilePath.Count;k++ )
            {
                TmpFilePath[k] = tmpFilePath[k];
            }

            SinexFiles = SinexReader.Read(TmpFilePath);//把日解和上周周解文件读进来
            BasicInformation.FileNumber = SinexFiles.Count;
            ConFirmPointBasicInformation(SinexFiles);//获取点基本信息

            BasicInformation.TotalBaselineNumber = BasicInformation.TotalBaselineNumber - (BasicInformation.TotalPointNumber - 1);//总的基线向量个数                  
            BasicInformation.FileNumber = BasicInformation.FileNumber - 1;//Sinex文件总数

            //建立基线
            List<int[]> TotalBeginOfBaselineList = new List<int[]>();//起点点号的索引,长度是总点数减1
            List<int[]> TotalEndOfBaselineList = new List<int[]>();//终点点号的索引,长度是总点数减1
            double[][] bList = new double[BasicInformation.FileNumber][];//基线向量的坐标差,（总点数-1）*3长度1302
            double[][] pList = new double[BasicInformation.FileNumber][];//L的协方差矩阵

            //建立基线信息:基线向量坐标差,bList基线向量的协方差矩阵pList,基线向量的起始点TotalBeginOfBaselineList和TotalEndOfBaselineList
            BuildCleanBaselineInformation(ref TotalBeginOfBaselineList, ref TotalEndOfBaselineList, ref bList, ref pList);

            ///////
            //建立先验信息,先验协方差矩阵和先验坐标值
            double[] Priori_X = new double[3 * BasicInformation.TotalPointNumber];
            double[] Priori_CovX = new double[3 * BasicInformation.TotalPointNumber * (3 * BasicInformation.TotalPointNumber + 1) / 2];
            PriorSinexFile = SinexReader.Read(Priori_WS_FilePath);
            BuildPriorInformation(ref Priori_CovX, ref Priori_X);//正确
            ///////

            string WeekSolution_Path = WS_FilePath[0];
            //获得公共点坐标的估值,该估值是1721周周解的坐标估值
            List<XYZ> PubEst = GetPubPointEstXYZ(WeekSolution_Path);

            //PointsAppXYZ是加入了随机数之后的坐标
            double[] temXYZ = new double[PointsAppXYZ.Count * 3];//435*3
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                temXYZ[3 * i + 0] = PointsAppXYZ[i].X;
                temXYZ[3 * i + 1] = PointsAppXYZ[i].Y;
                temXYZ[3 * i + 2] = PointsAppXYZ[i].Z;
            }

            var SW_Bayes = Stopwatch.StartNew();//对新的 Stopwatch 实例进行初始化，将运行时间属性设置为零，然后开始测量运行时间。

            //最小二乘解
            RobustBayesAdjustment BayesAdjustment = new RobustBayesAdjustment(BasicInformation, temXYZ, TotalBeginOfBaselineList, TotalEndOfBaselineList, bList, pList, Priori_X, Priori_CovX);
            BayesAdjustment.BayesEstimate();
            double[] Bayes_Est = BayesAdjustment.PointsAppXYZ;
            double Sigma_Bayes = BayesAdjustment.sigma;
            string Time_Bayes = SW_Bayes.ElapsedMilliseconds.ToString();

            //输出结果
            #region
            //设置保存路径
            string SavePath = "E:\\VS_Pro\\Test\\Robust_Bayes\\1721\\result\\Bayes_Result" + ".txt";

            FileInfo aFile = new FileInfo(SavePath);
            StreamWriter SW = aFile.CreateText();//FileInfo的CreateText方法创建写入新文本文件的 StreamWriter

            //CultureInfo:提供有关特定区域性的信息（对于非托管代码开发，则称为“区域设置”）。 这些信息包括区域性的名称、书写系统、使用的日历以及对日期和排序字符串的格式化设置。
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;// System.Globalization.NumberFormatInfo根据区域性定义如何设置数值格式以及如何显示数值
            GN.NumberDecimalDigits = 6; //小数点后6位
            for (int i = 0; i < PointsAppXYZ.Count; i++)
            {
                XYZ BayesXYZ = new XYZ(PubEst[i].X - Bayes_Est[3 * i + 0], PubEst[i].Y - Bayes_Est[3 * i + 1], PubEst[i].Z - Bayes_Est[3 * i + 2]);
                SW.Write(BayesXYZ.X.ToString("N", GN));
                SW.Write(" ");
                SW.Write(BayesXYZ.Y.ToString("N", GN));
                SW.Write(" ");
                SW.Write(BayesXYZ.Z.ToString("N", GN));
                SW.Write("\n");
            }
            SW.Write(Time_Bayes);
            SW.Write("\n");
            SW.Write(Sigma_Bayes);
            SW.Close();
            #endregion
        }
        /// <summary>
        /// 获取sinex的基本信息
        /// 公共的总点数：TotalPointNumber
        /// 基线向量总数：TotalBaselineNumber
        /// 未知点数：UnknowPointnumber
        /// 所有点的编号：TotalPointName
        /// 点坐标：PointsAppXYZ已加入了随机误差
        /// </summary>
        /// <param name="SinexFiles"></param>
        private void ConFirmPointBasicInformation(List<SinexFile> SinexFiles)
        {
            if(SinexFiles.Count < 1) { throw new Exception("sinex文件个数为0!");}
            
            //提取公共点部分
            SinexFile item0 = SinexFiles[0];//将第一个sinex文件作为处置
            BasicInformation.TotalPointNumber = 0;//总点数清零
            List<string> site0Cods = item0.GetSiteCods(); //对应于总点数,对1721周解文件来说是435
            //定义一个临时array数组
            double[] array0 = item0.GetEstimateVector(); //待估参数的个数,在Header中出现,对1721周解文件来说是1343
            for (int i = 0; i < site0Cods.Count; i++)
            {
                string tmp = site0Cods[i];//第一个周解文件的IGS跟踪站的点名
                bool IsContain = true;//要求所有的IGS跟踪站必须含有该点
                if (SinexFiles.Count > 1)
                {
                    for (int j = 1; j < SinexFiles.Count; j++)
                    {
                        //判断是不是所有的IGS跟踪站都有该点
                        List<string> siteCods = SinexFiles[j].GetSiteCods();
                        if (!siteCods.Contains(tmp)) IsContain = false;
                    }
                }
                if (IsContain)
                {
                    BasicInformation.TotalPointNumber += 1;//总点数+1
                    BasicInformation.TotalPointName.Add(site0Cods[i]);//点名更新,再加一个点名
                    XYZ xyz0 = new XYZ(array0[3 * i + 0], array0[3 * i + 1], array0[3 * i + 2]);//对应于SOLUTION/ESTIMATE中跟踪站的坐标值,STAX,STAY,STAZ
                    PointsAppXYZ.Add(xyz0);//实际上这个地方是坐标真值了,即estimate
                }     
            }

            BasicInformation.TotalBaselineNumber = (BasicInformation.TotalPointNumber - 1) * SinexFiles.Count;//总的基线向量个数
            BasicInformation.TotalPointNumber = BasicInformation.TotalPointName.Count; //总点数,这一句话不用要
            BasicInformation.UnknowPointnumber = BasicInformation.TotalPointNumber - BasicInformation.KnownPointName.Count;//未知点数目
            BasicInformation.FileNumber = SinexFiles.Count;//Sinex文件总数
            IsKnownPoint = new bool[BasicInformation.TotalPointNumber];//?????

            //判断哪个点是已知点
            for (int i = 0 ; i < BasicInformation.TotalPointNumber; i++)
            {
                if(BasicInformation.KnownPointName.Contains(BasicInformation.TotalPointName[i]))
                {
                    IsKnownPoint[i] = true;
                }
                else 
                {
                    IsKnownPoint[i] = false;
                }
            }

            //坐标近似值，默认第一个坐标为已知点,其他的加上随机小数,第一个点的坐标真值在周解文件中给出,参见GetPubPointEstXYZ
            Random seed1 = new Random();
            Random seed2 = new Random();
            Random seed3 = new Random();
            int[] r1 = new int[BasicInformation.TotalPointNumber - 1];
            int[] r2 = new int[BasicInformation.TotalPointNumber - 1];
            int[] r3 = new int[BasicInformation.TotalPointNumber - 1];
            for (int i = 1; i < BasicInformation.TotalPointNumber; i++)
            {
                r1[i - 1] = 10000 - seed1.Next(0, 20000);//(-1000,1000)之间的随机数
                PointsAppXYZ[i].X = PointsAppXYZ[i].X + r1[i - 1] * Math.Pow(10, -5);//(-0.1,0.1)之间的随机数                  
            
                r2[i - 1] = 10000 - seed1.Next(0, 20000);//(-1000,1000)之间的随机数
                PointsAppXYZ[i].Y = PointsAppXYZ[i].Y + r2[i - 1] * Math.Pow(10, -5);//(-0.1,0.1)之间的随机数                
            
                r3[i - 1] = 10000 - seed1.Next(0, 20000);//(-1000,1000)之间的随机数
                PointsAppXYZ[i].Z = PointsAppXYZ[i].Z + r3[i - 1] * Math.Pow(10, -5);//(-0.1,0.1)之间的随机数     
            }
        }

        /// <summary>
        /// 读取先验信息的坐标值和协方差矩阵
        /// </summary>
        /// <param name="Priori_CovX">先验协方差矩阵</param>
        /// <param name="Priori_X">先验坐标值</param>
        private void BuildPriorInformation(ref double[] Priori_CovX, ref double[] Priori_X)
        {
            double[][] Priori_estimateCovX = PriorSinexFile[0].GetEstimateCovaMatrix();
            double[] Priori_estimateX = PriorSinexFile[0].GetEstimateVector();

            //扣除非公共部分,只要与基线向量有关的坐标
            double[][] cleaned = GetCleanedMatrix(PriorSinexFile[0]);

            ArrayMatrix Priori_XX = new ArrayMatrix(MatrixUtil.GetMatrix(Priori_estimateX));//????
            ArrayMatrix Priori_CovXX = new ArrayMatrix(Priori_estimateCovX );//????

            ArrayMatrix CleanedMatrix = new ArrayMatrix(cleaned);//????

            Priori_XX = CleanedMatrix * Priori_XX;
            Priori_CovXX = CleanedMatrix * Priori_CovXX * CleanedMatrix.Transpose();//1304*1304

            for (int i = 0; i < Priori_XX.RowCount; i++)
            {
                Priori_X[i] = Priori_XX[i, 0];
            }

            int n = 0;
            for(int j = 0; j < Priori_XX.RowCount; j++)
            {
                for(int k = 0; k <= j; k++)
                {
                    Priori_CovX[n] = Priori_CovXX[j, k];
                    n++;
                }  
            }

        }
        /// <summary>
        /// 建立基线信息,输出：基线起点的索引（数量：总点数-1）、基线终点的索引（数量：总点数-1）、建立的基线向量（（数量：总点数-1）*3）、基线向量的协方差矩阵（基线向量数*（基线向量数-1）/2）
        /// 不引入粗差
        /// </summary>
        /// <param name="TotalBeginOfBaselineList"></param>
        /// <param name="TotalEndOfBaselineList"></param>
        /// <param name="bList"></param>
        /// <param name="pList"></param>
        private void BuildCleanBaselineInformation(ref List<int[]> TotalBeginOfBaselineList, ref List<int[]> TotalEndOfBaselineList, ref double[][] bList, ref double[][] pList)
        {
            //计算基线向量个数
            int count = 0;
            for (int i = 0; i < BasicInformation.FileNumber; i++)//原来是i < SinexFiles.Count,为了估计贝叶斯估计才改的
            {
                //保存测站点号
                List<string> SiteName = SinexFiles[i].GetSiteCods();
                int n = BasicInformation.TotalPointNumber; //测站总数
                //每个文件的向量起点取第一个出现的点,其余为独立观测向量的重点
                int[] BegDir = new int[n - 1];//基线向量数,n-1
                int[] EndDir = new int[n - 1];
                int s = 0;
                for (int k = 0; k < BasicInformation.TotalPointName.Count; k++)
                {
                    if (k != i)
                    {
                        string strB = BasicInformation.TotalPointName[i];
                        if (BasicInformation.TotalPointName.Contains(strB))
                        {
                            int tmp = BasicInformation.TotalPointName.IndexOf(strB);
                        }
                        BegDir[s] = BasicInformation.TotalPointName.IndexOf(BasicInformation.TotalPointName[i]);//对一个文件来说,k和s增加,i没有增加
                        EndDir[s] = BasicInformation.TotalPointName.IndexOf(BasicInformation.TotalPointName[k]);
                        s += 1;
                    }
                }
                TotalBeginOfBaselineList.Add(BegDir);
                TotalEndOfBaselineList.Add(EndDir);

                //生成公共点部分的基线向量并保存向量坐标差
                double[] Estimate_L = SinexFiles[i].GetEstimateVector();//参数估值,header文件里面有
                double[][] EstimateCov_L = SinexFiles[i].GetEstimateCovaMatrix();//第一次在这里运行出错,因为igs13p1721缺省,索引从0开始,1343*1343

                //扣除非公共部分,只要与基线向量有关的坐标
                double[][] cleaned = GetCleanedMatrix(SinexFiles[i]);

                ArrayMatrix L = new ArrayMatrix(MatrixUtil.GetMatrix(Estimate_L));//????
                ArrayMatrix CovL = new ArrayMatrix(EstimateCov_L);//????

                ArrayMatrix CleanedMatrix = new ArrayMatrix(cleaned);//????

                //double[,] L_Del = new double[Estimate_L.Length,CleanedMatrix.Columns];
                //for (int ii = 0; ii < Estimate_L.Length; ii++ )
                //{
                //    for (int jj = 0; jj < CleanedMatrix.Columns; jj++)
                //    {
                //        if (jj == ii)
                //        {
                //            L_Del[ii,jj] = 1;
                //        }
                //        else
                //        {
                //            L_Del[ii,jj] = 0;
                //        }
                        
                //    }
                //}
                //L = L * L_Del;
                L = CleanedMatrix * L;
                CovL = CleanedMatrix * CovL * CleanedMatrix.Transpose();//1304*1304

                //建立基线向量

                double[][] CoefficientOfL = new double[3 * (n - 1)][];
                int ss = 0;
                for (int j = 0; j < (n); j++)
                {
                    if (j != i)
                    {
                        CoefficientOfL[3 * ss + 0] = new double[3 * n];
                        CoefficientOfL[3 * ss + 1] = new double[3 * n];
                        CoefficientOfL[3 * ss + 2] = new double[3 * n];

                        CoefficientOfL[3 * ss + 0][3 * i + 0] = -1;//每一行的第0 3,1 4, 2 5个元素分别为-1和1，第0、1、2个元素固定为-1
                        CoefficientOfL[3 * ss + 0][3 * j + 0] = 1;

                        CoefficientOfL[3 * ss + 1][3 * i + 1] = -1;
                        CoefficientOfL[3 * ss + 1][3 * j + 1] = 1;

                        CoefficientOfL[3 * ss + 2][3 * i + 2] = -1;
                        CoefficientOfL[3 * ss + 2][3 * j + 2] = 1;
                        ss += 1;
                    }
                }
                ArrayMatrix CoeffiA = new ArrayMatrix(CoefficientOfL);
                ArrayMatrix Baseline = CoeffiA * L;//Baseline:1302*1,基线共有434条,加上xyz,就是1302

                double[] li = new double[Baseline.RowCount];
                for (int k = 0; k < li.Length; k++)
                {
                    li[k] = Baseline[k, 0];//获得基线向量,都是和第一个点的坐标值作差
                }
                bList[i] = (li); 

                ArrayMatrix BaselineCov = CoeffiA * CovL * CoeffiA.Transpose();//求出基线向量的协方差矩阵了
                int ni = BaselineCov.RowCount;//1302
                double[] pi = new double[(ni + 1) * ni / 2];//1302*1303/2=848253,但是索引从0开始，结束是848252
                int sk = 0;
                for (int s1 = 0; s1 < ni; s1++)
                {
                    for (int s2 = 0; s2 <= s1; s2++)
                    {
                        pi[sk] = BaselineCov[s1, s2];
                        sk++;
                    }
                }

                pList[i] = (pi);
                count += n - 1;
                    
            }
        }

        /// <summary>
        /// 抗差估计里面建立带有观测误差的基线
        /// </summary>
        /// <param name="TotalBeginOfBaselineList"></param>
        /// <param name="TotalEndOfBaselineList"></param>
        /// <param name="bList"></param>
        /// <param name="pList"></param>
        private void BuildBaselineInformation(ref List<int[]> TotalBeginOfBaselineList, ref List<int[]> TotalEndOfBaselineList, ref double[][] bList, ref double[][] pList)
        {
            int count = 0;//计算基线向量个数
            for (int i = 0; i < BasicInformation.FileNumber; i++)
            {
                //保存基线向量的点号
                List<string> SiteName = SinexFiles[i].GetSiteCods();

                int n = BasicInformation.TotalPointNumber;
                //每个文件的向量起点取第一个出现的点,剩余的为独立观测向量的终点
                int[] BegDir = new int[n - 1];
                int[] EndDir = new int[n - 1];
                int s = 0;

                for (int k = 0; k < BasicInformation.TotalPointName.Count; k++)
                {
                    if (k != i)
                    {
                        string strB = BasicInformation.TotalPointName[i];
                        if(BasicInformation.TotalPointName.Contains(strB))
                        {
                            int tmp = BasicInformation.TotalPointName.IndexOf(strB);
                        }
                        BegDir[s] = BasicInformation.TotalPointName.IndexOf(BasicInformation.TotalPointName[i]);
                        EndDir[s] = BasicInformation.TotalPointName.IndexOf(BasicInformation.TotalPointName[k]);
                        s += 1;
                    }
                }
                TotalBeginOfBaselineList.Add(BegDir);
                TotalEndOfBaselineList.Add(EndDir);

                //生成公共点部分的基线向量并保存坐标差
                double[] EstimateL = SinexFiles[i].GetEstimateVector();
                double[][] EstimateCovL = SinexFiles[i].GetEstimateCovaMatrix();

                //扣除非公共部分

                double[][] cleaned = GetCleanedMatrix(SinexFiles[i]);

                ArrayMatrix L = new ArrayMatrix(MatrixUtil.GetMatrix(EstimateL));
                ArrayMatrix CovL = new ArrayMatrix(EstimateCovL);

                ArrayMatrix CleanedMatrix = new ArrayMatrix(cleaned);

                L = CleanedMatrix * L;
                CovL = CleanedMatrix * CovL * CleanedMatrix.Transpose();

                //建立基线向量
                double[][] CoefficientOfL = new double[3 * (n - 1)][];
                int ss = 0;
                for (int j = 0; j < (n); j++)
                {
                    if (j != i)
                    {
                        CoefficientOfL[3 * ss + 0] = new double[3 * n];
                        CoefficientOfL[3 * ss + 1] = new double[3 * n];
                        CoefficientOfL[3 * ss + 2] = new double[3 * n];

                        CoefficientOfL[3 * ss + 0][3 * i + 0] = -1;//每一行的第0 3,1 4, 2 5个元素分别为-1和1，第0、1、2个元素固定为-1
                        CoefficientOfL[3 * ss + 0][3 * j + 0] = 1;

                        CoefficientOfL[3 * ss + 1][3 * i + 1] = -1;
                        CoefficientOfL[3 * ss + 1][3 * j + 1] = 1;

                        CoefficientOfL[3 * ss + 2][3 * i + 2] = -1;
                        CoefficientOfL[3 * ss + 2][3 * j + 2] = 1;
                        ss += 1;
                    }
                }
                ArrayMatrix CoeffiA = new ArrayMatrix(CoefficientOfL);
                ArrayMatrix Baseline = CoeffiA * L;//Baseline:1302*1,基线共有434条,加上xyz,就是1302

                #region 基线向量中加入观测误差,最大是4mm
                //观测值L中加入3倍中误差？加入？%
                int tmpcount = Baseline.RowCount;
                double sigma0 = 0.002;
                Random seed = new Random();
                
                for (int k = 0; k < tmpcount; k++)
                {                    
                    int error = seed.Next(000, 100);

                    double ranError = sigma0 * error / 100;
                    int IsD = Convert.ToInt32(Math.IEEERemainder(k,2));//返回一数字被另一数字相除的余数
                    if (IsD == 0)//偶数行加上,奇数行减去
                    {
                        Baseline[k, 0] += ranError;
                    }
                    else
                    {
                        Baseline[k, 0] -= ranError;
                    }
                }
                #endregion
                double[] li = new double[Baseline.RowCount];
                for (int k = 0; k < li.Length; k++)
                {
                    li[k] = Baseline[k, 0];//获得基线向量,都是和第一个点的坐标值作差
                }
                bList[i] = (li);

                ArrayMatrix BaselineCov = CoeffiA * CovL * CoeffiA.Transpose();//求出基线向量的协方差矩阵了
                int ni = BaselineCov.RowCount;//1302
                double[] pi = new double[(ni + 1) * ni / 2];//1302*1303/2=848253,但是索引从0开始，结束是848252
                int sk = 0;
                for (int s1 = 0; s1 < ni; s1++)
                {
                    for (int s2 = 0; s2 <= s1; s2++)
                    {
                        pi[sk] = BaselineCov[s1, s2];//用一维数组保存基线向量的协方差
                        sk++;
                    }
                }
                pList[i] = (pi);//基线向量协方差矩阵

                count += n - 1;//每个文件的独立基线向量=点数-1
            }
        }
        /// <summary>
        /// 返回多个文件干净的协方差矩阵
        /// </summary>
        /// <param name="snx"></param>
        /// <returns></returns>
        private double[][] GetCleanedMatrix(SinexFile snx)
        {
            List<int> TobeRemoveIndexs = new List<int>();
            int count = BasicInformation.TotalPointNumber;
            List<string> SiteCode = snx.GetSiteCods();
            for (int i = 0; i < SiteCode.Count; i++)
            {
                string tmp = SiteCode[i];
                if (!BasicInformation.TotalPointName.Contains(tmp))
                {
                    TobeRemoveIndexs.Add(i * 3 + 0);
                    TobeRemoveIndexs.Add(i * 3 + 1);
                    TobeRemoveIndexs.Add(i * 3 + 2);

                }
            }
            double[][] CovMatrix = MatrixUtil.CreateIdentity(SiteCode.Count * 3);
            double[][] CleanedCovMatrix = MatrixUtil.ShrinkMatrixRow(CovMatrix, TobeRemoveIndexs);

            return CleanedCovMatrix;
        }

        /// <summary>
        /// 获得周解文件的坐标,当作坐标真值
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private List<XYZ> GetPubPointEstXYZ(string FilePath)
        {
            SinexFile PubSinexFile = SinexReader.Read(FilePath);
            List<string> PubSiteCode = PubSinexFile.GetSiteCods();//获得测站的编码，对1722周周解文件来说,count=442，对1721周周解文件来说,count=435
            double[] PubAllEst = PubSinexFile.GetEstimateVector();//获得估值数量=总点数*3（坐标x、y、z），对1721周周解文件来说=1305
            for (int i = 0; i < BasicInformation.TotalPointNumber; i++)
            {
                string item = BasicInformation.TotalPointName[i];
                for(int j = 0; j < PubSiteCode.Count; j++)
                {
                    if(PubSiteCode[j] == item)
                    {
                        XYZ xyz = new XYZ();
                        xyz.X = PubAllEst[3 * j + 0];
                        xyz.Y = PubAllEst[3 * j + 1];
                        xyz.Z = PubAllEst[3 * j + 2];
                        PointsEstimateXYZ.Add(xyz);
                        break;
                    }                    
                }                
            }

            List<XYZ> PubXYZ = new List<XYZ>();
            for (int i = 0; i < PointsEstimateXYZ.Count; i++)
            {
                PubXYZ.Add(PointsEstimateXYZ[i]);
            }
            //对于第一个点，坐标真值赋给它，作为已知点
            PointsAppXYZ[0] = PointsEstimateXYZ[0];
            return PubXYZ;

        }

        /// <summary>
        /// 添加粗差
        /// </summary>
        /// <param name="bList"></param>
        /// <param name="rate">观测数据中计入粗差的比例</param>
        /// <param name="sigma">观测值中误差</param>
        private static void AddGrossError(ref double[][] bList, double rate, double sigma)
        {
            int row = bList.Length;//????bList不是二维的吗？多维数组length就是所有维数元素的总数

            double sigma0 = sigma;//最后都删了
            Random seed1 = new Random();
            Random seed2 = new Random();

            for (int i = 0; i < row; i++)
            {
                int obs = bList[i].Length;
                int intError = Convert.ToInt32(obs * rate);//在百分之几rate的观测数据中加入粗差
                //int k = 100;
                //for(int j =0; j<10;j++)
                //{
                //    bList[time][k] += sigma0 * 4;
                //    k = k + 100;
                //}    
                
                int[] r1 = new int[intError];
                int[] error = new int[intError];

                for (int k = 0; k < intError; k++)
                {
                    r1[k] = seed1.Next(0, obs);
                    error[k] = seed2.Next(2000, 6000);//粗差的大小可以改,这里对应的是3~6倍的误差

                    int IsD = Convert.ToInt32(Math.IEEERemainder(r1[k], 2));

                    if (IsD == 0)//偶数行加上,奇数行减去
                    {
                        bList[i][r1[k]] += sigma0 * error[k] / 1000;
                    }
                    else
                    {
                        bList[i][r1[k]] -= sigma0 * error[k] / 1000;
                    }
                }
            }
        }
    }
}
