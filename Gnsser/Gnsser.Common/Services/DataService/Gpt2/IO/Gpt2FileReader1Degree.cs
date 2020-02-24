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
    public class Gpt2FileReader1Degree
    {
        string path;
        /// <summary>
        /// 读取器
        /// </summary>
        /// <param name="filePath"></param>
        public Gpt2FileReader1Degree(string filePath)
        {
            this.path = filePath;
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public Gpt2File1Degree Read()
        {
            if (!File.Exists(path)) return null;

            List<Gpt2Value1Degree> Gpt2Info = new List<Gpt2Value1Degree>();
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
                    if(line.Length > 500)
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

                    la la = new la();
                    la.a0 = double.Parse(values[34]);
                    la.A1 = double.Parse(values[35]);
                    la.B1 = double.Parse(values[36]);
                    la.A2 = double.Parse(values[37]);
                    la.B2 = double.Parse(values[38]);

                    Tm Tm = new Tm();
                    Tm.a0 = double.Parse(values[39]);
                    Tm.A1 = double.Parse(values[40]);
                    Tm.B1 = double.Parse(values[41]);
                    Tm.A2 = double.Parse(values[42]);
                    Tm.B2 = double.Parse(values[43]);

                    Gpt2Value1Degree data = new Gpt2Value1Degree(lat, lon, pre, T, Q, dT, undu, Hs, ah, aw, la,Tm);
                    Gpt2Info.Add(data);
                }
                return new Gpt2File1Degree(Gpt2Info);
            }
        }
    }
}
