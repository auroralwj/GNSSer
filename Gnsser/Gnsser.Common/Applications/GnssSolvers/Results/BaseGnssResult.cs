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
using Geo.Algorithm;
using Geo.Times; 
using Geo.IO;

namespace Gnsser.Service
{

    //2016.10.04, czs, refactor in hongqing, 采用模板重新设计
    /// <summary>
    ///  基于模板的抽象GNSS处理结果。
    /// </summary>
    /// <typeparam name="TMaterial"></typeparam>
    public class BaseGnssResult<TMaterial> : BaseGnssResult
         where TMaterial : ISiteSatObsInfo
    {

        /// <summary>
        /// GNSS结果构造函数
        /// </summary>
        /// <param name="epochInfo">历元信息</param>
        /// <param name="Adjustment">平差信息</param>
        /// <param name="NameBuilder">参数生成器</param>
        public BaseGnssResult(
            TMaterial epochInfo,
            AdjustResultMatrix Adjustment,
            GnssParamNameBuilder NameBuilder,bool isTopSpeed=false)
            : base(epochInfo, Adjustment, NameBuilder)
        {
            this.Name = epochInfo.Name;
            this.MaterialObj = epochInfo;
            if(!isTopSpeed){
                TrySetCommonParamValues(Adjustment);
            }
        }

        /// <summary>
        /// 观测信息,或多站的基准星。
        /// </summary>
        public TMaterial MaterialObj { get; protected set; }


        /// <summary>
        /// 解析名字中的PRN，并判断是否周跳。
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public bool HasUnstablePrn(string paramName)
        {
            if (paramName.Length >= 3)
            {
                var prns = SatelliteNumber.TryParsePrns(paramName, "-", "_");
                foreach (var prn in prns)
                {
                    if (this.Material.UnstablePrns.Contains(prn))
                    { return true; }
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 通用GNSS平差结果。对平差结果进行加工整理。
    /// </summary>
    public abstract class BaseGnssResult : SimpleGnssResult<ISiteSatObsInfo>, IDisposable, IToTabRow
    {
        protected ILog log = new Log(typeof(BaseGnssResult));

        #region 构造函数 
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseGnssResult() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Material">历元信息</param>
        /// <param name="Adjustment">平差信息</param>
        /// <param name="NameBuilder">参数名称</param>
        public BaseGnssResult(
            ISiteSatObsInfo Material,
            AdjustResultMatrix Adjustment,
            GnssParamNameBuilder NameBuilder) : base(Adjustment, Material)
        {
            this.EnabledPrns = Material.EnabledPrns;
            this.NameBuilder = NameBuilder;
            //基础属性 
            this.Name = Material.Name;
        }

        #endregion

        #region 属性 

        /// <summary>
        /// 数据源实体
        /// </summary>
        public override ISiteSatObsInfo Material { get; set; }
        //2015.05.23, cy, edit,基准星
        /// <summary>
        /// 基准卫星。
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }

        /// <summary>
        /// 参数名称生成器
        /// </summary>
        public GnssParamNameBuilder NameBuilder { get; set; }

        /// <summary>
        /// 卫星编号 EnabledPrns
        /// </summary>
        public List<SatelliteNumber> EnabledPrns { get; set; }
         
        /// <summary>
        /// GPS 时间
        /// </summary>
        public override Time ReceiverTime { get { return Material.ReceiverTime; } }
        #endregion

        #region  属性

        #region 需计算或提取转化的属性
        /// <summary>
        /// 是否具有坐标估值，是否进行过定位计算
        /// </summary>
        public bool HasEstimatedXyz { get { return EstimatedXyz != null; } }
        /// <summary>
        /// 接收机位置,最开始是估值，后逐步精化，即始终是当前最精确位置。此坐标直接绑定历元信息。
        /// </summary>
        public XYZ EstimatedXyz { get; protected set; }//{ get { return SiteInfo.EstimatedXyz; } set { SiteInfo.EstimatedXyz = value; } }
   
        /// <summary>
        /// 直接平差估值
        /// </summary>
        public XYZ XyzCorrection { get; set; }
        /// <summary>
        /// 接收机钟差等效距离偏差.
        /// </summary>
        public double RcvClkErrDistance { get; set; }
        /// <summary>
        /// 接收机钟差改正数， //钟差到改正数，需要转换为相反数 // 2018.06.08, czs, HMX
        /// </summary>
        public double RcvClkCorrection { get { return -1.0 * RcvClkError; } }
        /// <summary>
        /// 接收机钟差
        /// </summary>
        public double RcvClkError{ get { return RcvClkErrDistance / GnssConst.LIGHT_SPEED; } } 
        /// <summary>
        /// BLH,大地坐标系。
        /// </summary>
        public GeoCoord GeoCoord { get; set; }
        /// <summary>
        /// 前三个参数（坐标）的协方差
        /// </summary>
        public Matrix CovaOfFirstThree => new Matrix(ResultMatrix.CovaOfEstimatedParam.SubMatrix(0, 3));
        /// <summary>
        /// 具有方差的计算值
        /// </summary>
        public CovaedXyz CovaedEstXyz
        {
            get
            {
                if (ResultMatrix.ParamNames.Count < 3)
                {
                    return null;
                } 
                return new CovaedXyz(EstimatedXyz, CovaOfFirstThree); 
            }
        }
        /// <summary>
        /// XYZ 的均方根/中误差。此处有错！
        /// </summary>
        public XYZ EstimatedXyzRms
        {
            get
            {
                if(ResultMatrix.ParamNames.Count < 3)
                {
                    return XYZ.Zero;
                }

                return new XYZ(Math.Sqrt(ResultMatrix.CovaOfEstimatedParam[0, 0]),
                    Math.Sqrt(ResultMatrix.CovaOfEstimatedParam[1, 1]),
                    Math.Sqrt(ResultMatrix.CovaOfEstimatedParam[2, 2]));
            }
        }
        /// <summary>
        /// DOP值
        /// </summary>
        public DilutionOfPrecision DilutionOfPrecision { get; protected set; }

        #endregion

        #endregion

        #region  方法

        public SimplePositionResult GetSimplePositionResult()
        {
            SimplePositionResult result = new SimplePositionResult();

            result.Xyz = EstimatedXyz.ToString();
            result.RmsXyz = EstimatedXyzRms.ToString();
            result.Name = Material.Name;
            result.GeoCoord = GeoCoord.ToString();
            result.Epoch = Material.ReceiverTime.ToString();
            return result;
        }
        /// <summary>
        /// 估值
        /// </summary>
        /// <returns></returns>
        public RmsedXYZ EstRmsedXYZ=> new RmsedXYZ(this.EstimatedXyz, this.EstimatedXyzRms); 

        /// <summary>
        /// 用空间直角坐标更新大地坐标。
        /// </summary>
        public void UpdateGeoCoordWithXyz() { GeoCoord = CoordTransformer.XyzToGeoCoord(EstimatedXyz, AngleUnit.Degree); }

        /// <summary>
        /// 设置常见的变量。
        /// </summary>
        /// <param name="Adjustment"></param>
        protected virtual void TrySetCommonParamValues(AdjustResultMatrix Adjustment)
        {
            Vector estmated = Adjustment.Estimated;
            if (estmated != null)
            {
                //  对流层设置
                if (Adjustment.ParamNames.Contains(Gnsser.ParamNames.WetTropZpd))
                {
                    if (this.Material is EpochInformation)
                    {
                        ((EpochInformation)Material).NumeralCorrections[Gnsser.ParamNames.WetTropZpd] = estmated[Adjustment.GetIndexOf(Gnsser.ParamNames.WetTropZpd)];
                    }
                }
                //需要计算的属性
                #region 设置 xyz 
                //坐标
                if (Adjustment.ParamNames.Contains(Gnsser.ParamNames.Dx))
                {
                    double x = estmated[Adjustment.GetIndexOf(Gnsser.ParamNames.Dx)];
                    double y = estmated[Adjustment.GetIndexOf(Gnsser.ParamNames.Dy)];
                    double z = estmated[Adjustment.GetIndexOf(Gnsser.ParamNames.Dz)];
                    this.XyzCorrection = new XYZ(x, y, z);

                    //????这种方法很有问题哦！！！
                    if (this.ResultMatrix.ObsMatrix.HasApprox)
                    {
                        this.EstimatedXyz = this.XyzCorrection;
                        log.Info(this.EstimatedXyz.ToString());
                    }
                    else
                    {
                        if (this.Material is EpochInformation)
                        {
                            this.EstimatedXyz = ((EpochInformation)Material).SiteInfo.EstimatedXyz + this.XyzCorrection;
                        }
                        else if (this.Material is MultiSitePeriodInfo)
                        {
                            this.EstimatedXyz = ((MultiSitePeriodInfo)Material).Last.OtherEpochInfo.SiteInfo.EstimatedXyz + this.XyzCorrection;
                        }
                        else if (this.Material is MultiSiteEpochInfo)
                        {
                            this.EstimatedXyz = ((MultiSiteEpochInfo)Material).OtherEpochInfo.SiteInfo.EstimatedXyz + this.XyzCorrection;
                        }
                    }

                    if (this.EstimatedXyz != null)
                    {
                        if (!this.EstimatedXyz.IsValid) throw new Exception("计算坐标无效！" + this.EstimatedXyz);
                        this.UpdateGeoCoordWithXyz();
                    }
                }
            }
            #endregion
        }

        #endregion

        #region 方法

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public virtual void Dispose()
        {
            if (ResultMatrix != null)
            {
                ResultMatrix.Dispose();
                ResultMatrix = null;
            }

            if (Material != null)
            {
                Material.Dispose();
                Material = null;
            }
        }

        #region 输出
        /// <summary>
        /// 显示表题
        /// </summary>
        /// <returns></returns>
        public virtual string GetTabTitles()
        {
            return Geo.Utils.StringUtil.ToString( this.ResultMatrix.ParamNames, "\t");
        }
        /// <summary>
        /// 显示内容
        /// </summary>
        /// <returns></returns>
        public virtual string GetTabValues()
        { 
            return Geo.Utils.StringUtil.ToString(this.ResultMatrix.Estimated, "\t");
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        /// <returns></returns>
        public string GetParamNameString()
        {
            StringBuilder sb = new StringBuilder();
            //  sb.Append(Time.ToString() + ":");
            foreach (var item in this.ResultMatrix.ParamNames)
            {
                sb.Append(" " + Geo.Utils.StringUtil.FillSpaceLeft(item, 8) + ",");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取参数改正数的字符串。与先验值之差。
        /// </summary>
        /// <returns></returns>
        public string GetEstimatedVectorString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Material.ReceiverTime.ToString() + ":");
            int i = 0;
            Vector vector = ResultMatrix.Estimated;

            foreach (var item in vector)
            {
                double val = item;
                sb.Append(" " + Geo.Utils.StringUtil.FillSpaceLeft(val.ToString("0.0000"), 8) + ",");
                i++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 参数标准差字符串。包括（X、Y、Z、测站钟差、对流程湿延迟、N个模糊度）
        /// </summary>
        /// <returns></returns>
        public string GetParamStdVectorString()
        {
            IVector covaOfParam = this.ResultMatrix.StdOfEstimatedParam;
            StringBuilder sb = new StringBuilder();
            sb.Append(Material.ReceiverTime.ToString() + ":\t");

            sb.AppendFormat(new Geo.EnumerableFormatProvider(), "{0:\t6.4}", covaOfParam);
            return sb.ToString();
        }
        /// <summary>
        /// 获取先验值向量字符串
        /// </summary>
        /// <returns></returns>
        public string GetApprioriVectorString()
        {
            IVector apriori = this.ResultMatrix.ObsMatrix.Apriori;
            StringBuilder sb = new StringBuilder();
            sb.Append(Material.ReceiverTime.ToShortTimeString());
            sb.Append("\t");
            sb.Append(apriori);
            return sb.ToString();
        }

        /// <summary>
        /// 简短表题
        /// </summary>
        /// <returns></returns>
        public virtual string ToShortTabTitles()
        {
            return GetTabTitles();
        }
        /// <summary>
        /// 简短内容
        /// </summary>
        /// <returns></returns>
        public virtual string ToShortTabValue()
        {
            return GetTabValues();
        }
        /// <summary>
        /// 参数包括（X、Y、Z、测站钟差、对流程湿延迟、N个模糊度）
        /// </summary>
        /// <returns></returns>
        public virtual string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ReceiverTime.ToString() + "\t");
            Vector vector = ResultMatrix.Corrected.CorrectedValue;
            foreach (var val in vector)
            {
                sb.Append(" " + Geo.Utils.StringUtil.FillSpaceLeft(val.ToString("0.0000"), 8) + ",");
            }
            return sb.ToString();
        }
        #endregion
        #endregion
    }
    //2018.05.15, czs, create in hmx,单星计算结果 
    /// <summary>
    /// 单星计算结果
    /// </summary>
    public class BaseSingleSatGnssResult : SimpleGnssResult<EpochSatellite>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseSingleSatGnssResult() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Material">历元信息</param>
        /// <param name="Adjustment">平差信息</param>
        public BaseSingleSatGnssResult(
               AdjustResultMatrix Adjustment,
               EpochSatellite Material) : base(Adjustment, Material)
        {
        }

        public override string ToString()
        {
            return this.Material + ", " +  this.ResultMatrix;
        }
    }
    //2018.06.04, czs, create in hmx,单星多历元计算结果 
    /// <summary>
    /// 单星多历元计算结果
    /// </summary>
    public class BaseSinglePeriodSatGnssResult : SimpleGnssResult<PeriodSatellite>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseSinglePeriodSatGnssResult() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Material">历元信息</param>
        /// <param name="Adjustment">平差信息</param>
        public BaseSinglePeriodSatGnssResult(
               AdjustResultMatrix Adjustment,
               PeriodSatellite Material) : base(Adjustment, Material)
        {
        }

        public override string ToString()
        {
            return this.Material + ", " +  this.ResultMatrix;
        }
    }

    //2018.05.15, czs, create in hmx,简单GNSS结果 
    /// <summary>
    /// 简单GNSS结果
    /// </summary>
    public class SimpleGnssResult<TMaterial> : SimpleGnssResult
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SimpleGnssResult() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Material">历元信息</param>
        /// <param name="Adjustment">平差信息</param>
        public SimpleGnssResult(
               AdjustResultMatrix Adjustment,
               TMaterial Material) : base(Adjustment)
        {
            this.Material = Material;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Material">历元信息</param>
        /// <param name="Adjustment">平差信息</param>
        /// <param name="NameBuilder">参数名称</param>
        public SimpleGnssResult(
            TMaterial Material,
            AdjustResultMatrix Adjustment,
            GnssParamNameBuilder NameBuilder) : base(Adjustment)
        {
            this.Material = Material;
        }

        /// <summary>
        /// 数据源实体
        /// </summary>
        public virtual TMaterial Material { get; set; }

    }

    /// <summary>
    /// 简单GNSS结果
    /// </summary>
    public class SimpleGnssResult : AdjustmentResult
    {

        /// <summary>
        /// 构造函数
        /// </summary> 
        public SimpleGnssResult()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Adjustment">平差结果</param> 
        public SimpleGnssResult(AdjustResultMatrix Adjustment)
        {
            this.ResultMatrix = Adjustment;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 历元参数精度估计
        /// </summary>
        public ParamAccuracyInfoManager ParamAccuracyInfos { get; set; }
        /// <summary>
        /// 是否具有历元参数精度估计
        /// </summary>
        public bool HasParamAccuracyInfos => (ParamAccuracyInfos != null && ParamAccuracyInfos.Count > 0);
        /// <summary>
        /// 时间
        /// </summary>
        public virtual Time ReceiverTime { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public virtual List<string> ParamNames
        {
            get
            {
                return this.ResultMatrix.ParamNames;
            }
        }

        public virtual string ToShortTabTitles()
        {
            return Geo.Utils.StringUtil.ToString(ParamNames, "\t");
        }

        public virtual string ToShortTabValue()
        {
            return this.ResultMatrix.Estimated.ToString();
                }

        public override string ToString()
        {
            return Name + ", " + ReceiverTime;
        }
    }
}