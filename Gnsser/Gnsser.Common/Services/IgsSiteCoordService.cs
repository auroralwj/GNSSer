//2018.06.05, czs, create in hmx, IGS 测站坐标服务


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Coordinates;
using Geo.Times;
using System.IO;

namespace Gnsser.Services
{
    /// <summary>
    /// IGS 测站坐标服务，解析自默认坐标文件
    /// </summary>
    public class IgsSiteCoordService : ISiteCoordService
    {
        private static IgsSiteCoordService instance = new IgsSiteCoordService();
        /// <summary>
        /// IGS 测站坐标服务，解析自默认坐标文件
        /// </summary>
        static public IgsSiteCoordService Instance
        {
            get { return instance; }
        }

        private IgsSiteCoordService()
        {
            var path = Setting.GnsserConfig.SiteCoordFile;
            SiteCoordService = new SiteCoordService(path);
            SiteCoordService.Init();
            Name = "IGS 坐标服务 " + Path.GetFileName(path);
        }

        SiteCoordService SiteCoordService;

        /// <summary>
        /// 服务时段
        /// </summary>
        public BufferedTimePeriod TimePeriod { get { return BufferedTimePeriod.MaxPeriod; } set { } }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 测站信息
        /// </summary>
        public List<string> SiteNames
        {
            get
            {
                return SiteCoordService.SiteNames;
            }
        }
        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <param name="name"></param>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public RmsedXYZ Get(string name, Time epoch)
        {
            return SiteCoordService.Get(name, epoch);
        }
        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public RmsedXYZ Get(string name)
        {
            return SiteCoordService.Get(name);
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
