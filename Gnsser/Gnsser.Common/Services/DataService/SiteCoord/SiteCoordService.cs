//2016.10.01, czs & double, add in hongqing, 坐标服务
//2018.06.05, czs, edit in hmx, 提取坐标服务接口， 默认懒加载，支持手动预先加载 Init
//2018.12.21, czs, edit in hmx, 取消名称字符限制，采用大写字母

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates; 
using Gnsser.Service; 
using Gnsser.Data;
using Gnsser.Times;
using Gnsser.Data.Rinex;

using Geo.IO;
using Geo; 
using Geo.Times;
using Gnsser.Data.Sinex;

namespace Gnsser
{
    /// <summary>
    /// 坐标服务接口
    /// </summary>
    public interface ISiteCoordService : ITimedService<BufferedTimePeriod>
    {
        /// <summary>
        /// 测站信息
        /// </summary>
        List<string> SiteNames { get; }
     
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="epoch"></param>
        /// <returns></returns>
        RmsedXYZ Get(string name, Time epoch);
        /// <summary>
        /// 忽略历元的坐标服务
        /// </summary>
        /// <param name="name"></param> 
        /// <returns></returns>
        RmsedXYZ Get(string name);

    }

    /// <summary>
    /// 通过 4 个字母的测站名称匹配。
    /// 坐标服务.有待扩展，如使用Bernese坐标文件，自动搜素IGS产品等。2016.10.02.07.59 czs
    /// 默认懒加载，支持手动预先加载 Init
    /// </summary>
    public class SiteCoordService : Named, ISiteCoordService
    {
        /// <summary>
        /// 坐标服务
        /// </summary>
        public SiteCoordService(FileOption option)
        {
            this.FileOption = option;
            this.Name = Path.GetFileName(option.FilePath);
            this.NamedRmsXyzes = new BaseDictionary<string, NamedRmsXyz>();

            log.Info("启用了坐标文件服务。");
        }
        /// <summary>
        /// 坐标服务 
        /// </summary>
        /// <param name="option"></param>
        public SiteCoordService(string option)
            : this(new FileOption(option))
        {
        }

        /// <summary>
        /// 文件选型
        /// </summary>
        FileOption FileOption { get; set; }

        SinexFile SinexFile { get; set; }
        /// <summary>
        /// 字典
        /// </summary>
        public BaseDictionary<string, NamedRmsXyz> NamedRmsXyzes { get; set; }
        static object locker = new object();
        /// <summary>
        /// 服务时段
        /// </summary>
        public  BufferedTimePeriod TimePeriod { get { return BufferedTimePeriod.MaxPeriod; } set { } }
        /// <summary>
        /// 测站信息
        /// </summary>
        public List<string> SiteNames
        {
            get
            {
                return NamedRmsXyzes.Keys;
            }
        }
        /// <summary>
        /// 忽略历元的坐标服务
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public RmsedXYZ Get(string name)
        {
            return Get(name, Time.Default);
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public RmsedXYZ Get(string name, Time epoch)
        {
            if (NamedRmsXyzes == null || NamedRmsXyzes.Count == 0)
            {
                Init();
            }
            if (NamedRmsXyzes == null) { return null; }

            var Name = name.ToUpper();// Geo.Utils.StringUtil.SubString( name.Trim(), 0, 4).ToUpper();
            if (NamedRmsXyzes.Contains(Name))
            {
                return NamedRmsXyzes[Name].Value;
            }
            return null;
        }
        /// <summary>
        /// 初始化加载
        /// </summary>
        public void Init()
        {
            lock (locker)
            {
                if (NamedRmsXyzes == null || NamedRmsXyzes.Count == 0)
                {
                    NamedRmsXyzes = GetCoords(this.FileOption.FilePath);
                    log.Info("加载了 " + NamedRmsXyzes.Count + " 个坐标。");
                }
            }
        }
        /// <summary>
        /// 列表
        /// </summary>
        public List<NamedXyz> NamedXyzs
        {
            get
            {
                List<NamedXyz> result = new List<NamedXyz>();
                foreach (var item in NamedRmsXyzes)
                {
                    result.Add(new NamedXyz(item.Name, item.Value.Value));
                }
                return result;
            }
        }


        /// <summary>
        /// 根据坐标文件读取坐标
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BaseDictionary<string, NamedRmsXyz> GetCoords(string path)
        {
            string extension = Path.GetExtension(path).ToLower();
            switch (extension)
            {
                case ".snx":
                    var SinexFile = SinexReader.Read(path);// SinexFileReader
                    return SinexFile.GetSiteEstimateRmsdCoords();
                    break;
                case ".xls":
                    BaseDictionary<string, NamedRmsXyz> dic = new BaseDictionary<string, NamedRmsXyz>();
                    var list = NamedXyz.ReadNamedXyz(path);
                    foreach (var item in list)
                    {
                        dic[item.Name.Trim().ToUpper()] = new NamedRmsXyz(item.Name, new RmsedXYZ(item.Value));
                    }
                    return dic;
                    break;
                case ".txt":
                    BaseDictionary<string, NamedRmsXyz> dic2 = new BaseDictionary<string, NamedRmsXyz>();
                    var list2 = NamedXyz.ReadNamedXyztxt(path);
                    foreach (var item in list2)
                    {
                        dic2[item.Name.Trim().ToUpper()] = new NamedRmsXyz(item.Name, new RmsedXYZ(item.Value));
                    }
                    return dic2;
                    break;
                default:
                    break;
            }
            return new BaseDictionary<string, NamedRmsXyz>();
        }
    }

}