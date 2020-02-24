using System;

namespace GeodeticX
{
	/// <summary>
	/// 点的高斯平面坐标
	/// </summary>
	public class GaussCoordinate
	{
        /// <summary>
        /// 高斯平面纵坐标
        /// </summary>
        private double m_x;

        /// <summary>
        /// 高斯平面横坐标
        /// </summary>
        private double m_y;

        /// <summary>
        /// 正常高
        /// </summary>
        private double m_h;

        /// <summary>
        /// 是否是假定坐标
        /// </summary>
        private bool m_AssumedCoord;

        /// <summary>
        /// 高斯投影的带宽
        /// </summary>
        private int m_BeltWidh;

        /// <summary>
        /// 是否已赋值的标记
        /// </summary>
        private bool xyIsNull;
        private bool hIsNull;

        public GaussCoordinate()
        {
            xyIsNull = true;
            hIsNull = true;
        }

        /// <summary>
        /// 带坐标值的初始化函数，由于xy必须成对出现，因此只能在此设置坐标值，不能分开设置
        /// </summary>
        /// <param name="x">纵坐标值</param>
        /// <param name="y">横坐标值</param>
        public GaussCoordinate(double x, double y)
        {
            m_x = x;
            m_y = y;
            xyIsNull = false;

            m_AssumedCoord = (y < 1000000) ? false : true;
            m_BeltWidh = 6;

            hIsNull = true;
        }

        /// <summary>
        /// 带坐标值的初始化函数
        /// </summary>
        /// <param name="x">纵坐标值</param>
        /// <param name="y">横坐标值</param>
        /// <param name="h">正常高值</param>
        public GaussCoordinate(double x, double y, double h)
            : this(x, y)
        {
            m_h = h;
            hIsNull = false;
        }

        /// <summary>
        /// 带坐标值的初始化函数
        /// </summary>
        /// <param name="x">纵坐标值</param>
        /// <param name="y">横坐标值</param>
        /// <param name="h">正常高值</param>
        /// <param name="beltWidth">投影分带的带宽</param>
        public GaussCoordinate(double x, double y, double h, int beltWidth)
            : this(x, y, h)
        {
            //仅接受3和6两个数值
            if (BeltWidth == 3 || BeltWidth == 6)
                m_BeltWidh = BeltWidth;
            else
                throw new Exception("错误的投影带宽");
        }

        /// <summary>
        /// 获取纵坐标值
        /// </summary>
        public double x
        {
            get
            {
                if (!xyIsNull)
                    return m_x;
                else
                    throw new Exception("平面纵坐标为空");
            }
        }

        /// <summary>
        /// 获取横坐标值
        /// </summary>
        public double y
        {
            get
            {
                if (!xyIsNull)
                    return m_y;
                else
                    throw new Exception("平面横坐标为空");
            }
        }

        /// <summary>
        /// 获取横坐标值（自然坐标）
        /// </summary>
        public double y0
        {
            get
            {
                if (!xyIsNull)
                {
                    if (m_AssumedCoord)
                        return m_y - 1000000 * BeltNumber + 500000;
                    else
                        return m_y;
                }
                else
                    throw new Exception("平面横坐标为空");
            }
        }

        /// <summary>
        /// 获取正常高值
        /// </summary>
        public double h
        {
            get
            {
                if (!hIsNull)
                    return m_h;
                else
                    throw new Exception("正常高为空");
            }
            set
            {
                m_h = value;
                hIsNull = false;
            }
        }

        /// <summary>
        /// 获取/设置是否为假定坐标
        /// </summary>
        public bool AssumedCoord
        {
            get
            {
                if (!xyIsNull)
                    return m_AssumedCoord;
                else
                    throw new Exception("坐标值为空");
            }
            set
            {
                if (!value && m_AssumedCoord)
                {
                    m_y -= Math.Floor(m_y / 1000000) * 1000000 + 500000;
                    m_AssumedCoord = value;
                }
                else if (value && !m_AssumedCoord)
                    throw new Exception("无法知道带号，不能转换为通用/假定坐标!");
            }
        }

        /// <summary>
        /// 获取/设置投影分带的带宽
        /// </summary>
        public int BeltWidth
        {
            get
            {
                if (!xyIsNull)
                    return m_BeltWidh;
                else
                    throw new Exception("坐标值为空");
            }
            set
            {
                //仅接受3和6两个数值
                if (value == 3 || value == 6)
                    m_BeltWidh = value;
            }
        }

        /// <summary>
        /// 获取/设置投影分带的带号
        /// </summary>
        public int BeltNumber
        {
            get
            {
                if (m_AssumedCoord)
                    return (int)Math.Floor(m_y / 1000000);
                else if (xyIsNull)
                    throw new Exception("坐标值为空");
                else
                    throw new Exception("当前为自然坐标，无法获取带号");
            }
            set
            {
                if (m_AssumedCoord && (int)Math.Floor(m_y / 1000000) != value)
                    throw new Exception("设置的带号与坐标值不符");
                else if (!m_AssumedCoord)
                {
                    m_AssumedCoord = true;
                    m_y += value * 1000000 + 500000;
                }
            }
        }

        public void SetCoordinate(double x, double y)
        {
            m_x = x;
            m_y = y;
            xyIsNull = false;

            m_AssumedCoord = (y < 1000000) ? false : true;
            m_BeltWidh = 6;
        }

        public void SetCoordinate(double x, double y, double h)
        {
            SetCoordinate(x, y);

            m_h = h;
            hIsNull = false;
        }

        public void SetCoordinate(double x, double y, double h, int beltWidth)
        {
            SetCoordinate(x, y, h);

            //仅接受3和6两个数值
            if (BeltWidth == 3 || BeltWidth == 6)
                m_BeltWidh = BeltWidth;
            else
                throw new Exception("错误的投影带宽");
        }

        public override string ToString()
        {
            string str = String.Empty;

            if (!xyIsNull)
            {
                str = "x=" + m_x.ToString("### ###.###");

                if (m_AssumedCoord)
                    str += ", y=" + m_y.ToString("0# ### ###.###");
                else
                    str += ", y=" + m_y.ToString("### ###.###");
            }
            else if (!hIsNull)
            {
                if (str.Length > 0)
                    str += ", h = " + m_h.ToString("# ###.###");
                else
                    str = "h=" + m_h.ToString("# ###.###");
            }

            return str;
        }
	}
}
