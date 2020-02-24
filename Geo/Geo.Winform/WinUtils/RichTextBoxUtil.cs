//2018.11.14, czs, create in hmx, RichTextBoxUtil工具

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Geo.Utils
{
    /// <summary>
    /// 工具
    /// </summary>
   public class RichTextBoxUtil
    {
        public static void AppendFormatedLine(RichTextBox rtBox, string text, Color color, int fontSizeIncreaser = -1)
        {
            text += Environment.NewLine;
            int start = rtBox.TextLength;
            int newTextLen = text.Length;
            rtBox.AppendText(text);
            rtBox.Select(start, newTextLen);

            var fontFamily = rtBox.Font.FontFamily;//.SelectionFont.FontFamily.Name;        
            var fontSize = rtBox.Font.Size + fontSizeIncreaser;
            rtBox.SelectionColor = color;
            rtBox.SelectionFont = new Font(fontFamily, fontSize);
        }
        public static void AppendFormatedLine(RichTextBox rtBox, string text, int fontSizeIncreaser = -1)
        {
            text += Environment.NewLine;
            int start = rtBox.TextLength;
            int newTextLen = text.Length;
            rtBox.AppendText(text);
            rtBox.Select(start, newTextLen);

            var fontFamily = rtBox.Font.FontFamily;//.SelectionFont.FontFamily.Name;        
            var fontSize = rtBox.Font.Size + fontSizeIncreaser;

            rtBox.SelectionFont = new Font(fontFamily, fontSize);
        }
        /// <summary>
        /// 增加带格式的字体
        /// </summary>
        /// <param name="rtBox"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="Style"></param>
        /// <param name="fontFamily"></param>
        /// <param name="fontSize"></param>
        /// <param name="addNewLine"></param>
        public static void AppendFormatedText(RichTextBox rtBox, string text, Color color, FontStyle Style= FontStyle.Regular, string fontFamily=null,float fontSize=-1, bool addNewLine = true)
        {
            if (addNewLine)
            {
                text += Environment.NewLine;
            }
            int start = rtBox.TextLength;
            int newTextLen = text.Length;
            rtBox.AppendText(text);
            rtBox.Select(start, newTextLen);
            rtBox.SelectionColor = color;
            if (fontFamily == null)
            {
                fontFamily = rtBox.Font.FontFamily.Name;
            }
            if(fontSize == -1)
            {
                fontSize = rtBox.Font.Size;
            }

            rtBox.SelectionFont = new Font(fontFamily, fontSize, Style);
        }
    }
}
