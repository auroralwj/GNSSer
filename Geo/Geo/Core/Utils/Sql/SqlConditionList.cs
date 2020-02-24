using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


public enum SqlLogicOper
{
    AND_LOGIC,     // AND
    OR_LOGIC,      // OR
}
public enum SqlCompareOper
{
    MORE_THAN,     // >
    LESS_THAN,     // <
    NOT_MORE_THAN, // <=
    NOT_LESS_THAN, // >=
    EQUAL,         // =
    NOT_EQUAL,     // <>
    LIKE,          // LIKE
    NOT_LIKE,      // NOT LIKE
    IN,            // IN
    BETWEEN,       // BETWEEN
}

/// <summary>
/// SQL条件集合，
/// 负责处理各个条件的合并和最终SQL的生成。
/// </summary>
public class SqlConditionList
{
    // 缓存所有条件
    private ArrayList _condList = new ArrayList();

    public SqlConditionList()
    {
    }
    /// <summary>
    /// 添加一个条件
    /// </summary>
    /// <param name="cond">条件</param>
    public SqlConditionList Add(SqlCondition cond)
    {
        _condList.Add(cond);
        return this;
    }

    /// <summary>
    /// 添加一个条件
    /// </summary>
    /// <param name="condition">条件</param>
    /// <param name="logicOper">与前一个条件的关系</param>
    public SqlConditionList AddLogic(SqlCondition condition,
        SqlLogicOper logicOper)
    {
        SqlCondition cond = null;
        if (_condList.Count > 0)
        {
            SqlCondition lastCond = (SqlCondition)

                         _condList[_condList.Count - 1];
            cond = Merge(lastCond, condition, logicOper);
        }
        else
        {
            cond = condition;
        }

        //remove all exit condition
        this.Clear(cond);

        _condList.Add(cond);
        return this;
    }

    // 合并两个条件，合二为一。
    public static SqlCondition Merge(
        SqlCondition cond1, SqlCondition cond2,
        SqlLogicOper logicOper)
    {
        return new SqlConditionRelation(cond1, cond2, logicOper);
    }

    //生成SQL语句。
    public string ToSql()
    {
        StringBuilder buff = new StringBuilder();
        foreach (SqlCondition cond in _condList)
        {
            buff.Append(cond.ToSql());
        }
        return buff.ToString();
    }

    private void Clear(SqlCondition condition)
    {
        if (_condList.Contains(condition))
        {
            _condList.Remove(condition);
        }

        if (condition.GetType() == typeof(SqlConditionRelation))
        {
            SqlConditionRelation richCond =

                         (SqlConditionRelation)condition;
            this.Clear(richCond._cond1);
            this.Clear(richCond._cond2);
        }
    }


    /// <summary>
    /// SQL条件，也就是WHERE部分。
    /// </summary>
    public class SqlCondition
    {
        private static string[] _compareOpers = new string[] {
                " > ", " < ", " <= ", " >= ", " = ", " <> ",
                " LIKE ", " NOT LIKE ", " IN " , " BETWEEN "};

        private string _filedName;
        private object _value;
        private SqlCompareOper _compareOper;
       // private string _templateName;

        protected SqlCondition()
        {
        }

        public SqlCondition(SqlCompareOper compareOper,
            string fieldName, object value)
        {
            _compareOper = compareOper;
            _filedName = fieldName;
            _value = value;
        }

        // 生成条件的SQL
        public virtual string ToSql()
        {
            if (_value == null)
            {
                throw new Exception(
                    "Can not parse SQL because value is null.");
            }
            StringBuilder buff = new StringBuilder();
            if (_compareOper == SqlCompareOper.IN)
            {
                if (!_value.GetType().IsSubclassOf(typeof(Array)))
                {
                    throw new Exception("Can not parse [IN].");
                }
                buff.Append(_filedName);
                buff.Append(" IN (");

                Array arrVal = (Array)_value;
                foreach (object val in arrVal)
                {
                    buff.Append(this.ToValueSqlString(val))

                        .Append(",");
                }
                buff.Remove(buff.Length - 1, 1);
                buff.Append(") ");
            }
            else if (_compareOper == SqlCompareOper.BETWEEN)
            {
                if (!_value.GetType().IsSubclassOf(typeof(Array)))
                {
                    throw new Exception("Can not parse [BETWEEN].");
                }
                Array arrVal = (Array)_value;
                if (arrVal.Length != 2)
                {
                    throw new Exception("Can not parse [BETWEEN].");
                }
                buff.Append(" (");
                buff.Append(_filedName);
                buff.Append(" BETWEEN ");

                buff.Append(arrVal.GetValue(0));
                buff.Append(" AND ");
                buff.Append(arrVal.GetValue(1));
                buff.Append(") ");
            }
            else
            {
                buff.Append(_filedName);
                buff.Append(_compareOpers[(int)_compareOper]);
                buff.Append(this.ToValueSqlString(_value));
            }
            return buff.ToString();
        }

        private string ToValueSqlString(object value)
        {
            StringBuilder buff = new StringBuilder();
            if (value.GetType() == typeof(Int16)
                || value.GetType() == typeof(Int32)
                || value.GetType() == typeof(Int64)
                || value.GetType() == typeof(Decimal)
                || value.GetType() == typeof(Single)
                || value.GetType() == typeof(Double))
            {
                buff.Append(value);
            }
            else if (value.GetType() == typeof(Boolean))
            {
                if ((bool)value)
                {
                    buff.Append(1);
                }
                else
                {
                    buff.Append(0);
                }
            }
            else if (value.GetType() == typeof(DateTime))
            {
                string dValue = ((DateTime)value).
                    ToString("yyyy-MM-dd hh:mm:ss.fff");
                buff.Append("'").Append(dValue).Append("'");
            }
            else
            {
                string sValue = value.ToString().Replace("'", "''");
                buff.Append("'").Append(sValue).Append("'");
            }
            return buff.ToString();
        }
    }


    /// <summary>
    /// 两个条件合并后的条件。
    /// </summary>
    class SqlConditionRelation : SqlCondition
    {
        private static string[] _logicOpers = new string[] {
            " AND ", " OR " };
        internal SqlLogicOper _logicOper;
        internal SqlCondition _cond1;
        internal SqlCondition _cond2;

        internal SqlConditionRelation(SqlCondition cond1,
            SqlCondition cond2, SqlLogicOper logicOper)
        {
            _cond1 = cond1;
            _cond2 = cond2;
            _logicOper = logicOper;
        }

        public override string ToSql()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append(_cond1.ToSql())
                .Append(_logicOpers[(int)_logicOper])
                .Append(_cond2.ToSql());
            if (_logicOper == SqlLogicOper.OR_LOGIC)
            {
                buff.Insert(0, " (");
                buff.Append(") ");
            }
            return buff.ToString();
        }

    }
}
