using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

using MSExcel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;

namespace Geo.Utils
{ 
    /// <summary>
    /// 用于操作 Word 的便捷工具方法。
    /// </summary>
     public static  class WordUtil
    {   
         /// <summary>
         /// System.Reflection.Missing.Value
         /// </summary>
         public static  object mis = System.Reflection.Missing.Value;

         /// <summary>
         /// 将光标移动至文件头部
         /// </summary>
         public static void MoveStart(Microsoft.Office.Interop.Word.Application wordApp)
         {
             object story = Microsoft.Office.Interop.Word.WdUnits.wdStory;
             wordApp.Selection.HomeKey(ref story, ref mis);//向上移到顶
         }

         /// <summary>
         /// 移动到指定页,正往前，负往后。
         /// </summary>
         /// <param name="wordApp"></param>
         /// <param name="count"></param>
         private static void MovePage(Microsoft.Office.Interop.Word.Application wordApp,int count)
         {
             bool isnext = count >0;
             count = Math.Abs(count);
             for (int i = 0; i < count; i++)
             {
                 Microsoft.Office.Interop.Word.WdGoToItem goPage = Microsoft.Office.Interop.Word.WdGoToItem.wdGoToPage;
                 if(isnext)   wordApp.Selection.GoToNext(goPage);
                 else  wordApp.Selection.GoToPrevious(goPage);
             }
         }

         /// <summary>
         /// 插入文本
         /// </summary>
         /// <param name="text"></param>
         /// <param name="wordApp">word app</param>
         public static void InsertText(Microsoft.Office.Interop.Word.Application wordApp, string text)
         {
             wordApp.Selection.TypeText(text);
         }

         /// <summary>
         /// 到行尾部
         /// </summary>
         /// <param name="wordApp">word app</param>
         public static void ToLineEnd(Microsoft.Office.Interop.Word.Application wordApp)
         {
             object breakPage = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
             wordApp.Selection.EndOf(Word.WdUnits.wdLine);
         }

        /// <summary>
        /// 设置默认一页行数
         /// </summary>
         /// <param name="wordApp"></param>
         /// <param name="aboutSize"></param>
         public static  void SetLinesPage(Microsoft.Office.Interop.Word.Application wordApp,int size)
        {
            wordApp.ActiveDocument.PageSetup.LinesPage = size;
        } 
        /// <summary>
        /// 设置添加页眉
        /// </summary>
         /// <param name="context">内容</param>
         /// <param name="wordApp">word app</param>
         public static  void SetPageHeader(Microsoft.Office.Interop.Word.Application wordApp,string context)
        {
            wordApp.ActiveWindow.View.Type = WdViewType.wdOutlineView;
            wordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryHeader;
            wordApp.ActiveWindow.ActivePane.Selection.InsertAfter(context);
            wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            //跳出页眉设置    
            wordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument;
        }
        /// <summary>
        /// 当前位置处插入文字
         /// </summary>
         /// <param name="wordApp">word app</param>
        /// <param name="context">文字内容</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="fontColor">字体颜色</param>
        /// <param name="fontBold">粗体</param>
        /// <param name="familyName">字体</param>
        /// <param name="align">对齐方向</param>
         public static  void InsertText(Microsoft.Office.Interop.Word.Application wordApp,string context, int fontSize, WdColor fontColor, int fontBold, string familyName, WdParagraphAlignment align)
        {
            //设置字体样式以及方向    
            wordApp.Application.Selection.Font.Size = fontSize;
            wordApp.Application.Selection.Font.Bold = fontBold;
            wordApp.Application.Selection.Font.Color = fontColor;
            wordApp.Selection.Font.Name = familyName;
            wordApp.Application.Selection.ParagraphFormat.Alignment = align;
            wordApp.Application.Selection.TypeText(context);
        }
        /// <summary>
        /// 翻页
        /// </summary>
         public static  void ToNextPage(Microsoft.Office.Interop.Word.Application wordApp)
        {
            object breakPage = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
            wordApp.Selection.InsertBreak(ref breakPage);
        }
        /// <summary>
        /// 焦点移动count段落
         /// </summary>
         /// <param name="wordApp">word app</param>
         /// <param name="count"></param>
         public static void MoveParagraph(Microsoft.Office.Interop.Word.Application wordApp, int count)
         {
             object _count = count;
             object wdP = WdUnits.wdParagraph;//换一段落
             wordApp.Selection.Move(ref wdP, ref _count);
         } 
        /// <summary>
        /// 焦点移动count行
         /// </summary>
         /// <param name="wordApp">word app</param>
         /// <param name="count"></param>
         public static  void MoveRow(Microsoft.Office.Interop.Word.Application wordApp,int count)
        {
            object _count = count;
            object WdLine = WdUnits.wdLine;//换一行
            wordApp.Selection.Move(ref WdLine, ref _count);
        }
        /// <summary>
        /// 焦点移动字符数
         /// </summary>
         /// <param name="wordApp">word app</param>
         /// <param name="count">移动数量</param>
         public static  void MoveCharacter(Microsoft.Office.Interop.Word.Application wordApp,int count)
        {
            object _count = count;
            object wdCharacter = WdUnits.wdCharacter;
            wordApp.Selection.Move(ref wdCharacter, ref _count);
        }
        /// <summary>
        /// 插入段落
         /// </summary>
         /// <param name="wordApp">word app</param>
         public static  void ToNextParagraph(Microsoft.Office.Interop.Word.Application wordApp)
        {
            wordApp.Selection.TypeParagraph();//插入段落
        }

