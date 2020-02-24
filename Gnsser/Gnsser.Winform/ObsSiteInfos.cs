//2019.01.17, czs, create in hmx,观测文件集合

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Gnsser;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using Gnsser.Service;
using Geo.IO;
using Geo;
using Geo.Winform;
using System.Threading;
using Geo.Draw;


namespace Gnsser.Winform
{
    /// <summary>
    /// 观测文件集合
    /// </summary>
    public class ObsSiteInfos
    {
        public ObsSiteInfos() { }
        public ObsSiteInfos(List<ObsSiteInfo> obsSiteInfos)
        {
            this.obsSiteInfos = obsSiteInfos;
        }
        List<ObsSiteInfo> obsSiteInfos { get; set; }
        public List<string> GetFilePathes()
        {
            List<string> pathes = new List<string>();
            foreach (var item in obsSiteInfos)
            {
                pathes.Add(item.FilePath);
            }
            return pathes;
        }
        public ObsSiteInfo GetFirst()
        {
            foreach (var item in obsSiteInfos)
            {
                return item;
            }
            return null;
        }

        public ObsSiteInfo Get(string path)
        {
            foreach (var item in obsSiteInfos)
            {
                if (item.FilePath == path)
                {
                    return item;
                }
            }
            return null;

        }
    }
}
