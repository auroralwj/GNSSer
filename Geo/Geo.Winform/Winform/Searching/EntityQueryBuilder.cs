//2015.06.15, czs, create in namu, 封装实体查询

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{ 
    /// <summary>
    /// 实体查询管理器。
    /// </summary>
    public class EntityQueryBuilder : AbstractBuilder<QueryItem>
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="IsNeedResetParamOrder"></param>
        /// <param name="IsClearQueryAfterBuild"></param>
        public EntityQueryBuilder(bool IsNeedResetParamOrder = true) { 
            this.Queries = new List<QueryItem>();
            this.IsNeedResetParamOrder = IsNeedResetParamOrder; 
        }

        #region 常用属性
        /// <summary>
        /// 是否由程序组织参数排序。
        /// 从 0 开始往后排。如果为 false，则不设置。
        /// </summary>
        public bool IsNeedResetParamOrder { get; set; }
        /// <summary>
        /// 当前查询是否和上一查询条件相同（只是字面上比较）。
        /// </summary>
        public bool IsSameAsPrevious { get { return (this.PrevQuery != null) && this.PrevQuery.Equals(BuildPreView); } }

        /// <summary>
        /// 初始限制查询。后继查询在此限制基础上进行。通常用于权限控制。
        /// </summary>
        public QueryItem InitialQuery { get; set; }
        /// <summary> 
        /// 上一查询，用以存储上一个（当前生成前的）查询结果，若要继续参与查询，请手动添加到查询列表中。
        /// </summary>
        public QueryItem PrevQuery { get; set; } 

        /// <summary>
        /// 当前查询
        /// </summary>
        public List<QueryItem> Queries { get; set; }

        #region 扩展属性
        /// <summary>
        /// 指示是否可以查询。
        /// </summary>
        public bool CanQuery { get { return HasInitialQuery || HasQuery; } }

        /// <summary>
        /// 是否具有查询，或初始查询是否被清空。
        /// </summary>
        public bool HasQuery { get { return Queries != null && Queries.Count > 0; } }
        /// <summary>
        /// 是否具有初始查询，或初始查询是否被清空。
        /// </summary>
        public bool HasInitialQuery { get { return InitialQuery != null; } }
        /// <summary>
        /// 是否具有上一查询，或上一查询是否被清空。
        /// </summary>
        public bool HasPrevQuery { get { return PrevQuery != null; } }
        #endregion

        #endregion

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public EntityQueryBuilder AddQuery(QueryItem Query)
        {
            this.Queries.Add(Query);
            return this;
        }

        /// <summary>
        /// 将上一查询设置为null。不影响外在引用。
        /// </summary>
        public EntityQueryBuilder ClearPrevQuery() { PrevQuery = null; return this; }
        /// <summary>
        /// 清空初始查询，将查询设置为null。不影响外在引用。
        /// </summary>
        public EntityQueryBuilder ClearInitialQuery() { InitialQuery = null; return this; }
        /// <summary>
        /// 用于查看最后的生成结果，但是不会设置 PrevQuery 属性。
        /// </summary>
        public QueryItem BuildPreView {get{ return this.Produce();   }   }

        /// <summary>
        /// 构造查询条件，并设置为上一查询
        /// </summary>
        /// <returns></returns>
        public override QueryItem Build()
        {
            var q = Produce();
            this.PrevQuery = q;
            return q;
        }

        /// <summary>
        /// 构造查询条件
        /// </summary>
        /// <returns></returns>
        private QueryItem Produce()
        {
            StringBuilder sb = new StringBuilder();
            List<object> parameters = new List<object>();
            if (HasInitialQuery)
            {
                //bracket
                var str = InitialQuery.Condition.Trim();
                str = Geo.Utils.StringUtil.Bracket(str); 
                sb.Append(str);

                if (InitialQuery.Parameters != null)
                { parameters.AddRange(InitialQuery.Parameters); }
            }
             
            if (HasQuery)
            {
                foreach (var Query in Queries)
                {
                    sb.Append(Query.GetCondition(parameters.Count > 0 || sb.Length > 0));
                    if (Query.Parameters != null)
                    { parameters.AddRange(Query.Parameters); }
                }
            }
            var finalCondition = sb.ToString();
            //将参数重新排， 从 0 往后
            if (IsNeedResetParamOrder && parameters.Count > 1)
            {
                finalCondition = ResetParamOrder(finalCondition);
            }
             
            var q = new QueryItem(finalCondition, parameters); 
            return q;
        }
 

        /// <summary>
        /// 重新参数排序。如 将 @0 @2 @1 替换为 @0 @1 @2
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string ResetParamOrder(string condition)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            bool isInNumber = false;
            //确保最后为空格，否则不易判断。
            if (!condition.EndsWith(" "))
            {
               condition = condition + " "; 
            }
            foreach (var item in condition)
            {
                if (item == '@' || isInNumber)
                {
                    isInNumber = true;

                    if (item == ' ' || item == ')' )
                    {
                        sb.Append(i++);
                        sb.Append(item);   
                        isInNumber = false;
                    }

                    if (item == '@')
                    {
                        sb.Append(item);   
                    }

                    continue;
                }

                sb.Append(item);                
            }
             
            return sb.ToString();
        }

        /// <summary>
        /// 清除当前查询条件列表，只保留最初查询（如果有）。
        /// </summary>
        public EntityQueryBuilder ClearQuery()
        {
            this.Queries = new List<QueryItem>();
            return this; 
        }

        #region override
        public override string ToString()
        {
            return this.BuildPreView.ToString();
        }

        public override bool Equals(object obj)
        {
            EntityQueryBuilder o = obj as EntityQueryBuilder;
            if (o == null) { return false; }
            return BuildPreView.Equals(o.BuildPreView);
        }
        public override int GetHashCode()
        {
            return BuildPreView.GetHashCode();
        }
        #endregion
    }

    /// <summary>
    /// 查询项目s
    /// </summary>
    public class QueryItem
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Condition"></param>
        /// <param name="Params"></param>
        public QueryItem(string Condition, object Params)
            : this(Condition, new List<object>() { Params})
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Condition"></param>
        /// <param name="Params"></param>
        public QueryItem(string Condition, List<object> Params = null)
        {
            this.Connection = " and ";
            this.Condition = Condition;
            this.Parameters = Params;
        }

        /// <summary>
        /// 查询条件字符串，如 Id == @0
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// 查询参数
        /// </summary>
        public List<object> Parameters { get; set; }
        /// <summary>
        /// 参数数量
        /// </summary>
        public int ParamCount { get { return Parameters.Count; } }

        /// <summary>
        /// 连接词，为 and 或 or。
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 连接好的字符串。若是多个条件则使用。
        /// </summary>
        public string ConnectedCondition { get { return Connection + " " + Condition; } }
        /// <summary>
        /// 获取查询条件。
        /// </summary>
        /// <param name="conndected"></param>
        /// <returns></returns>
        public string GetCondition(bool conndected = false)
        {
            if (conndected) return ConnectedCondition;
            return Condition;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ConnectedCondition + " ");

            foreach (var item in Parameters)
            {
                sb.Append(",");
                sb.Append(item);
            }

            return sb.ToString();
        }
        /// <summary>
        /// 只能简单的从字面上判断。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            QueryItem o = obj as QueryItem;
            if (o == null) { return false; } 

            return o.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        /// <summary>
        /// Id比0小的集合，意思是返回所有结果
        /// </summary>
        public static QueryItem IdSmallerThanZero { get { return new QueryItem("Id<0"); } }
        public static QueryItem IdLargerThanZero { get { return new QueryItem("Id>0"); } }
    }
}
