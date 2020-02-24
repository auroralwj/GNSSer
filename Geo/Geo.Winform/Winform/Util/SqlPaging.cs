using System;
using System.Collections.Generic;
using System.Text;

namespace Geodesy.Winform.Utils.SQL
{
    public class SqlPaging
    {
        private string id = "Id";
        private int pageSize = 20;
        private int pageNum = 1;
        private string tableName;
        private string fileds = "*";
        private string queryCondition;



        //以下参数计算得来。
        private int allRowCount; //表中记录总数

        private int allPageCount;



        public SqlPaging(String tableName, int pageSize, string id, string fileds, string queryCondition)
        {
            this.pageSize = pageSize;
            this.tableName = tableName;
            this.id = id;
            this.fileds = fileds;
            this.queryCondition = queryCondition;

            InitParams();
        }
        public SqlPaging(String tableName)
        {
            this.tableName = tableName;
            InitParams();
        }
        public SqlPaging(String tableName, int pageSize, string queryCondition)
        {
            this.pageSize = pageSize;
            this.tableName = tableName;
            this.queryCondition = queryCondition;


            InitParams();
        }

        private void InitParams()
        {
           // this.allRowCount = SqlTableUtil.GetCount( tableName, queryCondition );
            int div = allRowCount / pageSize;
            if (allRowCount % pageSize == 0)
                this.allPageCount = div;
            else
                this.allPageCount = div + 1;
        }

        public void Refresh() { InitParams(); }
        public int AllPageCount
        {
            get { return allPageCount; }
        }

        public int AllRowCount
        {
            get { return allRowCount; }
            set { allRowCount = value; }
        }
        public string QueryCondition
        {
            get { return queryCondition; }
            set { queryCondition = value; }
        }
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; InitParams(); }
        }
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public int PageNum
        {
            get { return pageNum; }
            set
            {
                if (value > allPageCount) pageNum = allPageCount;
                else if (value < 1) pageNum = 1;
                else pageNum = value;
            }
        }

        public string GetFirstSql() { return GetSql(1); }
        public string GetLastSql() { return GetSql(allPageCount); }
        public string GetPrevSql() { return GetSql(pageNum - 1); }
        public string GetNextSql() { return GetSql(pageNum + 1); }

        public string GetSql(int pageNum)
        {
            this.PageNum = pageNum;
            return GetSql();
        }

        public string GetSql()
        {
            if (pageNum < 1) pageNum = 1;

            string sql = "SELECT TOP " + pageSize + " " + fileds + " FROM " + tableName + " WHERE ";
            sql += "(" + id + " >= ";
            sql += "(SELECT MAX(" + id + ") FROM (SELECT TOP " + (pageSize * (pageNum - 1) + 1) + " " + id + " FROM " + tableName;
            if (queryCondition != null) sql += " WHERE " + queryCondition;
            sql += " ORDER BY " + id + " ) AS T) ";
            if (queryCondition != null) sql += " AND " + queryCondition;
            sql += ") ";
            sql += "ORDER BY  " + id;
            return sql;
        }


    }
}
