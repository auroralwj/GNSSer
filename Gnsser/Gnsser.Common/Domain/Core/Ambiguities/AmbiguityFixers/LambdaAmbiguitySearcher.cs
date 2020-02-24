//2014.09.02 cuiyang,增加 LAMBDA 算法
//2015.06.16, cy, edit, RTKLIB提供的LAMBDA固定 2015.06.16
//2016.10.11, czs, edit, 重构整理重命名

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;

namespace Gnsser
{
    /// <summary>
    /// Integer estimation with the LAMBDA method
    /// Least-square Ambiguity Decorrelation Adjustment(最小二乘模糊度降相关平差法)，简称LAMBDA法，被认为是目前最好的一种整周模糊度解算方法
    /// LAMBDA算法包括两个过程：模糊度降相关处理（整数 变换）和整数模糊度搜索。其基本思想是首先基于拉格朗日降相关原理，通过整体变化方法降低
    /// 模糊度之间的相关性，然后在整数变换后的空间内，采用搜索技术获得其最优模糊度向量。
    /// </summary>
    public class LambdaAmbiguitySearcher
    {

        /// <summary>
        /// 最大的ratio比值。
        /// </summary>
        public static double MaxRatio { get; set; }

        /// <summary>
        /// 计算模糊度，部分模糊度固定策略。
        /// </summary>
        /// <param name="floatSolution">按照Q从小到大的顺序排列后的带权向量。</param>
        /// <param name="MaxRatio">最大比例</param>
        /// <returns></returns>
        public static WeightedVector GetAmbiguity(WeightedVector floatSolution, double MaxRatio =3, double defaultRms = 1e-20)
        {
            //integer least square
           
            #region RTKLIB lambda
            var currentFloat = floatSolution; 
            int i = 0;
            var lambda = new LambdaAmbiguitySearcher(currentFloat);


            double[] N1 = new double[currentFloat.Count * 2]; for (i = 0; i < currentFloat.Count; i++) { N1[i] = Math.Round(currentFloat[i]); }
            double[] s = new double[2];
            int info = 0;

            info = lambda.getLambda(ref N1, ref s);
            //if (info == -1 || s[0]<=0.0) return 0; //固定失败
            double ratio = s[1] / s[0];

            while (info == -1 || ratio <= MaxRatio) //检验没有通过
            {
                //去掉一个最大的方差值，继续固定。
                var nextParamCount = currentFloat.Count - 1;
                if (nextParamCount < 1) 
                { return new WeightedVector(); }
                currentFloat = currentFloat.GetWeightedVector(0, nextParamCount);
                //固定
                N1 = new double[currentFloat.Count * 2]; for (i = 0; i < currentFloat.Count; i++) { N1[i] = Math.Round(currentFloat[i]); }
                s = new double[2];
                lambda = new LambdaAmbiguitySearcher(currentFloat);
                info = lambda.getLambda(ref N1, ref s);
                if (s[0] == 0 && s[1] != 0) {//
                    break;
                }
                ratio = s[1] / s[0];
            }
            if (info == -1) { return new WeightedVector(); }
            #endregion 
            var integers = ArrayUtil.GetSubArray<double>(N1, 0,  currentFloat.Count );
            Vector Vector = new Geo.Algorithm.Vector(integers, currentFloat.ParamNames);
            return new WeightedVector( Vector, defaultRms);
        }

        #region RTKLIB提供的LAMBDA固定 2015.06.16

        /// <summary>
        /// RTKLIB提供的LAMBDA固定
        /// </summary>
        /// <param name="paramCount">浮点参数的个数.number of float parameters</param>
        /// <param name="fixingCount">固定解的个数，通常为两个，一个最小值，一个次小值. number of fixed solutions</param>
        /// <param name="Q">浮点解的协方差矩阵。covariance matrix of float parameters (n * n)</param>
        /// <param name="a">浮点解向量。float parameters (n * 1)</param>
        /// <param name="F">固定解向量。fixed solutions (n * m)</param>
        /// <param name="s">固定解的残差平方和，便于ratio检验。sum of squared residulas of fixed solutions (1 * m)</param>
        public LambdaAmbiguitySearcher(double[][] Qa, double[] a, int fixingCount = 2)
        {
            //
            this.n = a.Length;
            this.m = fixingCount;
            this.Qa = Qa;
            this.a = a;
            MaxRatio = 3;

        }

