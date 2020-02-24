//2014.05.22, Cui Yang, created
//2014.08.19, czs , edit, 将 载波频率采用 AntennaFrequency 进行了参数化，支持多系统计算（取决于天线文件）
//2014.09.16, cy, 面向对象
//2015.01.26, lly, Correct()中声明NeillTropModel类的部分参数的单位是角度，不是弧度。
//2015.04.22，cy & lly, 分别添加李GMF模型和VMF1模型。
//2017.05.10, lly, add in zz, gpt2模型
//2017.09.05, czs, edit in zz, 整理模型之间的顺序，确保可用
//2017.10, lly, add in zz, 增加对流层增强 

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Times;
using Gnsser.Domain;
using Geo.Times;
using Geo.Coordinates;


namespace Gnsser.Correction
{


    /// <summary>
    ///对流程改正。
    ///如果测站坐标为 0 或无效 则改正数为 0。
    /// 默认采用Neill模型。这里只是干分量slant改正????是计算的整个干湿分量耶，湿延迟作为未知参数估计。
    /// To compute the main values related to a given GNSS tropospheric model
    /// 
    /// This object will visit every satellite in the GNSS satData structure and will try to compute
    /// the main values of the corresponding tropospheric model: Total tropospheric slant correction, 
    /// dry vertical delay, wet vertical delay, dry mappint function value and wet mapping function value.
    /// 
    /// Be warned that if a given satellite does not have the information needed (main elevation), it will be summarily deleted from the satData structure. 
    /// This also implies that if you try to use a ComputeTroModel objet without prevObj defining the tropospheric model, then ALL satellites will be deleted.
    /// </summary>
    public class TroposphericModelCorrector : AbstractRangeCorrector
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public TroposphericModelCorrector()
        {
            this.Name = "对流层距离改正";
            this.CorrectionType = CorrectionType.TroposphericModel;
        }

        /// <summary>
        /// 构造函数
        /// </summary> 
        public TroposphericModelCorrector(GnssProcessOption Option,DataSourceContext DataSouceProvider)
        {
            this.Name = "对流层距离改正";
            this.Option = Option;
            this.CorrectionType = CorrectionType.TroposphericModel;
            this.DataSourceProvider = DataSouceProvider;
            IsTropAugmentEnabled = Option.IsTropAugmentEnabled;
        }

        GnssProcessOption Option{get;set;}
        /// <summary>
        ///  Pointer to default TropModel object when working with GNSS satData structures.
        /// </summary>
        private NeillTropModel pTroModel = new NeillTropModel();

        /// <summary>
        /// GMF对流层改正模型
        /// 崔阳，2015.03
        /// </summary>
        private GmfTropModel gTroModel = new GmfTropModel();
        /// <summary>
        /// GT2 模型 ，2017.05.10, lly, add in zz
        /// </summary>
        private Gpt2TropModel gpt2Model = new Gpt2TropModel();

       /// <summary>
       /// VMF1对流层改正模型
       /// 李林阳，2015.01
       /// </summary>
        private VMF1TropModel VMF1TroModel = new VMF1TropModel();

        DataSourceContext DataSourceProvider;


