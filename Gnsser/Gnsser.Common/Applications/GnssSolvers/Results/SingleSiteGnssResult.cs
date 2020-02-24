//2014.08.29, czs, edit, 行了继承设计
//2014.12.08, czs, edit, 再次提升为通用定位结果
//2016.10.04, czs, refactor in hongqing, 全新继承设计

using System;
using System.Collections.Generic;
using Gnsser.Times;
using System.Text;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using  Geo.Algorithm.Adjust;
using Geo;
using Geo.Times; 
using Geo.IO;

namespace Gnsser.Service
{

    //2016.10.04, czs, refactor in hongqing, 全新继承设计
    /// <summary>
    /// 单站单历元GNSS结果
    /// </summary>
    public class SingleSiteGnssResult : BaseGnssResult<EpochInformation>, IReadable
    {
        
        /// <summary>
        /// 单站ＧＮＳＳ结果
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <param name="adjust"></param>
        /// <param name="nameBuilder"></param>
        public SingleSiteGnssResult(EpochInformation epochInfo, AdjustResultMatrix adjust, GnssParamNameBuilder nameBuilder, bool isTopSpeed=false)
            : base(epochInfo, adjust, nameBuilder, isTopSpeed)
        {
            if (!isTopSpeed)
            {
                this.DilutionOfPrecision = new DilutionOfPrecision(epochInfo);
            }
        }


