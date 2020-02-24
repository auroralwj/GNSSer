using System;

namespace GeodeticX
{
	/// <summary>
	/// �ο�������
    ///
    /// 
    ///���òο��������
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
        ///  ����ϵ����
        /// </summary>
        private string m_name;

        /// <summary>
        /// �ο�����ĳ�����
        /// </summary>
        private double m_a;

        /// <summary>
        /// �ο�����ı��ʵ���
        /// </summary>
        private double m_f;

        private bool isNull;

        public ReferenceEllipsoid()
        {
            isNull = true;
        }

        /// <summary>
        /// ָ����Ϣ������ϵ��ʼ������
        /// </summary>
        /// <param name="a">������</param>
        /// <param name="f">���ʵ���</param>
        public ReferenceEllipsoid(double a, double f)
        {
            m_a = a;
            m_f = f;
            isNull = false;
        }

        /// <summary>
        /// ָ����Ϣ������ϵ��ʼ������
        /// </summary>
        /// <param name="name">����ϵ����</param>
        /// <param name="a">������</param>
        /// <param name="f">���ʵ���</param>
        public ReferenceEllipsoid(double a, double f, string name)
            : this(a, f)
        {
            m_name = name;
        }
        
        /// <summary>
        /// ��ȡ/��������ϵ��������
        /// </summary>
        public string Name
        {
            get
            {
                if (!isNull)
                    return m_name;
                else
                    throw new Exception("δ����������Ϣ");
            }
            set
            {
                m_name = value;
            }
        }

        /// <summary>
        /// ��ȡ/���òο����򳤰���
        /// </summary>
        public double a
        {
            get
            {
                if (!isNull)
                    return m_a;
                else
                    throw new Exception("δ����������Ϣ");
            }
        }

        /// <summary>
        /// ��ȡ/���òο�����ı��ʵ���
        /// </summary>
        public double f
        {
            get
            {
                if (!isNull)
                    return m_f;
                else
                    throw new Exception("δ����������Ϣ");
            }
        }

        /// <summary>
        /// ��ȡ�ο�����Ķ̰���
        /// </summary>
        public double b
        {
            get
            {
                if (!isNull)
                    return m_a * (m_f - 1) / m_f;
                else
                    throw new Exception("δ����������Ϣ");
           }
        }
        
        /// <summary>
        /// ��ȡ�����ʰ뾶
        /// </summary>
        public double c
        {
            get
            {
                if (!isNull)
                    return m_a * m_f / (m_f - 1);
                else
                    throw new Exception("δ����������Ϣ");
            }
        }

        /// <summary>
        /// ��ȡ��һƫ����
        /// </summary>
        public double e
        {
            get
            {
                if (!isNull)
                    return Math.Sqrt(2 * m_f - 1) / m_f;
                else
                    throw new Exception("δ����������Ϣ");
           }
        }

        /// <summary>
        /// ��ȡ�ڶ�ƫ����
        /// </summary>
        public double e2
        {
            get
            {
                if (!isNull)
                    return Math.Sqrt(m_f * 2 - 1) / (m_f - 1);
                else
                    throw new Exception("δ����������Ϣ");
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
