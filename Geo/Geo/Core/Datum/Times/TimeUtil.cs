//2018.06.04, czs, create in hmx, 时间工具


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Times
{
    /// <summary>
    /// 时间工具
    /// </summary>
    public class TimeUtil
    {
        /// <summary>
        /// 获取最接近的列表
        /// </summary>
        /// <param name="XArray">待选</param>
        /// <param name="tobeNear">指定值</param>
        /// <param name="count">选择数量</param>
        /// <returns></returns>
        public static List<Times.Time> GetNearst(List<Time> XArray, Time tobeNear, int count = 9) 
        {
            List<Time> indexes = new List<Time>();
            //如果数量大于数组数量，则返回全部
            if (count >= XArray.Count)
            {
                return XArray;
            }

            //找到离X最小的编号
            int halfCount = count / 2;
            int index = 0;
            double min = double.MaxValue;
            for (int i = 0; i < XArray.Count; i++)
            {
                double diff = Math.Abs(XArray[i] - (tobeNear));
                // if (diff == 0) return YArray[time];
                if (diff < min)
                {
                    min = diff;
                    index = i;
                }
            }
            //在数组的头部 
            int startIndex = 0;
            if (index - halfCount <= 0) { startIndex = 0; }//for (int i = 0; i < count; i++) indexes.Add(XArray[i]);
            //尾部
            else if (index + halfCount >= XArray.Count - 1) { startIndex = XArray.Count - count; }//for (int i = 0, j = XArray.Count - 1; i < count; i++, j--) indexes.Insert(0, XArray[j]);
            //中间
            else for (int i = 0; i < count; i++) { startIndex = index - halfCount; }// indexes.Add(XArray[index - halfCount + i]);

            //if (indexes[0] < 0) throw new Exception("出错了");

            indexes = XArray.GetRange(startIndex, count);

            return indexes;
        }



        /// <summary>
        /// 获取与指定对象最近的值。
        /// 如果返回只有一个值，则是刚好等于，或最小或最大边界值。
        /// 如果不是一个值，则返回为三个值。第一个为与指定对象最近的值，第二个为小于指定值的数，第三个为大于该值的数,如果数值为null默认值，则表示没有。
        /// </summary>
        /// <typeparam name="Time"></typeparam>
        /// <param name="list">从小到大已排序的列表</param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<Time> GetNearst(List<Time> list, Time t)
        {
            //以防万一
            list = list.OrderBy(m => m).ToList();

            var first = list.First();
            var last = list.Last();
            if (t - (first) < 0) //在之前
            {
                return new List<Time>() { first };
            }
            if (t - (last) > 0)//在之后
            {
                return new List<Time>() { last };
            }
            Time defa = default(Time);
            Time nearst = defa;
            double maxDiffer = Double.MaxValue;
            foreach (var item in list)
            {
                double differ = t - (item);
                if (differ == 0)
                {
                    return new List<Time>() { item };
                }

                double absDiffer = Math.Abs(differ);
                if (absDiffer < maxDiffer) { nearst = item; maxDiffer = differ; }
            }

            Time smaller = defa;
            Time larger = defa;
            var index = list.IndexOf(nearst);
            if (t - (nearst) < 0) //在之前
            {
                smaller = list[index - 1];
                larger = nearst;
            }
            else //在之后
            {
                smaller = nearst;
                larger = list[index + 1];
            }

            return new List<Time>()
            {
                nearst, smaller, larger
            };
        }
        /// <summary>
        /// 获取在指定间隔内的时间列表，如果断裂，则只返回断裂后的时间。
        /// 数据进来先排序。
        /// </summary>
        /// <param name="times"></param>
        /// <param name="MaxTimeSpan">单位秒</param>
        /// <returns></returns>
        public static List<Time> GetOrderedNoJumpTimes(List<Time> times, double MaxTimeSpan)
        {
            times.Sort();
            Time time = times[0];
            List<Time> breakedTimes = new List<Time>();
            foreach (var item in times)
            {
                double differ = item - time;

                if (differ > MaxTimeSpan)
                {
                    breakedTimes.Add(item);
                }
                time = item;//update
            }
            if (breakedTimes.Count == 0)
            {
                return times;
            }
            List<Time> results = new List<Time>();
            var lastBreaked = breakedTimes[breakedTimes.Count - 1];
            foreach (var item in times)
            {
                if (item >= lastBreaked)
                {
                    results.Add(item);
                }
            }
            return results;
        }


    }
}
