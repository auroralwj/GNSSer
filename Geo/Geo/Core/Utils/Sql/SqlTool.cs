using System;
using System.Collections.Generic;
using System.Text;

namespace Geodesy
{
    public class SqlUtils
    {
        public static string StringInSql(string str)
        {
            if (str == "" || str == null)
                return "NULL";
            else
                return "'" + str + "'";
        }

        public static string DataInSql(string data)
        {
            if (data == "" || data == null)
                return "NULL";
            else
                return data;
        }

        public static string like(string data)
        {
            if (data == "" || data == null)
                return "NULL";
            else
                return "'%" + data + "%'";
        }

        /// <summary>
        /// �ڶ�������в�ѯʱ�����������ʾ�Ĺ��̸�ʽ��Ϊ����SQL����п���IN���ӵ��ַ���
        /// </summary>
        /// <param name="projs">��������</param>
        /// <returns>SQL����п���IN���ӵ��ַ���</returns>
        public static string SQLFormatProjects(List<string> projs)
        {
            string str = "(";
            foreach (string proj in projs)
            {
                str += "'" + proj + "',";
            }
            return str.Substring(0, str.Length - 1) + ")";
        }
    }
}
