//2017.07.24, czs, create in hongqing, 具有日志监听功能的控件
//2017.10.26, czs, edit in hongqing, 封装功能，设计刷新间隔
//2018.12.22, czs, edit in hmx, 增加固定窗口选项

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.IO;
using Geo.Utils;


namespace Geo.Winform
{
    /// <summary>
    /// 日志监听显示器
    /// </summary>
    public partial class LogListenerControl : UserControl
    {
        protected ILog log = Log.GetLog(typeof(LogListenerControl));
        LogWriter LogWriter = LogWriter.Instance;
        
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public LogListenerControl()
        {
            InitializeComponent();
            
            enumCheckBoxControl1.Init<LogType>();

            enumCheckBoxControl1.Select<LogType>(LogType.Info, Setting.IsShowInfo);
            enumCheckBoxControl1.Select<LogType>(LogType.Error, Setting.IsShowError);
            enumCheckBoxControl1.Select<LogType>(LogType.Warn, Setting.IsShowWarning);
            enumCheckBoxControl1.Select<LogType>(LogType.Debug, Setting.IsShowDebug);

            enumCheckBoxControl1.EnumItemSelected += enumCheckBoxControl1_EnumItemSelected; 

            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Items.AddRange(EnumUtil.GetNames<LogType>());



            LogWriter.MsgProduced += LogWriter_MsgProduced;

            MinUpdateIntervalInMiniSeconds = 50;
            MaxCount = 5000;
            Data = new List<string>();
            LastUpdatedTime = DateTime.Now;
            locker = new object();

            PickBox.WireControl(label_info);
        }
          PickBox PickBox = new PickBox(); 
          
        //
        // 摘要: 
        //     在 System.Windows.Forms.Control.Text 属性值更改时发生。
        public event EventHandler TextChanged;
        #region 属性
        /// <summary>
        /// 最小更新时间间隔。避免频繁更新引起性能下降。单位：毫秒。
        /// </summary>
        public double MinUpdateIntervalInMiniSeconds { get; set; }
        /// <summary>
        /// 最大记录条数，超过此则删除前面的。默认8k条。
        /// </summary>
        public int MaxCount { get; set; }
        /// <summary>
        /// 记录上一次更新时间。
        /// </summary>
        public  DateTime LastUpdatedTime { get; set; }
        /// <summary>
        /// 是否有新的信息应该显示
        /// </summary>
        public bool HasNewMesssage { get; set; }
      
        /// <summary>
        /// 数据缓存
        /// </summary>
        public List<string> Data { get; set; }

        static object locker;
        #endregion
        /// <summary>
        /// 窗口是否固定
        /// </summary>
        public bool IsFixWindow => checkBox_fixWindow.Checked;

        void enumCheckBoxControl1_EnumItemSelected(string val, bool isSelected)
        {
            LogType type = (LogType)Enum.Parse(typeof(LogType), val);
            switch (type)
            {
                case LogType.Fatal:
                    Setting.IsShowFatal = isSelected;
                    break;
                case LogType.Debug:
                    Setting.IsShowDebug = isSelected;
                    break;
                case LogType.Info:
                    Setting.IsShowInfo = isSelected;
                    break;
                case LogType.Warn:
                    Setting.IsShowWarning = isSelected;
                    break;
                case LogType.Error:
                    Setting.IsShowError = isSelected;
                    break;
                default: break;
            }
            var state = isSelected ? "启用显示":"取消显示";
            log.Fatal(val + "  " + state);
        }

        private void LogListenerControl_Load(object sender, EventArgs e)
        { 
        }
        static  DateTime lastUpdateTime = DateTime.Now;
        static TimeSpan IntervalOfUpdating = TimeSpan.FromSeconds(0.5);
        /// <summary>
        /// 实现日志读取
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="LogType"></param>
        /// <param name="msgProducer"></param>
        protected virtual void LogWriter_MsgProduced(string msg, LogType LogType, Type msgProducer)
        {
            if (String.IsNullOrWhiteSpace(msg)) { return; }
          

            var info = LogType.ToString() + "\t" + DateTime.Now.ToString("HH:mm:ss.fff ") + "\t" + msg;
            if (Setting.VersionType == VersionType.Development) {  info += "\t" + msgProducer.Name; }
            //插入缓存
            Append(info);
            //实时显示
            switch (LogType)
            {
                case IO.LogType.Error:
                    this.richTextBoxControl1.LogError(info);
                    break;
                case IO.LogType.Warn:
                    this.richTextBoxControl1.LogWarning(info);
                    break;
                case IO.LogType.Fatal:
                    this.richTextBoxControl1.LogGreen(info);
                    break;
                case IO.LogType.Debug:
                    this.richTextBoxControl1.LogSmaller(info);
                    break;                    
                default:
                    this.richTextBoxControl1.LogMessage(info);
                    break;
            }

            if (DateTime.Now - lastUpdateTime > IntervalOfUpdating &&  this.IsHandleCreated && !this.IsDisposed) 
            { 
                this.Invoke(new Action(delegate ()
                {
                    int lineCount = this.richTextBoxControl1.Lines.Length;
                    Geo.Utils.FormUtil.SetText(this.label_info, "共 " + lineCount + " 条记录");
                    lastUpdateTime = DateTime.Now;
                }));
            }
            //  CheckOrUpdateInfoToUI();
        }

