//2017.08.11, czs, edit in hongqing, 单独提出

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Service;
using Gnsser.Times;
using Geo.Times;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.Winform.Wizards;
 
namespace Gnsser.Winform
{
    /// <summary>
    /// GNSS 流程处理界面。
    /// </summary>
    public partial class BaseGnssProcessOptionPage : VizardPageControl
    {
        
        public BaseGnssProcessOptionPage(){
            InitializeComponent();
        }
         
        public BaseGnssProcessOptionPage(GnssProcessOption Option = null, GnssSolverType SolverType = GnssSolverType.无电离层组合PPP)
        {
            InitializeComponent();
            Init(Option, SolverType);
        }

        public void Init(GnssProcessOption Option, GnssSolverType SolverType = GnssSolverType.无电离层组合PPP)
        {
            if (Option == null)
            {
                Option =GnssProcessOptionManager.Instance[SolverType];
            }
            SetOption(Option); 
        } 

        public void SetOption(GnssProcessOption Option)
        {
            this.Option = Option; 
        }

        /// <summary>
        /// 选项
        /// </summary>
        public GnssProcessOption Option { get; set; }


        public virtual void UiToEntity()
        {
            if (Option == null) { Option = CreateNewModel(); }
                   
        }

        public virtual void EntityToUi()
        {
            if (Option == null) { Option = CreateNewModel(); }
        }

        public override void LoadPage()
        {
            //try
            //{
                EntityToUi();
            //}catch(Exception ex){
            //    Geo.Utils.FormUtil.ShowErrorMessageBox("转换到界面出错！" + ex.Message);
            //}
        }
        public override void Save()
        {
            try
            {
                UiToEntity();
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("界面输入不合法！" + ex.Message);
            }
        }
                 

        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
          
        }

        /// <summary>
        /// 计算选项。
        /// </summary>
        /// <returns></returns>
        public GnssProcessOption CreateNewModel()
        {  
            GnssProcessOption model = new GnssProcessOption(); 
            return model; 
        } 
    }
}