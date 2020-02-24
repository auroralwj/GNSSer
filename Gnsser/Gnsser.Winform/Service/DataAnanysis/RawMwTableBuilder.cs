//2017.02.09, czs, create in hongqing, FCB 宽巷计算器
//2017.03.08, czs, edit in hongqing, MwTableBuilder单独提出，便于后续组建产品
//2018.08.30, czs, edit in hmx,去除卫星高度角文件，以星历服务代替，DCB改正适应所有日期
//2018.09.03, czs, edit in hmx, 引入平滑伪距

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
    /// MW 单独提出，便于后续组建产品
    /// </summary>
    public class MwTableBuilder : AbstractBuilder<ObjectTableManager>
    {
        ILog log = new Log(typeof(MwTableBuilder));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathes"></param>
        /// <param name="AngleCut"></param> 
        /// <param name="RowCountToBeEmpty">移除前面的数据行数量，避免数据偏差太大</param>
        /// <param name="OutputDirectory"></param>
        public MwTableBuilder(
            string[] pathes,
            double AngleCut = 30,
            int RowCountToBeEmpty = 40,
            string OutputDirectory = null,
            List<SatelliteType> satelliteTypes = null, bool IsSmooth = true, bool IsCalculateFraction = true)
        {
            this.FilePathes = pathes;
            this.AngleCut = AngleCut;
            this.RowCountToBeEmpty = RowCountToBeEmpty;
            this.IsCalculateFraction = IsCalculateFraction;


            DcbRangeCorrector = new DcbRangeCorrector(GlobalDataSourceService.Instance.DcbDataService, false);
            EphemerisService = GlobalNavEphemerisService.Instance;

            if (OutputDirectory != null)
            {
                this.OutputDirectory = OutputDirectory;
            }
            else
            {
                this.OutputDirectory = Setting.TempDirectory;
            }
            if (satelliteTypes == null)
            {
                SatelliteTypes = new List<SatelliteType>() { SatelliteType.G };
            }
            else
            {
                this.SatelliteTypes = satelliteTypes;
            }
            this.IsSmooth = IsSmooth;
            AveMinCount = 8;
            AveMaxBreakCount = 2;
            AveMaxDiffer = 2;
        }
        /// <summary>
        /// 求平均最小数量
        /// </summary>
        public int AveMinCount { get; set; }
        /// <summary>
        /// 求平均最大允许断裂数
        /// </summary>
        public int AveMaxBreakCount { get; set; }
        /// <summary>
        /// 求平均最大允许差别
        /// </summary>
        public double AveMaxDiffer { get; set; }

        #region 属性
        /// <summary>
        /// 是否平滑
        /// </summary>
        public bool IsSmooth { get; set; }
        
        /// <summary>
        /// 是否计算小数部分
        /// </summary>
        public bool IsCalculateFraction { get; set; }
        /// <summary>
        /// DCB 改正
        /// </summary>
        DcbRangeCorrector DcbRangeCorrector { get; set; }

        public event Action OneFileProcessed;

        /// <summary>
        /// 移除前面的数据行数量，避免数据偏差太大
        /// </summary>
        public int RowCountToBeEmpty { get; set; }
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string[] FilePathes { get; set; }
        /// <summary>
        /// 卫星高度角
        /// </summary>
        public double AngleCut { get; set; }
        /// <summary>
        /// SatelliteTypes
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; set; }

        #region 输出产品
        /// <summary>
        /// 基于测站的宽巷原始数据
        /// </summary>
        public ObjectTableManager RawMwValue { get; private set; }
        /// <summary>
        /// 星历，用于计算卫星高度
        /// </summary>
        IEphemerisService EphemerisService { get; set; }
        /// <summary>
        /// 基于测站的宽巷原始数据的平滑结果
        /// </summary>
        public ObjectTableManager SmoothedMwValue { get; private set; }
        /// <summary>
        /// 网解MW平滑浮点数部分
        /// </summary>
        public ObjectTableManager FractionOfSmoothedMwValue { get; private set; }
        /// <summary>
        /// 是否平滑伪距
        /// </summary>
        public bool IsSmoothRange { get; set; }
        /// <summary>
        /// MW时段均值信息，不需要平滑即可输出
        /// </summary>
        public ObjectTableStorage MwPeriodAverage { get; set; }

        #endregion
        #endregion
        /// <summary>
        /// 构建,返回 SiteSmoothedMwTables。
        /// </summary>
        /// <returns></returns>
        public override ObjectTableManager Build()
        {
            //----------基于测站--------
            //基于测站的MW原始值
            log.Info("并行提取MW值开始。");
            this.RawMwValue = ExtractRawMwValueFromObsFiles();
            log.Info("并行提取MW值完毕。");

            //计算各测站卫星时段平均值，汇总到一个表格
            this.MwPeriodAverage =  this.RawMwValue.GetGroupAverageTableOfCols(AveMinCount, AveMaxBreakCount, AveMaxDiffer, "Site");


            //不要删除没有基准卫星的数据行，注意：对于大区域数据，不应该删除，否则会接不上，2017.02.26.23.13，czs
            // SiteRawMwTables.RemoveEmptyRows(); //删除空行，是否会影响索引判断??暂时未定 2017.03.08.06.27， czs

            //基于测站的MW平滑值，周跳特性保持不变
            if (IsSmooth)
            {
                log.Info("平滑MW值开始。");
                this.SmoothedMwValue = RawMwValue.GetSmoothedTable(1, false, "SmoothedMw", false);
                if (RowCountToBeEmpty > 0)
                {
                    this.SmoothedMwValue.EmptyFrontValueOfCols(RowCountToBeEmpty);//清空前面不稳定的部分数据，避免平滑前的偏移
                }
                //测试平滑后MW小数部分数据变化量
                if (IsCalculateFraction)
                {
                    this.FractionOfSmoothedMwValue = this.SmoothedMwValue.GetPeriodPipeFilterTable(1, 0, true, "FractionOfSmoothedMw");
                }
            }
            //   this.SiteSmoothedMwTables.WriteAllToFileAndCloseStream();
            return this.SmoothedMwValue;
        }

        #region 提取MW值
        /// <summary>
        /// 从观测文件中提取MW原始值。作为具有卫星和接收机硬件延迟的宽巷模糊度。
        /// </summary>
        /// <returns></returns>
        private ObjectTableManager ExtractRawMwValueFromObsFiles(string namePostfix = "_MwRaw")
        {
            //提取MW值    //并行提速.
            var dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            Parallel.ForEach<string>(FilePathes, filePath =>
            {
                var builder = new RawMwTableBuilder(filePath, AngleCut, RowCountToBeEmpty, SatelliteTypes);
                builder.Completed += Builder_Completed;
                builder.IsSmoothRange = IsSmoothRange; 
                var table = builder.Build();
                if (table != null)
                {
                    var siteName = RinexObsFileReader.ReadSiteName(filePath, 8).Trim() + namePostfix;

                    //var fileName = Path.GetFileName(filePath);
                    dic.TryAdd(siteName, table);
                }
            });
            var mwTables = new ObjectTableManager(dic) { OutputDirectory = OutputDirectory };

            OneFileProcessed?.Invoke();

            return mwTables;
        }

        private void Builder_Completed(object sender, EventArgs e)
        {
            log.Info(sender + " 计算完成！");
            OneFileProcessed?.Invoke();
        }
        #endregion
    }
}