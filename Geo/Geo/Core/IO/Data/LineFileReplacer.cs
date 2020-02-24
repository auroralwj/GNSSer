//2016.10.28, czs, create in hongqing, 文本行文件内容置换器

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm; 
using Geo.Times;
using Geo.IO;
using Gnsser; 
using Geo.Referencing; 
using Geo.Utils; 
using Gnsser.Checkers;

namespace Geo.IO
{
    /// <summary>
    /// 文本行文件内容置换器
    /// </summary>
    public class LineFileReplacer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="InPath">输入路径</param>
        /// <param name="OutPath">输出路径</param>
        /// <param name="Data">替换数据</param>
        public LineFileReplacer(string InPath, string OutPath, Dictionary<string, string> Data)
        {
            this.OutBufferCount = 100;
            this.Data = Data;
            this.InPath = InPath;
            this.OutPath = OutPath;
            this.StartMarkers = new List<string>();
            this.EndMarkers = new List<string>();
            this.AddingLines = new List<string>();
        }
        #region 属性
        /// <summary>
        /// 输出缓存数量，太小会减慢速度，太大要考虑内存容量。
        /// </summary>
        public int OutBufferCount { get; set; }
        /// <summary>
        /// 替换的新旧字符串，Key为旧字符包含，value为信大字符串句子。
        /// </summary>
        public Dictionary<string, string> Data { get; set; }
        /// <summary>
        /// 需要添加进行的行，将在匹配成功后添加。
        /// </summary>
        public List<string> AddingLines { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string InPath { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string OutPath { get; set; }
        
        /// <summary>
        /// 结束标记，匹配上一个就结束。
        /// </summary>
        public List<string> EndMarkers{get;set;}

        /// <summary>
        /// 起始标记，匹配上一个就开始。
        /// </summary>
        public List<string> StartMarkers{get;set;}
        #endregion

        /// <summary>
        /// 执行
        /// </summary>
        public void Run()
        {
            using (var streamReader = new StreamReader(InPath))
            {
                int lineCount = -1;
                string line = "";
                bool isStarted = false;
                bool isEnded = false;
                var streamWriter = new StreamWriter(OutPath, false);
                while ((line = streamReader.ReadLine()) != null)
                {
                    lineCount++;
                    var outputLine = line;

                    if (!isStarted) { isStarted = IsStart(line); }
                    if (!isEnded) { isEnded = IsEnd(line); }

                    if (isStarted && !isEnded) //已开始，但还没有结束。
                    {
                       if(TryReplace(line, out outputLine)){//成功后，将额外信息加入
                           foreach (var addineLine in AddingLines)
                           {
                                 streamWriter.WriteLine(addineLine);
                           }
                       }
                    }

                    streamWriter.WriteLine(outputLine);

                    if (lineCount % OutBufferCount == 0) { streamWriter.Flush(); }
                }
                streamWriter.Close();
                streamWriter.Dispose();
            }
        }
        /// <summary>
        /// 执行替换尝试，不成功则返回本身。
        /// </summary>
        /// <param name="line"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        public bool TryReplace(string line, out string newLine)
        {
            newLine = line;

            foreach (var item in Data)
            {
                if (line.Contains(item.Key))
                {
                    newLine = item.Value;
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// 通过结束标记，确定所读是否该终止。
        /// 若未指定，默认没有结束。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsEnd(string line)
        {
            if (line == null) { return true; }

            foreach (var item in EndMarkers)
            {
                if (line.ToUpper().Contains(item.ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 通过起始标记，确定所读是否该终止。
        /// 若没有设置，直接返回true，即默认从0行开始。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsStart(string line)
        {
            if (line == null) { return false; }
            if (StartMarkers.Count == 0) { return true; }
            foreach (var item in StartMarkers)
            {
                if (line.ToUpper().Contains(item.ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }
    }







     
}