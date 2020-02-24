//2014.12.03, czs, create in 金信粮贸 shuangliao, 星历文件服务
//2014.12.16, czs, edit in namu shuangliao, 多系统星历服务初步
//2014.12.26, czs, edit in namu, 命名为 Sp3PathBuilder
//2015.12.09, czs, edit in hongqing, 命名为 IgsProductLocalPathBuilder
//2015.12.22, czs, edit in hongqing,增加系统无关地址生成。
//2018.05.12, czs, edit in hmx, 输入目录修改为可以为多个目录

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gnsser; 
using Gnsser.Service;  
using Geo.Utils;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using System.Threading; 
using Geo.Common;
using Geo.Times;
using Gnsser.Times; 
using Geo.Algorithm;
using Gnsser.Data;


namespace Gnsser
{
    /// <summary>
    ///  IGS产品本地路径生成器。并不检查存在性。
    ///  若无系统则，采用系统为 U（Unkown）
    /// </summary>
    public class IgsProductLocalPathBuilder : ISatTypeTimeBasedService<List<string>>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public IgsProductLocalPathBuilder()
        {

        }
        /// <summary>
        /// 常用构造函数
        /// </summary>
        /// <param name="productDirectory">星历目录</param>
        /// <param name="sourceNames">星历文件数据源字典</param>
        /// <param name="type">类型</param>
        /// <param name="IsWeekly">是否为周解</param>
        public IgsProductLocalPathBuilder(string [] productDirectory, List<string> sourceNames = null, IgsProductType type = IgsProductType.Sp3, bool IsWeekly =false)
        {
            this.IsWeekly = IsWeekly;
            this.IgsProductSourceType = type;
            this.ProductDirectories = productDirectory;
            
            //Geo.Utils.FileUtil.CheckOrCreateDirectory(productDirectory);
            //   if (!Directory.Exists(productDirectory)) throw new DirectoryNotFoundException("星历目录未发现！" + productDirectory);

                this.SourceNameDic = new Dictionary<SatelliteType, List<string>>();
            if (sourceNames != null)
            {
                this.SourceNameDic.Add(SatelliteType.U, sourceNames);
            }
            else
            {
                this.SourceNameDic[SatelliteType.U].AddRange( new string[]{ "wum", "igs"}); 
            }
        }

        /// <summary>
        /// 常用构造函数
        /// </summary>
        /// <param name="productDirectory">星历目录</param>
        /// <param name="sourceNameDic">星历文件数据源字典</param>
        /// <param name="type">类型</param>
        /// <param name="IsWeekly">是否为周解</param>
        public IgsProductLocalPathBuilder(string [] productDirectory, Dictionary<SatelliteType, List<string>> sourceNameDic = null, IgsProductType type = IgsProductType.Sp3, bool IsWeekly = false)
        {
            this.IsWeekly = IsWeekly;
            this.IgsProductSourceType = type;
            this.ProductDirectories = productDirectory;
            //Geo.Utils.FileUtil.CheckOrCreateDirectory(productDirectory);
            //   if (!Directory.Exists(productDirectory)) throw new DirectoryNotFoundException("星历目录未发现！" + productDirectory);

            if (sourceNameDic != null)
            {
                this.SourceNameDic = sourceNameDic;
            }
            else
            {
                this.SourceNameDic = new Dictionary<SatelliteType, List<string>>();
                this.SourceNameDic.Add(SatelliteType.G, new List<string>());
                this.SourceNameDic.Add(SatelliteType.C, new List<string>());
                this.SourceNameDic[SatelliteType.C].Add("wum");
                this.SourceNameDic[SatelliteType.G].Add("qzf");
                this.SourceNameDic[SatelliteType.G].Add("igs");
            }
        }

        #region 属性
        /// <summary>
        /// 是否是周为周期
        /// </summary>
        public bool IsWeekly { get; set; }
        /// <summary>
        /// 多系统数据源字典
        /// </summary>
        protected Dictionary<SatelliteType, List<string>> SourceNameDic{ get; set; }

        /// <summary>
        /// 星历目录
        /// </summary>
        public string [] ProductDirectories { get; set; }
        /// <summary>
        /// IGS 产品数据源类型
        /// </summary>
        public IgsProductType IgsProductSourceType { get; set; }
        #endregion

        #region 服务获取
        /// <summary>
        /// 负责生成  路径，并不检查路径的存在性。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<string> Get(Time time)
        {
            return Get(SatelliteType.U, time);
        }

        /// <summary>
        /// 负责生成  路径，并不检查路径的存在性。
        /// </summary>
        /// <param name="satelliteType">系统类型</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public List<string> Get(SatelliteType satelliteType, Time time)
        {
            List<string> pathes = new List<string>();
            if (SourceNameDic.ContainsKey(satelliteType))
            {
                List<string> sourceNames = SourceNameDic[satelliteType];
                foreach (var name in sourceNames)
                {
                    var fileNames = new IgsProductNameBuilder(name, IgsProductSourceType, IsWeekly).Get(time).FilePathes;
                    foreach (var fileName in fileNames)
                    {
                        foreach (var dir in ProductDirectories)
                        { 
                            string sp3Path = Path.Combine(dir, fileName);
                            pathes.Add(sp3Path);
                        }               
                    }
                }
            }

            return pathes;
        }

        #endregion
    }
}