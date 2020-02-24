//2016.12.27 czs & cuiyang, created,  测站文件写入器

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Common;
using System.IO;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.IO;
using Geo.Referencing;
using Geo.Times;


namespace Gnsser.Data{

    /// <summary>
    /// 坐标文件写入器
    /// </summary>
    public class StationInfoReader : LineFileReader<StationInfo>
    { /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public StationInfoReader(string gofFilePath, string metaFilePath = null)
            : base(gofFilePath, metaFilePath)
        {

            //默认路径是 ProjectDirectory/Script/GofFileName.gof
            this.BaseDirectory = (Path.GetDirectoryName(gofFilePath));
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="Gmetadata"></param>
        public StationInfoReader(string gofFilePath, Gmetadata Gmetadata)
            : base(gofFilePath, Gmetadata)
        {
            //默认路径是 ProjectDirectory/Script/GofFileName.gof
            this.BaseDirectory = (Path.GetDirectoryName(gofFilePath));
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Gmetadata"></param>
        protected override void Init(Gmetadata Gmetadata)
        {
            base.Init(Gmetadata);
            this.ItemSpliters = new string[] { " " };
            StartIndex = 0;
            this.CommentMarkers = new string[] { "#", "*" };

            ItemLengthes = new List<int>()
            {
                7,18,19,19,9,8,8,8,20,20,20,20
            };  
        }


        /// <summary>
        /// 解析字符串为对象
        /// </summary>
        /// <param name="valString"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        protected override object ParseString(string valString, System.Reflection.PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(Time))
            {
                return Time.ParseYearDayOfYear(valString);
            }

            return base.ParseString(valString, propertyInfo);
        }
         
    }
}