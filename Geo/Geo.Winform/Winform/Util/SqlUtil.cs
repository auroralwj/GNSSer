using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Configuration; 
using System.Collections.Specialized;

using System.Collections;

namespace Geo.Winform
{
    /// <summary>
    /// 数据库连接和操作的实用类。
    /// </summary>
    public class SqlUtil
    {
        public static bool Login(string userName, string password, string userColName, string passColName, string canLoginColName, string tableName)
        {
            //去除敏感符号
            userName = userName.Replace('=', ' ');
            password = password.Replace('=', ' ');
            using (SqlConnection cn = new SqlConnection(SqlUtil.GetConnString()))
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
            DataTable table = SqlUtil.GetDataTable(sql);
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
            DataTable table = SqlUtil.GetDataTable(sql);
            List<String> items = new List<String>();
            foreach (DataRow row in table.Rows)
            {
                string title = row["name"].ToString();
                items.Add(title);
            }
            return items;
        }

        #region Basic SQL Manipulate

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
        /// <summary>
        /// get a data table by a sql string.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql)
        {
            DataTable table = new DataTable();
            using (SqlConnection cn = new SqlConnection(SqlUtil.GetConnString()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, cn);
                cn.Open();
                adapter.Fill(table);
            }
            return table;
        }
        public static bool HasRows(string tableName)
        {
            return GetRowCount(tableName) != 0;
        }
        public static int GetRowCount(string tableName)
        {
            string sql = "select count(*) from [" + tableName.Trim() + "]";
            int count = int.Parse(SqlUtil.ExecuteScalar(sql).ToString());
            return count;
        }

        public static Object ExecuteScalar(string sql)
        {
            object obj = null;
            using (SqlConnection cn = new SqlConnection(SqlUtil.GetConnString()))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = sql;
                obj = cmd.ExecuteScalar();
            }
            return obj;
        }
        /// <summary>
        /// 执行不返回结果的SQL语句。
        /// </summary>
        /// <param name="queryString"></param>
        public static int ExecuteNonQuery(string sql)
        {
            using (SqlConnection connection = new SqlConnection(SqlUtil.GetConnString()))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 利用 SQL 事务依次执行SQL语句。
        /// </summary>
        /// <param name="sqls">待执行的SQL语句</param>
        /// <returns>成功则True，否则为False</returns>
        public static bool BeginTransAction(List<String> sqls)
        {
            using (SqlConnection cn = new SqlConnection(SqlUtil.GetConnString()))
            {
                cn.Open();

                // Start a local transaction.
                SqlTransaction sqlTran = cn.BeginTransaction();

                // Enlist the cmd in the currentItem transaction.
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
            //创建数据库连接对象
            //   return "Data Source=.\\SQLEXPRESS;Initial Catalog=Geo2010;Integrated Security=True";
            // return "Data Source=.;Initial Catalog=Geo2010;Integrated Security=True";
            //  return connStr;
            //  return ConstructCnStr(@".", "Geo2010", "GeoUser2007", "gdser_123456");
            Geo.Utils.DbLoginInfo loginInfo = Setting.LoginInfo;
            return GetConnString(loginInfo);
        }

        public static string GetConnString(Geo.Utils.DbLoginInfo loginInfo)
        {
            return GetConnString(loginInfo.ServerAddress, loginInfo.DababaseName, loginInfo.LoginUser, loginInfo.LoginPass);

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
            builder.PersistSecurityInfo = true;
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

        public static string GetSqlString(DateTime time) { return GetSqlString(time.ToString("yyyy-MM-dd HH:mm:ss")); }
        public static string GetSqlString(string str) { return "'" + str + "'"; }
        public static string GetSqlString(bool boo) { return "'" + boo.ToString() + "'"; }
        public static string GetSqlString(int str) { return str.ToString(); }
        public static string GetSqlString(double str) { return str.ToString(); }

        public static int GetMaxId(string tableName, string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "select top 1 from " + tableName + " order by Id desc";
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
        }

        public static int GetMaxId(string tableName, SqlCommand cmd)
        {
            cmd.CommandText = "select top 1 Id from " + tableName + " order by Id desc";
            return int.Parse(cmd.ExecuteScalar().ToString());
        }

    }
}