using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Algorithm.Adjust;
using Gnsser.Data.Sinex;
using Geo.Algorithm;
//using Geo.Algorithm.CyMatrix;
using System.Diagnostics;//Added for the stopwatch
using System.Threading.Tasks;

namespace Gnsser.Service
{
    /// <summary>
    /// GNSS网平差类，涉及最小二乘平差、自由网平差、抗差估计等。
    /// 观测值是基于Sinex文件保存的基线向量，多个同步网（SINEX文件）的联合网平差
    /// Cui Yang
    /// 2013.12.03
    /// </summary>
    public class SinexNetworksAdjust
    {
        #region 公共参数的定义
        AdjustBasicInformation BasicInformation = new AdjustBasicInformation();

         
        /// <summary>
        /// 基线观测向量
        /// 存储所有的观测向量,每个基线向量三个分量构成一个XYZ
        /// </summary>
        public List<XYZ> BaselinesList = new List<XYZ>();
        /// <summary>
        /// 点坐标近似值
        /// 坐标数组，三个坐标构成一个XYZ，顺序与点号一一对应
        /// </summary>
        public List<XYZ> PointsXYZ = new List<XYZ>();

        /// <summary>
        /// 点坐标真值
        /// 坐标数组，三个坐标构成一个XYZ，顺序与点号一一对应
        /// </summary>
        public List<XYZ> PointsTrueXYZ = new List<XYZ>();


        /// <summary>
        /// 是否是已知点
        /// </summary>
        public bool[] IsKnownPoint { get; set; }

        ///// <summary>
        ///// 向量起始点点号
        ///// </summary>
        //public int[] BeginPointDir = null;
        ///// <summary>
        ///// 向量终点点号
        ///// </summary>
        //public int[] EndPointDir = null;


        //每个文件单独存储基线向量、协方差矩阵、基线向量点号
        /// <summary>
        /// 基线向量，3m*1一维的矩阵块列，m=点号
        /// </summary>
        public List<double[][]> BaselineBlockList = new List<double[][]>();
        /// <summary>
        /// 基线向量协方差矩阵列，3m*3m的方阵
        /// </summary>
        public List<double[][]> BaselineCovBlockList = new List<double[][]>();
        /// <summary>
        /// 基线向量点号，m*2维的矩阵列块，两列矩阵，首列是起点点号的序列，第二列是终点点号的序列
        /// </summary>
        public List<double[][]> BaselineNameBlcokList = new List<double[][]>();

        List<SinexFile> sinexFiles = null;

        ////
        //public double eps = 0.005;
        #endregion

        public SinexNetworksAdjust(string[] filePath, string[] strKnowPoint)
        {

            //获取已知点点号信息
            for (int i = 0; i < strKnowPoint.Length; i++)
            {
                BasicInformation.KnownPointName.Add(strKnowPoint[i]);
            }
            //
            sinexFiles = SinexReader.Read(filePath);
            BasicInformation.FileNumber = sinexFiles.Count;
            ConfirmPointsBasicInformation(sinexFiles);//获取点基本信息

            //建立基线信息
            // bulit(); 
            List<int[]> totalbeginOfBaselineList =new List<int[]>();
            List<int[]> totalendOfBaselineList= new List<int[]>();
            double[][] bList = new double[BasicInformation.FileNumber][];
            double[][] pList = new double[BasicInformation.FileNumber][];
            bulitCleanBaselineInfomation(ref  totalbeginOfBaselineList, ref totalendOfBaselineList, ref bList, ref pList);//只建立公共点部分

           // PointsTrueXYZ = PointsXYZ;
            string fileWeekPath = "D:\\Test\\igs13p1721.SNX";
            List<XYZ> pubEst = GetPubPointEstXYZ(fileWeekPath);//获取周解

            double[] tmpXYZ = new double[PointsXYZ.Count * 3];
            for (int i = 0; i < PointsXYZ.Count; i++)
            {
                tmpXYZ[3 * i + 0] = PointsXYZ[i].X;
                tmpXYZ[3 * i + 1] = PointsXYZ[i].Y;
                tmpXYZ[3 * i + 2] = PointsXYZ[i].Z;
            }
             

            var swLS = Stopwatch.StartNew();
            ////最小二乘解
            RobustAdjustment LSAdjustment = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);          
            LSAdjustment.LeastSquares();
            double[] LsEst = LSAdjustment.pointsXYZ;
            double sigmaLS = LSAdjustment.Sigma;
            string timeLS = swLS.Elapsed.ToString();

            var swLS1 = Stopwatch.StartNew();
            RobustAdjustment LSAdjustment1 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);         
            LSAdjustment1.Parallel_LeastSquares();
            double[] LsEst1 = LSAdjustment1.pointsXYZ;
            double sigmaLS1 = LSAdjustment1.Sigma;
            string timeLS1= swLS1.Elapsed.ToString();
        


