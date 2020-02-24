using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Geo.Coordinates;
using Geo.Utils;
using System.Diagnostics;//Time
using System.Threading.Tasks;//Parallel

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 抗差估计----废除！！！
    /// </summary>
    public class GnssRobustAdjustment
    {
        #region 公共参数的定义
        /// <summary>
        /// 文件数，同步区数，向量组总数
        /// </summary>
        public int FileNumber;// { get; set; }
        /// <summary>
        /// 总点数
        /// </summary>
        public int totalPointNumber;//  { get; set; }
        /// <summary>
        /// 向量总数
        /// </summary>
        public int totalBaselineNumber;// { get; set; }
        /// <summary>
        /// 未知点数=总点数-已知点数
        /// </summary>
        public int totalUnknowPointnumber;//  { get; set; }
        /// <summary>
        /// 点名指针数组,存储所有的点，不能遗漏也不能重复，根据点的位置进行系数矩阵的建立
        /// </summary>
        public List<string> totalPointName = new List<string>();//
        /// <summary>
        /// 已知点数组
        /// </summary>
        public List<string> totalKnownPointName = new List<string>();//
        /// <summary>
        /// 是否是已知点
        /// </summary>
        public bool[] isKnownPoint;//  { get; set; }

        /// <summary>
        /// 点坐标近似值
        /// 坐标数组，三个坐标构成一个XYZ，顺序与点号一一对应
        /// </summary>
        public List<Vector> pointsXYZ = new List<Vector>();

        //public List<double[][]> AList = new List<double[][]>();

        //按照每个文件单独存储基线向量、协方差矩阵、基线向量点号
        /// <summary>
        /// 基线向量起点点号， 是起点点号的序列 
        /// </summary>
        public List<int[]> totalBeginOfBaselineList = new List<int[]>();
        /// <summary>
        /// 基线向量终点点号
        /// </summary>
        public List<int[]> totalEndOfBaselineList = new List<int[]>();

        /// <summary>
        /// 基线向量，3m*1一维的矩阵块列，m=点号
        /// </summary>
        public List<double[]> lList = new List<double[]>();
        /// <summary>
        /// 基线向量协方差矩阵列，3m*3m的方阵，权矩阵
        /// </summary>
        private  List<double[]> pList = new List<double[]>();
        /// <summary>
        /// 残差
        /// </summary>
        public List<double[]> VList = new List<double[]>();
        /// <summary>
        ///残差的权倒数
        /// </summary>
        public List<double[]> QVList = new List<double[]>();
        /// <summary>
        /// 标准化残差，单位权残差
        /// </summary>
        public List<double[]> unitVList = new List<double[]>();


        public double[] ATPA, ATPL,dXYZ;
        public double MEDIAN = 0.0;
        public double Sigma;
        public List<double> tmpMaxX = new List<double>();
        //
       

        //public double eps = 0.005;
        //public double k0 = 1.5, k1 = 3.50;
        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="totalFileNum"></param>
        /// <param name="totalPointNum"></param>
        /// <param name="totalUnPointNum"></param>
        /// <param name="totalObsBaselineNum"></param>
        /// <param name="totalPointStr">点名</param>
        ///<param name="totalKnownPointStr">已知点点名</param>
        /// <param name="totalPointXYZ">点坐标，其中已知点的坐标为真值</param>
        /// <param name="AList"></param>
        /// <param name="bList"></param>
        /// <param name="PList"></param>
        public GnssRobustAdjustment(int totalFileNum,
            int totalPointNum,
            int totalUnPointNum, 
            int totalObsBaselineNum, 
            List<string> totalPointStr,
            List<string> totalKnownPointStr,
            List<Vector> totalPointXYZ,
            List<double[]> AList, 
            List<double[]> bList,
            List<double[]> PList)
        {
            //
            this.FileNumber = totalFileNum;
            this.totalPointNumber = totalPointNum;
            this.totalUnknowPointnumber = totalUnPointNum;
            this.totalBaselineNumber = totalObsBaselineNum;
            this.totalPointName = totalPointStr;
            this.totalKnownPointName = totalKnownPointStr;
            //


        }


    
        /// <summary>
        /// 构造函数
        /// 没有系数矩阵，通过基线点序列判断系数是1or-1or0。
        /// </summary>
        /// <param name="totalFileNum"></param>
        /// <param name="totalPointNum"></param>
        /// <param name="totalUnPointNum"></param>
        /// <param name="totalObsBaselineNum"></param>
        /// <param name="totalPointStr"></param>
        /// <param name="totalKnownPointStr"></param>
        /// <param name="totalPointXYZ"></param>
        /// <param name="totalbeginOfBaselineList"></param>
        /// <param name="totalendOfBaselineList"></param>
        /// <param name="bList"></param>
        /// <param name="pList"></param>
        public GnssRobustAdjustment(int totalFileNum,
            int totalPointNum,
            int totalUnPointNum,
            int totalObsBaselineNum,
            List<string> totalPointStr,
            List<string> totalKnownPointStr,
            double[] totalPointXYZ,
            List<int[]> totalbeginOfBaselineList,
            List<int[]> totalendOfBaselineList,
            double[][] bList,
            double[][] pList)
        {
            //
            this.FileNumber = totalFileNum;
            this.totalPointNumber = totalPointNum;
            this.totalUnknowPointnumber = totalUnPointNum;
            this.totalBaselineNumber = totalObsBaselineNum;
            this.totalPointName = totalPointStr;
            this.totalKnownPointName = totalKnownPointStr;
           // this.pointsXYZ = totalPointXYZ;
            int n = totalPointXYZ.Length / 3;
            for (int i = 0; i < n; i++)
            {
                Vector xyz = new Vector(totalPointXYZ[3 * i + 0], totalPointXYZ[3 * i + 1], totalPointXYZ[3 * i + 2]);
                this.pointsXYZ.Add(xyz);
            }
            //
            this.totalBeginOfBaselineList = totalbeginOfBaselineList;
            this.totalEndOfBaselineList = totalendOfBaselineList;
            //
           // this.lList = bList;
            for (int i = 0; i < FileNumber; i++)
            {
                double[] pi = new double[pList[i].Length];
                double[] li = new double[bList[i].Length];


                pList[i].CopyTo(pi, 0);
                bList[i].CopyTo(li, 0);

                this.pList.Add(pi);
                this.lList.Add(li);
            }

            //




        }

        /// <summary>
        /// 最小二乘平差函数
        /// </summary>
        public void LeastSquares()
        {
            int t = 3 * totalPointNumber; 
            int tt = t * (t + 1) / 2;
            ATPA = new double[tt];
            ATPL = new double[t];
            //协方差矩阵求逆，得到权矩阵
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = pList[i];
                int n = lList[i].Length;
                GetInverse(Pi, n);

            }

            //建立法方差
            GetATPA();
            //已知点处理
            SetKnownCondition();
            //残差
            double max= GetdX();//此处对法矩阵求逆了。
            //更新坐标
            UpdateXYZ();
            //计算最小二乘残差
            GetLV();
            //计算二次型
            double VTPV = GetVTPV();

            Sigma = Math.Sqrt(VTPV / (3 * totalBaselineNumber - 3 * totalUnknowPointnumber));
 
        }


        /// <summary>
        /// 抗差估计
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="esp"></param>
        public void Robust(FactorYype fname, double eps,double k0,double k1,int m)
        {
            List<double[]> PListBackuptmp = new List<double[]>();

            //----------------------------------------------------------
            //计算观测值的权矩阵
            for (int i = 0; i < FileNumber; i++)
            {
                int n = lList[i].Length;
                double[] QVi = new double[n];
                double[] Pi = new double[pList[i].Length];//此时还是协方差矩阵
                pList[i].CopyTo(Pi, 0);
                
                int ii = 0;
                for (int j = 0; j < n; j++)
                {
                    QVi[ii] = Pi[GetSunScript(j, j)];
                    ii++;
                }
                QVList.Add(QVi);
                //权矩阵
                GetInverse(Pi, n);
                
                PListBackuptmp.Add(Pi);
                pList[i] = Pi;
            }
            //----------------------------------------------------------
            //计算残差的权倒数
            //GetATPA();
            //if (!GetInverse(ATPA, 3 * TotalPointNumber))//法方程系数矩阵求逆
            //{
            //    throw new Exception("法方程系数降秩，求逆失败！");
            //}

            //GetQV();//计算残差的权倒数

            //-----------------------------------------------------------
            //将观测值的权备份到数组PPList
            List<double[]> PListBackup = new List<double[]>();
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = new double[pList[i].Length];
                pList[i].CopyTo(Pi, 0);
                PListBackup.Add(Pi);
            }


            //-----------------------------------------------------------
            //抗差估计迭代
            for (int i = 0; i < FileNumber; i++)
            {
                int n = QVList[i].Length;
                double[] unitVi = new double[n];//权因子，初始赋予初值1
                for (int j = 0; j < n; j++) { unitVi[j] = 1.0; }
                if (unitVList.Count < FileNumber)
                {
                    unitVList.Add(unitVi);
                }
                else
                {
                    unitVList[i] = unitVi;
                }

            }

            int No = 0;//迭代次数
            bool IsStill = true;
            while (IsStill)
            {
                No++;
                //计算等价权
                for (int i = 0; i < FileNumber; i++)
                {
                    double[] unintVi = unitVList[i];//权因子
                    int nk = lList[i].Length;//观测值个数
                    int index = 0;
                    for (int j = 0; j < nk; j++)
                    {
                        for (int k = 0; k <= j; k++)
                        {
                            if (unintVi[j] < 1.0 || unintVi[k] < 1.0)
                            {
                                //pList[i][index] = PListBackup[i][index] * Math.Sqrt(unintVi[j] * unintVi[k]);
                                pList[i][index] = pList[i][index] * Math.Sqrt(unintVi[j] * unintVi[k]);
                                if (pList[i][index] == 0 && k == j)
                                {
                                    pList[i][index] = Math.Pow(10, -20);
                                }
                            }
                            else
                            {
                                //pList[i][index] = PListBackup[i][index];
                                pList[i][index] = pList[i][index];
                            }

                            //PList[i][index] = PListBackup[i][index];
                            index++;
                        }
                    }
                    double[] tmpPi = new double[pList[i].Length];
                    for (int j = 0; j < pList[i].Length; j++)
                    {
                        tmpPi[j] = pList[i][j];
                    }
                    //PList[i].CopyTo(tmpPi, 0);
                    GetInverse(tmpPi, nk);

                    //double[][] mPi = new double[nk][];

                    //for (int s = 0; s < nk; s++)
                    //{
                    //    mPi[s] = new double[nk];
                    //    for (int k = 0; k <= s; k++)
                    //    {
                    //        mPi[s][k] = PList[i][GetSunScript(s, k)];
                    //    }
                    //}
                    //for (int s = 0; s < nk; s++)
                    //{

                    //    for (int k = s + 1; k < nk; k++)
                    //    {
                    //        mPi[s][k] = mPi[k][s];
                    //    }
                    //}
                    //Geo.Algorithm.Matrix mmPi = new Geo.Algorithm.Matrix(mPi);
                    //Geo.Algorithm.Matrix InvPi = mmPi.Inverse;

                    double[] tmpQVi = new double[nk];
                    for (int k = 0; k < nk; k++) { tmpQVi[k] = tmpPi[GetSunScript(k, k)]; }//求逆有错误！！！！
                    //for (int k = 0; k < nk; k++) { tmpQVi[k] = InvPi[k, k]; }
                    QVList[i] = tmpQVi;//残差的权矩阵要时刻更新，新的残差权矩阵=P_last-A*(Q_X)*AT
                }

                GetATPA();//组成法方程

                SetKnownCondition();//已知点处理

                //残差
                double max = GetdX();//此处对法矩阵求逆了。

                //更新坐标
                UpdateXYZ();

                //计算最小二乘残差
                GetLV();
                tmpMaxX.Add(max);
                if (Math.Abs(max) < eps && No > 1)
                {
                    break;
                }

                //---------------------------------------------------------------------
                //权因子计算
                GetQV();//更新残差权矩阵，（此前的法方程矩阵已经求逆，即是QX）
                GetUnitV();//标准化残差计算
                for (int i = 0; i < FileNumber; i++)
                {
                    //double[] unitVi = unitVList[i];
                    int n = unitVList[i].Length;
                    for (int j = 0; j < n; j++)
                    {
                        unitVList[i][j] = GetWeightFactor(FactorYype.IGG3, unitVList[i][j] / MEDIAN,k0, k1,m);
                    }
                }


            }//while循环结束


            //计算观测值降权的个数
            int dd = 0;//降权观测值个数
            for (int i = 0; i < FileNumber; i++)
            {
                double[] unitV = unitVList[i];
                double[] Pi = pList[i];
                for (int j = 0; j < unitV.Length; j++)
                {
                    if (unitV[j] < 1.0 || Pi[j] == Math.Pow(10, -30)) dd++;  //这里根据两步同步判断是否是作为粗差处理
                }
            }
            if (dd == 0)//所有观测值均未降权
            { }
            //计算二次型
            double VTPV = GetVTPV();
            int freedom = totalBaselineNumber * 3 - dd - (totalPointNumber - 1) * 3;//已知点个数为1
            Sigma = Math.Sqrt(VTPV / freedom);
        }

        /// <summary>
        /// 抗差估计
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="esp"></param>
        public void ParallRobust(FactorYype fname, double eps, double k0, double k1, int m)
        {
            List<double[]> PListBackuptmp = new List<double[]>();

            //----------------------------------------------------------
            //计算观测值的权矩阵
            for (int i = 0; i < FileNumber; i++)
            {
                int n = lList[i].Length;
                double[] QVi = new double[n];
                double[] Pi = new double[pList[i].Length];//此时还是协方差矩阵
                pList[i].CopyTo(Pi, 0);

                int ii = 0;
                for (int j = 0; j < n; j++)
                {
                    QVi[ii] = Pi[GetSunScript(j, j)];
                    ii++;
                }
                QVList.Add(QVi);
                //权矩阵
                GetInverse(Pi, n);

                PListBackuptmp.Add(Pi);
                pList[i] = Pi;
            }
            //----------------------------------------------------------
            //计算残差的权倒数
            //GetATPA();
            //if (!GetInverse(ATPA, 3 * TotalPointNumber))//法方程系数矩阵求逆
            //{
            //    throw new Exception("法方程系数降秩，求逆失败！");
            //}

            //GetQV();//计算残差的权倒数

            //-----------------------------------------------------------
            //将观测值的权备份到数组PPList
            List<double[]> PListBackup = new List<double[]>();
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi = new double[pList[i].Length];
                pList[i].CopyTo(Pi, 0);
                PListBackup.Add(Pi);
            }


            //-----------------------------------------------------------
            //抗差估计迭代
            for (int i = 0; i < FileNumber; i++)
            {
                int n = QVList[i].Length;
                double[] unitVi = new double[n];//权因子，初始赋予初值1
                for (int j = 0; j < n; j++) { unitVi[j] = 1.0; }
                if (unitVList.Count < FileNumber)
                {
                    unitVList.Add(unitVi);
                }
                else
                {
                    unitVList[i] = unitVi;
                }

            }

            int No = 0;//迭代次数
            bool IsStill = true;
            while (IsStill)
            {
                No++;
                //计算等价权
                for (int i = 0; i < FileNumber; i++)
                {
                    double[] unintVi = unitVList[i];//权因子
                    int nk = lList[i].Length;//观测值个数
                    int index = 0;
                    for (int j = 0; j < nk; j++)
                    {
                        for (int k = 0; k <= j; k++)
                        {
                            if (unintVi[j] < 1.0 || unintVi[k] < 1.0)
                            {
                                //
                                pList[i][index] = PListBackup[i][index] * Math.Sqrt(unintVi[j] * unintVi[k]);
                                if (pList[i][index] == 0 && k == j)
                                {
                                    pList[i][index] = Math.Pow(10, -20);
                                }
                            }
                            else
                            { pList[i][index] = PListBackup[i][index]; }

                            //PList[i][index] = PListBackup[i][index];
                            index++;
                        }
                    }
                    //double[] tmpPi = new double[pList[i].Length];
                    //for (int j = 0; j < pList[i].Length; j++)
                    //{
                    //    tmpPi[j] = pList[i][j];
                    //}
                    ////PList[i].CopyTo(tmpPi, 0);
                    //GetInverse(tmpPi, nk);

                    ////double[][] mPi = new double[nk][];

                    ////for (int s = 0; s < nk; s++)
                    ////{
                    ////    mPi[s] = new double[nk];
                    ////    for (int k = 0; k <= s; k++)
                    ////    {
                    ////        mPi[s][k] = PList[i][GetSunScript(s, k)];
                    ////    }
                    ////}
                    ////for (int s = 0; s < nk; s++)
                    ////{

                    ////    for (int k = s + 1; k < nk; k++)
                    ////    {
                    ////        mPi[s][k] = mPi[k][s];
                    ////    }
                    ////}
                    ////Geo.Algorithm.Matrix mmPi = new Geo.Algorithm.Matrix(mPi);
                    ////Geo.Algorithm.Matrix InvPi = mmPi.Inverse;

                    //double[] tmpQVi = new double[nk];
                    //for (int k = 0; k < nk; k++) { tmpQVi[k] = tmpPi[GetSunScript(k, k)]; }//求逆有错误！！！！
                    ////for (int k = 0; k < nk; k++) { tmpQVi[k] = InvPi[k, k]; }
                    //QVList[i] = tmpQVi;//残差的权矩阵要时刻更新，新的残差权矩阵=P_last-A*(Q_X)*AT
                }

                GetATPA();//组成法方程

                SetKnownCondition();//已知点处理

                //残差
                double max = GetdX();//此处对法矩阵求逆了。

                //更新坐标
                UpdateXYZ();

                //计算最小二乘残差
                GetLV();
                tmpMaxX.Add(max);
                if (Math.Abs(max) < eps && No > 1)
                {
                    break;
                }

                //---------------------------------------------------------------------
                //权因子计算
                //  GetQV();//更新残差权矩阵，（此前的法方程矩阵已经求逆，即是QX）
                GetUnitV();//标准化残差计算
                for (int i = 0; i < FileNumber; i++)
                {
                    //double[] unitVi = unitVList[i];
                    int n = unitVList[i].Length;
                    for (int j = 0; j < n; j++)
                    {
                        unitVList[i][j] = GetWeightFactor(FactorYype.IGG3, unitVList[i][j] / MEDIAN, k0, k1, m);
                    }
                }


            }//while循环结束


            //计算观测值降权的个数
            int dd = 0;//降权观测值个数
            for (int i = 0; i < FileNumber; i++)
            {
                double[] unitV = unitVList[i];
                double[] Pi = pList[i];
                for (int j = 0; j < unitV.Length; j++)
                {
                    if (unitV[j] < 1.0 || Pi[j] == Math.Pow(10, -30)) dd++;  //这里根据两步同步判断是否是作为粗差处理
                }
            }
            if (dd == 0)//所有观测值均未降权
            { }
            //计算二次型
            double VTPV = GetVTPV();
            int freedom = totalBaselineNumber * 3 - dd - (totalPointNumber - 1) * 3;//已知点个数为1
            Sigma = Math.Sqrt(VTPV / freedom);
        }

        /// <summary>
        /// 抗差估计
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="eps"></param>
        /// <param name="k0"></param>
        /// <param name="k1"></param>
        /// <param name="m"></param>
        public void parallelRobust(FactorYype fname, double eps, double k0, double k1, int m)
        {
            double[][] parallQVList = new double[FileNumber][];
            double[][] parallPListBackup = new double[FileNumber][];
            double[][] parallUnintVList = new double[FileNumber][];
            double[][] parallVList = new double[FileNumber][];
            double[][] parallPList = new double[FileNumber][];
            double[][] paralllList = new double[FileNumber][];

            //List<double[]> PListBackuptmp = new List<double[]>();
            for (int i = 0; i < FileNumber; i++)
            {
                parallPList[i] = pList[i];
                paralllList[i] = lList[i];
            }

            //----------------------------------------------------------
            //计算观测值的权矩阵
            for (int i = 0; i < FileNumber; i++)
            {
                int n = paralllList[i].Length;
                double[] QVi = new double[n];
                double[] Pi = new double[parallPList[i].Length];//此时还是协方差矩阵
                parallPList[i].CopyTo(Pi, 0);

                int ii = 0;
                for (int j = 0; j < n; j++)
                {
                    QVi[ii] = Pi[GetSunScript(j, j)];
                    ii++;
                }
                parallQVList[i] = QVi;
                //QVList.Add(QVi);
                //权矩阵
                GetInverse(Pi, n);
                parallPListBackup[i] = new double[Pi.Length];
                Pi.CopyTo(parallPListBackup[i], 0);//将观测值的权备份到数组PPList
               // parallPListBackup[i] = Pi;//将观测值的权备份到数组PPList
                //PListBackuptmp.Add(Pi);
                parallPList[i] = Pi;
               // pList[i] = Pi;
            }
            //----------------------------------------------------------
            //计算残差的权倒数
            //GetATPA();
            //if (!GetInverse(ATPA, 3 * TotalPointNumber))//法方程系数矩阵求逆
            //{
            //    throw new Exception("法方程系数降秩，求逆失败！");
            //}

            //GetQV();//计算残差的权倒数

            //-----------------------------------------------------------
            
            //List<double[]> PListBackup = new List<double[]>();
            //for (int i = 0; i < FileNumber; i++)
            //{
            //    double[] Pi = new double[pList[i].Length];
            //    pList[i].CopyTo(Pi, 0);
            //    PListBackup.Add(Pi);
            //}
            //-----------------------------------------------------------
            //抗差估计迭代
            for (int i = 0; i < FileNumber; i++)
            {
                int n =parallQVList[i].Length;
                double[] unitVi = new double[n];//权因子，初始赋予初值1
                for (int j = 0; j < n; j++) { unitVi[j] = 1.0; }
                parallUnintVList[i] = unitVi;
                //if (unitVList.Count < FileNumber)
                //{
                //    unitVList.Add(unitVi);
                //}
                //else
                //{
                //    unitVList[i] = unitVi;
                //}

            }

            int No = 0;//迭代次数
            bool IsStill = true;
            while (IsStill)
            {
                No++;
                //计算等价权
                for (int i = 0; i < FileNumber; i++)
                {
                    double[] unintVi = parallUnintVList[i];//权因子
                    int nk =paralllList[i].Length;//观测值个数
                    int index = 0;
                    for (int j = 0; j < nk; j++)
                    {
                        for (int k = 0; k <= j; k++)
                        {
                            if (unintVi[j] < 1.0 || unintVi[k] < 1.0)
                            {
                                // parallPList[i][index] = parallPListBackup[i][index] * Math.Sqrt(unintVi[j] * unintVi[k]);
                                parallPList[i][index] = parallPList[i][index] * Math.Sqrt(unintVi[j] * unintVi[k]);
                                if (parallPList[i][index] == 0 && k == j)
                                {
                                    parallPList[i][index] = Math.Pow(10, -20);
                                }
                            }
                            else
                            {
                               // parallPList[i][index] = parallPListBackup[i][index];
                                parallPList[i][index] = parallPList[i][index];
                            }

                            //PList[i][index] = PListBackup[i][index];
                            index++;
                        }
                    }
                    double[] tmpPi = new double[parallPList[i].Length];
                    //for (int j = 0; j < parallPList[i].Length; j++)
                    //{
                    //    tmpPi[j] = parallPList[i][j];
                    //}
                    parallPList[i].CopyTo(tmpPi, 0);
                    GetInverse(tmpPi, nk);

                    double[] tmpQVi = new double[nk];
                    for (int k = 0; k < nk; k++) { tmpQVi[k] = tmpPi[GetSunScript(k, k)]; }
                    //for (int k = 0; k < nk; k++) { tmpQVi[k] = InvPi[k, k]; }
                    parallQVList[i] = tmpQVi;//残差的权矩阵要时刻更新，新的残差权矩阵=P_last-A*(Q_X)*AT
                }

                #region 组成法方程 GetATPA();//
                int t = 3 * totalPointNumber;//
                int tt = t * (t + 1) / 2;

                ATPA = new double[tt];
                ATPL = new double[t];
                for (int i = 0; i < t; i++) ATPL[i] = 0.0;
                for (int i = 0; i < tt; i++) ATPA[i] = 0.0;
               
                #region  GetLV();//误差方差自由项计算
               
                for (int i = 0; i < FileNumber; i++)
                {
                    double[] Li = paralllList[i];//当前文件的基线向量观测值个数,是下面两个变量长度的三倍
                    int[] BegPoint = totalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                    int[] EndPoint = totalEndOfBaselineList[i];//当前文件的基线向量终点点号
                    int n = BegPoint.Length;//
                    double[] Vi = new double[n * 3];
                    for (int j = 0; j < n; j++)
                    {
                        int b1 = BegPoint[j];
                        int e2 = EndPoint[j];
                        Vi[j * 3 + 0] = (pointsXYZ[e2][0] - pointsXYZ[b1][0]) - Li[3 * j + 0];
                        Vi[j * 3 + 1] = (pointsXYZ[e2][1] - pointsXYZ[b1][1]) - Li[3 * j + 1];
                        Vi[j * 3 + 2] = (pointsXYZ[e2][2] - pointsXYZ[b1][2]) - Li[3 * j + 2];

                    }
                    parallVList[i] = Vi;              
                }
                #endregion

                for (int i = 0; i < FileNumber; i++)
                {
                    int[] BegPoint = totalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                    int[] EndPoint = totalEndOfBaselineList[i];//当前文件的基线向量终点点号
                    int n = BegPoint.Length;//
                    double[] Pi = parallPList[i];//
                    double[] li = parallVList[i];
                    //ATAPi
                    int nk = 3 * n;
                    for (int s1 = 0; s1 < nk; s1++)
                    {
                        int ttttt = s1 / 3;
                        int i1 = 3 * BegPoint[s1 / 3] + s1 % 3;//误差方差中系数为-1的未知数编号
                        int i2 = 3 * EndPoint[s1 / 3] + s1 % 3;//误差方差中系数为1的未知数编号

                        for (int s2 = 0; s2 < nk; s2++)
                        {
                            int j1 = 3 * BegPoint[s2 / 3] + s2 % 3;
                            int j2 = 3 * EndPoint[s2 / 3] + s2 % 3;

                            double p12 = Pi[GetSunScript(s1, s2)];

                            if (i1 >= j1) ATPA[GetSunScript(i1, j1)] += p12;
                            if (i1 >= j2) ATPA[GetSunScript(i1, j2)] -= p12;
                            if (i2 >= j1) ATPA[GetSunScript(i2, j1)] -= p12;
                            if (i2 >= j2) ATPA[GetSunScript(i2, j2)] += p12;

                            if (p12 == Math.Pow(10, -20)) p12 = 0.0;//粗差观测值，相当于剔除
                            double l = li[s2];
                            ATPL[i1] -= p12 * l;
                            ATPL[i2] += p12 * l;

                        }
                    }
                    //
                }
                #endregion

           
                #region SetKnownCondition();//已知点处理  
                ATPA[GetSunScript(3 * 0 + 0, 3 * 0 + 0)] += Math.Pow(10, 30);
                ATPA[GetSunScript(3 * 0 + 1, 3 * 0 + 1)] += Math.Pow(10, 30);
                ATPA[GetSunScript(3 * 0 + 2, 3 * 0 + 2)] += Math.Pow(10, 30);
                #endregion
               
                #region //残差 double max = GetdX();//此处对法矩阵求逆了。
                //对法矩阵求逆
                int ttt = 3 * totalPointNumber;
                if (!GetInverse(ATPA, ttt))
                {
                    throw new Exception("求逆失败！");
                }

                dXYZ = new double[ttt];
                for (int i = 0; i < ttt; i++) { dXYZ[i] = 0.0; }

                double max = 0.0;
                for (int i = 0; i < ttt; i++)
                {
                    double xi = 0.0;
                    for (int j = 0; j < ttt; j++)
                    {
                        xi -= ATPA[GetSunScript(i, j)] * ATPL[j];
                    }
                    dXYZ[i] += xi;

                    if (Math.Abs(xi) > Math.Abs(max)) max = xi;
                }
                #endregion

                
               
                #region //更新坐标 UpdateXYZ();
                int n1 = pointsXYZ.Count;
                for (int i = 0; i < n1; i++)
                {
                    pointsXYZ[i][0] += dXYZ[3 * i + 0];
                    pointsXYZ[i][1] += dXYZ[3 * i + 1];
                    pointsXYZ[i][2] += dXYZ[3 * i + 2];
                }
                #endregion

               
               
                #region   GetLV();//计算最小二乘残差
               // List<double[]> tmpVList = new List<double[]>(); ;
                for (int i = 0; i < FileNumber; i++)
                {
                    double[] Li =paralllList[i];//当前文件的基线向量观测值个数,是下面两个变量长度的三倍
                    int[] BegPoint = totalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                    int[] EndPoint = totalEndOfBaselineList[i];//当前文件的基线向量终点点号
                    int n = BegPoint.Length;//
                    double[] Vi = new double[n * 3];
                    for (int j = 0; j < n; j++)
                    {
                        int b1 = BegPoint[j];
                        int e2 = EndPoint[j];
                        Vi[j * 3 + 0] = (pointsXYZ[e2][0] - pointsXYZ[b1][0]) - Li[3 * j + 0];
                        Vi[j * 3 + 1] = (pointsXYZ[e2][1] - pointsXYZ[b1][1]) - Li[3 * j + 1];
                        Vi[j * 3 + 2] = (pointsXYZ[e2][2] - pointsXYZ[b1][2]) - Li[3 * j + 2];
                    }
                    parallVList[i] = Vi;                  
                }  
                #endregion

                tmpMaxX.Add(max);

                if (Math.Abs(max) < eps && No > 1)
                {
                    break;
                }

                //---------------------------------------------------------------------
                //权因子计算
                
                #region GetQV();//更新残差权矩阵，（此前的法方程矩阵已经求逆，即是QX）
                //计算观测值平差值权倒数
                for (int i = 0; i < FileNumber; i++)
                {

                    int[] BegPoint = totalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                    int[] EndPoint = totalEndOfBaselineList[i];//当前文件的基线向量终点点号
                    int mn = BegPoint.Length;//
                    double[] QVi = parallQVList[i];
                    if (QVi.Length != 3 * mn)
                    { throw new Exception("出错！"); }
                    for (int j = 0; j < mn; j++)
                    {
                        int b1 = BegPoint[j];//起点未知数编号
                        int e2 = EndPoint[j];//终点编号
                        double q0 = ATPA[GetSunScript(3 * b1 + 0, 3 * b1 + 0)] + ATPA[GetSunScript(3 * e2 + 0, 3 * e2 + 0)] - 2.0 * ATPA[GetSunScript(3 * b1 + 0, 3 * e2 + 0)];
                        QVi[3 * j + 0] = QVi[3 * j + 0] - q0;
                        double q1 = ATPA[GetSunScript(3 * b1 + 1, 3 * b1 + 1)] + ATPA[GetSunScript(3 * e2 + 1, 3 * e2 + 1)] - 2.0 * ATPA[GetSunScript(3 * b1 + 1, 3 * e2 + 1)];
                        QVi[3 * j + 1] = QVi[3 * j + 1] - q1;
                        double q2 = ATPA[GetSunScript(3 * b1 + 2, 3 * b1 + 2)] + ATPA[GetSunScript(3 * e2 + 2, 3 * e2 + 2)] - 2.0 * ATPA[GetSunScript(3 * b1 + 2, 3 * e2 + 2)];
                        QVi[3 * j + 2] = QVi[3 * j + 2] - q2;
                    }
                    parallQVList[i] = QVi;
                }
                #endregion
                

                #region GetUnitV();//标准化残差计算
                for (int i = 0; i < FileNumber; i++)
                {
                    double[] QVi = parallQVList[i];
                    double[] Vi = parallVList[i];
                    int n = QVi.Length;
                    double[] Wi = new double[n];
                    for (int j = 0; j < n; j++)
                    {
                        double mi = Math.Sqrt(QVi[j]); ;
                        if (Math.Abs(mi) < Math.Pow(10, -15))
                        {
                            Wi[j] = 1.0;
                        }
                        else if (Math.Abs(mi) >= Math.Pow(10, 10))
                        {
                            Wi[j] = 0.0;//已经是作为粗差处理了
                        }
                        else
                        {
                            Wi[j] = Vi[j] / mi;
                        }
                    }
                    parallUnintVList[i] = Wi;
                   
                }
                //

                int count = 0;
                for (int i = 0; i < FileNumber; i++)
                {
                    count += parallUnintVList[i].Length;

                }
                double[] allV = new double[count];
                int kk = 0;
                for (int i = 0; i < FileNumber; i++)
                {
                    double[] V = parallUnintVList[i];
                    for (int j = 0; j < V.Length; j++)
                    {
                        allV[kk + j] = V[j];
                    }
                    kk += V.Length;
                }
                MEDIAN = GetMedian(allV, count, true);

                //MEDIAN = GetMedian(unitVList, true) * 1.4826;

                #endregion

                for (int i = 0; i < FileNumber; i++)
                {
                    //double[] unitVi = unitVList[i];
                    int n = parallUnintVList[i].Length;
                    for (int j = 0; j < n; j++)
                    {
                        parallUnintVList[i][j] = GetWeightFactor(FactorYype.IGG3, parallUnintVList[i][j] / MEDIAN, k0, k1, m);
                    }
                }


            }//while循环结束


            //计算观测值降权的个数
            int dd = 0;//降权观测值个数
            for (int i = 0; i < FileNumber; i++)
            {
                double[] unitV = parallUnintVList[i];
                double[] Pi =parallPList[i];
                for (int j = 0; j < unitV.Length; j++)
                {
                    if (unitV[j] < 1.0 || Pi[j] == Math.Pow(10, -30)) dd++;  //这里根据两步同步判断是否是作为粗差处理
                }
            }
            if (dd == 0)//所有观测值均未降权
            { }
            //计算二次型
            // double VTPV = GetVTPV();

            #region 
            double VTPV = 0.0;
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Pi =parallPList[i];
                double[] Vi =parallVList[i];
                int mn = Vi.Length;
                double[] PVi = new double[mn];
                double pvv = 0.0;
                for (int j = 0; j < mn; j++)
                {
                    double pvj = 0.0;
                    for (int k = 0; k < mn; k++) pvj += Pi[GetSunScript(j, k)] * Vi[k];
                    PVi[j] = pvj;
                    pvv += pvj * Vi[j];
                }
                VTPV += pvv;
            }
            #endregion
            int freedom = totalBaselineNumber * 3 - dd - (totalPointNumber - 1) * 3;//已知点个数为1
            Sigma = Math.Sqrt(VTPV / freedom);
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
                if (a00  == 0.0)
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

       
        /// <summary>
        /// 对称正定矩阵的并行求逆
        /// </summary>
        /// <param name="a"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool GetParallelInverse(double[] a, int n)
        {
            int k, i, j;
            double[] a0 = new double[n];
            for (k = 0; k < n; k++)
            {
                double a00 = a[0];
                if (a00 + 1.0 == 1.0)
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
                //for (i = 1; i < n; i++)
                //{
                //    a[(n - 1) * n / 2 + i - 1] = a0[i];
                //}
                Parallel.For(1, n, (int ii) =>
                {
                    a[(n - 1) * n / 2 + ii - 1] = a0[ii];
                });
                a[n * (n + 1) / 2 - 1] = 1.0 / a00;
            }
            return true;
        }

        /// <summary>
        /// 对称矩阵下标计算函数
        /// 对称矩阵采用下三角元素存储，则根据下标i,j计算矩阵所在序列中的元素值
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public int GetSunScript(int i, int j)
        {
            return (i >= j) ? i * (i + 1) / 2 + j : j * (j + 1) / 2 + i;
        }

        #endregion

        #region Gnss网平差基本工具
        /// <summary>
        /// 法方程的求取。
        /// </summary>
        public void GetATPA()
        {
            int t = 3 * totalPointNumber;//
            int tt = t * (t + 1) / 2;

            if (ATPA == null)
            {

                ATPA = new double[tt];
                ATPL = new double[t];
                for (int i = 0; i < t; i++) ATPL[i] = 0.0;
                for (int i = 0; i < tt; i++) ATPA[i] = 0.0;
            }
            else
            {
                ATPA = new double[tt];
                ATPL = new double[t];
                for (int i = 0; i < t; i++) ATPL[i] = 0.0;
                for (int i = 0; i < tt; i++) ATPA[i] = 0.0;
            }

            GetLV();//误差方差自由项计算

            for (int i = 0; i < FileNumber; i++)
            {
                int[] BegPoint = totalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                int[] EndPoint = totalEndOfBaselineList[i];//当前文件的基线向量终点点号
                int m = BegPoint.Length;//
                double[] Pi = pList[i];//
                double[] li = VList[i];
                //ATAPi
                int nk = 3 * m;
                for (int s1 = 0; s1 < nk; s1++)
                {
                    int ttttt = s1 / 3;
                    int i1 = 3 * BegPoint[s1 / 3] + s1 % 3;//误差方差中系数为-1的未知数编号
                    int i2 = 3 * EndPoint[s1 / 3] + s1 % 3;//误差方差中系数为1的未知数编号

                    for (int s2 = 0; s2 < nk; s2++)
                    {
                        int j1 = 3 * BegPoint[s2 / 3] + s2 % 3;
                        int j2 = 3 * EndPoint[s2 / 3] + s2 % 3;

                        double p12 = Pi[GetSunScript(s1, s2)];




                        if (i1 >= j1) ATPA[GetSunScript(i1, j1)] += p12;
                        if (i1 >= j2) ATPA[GetSunScript(i1, j2)] -= p12;
                        if (i2 >= j1) ATPA[GetSunScript(i2, j1)] -= p12;
                        if (i2 >= j2) ATPA[GetSunScript(i2, j2)] += p12;


                        if (p12 == Math.Pow(10, -20)) p12 = 0.0;//粗差观测值，相当于剔除
                        double l = li[s2];
                        ATPL[i1] -= p12 * l;
                        ATPL[i2] += p12 * l;

                    }
                }
                //
            }

        }
        /// <summary>
        /// 误差方差自由项或最小二乘残差的计算 V=AX-L
        /// </summary>
        /// <returns></returns>
        public  void GetLV()
        {
            List<double[]> tmpVList = new List<double[]>(); ;
            for (int i = 0; i < FileNumber; i++)
            {
                double[] Li = lList[i];//当前文件的基线向量观测值个数,是下面两个变量长度的三倍
                int[] BegPoint = totalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                int[] EndPoint = totalEndOfBaselineList[i];//当前文件的基线向量终点点号
                int m = BegPoint.Length;//
                double[] Vi = new double[m*3];
                for (int j = 0; j < m; j++)
                {
                    int k1 = BegPoint[j];
                    int k2 = EndPoint[j];
                    Vi[j * 3 + 0] = (pointsXYZ[k2][0] - pointsXYZ[k1][0]) - Li[3 * j + 0];
                    Vi[j * 3 + 1] = (pointsXYZ[k2][1] - pointsXYZ[k1][1]) - Li[3 * j + 1];
                    Vi[j * 3 + 2] = (pointsXYZ[k2][2] - pointsXYZ[k1][2]) - Li[3 * j + 2];

                }
                tmpVList.Add(Vi);
            }

            this.VList = tmpVList;
        }

        /// <summary>
        /// 已知点处理
        /// 目前仅采用已知点固定的方法处理，且强约束第一个点作为已知点。
        /// </summary> 
        public  void SetKnownCondition()
        {
            double tt = 1.0e30;
            ATPA[GetSunScript(3 * 0 + 0, 3 * 0 + 0)] += Math.Pow(10, 30);
            ATPA[GetSunScript(3 * 0 + 1, 3 * 0 + 1)] += Math.Pow(10, 30);
            ATPA[GetSunScript(3 * 0 + 2, 3 * 0 + 2)] += Math.Pow(10, 30);
        }

        /// <summary>
        /// 未知数计算
        /// </summary>
        /// <returns></returns>
        public double GetdX()
        {

            //对法矩阵求逆
            int t = 3 * totalPointNumber;
            if (!GetInverse(ATPA, t))
            {
                throw new Exception("求逆失败！");
            }


            dXYZ = new double[t];
            for (int i = 0; i < t; i++) { dXYZ[i] = 0.0; }

            double max = 0.0;
            for (int i = 0; i < t; i++)
            {
                double xi = 0.0;
                for (int j = 0; j < t; j++)
                {
                    xi -= ATPA[GetSunScript(i, j)] * ATPL[j];
                }
                dXYZ[i] += xi;

                if (Math.Abs(xi) > Math.Abs(max)) max = xi;
            }
            return max;
        }


        /// <summary>
        /// 残差二次型计算，先计算PV，再计算VTPV
        /// PV还能继续利用。
        /// </summary>
        /// <returns></returns>
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
                    for (int k = 0; k < m; k++) pvj += Pi[GetSunScript(j, k)] * Vi[k];
                    PVi[j] = pvj;
                    pvv += pvj * Vi[j];
                }
                VTPV += pvv;
            }
            return VTPV;
        }
        /// <summary>
        /// 更新参数值
        /// </summary>
        public void UpdateXYZ()
        {
            int n = pointsXYZ.Count;
            for (int i = 0; i < n; i++)
            {
                pointsXYZ[i][0] += dXYZ[3 * i + 0];
                pointsXYZ[i][1] += dXYZ[3 * i + 1];
                pointsXYZ[i][2] += dXYZ[3 * i + 2];
            }
        }
        /// <summary>
        /// 计算残差V的权倒数
        /// </summary>
        public void GetQV()
        {
            //计算观测值平差值权倒数
            for (int i = 0; i < FileNumber; i++)
            {

                int[] BegPoint = totalBeginOfBaselineList[i];//当前文件的基线向量起点点号
                int[] EndPoint = totalEndOfBaselineList[i];//当前文件的基线向量终点点号
                int m = BegPoint.Length;//
                double[] QVi = QVList[i];
                if (QVi.Length != 3 * m)
                { throw new Exception("出错！"); }
                for (int j = 0; j < m; j++)
                {
                    int k1 = BegPoint[j];//起点未知数编号
                    int k2 = EndPoint[j];//终点编号
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
                    if (Math.Abs(mi) < Math.Pow(10, -15))
                    {
                        Wi[j] = 1.0;
                    }
                    else if (Math.Abs(mi) >= Math.Pow(10, 10)) 
                    {
                        Wi[j] = 0.0;//已经是作为粗差处理了
                    }
                    else
                    {
                        Wi[j] = Vi[j] / mi;
                    }
                }
                if (unitVList.Count < FileNumber)
                {
                    unitVList.Add(Wi);
                }
                else
                {
                    unitVList[i] = Wi;
                }
            }
            //
            MEDIAN = GetMedian(unitVList, true) * 1.4826;
        }

        /// <summary>
        /// 中位数计算
        /// </summary>
        /// <param name="pp"></param>
        /// <param name="n"></param>
        /// <param name="IsAbs"></param>
        /// <returns></returns>
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

        public double GetMedian(List<double[]> unitVList, bool IsAbs)
        {
            int count = 0;
            for (int i = 0; i < unitVList.Count; i++)
            {
                count += unitVList[i].Length;

            }
            double[] allV = new double[count];
            int k = 0;
            for (int i = 0; i < unitVList.Count; i++)
            {
                double[] V = unitVList[i];
                for (int j = 0; j < V.Length; j++)
                {
                    allV[k + j] = V[j];
                }
                k += V.Length;
            }
            double mean = GetMedian(allV, count, IsAbs);
            return mean;
        }

        /// <summary>
        /// 权因子函数
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="v"></param>
        /// <param name="k0"></param>
        /// <param name="k1"></param>
        /// <returns></returns>
        private double GetWeightFactor(FactorYype fname, double v, double k0, double k1,int m)
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

    //public enum Factor
    //{
    //    IGG1,
    //    IGG3,
    //    IGG33,//去平方
    //    Huber,
    //}
}


