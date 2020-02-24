using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Geo.Utils;
using System.Diagnostics;
using System.Threading.Tasks;
using Geo.Algorithm.Adjust;

namespace Algorithm.Adjust
{
    /// <summary>
    /// 抗差贝叶斯估计
    /// </summary>
    public class RobustBayesAdjustment
    {
        #region 公共参数定义
        /// <summary>
        /// 文件数,同步区数,向量组个数
        /// </summary>
        public int FileNumber;
        /// <summary>
        /// 总点数
        /// </summary>
        public int TotalPointNumber;
        /// <summary>
        /// 基线总数
        /// </summary>
        public int TotalBaselineNumber;
        /// <summary>
        /// 未知点个数
        /// </summary>
        public int TotalUnknownPointNumber;
        /// <summary>
        /// 点名数组指针,存储所有的点,不能遗漏,也不能重复,根据点的位置进行系数矩阵的建立
        /// </summary>
        public List<string> TotalPointName = new List<string>();
        /// <summary>
        /// 已知点数组
        /// </summary>
        public List<string> TotalKonwnPointName = new List<string>();
        /// <summary>
        /// 判断是否是已知点
        /// </summary>
        public bool[] IsKnownPoint;
        /// <summary>
        /// 近似坐标值,三个坐标构成一个XYZ,顺序与点号一一对应
        /// </summary>
        public double[] PointsAppXYZ;
        /// <summary>
        /// 基线向量起点的点号，是起点点号的序列
        /// </summary>
        public int[][] TotalBeginOfBaselineList = null;
        /// <summary>
        /// 基线向量终点的点号，是终点点号的序列
        /// </summary>
        public int[][] TotalEndOfBaselineList = null;
        /// <summary>
        /// 基线向量,3m*1维的矩阵块列,m=点号
        /// </summary>
        public double[][] lList = null;
        /// <summary>
        /// 基线向量协方差矩阵,3m*3m的方针,权矩阵
        /// </summary>
        public double[][] pList = null;
        /// <summary>
        /// 残差
        /// </summary>
        public double[][] VList = null;
        /// <summary>
        /// 残差的权倒数
        /// </summary>
        public double[][] QVList = null;
        /// <summary>
        /// 标准化残差,单位权残差
        /// </summary>
        public double[][] unitVlist = null;
        /// <summary>
        /// 先验坐标值
        /// </summary>
        public double[] PrioriX = null;
        /// <summary>
        /// 先验坐标的协方差矩阵
        /// </summary>
        public double[] PrioriCovX = null;

        public double[] ATPA, ATPL, dXYZ;
        public double MEDIAN = 0.0;
        public double sigma;
        public List<double> tmpMaxX = new List<double>();


        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BasicInfor"></param>
        /// <param name="TotalPointXYZ"></param>
        /// <param name="totalBeginOfBaselineList"></param>
        /// <param name="totalEndOfBaselineList"></param>
        /// <param name="bList"></param>
        /// <param name="ppList"></param>
        /// <param name="PPrioriX"></param>
        /// <param name="PPrioriCovX"></param>
        public RobustBayesAdjustment(AdjustBasicInformation BasicInfor, double[] TotalPointXYZ, 
            List<int[]> totalBeginOfBaselineList, List<int[]> totalEndOfBaselineList, double[][] bList, double[][] ppList,double[] PPrioriX, double[] PPrioriCovX)
        {
            this.FileNumber = BasicInfor.FileNumber;
            this.TotalPointNumber = BasicInfor.TotalPointNumber;
            this.TotalUnknownPointNumber = BasicInfor.UnknowPointnumber;
            this.TotalBaselineNumber = BasicInfor.TotalBaselineNumber;
            this.TotalPointName = BasicInfor.TotalPointName;
            this.TotalKonwnPointName = BasicInfor.KnownPointName;

            PointsAppXYZ = new double[TotalPointXYZ.Length];
            TotalPointXYZ.CopyTo(PointsAppXYZ, 0);//copyto????

            //初始化，否则会出错
            TotalBeginOfBaselineList = new int[FileNumber][];
            TotalEndOfBaselineList = new int[FileNumber][];

            lList = new double[FileNumber][];
            pList = new double[FileNumber][];
            VList = new double[FileNumber][];
            QVList = new double[FileNumber][];
            unitVlist = new double[FileNumber][];


            PrioriX = new double[TotalPointNumber * 3];
            PrioriCovX = new double[TotalPointNumber * 3 * (TotalPointNumber * 3 + 1) / 2];
            this.PrioriX = PPrioriX;
            this.PrioriCovX = PPrioriCovX;
            
            for (int i = 0; i < FileNumber; i++)
            {
                double[] pi = new double[ppList[i].Length];
                double[] li = new double[bList[i].Length];
                ppList[i].CopyTo(pi, 0);
                bList[i].CopyTo(li, 0);
                this.pList[i] = (pi);
                this.lList[i] = (li);

                this.TotalBeginOfBaselineList[i] = totalBeginOfBaselineList[i];
                this.TotalEndOfBaselineList[i] = totalEndOfBaselineList[i];
            }           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BasicInfor"></param>
        /// <param name="TotalPointXYZ"></param>
        /// <param name="totalBeginOfBaselineList"></param>
        /// <param name="totalEndOfBaselineList"></param>
        /// <param name="bList"></param>
        /// <param name="ppList"></param>
        public RobustBayesAdjustment(AdjustBasicInformation BasicInfor, double[] TotalPointXYZ,
           List<int[]> totalBeginOfBaselineList, List<int[]> totalEndOfBaselineList, double[][] bList, double[][] ppList)
        {
            this.FileNumber = BasicInfor.FileNumber;
            this.TotalPointNumber = BasicInfor.TotalPointNumber;
            this.TotalUnknownPointNumber = BasicInfor.UnknowPointnumber;
            this.TotalBaselineNumber = BasicInfor.TotalBaselineNumber;
            this.TotalPointName = BasicInfor.TotalPointName;
            this.TotalKonwnPointName = BasicInfor.KnownPointName;

            PointsAppXYZ = new double[TotalPointXYZ.Length];
            TotalPointXYZ.CopyTo(PointsAppXYZ, 0);//copyto????

            //初始化，否则会出错
            TotalBeginOfBaselineList = new int[FileNumber][];
            TotalEndOfBaselineList = new int[FileNumber][];

            lList = new double[FileNumber][];
            pList = new double[FileNumber][];
            VList = new double[FileNumber][];
            QVList = new double[FileNumber][];
            unitVlist = new double[FileNumber][];
            PrioriX = new double[TotalPointNumber * 3];
            PrioriCovX = new double[TotalPointNumber * 3 * (TotalPointNumber * 3 + 1) / 2];


            for (int i = 0; i < FileNumber; i++)
            {
                double[] pi = new double[ppList[i].Length];
                double[] li = new double[bList[i].Length];
                ppList[i].CopyTo(pi, 0);
                bList[i].CopyTo(li, 0);
                this.pList[i] = (pi);
                this.lList[i] = (li);

                this.TotalBeginOfBaselineList[i] = totalBeginOfBaselineList[i];
                this.TotalEndOfBaselineList[i] = totalEndOfBaselineList[i];
            }
        }
        /// <summary>
        /// 最小二乘平差函数
        /// </summary>
        public void LeastSquare()
        {
            int t = 3 * TotalPointNumber;//总点数395
            int tt = t * (t + 1) / 2;
            ATPA = new double[tt];
            ATPL = new double[t];

            //协方差矩阵求逆,得到权矩阵
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = pList[i];
                int n = lList[i].Length;
                GetInverse(Pi, n);
            }

            //建立法方程
            GetATPA();
            //已知点处理
            SetKnownCondition();
            //残差
            double max = GetdX();//此处对法矩阵求逆了？？
            //更新坐标
            UpdateXYZ();
            //计算残差
            GetLV();
            //计算二次型
            double VTPV = GetVTPV();

            sigma = Math.Sqrt(VTPV / (3 * TotalBaselineNumber - 3 * TotalUnknownPointNumber));
        }

