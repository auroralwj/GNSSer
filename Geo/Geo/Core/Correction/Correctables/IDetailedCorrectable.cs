//2014.10.25, czs, create in numu, 可以改正的对象，对象与改正数不必是同一种类型, 显示改正数的详细情况
//2014.11.19, czs, edit in numu, 名称 IDetailedCorrectableObject 修改为 IDetailedCorrectable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Correction
{  
   /// <summary>
    /// 可以改正的对象，对象与改正数不必是同一种类型, 显示改正数的详细情况
    /// </summary>
    /// <typeparam name="TValue">原数值类型</typeparam>
    /// <typeparam name="TCorrection">改正数类型</typeparam> 
    public interface IDetailedCorrectable<TValue, TCorrection> : ICorrectable<TValue, TCorrection>, ICorrectionDic<TCorrection>
    {
    }     
}
