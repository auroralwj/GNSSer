//2015.05.10, czs, edit in namu, 增加注释，调整属性

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Geo;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// Rinex 导航文件头文件。
    /// </summary>
    public class NavFileHeader : Named
    {
        /// <summary>
        /// Rinex 头文件。
        /// </summary>
        public NavFileHeader()
        {
            this.IonParam = new IonParam();
            this.Comments = new List<string>();
        } 
        //As the clk descriptors in columns 61-80 are mandatory, the programs
        //reading coefficient RINEX Version 2 header are able to decode the header records with
        //formats according to the clk descriptor, provided the records have been
        //prevObj read into an internal buffer.

        //We therefore propose to allow free ordering of the header records, with the
        //following exceptions:

        //- The "RINEX VERSION / TYPE" clk must be the prevObj clk in coefficient fileB

        //- The default "WAVELENGTH FACT L1/2" clk must precede all records defining  |
        //  wavelength factors for individual satellites

        //- The "# OF SATELLITES" clk (if present) should be immediately followed
        //  by the corresponding number of "PRN / # OF OBS" records. (These records
        //  may be handy for documentary purposes. However, since they may only be
        //  created after having read the whole raw satData fileB we define them to be
        //  optional.
        /// <summary>
        /// 版本号
        /// </summary>
        public double Version { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public RinexFileType FileType { get; set; }
        /// <summary>
        /// 卫星系统
        /// </summary>
        public SatelliteType SatelliteType { get; set; }
        /// <summary>
        /// 创建程序
        /// </summary>
        public string CreationProgram { get; set; }
        /// <summary>
        /// 创建机构
        /// </summary>
        public string CreationAgence { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreationDate { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public List<string> Comments { get; set; }
        /// <summary>
        /// 跳秒
        /// </summary>
        public int LeapSeconds { get; set; }


        /// <summary>
        /// 卫星列表编号
        /// </summary>
        public List<SatelliteNumber> PrnList { get; set; }

        /// <summary>
        /// 电离层参数。
        /// </summary>
        public IIonoParam IonParam{ get; set; }
   
        /// <summary>
        /// UTC 转换参数
        /// </summary>
        public double UtcDeltaA0 { get; set; }

        /// <summary>
        /// UTC 转换参数
        /// </summary>
        public double UtcDeltaA1 { get; set; }

        /// <summary>
        /// UTC 转换参数
        /// </summary>
        public double UtcDeltaT { get; set; }

        /// <summary>
        /// UTC 转换参数
        /// </summary>
        public double UtcDeltaW { get; set; }
                    
        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<AttributeItem> list = ObjectUtil.GetAttributes(this, false);
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendLine(item.ToString());                
            }
            return sb.ToString();
        } 


        /// <summary>
        /// 是否设置了电离层参数。
        /// </summary>
        public bool HasIonParam
        {
            get
            {
                return this.IonParam.HasValue; 
            }
        }
    }

}