        /// <summary>
        /// 回车换行
        /// </summary>
         public static  void ToNextLine(Microsoft.Office.Interop.Word.Application wordApp)
        {
            wordApp.Selection.TypeParagraph();
        }
        /// <summary>
        /// 当前位置插入图片
         /// </summary>
         /// <param name="wordApp">word app</param>
        /// <param name="picture"></param>
         public static  void InsertPicture(Microsoft.Office.Interop.Word.Application wordApp,string picture)
        {
            //图片居中显示    
            wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            wordApp.Application.Selection.InlineShapes.AddPicture(picture, ref mis, ref mis, ref mis);
        }
        /// <summary>
        /// 添加表格
        /// </summary>
         /// <param name="wordApp">word 程序</param>
         /// <param name="_wordDocument">文档</param>
         /// <param name="rowNum">表格编号</param>
         /// <param name="cellNum">表格编号</param>
        /// <returns></returns>
         public static Table CreatTable(Microsoft.Office.Interop.Word.Application wordApp, Microsoft.Office.Interop.Word.Document _wordDocument, int rowNum, int cellNum)
        {
            return _wordDocument.Tables.Add(wordApp.Selection.Range, rowNum, cellNum, ref mis, ref mis);
        }
        /// <summary>
        /// 设置列宽
         /// </summary>
         /// <param name="widths"></param>
         /// <param name="tb"></param>
         public static  void SetColumnWidth(float[] widths, Table tb)
        {
            if (widths.Length > 0)
            {
                int len = widths.Length;
                for (int i = 0; i < len; i++)
                {
                    tb.Columns[i].Width = widths[i];
                }
            }
        }
        /// <summary>
        /// 合并单元格
        /// </summary>
         /// <param name="wordApp">word app</param>
        /// <param name="tb"></param>
        /// <param name="cells"></param>
         public static  void MergeColumn(Microsoft.Office.Interop.Word.Application wordApp,Table tb, Cell[] cells)
        {
            if (cells.Length > 1)
            {
                Cell c = cells[0];
                int len = cells.Length;
                for (int i = 1; i < len; i++)
                {
                    c.Merge(cells[i]);
                }
            }
            wordApp.Selection.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

        }
        /// <summary>
        /// 设置单元格内容
        /// </summary>
         /// <param name="wordApp">word程序</param>
        /// <param name="cell">单元格</param>
        /// <param name="text">内容</param>
        /// <param name="align">对齐方式</param>
         public static  void SetCellValue(Microsoft.Office.Interop.Word.Application wordApp,Cell cell, string text, WdParagraphAlignment align)
        {
            wordApp.Selection.ParagraphFormat.Alignment = align;
            cell.Range.Text = text;
        }

         /// <summary>
         /// 保存新文件
         /// </summary>
         /// <param name="wordApp">word程序</param>
         /// <param name="_wordDocument">文档对象</param>
         /// <param name="path">保存路径</param>
         public static  void SaveAsWord(Microsoft.Office.Interop.Word.Application wordApp,Microsoft.Office.Interop.Word.Document _wordDocument, object path)
      
        {
            object doNotSaveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
            try
            {
                object fileFormat = WdSaveFormat.wdFormatRTF;
                _wordDocument.SaveAs(ref path, ref fileFormat, ref mis, ref mis, ref mis, ref mis, ref mis, ref mis, ref mis,
                    ref mis, ref mis, ref mis, ref mis, ref mis, ref mis, ref mis);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }
            finally
            {
                disponse(wordApp, _wordDocument);
            }
        }
         /// <summary>
         /// 释放资源
         /// </summary>
         /// <param name="wordApp">Word App</param>
         /// <param name="_wordDocument">文档对象</param>
         public static  void disponse(Microsoft.Office.Interop.Word.Application wordApp,Microsoft.Office.Interop.Word.Document _wordDocument)
        {
            object missingValue = Type.Missing;
            object doNotSaveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
           ((Microsoft.Office.Interop.Word . _Document) _wordDocument).Close(ref doNotSaveChanges);
           ((_Application)wordApp.Application).Quit(ref mis, ref mis, ref mis);
            _wordDocument = null;
            wordApp = null;
        }
         /// <summary>
         /// 升序排序
         /// </summary>
         /// <param name="wordTable"></param>
         public static void SortAscending(Word.Table wordTable)
         { 
                 wordTable.SortAscending();//排序 
         }
         /// <summary>
         /// 设置基本的表格样式
         /// </summary>
         /// <param name="wordTable"></param>
         public static void SetTableStyle(Word.Table wordTable)
         {
             //设置表格格式
             try
             {//2007
                 object styleName = "网格型";// "Table Grid";
                 wordTable.Range.Font.Size = 10;
                 wordTable.set_Style(ref styleName);
             }
             catch (Exception e)
             {//2003
                 try
                 {
                     object styleName = "Table Grid";
                     //    wordTable.Range.Font.Size = 8;
                     wordTable.set_Style(ref styleName);
                 }
                 catch (Exception e2)
                 {
                     Console.WriteLine(e2.Message + e.Message);
                 }
             }
         }
    }
}