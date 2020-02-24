//2017.11.06, czs, added, 球谐系数


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Times;
using Geo.Utils;
using Gnsser;
using Geo;
using Geo.IO;

namespace Gnsser.Data
{ 
    /// <summary>
    /// 卫星信息读取器
    /// </summary>
    public class SphericalHarmonicsReader
    {
        ILog log = new Log(typeof(SphericalHarmonicsReader));
        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public SphericalHarmonicsReader(FileOption filePath)
        {
            this.path = filePath.FilePath;
        }
        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public SphericalHarmonicsReader(string filePath)
        {
            this.path = filePath;
        }

        string path; 

        /// <summary>
        /// 读取 
        /// </summary> 
        /// <param name="maxOrder">只读取到相应的阶次，节约时间</param>
        /// <returns></returns>
        public SphericalHarmonicsFile Read(int maxOrder = int.MaxValue)
        {
            SphericalHarmonicsFile file = new SphericalHarmonicsFile();
            
            using (StreamReader sr = new StreamReader(path))
            {
                string line = null;
                SphericalHarmonicsItem current = null;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace("D", "E");
                    string[] strs = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length < 4) { continue; }
                    int n = int.Parse(strs[0]);
                    int m = int.Parse(strs[1]);

                    if (n > maxOrder) { break; }

                    if (m == 0) { current = file.GetOrCreate(n); }// current = new SphericalHarmonicsItem(n + 1); file[n] = current; }
                    var cVal=   double.Parse(strs[2]);
                    var sVal=   double.Parse(strs[3]);
                    var cSigma= 0.0;
                    if (strs.Length > 4) { cSigma = double.Parse(strs[4]); }
                    var sSigma=  0.0;
                    if (strs.Length > 5) { sSigma = double.Parse(strs[5]); }

                    current.Set(m, new RmsedNumeral(cVal, cSigma) ,new RmsedNumeral(sVal, sSigma));
                }
            }
           return file;
          
        }

    }//End SatDataReader
}
