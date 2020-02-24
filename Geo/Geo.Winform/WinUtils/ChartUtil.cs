using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;



namespace Geo.Utils
{
    /// <summary>
    /// ���繤��
    /// </summary>
    public static class ChartUtil
    {
        static Geo.IO.ILog log = Geo.IO.Log.GetLog(typeof(ChartUtil));

        /// <summary>
        /// �Զ�����ʱ���ʽ��
        /// </summary> 
        /// <param name="ser"></param>
        public static string GetTimeLableFormat(Series ser)
        {
            double interval = 1;
            int count = ser.Points.Count;
            string Format = "HH:mm:ss";
            if (count > 1)
            {
                //The base OLE Automation Date is midnight, 30 December 1899. 
                //The minimum OLE Automation date is midnight, 1 January 0100. The maximum OLE Automation Date is the same as DateTime.MaxValue, the last moment of 31 December 9999.
                //DateTime.ToOADate ���� (),����Ϊ��λ
                interval = (ser.Points[count - 1].XValue - ser.Points[0].XValue) * 86400;//OA,ת��Ϊ��Ϊ��λ 
                if (interval < 60)//1����
                {
                    Format = "mm:ss";
                }
                else if (interval < 3600)//1Сʱ��
                {
                    Format = "HH:mm";
                }
                else if (interval < 24 * 3600)//1����
                {
                    Format = "HH:mm";
                }
                else if (interval < 30 * 24 * 3600)//��
                {
                    Format = "MM-dd";
                }
                else if (interval < 12 * 30 * 24 * 3600)//��
                {
                    Format = "yyyy-MM";
                }
                else//����
                {
                    Format = "yyyy";
                }
            }

            return Format;
        }


    }
}
