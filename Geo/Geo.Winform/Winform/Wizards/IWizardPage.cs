//2015.12.05, czs, edit in hongqing, 下一步页面接口

using System.Windows.Forms;

namespace Geo.Winform.Wizards
{
    /// <summary>
    /// 下一步页面接口
    /// </summary>
    public interface IWizardPage
    {
        /// <summary>
        /// 用户控件
        /// </summary>
        UserControl Content { get; }
        /// <summary>
        /// 加载数据
        /// </summary>
        void LoadPage();
        /// <summary>
        /// 保存数据
        /// </summary>
        void Save();
         /// <summary>
         /// 取消
         /// </summary>
        void Cancel();
        /// <summary>
        /// 是否正忙
        /// </summary>
        bool IsBusy { get; }
        /// <summary>
        /// 页面是否有效
        /// </summary>
        bool PageValid { get; }
        /// <summary>
        /// 验证信息
        /// </summary>
        string ValidationMessage { get; }
    }
}