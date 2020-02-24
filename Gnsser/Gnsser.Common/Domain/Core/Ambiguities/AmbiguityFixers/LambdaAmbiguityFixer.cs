//2014.09.02 cuiyang,增加 LAMBDA 算法
//2015.01.15, czs,  算法重构， LAMBDA 命名为 LambdaAmbiguitySolver
//2018.10.19, czs, edit in hmx， 模糊度增加RMS信息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Gnsser;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;


namespace Gnsser
{
    /// <summary>
    /// Integer estimation with the LAMBDA method Least-square Ambiguity Decorrelation Adjustment
    /// (最小二乘模糊度降相关平差法)，简称LAMBDA法。
    /// 被认为是目前最好的一种整周模糊度解算方法。
    /// LAMBDA算法包括两个过程：模糊度降相关处理（整数 变换）和整数模糊度搜索。
    /// 其基本思想是首先基于拉格朗日降相关原理，通过整体变化方法降低
    /// 模糊度之间的相关性，然后在整数变换后的空间内，采用搜索技术获得其最优模糊度向量。
    /// </summary>
    public class LambdaAmbiguityFixer :  IAmbiguityFixer
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public LambdaAmbiguityFixer()
        { }
            
        /// <summary>
        /// 一些初始化工作。
        /// </summary>
        private void Init()
        {
            //将浮点解分解为小数部分和整数部分。
            this.Fractions = new Vector(Dimension); //小数部分
            this.RawIntegers = new Vector(Dimension); //原始浮点模糊度的整数部分 contains the (integer) increments of the original float ambiguities
            DefaultRms = 1e-20;
            for (int i = 0; i < Dimension; i++)
            {
                this.RawIntegers[i] = Math.Truncate(FloatResolution[i]);
                this.Fractions[i] = FloatResolution[i] - RawIntegers[i];
            }
        }


        #region 属性
        /// <summary>
        /// dimension of matrix Q and vector a 
        /// 向量和权逆阵的维数。
        /// </summary>
        public int Dimension { get; set; }

        /// <summary>
        /// variance covariance matrix(symmetric)
        /// 协方差阵。对称的。
        /// </summary>
        public IMatrix Cova { get; set; }

        /// <summary>
        /// the vetor with real valued estimated(float solution)
        /// 浮点解算向量。
        /// </summary>
        public Vector FloatResolution { get; set; }
        /// <summary>
        /// 原始数据取整
        /// </summary>
        public Vector RawIntegers { get; set; }
        /// <summary>
        /// 原始解的小数部分
        /// </summary>
        public Vector Fractions { get; set; }
        #endregion


