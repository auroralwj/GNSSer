//2014.05.22, Cui Yang, created
//2014.07.04, Cui Yang, 增加多映射通用集合类，添加了MultiMap引用

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
using Geo.Times; 
using Geo.Utils;
using Gnsser;

namespace Gnsser.Data
{
    /** This is a class to read and parse satellite satData from
      *  PRN_GPS-like files.
      *
      *
      * Jet Propulsion Laboratory (JPL) provides a file called "PRN_GPS"
      * with satellite information such as launch and deactivation dates,
      * block type GPS number, etc. This information is important for some
      * precise GPS satData processing algorithms, and is used in Gipsy/OASIS
      * software.
      *
      * You may find this file using FTP:
      *
      * ftp://sideshow.jpl.nasa.gov:/pub/gipsy_products/gipsy_params
      *
      * where the PRN_GPS file resides, usually compressed in .gz format.
      *
      * A typical way to use this class follows:
      *
      * @obsCodeode
      *   SatDataReader satread;
      *
      *   SatID prn28(28, SatID::systemGPS);
      *   DayTime time(1995, 331, 43200);
      *
      *   satread.open("PRN_GPS");
      *
      *   string prn28Block = satread.getBlock(prn28, time);
      *   // From 1992 to 1997, PRN 28 belonged to a block IIA satellite
      * @endcode
      *
      * @warning Be aware that PRN numbers are recycled, so several
      * different satellites may have the same PRN number at different
      * epochs. Therefore, you must provide the epoch of interest when
      * calling get methods.
      */
    /// <summary>
    /// 卫星信息读取器
    /// </summary>
    public class SatInfoReader
    {
        string path;

        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public SatInfoReader(string filePath)
        {
            this.path = filePath;
            // this.PeriodSatInfos =  Read(filePath);
        }

        /// <summary>
        /// 读取卫星信息。
        /// 由于卫星信息文件较小，这里一次性读取完毕。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public SatInfoFile Read(string filePath = null)
        {
            if (filePath != null) path = filePath;

            List<SatInfo> sats = new List<SatInfo>();

            //Do this until end-of-file reached or something else happens
            bool isEnd = true;
            using (StreamReader sr = new StreamReader(path))
            {
                while (isEnd)
                {

                    string line = sr.ReadLine();

                    if (line == null) break;

                    //Remove trailing and leading blanks
                    // line = line.Trim();

                    //If line is too long, we throw an exception
                    if (line.Length > 255)
                    { throw new Exception("Line too long"); }

                    //Let's find and strip comments,wherever they are
                    if (StringUtil.firstWord(ref line)[0] == '#')
                    {
                        line = sr.ReadLine();
                    }

                    int idx = line.IndexOf('#');
                    if (idx != -1)//说明找到
                    { line = line.Substring(0, idx); }
                    // We erase the header (prevObj line)
                    if (StringUtil.firstWord(ref line) == "Launch")
                    {
                        line = sr.ReadLine();
                    }


                    //Remove trailing and leading blanks
                    line = line.Trim();
                    //Skip bland lines
                    if (line.Length == 0)
                    { continue; }


                    // Let's start to get satData out of file
                    // Launch date
                    string ldate = StringUtil.TrimFirstWord(ref line);

                    // Deactivation date
                    string ddate = StringUtil.TrimFirstWord(ref line);
                    // GPS number
                    string gnumber = StringUtil.TrimFirstWord(ref line);
                    // PRN number
                    string prnString = StringUtil.TrimFirstWord(ref line);
                    // Block tipe
                    string block = StringUtil.TrimFirstWord(ref line).ToUpper();

                    string orbit = StringUtil.TrimFirstWord(ref line);

                    string clock = line;

                    // Get satellite id. If it doesn't fit GPS or Glonass, it is
                    // marked as unknown
                    SatelliteNumber prn = new SatelliteNumber();
                    prn.PRN = Convert.ToInt32(prnString);
                    prn.SatelliteType = SatelliteType.U;// ??
                    // Let's identify satellite system
                    if (block[0] == 'I')
                    {

                        prn.SatelliteType = SatelliteType.G;//GPS
                    }
                    else
                    {
                        if (block.Substring(0, 3) == "GLO")
                        {
                            prn.SatelliteType = SatelliteType.R;
                        }
                    }

                    SatInfo data = new SatInfo();
                    data.Prn = prn;
                    data.Block = block;
                    data.GpsNumber = Convert.ToInt32(gnumber);
                    // Get launch date in a proper format
                    if (ldate[0] != '0')
                    {
                        data.TimePeriod.Start = Time.Parse(ldate);
                    }
                    // Get deactivation date in a proper format
                    if (ddate[0] != '0')
                    {
                        data.TimePeriod.End= Time.Parse(ddate);
                    } 
                    data.Orbit = orbit;
                    data.Clock = clock;


                    //Insert satData in satData map
                    sats.Add(data);
                }
            }
            return new SatInfoFile(sats);
        }

    }//End SatDataReader
}
