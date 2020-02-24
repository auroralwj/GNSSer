//2015.09.27, czs, create in xi'an hongqing, 元数据.数据变量名称。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;

namespace Geo.IO
{
    /// <summary>
    /// 元数据.数据变量名称。
    /// </summary>
    public class VariableNames
    {
        /// <summary>
        /// 版本
        /// </summary>
        public const string Version = "Version";
        /// <summary>
        /// 作者
        /// </summary>
        public const string Author = "Author";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string CreationTime = "CreationTime";
        /// <summary>
        /// 文件类型
        /// </summary>
        public const string FileType = "FileType";
        /// <summary>
        /// 项目分隔符号
        /// </summary>
        public const string ItemSplliter = "ItemSplliter";
        /// <summary>
        /// 项目分隔符号
        /// </summary>
        public const string CommentMarker = "CommentMarker";
        /// <summary>
        /// 属性。
        /// </summary>
        public const string PropertyNames = "PropertyNames";
        /// <summary>
        /// 属性。
        /// </summary>
        public const string PropertyUnits = "PropertyUnits";
        /// <summary>
        /// 名称而已
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        /// 名称而已
        /// </summary>
        public const string Id = "Id";
        /// <summary>
        /// 名称而已
        /// </summary>
        public const string CoordSys = "CoordSys";
        /// <summary>
        /// Value 量
        /// </summary>
        public const string Value = "Value";
        /// <summary>
        /// Value2 量
        /// </summary>
        public const string Value2 = "Value2";
        /// <summary>
        /// Value3 量
        /// </summary>
        public const string Value3 = "Value3";
        /// <summary>
        /// X 坐标分量
        /// </summary>
        public const string X = "X";
        /// <summary>
        /// Y 坐标分量
        /// </summary>
        public const string Y = "Y";
        /// <summary>
        /// Z 坐标分量
        /// </summary>
        public const string Z = "Z";
        /// <summary>
        /// X 坐标分量
        /// </summary>
        public const string XMeter = "X(M)";
        /// <summary>
        /// Y 坐标分量
        /// </summary>
        public const string YMeter = "Y(M)";
        /// <summary>
        /// Z 坐标分量
        /// </summary>
        public const string ZMeter = "Z(M)";
        /// <summary>
        /// 椭球长半径。
        /// </summary>
        public const string SemiMinorAxis = "SemiMinorAxis";
        /// <summary>
        /// 地球扁率
        /// </summary>
        public const string Flattening = "Flattening";
        /// <summary>
        /// 地球扁率或者倒数。
        /// </summary>
        public const string FlatteningOrInverse = "FlatteningOrInverse";         
        /// <summary>
        /// 大地高 Geodetic Height
        /// </summary>
        public const string GeoHeight = "GeoHeight";
        public const string Height = "Height";
        public const string Azimuth = "Azimuth";
        public const string ToId = "ToId";
        /// <summary>
        /// 起始行编号
        /// </summary>
        public static string StartRowIndex = "StartRowIndex";
        /// <summary>
        /// 经度
        /// </summary>
        public const string Lon = "Lon";
        /// <summary>
        /// 纬度
        /// </summary>
        public const string Lat = "Lat";
        /// <summary>
        /// To经度
        /// </summary>
        public const string ToLon = "ToLon";
        /// <summary>
        /// To纬度
        /// </summary>
        public const string ToLat = "ToLat";
        /// <summary>
        /// 经度
        /// </summary>
        public const string LonDegree = "Lon(Degree)";
        /// <summary>
        /// 纬度
        /// </summary>
        public const string LatDegree = "Lat(Degree)";
        /// <summary>
        /// 经度
        /// </summary>
        public const string LonDms_s = "Lon(Dms_s)";
        /// <summary>
        /// 纬度
        /// </summary>
        public const string LatDms_s = "Lat(Dms_s)";
        /// <summary>
        /// 起始的名称
        /// </summary>
        public const string StartName = "StartName";
        /// <summary>
        /// 结束的名称
        /// </summary>
        public const string EndName = "EndName";
        /// <summary>
        /// 依赖
        /// </summary>
        public const string Depends = "Depends";
        /// <summary>
        /// 参数文件路径
        /// </summary>
        public const string ParamFilePath = "ParamFilePath";
        /// <summary>
        /// 编号
        /// </summary>
        public const string Index = "Index";
        /// <summary>
        /// 地址
        /// </summary>
        public const string Url = "Url";
        /// <summary>
        /// 本地路径
        /// </summary>
        public const string LocalDirectory = "LocalDirectory";
        /// <summary>
        /// 用户名
        /// </summary>
        public const string UserName = "UserName";
        /// <summary>
        /// 密码
        /// </summary>
        public const string Password = "Password";
        /// <summary>
        /// 源文件或目录
        /// </summary>
        public const string FromFileOrDirectory = "FromFileOrDirectory";
        /// <summary>
        /// 文件或目录
        /// </summary>
        public const string FileOrDirectory = "FileOrDirectory";
        /// <summary>
        /// 目标文件或目录
        /// </summary>
        public const string ToFileOrDirectory = "ToFileOrDirectory";
        /// <summary>
        /// 是否覆盖。
        /// </summary>
        public const string Overwrite = "Overwrite";
        /// <summary>
        /// 是否覆盖。
        /// </summary>
        public const string IsOverwrite = "IsOverwrite";

        /// <summary>
        /// 目标目录
        /// </summary>
        public static string ToDirectory = "ToDirectory";
        /// <summary>
        /// 输出目录
        /// </summary>
        public static string OutputDirectory = "OutputDirectory";
        /// <summary>
        /// 输出文件路径
        /// </summary>
        public static string OutputFilePath = "OutputFilePath";

        public static string DestPropertyUnits = "DestPropertyUnits";
    }
}
