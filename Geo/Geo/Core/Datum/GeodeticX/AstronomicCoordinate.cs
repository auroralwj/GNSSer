using System;

namespace GeodeticX
{
    /// <summary>
    /// ��������
    /// </summary>
    public class AstronomicCoordinate
    {
        /// <summary>
        /// ����γ�� (��90��90)
        /// </summary>
        private double latitude;
        /// <summary>
        /// ���ľ��� (-180��180)
        /// </summary>
        private double longitude;
        /// <summary>
        /// ���ķ�λ��
        /// </summary>
        private double azimuth;

        /// <summary>
        /// �Ƿ��趨������ֵ�ı��
        /// </summary>
        private bool blIsNull;
        private bool aIsNull;

        public AstronomicCoordinate()
        {
            blIsNull = true;
            aIsNull = true;
        }

        /// <summary>
        /// ������ֵ�ĳ�ʼ������
        /// </summary>
        /// <param name="��">���ľ���</param>
        /// <param name="��">����γ��</param>
        public AstronomicCoordinate(double ��, double ��)
        {
            bool bIsNull = true, lIsNull = true;

            //�ж�γ�ȵ�ֵ��Χ
            if (�� >= -180 && �� <= 180)
            {
                longitude = ��;
                lIsNull = false;
            }
            else
                throw new Exception("���ľ���ֵ���ޣ�Ӧ����-180��180֮��");

            //�жϾ��ȵ�ֵ��Χ
            if (�� >= -90 && �� <= 90)
            {
                latitude = ��;
                bIsNull = false;
            }
            else
                throw new Exception("����γ��ֵ���ޣ�Ӧ����-90��90֮��");

            if (!bIsNull && !lIsNull) 
                blIsNull = false;
            aIsNull = true;
        }

        /// <summary>
        /// ������ֵ�ĳ�ʼ������
        /// </summary>
        /// <param name="��">γ��</param>
        /// <param name="��">����</param>
        /// <param name="A">��ط�λ��</param>
        public AstronomicCoordinate(double ��, double ��, double A)
            : this(��, ��)
        {
            if (A >= 0 && A < 360)
            {
                azimuth = A;
                aIsNull = false;
            }
            else
                throw new Exception("���ķ�λ�ǳ��ޣ�Ӧ����0��360��֮��");
        }

        /// <summary>
        /// ��ȡ/�������ľ���
        /// </summary>
        public double ��
        {
            get
            {
                if (!blIsNull)
                    return longitude;
                else
                    throw new Exception("���ľ���Ϊ��");
            }
        }

        /// <summary>
        /// ��ȡ/��������γ��
        /// </summary>
        public double ��
        {
            get
            {
                if (!blIsNull)
                    return latitude;
                else
                    throw new Exception("����γ��Ϊ��");
            }
        }

        /// <summary>
        /// ��ȡ/���ô�ط�λ��
        /// </summary>
        public double A
        {
            get
            {
                if (!aIsNull)
                    return azimuth;
                else
                    throw new Exception("��ط�λ��Ϊ��");
            }
            set
            {
                if (value >= 0 && value < 360)
                {
                    azimuth = value;
                    aIsNull = false;
                }
                else
                    throw new Exception("���ķ�λ�ǳ��ޣ�Ӧ����0��360��֮��");
            }
        }

        public void SetCoordinate(double ��, double ��)
        {
            bool bIsNull = true, lIsNull = true;

            //�ж�γ�ȵ�ֵ��Χ
            if (�� >= -180 && �� <= 180)
            {
                longitude = ��;
                lIsNull = false;
            }
            else
                throw new Exception("���ľ���ֵ���ޣ�Ӧ����-180��180֮��");

            //�жϾ��ȵ�ֵ��Χ
            if (�� >= -90 && �� <= 90)
            {
                latitude = ��;
                bIsNull = false;
            }
            else
                throw new Exception("����γ��ֵ���ޣ�Ӧ����-90��90֮��");

            if (!bIsNull && !lIsNull)
                blIsNull = false;
            aIsNull = true;
        }

        public void SetCoordinate(double ��, double ��, double A)
        {
            SetCoordinate(��, ��);

            if (A >= 0 && A < 360)
            {
                azimuth = A;
                aIsNull = false;
            }
            else
                throw new Exception("���ķ�λ�ǳ��ޣ�Ӧ����0��360��֮��");
        }
    }
}
