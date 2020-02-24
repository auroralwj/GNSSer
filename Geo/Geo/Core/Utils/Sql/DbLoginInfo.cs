using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    /// 数据库登录信息。
    /// </summary>
    public class DbLoginInfo
    {
        /// <summary>
        /// 数据库服务器地址
        /// </summary>
        public string ServerAddress { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DababaseName { get; set; }
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string LoginUser { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPass { get; set; }

        /// <summary>
        /// 克隆一个
        /// </summary>
        /// <returns></returns>
        public DbLoginInfo Clone()
        {
            DbLoginInfo l = new DbLoginInfo()
            {
                DababaseName = this.DababaseName,
                LoginPass = this.LoginPass,
                LoginUser = this.LoginUser,
                ServerAddress = this.ServerAddress
            };
            return l;
        }

        //Data Source=.\SqlExpress;Initial Catalog=Geo2015;Persist Security Info=True;User ID=sa;Password=***********
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return "Data Source=" + ServerAddress
                + ";Initial Catalog=" + DababaseName
                + ";Integrated Security=True"
                + ";User ID=" + this.LoginUser
                + ";Password=" + this.LoginPass    
            ;
        }
        public override bool Equals(object obj)
        {
            var o = obj as DbLoginInfo;
            if (o == null) return false;

            return GetConnectionString().Equals(o.GetConnectionString());
        }
        public override int GetHashCode()
        {
            return GetConnectionString().GetHashCode();
        } 
    }


}
