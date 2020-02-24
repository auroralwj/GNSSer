//2014.09.24, czs, create, 卫星信息读取器
//2018.05.02, czs, hmx, 重构，采用新算法获取服务

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
using Geo.Times;
using Geo;
using Geo.IO;

namespace Gnsser.Data
{

    /// <summary>
    /// 卫星信息读取器
    /// </summary>
    public class FileErpService : AbstractErpService, IErpFileService
    {
        Log log = new Log(typeof(FileErpService));
        /// <summary>
        /// ERP读取与服务
        /// </summary>
        public FileErpService()
        {
        }
        /// <summary>
        /// ERP 文件服务
        /// </summary>
        public static FileErpService Empty => new FileErpService();

        /// <summary>
        /// ERP读取与服务
        /// </summary>
        /// <param name="filePath"></param>
        public FileErpService(string filePath)
        { 
            //不直接读取
            ErpFileReader reader = new ErpFileReader(filePath);
            this.ErpFile = reader.Read();
        }
        /// <summary>
        /// ERP读取与服务
        /// </summary>
        /// <param name="ErpFile"></param>
        public FileErpService(ErpFile ErpFile)
        {
            this.ErpFile = ErpFile; 
        } 

        /// <summary>
        /// 服务的时段信息
        /// </summary>
        public override BufferedTimePeriod TimePeriod { get { if (ErpFile == null) return BufferedTimePeriod.Zero; return ErpFile.TimePeriod; } set { } }

        private ErpFile ErpFile { get; set; }
        /// <summary>
        /// 是否为空
        /// </summary>
        public override  bool IsEmpty => ErpFile == null;

        /// <summary>
        /// 根据历元时刻查找相应的IGS发布的ERP信息
        /// ERP信息是读取某周的ERP文件
        /// </summary>
        /// <param name="date">历元时刻</param>
        /// <returns></returns>
        public override ErpItem Get(Time date)
        {
            if (this.ErpFile == null) { return  ErpItem.Zero; }

            var result = this.GetErpItem(date);
            return result;
            var result2 = GetErp(date);
        }

        /// <summary>
        /// 获取服务，新算法 //2018.05.02, czs, hmx
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ErpItem GetErpItem(Time date)
        {
            //以下为具体算法
            if (!this.TimePeriod.Contains(date))
            {
                return ErpItem.Zero;
            }

            double mjd = (double)date.MJulianDays;

            var list = Geo.Utils.DoubleUtil.GetNearst(ErpFile.Keys, mjd, true);
            var nearst = ErpFile[list[0]];
            var result = nearst.Clone();
            result.Mjd = mjd;
            if (list.Count == 1)//刚好等于，或者在边界
            {

                double day = mjd - nearst.Mjd;
                result.Xpole += nearst.Xrt * day;
                result.Ypole += nearst.Yrt * day;
                result.Ut12Utc -= nearst.Lod * day;

                return result;
            }
            //下面应该有3个值

            var smallerMjd = list[1];
            var largerMjd = list[2];
            var smaller = ErpFile[smallerMjd];
            var larger = ErpFile[largerMjd];

            result.Xpole = Geo.Utils.DoubleUtil.Interpolate(mjd, smallerMjd, largerMjd, smaller.Xpole, larger.Xpole);
            result.Ypole = Geo.Utils.DoubleUtil.Interpolate(mjd, smallerMjd, largerMjd, smaller.Ypole, larger.Ypole);
            result.Ut12Utc = Geo.Utils.DoubleUtil.Interpolate(mjd, smallerMjd, largerMjd, smaller.Ut12Utc, larger.Ut12Utc);

            return result;
        }

        #region 崔阳老算法
        /// <summary>
        /// 一周的ERP参数信息
        /// </summary>
        public Dictionary<double, ErpItem> Erps { get { return ErpFile.Erps; } }
        /// <summary>
        /// 崔阳老算法。
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private ErpItem GetErp(Time date)
        {
            //以下为具体算法

            double mjd = (double)date.MJulianDays;

            Time ep = new Time(2000, 1, 1, 12);

            double mjd0 = 51544.5 + (double)(date - ep) / 86400.0;

            if (mjd != mjd0)
                mjd = mjd0;


            if (Erps.Count == 0)
            {
                return ErpItem.Zero;
            }
            ErpItem erpItem = Erps.ElementAt(0).Value;

            if (mjd <= Erps.ElementAt(0).Value.Mjd - mjd)
            {
                if (Erps.ElementAt(0).Value.Mjd - mjd <= 1.5)
                {
                    double day = mjd - Erps.ElementAt(0).Value.Mjd;

                    erpItem = Erps.ElementAt(0).Value;

                    erpItem.Xpole += erpItem.Xrt * day;

                    erpItem.Ypole += erpItem.Yrt * day;

                    erpItem.Ut12Utc -= erpItem.Lod * day;

                    return erpItem;
                }
                else
                {
                    erpItem = new ErpItem();
                    return erpItem;
                }
            }
            if (mjd >= Erps.ElementAt(Erps.Count - 1).Value.Mjd)
            {
                if (mjd - Erps.ElementAt(0).Value.Mjd <= 1.5)
                {
                    double day = mjd - Erps.ElementAt(Erps.Count - 1).Value.Mjd;

                    erpItem = Erps.ElementAt(Erps.Count - 1).Value;

                    erpItem.Xpole += erpItem.Xrt * day;

                    erpItem.Ypole += erpItem.Yrt * day;

                    erpItem.Ut12Utc -= erpItem.Lod * day;

                    return erpItem;
                }
                else
                {
                    erpItem = new ErpItem();
                    return erpItem;
                }
            }


            int i = 0, j, k;
            for (j = 0, k = Erps.Count - 1; j <= k; )
            {
                i = (j + k) / 2;
                if (mjd < Erps.ElementAt(i).Value.Mjd) k = i - 1; else j = i + 1;
            }

            for (i = 0; i < Erps.Count - 1; i++)
            {
                double t0 = Erps.ElementAt(i).Value.Mjd;
                double t1 = Erps.ElementAt(i + 1).Value.Mjd;
                if (t0 < mjd && t1 > mjd)
                {
                    j = i;
                    break;
                }
            }

            //add by czs, 2016.06.29
            if (j + 1 > 6)
            {
                j = 5;
            }

            double a = 0.5;
            if (Erps.ElementAt(j).Value.Mjd == mjd - Erps.ElementAt(j + 1).Value.Mjd)
            {
                a = 0.5;
            }
            else
            {
                a = (mjd - Erps.ElementAt(j + 1).Value.Mjd) / (Erps.ElementAt(j).Value.Mjd - Erps.ElementAt(j + 1).Value.Mjd);
            }


            erpItem = Erps.ElementAt(j).Value;


            erpItem.Xpole = (1.0 - a) * Erps.ElementAt(j).Value.Xpole + a * Erps.ElementAt(j + 1).Value.Xpole;
            erpItem.Ypole = (1.0 - a) * Erps.ElementAt(j).Value.Ypole + a * Erps.ElementAt(j + 1).Value.Ypole;
            erpItem.Ut12Utc = (1.0 - a) * Erps.ElementAt(j).Value.Ut12Utc + a * Erps.ElementAt(j + 1).Value.Ut12Utc;
            erpItem.Lod = (1.0 - a) * Erps.ElementAt(j).Value.Lod + a * Erps.ElementAt(j + 1).Value.Lod;



             
            return erpItem;
        }

        #endregion
    }//End SatDataReader
}
