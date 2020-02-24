//2014.10.22,cuiyang, DCB文件读取

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
using Geo.IO;

namespace Gnsser.Data
{
    /** T差分码偏差是不同类型的两种伪距观测量之间的相对硬件延迟。
     * CODE每月发布DCB改正参数文件
     * DCB大小通常为-10ns到10ns
      */
    /// <summary>
    /// DCB信息读取器
    /// </summary>
    public class DcbFileReader
    {
        string path;
        ILog log = new Log(typeof(DcbFileReader));
        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public DcbFileReader(string filePath)
        {
            this.path = filePath;
            // this.PeriodSatInfos =  Read(filePath);
        }

        /// <summary>
        /// 读取DCB信息。
        /// 由于信息文件较小，这里一次性读取完毕。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public DcbFile Read()
        {
            if (!File.Exists(path))
            {
                log.Info("DCB文件不存在！" +　path);
                return null;
            }

            log.Info("载入：" +path);
            List<DcbValue> sats = new List<DcbValue>();

            //Do this until end-of-file reached or something else happens
            bool isEnd = true;
            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                while (isEnd)
                {
                    line = sr.ReadLine();

                    if (line == null || line=="") break;
                    if (line.Length == 0)
                    { isEnd = false; }
                    //If line is too long, we throw an exception
                    if (line.Length > 255)
                    { throw new Exception("Line too long"); }
                    // We erase the header (prevObj line)

                    if (StringUtil.firstWord(ref line) == "***")
                    {
                        continue;
                    }

                    // Let's start to get satData out of file PRN number
                    string prnString = line.Substring(0, 3);// StringUtil.TrimFirstWord(ref line);
                    string value = line.Substring(26, 9);// StringUtil.TrimFirstWord(ref line);
                    string rms = line.Substring(38, 9);// StringUtil.TrimFirstWord(ref line);                 

                    // Get satellite id. If it doesn't fit GPS or Glonass, it is
                    // marked as unknown
                    SatelliteNumber prn = new SatelliteNumber();
                    prn.PRN = Convert.ToInt32(prnString.Substring(1,2));
                    prn.SatelliteType = SatelliteType.U;// ??
                    // Let's identify satellite system
                    if (prnString[0] == 'G')
                    {

                        prn.SatelliteType = SatelliteType.G;//GPS
                    }
                    else
                    {
                        if (prnString[0] == 'R')
                        {
                            prn.SatelliteType = SatelliteType.R;
                        }
                    }

                    DcbValue data = new DcbValue(prn, Convert.ToDouble(value.Trim()), Convert.ToDouble(rms.Trim())); 

                    //Insert satData in satData map
                    sats.Add(data);
                }
            }
            return new DcbFile(sats);
        }

        public DcbFile ReadP2C2()
        {
            if (!File.Exists(path))
            {
                log.Info("DCB文件不存在！" + path);
                return null;
            }

            log.Info("载入：" + path);
            List<DcbValue> sats = new List<DcbValue>();

            //Do this until end-of-file reached or something else happens
            bool isEnd = true;
            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                while (isEnd)
                {
                    line = sr.ReadLine();

                    if (line == null || line == "") break;
                    if (line.Length == 0)
                    { isEnd = false; }
                    //If line is too long, we throw an exception
                    if (line.Length > 255)
                    { throw new Exception("Line too long"); }
                    // We erase the header (prevObj line)

                    if (StringUtil.firstWord(ref line) == "***")
                    {
                        continue;
                    }

                    // Let's start to get satData out of file PRN number

                    string prnString = null;
                    string value = null;
                    string rms = null;
                    string staname = null;
                    string[] infos =  StringUtil.SplitByBlank(line);
                    if(infos.Length == 3)
                    {
                        prnString = line.Substring(0, 3);// StringUtil.TrimFirstWord(ref line);
                        value = line.Substring(26, 9);// StringUtil.TrimFirstWord(ref line);
                        rms = line.Substring(38, 9);// StringUtil.TrimFirstWord(ref line);
                    }
                    else if (infos.Length == 4)
                    {
                        prnString = line.Substring(0, 1);// StringUtil.TrimFirstWord(ref line);
                        staname = line.Substring(5, 6);//station name
                        value = line.Substring(26, 9);// StringUtil.TrimFirstWord(ref line);
                        rms = line.Substring(38, 9);// StringUtil.TrimFirstWord(ref line);
                        break;
                    }
                    prnString = line.Substring(0, 3);// StringUtil.TrimFirstWord(ref line);
                    value = line.Substring(26, 9);// StringUtil.TrimFirstWord(ref line);
                    rms = line.Substring(38, 9);// StringUtil.TrimFirstWord(ref line);                 

                    // Get satellite id. If it doesn't fit GPS or Glonass, it is
                    // marked as unknown
                    SatelliteNumber prn = new SatelliteNumber();
                    prn.PRN = Convert.ToInt32(prnString.Substring(1, 2));
                    prn.SatelliteType = SatelliteType.U;// ??
                    // Let's identify satellite system
                    if (prnString[0] == 'G')
                    {

                        prn.SatelliteType = SatelliteType.G;//GPS
                    }
                    else
                    {
                        if (prnString[0] == 'R')
                        {
                            prn.SatelliteType = SatelliteType.R;
                        }
                    }

                    DcbValue data = new DcbValue(prn, Convert.ToDouble(value.Trim()), Convert.ToDouble(rms.Trim()));

                    //Insert satData in satData map
                    sats.Add(data);
                }
            }
            return new DcbFile(sats);
        }

    }//End SatDataReader
}
