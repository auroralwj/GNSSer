//2015.11.10, czs, create in K166 成都到西安列车, 操作参数管理器  

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data; 
using System.Linq;
using System.Text; 
using Gnsser.Api;
using Geo;
using Geo.IO;
using Gnsser;

namespace Gnsser
{
    /// <summary>
    /// 操作参数管理器
    /// </summary>
    public class ParamManager 
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ParamManager()
        {
            ParamIoerManager = ParamIoerManager.Default;
            ParamNameManager = ParamNameManager.Default;
        }

        public ParamNameManager ParamNameManager { get; set; }

        public ParamIoerManager ParamIoerManager { get; set; }
         
    }
}
