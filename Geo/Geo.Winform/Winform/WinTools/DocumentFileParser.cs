using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//using org.pdfbox.pdmodel;
//using org.pdfbox.util;

using Word = Microsoft.Office.Interop.Word;
using Ppt = Microsoft.Office.Interop.PowerPoint;

namespace Geo.Service
{
    /// <summary>
    /// 文件解析器。
    /// 1.必须添加 Office，Microsoft.Office.Interop.Word 和 Microsoft.Office.Interop.PowerPoint才能解析PPT和WORD。
    /// 2.PDF采用的是 pdfbox，有四个组件：
    /// FontBox-0.1.0-dev.dll， IKVM.GNU.Classpath.dll， PDFBox-0.7.3.dll， IKVM.Runtime.dll
    /// 在项目中引用前三个，最后一个IKVM.Runtime.dll，要复制到bin目录中，否则报错。
    /// </summary>
    public static class DocumentFileParser
    {
        /// <summary>
        /// 读取表格中的文本。
        /// 对于doc文件中的表格，读出的结果是去除掉了网格线，内容按行读取。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ParseWord(string filePath)
        {
            object readOnly = true;
            object missing = System.Reflection.Missing.Value;
            object fileName = filePath;
            Microsoft.Office.Interop.Word._Application wordapp = new Microsoft.Office.Interop.Word.Application();
            Word._Document doc = wordapp.Documents.Open(ref fileName);
            string text = doc.Content.Text;
            doc.Close();
            wordapp.Quit();
            return text;
        }
        /// <summary>
        /// 提取PPT中的文字。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ParsePpt(string filePath)
        {
            //Ppt.Application pa = new Ppt.Application();
            ////Microsoft.Office.Core.MsoTriState 
            //Ppt.Presentation pp = pa.Presentations.Open(filePath);
            //StringBuilder sb = new StringBuilder();

            //foreach (Microsoft.Office.Interop.PowerPoint.Slide slide in pp.Slides)
            //{
            //    foreach (Microsoft.Office.Interop.PowerPoint.Shape shape in slide.Shapes)
            //        sb.Append(shape.TextFrame.TextRange.Text);
            //}
            //return sb.ToString();
            return "";
        }
        /// <summary>
        /// 提取PDF中的文字。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ParsePdf(string filePath)
        {
            //FileInfo fileInfo = new FileInfo(filePath);
            //PDDocument doc = PDDocument.load(fileInfo.FullName);
            //PDFTextStripper stripper = new PDFTextStripper();
            //return stripper.getText(doc);
               throw new Exception("to be fixed!");
        }
        /// <summary>
        /// 按照默认编码提取文本文字。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ParseTxt(string filePath)
        {
            return File.ReadAllText(filePath, Encoding.UTF8);
        }
        /// <summary>
        /// 根据路径名称自动判断文件类型，并提取文字。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string Parse(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            string entityText;

            //PDF 等文档的内容。
            switch (fileInfo.Extension.ToLower())
            {
                case ".pdf":
                    entityText = PDFReader.Instance.ParsePdf(fileName);//ParsePdf(fileName);
                    break;
                case ".doc":
                case ".docx":
                    entityText = ParseWord(fileName);
                    break;
                case ".ppt":
                case ".pptx":
                    entityText = ParsePpt(fileName);
                    break;
                case ".txt":
                    entityText = ParseTxt(fileName);
                    break;

                default:
                    throw new Exception( "提取内容没有实现这种类型！");
                    //break;
            }
            return entityText;
        }
    }


    /// <summary>
    /// PDF阅读器，必须在第一时间初始化才能使用，如在Program的Main中
    /// </summary>
    public class PDFReader
    {
        private static PDFReader instance = new PDFReader();

        public static PDFReader Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// 必须先初始化PDF阅读器才能使用之。在任何程序之前。
        /// </summary>
        public static void Init()
        {
            #region 初始化PDF阅读器
            //必须先初始化PDF阅读器才能使用之。
            string path = ".\\Docs\\ForPdfInit.pdf";
            string text = PDFReader.Instance.ParsePdf(path);
            // Console.Write(text);
            //Console.Read();
            #endregion
        }

        public string ParsePdf(string path)
        {
                //PDDocument doc = PDDocument.load(path);
                //PDFTextStripper pdfStripper = new PDFTextStripper();
                //string text = pdfStripper.getText(doc);
                //return text;
            throw new Exception("to be fixed!");
        }
    }
}