            //输出结果
            #region
            string savePath = "D:\\Test\\Restult" + ".txt";
            FileInfo aFile = new FileInfo(savePath);
            StreamWriter sw = aFile.CreateText();
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;
            GN.NumberDecimalDigits = 6;//小数点6位
            for (int i = 0; i < PointsXYZ.Count; i++)
            {
                XYZ LsXYZ = new XYZ(pubEst[i].X - LsEst[3 * i + 0], pubEst[i].Y - LsEst[3 * i + 1], pubEst[i].Z - LsEst[3 * i + 2]);
                 
                sw.Write(LsXYZ.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(LsXYZ.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(LsXYZ.Z.ToString("N", GN)); 
                sw.Write("\n");
            }
            sw.Close();
            #endregion

            
        }


        public SinexNetworksAdjust(string[] filePath)
        {

            StreamReader sr = new StreamReader(filePath[0]);
            SortedDictionary<string, XYZ> prioriXyzDictorary = new SortedDictionary<string, XYZ>();
            List<string> stationNameList = new List<string>();

            string line = sr.ReadLine();
            int count = Convert.ToInt32(line);
            for (int i = 0; i < count; i++)
            {
                line = sr.ReadLine();
                string[] cha2 = line.Split(new Char[] { ' ', '\t' });
                string stationName = cha2[2].Trim().ToLower();
                XYZ xyz = new XYZ(Convert.ToDouble(cha2[5]), Convert.ToDouble(cha2[6]), Convert.ToDouble(cha2[7]));
                prioriXyzDictorary.Add(stationName, xyz);
            }


            SortedDictionary<string, XYZ> appriXyzDictorary = new SortedDictionary<string, XYZ>();
            line = sr.ReadLine();
            count = Convert.ToInt32(line);
            for (int i = 0; i < count; i++)
            {
                line = sr.ReadLine();
                string[] cha2 = line.Split(new Char[] { ' ', '\t' });
                string stationName = cha2[2].Trim().ToLower();
                XYZ xyz = new XYZ(Convert.ToDouble(cha2[5]), Convert.ToDouble(cha2[6]), Convert.ToDouble(cha2[7]));
                appriXyzDictorary.Add(stationName, xyz);
                stationNameList.Add(stationName);
            }

            stationNameList.Sort();

            line = sr.ReadLine();
            count = Convert.ToInt32(line);

            Geo.Algorithm.IMatrix A = new ArrayMatrix(count * 3, (count + 1) * 3);
            Geo.Algorithm.IMatrix L = new ArrayMatrix(count * 3, 1);
            Geo.Algorithm.IMatrix Q = new ArrayMatrix(count * 3, count * 3);

            for (int i = 0; i < count; i++)
            {

                line = sr.ReadLine();
                string[] cha2 = line.Split(new Char[] { ' ', '\t' });

                string begStationName = cha2[0].Trim().ToLower();
                string endStationName = cha2[1].Trim().ToLower();
                XYZ DetXyz = new XYZ(Convert.ToDouble(cha2[2]), Convert.ToDouble(cha2[3]), Convert.ToDouble(cha2[4]));

                int begin = stationNameList.IndexOf(begStationName);
                int end = stationNameList.IndexOf(endStationName);

                A[3 * i + 0, begin * 3 + 0] = -1; A[3 * i + 0, end * 3 + 0] = 1; L[3 * i + 0, 0] = DetXyz.X -(appriXyzDictorary[endStationName].X - appriXyzDictorary[begStationName].X);
                A[3 * i + 1, begin * 3 + 1] = -1; A[3 * i + 1, end * 3 + 1] = 1; L[3 * i + 1, 0] = DetXyz.Y -(appriXyzDictorary[endStationName].Y - appriXyzDictorary[begStationName].Y);
                A[3 * i + 2, begin * 3 + 2] = -1; A[3 * i + 2, end * 3 + 2] = 1; L[3 * i + 2, 0] = DetXyz.Z -(appriXyzDictorary[endStationName].Z - appriXyzDictorary[begStationName].Z);

                line = sr.ReadLine();
                cha2 = line.Split(new Char[] { ' ', '\t' });
                Q[3 * i + 0, 3 * i + 0] = Convert.ToDouble(cha2[0]);

                line = sr.ReadLine();
                cha2 = line.Split(new Char[] { ' ', '\t' });
                Q[3 * i + 1, 3 * i + 0] = Convert.ToDouble(cha2[0]);
                Q[3 * i + 1, 3 * i + 1] = Convert.ToDouble(cha2[1]);

                line = sr.ReadLine();
                cha2 = line.Split(new Char[] { ' ', '\t' });
                Q[3 * i + 2, 3 * i + 0] = Convert.ToDouble(cha2[0]);
                Q[3 * i + 2, 3 * i + 1] = Convert.ToDouble(cha2[1]);
                Q[3 * i + 2, 3 * i + 2] = Convert.ToDouble(cha2[2]);

            }



            for (int i = 0; i < Q.RowCount; i++)
            {
                for (int j = i; j < Q.RowCount; j++)
                {
                    Q[i, j] = Q[j, i];
                }
            }


            //
            Geo.Algorithm.IMatrix P = Q.GetInverse();

            Geo.Algorithm.IMatrix N = A.Transposition.Multiply(P).Multiply(A); //A.Transposition.Multiply(A); // 

            Geo.Algorithm.IMatrix U = A.Transposition.Multiply(P).Multiply(L); //A.Transposition.Multiply(L);// 



            #region 法1：自由网平差（宋课件）
            //double tmp = 1.0 / (stationNameList.Count);
            ////秩亏的条件
            //for (int time = 0; time < N.RowCount; time++)
            //    N[time, time] = N[time, time] + tmp;
            #endregion

            #region 法2：固定点平差（宋课件）
            //for (int time = 0; time < 3; time++)
            //    N[time, time] = N[time, time] + 1.0e30;

            //foreach (var key in prioriXyzDictorary)
            //{
            //    string staName = key.Key;
            //    XYZ xyz = key.Value;

            //    int index = stationNameList.IndexOf(staName);
            //    N[3 * index + 0, 3 * index + 0] = N[3 * index + 0, 3 * index + 0] + 1.0e30;
            //    N[3 * index + 1, 3 * index + 1] = N[3 * index + 1, 3 * index + 1] + 1.0e30;
            //    N[3 * index + 2, 3 * index + 2] = N[3 * index + 2, 3 * index + 2] + 1.0e30;
            //}


            #endregion

            #region 法3：minimum constraint conditions
            //add minimum constraint conditions
            count = appriXyzDictorary.Count;
            Geo.Algorithm.IMatrix B = new ArrayMatrix(count * 3, 7);
            Geo.Algorithm.IMatrix h = new ArrayMatrix(count * 3, 1);
            Geo.Algorithm.IMatrix Ph = new ArrayMatrix(count * 3, count * 3);

            int k = 0;
            foreach (var item in prioriXyzDictorary)
            {
                string staName = item.Key;
                XYZ xyz = item.Value;

                int index = stationNameList.IndexOf(staName);

                B[3 * index + 0, 0] = 1; B[3 * index + 0, 1] = 0; B[3 * index + 0, 2] = 0; B[3 * index + 0, 3] = 0; B[3 * index + 0, 4] = -xyz.Z; B[3 * index + 0, 5] = xyz.Y; B[3 * index + 0, 6] = xyz.X;
                B[3 * index + 1, 0] = 0; B[3 * index + 1, 1] = 1; B[3 * index + 1, 2] = 0; B[3 * index + 1, 3] = xyz.Z; B[3 * index + 1, 4] = 0; B[3 * index + 1, 5] = -xyz.X; B[3 * index + 1, 6] = xyz.Y;
                B[3 * index + 2, 0] = 0; B[3 * index + 2, 1] = 0; B[3 * index + 2, 2] = 1; B[3 * index + 2, 3] = -xyz.Y; B[3 * index + 2, 4] = xyz.X; B[3 * index + 2, 5] = 0; B[3 * index + 2, 6] = xyz.Z;

                h[3 * index + 0, 0] = xyz.X - appriXyzDictorary[staName].X;
                h[3 * index + 1, 0] = xyz.Y - appriXyzDictorary[staName].Y;
                h[3 * index + 2, 0] = xyz.Z - appriXyzDictorary[staName].Z;

                Ph[3 * index + 0, 3 * index + 0] = 10e10;
                Ph[3 * index + 1, 3 * index + 1] = 10e10;
                Ph[3 * index + 2, 3 * index + 2] = 10e10;

                k++;
            }
            Geo.Algorithm.IMatrix tmp = (B.Transposition).Multiply(B); //7*7

            Geo.Algorithm.IMatrix H = (B.Transposition); //tmp.GetInverse().Multiply(B.Transposition); //7*3n

            Geo.Algorithm.IMatrix lb = H.Multiply(h); //7*1


            Geo.Algorithm.IMatrix Nb = H.Transposition.Multiply(H); //3n*3n
            Geo.Algorithm.IMatrix Ub = H.Transposition.Multiply(lb);//3n*1



            N = N.Plus(Nb);
            U = U.Plus(Ub);
            #endregion



           
          //  Geo.Algorithm.IMatrix QX = (N.GetInverse());
          
            Geo.Algorithm.IMatrix QX = (N).GetInverse();

            Geo.Algorithm.IMatrix X = QX.Multiply(U);
            //Geo.Algorithm.IMatrix X = QX.Multiply(U);



            //输出结果
            #region
            string savePath = "D:\\Restult" + ".txt";
            FileInfo aFile = new FileInfo(savePath);
            StreamWriter sw = aFile.CreateText();
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;
            GN.NumberDecimalDigits = 6;//小数点6位
            for (int i = 0; i < stationNameList.Count; i++)
            {
                sw.Write(stationNameList[i]);
                sw.Write("\t");
                sw.Write((appriXyzDictorary[stationNameList[i]].X + X[3 * i + 0, 0]).ToString("N", GN));//
                sw.Write("\t");
                sw.Write((appriXyzDictorary[stationNameList[i]].Y + X[3 * i + 1, 0]).ToString("N", GN));//
                sw.Write("\t");
                sw.Write((appriXyzDictorary[stationNameList[i]].Z + X[3 * i + 2, 0]).ToString("N", GN));//
                sw.Write("\n");
            }

            //for (int time = 0; time < Nb.RowCount; time++)
            //{
            //    for (int j = 0; j < Nb.ColCount; j++)
            //    {
            //        sw.Write(Nb[time, j].ToString("N", GN));
            //        sw.Write(",");
            //    }
            //    sw.Write(";");
            //    sw.Write("\n");
            //}

            for (int i = 0; i < QX.RowCount; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    sw.Write(QX[i, j].ToString("N", GN));
                    sw.Write("\t");
                }
                sw.Write("\n");
            }

            sw.Close();
            #endregion


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="strKnowPoint"></param>
        /// <param name="sk0"></param>
        /// <param name="sk1"></param>
        /// <param name="seps"></param>
        public SinexNetworksAdjust(string[] filePath, string[] strKnowPoint, double sk0, double sk1, double seps)
        {
            double k0 = sk0;
            double k1 = sk1;
            if (k1 <= k0)
            { throw new Exception("抗差初值错误！"); }           
            double eps = seps;
            if (eps <= 0)
            { throw new Exception("限差必须大于0！"); }
            //获取已知点点号信息
            for (int i = 0; i < strKnowPoint.Length; i++)
            {
                BasicInformation.KnownPointName.Add(strKnowPoint[i]);
            }
            //
            sinexFiles = SinexReader.Read(filePath);
            BasicInformation.FileNumber = sinexFiles.Count;
            ConfirmPointsBasicInformation(sinexFiles);//获取点基本信息

            //建立基线信息
            // bulit(); 
            List<int[]> totalbeginOfBaselineList = new List<int[]>();
            List<int[]> totalendOfBaselineList = new List<int[]>();
            double[][] bList = new double[BasicInformation.FileNumber][];
            double[][] pList = new double[BasicInformation.FileNumber][];
            bulitBaselineInfomation(ref  totalbeginOfBaselineList, ref totalendOfBaselineList, ref bList, ref pList);//只建立公共点部分

            // PointsTrueXYZ = PointsXYZ;
            string fileWeekPath = "D:\\Test\\igs13p1721.SNX";
            List<XYZ> pubEst = GetPubPointEstXYZ(fileWeekPath);//获取周解

            double[] tmpXYZ = new double[PointsXYZ.Count * 3];
            for (int i = 0; i < PointsXYZ.Count; i++)
            {
                tmpXYZ[3 * i + 0] = PointsXYZ[i].X;
                tmpXYZ[3 * i + 1] = PointsXYZ[i].Y;
                tmpXYZ[3 * i + 2] = PointsXYZ[i].Z;
            }


            var swLS = Stopwatch.StartNew();
            ////最小二乘解
            RobustAdjustment LSAdjustment = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            // GnssRobustAdjustment LSAdjustment = new GnssRobustAdjustment(BasicInformation.FileNumber, BasicInformation.TotalPointNumber, BasicInformation.UnknowPointnumber, BasicInformation.TotalBaselineNumber,
            //    BasicInformation.TotalPointName, BasicInformation.KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList,  bList,  pList);
            LSAdjustment.LeastSquares();
            double[] LsEst = LSAdjustment.pointsXYZ;
            double sigmaLS = LSAdjustment.Sigma;
            string timeLS = swLS.Elapsed.ToString();

            var swLS1 = Stopwatch.StartNew();
            RobustAdjustment LSAdjustment1 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            // GnssRobustAdjustment LSAdjustment = new GnssRobustAdjustment(BasicInformation.FileNumber, BasicInformation.TotalPointNumber, BasicInformation.UnknowPointnumber, BasicInformation.TotalBaselineNumber,
            //    BasicInformation.TotalPointName, BasicInformation.KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList,  bList,  pList);
            LSAdjustment1.Parallel_LeastSquares();
            double[] LsEst1 = LSAdjustment1.pointsXYZ;
            double sigmaLS1 = LSAdjustment1.Sigma;
            string timeLS1 = swLS1.Elapsed.ToString();

            //var sw4 = Stopwatch.StartNew();
            //RobustAdjustment RoAdjustment4 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            ////GnssRobustAdjustment RoAdjustment = new GnssRobustAdjustment(FileNumber, TotalPointNumber, UnknowPointnumber, TotalBaselineNumber,
            ////    TotalPointName, KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            //RoAdjustment4.Robust(FactorYype.IGG3, 0.001, 3.5, 11.5, 0);
            ////RoAdjustment.LeastSquares();
            //double[] robustEst4 = RoAdjustment4.pointsXYZ;
            //double sigmaRo4 = RoAdjustment4.Sigma;
            //string time4 = sw4.Elapsed.ToString();


            //var sw5 = Stopwatch.StartNew();
            //RobustAdjustment RoAdjustment5 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            ////GnssRobustAdjustment RoAdjustment3 = new GnssRobustAdjustment(BasicInformation.FileNumber, TotalPointNumber, UnknowPointnumber, TotalBaselineNumber,
            ////    TotalPointName, KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            //RoAdjustment5.parallelRobust(FactorYype.IGG3, 0.001, 3.5, 11.5, 0);//parallelRobust
            ////RoAdjustment3.ParallRobust(FactorYype.IGG3, 0.005, 1.5, 3.5, 2);
            ////RoAdjustment.LeastSquares();
            //double[] robustEst5 = RoAdjustment5.pointsXYZ;
            //double sigmaRo5 = RoAdjustment5.Sigma;
            //string time5 = sw5.Elapsed.ToString();



            var sw1 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment1 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment1.Robust(FactorYype.IGG3, eps, k0, k1, 1);
            double[] robustEst1 = RoAdjustment1.pointsXYZ;
            double sigmaRo1 = RoAdjustment1.Sigma;
            string timeR1 = sw1.Elapsed.ToString();


            var sw2 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment2 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment2.parallelRobust(FactorYype.IGG3, eps, k0, k1, 1);//parallelRobust
            double[] robustEst2 = RoAdjustment2.pointsXYZ;
            double sigmaRo2 = RoAdjustment2.Sigma;
            string timeR2 = sw2.Elapsed.ToString();

            var sw3 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment3 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            //GnssRobustAdjustment RoAdjustment = new GnssRobustAdjustment(FileNumber, TotalPointNumber, UnknowPointnumber, TotalBaselineNumber,
            //    TotalPointName, KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment3.Robust(FactorYype.IGG3, eps, k0, k1, 2);
            //RoAdjustment.LeastSquares();
            double[] robustEst3 = RoAdjustment3.pointsXYZ;
            double sigmaRo3 = RoAdjustment3.Sigma;
            string timeR3 = sw3.Elapsed.ToString();

            var sw4 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment4 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            //GnssRobustAdjustment RoAdjustment3 = new GnssRobustAdjustment(BasicInformation.FileNumber, TotalPointNumber, UnknowPointnumber, TotalBaselineNumber,
            //    TotalPointName, KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment4.parallelRobust(FactorYype.IGG3, eps, k0, k1, 2);//parallelRobust
            //RoAdjustment3.ParallRobust(FactorYype.IGG3, 0.005, 1.5, 3.5, 2);
            //RoAdjustment.LeastSquares();
            double[] robustEst4 = RoAdjustment4.pointsXYZ;
            double sigmaRo4 = RoAdjustment4.Sigma;
            string timeR4 = sw4.Elapsed.ToString();


            var sw5 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment5 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment5.Robust(FactorYype.IGG3, eps, k0, k1, 3);
            double[] robustEst5 = RoAdjustment5.pointsXYZ;
            double sigmaRo5 = RoAdjustment5.Sigma;
            string timeR5 = sw5.Elapsed.ToString();

            var sw6 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment6 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment6.parallelRobust(FactorYype.IGG3, eps, k0, k1, 3);//parallelRobust       
            double[] robustEst6 = RoAdjustment6.pointsXYZ;
            double sigmaRo6 = RoAdjustment6.Sigma;

            string timeR6 = sw6.Elapsed.ToString();



            //输出结果
            #region
            string savePath = "D:\\Test\\Restult" + ".txt";
            FileInfo aFile = new FileInfo(savePath);
            StreamWriter sw = aFile.CreateText();
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;
            GN.NumberDecimalDigits = 6;//小数点6位
            for (int i = 0; i < PointsXYZ.Count; i++)
            {
                XYZ LsXYZ = new XYZ(pubEst[i].X - LsEst[3 * i + 0], pubEst[i].Y - LsEst[3 * i + 1], pubEst[i].Z - LsEst[3 * i + 2]);
                XYZ RoXYZ2 = new XYZ(pubEst[i].X - robustEst2[3 * i + 0], pubEst[i].Y - robustEst2[3 * i + 1], pubEst[i].Z - robustEst2[3 * i + 2]);
                XYZ RoXYZ4 = new XYZ(pubEst[i].X - robustEst4[3 * i + 0], pubEst[i].Y - robustEst4[3 * i + 1], pubEst[i].Z - robustEst4[3 * i + 2]);
                XYZ RoXYZ6 = new XYZ(pubEst[i].X - robustEst6[3 * i + 0], pubEst[i].Y - robustEst6[3 * i + 1], pubEst[i].Z - robustEst6[3 * i + 2]);

                sw.Write(LsXYZ.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(LsXYZ.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(LsXYZ.Z.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ2.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ2.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ2.Z.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ4.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ4.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ4.Z.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ6.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ6.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ6.Z.ToString("N", GN));

                sw.Write("\n");
            }
            sw.Close();
            #endregion


        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="strKnowPoint"></param>
        /// <param name="sk0"></param>
        /// <param name="sk1"></param>
        /// <param name="seps"></param>
        /// <param name="rate"></param>
        public SinexNetworksAdjust(string[] filePath, string[] strKnowPoint, double sk0, double sk1, double seps, double maxRrate)
        {
            //不同粗差率
            int m = Convert.ToInt32(Math.Floor(maxRrate / 0.005));//从0.5%起始算
            for (int i = 0; i < m; i++)
            {
                double rate = (i + 1) * 0.005;
                BaselinesList = new List<XYZ>();
                PointsXYZ = new List<XYZ>();
                PointsTrueXYZ = new List<XYZ>();
                BaselineBlockList = new List<double[][]>();
                BaselineCovBlockList = new List<double[][]>();
                BaselineNameBlcokList = new List<double[][]>();
                BasicInformation = new AdjustBasicInformation();
                sinexFiles = null;
                SinexNetworksAdjustTest(filePath, strKnowPoint, sk0, sk1, seps, rate);
            }

            //不同规模，每个文件代表一个规模
            //int m = filePath.Length;
            //for (int time = 0; time < m; time++)
            //{
            //    BaselinesList = new List<XYZ>();
            //    PointsXYZ = new List<XYZ>();
            //    PointsTrueXYZ = new List<XYZ>();
            //    BaselineBlockList = new List<double[][]>();
            //    BaselineCovBlockList = new List<double[][]>();
            //    BaselineNameBlcokList = new List<double[][]>();
            //    BasicInformation = new AdjustBasicInformation();
            //    sinexFiles = null;
            //    SinexNetworksAdjustTest(filePath[time], strKnowPoint, sk0, sk1, seps, maxRrate, time);
            //}
        }

        /// <summary>
        /// 不同粗差率的抗差估计
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="strKnowPoint"></param>
        /// <param name="sk0"></param>
        /// <param name="sk1"></param>
        /// <param name="seps"></param>
        /// <param name="rate">粗差率,换算为小数</param>
        public void SinexNetworksAdjustTest(string sfilePath, string[] strKnowPoint, double sk0, double sk1, double seps, double rate,int Time)
        {
            string orignFile = sfilePath;

            string strFileName = sfilePath.Substring(sfilePath.LastIndexOf("\\"),sfilePath.Length- sfilePath.LastIndexOf("\\"));

            int m = 7;//模拟算7天
            string[] filePath = new string[m];
            for (int i = 0; i < m; i++)
            {
                filePath[i] = sfilePath;
            }

            double k0 = sk0;
            double k1 = sk1;
            if (k1 <= k0)
            { throw new Exception("抗差初值错误！"); }
            double eps = seps;
            if (eps <= 0)
            { throw new Exception("限差必须大于0！"); }
            //获取已知点点号信息
            for (int i = 0; i < strKnowPoint.Length; i++)
            {
                BasicInformation.KnownPointName.Add(strKnowPoint[i]);
            }
            //
            sinexFiles = SinexReader.Read(filePath);
            BasicInformation.FileNumber = sinexFiles.Count;
            ConfirmPointsBasicInformation(sinexFiles);//获取点基本信息

            //建立基线信息：纯净的数据
            // bulit(); 
            List<int[]> totalbeginOfBaselineList = new List<int[]>();
            List<int[]> totalendOfBaselineList = new List<int[]>();
            double[][] bList = new double[BasicInformation.FileNumber][];
            double[][] pList = new double[BasicInformation.FileNumber][];
            bulitBaselineInfomation(ref  totalbeginOfBaselineList, ref totalendOfBaselineList, ref bList, ref pList);//只建立公共点部分

            //对观测值添加粗差，即对bList修改
            double sigma = 0.002;//2mm中误差
            AddGrossError(ref bList, rate, sigma);


            // PointsTrueXYZ = PointsXYZ;
            List<XYZ> pubEst = GetPubPointEstXYZ(orignFile);//获取周解

            double[] tmpXYZ = new double[PointsXYZ.Count * 3];
            for (int i = 0; i < PointsXYZ.Count; i++)
            {
                tmpXYZ[3 * i + 0] = PointsXYZ[i].X;
                tmpXYZ[3 * i + 1] = PointsXYZ[i].Y;
                tmpXYZ[3 * i + 2] = PointsXYZ[i].Z;
            }


            var swLS = Stopwatch.StartNew();
            ////最小二乘解
            RobustAdjustment LSAdjustment = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            // GnssRobustAdjustment LSAdjustment = new GnssRobustAdjustment(BasicInformation.FileNumber, BasicInformation.TotalPointNumber, BasicInformation.UnknowPointnumber, BasicInformation.TotalBaselineNumber,
            //    BasicInformation.TotalPointName, BasicInformation.KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList,  bList,  pList);
            LSAdjustment.LeastSquares();
            double[] LsEst = LSAdjustment.pointsXYZ;
            double sigmaLS = LSAdjustment.Sigma;
            string timeLS = swLS.Elapsed.ToString();

            var swLS1 = Stopwatch.StartNew();
            RobustAdjustment LSAdjustment1 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            // GnssRobustAdjustment LSAdjustment = new GnssRobustAdjustment(BasicInformation.FileNumber, BasicInformation.TotalPointNumber, BasicInformation.UnknowPointnumber, BasicInformation.TotalBaselineNumber,
            //    BasicInformation.TotalPointName, BasicInformation.KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList,  bList,  pList);
            LSAdjustment1.Parallel_LeastSquares();
            double[] LsEst1 = LSAdjustment1.pointsXYZ;
            double sigmaLS1 = LSAdjustment1.Sigma;
            string timeLS1 = swLS1.Elapsed.ToString();

            var sw1 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment1 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment1.Robust(FactorYype.IGG3, eps, k0, k1, 1);
            double[] robustEst1 = RoAdjustment1.pointsXYZ;
            double sigmaRo1 = RoAdjustment1.Sigma;
            string timeR1 = sw1.Elapsed.ToString();


            var sw2 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment2 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment2.parallelRobust(FactorYype.IGG3, eps, k0, k1, 1);//parallelRobust
            double[] robustEst2 = RoAdjustment2.pointsXYZ;
            double sigmaRo2 = RoAdjustment2.Sigma;
            string timeR2 = sw2.Elapsed.ToString();

            var sw3 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment3 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            //GnssRobustAdjustment RoAdjustment = new GnssRobustAdjustment(FileNumber, TotalPointNumber, UnknowPointnumber, TotalBaselineNumber,
            //    TotalPointName, KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment3.Robust(FactorYype.IGG3, eps, k0, k1, 2);
            //RoAdjustment.LeastSquares();
            double[] robustEst3 = RoAdjustment3.pointsXYZ;
            double sigmaRo3 = RoAdjustment3.Sigma;
            string timeR3 = sw3.Elapsed.ToString();

            var sw4 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment4 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            //GnssRobustAdjustment RoAdjustment3 = new GnssRobustAdjustment(BasicInformation.FileNumber, TotalPointNumber, UnknowPointnumber, TotalBaselineNumber,
            //    TotalPointName, KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment4.parallelRobust(FactorYype.IGG3, eps, k0, k1, 2);//parallelRobust
            //RoAdjustment3.ParallRobust(FactorYype.IGG3, 0.005, 1.5, 3.5, 2);
            //RoAdjustment.LeastSquares();
            double[] robustEst4 = RoAdjustment4.pointsXYZ;
            double sigmaRo4 = RoAdjustment4.Sigma;
            string timeR4 = sw4.Elapsed.ToString();


            var sw5 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment5 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment5.Robust(FactorYype.IGG3, eps, k0, k1, 3);
            double[] robustEst5 = RoAdjustment5.pointsXYZ;
            double sigmaRo5 = RoAdjustment5.Sigma;
            string timeR5 = sw5.Elapsed.ToString();

            var sw6 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment6 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment6.parallelRobust(FactorYype.IGG3, eps, k0, k1, 3);//parallelRobust       
            double[] robustEst6 = RoAdjustment6.pointsXYZ;
            double sigmaRo6 = RoAdjustment6.Sigma;

            string timeR6 = sw6.Elapsed.ToString();



            //输出结果
            #region
            string savePath = "D:\\Test\\" + strFileName + Time.ToString() + "-" + rate.ToString() + "-Restult" + ".txt";
            FileInfo aFile = new FileInfo(savePath);
            StreamWriter sw = aFile.CreateText();
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;
            GN.NumberDecimalDigits = 6;//小数点6位
            sw.WriteLine(BasicInformation.TotalPointNumber.ToString());

            sw.WriteLine("LS单位中误差 = " + sigmaLS.ToString("N", GN));

            sw.WriteLine("LS串行计算时间 = " + timeLS);

            sw.WriteLine("LS单位中误差 = " + sigmaLS1.ToString("N", GN));
          
            sw.Write("LS并行计算时间 = " + timeLS1);
            sw.Write("\n");
            sw.Write("抗差1单位中误差 = " + sigmaRo1.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差1串行计算时间 = " + timeR1);
            sw.Write("\n");
            sw.Write("抗差1单位中误差 = " + sigmaRo2.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差1并行计算时间 = " + timeR2);
            sw.Write("\n");
            sw.Write("抗差2单位中误差 = " + sigmaRo3.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差2串行计算时间 = " + timeR3);
            sw.Write("\n");
            sw.Write("抗差2单位中误差 = " + sigmaRo4.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差2并行计算时间 = " + timeR4);
            sw.Write("\n");
            sw.Write("抗差3单位中误差 = " + sigmaRo5.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差3串行计算时间 = " + timeR5);
            sw.Write("\n");
            sw.Write("抗差3单位中误差 = " + sigmaRo6.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差3并行计算时间 = " + timeR6);
            sw.Write("\n");
            for (int i = 0; i < PointsXYZ.Count; i++)
            {
                XYZ LsXYZ = new XYZ(pubEst[i].X - LsEst[3 * i + 0], pubEst[i].Y - LsEst[3 * i + 1], pubEst[i].Z - LsEst[3 * i + 2]);
                XYZ RoXYZ2 = new XYZ(pubEst[i].X - robustEst2[3 * i + 0], pubEst[i].Y - robustEst2[3 * i + 1], pubEst[i].Z - robustEst2[3 * i + 2]);
                XYZ RoXYZ4 = new XYZ(pubEst[i].X - robustEst4[3 * i + 0], pubEst[i].Y - robustEst4[3 * i + 1], pubEst[i].Z - robustEst4[3 * i + 2]);
                XYZ RoXYZ6 = new XYZ(pubEst[i].X - robustEst6[3 * i + 0], pubEst[i].Y - robustEst6[3 * i + 1], pubEst[i].Z - robustEst6[3 * i + 2]);

                sw.Write(LsXYZ.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(LsXYZ.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(LsXYZ.Z.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ2.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ2.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ2.Z.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ4.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ4.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ4.Z.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ6.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ6.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ6.Z.ToString("N", GN));

                sw.Write("\n");
            }
            sw.Close();
            #endregion


        }



        /// <summary>
        /// 不同粗差率的抗差估计
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="strKnowPoint"></param>
        /// <param name="sk0"></param>
        /// <param name="sk1"></param>
        /// <param name="seps"></param>
        /// <param name="rate">粗差率,换算为小数</param>
        public void SinexNetworksAdjustTest(string[] filePath, string[] strKnowPoint, double sk0, double sk1, double seps, double rate)
        {
            double k0 = sk0;
            double k1 = sk1;
            if (k1 <= k0)
            { throw new Exception("抗差初值错误！"); }
            double eps = seps;
            if (eps <= 0)
            { throw new Exception("限差必须大于0！"); }
            //获取已知点点号信息
            for (int i = 0; i < strKnowPoint.Length; i++)
            {
                BasicInformation.KnownPointName.Add(strKnowPoint[i]);
            }
            //
            sinexFiles = SinexReader.Read(filePath);
            BasicInformation.FileNumber = sinexFiles.Count;
            ConfirmPointsBasicInformation(sinexFiles);//获取点基本信息

            //建立基线信息：纯净的数据
            // bulit(); 
            List<int[]> totalbeginOfBaselineList = new List<int[]>();
            List<int[]> totalendOfBaselineList = new List<int[]>();
            double[][] bList = new double[BasicInformation.FileNumber][];
            double[][] pList = new double[BasicInformation.FileNumber][];
            bulitBaselineInfomation(ref  totalbeginOfBaselineList, ref totalendOfBaselineList, ref bList, ref pList);//只建立公共点部分

            //对观测值添加粗差，即对bList修改
            double sigma = 0.002;//2mm中误差
            AddGrossError(ref bList, rate, sigma);


            // PointsTrueXYZ = PointsXYZ;
            string fileWeekPath = "D:\\Test\\igs13p1721.SNX";
            List<XYZ> pubEst = GetPubPointEstXYZ(fileWeekPath);//获取周解

            double[] tmpXYZ = new double[PointsXYZ.Count * 3];
            for (int i = 0; i < PointsXYZ.Count; i++)
            {
                tmpXYZ[3 * i + 0] = PointsXYZ[i].X;
                tmpXYZ[3 * i + 1] = PointsXYZ[i].Y;
                tmpXYZ[3 * i + 2] = PointsXYZ[i].Z;
            }


            var swLS = Stopwatch.StartNew();
            ////最小二乘解
            RobustAdjustment LSAdjustment = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            // GnssRobustAdjustment LSAdjustment = new GnssRobustAdjustment(BasicInformation.FileNumber, BasicInformation.TotalPointNumber, BasicInformation.UnknowPointnumber, BasicInformation.TotalBaselineNumber,
            //    BasicInformation.TotalPointName, BasicInformation.KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList,  bList,  pList);
            LSAdjustment.LeastSquares();
            double[] LsEst = LSAdjustment.pointsXYZ;
            double sigmaLS = LSAdjustment.Sigma;
            string timeLS = swLS.Elapsed.ToString();

            var swLS1 = Stopwatch.StartNew();
            RobustAdjustment LSAdjustment1 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            // GnssRobustAdjustment LSAdjustment = new GnssRobustAdjustment(BasicInformation.FileNumber, BasicInformation.TotalPointNumber, BasicInformation.UnknowPointnumber, BasicInformation.TotalBaselineNumber,
            //    BasicInformation.TotalPointName, BasicInformation.KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList,  bList,  pList);
            LSAdjustment1.Parallel_LeastSquares();
            double[] LsEst1 = LSAdjustment1.pointsXYZ;
            double sigmaLS1 = LSAdjustment1.Sigma;
            string timeLS1 = swLS1.Elapsed.ToString();
            
            var sw1 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment1 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment1.Robust(FactorYype.IGG3, eps, k0, k1, 1);
            double[] robustEst1 = RoAdjustment1.pointsXYZ;
            double sigmaRo1 = RoAdjustment1.Sigma;
            string timeR1 = sw1.Elapsed.ToString();


            var sw2 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment2 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment2.parallelRobust(FactorYype.IGG3, eps, k0, k1, 1);//parallelRobust
            double[] robustEst2 = RoAdjustment2.pointsXYZ;
            double sigmaRo2 = RoAdjustment2.Sigma;
            string timeR2 = sw2.Elapsed.ToString();

            var sw3 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment3 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            //GnssRobustAdjustment RoAdjustment = new GnssRobustAdjustment(FileNumber, TotalPointNumber, UnknowPointnumber, TotalBaselineNumber,
            //    TotalPointName, KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment3.Robust(FactorYype.IGG3, eps, k0, k1, 2);
            //RoAdjustment.LeastSquares();
            double[] robustEst3 = RoAdjustment3.pointsXYZ;
            double sigmaRo3 = RoAdjustment3.Sigma;
            string timeR3 = sw3.Elapsed.ToString();

            var sw4 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment4 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            //GnssRobustAdjustment RoAdjustment3 = new GnssRobustAdjustment(BasicInformation.FileNumber, TotalPointNumber, UnknowPointnumber, TotalBaselineNumber,
            //    TotalPointName, KnownPointName, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment4.parallelRobust(FactorYype.IGG3, eps, k0, k1, 2);//parallelRobust
            //RoAdjustment3.ParallRobust(FactorYype.IGG3, 0.005, 1.5, 3.5, 2);
            //RoAdjustment.LeastSquares();
            double[] robustEst4 = RoAdjustment4.pointsXYZ;
            double sigmaRo4 = RoAdjustment4.Sigma;
            string timeR4 = sw4.Elapsed.ToString();


            var sw5 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment5 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment5.Robust(FactorYype.IGG3, eps, k0, k1, 3);
            double[] robustEst5 = RoAdjustment5.pointsXYZ;
            double sigmaRo5 = RoAdjustment5.Sigma;
            string timeR5 = sw5.Elapsed.ToString();

            var sw6 = Stopwatch.StartNew();
            RobustAdjustment RoAdjustment6 = new RobustAdjustment(BasicInformation, tmpXYZ, totalbeginOfBaselineList, totalendOfBaselineList, bList, pList);
            RoAdjustment6.parallelRobust(FactorYype.IGG3, eps, k0, k1, 3);//parallelRobust       
            double[] robustEst6 = RoAdjustment6.pointsXYZ;
            double sigmaRo6 = RoAdjustment6.Sigma;

            string timeR6 = sw6.Elapsed.ToString();



            //输出结果
            #region
            string savePath = "D:\\Test\\"+rate.ToString()+"-Restult" + ".txt";
            FileInfo aFile = new FileInfo(savePath);
            StreamWriter sw = aFile.CreateText();
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;
            GN.NumberDecimalDigits = 6;//小数点6位
            sw.Write("LS单位中误差 = " + sigmaLS.ToString("N", GN));
            sw.Write("\n");
            sw.Write("LS串行计算时间 = " + timeLS);
            sw.Write("\n");
            sw.Write("LS单位中误差 = " + sigmaLS1.ToString("N", GN));
            sw.Write("\n");
            sw.Write("LS并行计算时间 = " + timeLS1);
            sw.Write("\n");
            sw.Write("抗差1单位中误差 = " + sigmaRo1.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差1串行计算时间 = " + timeR1);
            sw.Write("\n");
            sw.Write("抗差1单位中误差 = " + sigmaRo2.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差1并行计算时间 = " + timeR2);
            sw.Write("\n");
            sw.Write("抗差2单位中误差 = " + sigmaRo3.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差2串行计算时间 = " + timeR3);
            sw.Write("\n");
            sw.Write("抗差2单位中误差 = " + sigmaRo4.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差2并行计算时间 = " + timeR4);
            sw.Write("\n");
            sw.Write("抗差3单位中误差 = " + sigmaRo5.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差3串行计算时间 = " + timeR5);
            sw.Write("\n");
            sw.Write("抗差3单位中误差 = " + sigmaRo6.ToString("N", GN));
            sw.Write("\n");
            sw.Write("抗差3并行计算时间 = " + timeR6);
            sw.Write("\n");
            for (int i = 0; i < PointsXYZ.Count; i++)
            {
                XYZ LsXYZ = new XYZ(pubEst[i].X - LsEst[3 * i + 0], pubEst[i].Y - LsEst[3 * i + 1], pubEst[i].Z - LsEst[3 * i + 2]);
                XYZ RoXYZ2 = new XYZ(pubEst[i].X - robustEst2[3 * i + 0], pubEst[i].Y - robustEst2[3 * i + 1], pubEst[i].Z - robustEst2[3 * i + 2]);
                XYZ RoXYZ4 = new XYZ(pubEst[i].X - robustEst4[3 * i + 0], pubEst[i].Y - robustEst4[3 * i + 1], pubEst[i].Z - robustEst4[3 * i + 2]);
                XYZ RoXYZ6 = new XYZ(pubEst[i].X - robustEst6[3 * i + 0], pubEst[i].Y - robustEst6[3 * i + 1], pubEst[i].Z - robustEst6[3 * i + 2]);

                sw.Write(LsXYZ.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(LsXYZ.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(LsXYZ.Z.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ2.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ2.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ2.Z.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ4.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ4.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ4.Z.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ6.X.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ6.Y.ToString("N", GN));
                sw.Write(" ");
                sw.Write(RoXYZ6.Z.ToString("N", GN));

                sw.Write("\n");
            }
            sw.Close();
            #endregion


        }


        /// <summary>
        /// 获取基本的信息：所有点编号、点总数、点（近似）坐标、未知点数、向量个数等。
        /// </summary>
        /// <param name="sinexFiles"></param>
        private void ConfirmPointsBasicInformation(List<SinexFile> sinexFiles)
        {
            if (sinexFiles.Count < 1) { throw new Exception("请先输入文件信息"); }
            //提取公共点部分
            SinexFile item0 = sinexFiles[0];
            BasicInformation.TotalPointNumber = 0;
            List<string> site0Cods = item0.GetSiteCods();
            double[] array0 = item0.GetEstimateVector();
            for (int i = 0; i < site0Cods.Count; i++)
            {
                string tmp = site0Cods[i];
                bool isContain = true;
                if (sinexFiles.Count > 1)
                {
                    for (int j = 1; j < sinexFiles.Count; j++)
                    {
                        //
                        List<string> siteCods = sinexFiles[j].GetSiteCods();
                        if (!siteCods.Contains(tmp)) isContain = false;
                    }
                }
                if (isContain == true)
                {
                    BasicInformation.TotalPointNumber += 1;
                    BasicInformation.TotalPointName.Add(site0Cods[i]);
                    XYZ xyz0 = new XYZ(array0[3 * i + 0], array0[3 * i + 1], array0[3 * i + 2]);
                    PointsXYZ.Add(xyz0);
              
                }
            }
            BasicInformation.TotalBaselineNumber = (BasicInformation.TotalPointNumber - 1) * sinexFiles.Count;//总的基线向量个数
            //BeginPointDir = new int[BasicInformation.TotalBaselineNumber];
            //EndPointDir = new int[BasicInformation.TotalBaselineNumber];
            BasicInformation.TotalPointNumber = BasicInformation.TotalPointName.Count;
            BasicInformation.UnknowPointnumber = BasicInformation.TotalPointNumber - BasicInformation.KnownPointName.Count;
            BasicInformation.FileNumber = sinexFiles.Count;
            IsKnownPoint = new bool[BasicInformation.TotalPointNumber];
            for (int i = 0; i < BasicInformation.TotalPointNumber; i++)
            {
                if (BasicInformation.KnownPointName.Contains(BasicInformation.TotalPointName[i]))
                { IsKnownPoint[i] = true; }
                else { IsKnownPoint[i] = false; }
            }

            //坐标近似值,默认第一个坐标为已知点，其他的加上随机小数
            for (int i = 1; i < PointsXYZ.Count; i++)
            {
                Random seed = new Random();
                int r1 = 1000 - seed.Next(0, 2000);
                int r2 = 1000 - seed.Next(0, 2000);
                int r3 = 1000 - seed.Next(0, 2000);
                // double rand = r1 / 10000;//分米级误差
                PointsXYZ[i].X = PointsXYZ[i].X + r1 * Math.Pow(10, -4);//分米级误差(米-0.1~0.1之间随机数）
                PointsXYZ[i].Y = PointsXYZ[i].Y + r2 * Math.Pow(10, -4);//分米级误差(米-0.1~0.1之间随机数）
                PointsXYZ[i].Z = PointsXYZ[i].Z + r3 * Math.Pow(10, -4);//分米级误差(米-0.1~0.1之间随机数）

            }
  
        }

        /// <summary>
        /// 基本信息
        /// </summary>
        private void bulitBaselineInfomation(ref List<int[]> totalbeginOfBaselineList, ref List<int[]> totalendOfBaselineList, ref double[][] bList, ref double[][] pList)
        {
            //List<int[]> totalbeginOfBaselineList 
            //List<int[]> totalendOfBaselineList 
            //List<double[]> bList 
            //List<double[]> pList
            int count = 0;//计算基线向量个数
            for (int i = 0; i < sinexFiles.Count; i++)
            {
                //保存基线向量的点号
                List<string> siteName = sinexFiles[i].GetSiteCods();
                int n = BasicInformation.TotalPointNumber;
                //每个文件的向量起点取第一个出现的点，剩余的为独立观测向量的终点     
                int[] BenDir = new int[n - 1];
                int[] EndDir = new int[n - 1];
                int s = 0;
                for (int k = 0; k < BasicInformation.TotalPointName.Count; k++)
                {
                    //
                    if (k != i)
                    {
                        string strB = BasicInformation.TotalPointName[i];
                        if (BasicInformation.TotalPointName.Contains(strB)) { int tmp = BasicInformation.TotalPointName.IndexOf(strB); }
                        BenDir[s] = BasicInformation.TotalPointName.IndexOf(BasicInformation.TotalPointName[i]);
                        EndDir[s] = BasicInformation.TotalPointName.IndexOf(BasicInformation.TotalPointName[k]);
                        s += 1;
                    }
                }
                totalbeginOfBaselineList.Add(BenDir);
                totalendOfBaselineList.Add(EndDir);

                //生成公共点部分的基线向量并保存向量坐标差
                double[] estimateL = sinexFiles[i].GetEstimateVector();
                double[][] estimeatLCov = sinexFiles[i].GetEstimateCovaMatrix();
                //（1）扣除非公共部分

                double[][] cleaned = GetCleanedMatrix(sinexFiles[i]);

              
                ArrayMatrix L = new ArrayMatrix(MatrixUtil.GetMatrix(estimateL));
                ArrayMatrix CovL = new ArrayMatrix(estimeatLCov);

                ArrayMatrix cleanedMatrix = new ArrayMatrix(cleaned);

                L = cleanedMatrix * L;
                CovL = cleanedMatrix * CovL * cleanedMatrix.Transpose();


                //建立基线向量
                double[][] coefficientOfL = new double[3 * (n - 1)][];
                int ss = 0;
                for (int j = 0; j < (n); j++)
                {

                    if (j != i)
                    {
                        coefficientOfL[3 * ss + 0] = new double[3 * n];
                        coefficientOfL[3 * ss + 1] = new double[3 * n];
                        coefficientOfL[3 * ss + 2] = new double[3 * n];

                        coefficientOfL[3 * ss + 0][3 * i + 0] = -1;
                        coefficientOfL[3 * ss + 0][3 * j + 0] = 1;

                        coefficientOfL[3 * ss + 1][3 * i + 1] = -1;
                        coefficientOfL[3 * ss + 1][3 * j + 1] = 1;

                        coefficientOfL[3 * ss + 2][3 * i + 2] = -1;
                        coefficientOfL[3 * ss + 2][3 * j + 2] = 1;
                        ss += 1;
                    }
                }
                ArrayMatrix coeffiA = new ArrayMatrix(coefficientOfL);
                ArrayMatrix Baseline = coeffiA * L;

                #region 加入观测误差
                //观测值L加入3倍中误差中误差？？？
                //加入数据量多少？1%合适？
               // AddGrossError(Baseline,rate);
                //int tmpCount = Baseline.Rows;
                //double sigma0 = 0.002;
                //for (int k = 0; k< tmpCount; k++)
                //{
                //    Random seed = new Random();
                    
                //    int error = seed.Next(000, 200);

                //    double ranError = sigma0 * error / 100;
                //    int isD = Convert.ToInt32(Math.IEEERemainder(k, 2));
                //    if (isD == 0)//偶数行加上，奇数行减去
                //    {
                //        Baseline[k,0] += ranError;
                //    }
                //    else
                //    {
                //        Baseline[k, 0] -= ranError;
                //    }
                //}
                #endregion
                double[] li = new double[Baseline.RowCount];
                for (int k = 0; k < li.Length; k++) { li[k] = Baseline[k, 0]; }

                bList[i]=(li);//基线向量
                //  double[] BaselineCov = CalculateAQAT(coefficientOfL.Length, coefficientOfL[0].Length, tmpcoefficientOfL, CovL);

                ArrayMatrix BaselineCov = coeffiA * CovL * coeffiA.Transpose();
                int ni = BaselineCov.RowCount;
                double[] pi = new double[(ni + 1) * ni / 2];
                int sk = 0;
                for (int s1 = 0; s1 < ni; s1++)
                {
                    for (int s2 = 0; s2 <= s1; s2++)
                    {
                        pi[sk] = BaselineCov[s1, s2];
                        sk++;
                    }
                }
                pList[i]=(pi);//基线向量协方差矩阵
     
                count += n - 1;//每个文件的独立基线向量=点数-1
            }
        }
        /// <summary>
        /// 添加粗差
        /// </summary>
        /// <param name="bList"></param>
        /// <param name="rate"></param>
        /// <param name="sigma"></param>
        private static void AddGrossError(ref double[][] bList, double rate, double sigma)
        {
            int row = bList.Length;
            double sigma0 = sigma;//2毫米，2厘米还是2米？？？
            for (int i = 0; i < row; i++)
            {
                int obs = bList[i].Length;
                int intError = Convert.ToInt32(obs * rate);//百分之几的粗差
                for (int k = 0; k < intError; k++)
                {
                    Random seed = new Random();
                    int r1 = seed.Next(0, obs);
                    int error = seed.Next(300, 600);

                    double ranError = sigma0 * error / 100;
                    int isD = Convert.ToInt32(Math.IEEERemainder(r1, 2));
                    if (isD == 0)//偶数行加上，奇数行减去
                    {
                        bList[i][r1] += ranError;
                    }
                    else
                    {
                        bList[i][r1] -= ranError;
                    }
                }
            }
            //int count = 0;
            //for (int time = 0; time < row; time++)
            //{
            //    count += bList[time].Length;
            //}
            //double[] llist = new double[count];
            //int s = 0;
            //for (int time = 0; time < row; time++)
            //{
            //    int column = bList[time].Length;
            //    for (int j = 0; j < column; j++)
            //    {
            //        llist[s] = bList[time][j];
            //        s += 1;
            //    }
            //}

            
           
            //int intError = Convert.ToInt32(count * rate);//百分之几的粗差
            //for (int k = 0; k < intError; k++)
            //{
            //    Random seed = new Random();
            //    int r1 = seed.Next(0, count);
            //    int error = seed.Next(300, 450);

            //    double ranError = sigma0 * error / 100;
            //    int isD = Convert.ToInt32(Math.IEEERemainder(r1, 2));
            //    if (isD == 0)//偶数行加上，奇数行减去
            //    {
            //        llist[r1] += ranError;
            //    }
            //    else
            //    {
            //        llist[r1] -= ranError;
            //    }
            //}
        }
        /// <summary>
        /// 基本信息
        /// 不引入粗差
        /// </summary>
        private void bulitCleanBaselineInfomation(ref List<int[]> totalbeginOfBaselineList, ref List<int[]> totalendOfBaselineList, ref double[][] bList, ref double[][] pList)
        {
            //List<int[]> totalbeginOfBaselineList 
            //List<int[]> totalendOfBaselineList 
            //List<double[]> bList 
            //List<double[]> pList
            int count = 0;//计算基线向量个数
            for (int i = 0; i < sinexFiles.Count; i++)
            {
                //保存基线向量的点号
                List<string> siteName = sinexFiles[i].GetSiteCods();
                int n = BasicInformation.TotalPointNumber;
                //每个文件的向量起点取第一个出现的点，剩余的为独立观测向量的终点     
                int[] BenDir = new int[n - 1];
                int[] EndDir = new int[n - 1];
                int s = 0;
                for (int k = 0; k < BasicInformation.TotalPointName.Count; k++)
                {
                    //
                    if (k != i)
                    {
                        string strB = BasicInformation.TotalPointName[i];
                        if (BasicInformation.TotalPointName.Contains(strB)) { int tmp = BasicInformation.TotalPointName.IndexOf(strB); }
                        BenDir[s] = BasicInformation.TotalPointName.IndexOf(BasicInformation.TotalPointName[i]);
                        EndDir[s] = BasicInformation.TotalPointName.IndexOf(BasicInformation.TotalPointName[k]);
                        s += 1;
                    }
                }
                totalbeginOfBaselineList.Add(BenDir);
                totalendOfBaselineList.Add(EndDir);

                //生成公共点部分的基线向量并保存向量坐标差
                double[] estimateL = sinexFiles[i].GetEstimateVector();
                double[][] estimeatLCov = sinexFiles[i].GetEstimateCovaMatrix();
                //（1）扣除非公共部分

                double[][] cleaned = GetCleanedMatrix(sinexFiles[i]);


                ArrayMatrix L = new ArrayMatrix(MatrixUtil.GetMatrix(estimateL));
                ArrayMatrix CovL = new ArrayMatrix(estimeatLCov);

                ArrayMatrix cleanedMatrix = new ArrayMatrix(cleaned);

                L = cleanedMatrix * L;
                CovL = cleanedMatrix * CovL * cleanedMatrix.Transpose();


                //建立基线向量
                double[][] coefficientOfL = new double[3 * (n - 1)][];
                int ss = 0;
                for (int j = 0; j < (n); j++)
                {

                    if (j != i)
                    {
                        coefficientOfL[3 * ss + 0] = new double[3 * n];
                        coefficientOfL[3 * ss + 1] = new double[3 * n];
                        coefficientOfL[3 * ss + 2] = new double[3 * n];

                        coefficientOfL[3 * ss + 0][3 * i + 0] = -1;
                        coefficientOfL[3 * ss + 0][3 * j + 0] = 1;

                        coefficientOfL[3 * ss + 1][3 * i + 1] = -1;
                        coefficientOfL[3 * ss + 1][3 * j + 1] = 1;

                        coefficientOfL[3 * ss + 2][3 * i + 2] = -1;
                        coefficientOfL[3 * ss + 2][3 * j + 2] = 1;
                        ss += 1;
                    }
                }
                ArrayMatrix coeffiA = new ArrayMatrix(coefficientOfL);
                ArrayMatrix Baseline = coeffiA * L;

                //#region 加入粗差
                ////观测值L加入3倍中误差~6倍中误差？？？
                ////加入数据量多少？1%合适？
                //double sigma0 = 0.002;//2毫米，2厘米还是2米？？？
                //int tmpCount = Baseline.Rows;
                //int intError = Convert.ToInt32(tmpCount * 0.01);//百分之几的粗差
                //for (int k = 0; k < intError; k++)
                //{
                //    Random seed = new Random();
                //    int r1 = seed.Next(0, tmpCount);
                //    int error = seed.Next(300, 450);

                //    double ranError = sigma0 * error / 100;
                //    int isD = Convert.ToInt32(Math.IEEERemainder(r1, 2));
                //    if (isD == 0)//偶数行加上，奇数行减去
                //    {
                //        Baseline[r1, 0] += ranError;
                //    }
                //    else
                //    {
                //        Baseline[r1, 0] -= ranError;
                //    }
                //}
                //#endregion

                double[] li = new double[Baseline.RowCount];
                for (int k = 0; k < li.Length; k++) { li[k] = Baseline[k, 0]; }

                bList[i] = (li);//基线向量
                //  double[] BaselineCov = CalculateAQAT(coefficientOfL.Length, coefficientOfL[0].Length, tmpcoefficientOfL, CovL);

                ArrayMatrix BaselineCov = coeffiA * CovL * coeffiA.Transpose();
                int ni = BaselineCov.RowCount;
                double[] pi = new double[(ni + 1) * ni / 2];
                int sk = 0;
                for (int s1 = 0; s1 < ni; s1++)
                {
                    for (int s2 = 0; s2 <= s1; s2++)
                    {
                        pi[sk] = BaselineCov[s1, s2];
                        sk++;
                    }
                }
                pList[i] = (pi);//基线向量协方差矩阵

                count += n - 1;//每个文件的独立基线向量=点数-1
            }
        }

        /// <summary>
        /// 上次参数的协方差阵
        /// </summary>
        /// <param name="recInfo"></param>
        /// <param name="lastPppResult"></param>
        /// <returns></returns>
        private double[][] GetCleanedMatrix(SinexFile snx)
        {
            List<int> tobeRemoveIndexes = new List<int>();
            int count =BasicInformation. TotalPointNumber;
            List<string> siteCode = snx.GetSiteCods();
            for (int i = 0; i < siteCode.Count; i++)
            {
                string tmp = siteCode[i];
                if (!BasicInformation.TotalPointName.Contains(tmp))
                {
                    tobeRemoveIndexes.Add(i * 3 + 0);//如果不存在，则表明需要舍去该点
                    tobeRemoveIndexes.Add(i * 3 + 1);//如果不存在，则表明需要舍去该点
                    tobeRemoveIndexes.Add(i * 3 + 2);//如果不存在，则表明需要舍去该点
                }
            }
            double[][] CovMatrix = MatrixUtil.CreateIdentity(siteCode.Count * 3);
            double[][] cleanedCovMatrix = MatrixUtil.ShrinkMatrixRow(CovMatrix, tobeRemoveIndexes);

            return cleanedCovMatrix;
        }

        private double[] GetPubPointEst()
        {
            SinexFile pubSinexFile = SinexReader.Read("D:\\Test\\igs13p1721.SNX");
            List<string> pubSiteCode = pubSinexFile.GetSiteCods();
            double[] pubAllEst = pubSinexFile.GetEstimateVector();
            for (int i = 0; i < BasicInformation.TotalPointNumber; i++)
            {
                string item = BasicInformation.TotalPointName[i];
                for (int j = 0; j < pubSiteCode.Count; j++)
                {
                    if (pubSiteCode[j] == item)
                    {
                        PointsTrueXYZ[i].X = pubAllEst[3 * j + 0];
                        PointsTrueXYZ[i].Y = pubAllEst[3 * j + 1];
                        PointsTrueXYZ[i].Z = pubAllEst[3 * j + 2];
                    }
                }
            }

            double[] pubEst = new double[ PointsTrueXYZ.Count * 3];
            for (int i = 0; i <  PointsTrueXYZ.Count; i++)
            {
                pubEst[3 * i + 0] =  PointsTrueXYZ[i].X;
                pubEst[3 * i + 1] =  PointsTrueXYZ[i].Y;
                pubEst[3 * i + 2] = PointsTrueXYZ[i].Z;
            }
            //第一个点作为真值
            PointsXYZ[0] = PointsTrueXYZ[0];
            return pubEst;
        }

        private List<XYZ> GetPubPointEstXYZ(string filePath)
        {
            //D:\\Test\\igs13p1721.SNX
            SinexFile pubSinexFile = SinexReader.Read(filePath);
            List<string> pubSiteCode = pubSinexFile.GetSiteCods();
            double[] pubAllEst = pubSinexFile.GetEstimateVector();
            for (int i = 0; i < BasicInformation.TotalPointNumber; i++)
            {
                string item = BasicInformation.TotalPointName[i];
                for (int j = 0; j < pubSiteCode.Count; j++)
                {
                    if (pubSiteCode[j] == item)
                    {
                        XYZ xyz = new XYZ();
                        xyz.X = pubAllEst[3 * j + 0];
                        xyz.Y = pubAllEst[3 * j + 1];
                        xyz.Z = pubAllEst[3 * j + 2];
                         PointsTrueXYZ.Add(xyz);
                    }
                }
            }
            List<XYZ> pubXYZ = new List<XYZ>();
            // double[] pubEst = new double[PointsTrueXYZ.Count * 3];
            for (int i = 0; i <  PointsTrueXYZ.Count; i++)
            {
                pubXYZ.Add( PointsTrueXYZ[i]);
                //pubEst[3 * time + 0] = PointsTrueXYZ[time].X;
                //pubEst[3 * time + 1] = PointsTrueXYZ[time].Y;
                //pubEst[3 * time + 2] = PointsTrueXYZ[time].Z;
            }
            //第一个点作为真值
            PointsXYZ[0] = PointsTrueXYZ[0];
            
            return pubXYZ;
        }

    }
}
