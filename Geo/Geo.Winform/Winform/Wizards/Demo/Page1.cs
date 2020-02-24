using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Winform.Wizards;

namespace Geo.Winform.Demo
{
    public partial class Page1 : UserControl, IWizardPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        #region IWizardPage Members

        public UserControl Content
        {
            get { return this; }
        }

        public new void LoadPage()
        {

        }

        public void Save()
        {
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public bool IsBusy
        {
            get { return false; }
        }

        public bool PageValid
        {
            get { return true; }
        }

        public string ValidationMessage
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
