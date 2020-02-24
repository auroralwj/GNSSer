//2017.09.19，CuiYang, Chongqing,新增 Houesholder 变换

using System;
using Geo.Algorithm;


namespace Geo.Algorithm
{
    /// <summary>
    ///	  利用Householder变换把（系数）矩阵正交三角化，以便于求出最小二乘解，关键在于求正交矩阵 T
    /// </summary>
    /// <remarks>
    /// 对于m*n的A(m>=n)，rankA=r>0，（通常系数矩阵是列满秩），求正交矩阵T(m*m)，使得：
    ///  T * A = [U 0]'，其中U是k*n的上梯形矩阵，T=H1*H2*H3*...*Hk-1*Hk.
    /// </remarks>
    public class HouseholderTransform
    {
        /// <summary>
        /// 系数矩阵
        /// </summary>
        private Matrix B;

        /// <summary>
        /// 正交矩阵
        /// </summary>
        public Matrix T { get; set; }


       /// <summary>
        /// householder变换
       /// </summary>
       /// <param name="Q">待正交化的系数矩阵</param>
        public HouseholderTransform(Matrix A)
        {
            B = new Matrix( A.Clone());
            double[][] q = B.Array;
            int m = B.RowCount;
            int n = B.ColCount;

            //初始化
            Matrix H = new Matrix(m, m); for (int k = 0; k < m; k++) H[k, k] = 1;

            for (int j = 0; j < n; j++)
            {
                Vector Aj = new Vector(m - j);
                for (int i = j; i < m; i++) { Aj[i - j] = B[i, j]; }

                Vector vj = new Vector(m - j); double baita = 0;
                house(Aj, ref vj, ref baita);


                Matrix subAj = B.GetSub(j, j, m-j, n-j);
                Matrix subVj = new Matrix(m - j, m - j);
                for (int k = 0; k < m - j; k++)
                {
                    for (int s = 0; s < m - j; s++)
                    { subVj[k, s] = vj[k] * vj[s]; }
                }


                Matrix subIj = new Matrix(m - j, m - j); for (int k = 0; k < m - j; k++) subIj[k, k] = 1;


                Matrix Hj = new Matrix(m, m); for (int k = 0; k < m; k++) Hj[k, k] = 1;

                Matrix subHj = (subIj - baita * subVj);

               // Matrix newSubAj = subHj * subAj;

                for (int k = j; k < m; k++)
                    for (int s = j; s < m; s++)
                        Hj[k, s] = subHj[k - j, s - j];


                H =Hj * H ;
                B = Hj * B;

               
            }     
            this.T = H;  
        }

        /// <summary>
        /// x: 是原矩阵的某一列向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="v"></param>
        /// <param name="baita"></param>
        public void house(Vector x, ref Vector v, ref double baita)
        {
            //求长度
            double n = x.Count;

            double sum = 0.0;
            for (int i = 0; i < x.Count; i++)
            {
                sum += x[i] * x[i];
            }
            double det0 = Math.Sqrt(sum);


            double afa = -Math.Sign(x[0]) * det0;

            v[0] = x[0] - afa * 1;
            for (int i = 1; i < x.Count; i++) v[i] = x[i];


            double bk = afa * afa - afa * x[0];

            baita = 1 / bk;

        }


    }



}

