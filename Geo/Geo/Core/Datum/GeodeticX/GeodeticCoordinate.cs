using System;

namespace GeodeticX
{
	/// <summary>
	/// ��Ĵ������
	/// </summary>
	public class GeodeticCoordinate
	{
        /// <summary>
        /// ���� (-180��180)
        /// </summary>
        private double longitude;

        /// <summary>
        /// γ�� (��90��90)
        /// </summary>
        private double latitude;

        /// <summary>
        /// ��ظ�
        /// </summary>
        private double geoidHeight;

        /// <summary>
        /// �Ƿ��趨������ֵ�ı��
        /// </summary>
        private bool blIsNull;
        private bool hIsNull;

        public GeodeticCoordinate()
        {
            blIsNull = true;
            hIsNull = true;
        }

        /// <summary>
        /// ������ֵ�ĳ�ʼ������
        /// </summary>
        /// <param name="B">γ��</param>
        /// <param name="L">����</param>
        public GeodeticCoordinate(double B, double L)
        {
            bool bIsNull = true, lIsNull = true;

            //�ж�γ�ȵ�ֵ��Χ
            if (Math.Abs(B) <= 90)
            {
                latitude = B;
                bIsNull = false;
            }
            else
                throw new Exception("γ��ֵ���ޣ�Ӧ����-90��90֮��");

            //�жϾ��ȵ�ֵ��Χ
            if (Math.Abs(L) <= 180)
            {
                longitude = L;
                lIsNull = false;
            }
            else
                throw new Exception("����ֵ���ޣ�Ӧ����-180��180֮��");

            if (!bIsNull && !lIsNull) 
                blIsNull = false;
            hIsNull = true;
        }

        /// <summary>
        /// ������ֵ�ĳ�ʼ������
        /// </summary>
        /// <param name="B">γ��</param>
        /// <param name="L">����</param>
        /// <param name="H">��ظ�</param>
        public GeodeticCoordinate(double B, double L, double H)
            : this(B, L)
        {
            geoidHeight = H;
            hIsNull = false;
        }

        /// <summary>
        /// ��ȡ/����γ��
        /// </summary>
        public double B
        {
            get
            {
                if (!blIsNull)
                    return latitude;
                else
                    throw new Exception("γ��Ϊ��");
            }
        }

        /// <summary>
        /// ��ȡ/���þ���
        /// </summary>
        public double L
        {
            get
            {
                if (!blIsNull)
                    return longitude;
                else
                    throw new Exception("����Ϊ��");
            }
        }

        /// <summary>
        /// ��ȡ/���ô�ظ�
        /// </summary>
        public double H
        {
            get
            {
                if (!hIsNull)
                    return geoidHeight;
                else
                    throw new Exception("��ظ�Ϊ��");
            }
            set
            {
                geoidHeight = value;
                hIsNull = false;
            }
        }

        public void SetCoordinate(double B, double L)
        {
            bool bIsNull = true, lIsNull = true;

            //�ж�γ�ȵ�ֵ��Χ
            if (Math.Abs(B) > 90)
            {
                latitude = B;
                bIsNull = false;
            }
            else
                throw new Exception("γ��ֵ���ޣ�Ӧ����-90��90֮��");

            //�жϾ��ȵ�ֵ��Χ
            if (Math.Abs(L) > 180)
            {
                longitude = L;
                lIsNull = false;
            }
            else
                throw new Exception("����ֵ���ޣ�Ӧ����-180��180֮��");

            if (!bIsNull && !lIsNull)
                blIsNull = false;
            hIsNull = true;
        }

        public void SetCoordinate(double B, double L, double H)
        {
            SetCoordinate(B, L);

            geoidHeight = H;
            hIsNull = false;
        }

        public override string ToString()
        {
            return string.Empty;// this.ToString("DMS");
        }
	}
}
