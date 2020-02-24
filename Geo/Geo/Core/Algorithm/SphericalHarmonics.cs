//2017.11.06, czs, create in hongqing, Spherical harmonics 球谐函数

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// Spherical harmonics 球谐函数
    /// </summary>
    public class SphericalHarmonics
    {
        const int N = 12;//数据行数
        const int Num = 3;//坐标个数
        const int N_degree = 4;//运算阶数
        const double GM = 3986004.415e+008;
        const double R = 3678136.3;
        int i, j;
        int n, m; double phi; double[] x;//[Num];
        double[][] result;//[Num][3];
        double temp1, temp2;
        double[][] coordinate;//[Num][3];
        double[][] data;//[N][6];

        //int main0()
        //{ 
        //    //从data文件以及input文件读取数据
        //   ifstream in1("data.txt");
        //   for (i=0;i<N;i++)
        //   {
        //       for(j=0;j<6;j++)
        //       {
        //           in1>>data[i][j];
        //       }
        //   }
        //   in1.close();
        //   ifstream in2("input.txt");
        //   for (i=0;i<Num;i++)
        //   {
        //       for(j=0;j<3;j++)
        //       {
        //           in2>>coordinate[i][j];
        //       }
        //   }
        //   in2.close();
        //    //数值计算
        //   for(i=0;i<Num;i++)
        //   {
        //       result[i][1]=sigma_g_rou(coordinate[i][1],coordinate[i][2],coordinate[i][3]);
        //       result[i][2]=sigma_g_phi(coordinate[i][1],coordinate[i][2],coordinate[i][3]);
        //       result[i][3]=sigma_g_lammbda(coordinate[i][1],coordinate[i][2],coordinate[i][3]);
        //   }
        //    //将运算结果写入output文件
        //    ofstream outf;
        //    outf.open("OUTPUT.txt");
        //    outf<<"计算结果为："<<endl;
        //    for(int i=0;i<Num;i++)  
        //    {  
        //       for(j=0;j<3;j++)
        //       {
        //        outf<<result[i][j]<<"\t";
        //       }
        //       outf<<endl;
        //    }
        //    outf.close();
        //    system("pause");
        //    return 0;
        //}

        //勒让德多项式函数
        public double Leg(int n, int m, double phi)
        {
            if (n == 0 && m == 0)
            {
                temp1 = 1.0;
            }
            else if (n == 1 && m == 0)
            {
                temp1 = sqrt(3.0) * sin(phi);
            }
            else if (n == 1 && m == 1)
            {
                temp1 = sqrt(3.0) * cos(phi);
            }
            else if (n == m)
            {
                temp1 = (sqrt((double)((2 * n + 1) / (2 * n)))) * cos(phi) * Leg(n - 1, n - 1, phi);
            }
            else if (n < m)
            {
                temp1 = 0.0;
            }
            else
            {
                temp1 = ((sqrt((double)((4 * n * n * n) - 1) / (n * n - m * m))) * sin(phi) * Leg(n - 1, m, phi)) - ((sqrt((double)((2 * n + 1) / (2 * n - 3)) * ((n - 1) * (n - 1) - m * m) / (n * n - m * m))) * Leg(n - 2, m, phi));
            }
            return temp1;
        }
        //计算勒让德导数函数
        double D_leg(int n, int m, double phi)
        {
            temp2 = (sqrt((double)((2 * n + 1) / (2 * n - 1)) * (n * n - m * m))) * (Leg(n - 1, m, phi) / cos(phi)) - n * tan(phi) * Leg(n, m, phi);
            return temp2;
        }
        //从data中取出位系数C
        double C(int n, int m)
        {
            int N_change0 = 0, N_change = 0;
            double temp_c;
            for (i = 3; i < N_degree + 1; i++)
            {
                N_change0 = N_change0 + i;
            }
            N_change = N_change0 + m + 1;
            temp_c = data[N_change][3] + data[N_change][5];
            return temp_c;
        }
        //从data中取出位系数S
        double S(int n, int m)
        {
            int N_change0 = 0, N_change;
            double temp_s;
            for (i = 3; i < N_degree + 1; i++)
            {
                N_change0 = N_change0 + i;
            }
            N_change = N_change0 + m + 1;
            temp_s = data[N_change][4] + data[N_change][6];
            return temp_s;
        }
        //计算向径rou方向重力分量
        double sigma_g_rou(double r, double phi, double lammbda)
        {
            double temp_rou0 = 0.0, temp_rou1 = 0.0, temp_rou2 = 0.0, temp_rou3 = 0.0, temp_3;
            for (i = 2; i <= N_degree; i++)
            {
                for (j = 0; j <= i; j++)
                {
                    temp_rou0 = (C(i, j) * cos(j * lammbda) + S(i, j) * sin(j * lammbda)) * Leg(i, j, phi);
                    temp_rou1 = temp_rou0 + temp_rou1;
                }
                temp_rou2 = temp_rou1 * (i + 1) * pow((R / r), i);
                temp_rou3 = temp_rou2 + temp_rou3;
            }
            temp_3 = (-GM / (r * r)) * temp_rou3;
            return temp_3;
        }
        //计算phi方向的重力分量
        double sigma_g_phi(double r, double phi, double lammbda)
        {
            double temp_phi0 = 0.0, temp_phi1 = 0.0, temp_phi2 = 0.0, temp_phi3 = 0.0, temp_4;
            for (i = 2; i <= N_degree; i++)
            {
                for (j = 0; j <= i; j++)
                {
                    temp_phi0 = (C(i, j) * cos(j * lammbda) + S(i, j) * sin(j * lammbda)) * D_leg(i, j, phi);
                    temp_phi1 = temp_phi0 + temp_phi1;
                }
                temp_phi2 = temp_phi1 * pow((R / r), i);
                temp_phi3 = temp_phi2 + temp_phi3;
            }
            temp_4 = (GM / (r * r)) * temp_phi3;
            return temp_4;
        }
        //计算lammbda方向的重力分量
        double sigma_g_lammbda(double r, double phi, double lammbda)
        {
            double temp_lammbda0 = 0.0, temp_lammbda1 = 0.0, temp_lammbda2 = 0.0, temp_lammbda3 = 0.0, temp_5;
            for (i = 2; i <= N_degree; i++)
            {
                for (j = 0; j <= i; j++)
                {
                    temp_lammbda0 = j * (S(i, j) * sin(j * lammbda) - C(i, j) * cos(j * lammbda)) * Leg(i, j, phi);
                    temp_lammbda1 = temp_lammbda0 + temp_lammbda1;
                }
                temp_lammbda2 = temp_lammbda1 * pow((R / r), i);
                temp_lammbda3 = temp_lammbda2 + temp_lammbda3;
            }
            temp_5 = (GM / ((r * r) * cos(phi))) * temp_lammbda3;
            return temp_5;
        }

        public static double cos(double rad) { return Math.Cos(rad); }
        public static double sin(double rad) { return Math.Sin(rad); }
        public static double tan(double rad) { return Math.Tan(rad); }
        public static double sqrt(double rad) { return Math.Sqrt(rad); }
        public static double pow(double x, double y) { return Math.Pow(x, y); }
    }
}
