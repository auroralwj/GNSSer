//2014.05.22, Cui Yang, created
//2018.11.17, czs, edit in hmx, 修复长期以来的解析报错

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Algorithm;
using Geo.Utils;
using Geo.IO;

namespace Gnsser
{ 

    /** This is a class to read and parse ocean tides harmonics satData
       *  in BLQ file format.
       *
       * Ocean loading displacement models usually use the ocean tide
       * harmonics in order to compute station biases due to this effect.
       *
       * A common format to encode such information is the so-called BLQ
       * format, where each station name is associated to a matrix with
       * 11 columns (corresponding to the most important harmonics) and
       * six rows: Three for amplitudes (radial, west, south), and three
       * for phases (radial, west, south).
       *
       * You may find this satData using the "Ocean tide loading provider" at:
       *
       * http://www.oso.chalmers.se/~loading/
       *
       * A typical way to use this class follows:
       *
       * @obsCode
       *   BLQDataReader blqread;
       *
       *   blqread.open("EBRE.GOT00.2");
       *
       *   Matrix<double> tides(6,11,0.0);
       *
       *   tides = blqread.getTideHarmonics("EBRE");
       * @endcode
       *
       * The eleven tide harmonics used are:
       *
       * - M2:  Principal lunar semidiurnal
       * - S2:  Principal solar semidiurnal
       * - N2:  Larger lunar elliptic semidiurnal
       * - K2:  Lunisolar semidiurnal
       * - K1:  Lunar diurnal
       * - O1:  Lunar diurnal
       * - P1:  Solar diurnal
       * - Q1:  Larger lunar elliptic diurnal
       * - MF:  Lunisolar fortnightly
       * - MM:  Lunar monthly
       * - SSA: Solar semiannual
       *
       * @warning Be aware that you may select several different tide models
       * to generate tide harmonics. It is advised to use the latest
       * models such as GOT00.2, FES99, TPXO.6.2, etc.
       */
    public class BLQDataReader
    {
        Log log = new Log(typeof(BLQDataReader));
        /** Common constructor. It will always open file for read and will
         *  load ocean tide harmonics satData in one pass.
         *
         * @param fn   BLQ satData file to read
         *
         */
        public BLQDataReader(string filePath)
        {
            LoadData(filePath);
        }
   
        //Dictionary holding the information regarding oecean tied harmonics
        //private class OceanTidesData : Dictionary<string, tideDataIt> { };

        //OceanTidesData OceanTidesData = new OceanTidesData();

        public Dictionary<string, TideData> OceanTidesData = new Dictionary<string, TideData>();

        /** Method to store ocean tide harmonics satData in this class'
          *  satData map
          *
          * @param stationName String holding station name.
          * @param satData        tideData structure holding the harmonics satData
          */
        private void SetData(ref string stationName,ref TideData datas)
        {
            if (!OceanTidesData.ContainsKey(stationName)) OceanTidesData.Add(stationName, datas); 
        }

        // Method to store ocean tide harmonics satData in this class' satData map
        private void LoadData( string fn)
        {
            StreamReader sr = new StreamReader(fn);

            //Conter of valid satData lines
            int row = 0;
            //store here the station name
            string nameString = "";
            //Declare structure to store tide harmonics satData
            TideData data = new TideData();

            //Do this until end-of-file reached or something else happens
            bool isEnd = true;
            while (isEnd)
            {
                try
                {
                    if (row > 6)
                    {
                        // If row>6, all station harmonics are already read,
                        // so let's store tide satData in satData map
                        SetData(ref nameString,ref data);
                        //Clear harmonics satData
                        data = new TideData();
                        //satData.harmonics = new Matrix(6, 11);

                        //Reset counter to  get satData from an additional station
                        row = 0;
                    }
                    string line = sr.ReadLine();
                    if (line == null) break;


                    //If line is too long, we throw an exception
                    if (line.Length > 255)
                    { throw new Exception("Line too long"); }

                    //Let's find and strip comments,wherever they are
                    if (StringUtil.firstWord(ref line)[0] == '$')
                    {
                        line = sr.ReadLine();
                      if (line == null) break;
                }

                    int idx = line.IndexOf('$');
                    if (idx != -1)//说明找到
                    { line = line.Substring(0, idx); }

                    //Remove trailing and leading blanks
                    line = line.Trim();
                    //Skip bland lines
                    if (line.Length == 0)
                    { continue; }

                    //Let's start to get satData out of file
                    //If this is the prevObj valid line, it contains station name
                    if (row == 0)
                    {
                        nameString = StringUtil.firstWord(ref line).ToUpper();
                        ++row;
                        continue;
                    }
                    else
                    {
                        //2nd to 7th valid lines contains tide harmonics
                        if ((row > 0) && (row <= 6))
                        {
                            for (int col = 0; col < 11; col++)
                            {
                                string value = StringUtil.TrimFirstWord(ref line);
                                data.HarmonicsOfTideData[row - 1, col] = Convert.ToDouble(value);
                            }
                            ++row;
                            continue;
                        }
                    }

                }
                catch (Exception e)
                {
                    log.Error("BLQ 文件数据解析出错（已经修复了啊，是不是格式变了？？？）！" + e.Message);
                    //close this satData stream before returning
                    sr.Close();
                    return;
                }
            }

        }

        /** Method to get the ocean tide harmonics corresponding to a
           *  given station.
           *
           * @param station   Station name (case is NOT relevant).
           *
           * @return A Matrix<double> of siw rows and eleven columns
           * containing tide harmonics M2, S2, N2, K2, K1, O1, P1, Q1, MF,
           * MM and SSA for amplitudes (radial, west, south, in meters) and
           * phases (radial, west, south, in degrees). If station is 
           * not found, this method will return a matrix full of zeros.
           */
        public ArrayMatrix GetTideHarmonics(string station)
        {
            //First, look if such station exist in satData map
            if (OceanTidesData.ContainsKey(station.ToUpper()))
            {
                TideData iter = OceanTidesData[station.ToUpper()];
                return iter.HarmonicsOfTideData;
            }
            else
            {
                // If not, return an empty harmonics matrix
                ArrayMatrix dummy = new ArrayMatrix(6, 11, 0.0);
                return dummy;
            }
        }

    }
}