        private void InsertToTop(string info)
        {
            while (Data.Count > MaxCount)
            {
                Data.RemoveAt(Data.Count - 1);
            } 

            try
            {
                Data.Insert(0, info);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
        private void Append(string info)
        { 
            try
            {
                while (Data.Count > MaxCount)
                {
                    Data.RemoveAt(0);
                }
                Data.Add(info);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private void CheckOrUpdateInfoToUI()
        {
            var span = DateTime.Now - LastUpdatedTime;
            if (span.TotalMilliseconds >= MinUpdateIntervalInMiniSeconds)
            {
                var text = ListToLines();
                SetText(text);
                LastUpdatedTime = DateTime.Now;
                HasNewMesssage = false;
            }
            else
            {
                HasNewMesssage = true;
            }
        }
        
         
        /// <summary>
        /// 列表转换为字符串行
        /// </summary>
        /// <returns></returns>
        private string ListToLines()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in Data.ToArray())
            {
                sb.AppendLine(item);
            }
            var text = sb.ToString();
            return text;
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="info"></param>
        protected virtual void ShowInfo(string info)
        { 
            Geo.Utils.FormUtil.InsertLineWithTimeToTextBox(this.richTextBoxControl1, info);
        }

        private void SetText(string info)
        {
            Geo.Utils.FormUtil.SetText(this.richTextBoxControl1, info);
        }

        /// <summary>
        /// 移除监听器
        /// </summary>
        public void RomoveLogListner()
        {
            LogWriter.MsgProduced -= LogWriter_MsgProduced;
        }

        private void richTextBoxControl1_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null) { TextChanged(sender, e); }

        }
      
        private void toolStripLabel1_search_Click(object sender, EventArgs e)
        {
            var keyword = toolStripTextBox_keyword.Text;
            if (String.IsNullOrWhiteSpace(keyword)) { return; }

            StringBuilder sb = new StringBuilder();
            foreach (var item in Data.ToArray())
            {
                if (String.IsNullOrWhiteSpace(item)) { continue; }

                if (item.Contains(keyword))
                {
                    sb.AppendLine(item);
                }
            }
            this.SetText(sb.ToString());
            this.richTextBoxControl1.ScrollToStart(); 
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var filterText = toolStripComboBox1.SelectedItem.ToString();
            if (String.IsNullOrWhiteSpace(filterText)) { return; }

            var type = EnumUtil.Parse<LogType>(filterText);

            StringBuilder sb = new StringBuilder();
            foreach (var item in Data.ToArray())
            {
                if (String.IsNullOrWhiteSpace(item)) { continue; }

                var indexOf = item.IndexOf('\t');
                var text = item.Substring(0, indexOf);
                var tp = EnumUtil.Parse<LogType>(text);
                if (tp == type)
                {
                    sb.AppendLine(item);
                }
            }
            this.SetText(sb.ToString());
            this.richTextBoxControl1.ScrollToStart();    
        }

        #region 尺寸调整
        private void toolStripLabel_expand_Click(object sender, EventArgs e)
        {
            this.Height = this.Parent.Height / 3;
        }

        private void toolStripLabel_shrink_Click(object sender, EventArgs e)
        {
            this.Height = 25;
        }

        private void toolStripLabel_up_Click(object sender, EventArgs e)
        {
            this.Height += 90;
        }

        private void toolStripLabel_down_Click(object sender, EventArgs e)
        {
            this.Height -= 90;
        }
        bool isSizing = false; 

        private void toolStrip1_MouseHover(object sender, EventArgs e)
        {
            isSizing = true;
        } 
        private void toolStrip1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) isSizing = false;
        }
         
        private void toolStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && isSizing)
            {
                //鼠标相对于窗体的坐标
                Point p2 = toolStrip1.PointToClient(MousePosition);
                this.Height = this.Height - p2.Y + toolStrip1.Height/2;
            }
        }         

        private void LogListenerControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) isSizing = false;
        }
        #endregion

        //注意:代码中this是当前窗体,也就是给当前窗体加滚轮事件,如果你的是richtextebox控件,就给它加事件,还有滚轮事件触发条件必须是你的光标在richtextebox控件上才行.如果你光标在浏览器上或其他文本框上那肯定是不好使的.
        //public GetThreadForm()
        //{ 
        //    InitializeComponent();
        //    //加入窗体鼠标滚轮事件 
        //    this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.AutoMouseWheel);
          
        //}
        //private void AutoMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e) //窗体鼠标滚轮事件
        //{   
        //    //控制纵向滚动条滚动 
        //    this.AutoScrollPosition = new Point(this.HorizontalScroll.Value, this.VerticalScroll.Value + e.Delta);
        //} 
    }
}
