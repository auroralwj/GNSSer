using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Utils
{
    /// <summary>
    /// �����쳣��飬ֱ���׳��쳣��
    /// </summary>
    public static class ExceptionCheckUtil
    {
        /// <summary>
        /// ����Ƿ��ڷ�Χ��(���߽�)�������׳������쳣��
        /// </summary>
        /// <param name="val">����ֵ</param>
        /// <param name="min">��Сֵ</param>
        /// <param name="max">���ֵ</param>
        /// <param name="valName">��������</param>
        public static void Scope(double val, double min, double max = Double.MaxValue, string valName = "valName")
        {
            if (val < min || val > max)
                throw new ArgumentException("�������ڸ�����Χ��:[" + min + "," + max + "]", valName);
        }
        /// <summary>
        /// �Ƿ�С��
        /// </summary>
        /// <param name="val">����ֵ</param>
        /// <param name="max">���ֵ</param>
        /// <param name="valName">��������</param>
        public static void Smaller(double val, double max, string valName = "valName")
        {
            if (val >= max)
                throw new ArgumentException("����Ӧ��С��: " + max, valName);
        }  
        
        /// <summary>
        /// �Ƿ�С�ڵ���
        /// </summary>
        /// <param name="val">����ֵ</param>
        /// <param name="max">���ֵ</param>
        /// <param name="valName">��������</param>
        public static void EqualOrSmaller(double val, double max, string valName = "valName")
        {
            if (val > max)
                throw new ArgumentException("����Ӧ��С�ڻ����: " + max, valName);
        }


        /// <summary>
        /// �Ƿ����
        /// </summary>
        /// <param name="val">����ֵ</param>
        /// <param name="min">��Сֵ</param>
        /// <param name="valName">��������</param>
        public static void Larger(double val, double min, string valName = "valName")
        {
            if (val >= min)
                throw new ArgumentException("����Ӧ�ô���: " + min, valName);
        }
        /// <summary>
        /// �Ƿ���ڻ����
        /// </summary>
        /// <param name="val">����ֵ</param>
        /// <param name="min">��Сֵ</param>
        /// <param name="valName">��������</param>
        public static void EqualOrLarger(double val, double min, string valName = "valName")
        {
            if (val > min)
                throw new ArgumentException("����Ӧ�ô��ڻ����: " + min, valName);
        }  
    
    }
}
