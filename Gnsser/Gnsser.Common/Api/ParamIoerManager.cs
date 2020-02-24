//2015.10.21, czs, create in hongqing, 参数读取和写入管理器

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data; 
using System.Linq;
using System.Text; 
using Gnsser.Api;
using Geo;
using Geo.IO;
using Gnsser;

namespace Gnsser
{
    /// <summary>
    /// 参数读写管理器.同 GnsserOperationManager 类似，是操作参数文件的注册机构。
    /// </summary>
    public class ParamIoerManager : BaseDictionary<string, ParamIoer>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ParamIoerManager()
        {
        }

        /// <summary>
        /// 注册。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Reader"></param>
        /// <param name="Writer"></param>
        public void Add(Type type, IEntityFileReader Reader, IEntityWriter Writer)
        {
            this.Set(type.Name, new ParamIoer() { Name = type.Name, Reader = Reader, Writer = Writer });
        }

        //在此毫无意义，只是方便跳转。！！！！czs, 2015.10.22
        GnsserOperationManager GnsserOperationManager;

        /// <summary>
        /// 默认的注册结果
        /// </summary>
        public static ParamIoerManager Default
        {
            get
            {
                var mgr = new ParamIoerManager();
                mgr.Add(typeof(IoParam), new IoParamReader(), new IoParamWriter());
                mgr.Add(typeof(VersionedIoParam), new VersionedIoParamReader(), new LineFileWriter<VersionedIoParam>());
                mgr.Add(typeof(AppendStringToLineParam), new AppendStringToLineParamReader(), new AppendStringToLineParamWriter());
                mgr.Add(typeof(CreateTxtParam), new CreateTxtParamReader(), new CreateTxtParamWriter());
                mgr.Add(typeof(DecompressParam), new DecompressParamReader(), new DecompressParamWriter());
                mgr.Add(typeof(FtpParam), new FtpParamReader(), new FtpParamWriter());
                mgr.Add(typeof(InputParam), new InputParamReader(), new InputParamWriter());
                mgr.Add(typeof(Sp3UrlGeneratorParam), new Sp3UrlGeneratorReader(), new Sp3UrlGeneratorWriter());
                mgr.Add(typeof(TimeScopeStringGeneratorParam), new TimeScopeStringGeneratorReader(), new TimeScopeStringGeneratorWriter());
                mgr.Add(typeof(GeoCoordToXyzParam), new GeoCoordToXyzReader(), new GeoCoordToXyzWriter());
                mgr.Add(typeof(XyzToGeoCoordParam), new XyzToGeoCoordReader(), new XyzToGeoCoordWriter());
                mgr.Add(typeof(PointPositionParam), new PointPositionParamReader(), new PointPositionParamWriter());
                mgr.Add(typeof(DoubleDifferParam), new DoubleDifferParamReader(), new DoubleDifferParamWriter());
                mgr.Add(typeof(SiteInfo), new SiteInfoReader(), new SiteInfoWriter());

                return mgr;
            }
        }
    }

    /// <summary>
    /// 参数读写器
    /// </summary>
    public class ParamIoer : Named
    {

        /// <summary>
        /// 参数名称
        /// </summary>
        //public string ParamName { get; set; }

        /// <summary>
        /// 读取
        /// </summary>
        public IEntityFileReader Reader { get; set; }

        /// <summary>
        /// 写
        /// </summary>
        public IEntityWriter Writer { get; set; }
    }
}
