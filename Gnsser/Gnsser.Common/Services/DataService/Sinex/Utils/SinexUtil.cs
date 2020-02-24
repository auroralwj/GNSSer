//2015.04.16, czs , edit in namu, CyTest 修改为 SinexUtil

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Algorithm.Adjust;
using Gnsser.Data.Sinex;
using Geo.Algorithm;
//using Geo.Algorithm.CyMatrix;
using System.Diagnostics;//Added for the stopwatch
using System.Threading.Tasks; 

namespace Gnsser.Service
{
    /// <summary>
    /// 实用工具
    /// </summary>
    public class SinexUtil
    {
        /// <summary>
        /// 从 Sinex 文件中提取 XYZ 坐标。
        /// </summary>
        /// <param name="Site"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<XYZ> GetEstXYZ(string[] Site, string path = "D:\\Test\\igs13p17212.SNX")
        {
            SinexFile pubSinexFile = SinexReader.Read(path);
            List<string> pubSiteCode = pubSinexFile.GetSiteCods();
            double[] pubAllEst = pubSinexFile.GetEstimateVector();
            int m = Site.Length;
            List<XYZ> pubXYZ = new List<XYZ>();
            for (int i = 0; i < m; i++)
            {
                string item = Site[i];
                for (int j = 0; j < pubSiteCode.Count; j++)
                {
                    if (pubSiteCode[j].ToUpper() == item.ToUpper())
                    {
                        XYZ xyz = new XYZ();
                        xyz.Site = item;
                        xyz.X = pubAllEst[3 * j + 0];
                        xyz.Y = pubAllEst[3 * j + 1];
                        xyz.Z = pubAllEst[3 * j + 2];
                        pubXYZ.Add(xyz);
                    }
                }
            }
            return pubXYZ;
        }
    }
}
