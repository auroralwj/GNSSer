//2014.05.22, Cui Yang, created
//2014.07.04, Cui Yang, 增加多映射通用集合类，添加了MultiMap引用
//2014.07.05, czs, edit, 进行了代码重构
//2014.09.24, czs, edit, 重命名为 SateInfoFile

using System;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Times;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;

namespace Gnsser.Data
{ 
    /// <summary>
    /// 时间段的卫星状态文件。
    /// </summary>
    public class DcbFile : IEnumerable<DcbValue>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SatInfos">卫星状态列表</param>
        public DcbFile(List<DcbValue> DcbInfos)
        {
            this.DcbInfos = DcbInfos;
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get { return DcbInfos.Count; } }
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<DcbValue> DcbInfos { get; set; }

        /// <summary>
        /// Method to clear all previously loaded satellite satData.
        /// </summary>
        public void Clear()
        {
            DcbInfos.Clear();
        }

       
        public double GetValue(SatelliteNumber prn)
        {
            DcbValue sat = GetSatInfo(prn);
            if (sat == null) return 0.0;
            return sat.Value;
        }
        /// <summary>
        /// 获取第一个满足条件的对象
        /// </summary>
        /// <param name="satelliteType">卫星编号</param>
        /// <param name="epoch">时间</param>
        /// <returns></returns>
        public DcbValue GetSatInfo(SatelliteNumber prn)
        {
            DcbValue sat = DcbInfos.Find(m => m.Prn.Equals(prn));
            if (sat == null) return new DcbValue(prn, 0.0, 0.0);//对于P2C2，可能不存在
            return sat;
        }

        #region override
        public IEnumerator<DcbValue> GetEnumerator()
        {
            return DcbInfos.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return DcbInfos.GetEnumerator();
        }
        #endregion
    }
  
}