        /// <summary>
        /// 默认的RMS
        /// </summary>
       public  double DefaultRms { get; set; }
        /// <summary>
        /// 计算，获取固定后的模糊度结果。
        /// </summary>
        /// <returns></returns>
        public WeightedVector GetFixedAmbiguities(WeightedVector floatResolution)
        {
            this.Dimension = floatResolution.Count;
            this.Cova = floatResolution.InverseWeight;
            this.FloatResolution = floatResolution;
            Init(); 


            int MaxCan = 2;

            ArrayMatrix ZTransformation = ArrayMatrix.Diagonal(Dimension, Dimension, 1); // initialize Z to a unit matrix 

            //make the L_1^{-TProduct} D_1^{-1} L_1^{-1} decomposition of the covariance matrix Q 
            //分解协方差阵为 D L
            double[] D = new double[Dimension];
            ArrayMatrix L = new ArrayMatrix(Dimension, Dimension);

            FactorizationQToLTDL(Cova, ref D, ref L);

            //计算 Z 转换矩阵，采用 D 和 L
            Re_Order(ZTransformation, D, L);

            ArrayMatrix Zt = ZTransformation.Inverse; //inversion yields transpose of Z 

            //For the search we need L and D of Q^{-1} 
            L = GetInverseOfLowerTriangular(L, Dimension);

            for (int i = 0; i < Dimension; i++) D[i] = 1.0 / D[i];

            //find a suitable Chi^2 such that we have dayServices candidates an minimum:
            //use an eps to make sure the dayServices candidates are inside the ellipsoid some small number
            double eps = Math.Pow(10, -6);
            double[] v3 = Chistart(D, L);

            double Chi_1 = v3[1] + eps;
            //find the dayServices candidates with minimun norm
            ArrayMatrix candidates = new ArrayMatrix(Dimension, MaxCan); // 2-dimensional array to store the candidates
            ArrayMatrix disal1 = new ArrayMatrix(1, MaxCan); //according squared norms 
            int ipos = 0;//最佳候选整数的位置
            int ncan = 0;
            Search(Chi_1, MaxCan, Dimension, D, L, Fractions, ref disal1, ref candidates, ref ipos, ref ncan);

            //back-transformation (and adding increments)
            //还原为Y 的整数估计^Y
            Vector Results = new Vector(Dimension);
            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < Dimension; j++)
                {
                    Results[i] += ZTransformation[i, j] * candidates[j, ipos];
                }
                Results[i] = Results[i] + RawIntegers[i];
            }

            //'sort' the squared norms in increasing order (if ipos equals 1, the best candidate is in the second place: so reverse disal1)
            if (ipos == 1)
            {
                double h = disal1[0, 0];
                disal1[0, 0] = disal1[0, 1];
                disal1[0, 1] = h;
            }

            return new WeightedVector( Results, DefaultRms);
        }
        /// <summary>
        /// 找到最大可能的整数向量，使其与浮点数向量解的距离最小，并满足条件 Q = transpose(L) D L。
        /// finds 'MaxCan' integer vectors whose distances to the real vector 'a' are minimal in the metric of Q = transpose(L) D L.
        /// Only integer vectors with a distance less than sqrt(Chic) are regared.
        /// The search for gridpoints inside the ambiguity search ellipsoid is a sequential conditional adjustment upon the ambiguities.
        /// The search starts by conditioning the last ambiguity a_n to an integer, then the one-but-last a_{n-1} etc., until
        ///   - the squared norm grows too large (out of the ellipsoid)
        ///   - an integer for a_1 is found: a full integer vector is encountered (a gridpoint inside the ellipsoid)
        ///   then is goes back to some previous (towards a_n) ambiguity 
        ///   and considers another integer for it.
        /// </summary>
        /// <param name="Chic">Chi squared</param>
        /// <param name="MaxCan">Number of minimum integer vectors required</param>
        /// <param name="dimension">dimension of matrix</param>
        /// <param name="diagonal">diagonal matrix</param>
        /// <param name="lowerTriangular">kiwer truabgykar natrux, only stroe lower triangular</param>
        /// <param name="floatSolution">vector of real valued estimates(float solution)</param>
        /// <param name="candidates">2-dimensional array to store the candidates</param>
        /// <param name="disal1">according squared norms</param>
        /// <param name="ipos"> 最佳候选整数的位置。 column number in 'cands' where the candidate belonging to the minimum distance is stored</param>
        /// <param name="ncan">找到的可能的整数数量。number of integer vectors found</param>
        private void Search(
            double Chic, 
            int MaxCan,
            int dimension,
            double[] diagonal,
            ArrayMatrix lowerTriangular,
            Vector floatSolution, 
            ref  ArrayMatrix disal1,
            ref ArrayMatrix candidates, 
            ref int ipos,
            ref int ncan)
        {
            candidates = new ArrayMatrix(dimension, MaxCan); // 2-dimensional array to store the 
            disal1 = new ArrayMatrix(1, MaxCan); //according squared norms 
            ipos = 0;
            ncan = 0;

            double tmax = 0;
            int imax = 0;

            if (MaxCan < 1)
            {
                throw new Exception("Error: number of requested candidates < 1");
            }
            else
            {
                if (dimension < 2)
                {
                    throw new Exception("Error: dimension of system < 2");
                }
            }
            bool ende = false;
            double[] right = new double[dimension + 1];
            double[] left = new double[dimension + 1];

            right[dimension] = Chic;
            double[] dq = new double[dimension];

            for (int i = 0; i < dimension - 1; i++) dq[i] = diagonal[i + 1] / diagonal[i];

            dq[dimension - 1] = 1 / diagonal[dimension - 1];



            int icurrent = dimension;
            int iold = icurrent;

            double[] lef = new double[dimension];

            double[] dist1 = new double[dimension];

            double[] endd = new double[dimension];

            while (ende == false)
            {


                icurrent = icurrent - 1;   //go a leval deeper . we were here before only one dist is one bigger
                if (iold <= icurrent)
                {
                    lef[icurrent] = lef[icurrent] + lowerTriangular[icurrent + 1, icurrent];
                }
                else
                {
                    lef[icurrent] = 0;
                    for (int j = icurrent + 1; j < dimension; j++)
                    {

                        lef[icurrent] = lef[icurrent] + lowerTriangular[j, icurrent] * dist1[j];
                    }
                }
                iold = icurrent; //keep track of level

                right[icurrent] = (right[icurrent + 1] - left[icurrent + 1]) * dq[icurrent];

                double reach = Math.Sqrt(right[icurrent]);

                //delta=a[time]-reach-lef[time] is the left border
                //ceil(delta) is the left most integer in the interval 
                //idst[time] is the distance of this left most integer to the a_hat
                double delta = floatSolution[icurrent] - reach - lef[icurrent];
                dist1[icurrent] = Math.Ceiling(delta) - floatSolution[icurrent];



                if (dist1[icurrent] > reach - lef[icurrent]) //there is nothing at this level , so, ... back track
                {
                    Backtrack(ref ende, left, ref icurrent, lef, dist1, endd);

                }
                else
                {
                    endd[icurrent] = reach - lef[icurrent] - 1; //set the right 'border'
                    left[icurrent] = Math.Pow(dist1[icurrent] + lef[icurrent], 2);
                }
                if (icurrent == 0)
                {
                    Collects(MaxCan, diagonal, Chic, candidates, disal1, ref tmax, ref imax, right, left, ref ncan, lef, dist1, endd);
                    Backtrack(ref ende, left, ref icurrent, lef, dist1, endd);
                }

            }


            //the candidate vectors are stored and the index for the best candidate is set(ipos)
            double dminn = 1.0e20;

            for (int i = 0; i < Math.Min(ncan, MaxCan); i++)
            {
                if (disal1[0, i] < dminn)
                {
                    dminn = disal1[0, i];
                    ipos = i;
                }
                for (int j = 0; j < dimension; j++) candidates[j, i] = candidates[j, i] + floatSolution[j];
            }
        }


        /// <summary>
        /// collexts integer vectors and corresponding squared distances
        /// </summary>
        /// <param name="MaxCan">number of minimum integer vectors required</param>
        /// <param name="D">diagonal matrix</param>
        /// <param name="Chic">Chi squared</param>
        /// <param name="cands">2-dimensional array to store the candidates</param>
        /// <param name="disal1">according squared norms \hat{a}-\check{a}</param>
        /// <param name="tmax">the largest distance of the Min(ncan, MaxCan) vectors with minimum distance found until now</param>
        /// <param name="imax">position in disall/cands of the vector with the largest distance of the Min (ncan, MaxCan) vectors with minimum distance found until now</param>
        /// <param name="right">vector</param>
        /// <param name="left">vector</param>
        /// <param name="ncan">number of integer vectors found</param>
        /// <param name="lef">vector</param>
        /// <param name="dist">difference between the integer tried and \hat{a}_n</param>
        /// <param name="endd">vector</param>
        private void Collects(int MaxCan, double[] D, double Chic, ArrayMatrix cands, ArrayMatrix disal1,
            ref double tmax, ref int imax, double[] right, double[] left, ref int ncan, double[] lef, double[] dist, double[] endd)
        {
            double t = Chic - (right[0] - left[0]) * D[0];
            endd[0] = endd[0] + 1;
            //The following loop should be run through at least once
            while (dist[0] <= endd[0])
            {
                ncan = ncan + 1;
                if (ncan <= MaxCan)
                {
                    //
                    Stores(ncan, ncan, t, cands, disal1, ref tmax, ref imax, dist);
                }
                else
                {
                    if (t < tmax)
                    {
                        Stores(MaxCan, imax, t, cands, disal1, ref tmax, ref imax, dist);
                    }
                }
                t = t + (2 * (dist[0] + lef[0]) + 1) * D[0];
                dist[0] = dist[0] + 1;
            }
        }

        /// <summary>
        /// 存储候选者和相关距离。
        /// Stores candidates and correspoding distances
        /// </summary>
        /// <param name="ican">Min (number of vectors found until now, MaxCan)</param>
        /// <param name="ipos">position in disall/cands to put the new found vector </param>
        /// <param name="t">distance of the new found vector</param>
        /// <param name="cands">2d-array to store the integer vectors</param>
        /// <param name="disal1">distance of the MaxCan integer vectors</param>
        /// <param name="tmax">the largest distance of the ican vectors with minmum distance found until now</param>
        /// <param name="imax">position in disal1/cands of the vector with the largest distance of the ican vectors with minimum distance found until now</param>
        /// <param name="dist">difference between the integer tried and \hat{a}_n</param>
        private void Stores(int ican, int ipos, double t, ArrayMatrix cands, ArrayMatrix disal1, ref double tmax, ref int imax, double[] dist)
        {
            for (int i = 0; i < Dimension; i++) cands[i, ipos - 1] = dist[i];
            disal1[0, ipos - 1] = t;
            tmax = t;
            imax = ipos - 1;
            for (int i = 0; i < ican; i++)
            {
                if (disal1[0, i] > tmax)
                {
                    imax = i;
                    tmax = disal1[0, i];
                }
            }
        }

        /// <summary>
        /// backtrack in the search tree: used in SEARCH
        /// </summary>
        /// <param name="ende">if 'true', then search is done</param>
        /// <param name="left">vector</param>
        /// <param name="time">level in the tree</param>
        /// <param name="lef">vector</param>
        /// <param name="dist">difference between the integer tried and hat {a}_i</param>
        /// <param name="endd">vector</param>
        private void Backtrack(ref bool ende, double[] left, ref int i, double[] lef, double[] dist, double[] endd)
        {
            bool cand_n = false; //candidate at level n
            bool c_stop = false; // criterion to stop the loop
            while (c_stop == false && i < Dimension - 1)
            {
                i = i + 1;
                if (dist[i] <= endd[i])
                {
                    dist[i] = dist[i] + 1;
                    left[i] = Math.Pow(dist[i] + lef[i], 2);
                    c_stop = true;
                    if (i == Dimension - 1)
                    { cand_n = true; }
                }
            }
            if (i == Dimension - 1 && cand_n == false)
            { ende = true; }
        }

        /// <summary>
        /// Compute squared distance of partially rounded float vectors to the float vector in the metric of the covariance matrix
        /// </summary>
        /// <param name="D"></param>
        /// <param name="L"></param>
        /// <returns></returns>
        private double[] Chistart(double[] D, ArrayMatrix L)
        {
            double[] tm = new double[2];

            double bigNum = Math.Pow(10, 10); //some big number
            double[] dist = new double[Dimension];
            double[] e = new double[Dimension];
            double[] t = new double[Dimension];

            for (int i = 0; i < Dimension; i++) dist[i] = Math.Round(FloatResolution[i]) - FloatResolution[i];

            for (int i = 0; i < Dimension; i++)
            {
                for (int j = i; j < Dimension; j++)
                {
                    e[i] += L[j, i] * dist[j];
                }
            }

            double t_0 = 0;
            for (int i = 0; i < Dimension; i++) t_0 = t_0 + D[i] * e[i] * e[i];

            tm[0] = t_0;
            tm[1] = bigNum;


            //for 1 to n take the second nearest integer to a_i
            for (int i = 0; i < Dimension; i++)
            {
                if (dist[i] < 0)
                {
                    t[i] = t_0;
                    for (int j = 0; j <= i; j++)
                        t[i] = t[i] + D[j] * L[i, j] * (2 * e[j] + L[i, j]);
                }
                else
                {
                    t[i] = t_0;
                    for (int j = 0; j <= i; j++)
                        t[i] = t[i] - D[j] * L[i, j] * (2 * e[j] - L[i, j]);
                }

                //find the second smallest squared norm
                if (t[i] < tm[0])
                {
                    tm[1] = tm[0];
                    tm[0] = t[i];
                }
                else if (t[i] < tm[1])
                {
                    tm[1] = t[i];
                }
            }
            return tm;
        }

        /// <summary>
        /// 计算 Z 变换矩阵，最后的 Z 变换矩阵是构建自一些列相邻模糊度的整数。
        /// Computation of the Z-transformation matrix
        /// The final Z-transformation is constructed from a sequence of interchanges of dayServices neighbouring ambiguities(this function) and
        /// integer Gauss transformations (function ztransi) that pair-wise decorrelate the ambiguities
        /// </summary>
        /// <param name="Zti"></param>
        /// <param name="D"></param>
        /// <param name="L"></param>
        private void Re_Order(ArrayMatrix Zti, double[] D, ArrayMatrix L)
        {
            int Dimension = D.Length;
            int i1 = Dimension - 1;
            bool sw = true; //the logical sw traces whether an interchange has occured or not 

            while (sw == true)
            {
                int i = Dimension - 1;
                sw = false;
                while (sw == false && i > 0)
                {
                    i = i - 1;
                    if (i <= i1)
                    {
                        Ztransi(Zti, L, i, i);
                    }

                    double delta = D[i] + L[i + 1, i] * L[i + 1, i] * D[i + 1];

                    double[] lambda = new double[3];

                    if (delta < D[i + 1])
                    {
                        lambda[2] = D[i + 1] * L[i + 1, i] / delta;
                        double eta = D[i] / delta;
                        D[i] = eta * D[i + 1];
                        D[i + 1] = delta;
                        for (int j = 0; j <= i - 1; j++)
                        {
                            lambda[0] = L[i, j];
                            lambda[1] = L[i + 1, j];
                            L[i, j] = lambda[1] - L[i + 1, i] * lambda[0];
                            L[i + 1, j] = lambda[2] * lambda[1] + eta * lambda[0];

                        }

                        L[i + 1, i] = lambda[2];

                        double temp = 0;

                        for (int j = i + 2; j < Dimension; j++) // to be coded effectively: interchange of columns L[time+2:n,time] and L[time+2:n,time+1]
                        {
                            temp = L[j, i];
                            L[j, i] = L[j, i + 1];
                            L[j, i + 1] = temp;
                        }

                        for (int j = 0; j < Dimension; j++) // to be coded effectively: swap Zti[1:n,time] with Zti[1:n,time+1]
                        {
                            temp = Zti[j, i];
                            Zti[j, i] = Zti[j, i + 1];
                            Zti[j, i + 1] = temp;
                        }

                        temp = FloatResolution[i];
                        FloatResolution[i] = FloatResolution[i + 1];
                        FloatResolution[i + 1] = temp;

                        i1 = i;
                        sw = true;
                    }
                }
            }
        }

        /// <summary>
        /// Update integral Z-transform for L
        /// only column 'prevObj' until 'last'.
        /// the output is the inverse of Z transpose
        /// </summary>
        /// <param name="Zti">the inverse Z transposed transformation matrix</param>
        /// <param name="L">lower triangular matrix L</param>
        /// <param name="prevObj">prevObj column to be updated</param>
        /// <param name="last">last column to be updated</param>
        private void Ztransi(ArrayMatrix Zti, ArrayMatrix L, int first, int last)
        {
            for (int i = last; i >= first; i--)
            {
                for (int j = i + 1; j < Dimension; j++)
                {
                    double mu = Math.Round(L[j, i]);
                    if (mu != 0)
                    {
                        for (int s = j; s < Dimension; s++)
                        {
                            L[s, i] = L[s, i] - mu * L[s, j];
                        }
                        for (int s = 0; s < Dimension; s++) Zti[s, j] = Zti[s, j] + mu * Zti[s, i];
                        FloatResolution[i] = FloatResolution[i] - mu * FloatResolution[j];
                    }
                }
            }
        }

        #region 工具算法
        /// <summary>
        /// 计算下三角矩阵的逆。
        /// Compute the inverse of a lower triangular matrix
        /// </summary>
        /// <param name="lowerTriangular">下三角矩阵</param>
        /// <param name="dimension">矩阵的维数</param>
        private static ArrayMatrix GetInverseOfLowerTriangular(ArrayMatrix lowerTriangular, int dimension)
        {
            ArrayMatrix inverse = new ArrayMatrix(dimension, dimension);

            double[] vec = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j <= i - 1; j++) vec[j] = lowerTriangular[i, j];

                for (int j = 0; j <= i - 1; j++)
                {
                    for (int k = j; k <= i - 1; k++)
                    {
                        inverse[i, j] += -inverse[k, j] * vec[k] / lowerTriangular[i, i];
                    }
                }
                inverse[i, i] = 1 / lowerTriangular[i, i];
            }
            return inverse;
        }

        /// <summary>
        /// 因式分解Q为LTDL
        /// factorization of Q into L^TProduct D L
        /// </summary>
        /// <param name="D"></param>
        /// <param name="L"></param>
        private static void FactorizationQToLTDL(IMatrix Cova, ref double[] D, ref ArrayMatrix L)
        {
            int Dimension = Cova.RowCount;
            for (int i = Dimension - 1; i >= 0; i--)
            {
                D[i] = Cova[i, i];
                for (int j = 0; j <= i; j++) L[i, j] = Cova[i, j] / Math.Sqrt(Cova[i, i]);

                for (int j = 0; j <= i - 1; j++)
                {
                    for (int k = 0; k <= j; k++)
                    {
                        Cova[j, k] = Cova[j, k] - L[i, k] * L[i, j];
                    }
                }
                for (int j = 0; j <= i; j++) L[i, j] = L[i, j] / L[i, i];
            }
        }
        #endregion

    }
}