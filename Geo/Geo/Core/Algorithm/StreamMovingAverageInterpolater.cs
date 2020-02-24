//2014.11.08, czs, create in mamu, 滑动平均插值器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 滑动平均插值器。
    /// </summary>
    public class StreamMovingAverageInterpolater : YGetter
    {
        public StreamMovingAverageInterpolater(IEnumerable<double> ys,  int order =10)
        {
            this.Ys = ys;
            this.Order = order;
            this.Currents = new List<double>();
            YEnumer = Ys.GetEnumerator(); 
        }
        /// <summary>
        /// 滑动个数。
        /// </summary>
        public int Order { get; protected set; }

        IEnumerable<double> Ys { get; set; }
        IEnumerator<Double> YEnumer;
         
        List<double> Currents { get;set;}

        public override double GetY(double xValue)
        {
            List<double> nexts = GetNexts();
            if (nexts.Count == Order)
            {
                double temp = DoubleUtil.Average(nexts);
                return temp;
            }
            return 0;
            //throw new NotImplementedException();
        }


        public List<double> GetNexts()
        {
            List<double> nexts = new List<double>();
            nexts.AddRange(Currents);
            if (nexts.Count >= Order) nexts.RemoveAt(0);//去掉第一个
            while (nexts.Count < 10)
            {  
                if(YEnumer.MoveNext()){
                    nexts.Add(YEnumer.Current);
                }
                else
                { 
                    break;
                }
            }
            return nexts;
        }
    }
}
