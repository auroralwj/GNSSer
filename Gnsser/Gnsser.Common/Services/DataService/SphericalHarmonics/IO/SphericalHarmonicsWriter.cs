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
    public class SphericalHarmonicsWriter
    {
        ILog log = new Log(typeof(SphericalHarmonicsWriter));
        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public SphericalHarmonicsWriter(FileOption filePath)
        {
            this.path = filePath.FilePath;
        }
        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public SphericalHarmonicsWriter(string filePath)
        {
            this.path = filePath;
        }

        string path; 

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="maxOrder">只读取到相应的阶次，节约时间</param>
        /// <returns></returns>
        public void Write(SphericalHarmonicsFile file, int maxOrder = int.MaxValue)
        {

            using (StreamWriter wr = new StreamWriter(path))
            {
                var format = "0.000000000000000E+00";
                var sigmaFormat = "0.0000000000E+00";
                
                foreach (var kv in file.Data)
                {
                    var order = kv.Key;
                    if (order > maxOrder) { break; }
                         
                    var n = Geo.Utils.StringUtil.FillSpaceLeft( kv.Key.ToString(),5 );
                    int i = 0;
                    foreach (var item in kv.Value.C)
	               {  
                        var m = Geo.Utils.StringUtil.FillSpaceLeft( i.ToString(),5 );
                        var c =Geo.Utils.StringUtil.FillSpaceLeft(  item.Value.ToString(format), 25);
                        var s =Geo.Utils.StringUtil.FillSpaceLeft(  kv.Value.S[i].Value.ToString(format), 25);

                        var sigmaC = Geo.Utils.StringUtil.FillSpaceLeft(item.Rms.ToString(sigmaFormat), 20);
                        var sigmaS = Geo.Utils.StringUtil.FillSpaceLeft(kv.Value.S[i].Rms.ToString(sigmaFormat), 20);
                        var line = n + m + c + s + sigmaC + sigmaS;
                        wr.WriteLine(line);
                        i++; 
	                }  
                } 
            } 
          
        }

    }//End SatDataReader
}
