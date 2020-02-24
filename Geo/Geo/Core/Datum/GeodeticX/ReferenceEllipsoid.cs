using System;

namespace GeodeticX
{
	/// <summary>
	/// 参考椭球类
    ///
    /// 
    ///常用参考椭球参数
    ///a84 6378137
    ///f84 298.257223563
    /// 
    ///a54 6378245
    ///f54 298.3
    /// 
    ///a88 6378140
    ///f88 298.257
    /// 
	/// </summary>
	public class ReferenceEllipsoid
	{
        /// <summary>
        ///  坐标系名称
        /// </summary>
        private string m_name;

        /// <summary>
        /// 参考椭球的长半轴
        /// </summary>
        private double m_a;

        /// <summary>
        /// 参考椭球的扁率倒数
        /// </summary>
        private double m_f;

        private bool isNull;

        public ReferenceEllipsoid()
        {
            isNull = true;
        }

        /// <summary>
        /// 指定信息的坐标系初始化函数
        /// </summary>
        /// <param name="a">长半轴</param>
        /// <param name="f">扁率倒数</param>
        public ReferenceEllipsoid(double a, double f)
        {
            m_a = a;
            m_f = f;
            isNull = false;
        }

        /// <summary>
        /// 指定信息的坐标系初始化函数
        /// </summary>
        /// <param name="name">坐标系名称</param>
        /// <param name="a">长半轴</param>
        /// <param name="f">扁率倒数</param>
        public ReferenceEllipsoid(double a, double f, string name)
            : this(a, f)
        {
            m_name = name;
        }
        
        /// <summary>
        /// 获取/设置坐标系名称属性
        /// </summary>
        public string Name
        {
            get
            {
                if (!isNull)
                    return m_name;
                else
                    throw new Exception("未设置椭球信息");
            }
            set
            {
                m_name = value;
            }
        }

        /// <summary>
        /// 获取/设置参考椭球长半轴
        /// </summary>
        public double a
        {
            get
            {
                if (!isNull)
                    return m_a;
                else
                    throw new Exception("未设置椭球信息");
            }
        }

        /// <summary>
        /// 获取/设置参考椭球的便率倒数
        /// </summary>
        public double f
        {
            get
            {
                if (!isNull)
                    return m_f;
                else
                    throw new Exception("未设置椭球信息");
            }
        }

        /// <summary>
        /// 获取参考椭球的短半轴
        /// </summary>
        public double b
        {
            get
            {
                if (!isNull)
                    return m_a * (m_f - 1) / m_f;
                else
                    throw new Exception("未设置椭球信息");
           }
        }
        
        /// <summary>
        /// 获取极曲率半径
        /// </summary>
        public double c
        {
            get
            {
                if (!isNull)
                    return m_a * m_f / (m_f - 1);
                else
                    throw new Exception("未设置椭球信息");
            }
        }

        /// <summary>
        /// 获取第一偏心率
        /// </summary>
        public double e
        {
            get
            {
                if (!isNull)
                    return Math.Sqrt(2 * m_f - 1) / m_f;
                else
                    throw new Exception("未设置椭球信息");
           }
        }

        /// <summary>
        /// 获取第二偏心率
        /// </summary>
        public double e2
        {
            get
            {
                if (!isNull)
                    return Math.Sqrt(m_f * 2 - 1) / (m_f - 1);
                else
                    throw new Exception("未设置椭球信息");
            }
        }

        public void SetEllisoid(double a, double f)
        {
            m_a = a;
            m_f = f;
            isNull = false;
        }

        public void SetEllisoid(double a, double f, string name)
        {
            m_a = a;
            m_f = f;
            m_name = name;
            isNull = false;
        }

        public override string ToString()
        {
            if (!isNull)
                return "a=" + m_a.ToString() + ", f=" + m_f.ToString();
            else
                return base.ToString();
        }
	}
}
