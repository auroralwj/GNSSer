//2017.02.09, czs, create in hongqing, FCB 宽巷计算器
//2017.03.08, czs, edit in hongqing, MwTableBuilder单独提出，便于后续组建产品
//2018.08.30, czs, edit in hmx,去除卫星高度角文件，以星历服务代替，DCB改正适应所有日期
//2018.09.02, czs, create in hmx, 全球测站MW快速提取。

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

namespace Gnsser.Winform
{ 

    /// <summary>
    /// 全球站星时段MW数值快速提取
    /// </summary>
    public class SiteMwValueManagerBuilder : AbstractBuilder<MultiSitePeriodValueStorage>
    {
        ILog log = new Log(typeof(MwTableBuilder));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathes"></param>
        /// <param name="AngleCut"></param> 
        public SiteMwValueManagerBuilder(
            string[] pathes,
            double AngleCut = 15,
            List<SatelliteType> satelliteTypes = null)
        {
            this.FilePathes = pathes;
            this.AngleCut = AngleCut;

            DcbRangeCorrector = new DcbRangeCorrector(GlobalDataSourceService.Instance.DcbDataService, false);
            ephemerisService = GlobalNavEphemerisService.Instance;

            MinEpochCount = 40;
            MaxAllowedRmsOfAveMw = 1;
            IsSmoothRange = false;
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
        ///处理完毕的事件
        /// </summary>
        public event Action OneFileProcessed;
        /// <summary>
        /// SatelliteTypes
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; set; }
        /// <summary>
        /// 平滑MW最大允许的中误差，不应超过1周
        /// </summary>
        public double MaxAllowedRmsOfAveMw { get; set; }
        DcbRangeCorrector DcbRangeCorrector { get; set; } 
        /// <summary>
        /// 路径
        /// </summary>
        public string[] FilePathes { get; set; }
        /// <summary>
        /// 卫星高度角
        /// </summary>
        public double AngleCut { get; set; }
        /// <summary>
        /// 最小的历元数量，小于此则不考虑
        /// </summary>
        int MinEpochCount { get; set; }
        IEphemerisService ephemerisService { get; set; }
        public bool IsSmoothRange { get; set; }
        #region 输出产品 
        public MultiSitePeriodValueStorage SiteMwValueManager { get; set; }
        #endregion
        #endregion

        public override MultiSitePeriodValueStorage Build()
        {
            //提取MW值    //并行提速.
            var result = new MultiSitePeriodValueStorage("各站各星各时段MW均值");
            log.Info("即将并行提取各站MW值。。。");
            Parallel.ForEach<string>(FilePathes, filePath =>
            {
                var builder = new SingleSiteMwValueBuilder(filePath, AngleCut, SatelliteTypes);
                builder.Completed += Builder_Completed;
                builder.IsSmoothRange = IsSmoothRange;
                builder.MinEpochCount = MinEpochCount;
                builder.EphemerisService = ephemerisService;
                builder.DcbRangeCorrector = DcbRangeCorrector;
                builder.MaxAllowedRmsOfAveMw = MaxAllowedRmsOfAveMw;

                var table =  builder.Build();
                if (table != null)
                {
                    var fileName = Path.GetFileName(filePath);
                    result.Add(fileName, table);
                }
            });

            this.SiteMwValueManager = result;

            return result;
        }

        private void Builder_Completed(object sender, EventArgs e)
        {
            log.Info(sender + " 计算完成！");
            OneFileProcessed?.Invoke();
        }         
    }    

}