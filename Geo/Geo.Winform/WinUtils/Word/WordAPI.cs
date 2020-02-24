using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.IO;
//using System.Windows.Forms;

using MSExcel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;

namespace Geo.Utils
{
    /// <summary>
    /// http://www.cnblogs.com/koolay/articles/1398110.html ,2012.6.15
    /// </summary>
    public class WordAPI
    {
        private object _template;
        private object _newWord;
        private Microsoft.Office.Interop.Word.Application wordApp;
        private Microsoft.Office.Interop.Word.Document _wordDocument;
        private object defaultV = System.Reflection.Missing.Value;
        private object documentType;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="template">ģ���ļ�λ��</param>
        /// <param name="newWord">����λ��</param>
        public WordAPI(string template, string newWord)
        {
            this._template = template;
            this._newWord = newWord;
            wordApp = new Application();
            documentType = Microsoft.Office.Interop.Word.WdDocumentType.wdTypeDocument;
            _wordDocument = wordApp.Documents.Add(ref _template, ref defaultV, ref documentType, ref defaultV);
        }
        /// <summary>
        /// ����Ĭ��һҳ����
        /// </summary>
        /// <param name="aboutSize"></param>
        public void SetLinesPage(int size)
        {
            wordApp.ActiveDocument.PageSetup.LinesPage = 40;
        }
        /// <summary>
        /// ������ǩ��ֵ
        /// </summary>
        /// <param name="markName">��ǩ��</param>
        /// <param name="markValue">��ǩֵ</param>
        public void SetBookMark(string markName, string markValue)
        {
            object _markName = markName;
            try
            {
                _wordDocument.Bookmarks.get_Item(ref _markName).Range.Text = markValue;
            }
            catch
            {
                throw new Exception(markName + "δ�ҵ�!!");
            }
        }
        /// <summary>
        /// �������ҳü
        /// </summary>
        /// <param name="context">����</param>
        public void SetPageHeader(string context)
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
        /// <param name="context">��������</param>
        /// <param name="fontSize">�����С</param>
        /// <param name="fontColor">������ɫ</param>
        /// <param name="fontBold">����</param>
        /// <param name="familyName">����</param>
        /// <param name="align">���뷽��</param>
        public void InsertText(string context, int fontSize, WdColor fontColor, int fontBold, string familyName, WdParagraphAlignment align)
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
        public void ToNextPage()
        {
            object breakPage = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
            wordApp.Selection.InsertBreak(ref breakPage);
        }
        /// <summary>
        /// �����ƶ�count����
        /// </summary>
        /// <param name="count"></param>
        public void MoveParagraph(int count)
        {
            object _count = count;
            object wdP = WdUnits.wdParagraph;//��һ����
            wordApp.Selection.Move(ref wdP, ref _count);
        }
        /// <summary>
        /// �����ƶ�count��
        /// </summary>
        /// <param name="count"></param>
        public void MoveRow(int count)
        {
            object _count = count;
            object WdLine = WdUnits.wdLine;//��һ��
            wordApp.Selection.Move(ref WdLine, ref _count);
        }
        /// <summary>
        /// �����ƶ��ַ���
        /// </summary>
        /// <param name="count"></param>
        public void MoveCharacter(int count)
        {
            object _count = count;
            object wdCharacter = WdUnits.wdCharacter;
            wordApp.Selection.Move(ref wdCharacter, ref _count);
        }
        /// <summary>
        /// �������
        /// </summary>
        public void ToNextParagraph()
        {
            wordApp.Selection.TypeParagraph();//�������
        }

        /// <summary>
        /// �س�����
        /// </summary>
        public void ToNextLine()
        {
            wordApp.Selection.TypeParagraph();
        }
        /// <summary>
        /// ��ǰλ�ò���ͼƬ
        /// </summary>
        /// <param name="picture"></param>
        public void InsertPicture(string picture)
        {
            //ͼƬ������ʾ    
            wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            wordApp.Application.Selection.InlineShapes.AddPicture(picture, ref defaultV, ref defaultV, ref defaultV);
        }
        /// <summary>
        /// ��ӱ��
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="cellNum"></param>
        /// <returns></returns>
        public Table CreatTable(int rowNum, int cellNum)
        {
            return this._wordDocument.Tables.Add(wordApp.Selection.Range, rowNum, cellNum, ref defaultV, ref defaultV);
        }
        /// <summary>
        /// �����п�
        /// </summary>
        /// <param name="widths"></param>
        /// <param name="tb"></param>
        public void SetColumnWidth(float[] widths, Table tb)
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
        /// <param name="tb"></param>
        /// <param name="cells"></param>
        public void MergeColumn(Table tb, Cell[] cells)
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
        /// <param name="_c"></param>
        /// <param name="v"></param>
        /// <param name="align">���뷽ʽ</param>
        public void SetCellValue(Cell _c, string v, WdParagraphAlignment align)
        {
            wordApp.Selection.ParagraphFormat.Alignment = align;
            _c.Range.Text = v;
        }

        /// <summary>
        /// �������ļ�
        /// </summary>
        public void SaveAsWord()
        {
            object doNotSaveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
            try
            {
                object fileFormat = WdSaveFormat.wdFormatRTF;
                _wordDocument.SaveAs(ref _newWord, ref fileFormat, ref defaultV, ref defaultV, ref defaultV, ref defaultV, ref defaultV, ref defaultV, ref defaultV,
                    ref defaultV, ref defaultV, ref defaultV, ref defaultV, ref defaultV, ref defaultV, ref defaultV);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }
            finally
            {
                disponse();
            }
        }
        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        private void disponse()
        {
            object missingValue = Type.Missing;
            object doNotSaveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
            ((Microsoft.Office.Interop.Word._Document)_wordDocument).Close(ref doNotSaveChanges);
            ((_Application)wordApp.Application).Quit(ref defaultV, ref defaultV, ref defaultV);
            _wordDocument = null;
            wordApp = null;
        }
    }
}