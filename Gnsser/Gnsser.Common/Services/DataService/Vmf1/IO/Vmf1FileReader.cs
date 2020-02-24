using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Algorithm;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser; 
using Geo;
using Geo.Common;
using Gnsser.Times;

namespace Gnsser.Data
{
    /// <summary>
    /// 读取器
    /// </summary>
    public class Vmf1FileReader
    {
        string path;
        /// <summary>
        /// 读取器
        /// </summary>
        /// <param name="filePath"></param>
        public Vmf1FileReader(string filePath)
        {
            this.path = filePath;
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public Vmf1File Read()
        {
            if (!File.Exists(path)) return null;

            List<Vmf1Value> staInfo = new List<Vmf1Value>();
            bool isEnd = true;

            using (StreamReader sr = new StreamReader(path))
            {
                //string line = sr.ReadLine();
                while(isEnd)
                {
                    string line = sr.ReadLine();
                    if (line == null || line == "") 
                        break;
                    if(line.Length > 255)
                    {
                        throw new Exception("Line too long");
                    }
                    if(line.Length == 0)
                    {
                        isEnd = false;
                    }
                    string stanam = StringUtil.SubString(line, 0, 8).Trim();
                    double mjd = double.Parse(StringUtil.SubString(line, 10, 8).Trim());
                    double ah = double.Parse(StringUtil.SubString(line, 20, 10).Trim());
                    double aw = double.Parse(StringUtil.SubString(line, 32, 10).Trim());
                    double hzd = double.Parse(StringUtil.SubString(line, 44, 6).Trim());
                    double wzd = double.Parse(StringUtil.SubString(line, 52, 6).Trim());
                    double meantemKe = double.Parse(StringUtil.SubString(line, 60, 5).Trim());
                    double pre = double.Parse(StringUtil.SubString(line, 67, 7).Trim());
                    double temCe = double.Parse(StringUtil.SubString(line, 76, 7).Trim());
                    double watpre = double.Parse(StringUtil.SubString(line, 85, 5).Trim());
                    double ortHeight = double.Parse(StringUtil.SubString(line, 91, 6).Trim());

                    Vmf1Value data = new Vmf1Value(stanam, mjd, ah, aw, hzd, wzd, meantemKe, pre, temCe, watpre, ortHeight);
                    staInfo.Add(data);
                }
                return new Vmf1File(staInfo);
            }
        }
    }
}
