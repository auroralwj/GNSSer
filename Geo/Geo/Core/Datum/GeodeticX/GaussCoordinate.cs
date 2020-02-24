using System;

namespace GeodeticX
{
	/// <summary>
	/// ��ĸ�˹ƽ������
	/// </summary>
	public class GaussCoordinate
	{
        /// <summary>
        /// ��˹ƽ��������
        /// </summary>
        private double m_x;

        /// <summary>
        /// ��˹ƽ�������
        /// </summary>
        private double m_y;

        /// <summary>
        /// ������
        /// </summary>
        private double m_h;

        /// <summary>
        /// �Ƿ��Ǽٶ�����
        /// </summary>
        private bool m_AssumedCoord;

        /// <summary>
        /// ��˹ͶӰ�Ĵ���
        /// </summary>
        private int m_BeltWidh;

        /// <summary>
        /// �Ƿ��Ѹ�ֵ�ı��
        /// </summary>
        private bool xyIsNull;
        private bool hIsNull;

        public GaussCoordinate()
        {
            xyIsNull = true;
            hIsNull = true;
        }

        /// <summary>
        /// ������ֵ�ĳ�ʼ������������xy����ɶԳ��֣����ֻ���ڴ���������ֵ�����ֿܷ�����
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
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
        /// ������ֵ�ĳ�ʼ������
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="h">������ֵ</param>
        public GaussCoordinate(double x, double y, double h)
            : this(x, y)
        {
            m_h = h;
            hIsNull = false;
        }

        /// <summary>
        /// ������ֵ�ĳ�ʼ������
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="h">������ֵ</param>
        /// <param name="beltWidth">ͶӰ�ִ��Ĵ���</param>
        public GaussCoordinate(double x, double y, double h, int beltWidth)
            : this(x, y, h)
        {
            //������3��6������ֵ
            if (BeltWidth == 3 || BeltWidth == 6)
                m_BeltWidh = BeltWidth;
            else
                throw new Exception("�����ͶӰ����");
        }

        /// <summary>
        /// ��ȡ������ֵ
        /// </summary>
        public double x
        {
            get
            {
                if (!xyIsNull)
                    return m_x;
                else
                    throw new Exception("ƽ��������Ϊ��");
            }
        }

        /// <summary>
        /// ��ȡ������ֵ
        /// </summary>
        public double y
        {
            get
            {
                if (!xyIsNull)
                    return m_y;
                else
                    throw new Exception("ƽ�������Ϊ��");
            }
        }

        /// <summary>
        /// ��ȡ������ֵ����Ȼ���꣩
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
                    throw new Exception("ƽ�������Ϊ��");
            }
        }

        /// <summary>
        /// ��ȡ������ֵ
        /// </summary>
        public double h
        {
            get
            {
                if (!hIsNull)
                    return m_h;
                else
                    throw new Exception("������Ϊ��");
            }
            set
            {
                m_h = value;
                hIsNull = false;
            }
        }

        /// <summary>
        /// ��ȡ/�����Ƿ�Ϊ�ٶ�����
        /// </summary>
        public bool AssumedCoord
        {
            get
            {
                if (!xyIsNull)
                    return m_AssumedCoord;
                else
                    throw new Exception("����ֵΪ��");
            }
            set
            {
                if (!value && m_AssumedCoord)
                {
                    m_y -= Math.Floor(m_y / 1000000) * 1000000 + 500000;
                    m_AssumedCoord = value;
                }
                else if (value && !m_AssumedCoord)
                    throw new Exception("�޷�֪�����ţ�����ת��Ϊͨ��/�ٶ�����!");
            }
        }

        /// <summary>
        /// ��ȡ/����ͶӰ�ִ��Ĵ���
        /// </summary>
        public int BeltWidth
        {
            get
            {
                if (!xyIsNull)
                    return m_BeltWidh;
                else
                    throw new Exception("����ֵΪ��");
            }
            set
            {
                //������3��6������ֵ
                if (value == 3 || value == 6)
                    m_BeltWidh = value;
            }
        }

        /// <summary>
        /// ��ȡ/����ͶӰ�ִ��Ĵ���
        /// </summary>
        public int BeltNumber
        {
            get
            {
                if (m_AssumedCoord)
                    return (int)Math.Floor(m_y / 1000000);
                else if (xyIsNull)
                    throw new Exception("����ֵΪ��");
                else
                    throw new Exception("��ǰΪ��Ȼ���꣬�޷���ȡ����");
            }
            set
            {
                if (m_AssumedCoord && (int)Math.Floor(m_y / 1000000) != value)
                    throw new Exception("���õĴ���������ֵ����");
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

            //������3��6������ֵ
            if (BeltWidth == 3 || BeltWidth == 6)
                m_BeltWidh = BeltWidth;
            else
                throw new Exception("�����ͶӰ����");
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
