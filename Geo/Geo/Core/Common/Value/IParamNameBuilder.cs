//2015.01.13, czs, create in namu, 名称生成接口

using System;
using System.Collections.Generic;

namespace Geo
{
    /// <summary>
    /// 参数命名器,生成接口
    /// </summary>
    public interface IParamNameBuilder : IBuilder<List<string>>
    { 
    }

    /// <summary>
    /// 参数名称生成器
    /// </summary>
    public abstract class ParamNameBuilder : IParamNameBuilder
    {
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract string GetParamName(object obj);
        /// <summary>
        /// 生成。
        /// </summary>
        /// <returns></returns>
        public abstract List<string> Build();
    }
}
