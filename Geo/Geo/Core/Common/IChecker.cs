//2015.10.26, czs, edit in hongqing, 提取抽象接口，对某一对象进行检核

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Geo;

namespace Gnsser.Checkers
{
    /// <summary>
    /// 抽象接口，对某一对象进行检核
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IChecker<T> : Geo.Namable
    {
        /// <summary>
        /// 检核是否满足要求
        /// </summary>
        /// <param name="epochInfo"></param>
        bool Check(T t);

        /// <summary>
        /// 异常或错误信息，当且仅当检查不通过时，才具有该信息。
        /// </summary>
        Exception Exception { get; }

    }
     
    /// <summary>
    /// 抽象接口，对某一对象进行检核
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Checker<T> : Named, IChecker<T>
    {
        /// <summary>
        /// 检核是否满足要求
        /// </summary>
        /// <param name="t"></param>
        public abstract bool Check(T t);

        /// <summary>
        /// 异常或错误信息，当且仅当检查不通过时，才具有该信息。
        /// </summary>
        public Exception Exception { get; protected set; }

    }
    /// <summary>
    /// 历元信息检核,卫星数量检核。
    /// </summary>
    public class CheckerChain<T> : Checker<T>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public CheckerChain()
        {
            Checkers = new List<IChecker<T>>();
        }


        /// <summary>
        /// 卫星数量
        /// </summary>
        private List<IChecker<T>> Checkers { get; set; }
        /// <summary>
        /// 添加一个检核器。
        /// </summary>
        /// <param name="checker"></param>
        public void Add(IChecker<T> checker)
        {
            Checkers.Add(checker);
        }

        /// <summary>
        /// 检核是否满足要求,不对数据本身进行修改
        /// </summary>
        /// <param name="epochInfo"></param>
        public override bool Check(T epochInfo)
        {
            bool result = true;
            foreach (var item in Checkers)
            {
                result = item.Check(epochInfo);
                if (!result)
                {
                    this.Exception = item.Exception;
                    return result;
                }

            }
            return result;
        }
    }
}