        public LambdaAmbiguitySearcher(WeightedVector floatSolution, int fixingCount = 2)
        {
            this.n = floatSolution.Count;
            this.Qa = floatSolution.InverseWeight.Array ;
            this.a = floatSolution.OneDimArray;
            this.m = fixingCount;
            MaxRatio = 3;
        }
        //public LambdaAmbiguitySearcher(int n, double[][] Q, double[] a)
        //{
        //    this.n = n;
        //    this.Q = new Matrix(Q);
        //    this.a = a;
        //    MaxRatio = 3;
        //}

        /// <summary>
        /// 返回计算结果信息，若info为-1，表示固定失败。
        /// </summary>
        /// <param name="F"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public int getLambda(ref double[] F, ref double[] s)
        {
            int info = 0;  int i, j;

            if (n <= 0 || m <= 0) { info = -1; return info; }
            double[] L, D, Z, z, E;
            L = new double[n * n];
            D = new double[n * 1];
            Z = new double[n * n]; for (i = 0; i < n; i++) Z[i + i * n] = 1;
            z = new double[n * 1];
            E = new double[n * m];

            double[] Q = new double[n * n];
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    Q[i * n + j] = Qa[i][j];
                }
            }

            //if (!(info = LD(n, Q, ref L, ref D))) { }
            if ((info = LD(n, Q, ref L, ref D)) != -1) //说明分解成功
            {
                //lambad reduction 
                reduction(n, ref L, ref D, ref Z);

                //z=Z'*a;

                char[] tr = new char[2]; tr[0] = 'T'; tr[1] = 'N';
                matmul(tr, n, 1, n, 1.0, Z, a, 0.0, ref z);


                //mlambda search
                if ((info = msearch(n, m, ref L, ref D, ref z, ref E, ref s)) != -1)
                {
                    info = solve(tr, Z, E, n, m, ref F); //  F=Z'\E
                }

            }

