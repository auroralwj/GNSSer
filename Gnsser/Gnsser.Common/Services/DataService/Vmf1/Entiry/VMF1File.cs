using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Times;

namespace Gnsser.Data
{
    public class Vmf1File: IEnumerable<Vmf1Value>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="VMF1Infos"></param>
        public Vmf1File( List<Vmf1Value> VMF1Infos)
        {
            this.VMF1Infos = VMF1Infos;
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get { return VMF1Infos.Count; } }

        /// <summary>
        /// 数据列表
        /// </summary>
   
        public List<Vmf1Value> VMF1Infos { get; set; }

        /// <summary>
        /// 清除所有
        /// </summary>
        public void Clear()
        {
            VMF1Infos.Clear();
        }

        public List<Vmf1Value> GetStaInfo(string staname)
        {
            List<Vmf1Value> staInfos = new List<Vmf1Value>();
            //VMF1Value satInfo = VMF1Infos.Find(m => m.stanam.Equals(staname));
            //double mjd = (double)time.MJulianDays;
            foreach(var item in VMF1Infos)
            {
                
                if (item.stanam == staname.ToUpper())
                {
                    staInfos.Add(item);
                }
            }            
            //double[] arraySolu = new double[10];
            return staInfos;
        }
        #region override
        public IEnumerator<Vmf1Value> GetEnumerator()
        {
            return VMF1Infos.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return VMF1Infos.GetEnumerator();
        }
        #endregion
    }
}
