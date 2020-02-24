//2017.09.19��CuiYang, Chongqing,���� Houesholder �任

using System;
using Geo.Algorithm;


namespace Geo.Algorithm
{
    /// <summary>
    ///	  ����Householder�任�ѣ�ϵ���������������ǻ����Ա��������С���˽⣬�ؼ��������������� T
    /// </summary>
    /// <remarks>
    /// ����m*n��A(m>=n)��rankA=r>0����ͨ��ϵ�������������ȣ�������������T(m*m)��ʹ�ã�
    ///  T * A = [U 0]'������U��k*n�������ξ���T=H1*H2*H3*...*Hk-1*Hk.
    /// </remarks>
    public class HouseholderTransform
    {
        /// <summary>
        /// ϵ������
        /// </summary>
        private Matrix B;

        /// <summary>
        /// ��������
        /// </summary>
        public Matrix T { get; set; }


       /// <summary>
        /// householder�任
       /// </summary>
       /// <param name="Q">����������ϵ������</param>
        public HouseholderTransform(Matrix A)
        {
            B = new Matrix( A.Clone());
            double[][] q = B.Array;
            int m = B.RowCount;
            int n = B.ColCount;

            //��ʼ��
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
        /// x: ��ԭ�����ĳһ������
        /// </summary>
        /// <param name="x"></param>
        /// <param name="v"></param>
        /// <param name="baita"></param>
        public void house(Vector x, ref Vector v, ref double baita)
        {
            //�󳤶�
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

