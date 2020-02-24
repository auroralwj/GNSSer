using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 指示该数据是否需要审核
    /// </summary>
   public  interface ICheckable
    {
        bool Checked { get; set; }
    }
}