        public void Parallel_LeastSquare()
        {
            int t = 3 * TotalPointNumber;//总点数395
            int tt = t * (t + 1) / 2;
            ATPA = new double[tt];
            ATPL = new double[t];

            //协方差矩阵求逆,得到权矩阵
            Parallel.For(0, FileNumber, (int i) =>
            //for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = pList[i];
                int n = lList[i].Length;
                GetInverse(Pi, n);
            });

            //建立法方程
            Parallel_GetATPA();
            //已知点处理
            SetKnownCondition();
            //残差
            double max = GetdX();//此处对法矩阵求逆了？？
            //更新坐标
            UpdateXYZ();
            //计算残差
            Parallel_GetLV();
            //计算二次型
            double VTPV = GetVTPV();

            sigma = Math.Sqrt(VTPV / (3 * TotalBaselineNumber - 3 * TotalUnknownPointNumber));
        }

        /// <summary>
        /// 抗差估计
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="eps"></param>
        /// <param name="k0"></param>
        /// <param name="k1"></param>
        /// <param name="m"></param>
        public void Robust(FactorYype fname, double eps, double k0, double k1, int m)
        {
            List<double[]> PListBackuptmp = new List<double[]>();

            //计算观测值的权矩阵
            for (int i = 0; i < FileNumber; i++)
            {
                int n = lList[i].Length;//基线向量总数
                double[] QVi = new double[n];
                double[] Pi = new double[pList[i].Length];//此时还是协方差矩阵
                pList[i].CopyTo(Pi, 0);

                int ii = 0;
                for (int j = 0; j < n; j++)
                {
                    QVi[ii] = Pi[GetSunScript(j, j)];//参见测量平差程序设计P117公式4-5-5,协方差矩阵的对角线元素
                    ii++;
                }
                QVList[i] = (QVi);

                GetInverse(Pi, n);//协方差矩阵求逆

                PListBackuptmp.Add(Pi);
                pList[i] = Pi;//此时,pList中的元素是权,协方差矩阵的逆矩阵
            }
            //将观测值的权备份到数组PListBackup
            List<double[]> PListBackup = new List<double[]>();
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = new double[pList[i].Length];
                pList[i].CopyTo(Pi, 0);
                PListBackup.Add(Pi);
            }

            //将L备份,修改
            //double[][] VListBackup = new double[FileNumber][];
            //for (int i = 0; i < FileNumber; i++)
            //{
            //    double[] Li = lList[i];//当前文件的基线向量观测值个数,是下面两个变量长度的三倍
            //    int[] BegPoint = TotalBeginOfBaselineList[i];//当前文件的基线向量起点点号
            //    int[] EndPoint = TotalEndOfBaselineList[i];//当前文件的基线向量终点点号

            //    double[] Vi = new double[BegPoint.Length * 3];
            //    for (int j = 0; j < BegPoint.Length; j++)
            //    {
            //        int k11 = BegPoint[j];
            //        int k22 = EndPoint[j];
            //        Vi[j * 3 + 0] = (PointsAppXYZ[3 * k22 + 0] - PointsAppXYZ[3 * k11 + 0]) - Li[3 * j + 0];//X (PointsAppXYZ[3 * k2 + 0] - PointsAppXYZ[3 * k1 + 0]) - Li[3 * j + 0]
            //        Vi[j * 3 + 1] = (PointsAppXYZ[3 * k22 + 1] - PointsAppXYZ[3 * k11 + 1]) - Li[3 * j + 1];//Y
            //        Vi[j * 3 + 2] = (PointsAppXYZ[3 * k22 + 2] - PointsAppXYZ[3 * k11 + 2]) - Li[3 * j + 2];//Z
            //    }
            //    VListBackup[i] = Vi;
            //}

            //抗差估计迭代
            for (int i = 0; i < FileNumber; i++)
            {
                int n = QVList[i].Length;//1209
                double[] unitVi = new double[n];//权因子，初始赋予初值1
                for (int j = 0; j < n; j++)
                {
                    unitVi[j] = 1.0;
                }
                unitVlist[i] = unitVi;
            }

            int No = 0;//迭代次数
            bool IsStill = true;
            List<double[]> Max = new List<double[]>();
            while (IsStill)
            {
                No++;
                for (int i = 0; i < FileNumber; i++)
                {
                    double[] unintVi = unitVlist[i];//1209
                    int nk = lList[i].Length;//1209
                    int index = 0;
                    for (int j = 0; j < nk; j++)
                    {
                        for (int k = 0; k <= j; k++)
                        {
                            if (unintVi[j] < 1.0 || unintVi[k] < 1.0)
                            {
                                pList[i][index] = pList[i][index] * Math.Sqrt(unintVi[j] * unintVi[k]);
                                if (pList[i][index] == 0 && k == j)
                                {
                                    pList[i][index] = Math.Pow(10, -20);
                                }
                            }
                            else
                            {
                                pList[i][index] = pList[i][index];
                            }
                            index++;
                        }
                    }
                    double[] tmpPi = new double[pList[i].Length];
                    for (int j = 0; j < pList[i].Length; j++)
                    {
                        tmpPi[j] = pList[i][j];
                    }
                    GetInverse(tmpPi, nk);//求逆又回到协方差矩阵

                    double[] tmpQVi = new double[nk];
                    for (int k = 0; k < nk; k++)
                    {
                        tmpQVi[k] = tmpPi[GetSunScript(k, k)];
                    }
                    QVList[i] = tmpQVi;
                }

                GetATPA();//组成法方程
                //GetRobustATPA(VListBackup);
                SetKnownCondition();//已知点处理,默认第一个点是已知点

                double max = GetdX(); //残差,对法方程系数阵求逆了
                //Max.Add(max);
                UpdateXYZ(); //更新坐标

                GetLV();//计算最小二乘残差

                tmpMaxX.Add(max);
                //if(No > 1)
                //{
                //    if ((Math.Abs(tmpMaxX[No]) - Math.Abs(tmpMaxX[No-1])) < eps)
                //    {
                //        break;
                //    }
                //}
                if (Math.Abs(max) < eps && No > 1)
                {
                    break;
                }
                //权因子计算
                GetQV();//更新残差权矩阵（此前的法方程矩阵已经求逆，即是Qx）

                GetUnitV();//标准化残差计算

                for (int i = 0; i < FileNumber; i++)
                {
                    int n = unitVlist[i].Length;
                    for (int j = 0; j < n; j++)
                    {
                        unitVlist[i][j] = GetWeightFactor(FactorYype.IGG3, unitVlist[i][j] / MEDIAN, k0, k1, m);
                    }
                }
            }//while循环结束