        /// <summary>
        /// 测站固定信息
        /// </summary>
        public ISiteInfo SiteInfo { get { return MaterialObj.SiteInfo; } }
        /// <summary>
        /// 初始值
        /// </summary>
        public XYZ ApproxXyz { get { return MaterialObj.SiteInfo.ApproxXyz; } }
          /// <summary>
        /// 设置常见的变量。
        /// </summary>
        /// <param name="Adjustment"></param>
        protected override void TrySetCommonParamValues(AdjustResultMatrix Adjustment)
        {
            base.TrySetCommonParamValues(Adjustment);

            Vector estmated = Adjustment.Estimated;
            if (estmated != null)
            {
                //对流层设置
                //if (Adjustment.ParamNames.Contains(Gnsser.ParamNames.Trop))
                //{
                //    epochInfo.NumeralCorrections[Gnsser.ParamNames.Trop] = estmated[Adjustment.GetIndexOf(Gnsser.ParamNames.Trop)];
                //}
                //需要计算的属性 
                //接收机钟差 
                if (Adjustment.ParamNames.Contains(Gnsser.ParamNames.RcvClkErrDistance))
                {
                    this.RcvClkErrDistance = estmated[Adjustment.GetIndexOf(Gnsser.ParamNames.RcvClkErrDistance)];
                    //log.Debug("取消了接收机钟差改正！");
                    //钟差到改正数，需要转换为相反数 // 2018.06.08, czs, HMX
                    double clkError =  this.RcvClkErrDistance / GnssConst.LIGHT_SPEED;
                    if(Math.Abs(clkError) > GnssConst.LIGHT_SPEED)
                    {
                        log.Warn("钟差太大了吧！ " + clkError);
                    }
                    this.MaterialObj.Time.Correction = -clkError; //钟差变成改正数
                }
            }
        } 
        #region 显示


        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.Material.UnstablePrns.Count > 0)
            {
                sb.Append("CycleSlip:");
                foreach (var item in this.Material.UnstablePrns)
                {
                    sb.Append(item.ToString());
                    sb.Append(" ");
                }
            }
            return base.ToString() + " " + sb.ToString();
        }
     
        /// <summary>
        /// 以简短字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToShortTabValue()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.Append("\t");
            sb.Append(this.SiteInfo.SiteName);
            sb.Append("\t");
            sb.Append(this.ReceiverTime);
            if (EstimatedXyz != null)
            {
                sb.Append("\t");
                sb.Append(this.EstimatedXyz.GetTabValues());
                sb.Append("\t");
                sb.Append(this.EstimatedXyzRms.GetTabValues());
            }
            if (GeoCoord != null)
            {
                sb.Append("\t");
                sb.Append(this.GeoCoord.GetTabValues()); 
            }

            if (this.HasParamAccuracyInfos)
            {
                sb.Append("\t");

                sb.Append(this.ParamAccuracyInfos.GetTabValues());
            }

            sb.Append("\t");
            sb.Append(this.Material.Name);


            return sb.ToString();
        }
        /// <summary>
        /// 以简短标题显示，标准的测站坐标文件！！
        /// </summary>
        /// <returns></returns>
        public override string ToShortTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CalculationTime");
            sb.Append("\t");
            sb.Append(Gnsser.ParamNames.Name);
            sb.Append("\t");
            sb.Append( Gnsser.ParamNames.Epoch);
            sb.Append("\t");
            if (EstimatedXyz != null)
            {
                sb.Append(Gnsser.ParamNames.X);
                sb.Append("\t");
                sb.Append(Gnsser.ParamNames.Y);
                sb.Append("\t");
                sb.Append(Gnsser.ParamNames.Z);
                sb.Append("\t");
                sb.Append(Gnsser.ParamNames.RmsX);
                sb.Append("\t");
                sb.Append(Gnsser.ParamNames.RmsY);
                sb.Append("\t");
                sb.Append(Gnsser.ParamNames.RmsZ); 
            }
            if (GeoCoord != null)
            {
                sb.Append("\t");
                sb.Append(Gnsser.ParamNames.Lon);
                sb.Append("\t");
                sb.Append(Gnsser.ParamNames.Lat);
                sb.Append("\t");
                sb.Append(Gnsser.ParamNames.Height);
            }
            if (this.HasParamAccuracyInfos)
            {
                sb.Append("\t");

                sb.Append(this.ParamAccuracyInfos.GetTabTitles());
            }
            sb.Append("\t");
            sb.Append("FileName");





            return sb.ToString();
        }

        /// <summary>
        /// 参数名称字符串 ParamNames
        /// </summary>
        /// <returns></returns>
        public override string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("测站");
            sb.Append("\t");
            sb.Append("时间");
            sb.Append("\t");
            sb.Append("有上次结果否");
            sb.Append("\t");
            if (EstimatedXyz != null)
            {
                sb.Append("估计坐标" + this.EstimatedXyz.GetTabTitles());
                sb.Append("\t");
                sb.Append("Rms" + this.EstimatedXyzRms.GetTabTitles());
                sb.Append("\t");
                sb.Append("平差估值" + this.XyzCorrection.GetTabTitles());

                sb.Append("\t");
                sb.Append(Material.GetTabTitles());
                sb.Append("\t");
                sb.Append("估计值" + this.EstimatedXyz.GetTabTitles());
            }
            //sb.Append("平差" + ResultMatrix.GetTabTitles());


            if (this.HasParamAccuracyInfos)
            {
                sb.Append("\t");

                sb.Append(this.ParamAccuracyInfos.GetTabTitles());                     
            }


            return sb.ToString();
        }

        /// <summary>
        /// 以制表符分开的值为 Adjustment.Corrected
        /// </summary>
        /// <returns></returns>
        public override string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MaterialObj.SiteInfo.SiteName);
            sb.Append("\t");
            sb.Append(Material.ReceiverTime.ToString());
            //sb.Append(this.HasPreviousResult);
            //sb.Append("\t");
            if (EstimatedXyz != null)
            {
                sb.Append("\t");
                sb.Append(this.EstimatedXyz.GetTabValues());
                sb.Append("\t");
                sb.Append(this.EstimatedXyzRms.GetTabValues());
                sb.Append("\t");
                sb.Append(this.XyzCorrection.GetTabValues());

                sb.Append("\t");
                sb.Append(Material.GetTabValues());
                sb.Append("\t");
                sb.Append(this.EstimatedXyz.GetTabValues()); 
            }

            if (this.HasParamAccuracyInfos)
            {
                sb.Append("\t");

                sb.Append(this.ParamAccuracyInfos.GetTabValues());
            }

            //  sb.Append(ResultMatrix.GetTabValues());
            return sb.ToString();
        }

        public string ToReadableText(string splitter = ",")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("天线：" + this.SiteInfo.Antenna);
            sb.AppendLine("概略 XYZ： " + this.ApproxXyz.ToString());
            if (this.EstimatedXyz != null)
            {
                sb.AppendLine("估值XYZ：" + this.EstimatedXyz.ToString() + ", RMS： " + this.EstimatedXyzRms.ToString());
                var correcVector = (this.EstimatedXyz - this.ApproxXyz);
                var coEnu = Geo.Coordinates.CoordTransformer.LocaXyzToEnu(correcVector, this.ApproxXyz);

                sb.AppendLine("与概略坐标偏差 XYZ： " + correcVector.ToString() + ", 长度： " + Geo.Utils.StringUtil.GetReadableDistance(correcVector.Length)
                    + ", NEU： " + coEnu.ToString() + ", 长度： " + Geo.Utils.StringUtil.GetReadableDistance(coEnu.Length));
                sb.AppendLine("估值大地坐标： " + this.GeoCoord.ToString());
            }

            return sb.ToString();
        }

        #endregion

    }
}