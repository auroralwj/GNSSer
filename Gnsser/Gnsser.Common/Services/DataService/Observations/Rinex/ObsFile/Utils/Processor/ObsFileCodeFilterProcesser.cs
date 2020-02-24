//2015.02.09, czs, create in 麦克斯 双辽， 观测文件观测类型管理器

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Geo.Utils;
using System.Text;
using Geo.Coordinates; 
using Gnsser.Times;
using Geo;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// 观测文件处理器。
    /// </summary>
    public class ObsFileCodeFilterProcesser : ObsFileProcesser 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ObsFileCodeFilterProcesser() { }
        /// <summary>
        /// 最大出勤率，[0-100]
        /// </summary>
        public double MaxPercentage { get; set; }
        /// <summary>
        /// 处理过程
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Revise(ref RinexObsFile obsFile)
        {
            //this.Message = ObsFileTypeManager.RemoveObserversInfo(ref obsFile, MaxPercentage * 0.01);
            this.Message = ObsFileCountManager.RemoveObserversInfo(ref obsFile, MaxPercentage * 0.01);
            return true;
        }


    }
     
}