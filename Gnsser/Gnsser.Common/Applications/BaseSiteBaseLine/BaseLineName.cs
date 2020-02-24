//2018.11.28, czs, create in hmx, 基线名称

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Geo;

namespace Gnsser
{  

    /// <summary>
    /// GNSS 基线名称。
    /// </summary>
    public class GnssBaseLineName : BaseLineName , IComparable<GnssBaseLineName>
    {
         /// <summary>
         /// 构造函数
         /// </summary>
         public GnssBaseLineName() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseLineName"></param>
        public GnssBaseLineName(string baseLineName)
        {
            var strs = GetRefRovName(baseLineName);
            this.RovName = strs[0];
            this.RefName = strs[1];
            this.Name = baseLineName;
        }

        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="rovName"></param>
        /// <param name="refName"></param>
        /// <param name="isPath"></param>
        public GnssBaseLineName(string rovName, string refName, bool isPath = false)
        {
            this.RefName = refName;
            this.RovName = rovName;
            if (isPath && File.Exists(refName) && File.Exists(rovName))
            {
                this.RefName = GetSiteName(refName);
                this.RovName = GetSiteName(rovName);
                this.RefFilePath = refName;
                this.RovFilePath = rovName;
            }
            Init();
        }
        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="rovName"></param>
        /// <param name="refName"></param>
        /// <param name="isPath"></param>
        public GnssBaseLineName(ObsSiteInfo rovName, ObsSiteInfo refName)
        {
            this.RefName = refName.SiteName;
            this.RovName = rovName.SiteName; 
            this.RefFilePath = refName.FilePath;
            this.RovFilePath = rovName.FilePath;
            Init();
        }
        /// <summary>
        /// 初始化，必须要设置REF和ROV名称
        /// </summary>
        public void Init()
        {
            if (Name == null)
            {
                this.Name = GetBaseLineName(RefName, RovName);
            } 
        }
        #region 额外路径属性
        /// <summary>
        /// 是否自带路径
        /// </summary>
        public bool HasPath { get => !string.IsNullOrWhiteSpace(RefFilePath) && !string.IsNullOrWhiteSpace(RovFilePath); }
        /// <summary>
        ///  参考站文件名称
        /// </summary>
        public string RefFileName { get => Path.GetFileName(RefFilePath); }
        /// <summary>
        ///  流动站文件名称
        /// </summary>
        public string RovFileName { get => Path.GetFileName(RovFilePath); }
        /// <summary>
        ///  参考站文件路径
        /// </summary>
        public string RefFilePath{ get; set; }
        /// <summary>
        ///  流动站文件路径
        /// </summary>
        public string RovFilePath { get; set; }
        #endregion
        /// <summary>
        /// 反方向基线
        /// </summary>
        public GnssBaseLineName ReverseBaseLine
        {
            get => new GnssBaseLineName(this.RefName, this.RovName) { RovFilePath = this.RefFilePath, RefFilePath = this.RovFilePath };
        }
        /// <summary>
        /// 返回以指定名为起始的基线，若无匹配返回null
        /// </summary>
        /// <param name="refName"></param>
        /// <returns></returns>
        public GnssBaseLineName GetBaseLineNameWithRef(string refName)
        {
            if(this.RefName == refName) { return this; }
            if(this.RovName == refName) {  return ReverseBaseLine; }
            return null;
        }
        /// <summary>
        /// 是否相等，或反向相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsEqualOrReverseEqual(GnssBaseLineName other)
        {
            return this.Equals(other) || this.ReverseBaseLine.Equals(other);
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(GnssBaseLineName left, GnssBaseLineName right)
        {
            if ((left as object) != null)
            {
                return left.Equals(right);
            }
            return (right as object) == null; 
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(GnssBaseLineName left, GnssBaseLineName right)
        {
            if ((left as object) != null)
            {
                return !left.Equals(right);
            }
            return (right as object) != null;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        #region 工具
        /// <summary>
        /// 提取测站名称，以文件名表示,且都为大写字母
        /// </summary>
        /// <param name="nameOrPath"></param>
        /// <param name="charCount"></param>
        /// <returns></returns>
        public static string GetSiteName(string nameOrPath, int charCount = 8)
        {
            if (nameOrPath.Contains("/") || nameOrPath.Contains("\\"))
            {
                if (File.Exists(nameOrPath))
                {
                    var name = Gnsser.Data.Rinex.RinexObsFileReader.ReadSiteName(nameOrPath, charCount);
                    return name;
                }
            }
            return Gnsser.Data.Rinex.RinexObsFileReader.BuildSiteName(nameOrPath, charCount);//.ToUpper();
        }
        /// <summary>
        /// 获取连接路径
        /// </summary>
        /// <param name="baseLines"></param>
        /// <returns></returns>
        static public List<GnssBaseLineName> GetClosuredPath(List<GnssBaseLineName> baseLines)
        {
            List<GnssBaseLineName> result = new List<GnssBaseLineName>();
            if (baseLines.Count <= 1) { return result; }
            var firstLn = baseLines[0];
            var firstPt = firstLn.RefName;
            var current = firstLn;
            result.Add(current);
            GnssBaseLineName next = null;
           while( (next = GetNextLine(baseLines, current)) != null)
            {
                result.Add(next);
                if (next.Contains(firstPt)) { break; }
                current = next;
            }
            return result;
        }
        /// <summary>
        /// 获取下一条线
        /// </summary>
        /// <param name="baseLines"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        static public GnssBaseLineName GetNextLine(List<GnssBaseLineName> baseLines, GnssBaseLineName current)
        {
            foreach (var item in baseLines)
            {
                if (item.IsEqualOrReverseEqual(current)) { continue; }
                if (item.Contains(current.RovName)) { return item.GetBaseLineNameWithRef(current.RovName); }
            }
            return null;
        }
        /// <summary>
        /// 是否包含或相反包含
        /// </summary>
        /// <param name="lineNames"></param>
        /// <param name="lineName"></param>
        /// <returns></returns>
        static public bool ContainsOrReversedContains(List<GnssBaseLineName> lineNames, GnssBaseLineName lineName)
        {
            foreach (var item in lineNames)
            {
                if (item.IsEqualOrReverseEqual(lineName)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// 生成基线名称,Ref-Rov
        /// </summary>
        /// <param name="rovFileName"></param>
        /// <param name="refFileName"></param>
        /// <returns></returns>
        public static string GetBaseLineName(string refFileName, string rovFileName)
        {
            return GetSiteName(rovFileName) + BaseLineSplitter + GetSiteName(refFileName);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="lineName"></param>
        /// <returns></returns>
        public static GnssBaseLineName Parse(string lineName)
        {
            return new GnssBaseLineName(lineName);
        }

        public int CompareTo(GnssBaseLineName other)
        {
            return this.RefName.CompareTo(other.RefName);
        }
        #endregion

    }
}
