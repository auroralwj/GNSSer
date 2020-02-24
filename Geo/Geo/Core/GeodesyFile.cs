//2015.07.02, czs, create in TianJing, 通用数据库录入设计。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.IO;

namespace Geo.Data
{ 
    /// <summary>
    /// 头部信息
    /// </summary>
    public class GeodesyFileHeader
    {
        /// <summary>
        /// 文件标识符，应该出现在文件第一个数据内容中。
        /// </summary>
        public const string Identifier = "Geodesy File";
        /// <summary>
        /// 所有以井号“#”开始的行表示注释，含头部和数据体。
        /// </summary>
        public const string CommentMarker = "#";

        /// <summary>
        /// 文件版本
        /// </summary>
        public double Version { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }


        /// <summary>
        /// 数据表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 列
        /// </summary>
        public List<String> Columns { get; set; }

        /// <summary>
        /// 数据表内容分隔符
        /// </summary>
        public string[] Splitters { get; set; }


    }
       
    /// <summary>
    /// 文件
    /// </summary>
    public class GeodesyFile
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public GeodesyFile()
        {
            RawTable = new List<GeodesyFileRow>();
        }
        /// <summary>
        /// 头部信息
        /// </summary>
        public GeodesyFileHeader Header { get; set; }
        /// <summary>
        /// 数据表
        /// </summary>
        public List<GeodesyFileRow> RawTable { get;set; } 
    }

    /// <summary>
    /// 文件读取
    /// </summary>
    public class GeodesyFileReader
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path"></param>
        public GeodesyFileReader(string path)
        {
            this.Path = path;
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public GeodesyFile Read()
        {
            using (StreamReader reader = new StreamReader(Path, Encoding.UTF8))
            {
                GeodesyFile file = new GeodesyFile();
                GeodesyFileHeader header = new GeodesyFileHeader();
                //首先解析头部
                string line = null;
                int contentLine = -1;
                GeodesyFileIndecator GeodesyFileIndecator = Data.GeodesyFileIndecator.FileIndicator;
                while ((line = ReadNextContent( reader)) != null)
                {
                    contentLine++;

                    //第一行，标识
                    if(contentLine == 0){ 
                       if(!line.StartsWith(GeodesyFileHeader.Identifier)){
                           throw new FormatException("非 Geodesy 格式，内容行必须以 " + GeodesyFileHeader.Identifier + " 开始！");
                       }else{ 
                           GeodesyFileIndecator = Data.GeodesyFileIndecator.Header;
                           continue;
                       }
                    }
                    if(GeodesyFileIndecator == Data.GeodesyFileIndecator.Header){

                    //读取头文件
                    var kvs =line.Split(new char[]{':'},  StringSplitOptions.RemoveEmptyEntries);
                    var key = kvs[0].ToLower();
                    var val = kvs[1];
                    switch (key)
                    {
                        case "version": header.Version = double.Parse(val); break;
                        case "columns":
                            break;
                            //header.Columns = 
                            
                            
                            
                        //    double.Parse(currentVal); 
                            
                            
                        //    break;
                        //case "tableName":


                        //    header.Version = double.Parse(currentVal); 
                        //    break;


                        //case "splitter":
                            
                        //    header.Version = double.Parse(currentVal);
                            
                        //    break;
                        //case "version": header.Version = double.Parse(currentVal); break;
                        //case "version": header.Version = double.Parse(currentVal); break;
                        //case "version": header.Version = double.Parse(currentVal); break;



                    }

                    }


                }

                return null;
            }
        }

        /// <summary>
        /// 读取一个有效行，即非空白，非注释
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public string ReadNextContent(StreamReader reader)
        {
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(GeodesyFileHeader.CommentMarker)) { continue; }

                if (String.IsNullOrWhiteSpace(line)) { continue; }

                return line;
            }
            return line;
        }
    }

    /// <summary>
    /// 文件读取位置
    /// </summary>
     internal enum GeodesyFileIndecator
    {
        /// <summary>
        /// 正在寻找文件标识
        /// </summary>
        FileIndicator,
        /// <summary>
        /// 正在读取文件头部
        /// </summary>
        Header,
        /// <summary>
        /// 读取内容
        /// </summary>
        Content
    }


    /// <summary>
    /// 行
    /// </summary>
    public class GeodesyFileRow
    { 
        /// <summary>
        /// 关键字，如点名，点号。
        /// </summary>
        public String Key { get; set; }

        /// <summary>
        /// 数据集合
        /// </summary>
        public List<String> Items { get; set; } 
    }
}