        public override void Correct(EpochSatellite epochSatellite)
        {
            if (!epochSatellite.HasEphemeris) { return; }
            Time gpsTime = epochSatellite.RecevingTime;

            XYZ receiverPosition = epochSatellite.SiteInfo.EstimatedXyz;
            if (!receiverPosition.IsValid || receiverPosition.IsZero)
            {
                this.Correction = 0;
                return;
            }
            Polar p = epochSatellite.Polar;

            double elevation = p.Elevation;

            GeoCoord geoCoord = epochSatellite.SiteInfo.ApproxGeoCoord;


            #region Test

            //double FE_WGS84 = 1.0 / 298.257223563;
            //double RE_WGS84 = 6378137.0;

            //double e2 = FE_WGS84 * (2.0 - FE_WGS84);
            //double v = RE_WGS84;
            //XYZ r = epochSatellite.obsPath.ApproxXyz;
            //double r2 = r.X * r.X + r.Y * r.Y;
            //double sinp, z, zk;

            //for (z = r.Z, zk = 0.0; Math.Abs(z - zk) >= 1E-4; )
            //{
            //    zk = z;
            //    sinp = z / Math.Sqrt(r2 + z * z);
            //    v = RE_WGS84 / Math.Sqrt(1.0 - e2 * sinp * sinp);
            //    z = r.Z + v * e2 * sinp;
            //}

            //double pos0 = r2 > 1E-12 ? Math.Atan(z / Math.Sqrt(r2)) : (r.Z > 0.0 ? AstronomicalFunctions.PI / 2.0 : -AstronomicalFunctions.PI / 2.0);//lat
            //double pos1 = r2 > 1E-12 ? Math.Atan2(r.Y, r.Z) : 0.0; //lon
            //double pos2 = Math.Sqrt(r2 + z * z) - v;
            //GeoCoord geoCoord0 = new GeoCoord(pos1 * AstronomicalFunctions.R2D, pos0 * AstronomicalFunctions.R2D, pos2);

            //if (geoCoord != geoCoord0) geoCoord = geoCoord0;

            #endregion


            double wetMap = 0.0, correction = 0.0;
            double[] azel = new double[] { p.Azimuth * AngularTransformer.DegToRadMultiplier, p.Elevation * AngularTransformer.DegToRadMultiplier };
            double wetCorrectValue = epochSatellite.EpochInfo.NumeralCorrections[Gnsser.ParamNames.WetTropZpd];

            //首先尝试

            
            #region 方案4： GpT2 模型 ， 李林阳 added
            if (this.DataSourceProvider.gpt2DataService1Degree != null || this.DataSourceProvider.gpt2DataService != null)
            {
                this.gpt2Model = new Gpt2TropModel(DataSourceProvider);
                //返回干分量和湿分量对流层延迟的估值，wetMap 为湿分量映射函数，用于作为残余天定距参数的斜距映射。
                correction = gpt2Model.Correction(gpsTime, geoCoord, receiverPosition, azel, wetCorrectValue, ref wetMap);
            }
            #endregion


            //天顶对流层延迟，高度角为90度
            double[] azel2 = new double[] { p.Azimuth * SunMoonPosition.DegToRad, Math.PI / 2 };
            double wetMap_ZTD = 0;
            //double appriorWetDealy_ZTD = 0;
            double ZTD = gpt2Model.Correction(gpsTime, geoCoord, receiverPosition, azel2, wetCorrectValue, ref wetMap_ZTD);
            epochSatellite.AppriorTropDelay = ZTD;
            epochSatellite.WetMap_ZTD = wetMap_ZTD;




            if (wetMap == 0 && correction == 0)
            {
                #region 方案2： GMF 模型 , 崔阳， Added
                if (DataSourceProvider == null || DataSourceProvider.Vmf1DataService == null || DataSourceProvider.Vmf1DataService.Count == 0)
                {
                    correction = gTroModel.Correction(gpsTime, geoCoord, receiverPosition, azel, wetCorrectValue, ref wetMap);
                }
                #endregion
            }
            if (wetMap == 0 && correction == 0)
            {
                #region 方案1 ： Neill模型

                this.pTroModel = new NeillTropModel(geoCoord.Height, geoCoord.Lat, gpsTime.DayOfYear);
                wetMap = pTroModel.Wet_Mapping_Function(elevation);
                correction = GetTroposphericCorectValueWithNillModel(elevation, geoCoord, pTroModel);
                #endregion
            }

            if (wetMap == 0 && correction == 0)
            {
                #region 方案3： VMF 模型 ， 李林阳 added
                string stanam = epochSatellite.SiteInfo.SiteName;

                //double WetMap = 0.0;
                //采用VMF1模型~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~，李林阳添加
                this.VMF1TroModel = new VMF1TropModel(DataSourceProvider, stanam, geoCoord.Height, geoCoord.Lat * AngularTransformer.DegToRadMultiplier, gpsTime);
                wetMap = 0;
                //采用VMF1模型，GetSatPhaseCenterCorectValue函数也要作出改变~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~，李林阳添加
                //double correction = GetSatPhaseCenterCorectValue(elevation, geoCoord, VMF1TroModel,ref WetMap);
                correction = VMF1TroModel.Correction(elevation, ref wetMap);

                epochSatellite.Vmf1WetMap = wetMap;

                #endregion
            }

            epochSatellite.WetMap = wetMap;

            this.Correction = correction;

            //对流层增强 
            double augment = 0.0;
            if (this.IsTropAugmentEnabled && DataSourceProvider.TropAugService != null)
            {
                augment = DataSourceProvider.TropAugService.Correction(epochSatellite.RecevingTime);
                this.Correction = correction + wetMap * augment;
            } 
        }

        public bool IsTropAugmentEnabled { get; set; }

        /// <summary>
        /// 对流层延迟改正
        /// </summary>
        /// <param name="elevation"></param>
        /// <param name="geoCoord"></param>
        /// <param name="pTroModel"></param>
        /// <returns></returns>
        public static double GetTroposphericCorectValueWithNillModel(double elevation, GeoCoord geoCoord, NeillTropModel pTroModel)
        {
            double tropoCorr = 0.0;
            //Compute tropospheric slant correction
            tropoCorr = pTroModel.Correction(elevation);

            if (!pTroModel.isValid())
            {
                tropoCorr = 0.0;
            }

            return tropoCorr;
        }

    }
}
