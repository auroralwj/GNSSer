//2015.12.20, czs, create in hongqing, 提取主窗体接口，为多个版本做准备。


using System;

namespace Geo.Winform
{

    /// <summary>
    /// 具有 MainForm 属性。
    /// </summary>
    public interface IWithMainForm
    {
        /// <summary>
        /// 主页面属性
        /// </summary>
        IMainForm MainForm { get; set; }
    }

    /// <summary>
    /// 主窗体接口
    /// </summary>
   public interface IMainForm
    {
       /// <summary>
       /// 激活到当前显示窗口
       /// </summary>
       /// <param name="f"></param>
       /// <returns></returns>
        bool Activate(string f);
       /// <summary>
       /// 强制重新打开指定窗口
       /// </summary>
       /// <param name="title"></param>
       /// <param name="type"></param>
        void ForceOpenMidForm(string title, Type type);
       /// <summary>
        /// 强制重新打开指定窗口
       /// </summary>
       /// <param name="f"></param>
        void ForceOpenMidForm(System.Windows.Forms.Form f);
       /// <summary>
       /// 打开窗口，如果已经打开则激活该窗口
       /// </summary>
       /// <param name="title"></param>
       /// <param name="type"></param>
        void OpenMidForm(string title, Type type);
       /// <summary>
        /// 打开窗口，如果已经打开则激活该窗口
       /// </summary>
       /// <param name="f"></param>
        void OpenMidForm(System.Windows.Forms.Form f);
    }
}
