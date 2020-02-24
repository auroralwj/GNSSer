//2019.01.13, czs, create in hmx, 多算法设置
//2019.01.18, czs, create in hmx, 增加PPP，设为自动设置

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gnsser;

namespace Gnsser.Winform.Controls
{
    public partial class MultiSolverOptionControl : UserControl
    {
        public MultiSolverOptionControl()
        {
            InitializeComponent();
            Options = new Dictionary<GnssSolverType, GnssProcessOption>(GnssProcessOptionManager.Instance.Data);
            foreach (var option in Options.Values)
            {
                GettingOutputDirectory?.Invoke(option);
            }
            //初始化一个，避免出错
            this.enumRadioControl_solverType.Init<TwoSiteSolverType>();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <typeparam name="TSolverType"></typeparam>
        public void Init<TSolverType>()
        {
            this.enumRadioControl_solverType.Init<TSolverType>();
        }

        /// <summary>
        /// 输出目录设置事件
        /// </summary>
        public event Func<GnssProcessOption, string> GettingOutputDirectory;
        /// <summary>
        /// 设置已经设定
        /// </summary>
        public event Action<GnssProcessOption> OptionSetted;
        /// <summary>
        /// 准备设置
        /// </summary>
        public event Action<GnssProcessOption> OptionSetting;
        /// <summary>
        /// 设置
        /// </summary>
        public Dictionary<GnssSolverType, GnssProcessOption> Options { get; private set; }
        public GnssSolverType GnssSolverType => this.enumRadioControl_solverType.GetCurrent<GnssSolverType>();
         
        /// <summary>
        /// 设置名称
        /// </summary>
        /// <param name="title"></param>
        public void SetTitle(String title)
        {
            this.enumRadioControl_solverType.Title = title;
        }

        private void button_optSetting_Click(object sender, EventArgs e)
        { 
            GnssProcessOption CurrentOption=  GetCurrentOption();

            OptionSetting?.Invoke(CurrentOption);

            var optionForm = new OptionVizardForm(CurrentOption);
            optionForm.Init();
            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Options[GnssSolverType] = optionForm.Option;
                enumRadioControl_solverType.SetCurrent(optionForm.Option.GnssSolverType);
            }
            OptionSetted ?.Invoke(CurrentOption);
        }

        public GnssProcessOption GetCurrentOption()
        {
            return  GetOption(GnssSolverType);
        }

        /// <summary>
        /// 获取当前，如果没有，则载入默认
        /// </summary>
        /// <param name="solverType"></param>
        /// <returns></returns>
        public GnssProcessOption GetOption(GnssSolverType solverType)
        {
            GnssProcessOption defaultOpt = null;

            if (!this.Options.ContainsKey(solverType))
            {
                if (GnssProcessOptionManager.Instance.Contains(solverType))
                {
                    defaultOpt = GnssProcessOptionManager.Instance[solverType];
                }
                else
                {
                    defaultOpt = GnssProcessOption.GetDefaultIonoFreeDoubleDifferOption();
                }
                Options[solverType] = defaultOpt;
            }

            defaultOpt = Options[solverType];
            return defaultOpt;
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            GnssProcessOption CurrentOption = GetCurrentOption();
            if(Geo.Utils.FormUtil.ShowYesNoMessageBox("确定重置 " + GnssSolverType + "？ ") == DialogResult.Yes){

                Options[GnssSolverType] = GnssProcessOptionManager.Instance.CreateDefault()[GnssSolverType];
            }
           
        }
    }
}
