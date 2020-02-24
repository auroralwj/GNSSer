//2015.10.09, czs, create in xi'an hongqing, RINEX 格式转换工具

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Utils;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// RINEX 格式转换工具 
    /// </summary>
    public class  ObsFileConverter{

        /// <summary>
        /// 转换写入文件。
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <param name="outVersion"></param>
        public static void ToRinex(string inputPath, string outputPath, double outVersion)
        {
            var obsFile = new RinexObsFileReader(inputPath).ReadObsFile();
            var txt = new RinexObsFileWriter().GetRinexString(obsFile, outVersion);
            File.WriteAllText(outputPath, txt, Encoding.ASCII); 
        }

        /// <summary>
        /// 转换成Gnsser RINEX 3.02 格式。
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        static public void ToRinexV3(string inputPath, string outputPath)
        {
            ToRinex(inputPath, outputPath, 3.02);
        }

        /// <summary>
        /// 转换成Gnsser RINEX 2.11 格式。
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        static public void ToRinexV2(string inputPath, string outputPath)
        {
            ToRinex(inputPath, outputPath, 2.11);
        } 
    }
}
