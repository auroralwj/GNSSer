//2015.06.05, czs, create in namu, 文件服务

 using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo;
using Geo.Coordinates;

namespace Geo.Data
{
    public class GamitOrgFileService : FileDataService<GlobkOrgItem, GlobkOrgCoordFile>  
    {
        /// <summary>
        /// 文件数据源构造函数。
        /// </summary>
        /// <param name="Option">文件选项</param>
        /// <param name="name">名称</param>
        public GamitOrgFileService(string Option, string name = "文件数据源")
            : base(Option, name)
        {
            this.Reader = new GamitOrgFileReader(this.Option.FilePath);
        }
        /// <summary>
        /// 命名的XYZ坐标列表
        /// </summary>
        /// <returns></returns>
        public List<NamedXyz> GetNamedXyzs()
        {
            var xyzs = new List<NamedXyz>();

            var items = GetItems();
            foreach (var item in items)
            {
                xyzs.Add(new NamedXyz(item.Id, new XYZ(item.X, item.Y, item.Z)));
            }
            return xyzs;
        }
    }
}
