//2015.04, lly, create in zz, ERP 文件
//2015.05.12，czs, edit in namu, 面向对象重构
//2018.04.16， czs, edit in hongqing, 增加行数判断
//2018.06.07, kyc, edit in zz, 增加文件可读范围
//2018.06.09, czs, 改回迭代判断

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
    public class ErpFileReader //: AbstractReader<ErpFile>
    {
        ILog log = new Log(typeof(ErpFileReader));
        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public ErpFileReader(string filePath) //: base(filePath)
        {
            this.FilePath = filePath;
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 读取卫星信息。
        /// 由于卫星信息文件较小，这里一次性读取完毕。
        /// </summary> 
        /// <returns></returns>
        public ErpFile Read()
        {
            ErpFile ErpFile = new ErpFile();
            ErpFile.Name = Path.GetFileName(FilePath);
            //log.Debug("暂未启用ERP文件！");

            //return ErpFile;

            using (StreamReader sr = new StreamReader(FilePath))
            {
                string line = null;
                //前四行没有用
                line = sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                //2018.6.7 kyc add
                if (Path.GetFileName(FilePath).ToString().Contains("cod") || Path.GetFileName(FilePath).ToString().Contains("com"))
                {
                    line = sr.ReadLine();
                    line = sr.ReadLine();
                }
                if (Path.GetFileName(FilePath).ToString().Contains("gfz"))
                    line = sr.ReadLine();


                while ((line = sr.ReadLine()) != null)//czs, 2018.06.09, 改回
                {
                    if (String.IsNullOrEmpty(line)) { continue; }
                //while (!string.IsNullOrEmpty(line = sr.ReadLine()))//kyc: 部分erp文件有无谓的换行符
                //{

                    string[] strs = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    double mjd = 0;
                    if (strs.Length < 16 || !double.TryParse(strs[1], out mjd))
                    {
                        continue;
                    }

                    //此处保留先，ERP加上后，反而精度变得很差//2016.01.29, czs, hongqing
                    //if (strs.Length != 16) 
                    //    continue;

                    ErpItem item = new ErpItem();

                    item.Mjd = double.Parse(strs[0]);

                    item.Xpole = double.Parse(strs[1]) * 1E-6;

                    item.Ypole = double.Parse(strs[2]) * 1E-6;

                    item.Ut12Utc = double.Parse(strs[3]) * 1E-7;

                    item.Lod = double.Parse(strs[4]) * 1E-7;

                    item.Xsig = double.Parse(strs[5]) * 1E-6;

                    item.Ysig = double.Parse(strs[6]) * 1E-6;

                    item.UTsig = double.Parse(strs[7]) * 1E-7;

                    item.LODsig = double.Parse(strs[8]) * 1E-7;

                    item.Nr = double.Parse(strs[9]);

                    item.Nf = double.Parse(strs[10]);

                    item.Nt = double.Parse(strs[11]);


                    item.Xrt = double.Parse(strs[12]) * 1E-6;

                    item.Yrt = double.Parse(strs[13]) * 1E-6;

                    item.Xrtsig = double.Parse(strs[14]) * 1E-6;

                    item.Yrtsig = double.Parse(strs[15]) * 1E-6;





                    ErpFile.Add(item.Mjd, item);
                }
            }
            return ErpFile;

        }

    }//End SatDataReader
}
