//2017.02.09, czs, create in hongqing, FCB 宽巷计算器
//2017.03.08, czs, edit in hongqing, MwTableBuilder单独提出，便于后续组建产品
//2018.08.30, czs, edit in hmx,去除卫星高度角文件，以星历服务代替，DCB改正适应所有日期
//2018.09.02, czs, create in hmx, 全球测站MW快速提取。
//2018.09.09, czs, create in hmx,  单站星时段MW数值快速提取。

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Gnsser
{
    /// <summary>
    /// 历元信息处理数据流
    /// </summary>
    public abstract class EpochInfoProcessStreamer : AbstractProcess<EpochInformation>
    {
        ILog log = new Log(typeof(EpochInfoProcessStreamer));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="AngleCut"></param> 
        public EpochInfoProcessStreamer(
            string path,
            List<SatelliteType> satelliteTypes = null)
        {
            this.FilePath = path;

            if (satelliteTypes == null)
            {
                SatelliteTypes = new List<SatelliteType>() { SatelliteType.G };
            }
            else
            {
                this.SatelliteTypes = satelliteTypes;
            }
        }


        #region 属性
        /// <summary>
        /// SatelliteTypes
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string FilePath { get; set; }
        #endregion

        #region 过程和输出产品 
        public RinexFileObsDataSource DataSource { get; set; }
        public BufferedStreamService<EpochInformation> BuffferStream { get; set; }
        public EpochInfoReviseManager Revisers { get; set; }
        #endregion

        public override void Init()
        {
            this.DataSource = new RinexFileObsDataSource(FilePath);
            this.BuffferStream = new BufferedStreamService<EpochInformation>(DataSource);
        }

        #region run

        /// <summary>
        /// 提取一个MW原始值。作为具有卫星和接收机硬件延迟的宽巷模糊度。
        /// </summary>
        /// <returns></returns>
        public void Run()
        {
            Init();

            foreach (var epoch in BuffferStream)
            {
                //简单的质量控制
                if (epoch == null) { continue; }

                Run(epoch);

            }//end of stream

            this.OnCompleted();
        }

        /// <summary>
        /// 预处理
        /// </summary>
        /// <param name="epoch"></param>
        public virtual void PreProcess(EpochInformation epoch)
        {
        }

        public override string ToString()
        {
            return FilePath;
        }
        #endregion
    }

}