            //计算观测值降权的个数
            int dd = 0;
            for (int i = 0; i < FileNumber; i++)
            {
                double[] unitV = unitVlist[i];
                double[] Pi = pList[i];
                for (int j = 0; j < unitV.Length; j++)
                {
                    if (unitV[j] < 1.0 || Pi[j] == Math.Pow(10, -30))//根据两步同步判断是否是作为粗差处理
                        dd++;

                }
            }
            if (dd == 0)//所有观测值均未降权
            {
               
            }
            //计算二次型
            double VTPV = GetVTPV();
            int freedom = TotalBaselineNumber * 3 - dd - (TotalPointNumber - 1) * 3;//已知点个数为1
            sigma = Math.Sqrt(VTPV / freedom);
        }

        public void Parallel_Robust(FactorYype fname, double eps, double k0, double k1, int m)
        {
            List<double[]> PListBackuptmp = new List<double[]>();

            //计算观测值的权矩阵
            Parallel.For(0, FileNumber, (int i) =>
            //for (int i = 0; i < FileNumber; i++)
            {
                int n = lList[i].Length;//基线向量总数
                double[] QVi = new double[n];
                double[] Pi = new double[pList[i].Length];//此时还是协方差矩阵
                pList[i].CopyTo(Pi, 0);

                int ii = 0;
                for (int j = 0; j < n; j++)
                {
                    QVi[ii] = Pi[GetSunScript(j, j)];//参见测量平差程序设计P117公式4-5-5,协方差矩阵的对角线元素
                    ii++;
                }
                lock(QVList)
                {
                    QVList[i] = (QVi);
                }
                GetInverse(Pi, n);//协方差矩阵求逆

                PListBackuptmp.Add(Pi);
                lock(pList)
                {
                    pList[i] = Pi;//此时,pList中的元素是权,协方差矩阵的逆矩阵
                }
            });
            //将观测值的权备份到数组PListBackup
            List<double[]> PListBackup = new List<double[]>();
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = new double[pList[i].Length];
                pList[i].CopyTo(Pi, 0);
                PListBackup.Add(Pi);
            }

            //抗差估计迭代
            Parallel.For(0,FileNumber,(int i) =>
            //for (int i = 0; i < FileNumber; i++)
            {
                int n = QVList[i].Length;//1209
                double[] unitVi = new double[n];//权因子，初始赋予初值1
                for (int j = 0; j < n; j++)
                {
                    unitVi[j] = 1.0;
                }
                lock(unitVlist)
                {
                    unitVlist[i] = unitVi;
                }
                //unitVlist[i] = unitVi;
            });

            int No = 0;//迭代次数
            bool IsStill = true;
            List<double[]> Max = new List<double[]>();
            while (IsStill)
            {
                No++;
                Parallel.For(0,FileNumber,(int i) =>
                //for (int i = 0; i < FileNumber; i++)
                {
                    double[] unintVi = unitVlist[i];//1209
                    int nk = lList[i].Length;//1209
                    int index = 0;
                    for (int j = 0; j < nk; j++)
                    {
                        for (int k = 0; k <= j; k++)
                        {
                            if (unintVi[j] < 1.0 || unintVi[k] < 1.0)
                            {
                                pList[i][index] = pList[i][index] * Math.Sqrt(unintVi[j] * unintVi[k]);
                                if (pList[i][index] == 0 && k == j)
                                {
                                    pList[i][index] = Math.Pow(10, -20);
                                }
                            }
                            else
                            {
                                pList[i][index] = pList[i][index];
                            }
                            index++;
                        }
                    }
                    double[] tmpPi = new double[pList[i].Length];
                    for (int j = 0; j < pList[i].Length; j++)
                    {
                        tmpPi[j] = pList[i][j];
                    }
                    GetInverse(tmpPi, nk);//求逆又回到协方差矩阵

                    double[] tmpQVi = new double[nk];
                    for (int k = 0; k < nk; k++)
                    {
                        tmpQVi[k] = tmpPi[GetSunScript(k, k)];
                    }
                    lock(QVList)
                    {
                        QVList[i] = tmpQVi;
                    }
                    //QVList[i] = tmpQVi;
                });

                Parallel_GetATPA();//组成法方程
                //GetRobustATPA(VListBackup);
                SetKnownCondition();//已知点处理,默认第一个点是已知点

                double max = GetdX(); //残差,对法方程系数阵求逆了
                //Max.Add(max);
                UpdateXYZ(); //更新坐标

                Parallel_GetLV();//计算最小二乘残差

                tmpMaxX.Add(max);
                //if(No > 1)
                //{
                //    if ((Math.Abs(tmpMaxX[No]) - Math.Abs(tmpMaxX[No-1])) < eps)
                //    {
                //        break;
                //    }
                //}
                if (Math.Abs(max) < eps && No > 1)
                {
                    break;
                }
                //权因子计算
                Parallel_GetQV();//更新残差权矩阵（此前的法方程矩阵已经求逆，即是Qx）

                Parallel_GetUnitV();//标准化残差计算

                for (int i = 0; i < FileNumber; i++)
                {
                    int n = unitVlist[i].Length;
                    for (int j = 0; j < n; j++)
                    {
                        unitVlist[i][j] = GetWeightFactor(FactorYype.IGG3, unitVlist[i][j] / MEDIAN, k0, k1, m);
                    }
                }
            }//while循环结束

            //计算观测值降权的个数
            int dd = 0;
            for (int i = 0; i < FileNumber; i++)
            {
                double[] unitV = unitVlist[i];
                double[] Pi = pList[i];
                for (int j = 0; j < unitV.Length; j++)
                {
                    if (unitV[j] < 1.0 || Pi[j] == Math.Pow(10, -30))//根据两步同步判断是否是作为粗差处理
                        dd++;

                }
            }
            if (dd == 0)//所有观测值均未降权
            {

            }
            //计算二次型
            double VTPV = GetVTPV();
            int freedom = TotalBaselineNumber * 3 - dd - (TotalPointNumber - 1) * 3;//已知点个数为1
            sigma = Math.Sqrt(VTPV / freedom);
        }
        /// <summary>
        /// 贝叶斯估计
        /// </summary>
        public void BayesEstimate()
        {
            int t = 3 * TotalPointNumber;//总点数365
            int tt = t * (t + 1) / 2;
            ATPA = new double[tt];
            ATPL = new double[t];

            //协方差矩阵求逆,得到权矩阵
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = pList[i];
                int n = lList[i].Length;
                GetInverse(Pi, n);
            }

            //建立法方程
            GetATPA();
            //按照Bayes估计原理组成法方程
            GetPosterioriATPA();
            //已知点处理
            SetKnownCondition();
            //残差
            double max = GetdX();//此处对法矩阵求逆了
            //更新坐标
            UpdateXYZ();
            //计算残差
            GetLV();
            //计算二次型VTPV
            double VTPV = GetPosterVTPV();

            sigma = Math.Sqrt(VTPV / (3 * TotalBaselineNumber - 3 * TotalUnknownPointNumber));
        }

        public void Parallel_BayesEstimate()
        {
            int t = 3 * TotalPointNumber;//总点数365
            int tt = t * (t + 1) / 2;
            ATPA = new double[tt];
            ATPL = new double[t];

            //协方差矩阵求逆,得到权矩阵
            Parallel.For(0, FileNumber, (int i) =>
            //for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = pList[i];
                int n = lList[i].Length;
                GetInverse(Pi, n);
            });

            //建立法方程
            Parallel_GetATPA();
            //按照Bayes估计原理组成法方程
            GetPosterioriATPA();
            //已知点处理
            SetKnownCondition();
            //残差
            double max = GetdX();//此处对法矩阵求逆了
            //更新坐标
            UpdateXYZ();
            //计算残差
            Parallel_GetLV();
            //计算二次型VTPV
            double VTPV = GetPosterVTPV();

            sigma = Math.Sqrt(VTPV / (3 * TotalBaselineNumber - 3 * TotalUnknownPointNumber));
        }
        public void RobustBayes(FactorYype fname, double eps, double k0, double k1, int m)
        {
            List<double[]> PListBackuptmp = new List<double[]>();

            //计算观测值的权矩阵
            for (int i = 0; i < FileNumber; i++)
            {
                int n = lList[i].Length;//基线向量总数
                double[] QVi = new double[n];
                double[] Pi = new double[pList[i].Length];//此时还是协方差矩阵
                pList[i].CopyTo(Pi, 0);

                int ii = 0;
                for (int j = 0; j < n; j++)
                {
                    QVi[ii] = Pi[GetSunScript(j, j)];//参见测量平差程序设计P117公式4-5-5,协方差矩阵的对角线元素
                    ii++;
                }
                QVList[i] = (QVi);

                GetInverse(Pi, n);//协方差矩阵求逆

                PListBackuptmp.Add(Pi);
                pList[i] = Pi;//此时,pList中的元素是权,协方差矩阵的逆矩阵
            }
            //将观测值的权备份到数组PListBackup
            List<double[]> PListBackup = new List<double[]>();
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = new double[pList[i].Length];
                pList[i].CopyTo(Pi, 0);
                PListBackup.Add(Pi);
            }

            //抗差估计迭代
            for (int i = 0; i < FileNumber; i++)
            {
                int n = QVList[i].Length;//1209
                double[] unitVi = new double[n];//权因子，初始赋予初值1
                for (int j = 0; j < n; j++)
                {
                    unitVi[j] = 1.0;
                }
                unitVlist[i] = unitVi;
            }

            int No = 0;//迭代次数
            bool IsStill = true;
            List<double[]> Max = new List<double[]>();
            while (IsStill)
            {
                No++;
                for (int i = 0; i < FileNumber; i++)
                {
                    double[] unintVi = unitVlist[i];//1209
                    int nk = lList[i].Length;//1209
                    int index = 0;
                    for (int j = 0; j < nk; j++)
                    {
                        for (int k = 0; k <= j; k++)
                        {
                            if (unintVi[j] < 1.0 || unintVi[k] < 1.0)
                            {
                                pList[i][index] = pList[i][index] * Math.Sqrt(unintVi[j] * unintVi[k]);
                                //pList[i][index] = PListBackup[i][index] * Math.Sqrt(unintVi[j] * unintVi[k]);
                                if (pList[i][index] == 0 && k == j)
                                {
                                    pList[i][index] = Math.Pow(10, -20);
                                }
                            }
                            else
                            {
                                pList[i][index] = pList[i][index];
                            }
                            index++;
                        }
                    }
                    double[] tmpPi = new double[pList[i].Length];
                    for (int j = 0; j < pList[i].Length; j++)
                    {
                        tmpPi[j] = pList[i][j];
                    }
                    GetInverse(tmpPi, nk);//求逆又回到协方差矩阵

                    double[] tmpQVi = new double[nk];
                    for (int k = 0; k < nk; k++)
                    {
                        tmpQVi[k] = tmpPi[GetSunScript(k, k)];
                    }
                    QVList[i] = tmpQVi;
                }

                GetATPA();//组成法方程
                GetPosterioriATPA();
                SetKnownCondition();//已知点处理,默认第一个点是已知点

                double max = GetdX(); //残差,对法方程系数阵求逆了
                //Max.Add(max);
                UpdateXYZ(); //更新坐标

                GetLV();//计算最小二乘残差

                tmpMaxX.Add(max);
                if (Math.Abs(max) < eps && No > 1)
                {
                    break;
                }
                //权因子计算
                GetQV();//更新残差权矩阵（此前的法方程矩阵已经求逆，即是Qx）

                GetUnitV();//标准化残差计算

                for (int i = 0; i < FileNumber; i++)
                {
                    int n = unitVlist[i].Length;
                    for (int j = 0; j < n; j++)
                    {
                        unitVlist[i][j] = GetWeightFactor(FactorYype.IGG3, unitVlist[i][j] / MEDIAN, k0, k1, m);
                    }
                }
            }//while循环结束

            //计算观测值降权的个数
            int dd = 0;
            for (int i = 0; i < FileNumber; i++)
            {
                double[] unitV = unitVlist[i];
                double[] Pi = pList[i];
                for (int j = 0; j < unitV.Length; j++)
                {
                    if (unitV[j] < 1.0 || Pi[j] == Math.Pow(10, -30))//根据两步同步判断是否是作为粗差处理
                        dd++;

                }
            }
            if (dd == 0)//所有观测值均未降权
            {

            }
            //计算二次型
            double VTPV = GetPosterVTPV();

            int freedom = TotalBaselineNumber * 3 - dd - (TotalPointNumber - 1) * 3;//已知点个数为1
            sigma = Math.Sqrt(VTPV / freedom);
        }
        public void Parallel_RobustBayes(FactorYype fname, double eps, double k0, double k1, int m)
        {
            double[][] PListBackup = new double[FileNumber][];

            //计算观测值的权矩阵
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Parallel.For(0, FileNumber, (int i) =>
            //for (int i = 0; i < FileNumber; i++)
            {
                int n = lList[i].Length;//基线向量总数
                double[] QVi = new double[n];
                double[] Pi = new double[pList[i].Length];//此时还是协方差矩阵
                pList[i].CopyTo(Pi, 0);

                int ii = 0;
                for (int j = 0; j < n; j++)
                {
                    QVi[ii] = Pi[GetSunScript(j, j)];//参见测量平差程序设计P117公式4-5-5,协方差矩阵的对角线元素
                    ii++;
                }
                lock(QVList)
                {
                    QVList[i] = (QVi);
                }
                //QVList[i] = (QVi);

                GetInverse(Pi, n);//协方差矩阵求逆

                PListBackup[i] = new double[Pi.Length];
                Pi.CopyTo(PListBackup[i], 0);
                lock(pList)
                {
                    pList[i] = Pi;
                }
                //pList[i] = Pi;//此时,pList中的元素是权,协方差矩阵的逆矩阵
            });
            string sw1 = sw.ElapsedMilliseconds.ToString();
            //将观测值的权备份到数组PListBackup
            //List<double[]> PListBackup = new List<double[]>();
            //Parallel.For(0, FileNumber, (int i) =>
            ////for (int i = 0; i < FileNumber; i++)
            //{
            //    double[] Pi = new double[pList[i].Length];
            //    pList[i].CopyTo(Pi, 0);
            //    PListBackup.Add(Pi);
            //});

            //抗差估计迭代
            Parallel.For(0, FileNumber, (int i) =>
            //for (int i = 0; i < FileNumber; i++)
            {
                int n = QVList[i].Length;//1209
                double[] unitVi = new double[n];//权因子，初始赋予初值1
                for (int j = 0; j < n; j++)
                {
                    unitVi[j] = 1.0;
                }
                lock(unitVlist)
                {
                    unitVlist[i] = unitVi;
                }               
            });

            int No = 0;//迭代次数
            bool IsStill = true;
            //List<double[]> Max = new List<double[]>();
            while (IsStill)
            {
                Stopwatch sw2 = new Stopwatch();
                sw2.Start();
                No++;
                Parallel.For(0, FileNumber, (int i) =>
                //for (int i = 0; i < FileNumber; i++)
                {
                    double[] unintVi = unitVlist[i];//1209
                    int nk = lList[i].Length;//1209
                    int index = 0;
                    for (int j = 0; j < nk; j++)
                    {
                        for (int k = 0; k <= j; k++)
                        {
                            if (unintVi[j] < 1.0 || unintVi[k] < 1.0)
                            {
                                pList[i][index] = pList[i][index] * Math.Sqrt(unintVi[j] * unintVi[k]);
                                //pList[i][index] = PListBackup[i][index] * Math.Sqrt(unintVi[j] * unintVi[k]);
                                if (pList[i][index] == 0 && k == j)
                                {
                                    pList[i][index] = Math.Pow(10, -20);
                                }
                            }
                            else
                            {
                                pList[i][index] = pList[i][index];
                            }
                            index++;
                        }
                    }
                    double[] tmpPi = new double[pList[i].Length];
                    //for (int j = 0; j < pList[i].Length; j++)
                    //{
                    //    tmpPi[j] = pList[i][j];
                    //}
                    pList[i].CopyTo(tmpPi, 0);
                    GetInverse(tmpPi, nk);//求逆又回到协方差矩阵

                    double[] tmpQVi = new double[nk];
                    for (int k = 0; k < nk; k++)
                    {
                        tmpQVi[k] = tmpPi[GetSunScript(k, k)];
                    }
                    lock (QVList)
                    {
                        QVList[i] = tmpQVi;
                    }
                    //QVList[i] = tmpQVi;
                });

                Parallel_GetATPA();//组成法方程
                string sw_time = sw2.ElapsedMilliseconds.ToString();
                GetPosterioriATPA();
                SetKnownCondition();//已知点处理,默认第一个点是已知点

                double max = GetdX(); //残差,对法方程系数阵求逆了
                //Max.Add(max);
                UpdateXYZ(); //更新坐标

                Parallel_GetLV();//计算最小二乘残差

                tmpMaxX.Add(max);
                if (Math.Abs(max) < eps && No > 1)
                {
                    break;
                }
                //权因子计算
                Parallel_GetQV();//更新残差权矩阵（此前的法方程矩阵已经求逆，即是Qx）

                Parallel_GetUnitV();//标准化残差计算

                for (int i = 0; i < FileNumber; i++)
                {
                    int n = unitVlist[i].Length;
                    for (int j = 0; j < n; j++)
                    {
                        unitVlist[i][j] = GetWeightFactor(FactorYype.IGG3, unitVlist[i][j] / MEDIAN, k0, k1, m);
                    }
                }
            }//while循环结束

            //计算观测值降权的个数
            int dd = 0;
            for (int i = 0; i < FileNumber; i++)
            {
                double[] unitV = unitVlist[i];
                double[] Pi = pList[i];
                for (int j = 0; j < unitV.Length; j++)
                {
                    if (unitV[j] < 1.0 || Pi[j] == Math.Pow(10, -30))//根据两步同步判断是否是作为粗差处理
                        dd++;

                }
            }
            if (dd == 0)//所有观测值均未降权
            {

            }
            //计算二次型
            double VTPV = GetPosterVTPV();

            int freedom = TotalBaselineNumber * 3 - dd - (TotalPointNumber - 1) * 3;//已知点个数为1
            sigma = Math.Sqrt(VTPV / freedom);
        }

        #region 平差基本工具
        /// <summary>
        /// 对称正定矩阵求逆：变量循环重新编号法（宋力杰著）
        /// </summary>
        /// <param name="a">存储下三角矩阵元素的对称正定矩阵</param>
        /// <param name="n">矩阵阶数</param>
        /// <returns></returns>
        public static bool GetInverse(double[] a, int n)
        {
            int k, i, j;
            double[] a0 = new double[n];
            for (k = 0; k < n; k++)
            {
                double a00 = a[0];
                if (a00 == 0.0)
                {
                    return false;
                }
                for (i = 1; i < n; i++)
                {
                    double ai0 = a[i * (i + 1) / 2];
                    if (i <= n - k - 1) { a0[i] = -ai0 / a00; }
                    else { a0[i] = ai0 / a00; }

                    for (j = 1; j <= i; j++)
                    {
                        a[(i - 1) * i / 2 + j - 1] = a[i * (i + 1) / 2 + j] + ai0 * a0[j];
                    }
                }
                for (i = 1; i < n; i++)
                {
                    a[(n - 1) * n / 2 + i - 1] = a0[i];
                }
                a[n * (n + 1) / 2 - 1] = 1.0 / a00;
            }
            return true;
        }

        public int GetSunScript(int i, int j)
        {
            return (i >= j) ? i * (i + 1) / 2 + j : j * (j + 1) / 2 + i;
        }
        #endregion

        #region GNSS网平差工具

        /// <summary>
        /// 计算ATPA和ATPL
        /// </summary>
        public void GetATPA()
        {
            int t = 3 * TotalPointNumber;
            int tt = t * (t + 1) / 2;
            ATPA = new double[tt];
            ATPL = new double[t];
            for (int i = 0; i < t; i++)
            {
                ATPL[i] = 0.0;
            }
            for (int i = 0; i < tt; i++)
            {
                ATPA[i] = 0.0;
            }
            
            GetLV();//误差方程自由项计算

            for (int i = 0; i < FileNumber; i++)
            {
                int[] BegPoint = TotalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                int[] EndPoint = TotalEndOfBaselineList[i];//当前文件的基线向量终点点号
                int m = BegPoint.Length;
                double[] Pi = pList[i];
                double[] li = VList[i];

                int nk = 3 * m;
                for (int s1 = 0; s1 < nk; s1++)
                {
                    int tttt = s1 / 3;
                    int i1 = 3 * BegPoint[s1 / 3] + s1 % 3;//??
                    int i2 = 3 * EndPoint[s1 / 3] + s1 % 3;//??
                    for (int s2 = 0; s2 < nk; s2++)
                    {
                        int j1 = 3 * BegPoint[s2 / 3] + s2 % 3;//误差方程系数中为-1的未知数编号
                        int j2 = 3 * EndPoint[s2 / 3] + s2 % 3;//误差方程系数中为 1的未知数编号
                        double p12 = Pi[GetSunScript(s1, s2)];

                        if (i1 >= j1) ATPA[GetSunScript(i1, j1)] += p12;
                        if (i1 >= j2) ATPA[GetSunScript(i1, j2)] -= p12;
                        if (i2 >= j1) ATPA[GetSunScript(i2, j1)] -= p12;
                        if (i2 >= j2) ATPA[GetSunScript(i2, j2)] += p12;

                        if (p12 == Math.Pow(10, -20)) p12 = 0.0;
                        double l = li[s2];
                        ATPL[i1] -= p12 * l;
                        ATPL[i2] += p12 * l;
                    }

                }
            }
        }

        public void Parallel_GetATPA()
        {
            int t = 3 * TotalPointNumber;
            int tt = t * (t + 1) / 2;
            ATPA = new double[tt];
            ATPL = new double[t];
            for (int i = 0; i < t; i++)
            {
                ATPL[i] = 0.0;
            }
            for (int i = 0; i < tt; i++)
            {
                ATPA[i] = 0.0;
            }

            GetLV();//误差方程自由项计算

            //Parallel.For(0, FileNumber, (int i) =>
            for (int i = 0; i < FileNumber; i++)
            {
                int[] BegPoint = TotalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                int[] EndPoint = TotalEndOfBaselineList[i];//当前文件的基线向量终点点号
                int m = BegPoint.Length;
                double[] Pi = pList[i];
                double[] li = VList[i];

                int nk = 3 * m;
                for (int s1 = 0; s1 < nk; s1++)
                {
                    int tttt = s1 / 3;
                    int i1 = 3 * BegPoint[s1 / 3] + s1 % 3;//??
                    int i2 = 3 * EndPoint[s1 / 3] + s1 % 3;//??
                    for (int s2 = 0; s2 < nk; s2++)
                    {
                        int j1 = 3 * BegPoint[s2 / 3] + s2 % 3;//误差方程系数中为-1的未知数编号
                        int j2 = 3 * EndPoint[s2 / 3] + s2 % 3;//误差方程系数中为 1的未知数编号
                        double p12 = Pi[GetSunScript(s1, s2)];

                        if (i1 >= j1) ATPA[GetSunScript(i1, j1)] += p12;
                        if (i1 >= j2) ATPA[GetSunScript(i1, j2)] -= p12;
                        if (i2 >= j1) ATPA[GetSunScript(i2, j1)] -= p12;
                        if (i2 >= j2) ATPA[GetSunScript(i2, j2)] += p12;

                        if (p12 == Math.Pow(10, -20)) p12 = 0.0;
                        double l = li[s2];
                        ATPL[i1] -= p12 * l;
                        ATPL[i2] += p12 * l;
                    }

                }
            }
        }



        /// <summary>
        /// 由于抗差估计中自由项L不能变化,所以将初始的L备份到VListBackup中
        /// </summary>
        /// <param name="VListBackup"></param>
        //public void GetRobustATPA(double[][] VListBackup)
        //{
        //    int t = 3 * TotalPointNumber;
        //    int tt = t * (t + 1) / 2;
        //    ATPA = new double[tt];
        //    ATPL = new double[t];
        //    for (int i = 0; i < t; i++)
        //    {
        //        ATPL[i] = 0.0;
        //    }
        //    for (int i = 0; i < tt; i++)
        //    {
        //        ATPA[i] = 0.0;
        //    }
        //    Random seed1 = new Random();
        //    Random seed2 = new Random();
        //    Random seed3 = new Random();
        //    int[] r1= new int[TotalPointNumber-1];
        //    int[] r2= new int[TotalPointNumber-1];
        //    int[] r3= new int[TotalPointNumber-1];
        //    for (int i = 1; i < TotalPointNumber; i++)
        //    {                
        //        r1[i-1] = 10000 - seed1.Next(0, 20000);//(-1000,1000)之间的随机数
        //        PointsAppXYZ[3 * i + 0] = PointsAppXYZ[3 * i + 0] + r1[i-1] * Math.Pow(10, -5);//(-0.1,0.1)之间的随机数                  
        //    }
        //    for (int i = 1; i < TotalPointNumber; i++)
        //    {            
        //        r2[i-1] = 10000 - seed2.Next(0, 20000);//(-1000,1000)之间的随机数
        //        PointsAppXYZ[3 * i + 1] = PointsAppXYZ[3 * i + 1] + r2[i-1] * Math.Pow(10, -5);//(-0.1,0.1)之间的随机数                
        //    }
        //    for (int i = 1; i < TotalPointNumber; i++)
        //    {
        //        r3[i-1] = 10000 - seed3.Next(0, 20000);//(-1000,1000)之间的随机数
        //        PointsAppXYZ[3 * i + 2] = PointsAppXYZ[3 * i + 2] + r3[i-1] * Math.Pow(10, -5);//(-0.1,0.1)之间的随机数     
        //    }


        //    GetLV();//误差方程自由项计算

        //    for (int i = 0; i < FileNumber; i++)
        //    {
        //        int[] BegPoint = TotalBeginOfBaselineList[i];//当前文件的基线向量起点点号
        //        int[] EndPoint = TotalEndOfBaselineList[i];//当前文件的基线向量终点点号
        //        int m = BegPoint.Length;
        //        double[] Pi = pList[i];
        //        double[] li = VListBackup[i];

        //        int nk = 3 * m;
        //        for (int s1 = 0; s1 < nk; s1++)
        //        {
        //            int tttt = s1 / 3;
        //            int i1 = 3 * BegPoint[s1 / 3] + s1 % 3;//??
        //            int i2 = 3 * EndPoint[s1 / 3] + s1 % 3;//??
        //            for (int s2 = 0; s2 < nk; s2++)
        //            {
        //                int j1 = 3 * BegPoint[s2 / 3] + s2 % 3;//误差方程系数中为-1的未知数编号
        //                int j2 = 3 * EndPoint[s2 / 3] + s2 % 3;//误差方程系数中为 1的未知数编号
        //                double p12 = Pi[GetSunScript(s1, s2)];

        //                if (i1 >= j1) ATPA[GetSunScript(i1, j1)] += p12;
        //                if (i1 >= j2) ATPA[GetSunScript(i1, j2)] -= p12;
        //                if (i2 >= j1) ATPA[GetSunScript(i2, j1)] -= p12;
        //                if (i2 >= j2) ATPA[GetSunScript(i2, j2)] += p12;

        //                if (p12 == Math.Pow(10, -20)) p12 = 0.0;
        //                double l = li[s2];
        //                ATPL[i1] -= p12 * l;
        //                ATPL[i2] += p12 * l;
        //            }

        //        }
        //    }
        //}
        /// <summary>
        /// 计算贝叶斯估计的ATPA和ATPL,在GetATPA之后计算
        /// </summary>
        public void GetPosterioriATPA()
        {
            GetInverse(PrioriCovX, 3 * TotalPointNumber);
            //求ATPA
            for(int i = 0; i < 3 * TotalPointNumber * (3 * TotalPointNumber + 1) / 2; i++)
            {
                ATPA[i] = ATPA[i] + PrioriCovX[i];
            }

            //求ATPL
            double[] Post_X = new double[3 * TotalPointNumber];
            for (int i = 0; i < 3 * TotalPointNumber;i++ )
            {
                Post_X[i] = 0.0;
            }
                for (int j = 0; j < 3 * TotalPointNumber; j++)
                {
                    for (int k = 0; k < 3 * TotalPointNumber; k++)
                    {
                        Post_X[j] +=  - PrioriCovX[GetSunScript(j, k)] * (PrioriX[k] - PointsAppXYZ[k]);
                    }
                    ATPL[j] = ATPL[j] + Post_X[j];
                }
        }
        /// <summary>
        /// 误差方程自由项或最小二乘残差的计算
        /// 若计算误差方程自由项，算出来的是+L,方程为V = A * X + L
        /// </summary>
        public void GetLV()
        {
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Li = lList[i];//当前文件的基线向量观测值个数,是下面两个变量长度的三倍
                int[] BegPoint = TotalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                int[] EndPoint = TotalEndOfBaselineList[i];//当前文件的基线向量终点点号
                int m = BegPoint.Length;
                double[] Vi = new double[m * 3];
                for (int j = 0; j < m; j++)
                {
                    int k1 = BegPoint[j];
                    int k2 = EndPoint[j];
                    Vi[j * 3 + 0] = (PointsAppXYZ[3 * k2 + 0] - PointsAppXYZ[3 * k1 + 0]) - Li[3 * j + 0];//X (PointsAppXYZ[3 * k2 + 0] - PointsAppXYZ[3 * k1 + 0]) - Li[3 * j + 0]
                    Vi[j * 3 + 1] = (PointsAppXYZ[3 * k2 + 1] - PointsAppXYZ[3 * k1 + 1]) - Li[3 * j + 1];//Y
                    Vi[j * 3 + 2] = (PointsAppXYZ[3 * k2 + 2] - PointsAppXYZ[3 * k1 + 2]) - Li[3 * j + 2];//Z
                }
                VList[i] = (Vi);
            }
        }

        public void Parallel_GetLV()
        {
            Parallel.For(0, FileNumber, (int i) =>
            //for (int i = 0; i < FileNumber; i++)
            {
                double[] Li = lList[i];//当前文件的基线向量观测值个数,是下面两个变量长度的三倍
                int[] BegPoint = TotalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                int[] EndPoint = TotalEndOfBaselineList[i];//当前文件的基线向量终点点号
                int m = BegPoint.Length;
                double[] Vi = new double[m * 3];
                for (int j = 0; j < m; j++)
                {
                    int k1 = BegPoint[j];
                    int k2 = EndPoint[j];
                    Vi[j * 3 + 0] = (PointsAppXYZ[3 * k2 + 0] - PointsAppXYZ[3 * k1 + 0]) - Li[3 * j + 0];//X (PointsAppXYZ[3 * k2 + 0] - PointsAppXYZ[3 * k1 + 0]) - Li[3 * j + 0]
                    Vi[j * 3 + 1] = (PointsAppXYZ[3 * k2 + 1] - PointsAppXYZ[3 * k1 + 1]) - Li[3 * j + 1];//Y
                    Vi[j * 3 + 2] = (PointsAppXYZ[3 * k2 + 2] - PointsAppXYZ[3 * k1 + 2]) - Li[3 * j + 2];//Z
                }
                lock(VList)
                {
                    VList[i] = (Vi);
                }
                
            });
        }
        /// <summary>
        /// 处理已知点，加一个很大的权,这里固定第一个点为已知点
        /// </summary>
        public void SetKnownCondition()
        {
            ATPA[GetSunScript(3 * 0 + 0, 3 * 0 + 0)] += Math.Pow(10, 30);
            ATPA[GetSunScript(3 * 0 + 1, 3 * 0 + 1)] += Math.Pow(10, 30);
            ATPA[GetSunScript(3 * 0 + 2, 3 * 0 + 2)] += Math.Pow(10, 30);
        }

        /// <summary>
        /// 参数平差值计算
        /// </summary>
        /// <returns></returns>
        public double GetdX()
        {
            //对法方程系数阵求逆
            int t = 3 * TotalPointNumber;
            GetInverse(ATPA, t);
            //if (!GetInverse(ATPA, t))
            //{
            //    throw new Exception("法方程系数阵求逆失败,不满秩！");
            //}

            dXYZ = new double[t];
            for (int i = 0; i < t; i++)
            {
                dXYZ[i] = 0.0;
            }

            double max = 0.0;
            for (int i = 0; i < t; i++)
            {
                double xi = 0.0;
                for (int j = 0; j < t; j++)
                {
                    xi -= ATPA[GetSunScript(i, j)] * ATPL[j]; // X = - inv(N) * U
                }
                dXYZ[i] += xi;
                if (Math.Abs(xi) > Math.Abs(max)) max = xi;
            }
            return max;
        }

        /// <summary>
        /// 更新坐标值为平差后的坐标值
        /// </summary>
        public void UpdateXYZ()
        {
            int n = PointsAppXYZ.Length;
            if (n != dXYZ.Length)
            {
                throw new Exception("出错！");                
            }
            for (int i = 0; i < n; i++)
            {
                PointsAppXYZ[i] += dXYZ[i];
            }
        }

        /// <summary>
        /// 计算残差V的权倒数,即Qv
        /// </summary>
        public void GetQV()
        {
            //计算观测值平差值权倒数
            for (int i = 0; i < FileNumber; i++)
            {
                int[] BegPoint = TotalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                int[] EndPoint = TotalEndOfBaselineList[i];//当前文件的基线向量终点点号

                int m = BegPoint.Length;
                double[] QVi = QVList[i];
                if (QVi.Length != 3 * m)
                {
                    throw new Exception("出错！");
                }
                for (int j = 0; j < m; j++)
                {
                    int k1 = BegPoint[j];//起点
                    int k2 = EndPoint[j];//终点

                    double q0 = ATPA[GetSunScript(3 * k1 + 0, 3 * k1 + 0)] + ATPA[GetSunScript(3 * k2 + 0, 3 * k2 + 0)] - 2.0 * ATPA[GetSunScript(3 * k1 + 0, 3 * k2 + 0)];
                    QVi[3 * j + 0] = QVi[3 * j + 0] - q0;
                    double q1 = ATPA[GetSunScript(3 * k1 + 1, 3 * k1 + 1)] + ATPA[GetSunScript(3 * k2 + 1, 3 * k2 + 1)] - 2.0 * ATPA[GetSunScript(3 * k1 + 1, 3 * k2 + 1)];
                    QVi[3 * j + 1] = QVi[3 * j + 1] - q1; 
                    double q2 = ATPA[GetSunScript(3 * k1 + 2, 3 * k1 + 2)] + ATPA[GetSunScript(3 * k2 + 2, 3 * k2 + 2)] - 2.0 * ATPA[GetSunScript(3 * k1 + 2, 3 * k2 + 2)];
                    QVi[3 * j + 2] = QVi[3 * j + 2] - q2;
                }
                QVList[i] = QVi;
            }
        }

        public void Parallel_GetQV()
        {
            //计算观测值平差值权倒数
            Parallel.For(0,FileNumber,(int i) =>
            //for (int i = 0; i < FileNumber; i++)
            {
                int[] BegPoint = TotalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                int[] EndPoint = TotalEndOfBaselineList[i];//当前文件的基线向量终点点号

                int m = BegPoint.Length;
                double[] QVi = QVList[i];
                if (QVi.Length != 3 * m)
                {
                    throw new Exception("出错！");
                }
                for (int j = 0; j < m; j++)
                {
                    int k1 = BegPoint[j];//起点
                    int k2 = EndPoint[j];//终点

                    double q0 = ATPA[GetSunScript(3 * k1 + 0, 3 * k1 + 0)] + ATPA[GetSunScript(3 * k2 + 0, 3 * k2 + 0)] - 2.0 * ATPA[GetSunScript(3 * k1 + 0, 3 * k2 + 0)];
                    QVi[3 * j + 0] = QVi[3 * j + 0] - q0;
                    double q1 = ATPA[GetSunScript(3 * k1 + 1, 3 * k1 + 1)] + ATPA[GetSunScript(3 * k2 + 1, 3 * k2 + 1)] - 2.0 * ATPA[GetSunScript(3 * k1 + 1, 3 * k2 + 1)];
                    QVi[3 * j + 1] = QVi[3 * j + 1] - q1;
                    double q2 = ATPA[GetSunScript(3 * k1 + 2, 3 * k1 + 2)] + ATPA[GetSunScript(3 * k2 + 2, 3 * k2 + 2)] - 2.0 * ATPA[GetSunScript(3 * k1 + 2, 3 * k2 + 2)];
                    QVi[3 * j + 2] = QVi[3 * j + 2] - q2;
                }
                lock(QVList)
                {
                    QVList[i] = QVi;
                }
                //QVList[i] = QVi;
            });
        }

        /// <summary>
        /// 计算单位权残差，中位数计算方差因子
        /// </summary>
        public void GetUnitV()
        {
            for (int i = 0; i < FileNumber; i++)
            {
                double[] QVi = QVList[i];
                double[] Vi = VList[i];
                int n = QVi.Length;
                double[] Wi = new double[n];
                for (int j = 0; j < n; j++)
                {
                    double mi = Math.Sqrt(QVi[j]);
                    if(Math.Abs(mi) < Math.Pow(10,-15))
                    {
                        Wi[j] = 1.0;
                    }
                    else if (Math.Abs(mi) >= Math.Pow(10, 10))
                    {
                        Wi[j] = 0.0;//作粗差处理
                    }
                    else
                    {
                        Wi[j] = Vi[j] / mi;
                    }
                }
                unitVlist[i] = Wi;
            }
            MEDIAN = GetMedian(unitVlist, true) * 1.4826;
        }

        public void Parallel_GetUnitV()
        {
            Parallel.For(0, FileNumber, (int i) =>
            //for (int i = 0; i < FileNumber; i++)
            {
                double[] QVi = QVList[i];
                double[] Vi = VList[i];
                int n = QVi.Length;
                double[] Wi = new double[n];
                for (int j = 0; j < n; j++)
                {
                    double mi = Math.Sqrt(QVi[j]);
                    if (Math.Abs(mi) < Math.Pow(10, -15))
                    {
                        Wi[j] = 1.0;
                    }
                    else if (Math.Abs(mi) >= Math.Pow(10, 10))
                    {
                        Wi[j] = 0.0;//作粗差处理
                    }
                    else
                    {
                        Wi[j] = Vi[j] / mi;
                    }
                }
                lock(unitVlist)
                {
                    unitVlist[i] = Wi;
                }
                //unitVlist[i] = Wi;
            });
            MEDIAN = GetMedian(unitVlist, true) * 1.4826;
        }

        /// <summary>
        /// 中位数计算
        /// </summary>
        /// <param name="unitVList"></param>
        /// <param name="IsAbs"></param>
        /// <returns></returns>
        public double GetMedian(double[][] unitVList, bool IsAbs)
        {
            int count = 0;
            for (int i = 0; i < FileNumber; i++)
            {
                count += unitVlist[i].Length;
            }
            double[] allV = new double[count];
            int k = 0;
            for (int i = 0; i < FileNumber; i++)
            {
                double[] V = unitVlist[i];
                for (int j = 0; j < V.Length;j++)
                {
                    allV[k + j] = V[j];
                }
                k += V.Length;
            }
            double mean = GetMedian(allV, count, IsAbs);
            return mean;

        }

        public double GetMedian(double[] pp, int n, bool IsAbs)
        {
            double[] p = new double[n];
            if (IsAbs)
            {
                for (int i = 0; i < n; i++) p[i] = Math.Abs(pp[i]);
            }
            else
            {
                for (int i = 0; i < n; i++) p[i] = pp[i];
            }
            int k = (n / 2);
            while (k > 0)
            {
                for (int j = k; j <= n - 1; j++)
                {
                    double t = p[j];
                    int i = j - k;
                    while ((i >= 0) && (p[i] > t))
                    {
                        p[i + k] = p[i];
                        i = i - k;
                    }
                    p[i + k] = t;
                }
                k = (k / 2);
            }
            double mean = (n % 2 == 1) ? p[n / 2] : (p[n / 2] + p[n / 2 - 1]) / 2.0;
            return mean;
        }
        /// <summary>
        /// 残差二次型计算,先计算PV(在粗差探测中还能利用)，再计算VTPV
        /// </summary>
        public double GetVTPV()
        {
            double VTPV = 0.0;
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = pList[i];
                double[] Vi = VList[i];
                int m = Vi.Length;
                double[] PVi = new double[m];
                double pvv = 0.0;
                for (int j = 0; j < m; j++)
                {
                    double pvj = 0.0;
                    for (int k = 0; k < m; k++)
                    {
                        pvj += Pi[GetSunScript(j, k)] * Vi[k];
                    }
                    PVi[j] = pvj;
                    pvv += pvj * Vi[j];                    
                }
                VTPV += pvv;
            }
            return VTPV;
        }

        /// <summary>
        /// 贝叶斯估计加入先验信息之后，求单位权中误差的公式也变了
        /// </summary>
        /// <returns></returns>
        public double GetPosterVTPV()
        {
            double VTPV = 0.0;
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = pList[i];
                double[] Vi = VList[i];
                int m = Vi.Length;
                double[] PVi = new double[m];
                double pvv = 0.0;
                for (int j = 0; j < m; j++)
                {
                    double pvj = 0.0;
                    for (int k = 0; k < m; k++)
                    {
                        pvj += Pi[GetSunScript(j, k)] * Vi[k];
                    }
                    PVi[j] = pvj;
                    pvv += pvj * Vi[j];
                }
                VTPV += pvv;
            }
            for (int i = 0; i < FileNumber;i++ )
            {
                double[] Pi = pList[i];
                double[] PVii = new double[3 * TotalPointNumber];
                double pvvv = 0.0;
                for (int j = 0; j < 3 * TotalPointNumber; j++)
                {
                    double pvjj = 0.0;
                    for(int k = 0; k < 3 * TotalPointNumber;k++ )
                    {
                        pvjj += PrioriCovX[GetSunScript(j, k)] * (PrioriX[k] - PointsAppXYZ[k]);
                    }
                    PVii[j] += pvjj;
                    pvvv += pvjj * (PrioriX[j] - PointsAppXYZ[j]);
                }
                VTPV += pvvv;
            }
                return VTPV;
        }

        /// <summary>
        /// 权因子计算
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="v"></param>
        /// <param name="k0"></param>
        /// <param name="k1"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private double GetWeightFactor(FactorYype fname, double v, double k0, double k1, int m)
        {
            double a = 0;
            double wi = 0;
            switch (fname)
            {
                case FactorYype.IGG1:
                    v = Math.Abs(v);
                    if (v <= k0)
                    {
                        wi = 1.0;
                    }
                    if (v > k1) { wi = 0; }//权等于零的话，存在非满秩的情况
                    if (v <= k1 && v > k0) { wi = k0 / v; }
                    break;
                case FactorYype.IGG3:
                    v = Math.Abs(v);
                    if (v <= k0)
                    {
                        wi = 1.0;
                    }
                    if (v > k1) { wi = 0; }
                    if (v <= k1 && v > k0)
                    {
                        a = (k1 - v) / (k1 - k0);
                        wi = k0 / v * Math.Pow(a, m);
                    }
                    break;
                case FactorYype.Huber:
                    v = Math.Abs(v);
                    if (v <= k0)
                    {
                        wi = 1.0;
                    }
                    else
                    {
                        wi = k0 / v;
                    }
                    break;
                default: break;

            }
            return wi;
        }

        #endregion
    }
}
