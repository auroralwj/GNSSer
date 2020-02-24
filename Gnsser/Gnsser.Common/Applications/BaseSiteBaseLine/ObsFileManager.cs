//2016.04.19, czs, create in hongqing, 多个观测文件管理分析器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Gnsser.Times;
using System.IO;
using Gnsser.Data.Rinex;
using Geo.Coordinates;

namespace Gnsser
{

    /// <summary>
    /// 中心站基线管理器
    /// </summary>
    public class CenterSiteBaseLineMangager : BaseDictionary<string, List<GnssBaseLineName>>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CenterSiteBaseLineMangager()
        {

        }
    }

    /// <summary>
    /// 多个观测文件管理器。同步观测文件。
    /// </summary>
    public class ObsSiteFileManager : Geo.BaseDictionary<string, ObsSiteInfo>
    {
        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="obsFilePaths"></param>
        /// <param name="MinEpochTimeSpan"></param>
        public ObsSiteFileManager( IEnumerable<string> obsFilePaths, TimeSpan MinEpochTimeSpan)
        {
            this.MinEpochTimeSpan = MinEpochTimeSpan;
            foreach (var item in obsFilePaths)
            {
                var site = new ObsSiteInfo(item);
                Add(site);
            }
        }

        public ObsSiteFileManager(IEnumerable<ObsSiteInfo> obsFilePaths, TimeSpan MinEpochTimeSpan)
        {
            this.MinEpochTimeSpan = MinEpochTimeSpan;
            foreach (var item in obsFilePaths)
            {
                var site =  (item);
                Add(site);
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="site"></param>
        public void Add(ObsSiteInfo site)
        {
            this[site.SiteName] = site;
        }
        /// <summary>
        /// 最小的同步时段,少于则忽略
        /// </summary>
        public TimeSpan MinEpochTimeSpan { get; set; }
        /// <summary>
        /// 所有测站的名称
        /// </summary>
        public List<string> SiteNames { get => this.Keys.ToList(); }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ObsSiteFileManager(TimeSpan MinEpochTimeSpan)
        {
            this.MinEpochTimeSpan = MinEpochTimeSpan;
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Remove(ObsSiteInfo obj)
        {
           string findKey = null;//不能通过名称移除，否则会移除所有时段的该站！！！！必须通过路径判断，2019.01.18，czs， hmx
            foreach (var item in this)
            {
                if (item.Equals(obj))
                {
                    findKey = item.SiteName;
                    break;
                }
            }
            if (findKey!= null)
            {
                this.Remove(findKey);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 所有测站的路径
        /// </summary>
        /// <returns></returns>
        public List<string> GetSitePathes()
        {
            List<string> pathes = new List<string>();

            foreach (var item in this)
            {
                pathes.Add(item.FilePath);
            }

            return pathes;
        }

        /// <summary>
        /// 创建所有三角形名称
        /// </summary> 
        /// <returns></returns>
        public List<TriangularNetName> GenerateAllTriangularNetName()
        {
            var siteNames = Keys;
            return TriangularNetName.BuildTriangularNetNames(siteNames);
        }
        /// <summary>
        /// 构建当前与输入测站组合的所有基线
        /// </summary>
        /// <param name="newSite"></param>
        /// <returns></returns>
        public List<SiteObsBaseline> GenerateBaseLines(ObsSiteInfo newSite)
        {
           var list = new List<SiteObsBaseline>(); 
            foreach (var item in this)
            {
                if(item.SiteName == newSite.SiteName) { continue; }

                var line = new SiteObsBaseline(item, newSite);
                list.Add(line);
            }
            return list;
        }
            /// <summary>
            /// 获取所有的基线. 这是一个组合问题。
            /// </summary>
            /// <returns></returns>
         public List<SiteObsBaseline> GenerateObsBaseLines(BaseLineSelectionType baseLineSelectionType, string ceterSiteName, string outerBaseLineFilePath)
         {

            //读取基线
            var IndicatedBaseLines = new List<GnssBaseLineName>();
            if (File.Exists(outerBaseLineFilePath))
            {
                var lines = File.ReadAllLines(outerBaseLineFilePath);
                foreach (var line in lines)
                {
                    if (String.IsNullOrEmpty(line.Trim())) { continue; }

                    IndicatedBaseLines.Add(new GnssBaseLineName(line));
                }
            }


            BaseLineSelector baseLineSelector = new BaseLineSelector(baseLineSelectionType, ceterSiteName, IndicatedBaseLines);

            var lineNames = baseLineSelector.Build(this.Values);
            List<SiteObsBaseline> list = new List<SiteObsBaseline>();
            foreach (var item in lineNames)
            {
                var refSite = this.Get(item.RefName);
                var rovSite = this.Get(item.RovName);
                var line = new SiteObsBaseline(rovSite, refSite);
                if (line.TimeSpan > MinEpochTimeSpan)
                {
                    list.Add(line);
                }
            }
            return list;
        }

        public CenterSiteBaseLineMangager BuildOrSyncCenterSiteBaseLineMangager(CenterSiteBaseLineMangager CenterSiteBaseLineMangager)
        {
            //没有，则重新生成
            if (CenterSiteBaseLineMangager == null) { CenterSiteBaseLineMangager = GenerateCenterSiteBaseLineMangager(); return CenterSiteBaseLineMangager; }

            //有则同步
            var newBuild = GenerateCenterSiteBaseLineMangager();
            //删除没有包含的测站
            List<string> toremove = new List<string>();
            foreach (var item in CenterSiteBaseLineMangager.Keys)
            {
                if (!newBuild.Contains(item))
                {
                    toremove.Add(item);
                }
            }
            CenterSiteBaseLineMangager.Remove(toremove);

            //添加没有的 
            foreach (var kv in newBuild.KeyValues)
            {
                var list = CenterSiteBaseLineMangager.GetOrCreate(kv.Key);
                foreach (var item in kv.Value)
                {
                    if (!list.Contains(item)) { list.Add(item); }
                }
            }
            return CenterSiteBaseLineMangager;
        }


        /// <summary>
        /// 中心站的基线管理器
        /// </summary>
        /// <returns></returns>
        public CenterSiteBaseLineMangager GenerateCenterSiteBaseLineMangager()
        {
            var sites = this.Keys;
            CenterSiteBaseLineMangager siteObsBaselines = new CenterSiteBaseLineMangager();
            foreach (var refSite in sites)
            {
                BaseLineSelector baseLineSelector = new BaseLineSelector(BaseLineSelectionType.中心站法, refSite, "");
                siteObsBaselines[refSite] = baseLineSelector.Build(this.Values);
            }

            return siteObsBaselines;
        }


        /// <summary>
        /// 获取所有的基线. 这是一个组合问题。
        /// </summary>
        /// <returns></returns>
        public List<SiteObsBaseline> GenerateAllCenteredObsBaseLines(CenterSiteBaseLineMangager CenterSiteBaseLineMangager)
        {
            List<SiteObsBaseline> list = new List<SiteObsBaseline>();
            foreach (var lineNames in CenterSiteBaseLineMangager)
            {
                foreach (var item in lineNames)
                {
                    var refSite = this.Get(item.RefName);
                    var rovSite = this.Get(item.RovName);
                    var line = new SiteObsBaseline(rovSite, refSite);
                    if (line.TimeSpan > MinEpochTimeSpan)
                    {
                        list.Add(line);
                    }
                }
            }

            return list;
        }

    }


    /// <summary>
    /// 观测文件信息。一个类，对应一个文件。
    /// </summary>
    public class ObsSiteInfo : IComparable<ObsSiteInfo>
    {

        /// <summary>
        /// 默认构造函数。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="TempSubDirectory"></param>
        public ObsSiteInfo(string path, string TempSubDirectory = "Temp")
        {
            this.TempSubDirectory = TempSubDirectory;
            var reader = new Gnsser.Data.Rinex.RinexObsFileReader(path, false);
            this.SiteObsInfo = reader.GetHeader();
        }
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        /// <param name="TempSubDirectory"></param>
        /// <param name="header"></param>
        public ObsSiteInfo(RinexObsFileHeader header, string TempSubDirectory = "Temp")
        {
            this.TempSubDirectory = TempSubDirectory;
            this.SiteObsInfo = header;
        }
        /// <summary>
        /// 临时子目录路径
        /// </summary>
        public string TempSubDirectory { get; set; }

        /// <summary>
        /// 时段信息
        /// </summary>
        public TimePeriod TimePeriod { get { return this.SiteObsInfo.ObsInfo.TimePeriod; } }
        /// <summary>
        /// 时段网
        /// </summary>
        public TimePeriod NetPeriod { get; set; }
        /// <summary>
        /// 原始路径
        /// </summary>
        public string OriginalPath { get => SiteObsInfo.FileInfo.FilePath; }
        /// <summary>
        /// 临时文件路径，当修改了源文件时，启用临时文件。
        /// </summary>
        public string TempFilePath { get => Path.Combine(TempDirectory, SiteObsInfo.FileInfo.FileName); }
        /// <summary>
        /// 临时目录
        /// </summary>
        public string TempDirectory { get => Path.Combine(Directory, TempSubDirectory); } 
        /// <summary>
        /// 文件路径。优先返回临时路径。
        /// </summary>
        public string FilePath
        {
            get
            {
                if (File.Exists(TempFilePath))
                {
                    return TempFilePath; 
                }
                else
                {
                    return SiteObsInfo.FileInfo.FilePath;
                }
            }
        }
        /// <summary>
        /// 所在目录
        /// </summary>
        public string Directory { get => Path.GetDirectoryName(OriginalPath); }

        /// <summary>
        /// 测站名称
        /// </summary>
        public string SiteName { get { return SiteObsInfo.SiteName; } }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName => SiteObsInfo.FileInfo.FileName;
        /// <summary>
        /// 平差结果
        /// </summary>
        public EstimatedSite EstimatedSite { get; set; }

        /// <summary>
        /// 测站观测信息。
        /// </summary>
        public IExtendSiteObsInfo SiteObsInfo { get; set; }
        /// <summary>
        /// 检查是否存在临时文件，没有则复制一个
        /// </summary>
        public void CheckOrCopyToTempDirectory()
        {
            if (!File.Exists(TempFilePath))
            {
                CopyToTempDirectory(false);
            }
        }

        /// <summary>
        /// 复制到临时目录
        /// </summary>
        /// <param name="isOverrite"></param>
        public void CopyToTempDirectory(bool isOverrite = true)
        {
            Geo.Utils.FileUtil.CheckOrCreateDirectory(TempDirectory);
            Geo.Utils.FileUtil.CopyFile(OriginalPath, TempFilePath, isOverrite);
        }

        public int CompareTo(ObsSiteInfo other)
        {
            return SiteObsInfo.ObsInfo.TimePeriod.CompareTo(other.TimePeriod);
        }

        public override string ToString()
        {
            return SiteName;// + " " + TimePeriod + " " + SiteObsInfo.ApproxXyz + " " + FilePath;
        }
        public override bool Equals(object obj)
        {
            if(obj == null) { return false; }

            ObsSiteInfo o = obj as ObsSiteInfo;
            if(o == null) { return false; }

            return o.FileName == this.FileName && this.TimePeriod.Equals(o.TimePeriod); ;
        }
        public override int GetHashCode()
        {
            return FileName.GetHashCode();
        }
        /// <summary>
        /// 转换为可读文本
        /// </summary>
        /// <returns></returns>
        public string ToReadableText()
        {
            var sb = new StringBuilder();
            sb.AppendLine("测站名称：" + this.ToString());
            sb.AppendLine("观测时段：" + this.SiteObsInfo.ObsInfo.TimePeriod);
            sb.AppendLine("近似坐标：" + this.SiteObsInfo.ApproxXyz);
            sb.AppendLine("天线类型：" + this.SiteObsInfo.SiteInfo.AntennaType + " " + this.SiteObsInfo.SiteInfo.AntennaNumber);
            sb.AppendLine("导航文件：" + this.SiteObsInfo.NavFilePath);
            return sb.ToString();
        }

    } 

    /// <summary>
    /// 测站平差管理器
    /// </summary>
    public class EstimatedSiteManager : BaseDictionary<string, EstimatedSite>
    {

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static EstimatedSiteManager Parse(ObjectTableStorage table)
        {
            EstimatedSiteManager manager = new EstimatedSiteManager();
            foreach (var item in table.BufferedValues)
            {
                EstimatedSite estimatedSite = EstimatedSite.ParseRow(item);
                manager[estimatedSite.SiteName] = estimatedSite;
            }
            return manager;
        }
    }

    /// <summary>
    /// 平差后的测站坐标
    /// </summary>
    public class EstimatedSite : IObjectRow
    {
        public EstimatedSite() { }
        public EstimatedSite(string SiteName, RmsedXYZ RmsedXYZ, Time epoch)
        {
            this.SiteName = SiteName;
            this.RmsedXYZ = RmsedXYZ;
            this.Epoch = epoch;
        }

        public Time Epoch { get; set; }
        public string SiteName { get; set; }

        public RmsedXYZ RmsedXYZ { get; set; }

        public GeoCoord GeoCoord => Geo.Coordinates.CoordTransformer.XyzToGeoCoord(RmsedXYZ.Value);

        public Dictionary<string, object> GetObjectRow()
        {
            Dictionary<string, object> row = new Dictionary<string, object>();

            row[ParamNames.Name] = SiteName;
            row[ParamNames.X] = (this.RmsedXYZ.Value.X);
            row[ParamNames.Y]=( this.RmsedXYZ.Value.Y);
            row[ParamNames.Z]=( this.RmsedXYZ.Value.Z);
            row[ParamNames.Lon]=( this.GeoCoord.Lon);
            row[ParamNames.Lat]=( this.GeoCoord.Lat);
            row[ParamNames.Height]=( this.GeoCoord.Height);
            row[ParamNames.RmsX] = (this.RmsedXYZ.Rms.X);
            row[ParamNames.RmsY] = (this.RmsedXYZ.Rms.Y);
            row[ParamNames.RmsZ] = (this.RmsedXYZ.Rms.Z);
            row[ParamNames.Epoch] = Epoch;
            return row;
        }

        public static EstimatedSite ParseRow(Dictionary<string, object> row)
        {
            EstimatedSite est = new EstimatedSite();
            var name = row[ParamNames.Name]+"";
            double x = Geo.Utils.ObjectUtil.GetNumeral(row[ParamNames.X]);
            double y = Geo.Utils.ObjectUtil.GetNumeral(row[ParamNames.Y]);
            double z = Geo.Utils.ObjectUtil.GetNumeral(row[ParamNames.Z]);
            var rmsedXyz = new RmsedXYZ(new XYZ(x, y, z));
            if (row.ContainsKey(ParamNames.RmsX))
            {
                double rmsX = Geo.Utils.ObjectUtil.GetNumeral(row[ParamNames.RmsX]);
                double rmsY = Geo.Utils.ObjectUtil.GetNumeral(row[ParamNames.RmsY]);
                double rmsZ = Geo.Utils.ObjectUtil.GetNumeral(row[ParamNames.RmsZ]);
                rmsedXyz.Rms = new XYZ(rmsX, rmsY, rmsZ);
            } 

            var time = (Time)row[ParamNames.Epoch];
            return new EstimatedSite(name, rmsedXyz, time); 
        } 
    }
}
