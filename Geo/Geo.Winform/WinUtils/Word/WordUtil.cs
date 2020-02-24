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
    /// ���ڲ��� Word �ı�ݹ��߷�����
    /// </summary>
     public static  class WordUtil
    {   
         /// <summary>
         /// System.Reflection.Missing.Value
         /// </summary>
         public static  object mis = System.Reflection.Missing.Value;

         /// <summary>
         /// ������ƶ����ļ�ͷ��
         /// </summary>
         public static void MoveStart(Microsoft.Office.Interop.Word.Application wordApp)
         {
             object story = Microsoft.Office.Interop.Word.WdUnits.wdStory;
             wordApp.Selection.HomeKey(ref story, ref mis);//�����Ƶ���
         }

         /// <summary>
         /// �ƶ���ָ��ҳ,����ǰ��������
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
         /// �����ı�
         /// </summary>
         /// <param name="text"></param>
         /// <param name="wordApp">word app</param>
         public static void InsertText(Microsoft.Office.Interop.Word.Application wordApp, string text)
         {
             wordApp.Selection.TypeText(text);
         }

         /// <summary>
         /// ����β��
         /// </summary>
         /// <param name="wordApp">word app</param>
         public static void ToLineEnd(Microsoft.Office.Interop.Word.Application wordApp)
         {
             object breakPage = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
             wordApp.Selection.EndOf(Word.WdUnits.wdLine);
         }

        /// <summary>
        /// ����Ĭ��һҳ����
         /// </summary>
         /// <param name="wordApp"></param>
         /// <param name="aboutSize"></param>
         public static  void SetLinesPage(Microsoft.Office.Interop.Word.Application wordApp,int size)
        {
            wordApp.ActiveDocument.PageSetup.LinesPage = size;
        } 
        /// <summary>
        /// �������ҳü
        /// </summary>
         /// <param name="context">����</param>
         /// <param name="wordApp">word app</param>
         public static  void SetPageHeader(Microsoft.Office.Interop.Word.Application wordApp,string context)
        {
            wordApp.ActiveWindow.View.Type = WdViewType.wdOutlineView;
            wordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryHeader;
            wordApp.ActiveWindow.ActivePane.Selection.InsertAfter(context);
            wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            //����ҳü����    
            wordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument;
        }
        /// <summary>
        /// ��ǰλ�ô���������
         /// </summary>
         /// <param name="wordApp">word app</param>
        /// <param name="context">��������</param>
        /// <param name="fontSize">�����С</param>
        /// <param name="fontColor">������ɫ</param>
        /// <param name="fontBold">����</param>
        /// <param name="familyName">����</param>
        /// <param name="align">���뷽��</param>
         public static  void InsertText(Microsoft.Office.Interop.Word.Application wordApp,string context, int fontSize, WdColor fontColor, int fontBold, string familyName, WdParagraphAlignment align)
        {
            //����������ʽ�Լ�����    
            wordApp.Application.Selection.Font.Size = fontSize;
            wordApp.Application.Selection.Font.Bold = fontBold;
            wordApp.Application.Selection.Font.Color = fontColor;
            wordApp.Selection.Font.Name = familyName;
            wordApp.Application.Selection.ParagraphFormat.Alignment = align;
            wordApp.Application.Selection.TypeText(context);
        }
        /// <summary>
        /// ��ҳ
        /// </summary>
         public static  void ToNextPage(Microsoft.Office.Interop.Word.Application wordApp)
        {
            object breakPage = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
            wordApp.Selection.InsertBreak(ref breakPage);
        }
        /// <summary>
        /// �����ƶ�count����
         /// </summary>
         /// <param name="wordApp">word app</param>
         /// <param name="count"></param>
         public static void MoveParagraph(Microsoft.Office.Interop.Word.Application wordApp, int count)
         {
             object _count = count;
             object wdP = WdUnits.wdParagraph;//��һ����
             wordApp.Selection.Move(ref wdP, ref _count);
         } 
        /// <summary>
        /// �����ƶ�count��
         /// </summary>
         /// <param name="wordApp">word app</param>
         /// <param name="count"></param>
         public static  void MoveRow(Microsoft.Office.Interop.Word.Application wordApp,int count)
        {
            object _count = count;
            object WdLine = WdUnits.wdLine;//��һ��
            wordApp.Selection.Move(ref WdLine, ref _count);
        }
        /// <summary>
        /// �����ƶ��ַ���
         /// </summary>
         /// <param name="wordApp">word app</param>
         /// <param name="count">�ƶ�����</param>
         public static  void MoveCharacter(Microsoft.Office.Interop.Word.Application wordApp,int count)
        {
            object _count = count;
            object wdCharacter = WdUnits.wdCharacter;
            wordApp.Selection.Move(ref wdCharacter, ref _count);
        }
        /// <summary>
        /// �������
         /// </summary>
         /// <param name="wordApp">word app</param>
         public static  void ToNextParagraph(Microsoft.Office.Interop.Word.Application wordApp)
        {
            wordApp.Selection.TypeParagraph();//�������
        }

        /// <summary>
        /// �س�����
        /// </summary>
         public static  void ToNextLine(Microsoft.Office.Interop.Word.Application wordApp)
        {
            wordApp.Selection.TypeParagraph();
        }
        /// <summary>
        /// ��ǰλ�ò���ͼƬ
         /// </summary>
         /// <param name="wordApp">word app</param>
        /// <param name="picture"></param>
         public static  void InsertPicture(Microsoft.Office.Interop.Word.Application wordApp,string picture)
        {
            //ͼƬ������ʾ    
            wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            wordApp.Application.Selection.InlineShapes.AddPicture(picture, ref mis, ref mis, ref mis);
        }
        /// <summary>
        /// ��ӱ��
        /// </summary>
         /// <param name="wordApp">word ����</param>
         /// <param name="_wordDocument">�ĵ�</param>
         /// <param name="rowNum">�����</param>
         /// <param name="cellNum">�����</param>
        /// <returns></returns>
         public static Table CreatTable(Microsoft.Office.Interop.Word.Application wordApp, Microsoft.Office.Interop.Word.Document _wordDocument, int rowNum, int cellNum)
        {
            return _wordDocument.Tables.Add(wordApp.Selection.Range, rowNum, cellNum, ref mis, ref mis);
        }
        /// <summary>
        /// �����п�
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
        /// �ϲ���Ԫ��
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
        /// ���õ�Ԫ������
        /// </summary>
         /// <param name="wordApp">word����</param>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="text">����</param>
        /// <param name="align">���뷽ʽ</param>
         public static  void SetCellValue(Microsoft.Office.Interop.Word.Application wordApp,Cell cell, string text, WdParagraphAlignment align)
        {
            wordApp.Selection.ParagraphFormat.Alignment = align;
            cell.Range.Text = text;
        }

         /// <summary>
         /// �������ļ�
         /// </summary>
         /// <param name="wordApp">word����</param>
         /// <param name="_wordDocument">�ĵ�����</param>
         /// <param name="path">����·��</param>
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
         /// �ͷ���Դ
         /// </summary>
         /// <param name="wordApp">Word App</param>
         /// <param name="_wordDocument">�ĵ�����</param>
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
         /// ��������
         /// </summary>
         /// <param name="wordTable"></param>
         public static void SortAscending(Word.Table wordTable)
         { 
                 wordTable.SortAscending();//���� 
         }
         /// <summary>
         /// ���û����ı����ʽ
         /// </summary>
         /// <param name="wordTable"></param>
         public static void SetTableStyle(Word.Table wordTable)
         {
             //���ñ���ʽ
             try
             {//2007
                 object styleName = "������";// "Table Grid";
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