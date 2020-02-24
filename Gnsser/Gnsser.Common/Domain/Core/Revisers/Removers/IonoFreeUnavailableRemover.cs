//2018.12.26, czs, create in  ryd, 移除不能组成无电离层组合的卫星数据

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;
using Gnsser.Domain;

namespace Gnsser
{
    /// <summary>
    /// 移除不能组成无电离层组合的卫星数据
    /// </summary>
    public class IonoFreeUnavailableRemover : EpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IonoFreeUnavailableRemover( )
        {
            this.Name = "移除不能组成无电离层组合的卫星数据";
            log.Info("启用 " + Name);
        }

        public override bool Revise(ref EpochInformation obj)
        {
            obj.RemoveIonoFreeUnavailable();
            return true;
        }
    }
}
