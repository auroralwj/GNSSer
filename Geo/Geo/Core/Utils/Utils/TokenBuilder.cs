//2015.03.25, czs, create in namu, 创建令牌

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Utils
{
    /// <summary>
    /// 创建令牌
    /// </summary>
    public class TokenBuilder
    {
        /// <summary>
        /// 随机生成一个
        /// </summary>
        /// <returns></returns>
        static public string  RandomBuild(){
            var num = new Random(Int32.MaxValue / 2).NextDouble();
            return EncryptUtil.Encrypt(num.ToString());
        }
        
    }
}