            return info;
        }

        /// <summary>
        /// LD factorization (Q=L'*diag(D)*L)
        /// </summary>
        /// <param name="n"></param>
        /// <param name="Q"></param>
        /// <param name="L"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        private int LD(int n, double[] Q,ref double[] L,ref double[] D)
        {
            int i, j, k, info = 0;
            double a;
            double[] A = new double[n * n];


            for (i = 0; i < n * n; i++) A[i] = Q[i];

            for (i = n - 1; i >= 0; i--)
            {
                if ((D[i] = A[i + i * n]) <= 0.0) { info = -1; break; }
                a = Math.Sqrt(D[i]);
                for (j = 0; j <= i; j++) L[i + j * n] = A[i + j * n] / a;
                for (j = 0; j <= i - 1; j++) for (k = 0; k <= j; k++) A[j + k * n] -= L[i + k * n] * L[i + j * n];
                for (j = 0; j <= i; j++) L[i + j * n] /= L[i + i * n];
            }

            return info;
        }

        /// <summary>
        /// lambda reduction (z=Z'*a, Qz= Z'*Q*Z=L'*diag(D)*L)
        /// </summary>
        /// <param name="n"></param>
        /// <param name="L"></param>
        /// <param name="D"></param>
        /// <param name="Z"></param>
        private void reduction(int n,ref double[] L,ref double[] D,ref double[] Z)
        {
            int i, j, k;
            double del;
            j = n - 2; k = n - 2;

            while (j >= 0)
            {
                if (j <= k) for (i = j + 1; i < n; i++) { gauss(n, ref L, ref Z, i, j); }
                del = D[j] + L[j + 1 + j * n] * L[j + 1 + j * n] * D[j + 1];
                if ((del + 1E-6) < D[j + 1])
                {
                    //compared considering numerical 
                    perm(n, ref L, ref D, j, del, ref Z);
                    k = j; j = n - 2;
                }
                else
                    j--;
            }
        }

        //integer gauss transformation
        private void gauss(int n, ref double[] L,ref double[] Z, int i, int j)
        {
            int k, mu;
            if ((mu = (int)Math.Floor((L[i + j * n] + 0.5))) != 0)
            {
                for (k = i; k < n; k++) L[k + n * j] -= (double)mu * L[k + i * n];
                for (k = 0; k < n; k++) Z[k + n * j] -= (double)mu * Z[k + i * n];
            }
        }
        //permutations
        private void perm(int n, ref double[] L, ref double[] D, int j, double del, ref double[] Z)
        {
            int k;
            double eta, lam, a0, a1;

            eta = D[j] / del;
            lam = D[j + 1] * L[j + 1 + j * n] / del;
            D[j] = eta * D[j + 1]; D[j + 1] = del;
            for (k = 0; k <= j - 1; k++)
            {
                a0 = L[j + k * n]; a1 = L[j + 1 + k * n];
                L[j + k * n] = -L[j + 1 + j * n] * a0 + a1;
                L[j + 1 + k * n] = eta * a0 + lam * a1;
            }
            L[j + 1 + j * n] = lam;
            for (k = j + 2; k < n; k++)
            {
                double tmp;
                tmp = L[k + j * n];
                L[k + j * n] = L[k + (j + 1) * n];
                L[k + (j + 1) * n] = tmp;
            }
            for (k = 0; k < n; k++)
            {
                double tmp;
                tmp = Z[k + j * n];
                Z[k + j * n] = Z[k + (j + 1) * n];
                Z[k + (j + 1) * n] = tmp;
            }
        }


        /// <summary>
        /// multiply matrix
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <param name="m"></param>
        /// <param name="alpha"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        private double[] matmul(char[] tr, int n, int k, int m, double alpha, double[] A, double[] B, double beta, ref double[] C)
        {
            double d = 0.0;
            int f = tr[0] == 'N' ? (tr[1] == 'N' ? 1 : 2) : (tr[1] == 'N' ? 3 : 4);
            int i, j, x;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < k; j++)
                {
                    d = 0.0;
                    switch (f)
                    {
                        case 1: for ( x = 0; x < m; x++) d += A[i + x * n] * B[x + j * m]; break;
                        case 2: for ( x = 0; x < m; x++) d += A[i + x * n] * B[j + x * k]; break;
                        case 3: for ( x = 0; x < m; x++) d += A[x + i * m] * B[x + j * m]; break;
                        case 4: for ( x = 0; x < m; x++) d += A[x + i * m] * B[j + x * k]; break;

                    }
                    if (beta == 0.0) C[i + j * n] = alpha * d;
                    else C[i + j * n] = alpha * d + beta * C[i + j * n];
                }
            }

            return C;
        }

        /// <summary>
        /// modified lambda(mlambda) search
        /// 改良的、修正的LAMBDA
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="L"></param>
        /// <param name="D"></param>
        /// <param name="zs"></param>
        /// <param name="zn"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private int msearch(int n, int m, ref double[] L, ref double[] D, ref double[] zs, ref double[] zn, ref double[] s)
        {
            int i, j, k, c, nn = 0, imax = 0;



            double newdist, maxdist = 1E99, y;
            double[] S = new double[n * n];
            double[] dist = new double[n * 1];
            double[] zb = new double[n * 1];
            double[] z = new double[n * 1];
            double[] step = new double[n * 1];

            k = n - 1; dist[k] = 0.0;
            zb[k] = zs[k];
            z[k] = Math.Floor(zb[k] + 0.5); y = zb[k] - z[k]; step[k] = SGN(y);
            for (c = 0; c < loopMax; c++)
            {
                newdist = dist[k] + y * y / D[k];
                if (newdist < maxdist)
                {
                    if (k != 0)
                    {
                        dist[--k] = newdist;
                        for (i = 0; i <= k; i++)
                            S[k + i * n] = S[k + 1 + i * n] + (z[k + 1] - zb[k + 1]) * L[k + 1 + i * n];
                        zb[k] = zs[k] + S[k + k * n];
                        z[k] = Math.Floor(zb[k] + 0.5); y = zb[k] - z[k]; step[k] = SGN(y);
                    }
                    else
                    {
                        if (nn < m)
                        {
                            if (nn == 0 || newdist > s[imax]) imax = nn;
                            for (i = 0; i < n; i++) zn[i + nn * n] = z[i];
                            s[nn++] = newdist;
                        }
                        else
                        {
                            if (newdist < s[imax])
                            {
                                for (i = 0; i < n; i++) zn[i + imax * n] = z[i];
                                s[imax] = newdist;
                                for (i = imax = 0; i < m; i++) if (s[imax] < s[i]) imax = i;
                            }
                            maxdist = s[imax];
                        }
                        z[0] += step[0]; y = zb[0] - z[0]; step[0] = -step[0] - SGN(step[0]);
                    }
                }
                else
                {
                    if (k == n - 1) break;
                    else
                    {
                        k++;
                        z[k] += step[k]; y = zb[k] - z[k]; step[k] = -step[k] - SGN(step[k]);
                    }
                }
            }
            for (i = 0; i < m - 1; i++)
            { /* sort by s */
                for (j = i + 1; j < m; j++)
                {
                    if (s[i] < s[j]) continue;
                    double tmp = s[i]; s[i] = s[j]; s[j] = tmp;
                    for (k = 0; k < n; k++)
                    {
                        double tmp_ = zn[k + i * n];
                        zn[k + i * n] = zn[k + j * n];
                        zn[k + j * n] = tmp_;
                    }
                }
            }
            //free(S); free(dist); free(zb); free(z); free(step);

            if (c >= loopMax)
            {

                return -1;
            }
            return 0;
        }

        /// <summary>
        /// solve linear equation
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="A"></param>
        /// <param name="Y"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        private int solve(char[] tr, double[] A, double[] Y, int n, int m, ref double[] X)
        {
            double[] B = new double[n * n];

            int info = 0;
            for (int i = 0; i < n * n; i++) B[i] = A[i];

            string tmp=( tr[0] == 'N' ? "NN" : "TN");
            char[] tmp_ = new char[2]; tmp_[0] = tmp[0]; tmp_[1] = tmp[1];
            if ((info = matinv(ref B, n)) != -1)
                matmul(tmp_, n, m, n, 1.0, B, Y, 0.0, ref X);

            return info;

        }

        /// <summary>
        /// inverse of matrix
        /// </summary>
        /// <param name="A"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private int matinv(ref double[] A, int n)
        {
            double d = 0;
            int i, j;
            int[] indx = new int[n];
            double[] B = new double[n * n]; for (i = 0; i < n * n; i++) B[i] = A[i];

            
            if (ludcmp(ref B, n, ref indx, ref d) == -1) { return -1; }

            for (j = 0; j < n; j++) 
            {
                for (i = 0; i < n; i++) A[i + j * n] = 0.0; A[j + j * n] = 1.0;

                double[] tmp = new double[n]; for (int k = 0; k < n; k++) tmp[k] = A[j * n + k];
                lubksb(B, n, indx, ref tmp);
                for (int k = 0; k < n; k++) A[j * n + k] = tmp[k];
            }
            return 0;
        }


        /* LU decomposition ----------------------------------------------------------*/
        private int ludcmp(ref double[] A, int n, ref int[] indx, ref double d)
        {
            double big, s, tmp;
            double[] vv = new double[n * 1];
            int i, imax = 0, j, k;

            d = 1.0;

            for (i = 0; i < n; i++)
            {
                big = 0.0; for (j = 0; j < n; j++) if ((tmp = Math.Abs(A[i + j * n])) > big) big = tmp;
                if (big > 0.0) vv[i] = 1.0 / big; else { return -1; }
            }
            for (j = 0; j < n; j++)
            {
                for (i = 0; i < j; i++)
                {
                    s = A[i + j * n]; for (k = 0; k < i; k++) s -= A[i + k * n] * A[k + j * n]; A[i + j * n] = s;
                }
                big = 0.0;
                for (i = j; i < n; i++)
                {
                    s = A[i + j * n]; for (k = 0; k < j; k++) s -= A[i + k * n] * A[k + j * n]; A[i + j * n] = s;
                    if ((tmp = vv[i] * Math.Abs(s)) >= big) { big = tmp; imax = i; }
                }
                if (j != imax)
                {
                    for (k = 0; k < n; k++)
                    {
                        tmp = A[imax + k * n]; A[imax + k * n] = A[j + k * n]; A[j + k * n] = tmp;
                    }
                    d = -(d); vv[imax] = vv[j];
                }
                indx[j] = imax;
                if (A[j + j * n] == 0.0) { return -1; }
                if (j != n - 1)
                {
                    tmp = 1.0 / A[j + j * n]; for (i = j + 1; i < n; i++) A[i + j * n] *= tmp;
                }
            }

            return 0;
        }

        /* LU back-substitution ------------------------------------------------------*/
        private void lubksb(double[] A, int n, int[] indx, ref double[] b)
        {
            double s;
            int i, ii = -1, ip, j;

            for (i = 0; i < n; i++)
            {
                ip = indx[i]; s = b[ip]; b[ip] = b[i];
                if (ii >= 0) for (j = ii; j < i; j++) s -= A[i + j * n] * b[j]; 
               // else if (s == 0.0) ii = time;
                else ii = i;
                b[i] = s;
            }
            for (i = n - 1; i >= 0; i--)
            {
                s = b[i]; for (j = i + 1; j < n; j++) s -= A[i + j * n] * b[j]; b[i] = s / A[i + i * n];
            }
        }

        private double SGN(double x)
        {

            return ((x) <= 0.0 ? -1.0 : 1.0);
        }

        #endregion


        /// <summary>
        /// maximum count of search loop
        /// </summary>
        private int loopMax = 10000; 





        public bool process()
        {
            bool info = true;
            
            int MaxCan = 2;

            ArrayMatrix Zti = new ArrayMatrix(n, n); for (int i = 0; i < n; i++) Zti[i, i] = 1; // initialize Z to a unit matrix

            //make estimates in 'a' between -1 and +1 by subtracting an integer number,store the increments in ak (= shift the centre of the elliposid over the grid by an integer translation)

            double[] vl = new double[n]; // work array of length n

            double[] ak = new double[n]; // contains the (integer) increments of the original float ambiguities

            for (int i = 0; i < n; i++)
            {
                //
                ak[i] = Math.Truncate(a[i]);

             //   double kkk = Math.Round(a[time]);

                vl[i] = a[i] - ak[i];

                a[i] = vl[i];
            }

            //make the L_1^{-TProduct} D_1^{-1} L_1^{-1} decomposition of the covariance matrix Q
           
            //case 1:
            double[] D = new double[n];
            ArrayMatrix L = new ArrayMatrix(n, n);
            info = LTDL(D, L);

            if (info == false) return info;
           
            ////case 2:
            //EigenvalueDecomposition EigenvalueDecomposition = new Geo.Algorithm.EigenvalueDecomposition(Q);
            //double[] DD = EigenvalueDecomposition.RealEigenvalues;
            //Matrix LL = EigenvalueDecomposition.EigenvectorMatrix;


            //Compute the Z-transformation based on L and D of Q, ambiguities are transformed according to Z^TProduct.

            Re_Order(Zti, D, L);

            ArrayMatrix Zt = Zti.Inverse; //inversion yields transpose of Z 

            //For the search we need L and D of Q^{-1}


            L= L_Inv(L);

            for (int i = 0; i < n; i++) D[i] = 1.0 / D[i];

            //find a suitable Chi^2 such that we have dayServices candidates an minimum:
            //use an eps to make sure the dayServices candidates are inside the ellipsoid some small number
            double eps = Math.Pow(10, -6);
            double[] v3 = Chistart(D, L);

            double Chi_1 = v3[1] + eps;
            //find the dayServices candidates with minimun norm
            ArrayMatrix cands = new ArrayMatrix(n, MaxCan); // 2-dimensional array to store the candidates
            ArrayMatrix disal1 = new ArrayMatrix(1, MaxCan); //according squared norms 
            int ipos = 0;
            int ncan = 0;

            info = Search(Chi_1, MaxCan, n, D, L, a, ref disal1, ref cands, ref ipos, ref ncan);
            if (info == false) return info;
           



            //back-transformation (and adding increments)
            for (int i = 0; i < n; i++)
            {
                a[i]=0.0;
                for (int j = 0; j < n; j++)
                {
                    a[i] += Zti[i, j] * cands[j, ipos];
                }
                a[i] = a[i] + ak[i];
            }

            //'sort' the squared norms in increasing order (if ipos equals 1, the best candidate is in the second place: so reverse disal1)
            if (ipos == 1)
            {
                double h = disal1[0, 0];
                disal1[0, 0] = disal1[0, 1];
                disal1[0, 1] = h;
            }

            return true;
        }

        /// <summary>
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
        /// <param name="n">dimension of matrix</param>
        /// <param name="D">diagonal matrix</param>
        /// <param name="L">kiwer truabgykar natrux, only stroe lower triangular</param>
        /// <param name="a">vector of real valued estimates(float solution)</param>
        /// <param name="cands">2-dimensional array to store the candidates</param>
        /// <param name="disal1">according squared norms</param>
        /// <param name="ipos">column number in 'cands' where the candidate belonging to the minimum distance is stored</param>
        /// <param name="ncan">number of integer vectors found</param>
        private bool Search(double Chic,int MaxCan,int n, double[] D, ArrayMatrix L,double[] a,ref  ArrayMatrix disal1,ref ArrayMatrix cands, ref int ipos, ref int ncan)
        {
            bool info = true;

            cands = new ArrayMatrix(n, MaxCan); // 2-dimensional array to store the 
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
                if (n < 2)
                {
                    throw new Exception("Error: dimension of system < 2");
                }
            }
            bool ende = false;
            double[] right = new double[n + 1];
            double[] left = new double[n + 1];

            right[n] = Chic;
            double[] dq = new double[n];

            for (int i = 0; i < n - 1; i++) dq[i] = D[i + 1] / D[i];

            dq[n - 1] = 1 / D[n - 1];



            int icurrent = n;
            int iold = icurrent;
 
            double[] lef = new double[n];
               
            double[] dist1 = new double[n];

            double[] endd = new double[n];

            int c = 0; int loopMax = 10000; //maximum count of search loop

            while (ende == false)
            {
               

                icurrent = icurrent - 1;   //go a leval deeper . we were here before only one dist is one bigger
                if (iold <= icurrent)
                {
                    lef[icurrent] = lef[icurrent] + L[icurrent + 1, icurrent];
                }
                else
                {
                    lef[icurrent] = 0;
                    for (int j = icurrent + 1 ; j < n; j++)
                    {

                        lef[icurrent] = lef[icurrent] + L[j, icurrent] * dist1[j];
                    }
                }
                iold = icurrent; //keep track of level

                right[icurrent] = (right[icurrent + 1] - left[icurrent + 1]) * dq[icurrent];

                double reach = Math.Sqrt(right[icurrent]);

                //delta=a[time]-reach-lef[time] is the left border
                //ceil(delta) is the left most integer in the interval 
                //idst[time] is the distance of this left most integer to the a_hat
                double delta = a[icurrent] - reach - lef[icurrent];
                dist1[icurrent] = Math.Ceiling(delta) - a[icurrent];

               

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
                    Collects(MaxCan, D, Chic, cands, disal1, ref tmax, ref imax, right, left, ref ncan, lef, dist1, endd);
                    Backtrack(ref ende, left, ref icurrent, lef, dist1, endd);
                }


                c++;
                if (c > loopMax)
                {
                    info = false;
                    return info;
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
                for (int j = 0; j < n; j++) cands[j, i] = cands[j, i] + a[j];
            }

            return info;
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
            for (int i = 0; i < n; i++) cands[i, ipos-1] = dist[i];
            disal1[0, ipos-1] = t;
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
            while (c_stop == false && i < n - 1)
            {
                i = i + 1;
                if (dist[i] <= endd[i])
                {
                    dist[i] = dist[i] + 1;
                    left[i] = Math.Pow(dist[i] + lef[i], 2);
                    c_stop = true;
                    if (i == n - 1)
                    { cand_n = true; }
                }
            }
            if (i == n - 1 && cand_n == false)
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
            double[] dist = new double[n];
            double[] e = new double[n];
            double[] t = new double[n];

            for (int i = 0; i < n; i++) dist[i] = Math.Round(a[i]) - a[i];

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    e[i] += L[j, i] * dist[j];
                }
            }

            double t_0 = 0;
            for (int i = 0; i < n; i++) t_0 = t_0 + D[i] * e[i] * e[i];

            tm[0] = t_0;
            tm[1] = bigNum;


            //for 1 to n take the second nearest integer to a_i
            for (int i = 0; i < n; i++)
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
        /// Compute the inverse of a lower triangular matrix
        /// </summary>
        /// <param name="L"></param>
        private ArrayMatrix L_Inv(ArrayMatrix L)
        {
            ArrayMatrix Lm = new ArrayMatrix(n, n);

            double[] vec = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= i - 1; j++) vec[j] = L[i, j];

                for (int j = 0; j <= i - 1; j++)
                {
                    for (int k = j; k <= i - 1; k++)
                    {
                        Lm[i, j] += -Lm[k, j] * vec[k] / L[i, i];
                    }
                }
                Lm[i, i] = 1 / L[i, i];
            }
            return Lm;
        }


        /// <summary>
        /// Computation of the Z-transformation matrix
        /// The final Z-transformation is constructed from a sequence of interchanges of dayServices neighbouring ambiguities(this function) and
        /// integer Gauss transformations (function ztransi) that pair-wise decorrelate the ambiguities
        /// </summary>
        /// <param name="Zti"></param>
        /// <param name="D"></param>
        /// <param name="L"></param>
        private void Re_Order(ArrayMatrix Zti, double[] D, ArrayMatrix L)
        {
            int i1 = n - 1;
            bool sw = true; //the logical sw traces whether an interchange has occured or not 

            while (sw == true)
            {
                int i = n - 1;
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

                        for (int j = i + 2; j < n; j++) // to be coded effectively: interchange of columns L[time+2:n,time] and L[time+2:n,time+1]
                        {
                            temp = L[j, i];
                            L[j, i] = L[j, i + 1];
                            L[j, i + 1] = temp;
                        }

                        for (int j = 0; j < n; j++) // to be coded effectively: swap Zti[1:n,time] with Zti[1:n,time+1]
                        {
                            temp = Zti[j, i];
                            Zti[j, i] = Zti[j, i + 1];
                            Zti[j, i + 1] = temp;
                        }

                        temp = a[i];
                        a[i] = a[i + 1];
                        a[i + 1] = temp;

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
        private void Ztransi(ArrayMatrix Zti, ArrayMatrix L, int first,int last)
        {
            for (int i = last; i >= first; i--)
            {
                for (int j = i + 1; j < n; j++)
                {
                    double mu = Math.Round(L[j, i]);
                    if (mu != 0)
                    {
                        for (int s = j; s < n; s++)
                        {
                            L[s, i] = L[s, i] - mu * L[s, j];
                        }
                        for (int s = 0; s < n; s++) Zti[s, j] = Zti[s, j] + mu * Zti[s, i];
                        a[i] = a[i] - mu * a[j];
                    }
                }
            }
        }

        /// <summary>
        /// factorization of Q into L^TProduct D L
        /// </summary>
        /// <param name="D"></param>
        /// <param name="L"></param>
        private bool LTDL(double[] D, ArrayMatrix L)
        {
            for (int i = n - 1; i >= 0; i--)
            {
                D[i] = Q[i, i];
                if (Q[i, i] <= 0.0)
                {
                    return false;
                }
                for (int j = 0; j <= i; j++) L[i, j] = Q[i, j] / Math.Sqrt(Q[i, i]);

                for (int j = 0; j <= i - 1; j++)
                {
                    for (int k = 0; k <= j; k++)
                    {
                        Q[j, k] = Q[j, k] - L[i, k] * L[i, j];
                    }
                }
                for (int j = 0; j <= i; j++) L[i, j] = L[i, j] / L[i, i];
            }
            return true;
        }

        /// <summary>
        /// dimension of matrix Q and vector a 
        /// </summary>
        public int n { get; set; }

        /// <summary>
        /// 备选的值，通常为两组，一组最优，一组次优，便于RATIO判别
        /// </summary>
        public int m { get; set; }
        /// <summary>
        /// variance covariance matrix(symmetric)
        /// </summary>
        public IMatrix Q { get; set; }

        /// <summary>
        /// 数组表示的浮点方差矩阵
        /// </summary>
        public double[][] Qa { get; set; }

        /// <summary>
        /// the vetor with real valued estimated(float solution)
        /// </summary>
        public double[] a { get; set; }

    }
}
