//2015.10.09, czs, create in  xi'an hongqing, 坐标转换

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;
using Geo.Times;
using Geo.Coordinates;

namespace Gnsser.Api
{
    /// <summary>
    /// 坐标转换
    /// </summary>
    public class XyzToGeoCoord : ParamBasedOperation<XyzToGeoCoordParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XyzToGeoCoord()
        {
        }

      
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new XyzToGeoCoordReader(this.OperationInfo.ParamFilePath);
    
            bool isNewPath = true;
            int maxCount = 2000;  //缓存最大数量
            List<GeoCoord> coords = new List<GeoCoord>();
            XyzToGeoCoordParam prevParam = null; 
            foreach (var item in reader)
            {
                this.CurrentParam = item;
                var geoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(item.Xyz, item.Ellipsoid, item.AngleUnit);
                coords.Add(geoCoord);

                //是否新路径
                if (prevParam != null)
                {
                    if (prevParam.OutputPath != item.OutputPath) { isNewPath = true; }
                }

                //是否是第一个新路径。
                if (isNewPath)
                {
                    //删除已有老文件
                    if (prevParam == null)
                    { 
                        if (File.Exists(item.OutputPath))   {  File.Delete(item.OutputPath);   }
                    }
                    else//下一个输出路径发生变化，立刻写入老文件
                    {  
                        AppendToFile(prevParam.OutputPath, ref coords); 
                    }

                    isNewPath = false;
                }

                //先缓存，防止内存溢出，再写入
                if (coords.Count >= maxCount)
                {
                    AppendToFile(item.OutputPath, ref coords);
                } 
                
                //update
                prevParam = item; 
            }

            //最后追加，确保完全输出
            AppendToFile(prevParam.OutputPath, ref coords);

            return true;
        }
         

        /// <summary>
        /// 追加到文件。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="coords"></param>
        public void AppendToFile( string path, ref  List<GeoCoord> coords)
        {
            var splitter = ", ";
            StringBuilder sb = new StringBuilder();
            if (!File.Exists(path))
            {
                sb.AppendLine("#Lon" + splitter + "Lat" + splitter + "Height");
            }
            foreach (var item in coords)
            {
                sb.AppendLine(item.Lon + splitter + item.Lat + splitter + item.Height);
            }

            Geo.Utils.FileUtil.CheckOrCreateDirectory(Path.GetDirectoryName(path));

            //写入文件，追加
            File.AppendAllText(path, sb.ToString(), Encoding.UTF8);

            //通知
            this.StatedMessage = StatedMessage.Processing;
            this.StatedMessage.Message = "生成并追加 " + coords.Count + " 条结果到 " + path;
            this.OnStatedMessageProduced();
        }         
    }
}
