//2014.10.26，czs, create in numu, 具体的改正数表

using System;
using System.Text;
using System.Collections.Generic;
using Geo.Utils;

namespace Geo.Correction
{
    /// <summary>
    /// 详细的改正数表。
    /// </summary>
    public interface ICorrectionDic<TCorrection>
    {
        /// <summary>
        /// 所有改正数之和。
        /// </summary>
        TCorrection TotalCorrection { get; } 

        /// <summary>
        /// 详细的改正数表。
        /// </summary>
       // Dictionary<string, TCorrection> Corrections { get; }

        int Count { get; }

        /// <summary>
        /// 添加一个改正数。
        /// </summary>
        /// <param name="type">改正数类型</param>
        /// <param name="correction">改正数</param>
        void SetCorrection(string type, TCorrection correction);

        /// <summary>
        /// 添加一个改正数。
        /// </summary>
        /// <param name="corrections">改正数集合</param> 
        void SetCorrection(Dictionary<string, TCorrection> corrections);

        /// <summary>
        /// 清楚所有改正。
        /// </summary>
        void ClearCorrections();

        /// <summary>
        /// 是否包含改正数。
        /// </summary>
        /// <param name="type">改正数类型</param>
        /// <returns></returns>
        bool ContainsCorrection(string type);
    }     
}
