// 2014.09.10, czs, create in 海鲁吐， 具有名称的矩阵

using System;
using System.Collections.Generic;
using System.Text; 
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;


namespace Geo.Algorithm
{
    /// <summary>
    /// 元素具有名称的向量 的转换器。
    /// </summary>
    public class NamedVectorConvert
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="paramNames">名称列表</param>
        /// <param name="vector">向量</param>
        public NamedVectorConvert(List<string> paramNames, double[] vector)
        {
            dic = new Dictionary<string, double>();

            int i = 0;
            foreach (var item in paramNames)
            {
                dic.Add(item, vector[i]);
                i++;
            }
        }
        Dictionary<string, double> dic;
        /// <summary>
        /// 按照指定顺序获取新向量。
        /// </summary>
        /// <param name="newParamNames">名称列表</param>
        /// <returns></returns>
        public Vector GetNewVector(List<string> newParamNames)
        {
            int len = newParamNames.Count;
            Vector list = new Vector(len);
            int i = 0;
            foreach (var item in newParamNames)
            {
                list[i] = 0;
                if (dic.ContainsKey(item)) list[i] = dic[item];
                i++;
            }
            list.ParamNames = newParamNames;
            return list;
        }
        /// <summary>
        /// 名称向量转换静态方法。
        /// </summary>
        /// <param name="newParamNames">新的名称列表</param>
        /// <param name="oldParamNames">老的参数列表</param>
        /// <param name="oldMatrix">老的矩阵</param>
        /// <returns></returns>
        public static Vector ConvertNamedVector(List<string> newParamNames, List<string> oldParamNames, Vector oldMatrix)
        {
            NamedVectorConvert matrix = new NamedVectorConvert(oldParamNames, oldMatrix.OneDimArray);
            Vector matriResult = matrix.GetNewVector(newParamNames);
            return  matriResult;
        }
    }
     
}