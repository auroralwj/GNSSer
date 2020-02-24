using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data; 
using System.Xml;
using System.Text; 
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Configuration; 
using System.Collections.Specialized;
using System.Collections;

namespace Geo.Utils
{
    /// <summary>
    /// 数据库连接和操作的实用类。
    /// </summary>
    public class SqlUtils
    {
        /// <summary>
        /// 数据库连接字符串。
        /// </summary>
        public static string ConnectString ="必须初始化我后才能使用啊！！";
         
        /// <summary>
        /// get a data table with one cloum eaualing to the indicated value.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string tableName, string column, string value)
        {
            string sql = "SELECT *  FROM " + tableName + "  Where " + column + "=" + SqlUtils.StringInSql(value);
            DataTable table = GetDataTable(sql);
            return table;
        }
        /// <summary>
        /// get a data table by a sql string. ConnectString must be initlized previously.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql)
        {
            DataTable table = new DataTable();
            using (SqlConnection cn = new SqlConnection(GetConnStr()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, cn);
                cn.Open();
                adapter.Fill(table);
            }
            return table;
        }
        /// <summary>
        /// Get a datatable by sql string.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectString"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, string connectString)
        {
            DataTable table = new DataTable();
            using (SqlConnection cn = new SqlConnection(connectString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, cn);
                cn.Open();
                adapter.Fill(table);
            }
            return table;
        }
        /// <summary>
        /// get one row in a table by indicate it's index.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static DataRow GetDataRow(string tableName, int index)
        {
            string sql = "SELECT Top 1 *  FROM " + tableName + " WHERE 编号=" + index;
            DataTable table = GetDataTable(sql);
            if (table.Rows.Count == 0) return null;
            DataRow dataRow = table.Rows[0];
            return dataRow;
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int GetCount(string tableName)
        {
            string sql = "SELECT Count(*)  FROM " + tableName;
            DataTable table = GetDataTable(sql);
            if (table.Rows.Count == 0) return 0;
            return int.Parse(table.Rows[0][0].ToString());
        }
        public static int GetCount(string tableName, string condition)
        {
            if (condition == null) return GetCount(tableName);
            string sql = "SELECT Count(*)  FROM " + tableName + " WHERE " + condition;
            DataTable table = GetDataTable(sql);
            if (table.Rows.Count == 0) return 0;
            return int.Parse(table.Rows[0][0].ToString());
        }

        /// <summary>
        /// get the prevObj row with the column equaling the value
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="cloumnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataRow GetDataRow(string tableName, string cloumnName, string value)
        {
            string sql = "SELECT Top 1 *  FROM " + tableName + " WHERE " + cloumnName + "=" + SqlUtils.StringInSql(value);
            DataTable table = GetDataTable(sql);
            DataRow dataRow = table.Rows[0];
            return dataRow;
        }

        #region 数据库删除操作

        /// <summary>
        /// 删除表所有行
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool DeleteAllRow(string tableName)
        {
            string sqlDelete = "DELETE " + tableName;
            return  ExecuteNonQuery(sqlDelete); 
        }

        public static bool ExecuteNonQuery(string sql)
        {
            using (SqlConnection cn = new SqlConnection(GetConnStr()))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch// (SqlException se)
                {
                    //MessageBox.Show(se.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 数据库连接
        /*-----------------------------------------------------------------
         * For the database connection.
         * ---------------------------------------------------------------*/
        /// <summary>
        /// return a ConnectionString object for connecting to a database.
        /// </summary>
        /// <returns></returns>
        public static string GetConnStr()
        {
            //创建数据库连接对象
            return "";// Geo.Winform.Setting.LoginInfo.GetConnectionString();
            // return @"Data Source=.\SQLEXPRESS;Initial Catalog=StockData;Integrated Security=True";
            //  return connStr;
            //   return ConstructCnStr(@"GEOART\SQLEXPRESS", "Geo2010", "GeoUser2007", "gdser_123456");
             //   return ConstructCnStr(".", "Geo2010", "sa", "adminadmin");
            // return ConstructCnStr(@"192.168.11.1", "Geo2010", "sa", "adminadmin");
            // return ConstructCnStr(@".\SQLEXPRESS", "StockData", "GeoUser2007", "gdser_123456");

            // return Setting.ConnectionString;//和Geodata相同了。
            //  return getRegistryConnStr();

            //if (Setting.DatabaseServer == null)
            //{
            //    Setting.TryReadConfigFromXml();
            //}          
             //   return ConstructCnStr(Setting.DbInfo.Server, Setting.DbInfo.DbName, Setting.DbInfo.User, Setting.DbInfo.Pass);
            //throw new Exception("To be done.");
        }

        /// <summary>
        /// construct a ConnectionString with parameters.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="db"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string ConstructCnStr(string server, string db, string user, string password)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.PersistSecurityInfo = false;
            builder.IntegratedSecurity = false;
            builder.DataSource = server;
            builder.InitialCatalog = db;
            builder.UserID = user;
            builder.Password = password;
            builder.ConnectTimeout = 0;

            return builder.ConnectionString;
        }
        #endregion

        public static string StringInSql(bool boolean)
        {
            return StringInSql(boolean.ToString());

        }
        public static string StringInSql(string str)
        {
            if (str == "" || str == null)
                return "NULL";
            else
                return "'" + str + "'";
        }

        public static string DataInSql(int data)
        {
            return DataInSql(data.ToString());
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
        public static bool Login(string userName, string password, string userColName, string passColName, string canLoginColName, string tableName)
        {
            //去除敏感符号
            userName = userName.Replace('=', ' ');
            password = password.Replace('=', ' ');
            using (SqlConnection cn = new SqlConnection(SqlUtils.GetConnString()))
            {
                cn.Open();
                string sql = "select * from "
                    + "[" + tableName.Trim() + "]"
                    + " where "
                    + canLoginColName + " = 'True' and "
                    + passColName + " = '" + password + "' and "
                    + userColName + " = '" + userName + "'";
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = sql;
                SqlDataReader reader = cmd.ExecuteReader();
                //reader.Read();
                return reader.HasRows;
            }
        }
        
        public static string[] GetTableColumNames(string tableName)
        {
            string sql = "select name from sys.columns where object_id = object_id(N'" + tableName + "')";
            DataTable table = GetDataTable(sql);
            string[] cols = new string[table.Rows.Count];
            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                cols[i++] = row[0].ToString();
            }
            return cols;
        }
        public static List<String> GetColNames(string tableName)
        {
            string sql = "select name  from sys.columns where object_id = object_id(N'" + tableName + "')";
            DataTable table = SqlUtils.GetDataTable(sql);
            List<String> items = new List<String>();
            foreach (DataRow row in table.Rows)
            {
                string title = row["name"].ToString();
                items.Add(title);
            }
            return items;
        }

        public static List<String> GetTableNames()
        {
            string sql = "select name  from sys.tables";
            DataTable table = SqlUtils.GetDataTable(sql);
            List<String> items = new List<String>();
            foreach (DataRow row in table.Rows)
            {
                string title = row["name"].ToString();
                items.Add(title);
            }
            return items;
        }

        #region Basic SQL Manipulate

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中的第一列。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<string> GetList(string sql)
        {
            DataTable table = GetDataTable(sql);
            List<string> list = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(row[0].ToString());
            }
            return list;
        }
        public static bool HasRows(string tableName) { return GetCount(tableName) != 0; }


        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static Object ExecuteScalar(string sql)
        {
            object obj = null;
            using (SqlConnection cn = new SqlConnection(SqlUtils.GetConnString()))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = sql;
               obj = cmd.ExecuteScalar(); 
            }
            return obj;
        }

