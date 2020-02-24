using System;
using System.Text;
using System.Globalization;

namespace GeodeticX
{
    /// <summary>
    /// 转换
    /// </summary>
    public class TransformParameters
    {
        /// <summary>
        /// 三个平移参数
        /// </summary>
        private double m_Dx, m_Dy, m_Dz;
        /// <summary>
        /// DX
        /// </summary>
        public double Dx
        {
            get 
            {
                if (m_count >= 3) return m_Dx;
                else throw new Exception("未赋值的参数");
            } 
        }
        /// <summary>
        /// DY
        /// </summary>
        public double Dy
        {
            get
            {
                if (m_count >= 3) return m_Dy;
                else throw new Exception("未赋值的参数");
            }
        }
        /// <summary>
        /// DZ
        /// </summary>
        public double Dz
        {
            get
            {
                if (m_count >= 3) return m_Dz;
                else throw new Exception("未赋值的参数");
            }
        }
        /// <summary>
        /// 三个旋转参数
        /// </summary>
        private double m_Ex, m_Ey, m_Ez;
        /// <summary>
        /// EX
        /// </summary>
        public double Ex
        { 
            get 
            {
                if (m_count == 7) return m_Ex;
                else throw new Exception("未赋值的参数");
            } 
        }
        /// <summary>
        /// EY
        /// </summary>
        public double Ey
        {
            get
            {
                if (m_count == 7) return m_Ey;
                else throw new Exception("未赋值的参数");
            }
        }
        /// <summary>
        /// EZ
        /// </summary>
        public double Ez
        {
            get
            {
                if (m_count == 7) return m_Ez;
                else throw new Exception("未赋值的参数");
            }
        }
        /// <summary>
        /// 尺度因子
        /// </summary>
        private double m_m;
        /// <summary>
        /// 尺度因子
        /// </summary>
        public double m
        { 
            get 
            {
                if (m_count >= 4) return m_m;
                else throw new Exception("未赋值的参数");
            } 
        }

        private int m_count;
        /// <summary>
        /// /数量
        /// </summary>
        public int Count { get { return m_count; } }
        /// <summary>
        /// 构造
        /// </summary>

        public TransformParameters()
        {
            m_count = 0;
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="Dx"></param>
        /// <param name="Dy"></param>
        /// <param name="Dz"></param>
        public TransformParameters(double Dx, double Dy, double Dz)
        {
            m_Dx = Dx;
            m_Dy = Dy;
            m_Dz = Dz;
            m_count = 3;
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="Dx"></param>
        /// <param name="Dy"></param>
        /// <param name="Dz"></param>
        /// <param name="m"></param>
        public TransformParameters(double Dx, double Dy, double Dz, double m)
            : this(Dx, Dy, Dz)
        {
            m_m = m;
            m_count = 4;
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="Dx"></param>
        /// <param name="Dy"></param>
        /// <param name="Dz"></param>
        /// <param name="Ex"></param>
        /// <param name="Ey"></param>
        /// <param name="Ez"></param>
        /// <param name="m"></param>
        public TransformParameters(double Dx, double Dy, double Dz, double Ex, double Ey, double Ez, double m)
            : this(Dx, Dy, Dz, m)
        {
            m_Ex = Ex;
            m_Ey = Ey;
            m_Ez = Ez;
            m_count = 7;
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="Dx"></param>
        /// <param name="Dy"></param>
        /// <param name="Dz"></param>
        public void SetParameters(double Dx, double Dy, double Dz)
        {
            m_Dx = Dx;
            m_Dy = Dy;
            m_Dz = Dz;
            m_count = 3;
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="Dx"></param>
        /// <param name="Dy"></param>
        /// <param name="Dz"></param>
        /// <param name="m"></param>
        public void SetParameters(double Dx, double Dy, double Dz, double m)
        {
            SetParameters(Dx, Dy, Dz);
            m_m = m;
            m_count = 4;
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="Dx"></param>
        /// <param name="Dy"></param>
        /// <param name="Dz"></param>
        /// <param name="Ex"></param>
        /// <param name="Ey"></param>
        /// <param name="Ez"></param>
        /// <param name="m"></param>
        public void SetParameters(double Dx, double Dy, double Dz, double Ex, double Ey, double Ez, double m)
        {
            SetParameters(Dx, Dy, Dz, m);
            m_Ex = Ex;
            m_Ey = Ey;
            m_Ez = Ez;
            m_count = 7;
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = String.Empty;

            if (m_count >= 3)
                str += "Dx=" + m_Dx.ToString("0.000") +
                       ", Dy=" + m_Dy.ToString("0.000") +
                       ", Dz=" + m_Dz.ToString("0.000");

            if (m_count >= 4)
                str += ", m=" + m_m.ToString("0.000E+0");
            
            if (m_count == 7)
                str += ", Ex=" + m_Ex.ToString("0.000") +
                       ", Ey=" + m_Ey.ToString("0.000") +
                       ", Ez=" + m_Ez.ToString("0.000");

            return str;
        }
    }
}
