//2018.07.07, czs, create in HMX, 数值条件

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{
    /// <summary>
    /// 连接的条件,规则：所添加的都为顶层条件。
    /// 一个主条件。
    /// </summary>
    public class ConnectedStrCondition : BaseConnectedCondition<string>, IStringCondition
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConnectedStrCondition(StringCondition baseCondition):base( baseCondition)
        { 
        }  
        /// <summary>
        /// 增加条件
        /// </summary>
        /// <param name="connectedNumeralCondition"></param>
        /// <returns></returns>
        public ConnectedStrCondition AddConditon(ConnectedStringCondition connectedNumeralCondition)
        {
            Conditions.Add(connectedNumeralCondition);
            return this;
        }
        /// <summary>
        /// 增加条件
        /// </summary>
        /// <param name="connectOperator"></param>
        /// <param name="numeralCondition"></param>
        /// <returns></returns>
        public override BaseConnectedCondition<string> AddConditon(ConditionConnectOperator connectOperator, ICondition<string> numeralCondition)
        {
            return AddConditon(new ConnectedStringCondition(connectOperator, numeralCondition));
        }
    }


    /// <summary>
    /// 连接的条件,规则：所添加的都为顶层条件。
    /// 一个主条件。
    /// </summary>
    public class ConnectedNumCondition : BaseConnectedCondition<double>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConnectedNumCondition(NumeralCondition baseCondition):base( baseCondition)
        { 
        }  
        /// <summary>
        /// 增加条件
        /// </summary>
        /// <param name="connectedNumeralCondition"></param>
        /// <returns></returns>
        public ConnectedNumCondition AddConditon(ConnectedNumeralCondition connectedNumeralCondition)
        {
            Conditions.Add(connectedNumeralCondition);
            return this;
        }
        /// <summary>
        /// 增加条件
        /// </summary>
        /// <param name="connectOperator"></param>
        /// <param name="numeralCondition"></param>
        /// <returns></returns>
        public override BaseConnectedCondition<double> AddConditon(ConditionConnectOperator connectOperator, ICondition<double> numeralCondition)
        {
            return AddConditon(new ConnectedNumeralCondition(connectOperator, numeralCondition));
        }
    }
    /// <summary>
    /// 连接的条件,规则：所添加的都为顶层条件。
    /// 一个主条件。
    /// </summary> 
    public abstract class BaseConnectedCondition<TValue> : ICondition<TValue>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseConnectedCondition(ICondition<TValue> baseCondition)
        {
            this.Conditions = new List<AbstractConnectedCondition<TValue>>();
            this.BaseCondition = baseCondition;
        }
        /// <summary>
        /// 主条件
        /// </summary>
        public ICondition<TValue> BaseCondition { get; set; }
        /// <summary>
        /// 条件集合
        /// </summary>
        public List<AbstractConnectedCondition<TValue>> Conditions { get; set; }
        /// <summary>
        /// 增加条件
        /// </summary>
        /// <param name="connectOperator"></param>
        /// <param name="numeralCondition"></param>
        /// <returns></returns>
        public abstract  BaseConnectedCondition<TValue> AddConditon(ConditionConnectOperator connectOperator, ICondition<TValue> numeralCondition);
        /// <summary>
        /// 增加条件
        /// </summary>
        /// <param name="connectedNumeralCondition"></param>
        /// <returns></returns>
        public BaseConnectedCondition<TValue> AddConditon(AbstractConnectedCondition<TValue> connectedNumeralCondition)
        {
            Conditions.Add(connectedNumeralCondition);
            return this;
        }
        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool IsSatisfy(TValue val)
        {
            //如果顶层OR列表有一个满足条件，则为true。
            var ors = GetOrs();
            foreach (var condion in ors)
            {
                if (condion.IsSatisfy(val))
                {
                    return true;
                }
            }

            //主条件，不满足则返回 false
            if (!BaseCondition.IsSatisfy(val))
            {
                return false;
            }

            //下面全部是 true，否则全部需要 true 才行  
            foreach (var condion in this.Conditions)
            {
                if (!condion.IsSatisfy(val))
                {
                    return false;
                }
            }

            //都没有匹配成功，返回 false

            return true;
        }
        /// <summary>
        /// 获取顶层OR条件列表
        /// </summary>
        /// <returns></returns>
        public List<AbstractConnectedCondition<TValue>> GetOrs()
        {
            List<AbstractConnectedCondition<TValue>> list = new List<AbstractConnectedCondition<TValue>>();
            foreach (var condion in this.Conditions)
            {
                if (condion.ConnectOperator == ConditionConnectOperator.Or)
                {
                    list.Add(condion);
                }
            }
            return list;
        }
    }
    /// <summary>
    /// 条件连接条件
    /// </summary>
    public enum ConditionConnectOperator
    {
        /// <summary>
        /// 且
        /// </summary>
        And,
        /// <summary>
        /// 或
        /// </summary>
        Or
    }

    /// <summary>
    /// 字符串匹配
    /// </summary>
    public enum StringConditionOperator
    {
        /// <summary>
        /// 包含
        /// </summary>
        Contains,
        /// <summary>
        /// 包含于
        /// </summary>
        BeContained,
    }

    /// <summary>
    /// 数值条件
    /// </summary>
    public class StringCondition : IStringCondition
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conditionOperator"></param>
        /// <param name="FirstRefferValue"></param>
        /// <param name="SecondRefferValue"></param>
        /// <param name="IgnoreCase"></param>
        public StringCondition(StringConditionOperator conditionOperator, string FirstRefferValue, string SecondRefferValue = "" ,bool IgnoreCase = true)
        {
            this.IgnoreCase = IgnoreCase;
            this.ConditionOperator = conditionOperator;
            this.FirstRefferValue = IgnoreCase? FirstRefferValue.ToUpper(): FirstRefferValue;
            this.SecondRefferValue = IgnoreCase ? SecondRefferValue.ToUpper() : SecondRefferValue;
        }

        /// <summary>
        /// 条件操作符
        /// </summary>
        public StringConditionOperator ConditionOperator { get; set; }
        /// <summary>
        /// 忽略大小写
        /// </summary>
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// 第一个参考值
        /// </summary>
        public string FirstRefferValue { get; set; }

        /// <summary>
        /// 第二个参考值
        /// </summary>
        public string SecondRefferValue { get; set; }

        /// <summary>
        /// 是否满足
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool IsSatisfy(string val)
        {
            val = IgnoreCase ? val.ToUpper() : val;

            switch (ConditionOperator)
            {
                case StringConditionOperator.Contains:
                    return val.Contains(FirstRefferValue);
                case StringConditionOperator.BeContained:
                    return FirstRefferValue.Contains(val);
                    break;
                default:
                    break;
            } 
            return false;
        }

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ConditionOperator + " " + FirstRefferValue + ", " + SecondRefferValue;
        }

        /// <summary>
        /// 是否爽值条件
        /// </summary>
        /// <param name="numeralConditionOperator"></param>
        /// <returns></returns>
        public static bool IsDualValueCondition(NumeralConditionOperator numeralConditionOperator)
        {
            switch (numeralConditionOperator)
            {
                case NumeralConditionOperator.Between:
                case NumeralConditionOperator.BetweenOrEqual:
                    return true;
                default:
                    return false;
            }
        }
    }

    
    /// <summary>
    /// 数值条件操作符
    /// </summary>
    public enum NumeralConditionOperator
    {
        /// <summary>
        /// ==
        /// </summary>
        Equal,
        /// <summary>
        /// >
        /// </summary>
        GreaterThan,
        /// <summary>
        /// >=
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// <
        /// </summary>
        SmallerThan,
        /// <summary>
        /// <=
        /// </summary>
        SmallerThanOrEqual,
        /// <summary>
        /// ><
        /// </summary>
        Between,
        /// <summary>
        /// >= <=
        /// </summary>
        BetweenOrEqual,
    }

    /// <summary>
    /// 条件匹配
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface ICondition<TValue>
    {
        /// <summary>
        /// 是否满足
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        bool IsSatisfy(TValue val);
    }

    /// <summary>
    /// 字符串匹配
    /// </summary>
    public interface IStringCondition : ICondition<string>
    {


    }


    /// <summary>
    /// 数值条件
    /// </summary>
    public interface INumeralCondition: ICondition<double>
    { 
    }
