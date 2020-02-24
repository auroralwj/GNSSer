//2015.06.03, czs , ceate in namu, 提取标识接口
//2016.02.23, czs, edit in hongqing, 移动到顶层模型

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{

    /// <summary>
    /// 包含 Comment备注。
    /// </summary>
    public interface IComment
    {
        string Comment { get; set; }
    }
}