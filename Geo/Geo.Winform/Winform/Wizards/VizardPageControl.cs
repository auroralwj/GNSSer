////2017.08.11, czs, create in hongqing, 向导页基类

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.IO;

namespace Geo.Winform.Wizards
{
    /// <summary>
    /// IWizardPage
    /// </summary>
    public partial class VizardPageControl : UserControl, IWizardPage
    {
        Log log = new Log(typeof(VizardPageControl));
        public VizardPageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 内容
        /// </summary>
        public UserControl Content { get { return this; } }

        public virtual void LoadPage()
        {
            log.Warn(" throw new NotImplementedException();");
        }

        public virtual void Save()
        {
            log.Warn(" throw new NotImplementedException();");
        }

        public virtual void Cancel()
        {
            log.Warn(" throw new NotImplementedException();");
        }

        public virtual bool IsBusy
        {
            get { return false; }
        }

        public virtual bool PageValid
        {
            get { return true; }
        }

        public virtual string ValidationMessage
        {
            get { return "当前页面输入无效，请仔细检查后再试"; }
        }
    }
}
