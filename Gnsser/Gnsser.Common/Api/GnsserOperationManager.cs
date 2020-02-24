//2015.10.05, czs, create in xi'an hongqing, 数据处理引擎

using System;
using Geo.IO;
using Geo.Utils; 
using Geo;
using Geo.Times;
using Gnsser.Api;


namespace Gnsser
{
    /// <summary>
    /// 操作注册管理中心。操作首先在此注册后，才可以使用。
    /// </summary>
    public class GnsserOperationManager : OperationManager 
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public GnsserOperationManager() 
        {

        }
        static OperationManager mgr = new OperationManager();
         
        /// <summary>
        /// Gnsser 默认操作管理器，基本包括了所有的API
        /// </summary>
        public static OperationManager Default
        {
            get
            {
                if (mgr.Count != 0) { return mgr; }

                    //坐标转换，上传下载;
                    mgr.Regist(new FtpDownload());
                    mgr.Regist(new FtpUpload());
                    mgr.Regist(new PointPosition());
                    mgr.Regist(new XyzToGeoCoord());
                    mgr.Regist(new GeoCoordToXyz());

                if (Geo.Setting.VersionType == VersionType.Development)
                {
                    //工具
                    mgr.Regist(new Copy());
                    mgr.Regist(new Move());
                    mgr.Regist(new Delete());
                    mgr.Regist(new CreateTxt());
                    mgr.Regist(new DecompressD());
                    mgr.Regist(new DecompressZ());
                    mgr.Regist(new TimeScopeStringGenerator());
                    mgr.Regist(new Sp3UrlGenerator());


                    //14:41 16：  G97， 16:40 到
                    mgr.Regist(new CycleSlipDetect());
                    mgr.Regist(new CycleSlipMark());
                    mgr.Regist(new SatConsecutiveAnalysis());
                    mgr.Regist(new BreakingDetect());
                    mgr.Regist(new SmallPartsRemove());
                    mgr.Regist(new ToRinex());
                    mgr.Regist(new RinexConvert());
                    mgr.Regist(new PositionPreprocess());
                    mgr.Regist(new DoubleDiffer());
                    mgr.Regist(new BaselineSelect());
                    mgr.Regist(new EpochInfoExtract());

                    mgr.Regist(new CreateProject());
                }

                return mgr;
            }
        }
    }
}
