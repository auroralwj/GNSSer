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
    /// <summary>
    /// pinacal软件支持
    /// </summary>
    public class PinnacleRepFileService : FileDataService<CoordinateRecord, PinnacleRepFile>  
    {
        /// <summary>
        /// 文件数据源构造函数。
        /// </summary>
        /// <param name="Option">文件选项</param>
        /// <param name="name">名称</param>
        public PinnacleRepFileService(string Option, string name = "文件数据源")
            : base(Option, name)
        {
            this.Reader = new PinnacleRepFileReader(this.Option.FilePath);
        }

        public List<Coordinates.NamedXyz> GetNamedXyzs()
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
