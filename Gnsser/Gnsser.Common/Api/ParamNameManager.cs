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
    public class ParamNameManager : BaseDictionary<string, ParamName>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ParamNameManager()
        {
        }

        /// <summary>
        /// 注册。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Reader"></param>
        /// <param name="Writer"></param>
        public void Add(Type type)
        {
            this.Set(type.Name, new ParamName(type));
        }

        //在此毫无意义，只是方便跳转。！！！！czs, 2015.10.22
        GnsserOperationManager GnsserOperationManager;

        /// <summary>
        /// 默认的注册结果
        /// </summary>
        public static ParamNameManager Default
        {
            get
            {
                var mgr = new ParamNameManager();
                mgr.Add(typeof(IoParam));
                mgr.Add(typeof(VersionedIoParam));
                mgr.Add(typeof(AppendStringToLineParam));
                mgr.Add(typeof(CreateTxtParam));
                mgr.Add(typeof(DecompressParam));
                mgr.Add(typeof(FtpParam));
                mgr.Add(typeof(InputParam));
                mgr.Add(typeof(Sp3UrlGeneratorParam));
                mgr.Add(typeof(TimeScopeStringGeneratorParam));
                mgr.Add(typeof(GeoCoordToXyzParam));
                mgr.Add(typeof(XyzToGeoCoordParam));
                mgr.Add(typeof(PointPositionParam));
                mgr.Add(typeof(DoubleDifferParam));
                mgr.Add(typeof(SiteInfo));

                return mgr;
            }
        }
    }

    /// <summary>
    /// 参数读写器
    /// </summary>
    public class ParamName : Named
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        /// <param name="type"></param>
        public ParamName(Type type)
        {
            this.Type = type;

        }

        public Type Type { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get { return Type.Name; } }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string FullName { get { return Type.FullName; } }
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName { get { return Type.Assembly.FullName; } }

        public override string ToString()
        {
            return FullName.ToString();
        }

        public override bool Equals(object obj)
        {
            var o = obj as ParamName;
            if (o == null) return false;

            return o.FullName == this.FullName;
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }


    }
}
