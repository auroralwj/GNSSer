//2016.12.07 double create in hongqing 三次样条插值

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Geo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser
{
    /// <summary>
    /// Three_Times_spline_Interpolation 三次样条插值
    /// </summary>
    class TTSIFuction
    {
        public double[] x = { 0 }, y = { 0 };
        public int edgeCondition;		//1为第一种边界条件，2为第一种边界条件，3为第一种边界条件
        public double head, tail;
        public int NodeNum;

        public void Cauc(double[]x,double []y)
        {
            NodeNum=x.Length-1;
            double[] i = new double[NodeNum];
            double[] j = new double[NodeNum];
            double[] k = new double[NodeNum + 1];
            double[] i_j_k = new double[3];

            k[0] = ((y[1] - y[0]) / (x[1] - x[0]) - head) * 6 / (x[1] - x[0]);
            k[NodeNum] = (tail - (y[NodeNum] - y[NodeNum - 1]) / (x[NodeNum] - x[NodeNum - 1])) * 6 / (x[NodeNum] - x[NodeNum - 1]);
            for (int b = 1; b < NodeNum; b++)
            {
                i[b] = (x[b] - x[b - 1]) / (x[b + 1] - x[b - 1]);
                j[b] = 1 - i[b];
                k[b] = ((y[b + 1] - y[b]) / (x[b + 1] - x[b]) - (y[b] - y[b - 1]) / (x[b] - x[b - 1])) * 6 / (x[b + 1] - x[b - 1]);
            }
            i_j_k[0] = (x[NodeNum] - x[NodeNum - 1]) / (x[1] - x[0] + x[NodeNum] - x[NodeNum - 1]);
            i_j_k[1] = 1 - i_j_k[0];
            i_j_k[2] = (i_j_k[0] * (y[1] - y[0]) / (x[1] - x[0]) + i_j_k[1] * (y[NodeNum] - y[NodeNum - 1]) / (x[NodeNum] - x[NodeNum - 1])) * 3;
            drawPic(x, y, SolveEquation(i, j, k, NodeNum, i_j_k, edgeCondition));

        }

        double[] SolveEquation(double[] i, double[] j, double[] k, int n, double[] i_j_k, int edgeCondition)
        {
            double[] M = new double[n + 1];

            for (int a = 0; a < n; a++)
            {
                M[a] = 0;
            }

            switch (edgeCondition)
            {
                case 1:

                    for (int p = 1; p < 100; p++)
                    {
                        //temp = M[0];

                        M[0] = (double)0.5 * (k[0] - M[1]);
                        for (int a = 1; a <= n - 1; a++)
                        {
                            M[a] = (double)0.5 * (k[a] - i[a] * M[a - 1] - j[a] * M[a + 1]);
                        }
                        M[n] = (double)0.5 * (k[n] - M[n - 1]);
                        //e=M[0]-temp;
                        p++;
                    }
                    break;

                case 2:

                    k[1] = k[1] - i[0] * head;
                    k[n - 1] = k[n - 1] - j[n - 1] * tail;
                    i[1] = 0;
                    j[n - 1] = 0;
                    M[0] = head;
                    M[n] = tail;
                    for (int p = 1; p < 100; p++)
                    {

                        for (int a = 1; a <= n - 1; a++)
                        {
                            M[a] = (double)0.5 * (k[a] - i[a] * M[a - 1] - j[a] * M[a + 1]);
                        }

                    }

                    break;
                case 3:
                    double[] Q = new double[n];

                    for (int a = 0; a < n; a++)
                    {
                        Q[a] = 0;
                    }

                    for (int p = 1; p < 100; p++)
                    {
                        Q[0] = (double)0.5 * (k[1] - i[0] * Q[n - 1] - j[0] * Q[1]);
                        for (int a = 1; a <= n - 2; a++)
                        {
                            Q[a] = (double)0.5 * (k[a + 1] - i[a] * Q[a - 1] - j[a] * Q[a + 1]);
                        }
                        Q[n - 1] = (double)0.5 * (i_j_k[2] - i_j_k[1] * Q[0] - i_j_k[0] * Q[n - 2]);
                    }
                    break;
            }
            return M;
        }

        public double[] getValueofPolyY(double[] X, double[] Y, double[] M, double[] UnkownX)
        {
            int n = UnkownX.Length;
            double[] UnknownY = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < X.Length; j++)
                {
                    if (X[j] < UnkownX[i] && X[j + 1] > UnkownX[i])
                    {
                        double x1 = X[j]; double x2 = X[j + 1]; double x = UnkownX[i]; double fitY;
                        fitY = M[j] * Math.Pow((x2 - x), 3);
                        fitY = fitY + M[j + 1] * Math.Pow((x - x1), 3);
                        fitY = fitY + (6 * Y[j] - M[j] * Math.Pow((x2 - x1), 2)) * (x2 - x);
                        fitY = fitY + (6 * Y[j + 1] - M[j + 1] * Math.Pow((x2 - x1), 2)) * (x - x1);
                        UnknownY[i] = (double)((fitY / (x2 - x1)) / 6);

                    }
                }

            }
            return UnknownY;
        }
        void drawPic(double[] X, double[] Y, double[] M)
        {
            try
            {
                double y;
                double y_temp = 0;
               
                for (int q = 0; q < NodeNum; q++)
                {
                    double temp_x = X[q];
                    for (double x = (double)(X[q] + 0.0005); x <= X[q + 1]; x = (double)(x + 0.0005))
                    {
                        y_temp = M[q] * Math.Pow((X[q + 1] - x), 3);
                        y_temp = y_temp + M[q + 1] * Math.Pow((x - X[q]), 3);
                        y_temp = y_temp + (6 * Y[q] - M[q] * Math.Pow((X[q + 1] - X[q]), 2)) * (X[q + 1] - x);
                        y_temp = y_temp + (6 * Y[q + 1] - M[q + 1] * Math.Pow((X[q + 1] - X[q]), 2)) * (x - X[q]);
                        y = (double)((y_temp / (X[q + 1] - X[q])) / 6);
                    }
                }
            }
            catch { Exception ex; }
        }
    }
}



