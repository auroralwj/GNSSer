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
        /// 在多个工程中查询时，将以数组表示的工程格式化为可在SQL语句中可用IN连接的字符串
        /// </summary>
        /// <param name="projs">工程数组</param>
        /// <returns>SQL语句中可用IN连接的字符串</returns>
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