        /// <summary>
        /// 利用 SQL 事务依次执行SQL语句。
        /// </summary>
        /// <param name="sqls">待执行的SQL语句</param>
        /// <returns>成功则True，否则为False</returns>
        public static bool ExecuteWithTransanction(List<String> sqls)
        {
            using (SqlConnection cn = new SqlConnection(SqlUtils.GetConnString()))
            {
                cn.Open();

                // Start a local transaction.
                SqlTransaction sqlTran = cn.BeginTransaction();

                // Enlist the command in the currentItem transaction.
                SqlCommand command = cn.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    foreach (String sql in sqls)
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
                catch //(Exception ex)
                {
                    sqlTran.Rollback();
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 数据库连接配置
        /*-----------------------------------------------------------------
         * For the database connection.
         * ---------------------------------------------------------------*/
        /// <summary>
        /// return a ConnectionString object for connecting to a database.
        /// </summary>
        /// <returns></returns>
        public static string GetConnString()
        {
            return ConnectString;
            //创建数据库连接对象
            //return "Data Source=.\\SQLEXPRESS;Initial Catalog=Geo2010;Integrated Security=True";
             //   return "Data Source=.;Initial Catalog=Geo2010;Integrated Security=True";
             // return ConstructCnStr(@".", "Geo2010", "GeoUser2007", "gdser_123456"); 
        }

        /// <summary>
        /// construct a ConnectionString with parameters.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="db"></param>
        /// <param name="currentItem"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetConnString(string server, string db, string user, string password)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.PersistSecurityInfo = false;
            builder.IntegratedSecurity = false;
            builder.DataSource = server;
            builder.InitialCatalog = db;
            builder.UserID = user;
            builder.Password = password;
            // builder.ConnectTimeout = 0;

            return builder.ConnectionString;
        }
        public static string GetRegistryConnStr()
        {
            RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey("software");
            RegistryKey geodataKey = softwareKey.OpenSubKey("Geodesy");
            if (geodataKey == null) return null;
            RegistryKey databaseKey = geodataKey.OpenSubKey("database");
            if (databaseKey == null) return null;
            else
            {
                string server = databaseKey.GetValue("server").ToString();
                string dbName = databaseKey.GetValue("dbName").ToString();
                string userName = "GeoUser2007";// databaseKey.GetValue("userName").ToString();
                string password = "gdser_123456";// databaseKey.GetValue("password").ToString();

                string cnStr = GetConnString(server, dbName, userName, password);

                return cnStr;
            }
        }
        #endregion

        #region formate object to sql string
        public static string GetSqlString(DateTime time) { return GetSqlString(time.ToString("yyyy-MM-dd HH:mm:ss")); }
        public static string GetSqlString(string str) { return "'" + str + "'"; }
        public static string GetSqlString(bool boo) { return "'" + boo.ToString() + "'"; }
        public static string GetSqlString(int str) { return str.ToString(); }
        public static string GetSqlString(double str) { return str.ToString(); }
        #endregion
    }

     
}