/// <summary>
    /// 具有连接字符的条件
    /// </summary>
    public class ConnectedStringCondition : AbstractConnectedCondition<string>,  IStringCondition
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="condition"></param> 
        /// <param name="ConnectOperator"></param> 
        public ConnectedStringCondition(ConditionConnectOperator ConnectOperator, ICondition<string> condition) : base(ConnectOperator, condition)
        { 
        } 
    }
    /// <summary>
    /// 具有连接字符的条件
    /// </summary>
    public class ConnectedNumeralCondition : AbstractConnectedCondition<double>, INumeralCondition
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="condition"></param> 
        /// <param name="ConnectOperator"></param> 
        public ConnectedNumeralCondition(ConditionConnectOperator ConnectOperator, ICondition<double> condition) :base(ConnectOperator, condition) {
        }         
    }

    /// <summary>
    /// 数值条件
    /// </summary>
    public class NumeralCondition : AbstractCondition<double>, INumeralCondition
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conditionOperator"></param>
        /// <param name="FirstRefferValue"></param>
        /// <param name="SecondRefferValue"></param>
        public NumeralCondition(NumeralConditionOperator conditionOperator, double FirstRefferValue, double SecondRefferValue = 0)
        {
            this.ConditionOperator = conditionOperator;
            this.FirstRefferValue = FirstRefferValue;
            this.SecondRefferValue = SecondRefferValue;
        }

        /// <summary>
        /// 条件操作符
        /// </summary>
        public NumeralConditionOperator ConditionOperator { get; set; }

        /// <summary>
        /// 是否满足
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public override bool IsSatisfy(double val)
        {
            switch (ConditionOperator)
            {
                case NumeralConditionOperator.Equal:
                    return FirstRefferValue == val; 
                case NumeralConditionOperator.GreaterThan:
                    return val > FirstRefferValue; 
                case NumeralConditionOperator.GreaterThanOrEqual:
                    return val >= FirstRefferValue; 
                case NumeralConditionOperator.SmallerThan:
                    return val < FirstRefferValue; 
                case NumeralConditionOperator.SmallerThanOrEqual:
                    return val <= FirstRefferValue; 
                case NumeralConditionOperator.Between:
                    return val > FirstRefferValue && val < SecondRefferValue; 
                case NumeralConditionOperator.BetweenOrEqual:
                    return val >= FirstRefferValue && val <= SecondRefferValue;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ConditionOperator + " "  + FirstRefferValue + ", "  + SecondRefferValue;
        }

        /// <summary>
        /// 是否爽值条件
        /// </summary>
        /// <param name="numeralConditionOperator"></param>
        /// <returns></returns>
        public static bool IsDualValueCondition(NumeralConditionOperator numeralConditionOperator)
        {
            switch (numeralConditionOperator)
            { 
                case NumeralConditionOperator.Between: 
                case NumeralConditionOperator.BetweenOrEqual:
                    return true; 
                default:
                    return false; 
            } 
        }
    }

    /// <summary>
    /// 抽象类
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class AbstractConnectedCondition<TValue> : AbstractCondition<TValue>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="condition"></param> 
        /// <param name="ConnectOperator"></param> 
        public AbstractConnectedCondition(ConditionConnectOperator ConnectOperator, ICondition<TValue> condition)
        {
            this.Condition = condition;
            this.ConnectOperator = ConnectOperator;
        }

        /// <summary>
        /// 连接条件
        /// </summary>
        public ConditionConnectOperator ConnectOperator { get; set; }

        /// <summary>
        /// 数值条件
        /// </summary>
        public ICondition<TValue> Condition { get; set; }

        /// <summary>
        /// 是否满足
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public override bool IsSatisfy(TValue val)
        {
            return Condition.IsSatisfy(val);
        }
    }
    /// <summary>
    /// 抽象类
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class AbstractCondition<TValue> : ICondition<TValue>
    {
        /// <summary>
        /// 是否满足条件
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public abstract bool IsSatisfy(TValue val);

        /// <summary>
        /// 第一个参考值
        /// </summary>
        public TValue FirstRefferValue { get; set; }

        /// <summary>
        /// 第二个参考值
        /// </summary>
        public TValue SecondRefferValue { get; set; }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return " " + FirstRefferValue + ", " + SecondRefferValue;
        }
    }

}
