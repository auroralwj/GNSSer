//2016.12.23, lly, edit in zz, 修改，增加内容
//2017.09.10, czs, edit in hongqing, 由于导入的Adjustment常常出错误，因此删除并重新计算之

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using  Geo.Algorithm.Adjust;
using Geo;
using Geo.IO;
using Geo.Algorithm;

namespace Gnsser.Service
{  
    /// <summary>
    /// 衰减因子
    /// </summary>
    public class DilutionOfPrecision : IToTabRow
    {
        /// <summary>
        /// 计算观测值的DOP值。
        /// </summary>
        /// <param name="epochinfo">位置估值</param>
        public DilutionOfPrecision( EpochInformation epochinfo)
        {
            this.EpochInfo = epochinfo;
        }
        private EpochInformation EpochInfo;

        public IMatrix CoeffOfDesign
        {
            get
            {
                int ParamCount = 3 + 1;// +EpochInfo.EnabledSatelliteTypes.Count;
                int ObsCount = EpochInfo.EnabledSatCount;
                Matrix A = new Matrix(ObsCount, ParamCount);
                int satIndex = 0;
                foreach (var sat in EpochInfo.EnabledSats)
                {
                    XYZ vector = sat.EstmatedVector;

                    A[satIndex, 0] = -vector.CosX;
                    A[satIndex, 1] = -vector.CosY;
                    A[satIndex, 2] = -vector.CosZ;
                    A[satIndex, 3] = -1.0;////*** 接收机 钟差改正 in units of meters
                    satIndex++;
                }
                return A.Trans * A; 
            }            

        }
        /// <summary>
        /// 几何精度因子
        /// </summary>
        public double GDOP
        {
            get
            {
                double tmp = Math.Sqrt(CoeffOfDesign[0, 0] + CoeffOfDesign[1, 1] + CoeffOfDesign[2, 2] + CoeffOfDesign[3, 3]);
                
                return tmp;
            }
        }
        /// <summary>
        /// 位置精度衰减因子
        /// </summary>
        public double PDOP
        {
            get
            {
                double tmp = Math.Sqrt(CoeffOfDesign[0, 0] + CoeffOfDesign[1, 1] + CoeffOfDesign[2, 2]);
                return tmp;
            }
        }
        /// <summary>
        /// 水平精度衰减因子
        /// </summary>
        public double HDOP
        {
            get
            {
                double tmp = Math.Sqrt(CoeffOfDesign[0, 0] + CoeffOfDesign[1, 1]);
                return tmp;
            }
        }
        /// <summary>
        /// 垂直精度衰减银子
        /// </summary>
        public double VDOP
        {
            get
            {
                double tmp = Math.Sqrt(CoeffOfDesign[2, 2]);
                return tmp;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("G P H V TDOP:");
            sb.Append(" " + Geo.Utils.StringUtil.FillSpaceLeft(GDOP.ToString("0.000000"), 10) + ",");
            sb.Append(" " + Geo.Utils.StringUtil.FillSpaceLeft(PDOP.ToString("0.000000"), 10) + ",");
            sb.Append(" " + Geo.Utils.StringUtil.FillSpaceLeft(HDOP.ToString("0.000000"), 10) + ",");
            sb.Append(" " + Geo.Utils.StringUtil.FillSpaceLeft(VDOP.ToString("0.000000"), 10) + ",");
            //sb.Append(" " + Geo.Utils.StringUtil.FillSpaceLeft(TDOP.ToString("0.000000"), 10) + ",");  
            return sb.ToString();
        }



        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("GDOP");
            sb.Append("\t");
            sb.Append("PDOP");
            sb.Append("\t");
            sb.Append("HDOP");
            sb.Append("\t");
            sb.Append("VDOP");
            //sb.Append("\t");
            //sb.Append("TDOP"); 

            return sb.ToString();
        }

        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GDOP);
            sb.Append("\t");
            sb.Append(PDOP);
            sb.Append("\t");
            sb.Append(HDOP);
            sb.Append("\t");
            sb.Append(VDOP);
            //sb.Append("\t");
            //sb.Append(TDOP); 
            return sb.ToString();
        }
    }
}
