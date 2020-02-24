using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using System.Configuration;
using System.Configuration.Assemblies;
using System.Data.SqlClient;
using System.Xml;
using Microsoft.Win32;

//using Czs.Sql.SqlCondition;
//using Czs.Sql.Sys;

using System.Collections.Specialized;

namespace Geodesy.Winform.Utils
{
    /// <summary>
    /// 数据库连接和操作的实用类。
    /// </summary>
    public class SqlSysUtil
    {
        //public static List<DualisticItem> GetDualisticItems(string tableName)
        //{
        //       string sql = "select name , system_type_id from sys.columns where object_id = object_id(N'" + tableName + "')";
        //    DataTable table = GetDataTable(sql);
        //    List<DualisticItem> items = new List<DualisticItem>();
        //    foreach (DataRow row in table.Rows)
        //    {
        //        string title = row["name"].ToString();
        //        int typeId = int.Parse(row["system_type_id"].ToString());
        //        DataType type = SystemType.GetDataType(typeId);
        //        DualisticItem key = new DualisticItem(title, type);
        //        items.Add(key);
        //    }
        //    return items;
        //}


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
            using (SqlConnection cn = new SqlConnection(Winform.Utils.SqlSysUtil.GetConnString()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, cn);
                cn.Open();
                adapter.Fill(table);
            }
            return table;
        }

        /// <summary>
        /// 执行不返回结果的SQL语句。
        /// </summary>
        /// <param name="queryString"></param>
        public static int ExecuteNonQuery(string sql)
        {
            using (SqlConnection connection = new SqlConnection(Winform.Utils.SqlSysUtil.GetConnString()))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                return command.ExecuteNonQuery();
            }
        }
        public static Object ExecuteScalar(string sql)
        {
            using (SqlConnection connection = new SqlConnection(Winform.Utils.SqlSysUtil.GetConnString()))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// 利用 SQL 事务依次执行SQL语句。
        /// </summary>
        /// <param name="sqls">待执行的SQL语句</param>
        /// <returns>成功则True，否则为False</returns>
        public static bool BeginTransAction(List<String> sqls)
        {
            using (SqlConnection cn = new SqlConnection(Winform.Utils.SqlSysUtil.GetConnString()))
            {
                cn.Open();

                // Start a local transaction.
                SqlTransaction sqlTran = cn.BeginTransaction();

                // Enlist the cmd in the current transaction.
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
            //   return ConstructCnStr(@"GEOART\SQLEXPRESS", "Geo2010", "GeoUser2007", "gdser_123456");
            //    return ConstructCnStr(@"GEOART-CZS", "Geo2010", "sa", "adminadmin");
            // return ConstructCnStr(@"192.168.11.1", "Geo2010", "sa", "adminadmin");
            //  return ConstructCnStr(@".", "Geo2010", "GeoUser2007", "gdser_123456");

            // return Setting.ConnectionString;//和Geodata相同了。
            //return GetRegistryConnStr();

            return Geo.Winform.SqlUtil.GetConnString();       
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

                string cnStr = Winform.Utils.SqlSysUtil.ConstructCnStr(server, dbName, userName, password);

                return cnStr;
            }
        }
        #endregion

    }
}
