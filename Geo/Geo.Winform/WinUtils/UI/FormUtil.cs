using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Geo.Winform;

namespace Geo.Utils
{
    /// <summary>
    /// 窗口工具类
    /// </summary>
    public static class FormUtil
    {
        #region 欢迎界面
        /// <summary>
        /// 信息系统
        /// </summary>
        public static string title = "信息系统";
        /// <summary>
        /// 欢迎界面
        /// </summary>
        public static void ShowSlpash()
        {
            string title = System.Configuration.ConfigurationManager.AppSettings["Title"];
            FormUtil.title = title;
            //splash
            System.Threading.Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(myStartingMethod));
            myThread.Start();
        }
        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="title"></param>
        public static void ShowSlpash(string title)
        {
            FormUtil.title = title;
            //splash
            System.Threading.Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(myStartingMethod));
            myThread.Start();
        }
        /// <summary>
        /// 显示提示
        /// </summary>
        public static void myStartingMethod()
        {
            SplashForm splashForm = new SplashForm
            {
                Title = title
            };
            splashForm.ShowDialog();
        }
        #endregion

        #region 显示等待界面
        /// <summary>
        /// 显示等待界面
        /// </summary>
        public static void Waitting()
        {
            //splash
            System.Threading.Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(showWaittingMethod));
            myThread.Start();
        }
        /// <summary>
        /// 显示等待
        /// </summary>
        public static void showWaittingMethod()
        { 
             WaitingForm waitingForm = new WaitingForm();
            waitingForm.ShowDialog();
        }
        #endregion
        #region 显示等待界面
        static string msg = "正在努力处理中，请稍后……";
        static double second = 5;
        /// <summary>
        /// 显示等待界面
        /// </summary>
        public static void ShowWaittingForm()
        {
            msg = "正在努力处理中，请稍后……";
            ShowWaittingForm(msg);
        } 
        /// <summary>
        /// 提示等待
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="second"></param>
        public static void ShowWaittingForm(string msg, double second =5)
        {
            FormUtil.msg = msg;
            FormUtil.second = second;
            System.Threading.Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(ShowWaittingMethod));
            myThread.Start();
        }
        /// <summary>
        /// 等待
        /// </summary>
        public static void ShowWaittingMethod()
        {
            WaitingForm waitingForm = new WaitingForm(msg, second);
            waitingForm.ShowDialog();
        }
        #endregion

        #region 信息框

        /// <summary>
        /// 提示没有权限窗口。
        /// </summary>
        public static void ShowNoRightBox()
        {
            Geo.Utils.FormUtil.ShowWarningMessageBox("对不起，你没有访问权限！");
        }

        /// <summary>
        /// 提示操作成功窗口。
        /// </summary>
        //public static void ShowOkMessageBox()
        //{
        //    MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}
        /// <summary>
        /// 请将信息输入完整
        /// </summary>
        public static void ShowNotEmptyMessageBox()
        {
            MessageBox.Show("请将信息输入完整！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// 提示警告
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowWarningMessageBox(string msg)
        {
            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// 错误提示窗口
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowErrorMessageBox(string msg)
        {
            MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } 
        /// <summary>
        /// OK提示
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowOkMessageBox(string msg = "操作成功！")
        {
            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 提示是否打开指定的文件或文件夹。
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="path"></param>
        public static void ShowIfOpenDirMessageBox(string path, string msg = "操作完成！\r\n是否打开")
        {
            if (msg == "操作完成！\r\n是否打开") msg = "操作完成！\r\n是否打开 " + path + " ？";
            if (ShowYesNoMessageBox(msg) == DialogResult.Yes)
                FileUtil.OpenFileOrDirectory(path);  
        }


        /// <summary>
        /// 处理异常，可以抛出查看详情
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="msg"></param>
        public static void HandleException(Exception ex, string msg)
        {
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox(msg + "\r\n是否抛出异常？")
                 == System.Windows.Forms.DialogResult.Yes)
            {
                throw ex;
            }
            else
            {
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 提示选择
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult ShowYesNoMessageBox(string msg)
        {
            return MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }
        #endregion

        #region 输入框
        /// <summary>
        /// 插入信息
        /// </summary>
        /// <param name="textBoxBase"></param>
        /// <param name="info"></param>
        /// <param name="maxAllowCount"></param>
        public static void InsertLineWithTimeToTextBox(TextBoxBase textBoxBase, string info, int maxAllowCount = 5000)
        {
            info = DateTimeUtil.GetFormatedTimeNow(true) + ":\t" + info;
            FormUtil.InsertLineToTextBox(textBoxBase, info, maxAllowCount);
        }
        /// <summary>
        /// 在第一条插入行,异常也不会报错！
        /// </summary>
        /// <param name="textBoxBase"></param>
        /// <param name="info"></param>
        /// <param name="maxAllowCount"></param>
        public static void InsertLineToTextBox(TextBoxBase textBoxBase, string info, int maxAllowCount = 5000)
        {
            try
            {
                if (textBoxBase == null || textBoxBase.IsDisposed)
                {
                    return;
                }
                textBoxBase.Invoke(new Action(delegate()
                {
                    var count = textBoxBase.Lines.Length;

                    if (count >= maxAllowCount)
                    {
                        List<string> lines = new List<string>(textBoxBase.Lines);
                        lines.RemoveAt(count - 1);
                        lines.Insert(0, info);
                        textBoxBase.Lines = lines.ToArray();
                    }
                    else
                    {
                        textBoxBase.Text = info + "\r\n" + textBoxBase.Text;
                    }

                }));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
        static DateTime lastNoticeTime = DateTime.Now;
        //最小显示时间间隔
        static int MinNoticeInterval = 1; 

        /// <summary>
        /// 显示通知，基于控件。此处具有时间控制，避免过于频繁。
        /// </summary>
        /// <param name="control"></param>
        /// <param name="info"></param> 
        /// <param name="isControlFrequence"></param> 
        public static void ShowNotice(Control control, string info, bool isControlFrequence  =true)
        {
            var now = DateTime.Now;
            if (isControlFrequence && (now - lastNoticeTime).TotalSeconds < MinNoticeInterval)
            {
                return;
            }
            lastNoticeTime = now;
            SetText(control, info);
        }
        /// <summary>
        /// 直接设置文本，基于控件。不会报错。
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param> 
        public static void SetText(Control control, string text)
        {
            try
            {
                if (control.IsHandleCreated)
                {
                    control.Invoke(new Action(delegate()
                    {
                        control.Text = text;
                    }));
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        } 
        /// <summary>
        /// 添加一条到最后
        /// </summary>
        /// <param name="TextBoxBase"></param>
        /// <param name="info"></param>
        /// <param name="maxAllowCount"></param>
        public static void AppendLineToTextBox(TextBoxBase TextBoxBase, string info, int maxAllowCount = 5000)
        {
            TextBoxBase.Invoke(new Action(delegate()
            { 
            var count = TextBoxBase.Lines.Length;

            if (count >= maxAllowCount)
            {
                List<string> lines = new List<string>(TextBoxBase.Lines);
                lines.RemoveAt(count - 1);
                lines.Add(info);
                TextBoxBase.Lines = lines.ToArray();
            }
            else
            {
                TextBoxBase.Text =TextBoxBase.Text +  info + "\r\n";
            }
            }));
        }
        /// <summary>
        ///  弹出一个文本输入框,输入两个数据。
        /// </summary>
        /// <param name="titles"></param>
        /// <param name="outVals"></param>
        /// <param name="formTitle"></param>
        /// <param name="initVals"></param>
        /// <returns></returns>
        public static bool ShowTwoValueInputForm(StringPair titles, out NumerialPair outVals, NumerialPair initVals = null, string formTitle ="请输入")
        { 
            if(initVals == null) { initVals = NumerialPair.Zero; }
            var f = new InputTwoValueForm( );
            f.SetValue(initVals).SetTitle(titles);
            f.Text = formTitle;
            if (f.ShowDialog() == DialogResult.OK)
            {
                outVals = f.Numerials;
                return true;
            }
            outVals = NumerialPair.Zero;
            return false;
        }
        public static bool ShowThreeValueInputForm(StringTriple titles, out NumerialTriple outVals, NumerialTriple initVals = null, string formTitle = "请输入")
        {
            if (initVals == null) { initVals = NumerialTriple.Zero; }
            var f = new InputThreeValueForm();
            f.SetValue(initVals).SetTitle(titles);
            f.Text = formTitle;
            if (f.ShowDialog() == DialogResult.OK)
            {
                outVals = f.Numerials;
                return true;
            }
            outVals = NumerialTriple.Zero;
            return false;
        }
        /// <summary>
        ///  弹出一个文本输入框,输入两个数据。
        /// </summary>
        /// <param name="titles"></param>
        /// <param name="outVals"></param>
        /// <param name="initVals"></param>
        /// <param name="formTitle"></param>
        /// <returns></returns>
        public static bool ShowTwoStringInputForm(StringPair titles, out StringPair outVals, StringPair initVals = null, string formTitle ="请输入")
        { 
            if(initVals == null) { initVals = StringPair.Empty; }
            var f = new InputTwoStringForm( );
            f.SetValue(initVals).SetTitle(titles);
            f.Text = formTitle;
            if (f.ShowDialog() == DialogResult.OK)
            {
                outVals = f.NumerialPair;
                return true;
            }
            outVals = StringPair.Empty;
            return false;
        }
         
        /// <summary>
        /// 弹出一个框,选择。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="outEnum"></param>
        /// <param name="initEnm"></param>
        /// <param name="formTitle"></param>
        /// <returns></returns>
        public static bool ShowAndSelectEnumRadioForm<TEnum>(out TEnum outEnum, TEnum initEnm, string formTitle ="请选择")
        {
            EnumRadioForm form = new EnumRadioForm();
            form.Text = formTitle;
            form.Init<TEnum>();
            form.SetCurrent<TEnum>(initEnm);
            if (form.ShowDialog() == DialogResult.OK)
            {
                outEnum =(TEnum)Enum.Parse(typeof(TEnum), form.CurrentText);
                return true;
            }
            outEnum = initEnm;
            return false;
        }

        /// <summary>
        /// 弹出一个框,选择。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="outEnum"></param>
        /// <param name="initEnm"></param>
        /// <param name="formTitle"></param>
        /// <returns></returns>
        public static bool ShowAndSelectEnumsForm<TEnum>(out List<TEnum> outEnum, List<TEnum> initEnm = null, string formTitle ="请选择")
        {
            List<string> initSelect = new List<string>();
            if (initEnm != null)
            {
                foreach (var item in initEnm)
                {
                    initSelect.Add(item.ToString());
                }
            }
            SelectMultiNameForm form = new SelectMultiNameForm();
            form.Text = formTitle;
            form.Init(Enum.GetNames(typeof(TEnum)));
            form.SetSelected(initSelect);
            if (form.ShowDialog() == DialogResult.OK)
            {
                outEnum = new List<TEnum>();
                foreach (var item in form.SelectedNames)
                {
                    outEnum.Add((TEnum)(Enum.Parse(typeof(TEnum), item)));
                } 
                return true;
            }
            outEnum = initEnm;
            return false;
        }
        /// <summary>
        /// 获取输入文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="initEnm"></param>
        /// <param name="formTitle"></param>
        /// <returns></returns>
        public static bool ShowTextInputForm(out string text, string initEnm = null, string formTitle ="请输入")
        {
            text = "";
            TextInputForm form = new TextInputForm(initEnm);
            form.Text = formTitle;
            if (form.ShowDialog() == DialogResult.OK)
            {
                text = form.TextValue;
                return true;
            } 
            
            return false;
        }


        /// <summary>
        /// 弹出一个文本输入框。
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static bool ShowInputForm(out string inputValue)
        {
            return ShowInputForm("请输入", out  inputValue);
        }
        /// <summary>
        ///  显示输入数值的对话框，失败或默认返回0。
        /// </summary>
        /// <param name="formTitle"></param>
        /// <param name="inputValue"></param>
        /// <param name="initVal"></param>
        /// <returns></returns>
        public static bool ShowInputNumeralForm(string formTitle, out double inputValue, double initVal = 0)
        {
            string initValue = initVal +"";
            string inputValueS ;
            if( ShowInputForm(formTitle, initValue, out inputValueS))
            {
                inputValue = Double.Parse(inputValueS);
                return true;
            }
            inputValue = 0;
            return false;
        }
        /// <summary>
        ///  显示输入对话框
        /// </summary>
        /// <param name="formTitle"></param>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static bool ShowInputForm(string formTitle, out string inputValue)
        {
            string initValue = "请在此输入";
            return ShowInputForm(formTitle, initValue, out inputValue);
        }
        /// <summary>
        /// 显示输入对话框
        /// </summary>
        /// <param name="formTitle"></param>
        /// <param name="initValue"></param>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static bool ShowInputForm(string formTitle, string initValue, out string inputValue)
        {
            List<string> canNotBeValues = new List<string>();
            return ShowInputForm(formTitle, initValue, canNotBeValues, out inputValue);
        }
        /// <summary>
        /// 显示输入对话框
        /// </summary>
        /// <param name="formTitle"></param>
        /// <param name="initValue"></param>
        /// <param name="canNotBeValues"></param>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static bool ShowInputForm(string formTitle, string initValue, List<string> canNotBeValues, out string inputValue)
        {
            return ShowInputForm(formTitle, initValue, canNotBeValues, "该名称已经存在，请换一个 " ,out inputValue);
        }
        /// <summary>
        /// 显示输入对话框
        /// </summary>
        /// <param name="formTitle">窗口题目</param>
        /// <param name="initValue">初始值</param>
        /// <param name="canNotBeValues">不能为数据</param>
        /// <param name="canNotBeWarnMsg">不能为数据提示</param>
        /// <param name="inputValue">输入的数据</param>
        /// <returns></returns>
        public static bool ShowInputForm(string formTitle, string initValue, List<string> canNotBeValues, string canNotBeWarnMsg, out string inputValue)
        {
            Geo.Utils.OneTextInputForm f = new Geo.Utils.OneTextInputForm(initValue, canNotBeValues, canNotBeWarnMsg);
            f.Text = formTitle;
            if (f.ShowDialog() == DialogResult.OK)
            {
                inputValue = f.InputValue;
                return true;
            }
            inputValue = "";
            return false;
        }
         /// <summary>
         /// 一个对话框，提示输入行。
         /// </summary>
         /// <param name="formTitle"></param>
         /// <param name="lines"></param>
         /// <returns></returns>
        public static bool ShowInputLineForm(string formTitle, out string [] lines)
        {
            Geo.Utils.InputLinesForm f = new Geo.Utils.InputLinesForm();
            f.Text = formTitle;
            if (f.ShowDialog() == DialogResult.OK)
            {
                lines = f.Lines;
                return true;
            }
            lines = new string []{ "" };
            return false;
        }
        #endregion

        #region 初始化进度条
        /// <summary>
        /// 初始化 ProgressBar 的值。step为1。
        /// </summary>
        /// <param name="progressBar1"></param>
        /// <param name="max"></param>
        public static void InitProgressBar(ProgressBar progressBar1, int max)
        {
            progressBar1.Maximum = max;
            progressBar1.Minimum = 1;
            progressBar1.Value = progressBar1.Minimum;
            progressBar1.Step = 1;
        }

        #endregion


        #region 文件操作
        /// <summary>
        /// 弹出保存文件对话框，并保存文本文档。
        /// 保存成功后，提示是否打开文件夹。
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="filter">过滤器</param>
        public static void ShowFormSaveTextFileAndIfOpenFolder(string content, string fileName = "导出结果", string filter = "文本文件|*.txt|任何文件|*.*")
        {
            string dir = Path.GetDirectoryName(ShowFormSaveTextFile(content, fileName, filter));
            if (Directory.Exists(dir))
                Geo.Utils.FormUtil.ShowOkAndOpenDirectory(dir);
        }
        /// <summary>
        /// 弹出保存文件对话框，并保存文本文档。
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="filter">过滤器</param>
        public static string ShowFormSaveTextFile(string content, string fileName = "导出结果", string filter = "文本文件|*.txt|任何文件|*.*")
        {
            SaveFileDialog d = new SaveFileDialog
            {
                Filter = filter,
                FileName = fileName
            };
            if (d.ShowDialog() == DialogResult.OK)
            {
                Utils.FileUtil.WriteText(d.FileName, content);
            }
            return d.FileName;
        }   
        /// <summary>
        /// 弹出窗口，批量选择文件，返回文件路径，若无返回null。
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string [] ShowFormGetFilePathes(string filter = "文本文件|*.txt|任何文件|*.*")
        {
            OpenFileDialog d = new OpenFileDialog
            {
                Multiselect = true,
                Filter = filter
            };
            if (d.ShowDialog() == DialogResult.OK)
            {
                 return  (d.FileNames);
            }
            return null;
        }

        /// <summary>
        /// 弹出窗口，选择文件，返回文件路径，若无返回null。
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string ShowFormGetFilePath(string filter = "文本文件|*.txt|任何文件|*.*", string fileName = "")
        {
            OpenFileDialog d = new OpenFileDialog();
            d.FileName = fileName;
            d.Filter = filter;
            if (d.ShowDialog() == DialogResult.OK)
            {
                 return  (d.FileName);
            }

            return null;
        }

        /// <summary>
        /// 弹出窗口，选择文本，返回读取的内容，若无返回null。
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string ShowFormReadTextFile(string filter = "文本文件|*.txt|任何文件|*.*")
        { 
            var path = ShowFormGetFilePath(filter);
            if (path  != null)
            {
                if (!File.Exists(path)) { throw new FileNotFoundException(path); }

                return File.ReadAllText(path);
            }

            return null;
        }

        /// <summary>
        /// 弹出窗口，选择文本，返回读取的行内容
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string [] ShowFormReadTextLinesFile(string filter = "文本文件|*.txt|任何文件|*.*")
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = filter;
            if (d.ShowDialog() == DialogResult.OK)
            {
                return File.ReadAllLines(d.FileName);
            }

            return null;
        }

        /// <summary>
        /// 提示信息，并询问是否打开目录。如果输入的是文件，则打开所在目录。
        /// </summary>
        /// <param name="inDirPath"></param>
        /// <param name="msg"></param>
        public static void ShowOkAndOpenDirectory(string inDirPath, string msg = "执行完毕，是否打开所在目录？")
        {
            if (MessageBox.Show(msg, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                == DialogResult.OK)
            {               
                FileUtil.OpenDirectory(inDirPath);
            }
        }
        /// <summary>
        /// 显示OK并打开文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="msg"></param>
        public static void ShowOkAndOpenFile(string filePath, string msg = "执行完毕，是否打开文件？")
        {
            if (MessageBox.Show(msg, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                == DialogResult.OK)
            {
                FileUtil.OpenFile(filePath);
            }
        }
        /// <summary>
        /// 提示文件不存在。
        /// </summary>
        /// <param name="filePath"></param>
        public static void ShowFileNotExistBox(string filePath)
        {
            ShowErrorMessageBox("指定文件不存在！\r\n" + filePath);
        }
        #endregion

        /// <summary>
        ///可能出错的执行。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="ignoreException"></param>
        public static  void TryExecute(Action action, bool ignoreException)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                if (!ignoreException) ShowErrorMessageBox("出错了：" + ex.Message);
            }
        }
         
        /// <summary>
        /// 等待线程并赋值
        /// </summary>
        /// <param name="textBox">显示的文本框</param>
        /// <param name="str">显示文本</param>
        /// <param name="isAppend">是否添加，否则直接赋值</param>
        public static void InvokeTextBoxSetText(TextBoxBase textBox, string str, bool isAppend = false)
        {
            try
            {
                textBox.Invoke(new Action(delegate()
                {
                    if (isAppend)
                        textBox.Text += str;
                    else textBox.Text = str;
                }));

            }
            catch (Exception ex) { }
        } 
        /// <summary>
        /// 弹出一个窗口，内容是内存使用情况。
        /// </summary>
        public static void ShowMemoryStatusBox()
        {
            MemoryStatus MemoryStatus = MemoryUtil.GetMemoryStatus();
            StringBuilder sb = new StringBuilder();
            double Gb = 1024 * 1024 * 1024;
            sb.AppendLine("MemoryStatus.TotalPhysical:" + MemoryStatus.TotalPhysical / Gb);
            sb.AppendLine("MemoryStatus.AvailablePhysical:" + MemoryStatus.AvailablePhysical / Gb);
            sb.AppendLine("MemoryStatus.Length:" + MemoryStatus.Length / Gb);
            MessageBox.Show(sb.ToString());
        }


        /// <summary>
        /// 检查文件存在性，如果不存在则提示，并返回false.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CheckExistOrShowWarningForm(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("文件不存在，" + FilePath);
                return false;
            }
            return true;
        }

        #region 弹窗选择
        /// <summary>
        /// 弹出对话框选择名称列表
        /// </summary>
        /// <param name="titleList"></param>
        /// <returns></returns>
        public static List<string> OpenFormSelectTitles( IEnumerable<string> titleList, bool isSelectAll = true)
        { //
            List<string> list = new List<string>();
            SelectMultiNameForm form = new SelectMultiNameForm(titleList.ToArray(), isSelectAll);
            if (form.ShowDialog() == DialogResult.OK) list = form.SelectedNames;
            return list;
        }
        /// <summary>
        /// 弹出对话框选择一个名称
        /// </summary>
        /// <param name="titleList"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string OpenFormSelectOne(IEnumerable<string> titleList, string title = null)
        { 
            string list = "";
            var form = new RadioSelectingForm(titleList.ToArray());
            if(title != null) { form.Text = title; }
            if (form.ShowDialog() == DialogResult.OK) list = form.SelectedValue;
            return list;
        }
        /// <summary>
        /// 弹出对话框选择2个名称，否则返回null
        /// </summary>
        /// <param name="itemsA"></param>
        /// <param name="itemsB"></param>
        /// <param name="titleA"></param>
        /// <param name="titleB"></param>
        /// <returns></returns>
        public static StringPair OpenFormSelectPair(IEnumerable<string> itemsA, IEnumerable<string> itemsB, string titleA = "待选项目A", string titleB = "待选项目B")
        {  
            StringPair list = new StringPair();
            var form = new TwoRadioSelectingForm(itemsA.ToArray(), itemsB.ToArray(), titleA, titleB);
            if(titleA != null) { form.Text = titleA; }
            if (form.ShowDialog() == DialogResult.OK) { list = form.SelectedValue; }
            else { return null; }

            return list;

            
        }
        /// <summary>
        /// 弹出对话框选择一个名称
        /// </summary> 
        /// <returns></returns>
        public static bool OpenFormSelectOne<TEnum>(out TEnum resultEnum)
        {
            var result = OpenFormSelectOne(Enum.GetNames(typeof(TEnum)));
            if (String.IsNullOrWhiteSpace(result)) { resultEnum = default(TEnum); return false; }
            resultEnum = (TEnum)Enum.Parse(typeof(TEnum), result);
            return true;
        }

 
        #endregion
    }
}
