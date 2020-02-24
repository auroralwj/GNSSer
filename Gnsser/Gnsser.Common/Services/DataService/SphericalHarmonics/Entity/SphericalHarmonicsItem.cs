//2017.11.06, czs, added, 球谐系数项目
//2018.05.26, czs, edit in hmx, 球谐函数系数别称

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Gnsser.Times; 
using Geo;
using Geo.Common;

namespace Gnsser.Data
{

    /// <summary>
    /// 球谐系数项目，下标编号为次数Order。
    /// </summary>
    public class SphericalHarmonicsItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="count"></param> 
        public SphericalHarmonicsItem(int count)
        {
            C = new RmsedNumeral[count];
            S = new RmsedNumeral[count];
            S[0] = RmsedNumeral.Zero;//第一个始终为0.
        }

        /// <summary>
        /// C 
        /// </summary>
        public RmsedNumeral[] C { get; set; }
        /// <summary>
        /// S
        /// </summary>
        public RmsedNumeral[] S { get; set; }
        /// <summary>
        /// 系数 C 的别称
        /// </summary>
        public RmsedNumeral[] Anm { get => C; set => C = value; }
        /// <summary>
        /// 系数 S 的别称
        /// </summary>
        public RmsedNumeral[] Bnm { get => S; set => S = value; }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="order">阶次</param>
        /// <param name="cOrAnm"></param>
        /// <param name="sOrBnm"></param>
        public void Set(int order, RmsedNumeral cOrAnm, RmsedNumeral sOrBnm)
        {
            C[order] = cOrAnm;
            S[order] = sOrBnm;
        }
        /// <summary>
        /// 设置,通过Order的正负判断数据是C还是S，后者为负。
        /// </summary>
        /// <param name="order">阶次</param>
        /// <param name="cOrAnm_sOrBnm"></param> 
        public void Set(int order, RmsedNumeral cOrAnm_sOrBnm)
        {
            if(order >= 0)
            {
                C[order] = cOrAnm_sOrBnm;
            }
            else
            { 
                S[ - order] = cOrAnm_sOrBnm;
            } 
        }
        /// <summary>
        /// 获取，First cOrAnm, Second sOrBnm
        /// </summary>
        /// <param name="order">阶次</param>
        /// <returns></returns>
        public Pair<RmsedNumeral> GetPair(int order)
        {
            return new Pair<RmsedNumeral>(C[order], S[order]);
        }

        public override string ToString()
        {
            string msg = "Order : " + C.Length;
            if(C.Length >0)
            msg += ", First C " + C[0];

            return msg;
        }
    }

}