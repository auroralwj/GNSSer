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
    public class Gpt2FileReader
    {
        string path;
        /// <summary>
        /// 读取器
        /// </summary>
        /// <param name="filePath"></param>
        public Gpt2FileReader(string filePath)
        {
            this.path = filePath;
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public Gpt2File Read()
        {
            if (!File.Exists(path)) return null;

            List<Gpt2Value>  Gpt2Info = new List<Gpt2Value>();
            bool isEnd = true;

            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine();
                //string line = sr.ReadLine();
                while(isEnd)
                {
                    line = sr.ReadLine();
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

                    string[] values = StringUtil.SplitByBlank(line);
                    double lat = double.Parse(values[0]);
                    double lon = double.Parse(values[1]);
                    pressure pre = new pressure();
                    pre.a0 = double.Parse(values[2]);
                    pre.A1 = double.Parse(values[3]);
                    pre.B1 = double.Parse(values[4]);
                    pre.A2 = double.Parse(values[5]);
                    pre.B2 = double.Parse(values[6]);

                    T T = new T();
                    T.a0 = double.Parse(values[7]);
                    T.A1 = double.Parse(values[8]);
                    T.B1 = double.Parse(values[9]);
                    T.A2 = double.Parse(values[10]);
                    T.B2 = double.Parse(values[11]);

                    Q Q = new Q();
                    Q.a0 = double.Parse(values[12]) / 1000;
                    Q.A1 = double.Parse(values[13]) / 1000;
                    Q.B1 = double.Parse(values[14]) / 1000;
                    Q.A2 = double.Parse(values[15]) / 1000;
                    Q.B2 = double.Parse(values[16]) / 1000;

                    dT dT = new dT();
                    dT.a0 = double.Parse(values[17]) / 1000;
                    dT.A1 = double.Parse(values[18]) / 1000;
                    dT.B1 = double.Parse(values[19]) / 1000;
                    dT.A2 = double.Parse(values[20]) / 1000;
                    dT.B2 = double.Parse(values[21]) / 1000;

                    double undu = double.Parse(values[22]);
                    double Hs = double.Parse(values[23]);

                    ah ah = new ah();
                    ah.a0 = double.Parse(values[24]) / 1000;
                    ah.A1 = double.Parse(values[25]) / 1000;
                    ah.B1 = double.Parse(values[26]) / 1000;
                    ah.A2 = double.Parse(values[27]) / 1000;
                    ah.B2 = double.Parse(values[28]) / 1000;

                    aw aw = new aw();
                    aw.a0 = double.Parse(values[29]) / 1000;
                    aw.A1 = double.Parse(values[30]) / 1000;
                    aw.B1 = double.Parse(values[31]) / 1000;
                    aw.A2 = double.Parse(values[32]) / 1000;
                    aw.B2 = double.Parse(values[33]) / 1000;


                    //double lat = double.Parse(StringUtil.SubString(line, 0, 5).Trim());
                    //double mjd = double.Parse(StringUtil.SubString(line, 10, 8).Trim());
                    //double ah = double.Parse(StringUtil.SubString(line, 20, 10).Trim());
                    //double aw = double.Parse(StringUtil.SubString(line, 32, 10).Trim());
                    //double hzd = double.Parse(StringUtil.SubString(line, 44, 6).Trim());
                    //double wzd = double.Parse(StringUtil.SubString(line, 52, 6).Trim());
                    //double meantemKe = double.Parse(StringUtil.SubString(line, 60, 5).Trim());
                    //double pre = double.Parse(StringUtil.SubString(line, 67, 7).Trim());
                    //double temCe = double.Parse(StringUtil.SubString(line, 76, 7).Trim());
                    //double watpre = double.Parse(StringUtil.SubString(line, 85, 5).Trim());
                    //double ortHeight = double.Parse(StringUtil.SubString(line, 91, 6).Trim());
                   // 
                    Gpt2Value data = new Gpt2Value(lat, lon, pre, T, Q, dT, undu, Hs, ah, aw);
                    Gpt2Info.Add(data);
                }
                return new Gpt2File(Gpt2Info);
            }
        }
    }
}
