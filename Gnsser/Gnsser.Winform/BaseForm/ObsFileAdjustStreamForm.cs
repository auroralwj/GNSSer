//2016.09.04, czs, edit in hongqing, 观测文件平差计算数据流

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.IO;
using Geo.Referencing;
using Geo.Times;
using Geo.Utils;
using Gnsser;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;

namespace Gnsser.Winform
{
    public delegate void OptionChangedEventHandler(GnssProcessOption Option);

    /// <summary>
    /// 观测文件平差计算数据流。
    /// </summary>
    public partial class ObsFileAdjustStreamForm : MultiFileStreamerForm, Gnsser.Winform.IShowLayer
    {
        public ObsFileAdjustStreamForm()
        {
            InitializeComponent();
           

            //SetIsMultiObsFile(false);
            this.Coords = new List<NamedRmsXyz>();
        }

        #region 事件 属性 
        public event ShowLayerHandler ShowLayer;

        protected virtual IFileGnssSolver GnssSolver { get; set; }
        /// <summary>
        /// 用于显示在地图上的坐标。
        /// </summary>
        protected List<NamedRmsXyz> Coords { get; set; }
        #endregion

        #region 方法 


        protected void InitSolverType<TSolverType>()
        {
             enumRadioControl1_GnssSolverType.Init<TSolverType>();        
        }

        public TSolverType GetSolverType<TSolverType>()
        {
            return enumRadioControl1_GnssSolverType.GetCurrent<TSolverType>();
        }

        protected override void UiToOption()
        {
            base.UiToOption();
            Option.IsOpenReportWhenCompleted = checkBox_IsOpenReportWhenCompleted.Checked;
            Option.GnssSolverType = this.enumRadioControl1_GnssSolverType.GetCurrent<GnssSolverType>();
            Option.PositionType = this.enumRadioControl_positionType.GetCurrent<PositionType>();
        }

        protected override void OptionToUi(GnssProcessOption Option)
        {
            base.OptionToUi(Option);
            checkBox_IsOpenReportWhenCompleted.Checked = Option.IsOpenReportWhenCompleted ;
            this.enumRadioControl1_GnssSolverType.SetCurrent<GnssSolverType>(Option.GnssSolverType);
            this.enumRadioControl_positionType.SetCurrent<PositionType>(Option.PositionType);
        }
        #region 显示
        /// <summary>
        /// 最后的计算结果显示计算结果在界面上
        /// </summary>
        /// <param name="last"></param>
        protected void AppendFinalResultOnUI(SimpleGnssResult last)
        {
            if (last == null) { return; }
            ShowResultOnSummary(last);//这个必须显示，不然全白算了

            //写入汇总文件，必须写，不然无法找到计算结果 
            if (Setting.IsShowInfo)
            {
                ShowGeneralResultInfo(last);
            }

            if (IsShowData)
            { 
                ShowAdjustMatrix(last);
            }
            
            if (last != null)
            {
                log.Fatal(last.Name + "\t" + last.ReceiverTime + "\t" + "RmsOfEstimated\t" + FormatVector(last.ResultMatrix.Estimated.GetRmsVector()));
                log.Fatal(last.Name + "\t" + last.ReceiverTime + "\t" + "Estimated\t" + FormatVector(last.ResultMatrix.Estimated));
                log.Fatal(last.Name + "\t" + last.ReceiverTime + "\t" + "ParamNames\t" + String.Format(new EnumerableFormatProvider(), "{0:\t}", last.ParamNames));
            }
            //写最后的值
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Est and Rms:"); 
            sb.AppendLine( String.Format(new EnumerableFormatProvider(), "{0:,6.4 }", last.ResultMatrix.Estimated.ParamNames)); 
            var vector = last.ResultMatrix.Estimated.GetRmsedVector(); 
            int length = vector.Count;

            for (int i = 0; i < length; i++)
            {
                var rmsVal = vector.GetItem(i);
                if (i != 0) { sb.Append(", "); }
                sb.Append(rmsVal.Value.ToString("G5"));
            }
            sb.AppendLine();
            for (int i = 0; i < length; i++)
            {
                var rmsVal = vector.GetItem(i);
                if (i != 0) { sb.Append(", "); }
                sb.Append(rmsVal.Rms.ToString("G5"));
            }
            sb.AppendLine();

            log.Fatal(sb.ToString());
        }
        /// <summary>
        ///填充界面数据表，讲第一个和第二个表显示出。
        /// </summary>
        /// <param name="TableTextManager"></param>
        public void SetResultsToUITable(ObjectTableManager TableTextManager)
        {
            if (TableTextManager.Count > 0) { BindTableA(TableTextManager.First); }
            if (TableTextManager.Keys.Count > 1) { BindTableB(TableTextManager.Second); }
            if (TableTextManager.Keys.Count > 2) { BindTableC(TableTextManager.Values[2]); }
            if (TableTextManager.Keys.Count > 3) { BindTableD(TableTextManager.Last); }
        }

        public string FormatVector(Geo.Algorithm.IVector vector)
        {
            return String.Format(new EnumerableFormatProvider(), "{0:\t}", vector);
        }

        /// <summary>
        /// 显示一个
        /// </summary>
        /// <param name="last"></param>
        protected void ShowResultOnSummary(SimpleGnssResult last)
        {
            ShowResultTitleOnSummaryTextbox(last);
            ShowResultOnSummaryTextbox(last);
        }
        /// <summary>
        /// 在首个文本界面显示计算概略信息。
        /// </summary>
        protected void ShowGeneralResultInfo(SimpleGnssResult lastResult)
        { 
            TimeSpan span = DateTime.Now - StartTime;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("耗时：\t" + DateTimeUtil.GetFloatString(span)); 

            ReadableGnssResultBuilder builder = new ReadableGnssResultBuilder();
            var info = builder.Build(lastResult); 
             
            ShowInfo(sb.ToString() + info);
        }

        /// <summary>
        /// 输出结果。根据设置。
        /// </summary>
        /// <param name="GnssResults"></param>
        protected void OutputFinalResult(SimpleGnssResult GnssResult)
        {
            if (GnssResult == null) { return; }

            if (Option.IsOutputResult  &&  GnssResult is BaseGnssResult)
            {
                log.Info(GnssResult.Name + ", " + GnssResult .ReceiverTime + "， 即将输出结果文件...");
                var writer = new GnssResultWriter(Option,  this.Option.IsOutputEpochResult,
                    Option.IsOutputEpochSatInfo);

                writer.WriteFinal( (BaseGnssResult) GnssResult);
            }
        }

        /// <summary>
        /// 显示平差矩阵
        /// </summary>
        /// <param name="adjustResult"></param>
        protected void ShowAdjustMatrix(SimpleGnssResult adjustResult)
        {
            if (adjustResult == null) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(adjustResult.ResultMatrix.ToReadableText());
            var str = sb.ToString();
            ShowAdjustString(str);
        } 
        #endregion

        #region 显示
        #region 显示一个汇总结果
        /// <summary>
        /// 在结果上面显示参数名称行
        /// </summary>
        /// <param name="last"></param>
        protected void ShowResultTitleOnSummaryTextbox(SimpleGnssResult last)
        {
            if (last == null) { return; }

            this.Invoke(new Action(delegate()
            {
                if (this.OutputTextBoxes[1].Text.Length < 5)
                {
                    string msg = last.ToShortTabTitles();

                    if (this.Disposing || this.IsDisposed) { return; }
                    //   var info = DateTimeUtil.GetFormatedTimeNow(true) + ":\t" + msg;
                    this.AppdendLineToSummaryString(msg);
                }
            }));
        }

        /// <summary>
        /// 在界面上打印出执行结果,同时保存到日志中。
        /// 非差重要，必须输出，不然白算了。！！
        /// </summary>
        /// <param name="last"></param>
        protected void ShowResultOnSummaryTextbox(SimpleGnssResult last) { if (last == null) { return; } var val = last.ToShortTabValue(); log.Fatal(val); AppdendLineToSummaryString(val); }

        #endregion
        /// <summary>
        /// 绘图显示按钮
        /// </summary>
        /// <param name="truOrFalse"></param>
        protected void ShowDrawButtons(bool truOrFalse) { this.button_drawDifferLine.Visible = truOrFalse; this.button_drawRmslines.Visible = truOrFalse; }
        
       /// 平差偏差
        /// </summary>
        /// <param name="str"></param>
        protected void ShowAdjustString(string str) { this.Invoke(new Action(delegate() { this.OutputTextBoxes[0].Text = str; })); }
        /// <summary>
        /// 追加一行到摘要页面
        /// </summary>
        /// <param name="str"></param>
        protected void AppdendLineToSummaryString(string str) { this.Invoke(new Action(delegate() { this.OutputTextBoxes[1].Text += str + "\r\n"; })); }
        #endregion

        #region 设置     
       
        private void checkBox_autoMatchingFile_CheckedChanged(object sender, EventArgs e){Setting.GnsserConfig.EnableAutoFindingFile = this.checkBox_autoMatchingFile.Checked;  }

        private void checkBox_enableNet_CheckedChanged(object sender, EventArgs e) { Setting.EnableNet = checkBox_enableNet.Checked;      }

        #endregion

        #region 事件响应
        /// <summary>
        /// 构建获取当前
        /// </summary>
        /// <returns></returns>
        protected override GnssProcessOption CheckOrBuildGnssOption()
        {
            var type = this.enumRadioControl1_GnssSolverType.GetCurrent<GnssSolverType>();
            return CheckOrBuildGnssOption(type);
        }
        protected override void DetailSetting()
        { 
            var SolverType = this.enumRadioControl1_GnssSolverType.GetCurrent<GnssSolverType>();
            if (Option == null)
            {
                Option =   GnssProcessOptionManager.Instance[SolverType];
            }

            Option.GnssSolverType = this.enumRadioControl1_GnssSolverType.GetCurrent<GnssSolverType>();
            base.DetailSetting(); 
        }
        private void ObsFileAdjustStreamForm_Load(object sender, EventArgs e)
        {
            Setting.IsShowDebug = false;
            checkBox_enableNet.Checked = Setting.EnableNet;
            enumRadioControl_positionType.Init<PositionType>();

            panel_opt.Visible = (Setting.VersionType == VersionType.Development);
        }
        #endregion

        #region 绘图
        protected virtual void button_drawDifferLine_Click(object sender, EventArgs e)
        {
            if (this.GnssSolver != null && this.GnssSolver.HasTableData)
            {
             //   this.paramVectorRenderControl1.SetTableTextStorage();// (names);
                this.paramVectorRenderControl1.DrawTable(this.GnssSolver.TableTextManager.First);   
            }
        }
        protected virtual void button_drawRmslines_Click(object sender, EventArgs e)
        {
            if (this.GnssSolver != null && this.GnssSolver.HasTableData)
            {
                var rmsKey = this.GnssSolver.TableTextManager.Keys.Find(m => m.ToLower().Contains("rms"));
                if (String.IsNullOrWhiteSpace(rmsKey)) { return; }

                this.paramVectorRenderControl1.SetTableTextStorage(this.GnssSolver.TableTextManager[rmsKey]);// (names);
                this.paramVectorRenderControl1.DrawParamLines();
            }
        }
        protected virtual void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null)
            {
                if (checkBox_clearCoords.Checked) { this.Coords.Clear(); }

                if (Coords.Count == 0)
                {
                    foreach (var path in ParseInputPathes())
                    {
                        var header = RinexObsFileReader.ReadHeader(path);
                        this.Coords.Add(new  NamedRmsXyz(System.IO.Path.GetFileName(path), new RmsedXYZ( header.ApproxXyz)));
                    }
                }
                if (Coords == null || Coords.Count == 0) { log.Warn("输入结果为空。");return; }
                int start = this.paramVectorRenderControl1.StartIndex;

                if (start >= Coords.Count) { log.Warn("起始历元编号过大！总结果数 " + Coords.Count + "，起始历元编号：" + start + ", 为了可显示，已经将其设为 0"); start = 0; }

                PostionResultLayerBuilder builder = new PostionResultLayerBuilder(Coords, start);
                //builder.AddPt(GnssResults[0].SiteInfo.ApproxXyz, GnssResults[0].SiteInfo.MarkerName + "ApproxPoint");
                ShowLayer(builder.Build());
            }
        }
        #endregion

        private void checkBox_IsOpenReportWhenCompleted_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Option != null)
            {
                this.Option.IsOpenReportWhenCompleted = this.checkBox_IsOpenReportWhenCompleted.Checked;
            }
        }
        #endregion

        private void button_applyOpt_Click(object sender, EventArgs e)
        {
             var optPath = fileOpenControl_opt.FilePath;
            if (String.IsNullOrWhiteSpace(optPath))
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("请先设置路径。");
                return;
            }
            OptionManager optionManager = new OptionManager();
            this.Option = optionManager.Read(optPath);
            this.OptionToUi(Option);
        }

        private void button_saveCurrent_Click(object sender, EventArgs e)
        {
            if(Option == null)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("请先设置或初始化设置。");
                return;
            }
            this.UiToOption();
            var optPath = fileOpenControl_opt.FilePath;
            if (String.IsNullOrWhiteSpace(optPath))
            {
                 Geo.Utils.FormUtil.ShowErrorMessageBox("请先设置路径。");
                return;
            }
            OptionManager optionManager = new OptionManager();
            optionManager.Write(this.Option, optPath);
            Geo.Utils.FormUtil.ShowOkAndOpenFile(optPath, "已成功保存到 " + optPath + ", 是否打开？");
        }

        private void button_optSaveAs_Click(object sender, EventArgs e)
        {
            if (Option == null)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("请先设置或初始化设置。");
                return;
            }
            this.UiToOption();
            OptionConfigWriter objectConfigWriter = new OptionConfigWriter();
            var text = objectConfigWriter.BuildConfigText(Option.Data);

            Geo.Utils.FormUtil.ShowFormSaveTextFileAndIfOpenFolder(text, Option.Name, "opt文件|*.opt");

        }

        private void enumRadioControl1_EnumItemSelected(string arg1, bool arg2)
        {
            if (!arg2) { return; }
            var SolverType = (GnssSolverType)Enum.Parse( typeof(GnssSolverType), arg1);
           // var SolverType = GnssSolverTypeHelper.GetGnssSolverType(enumRadioControl1_GnssSolverType.CurrentdType);
            if (Option == null ||
                (Option.GnssSolverType != SolverType
                && Geo.Utils.FormUtil.ShowYesNoMessageBox(" 已选择 “" + arg1 + "”，是否加载默认其设置？") == DialogResult.Yes))
            {
                this.Option = GnssProcessOptionManager.Instance[SolverType];
                this.OptionToUi(this.Option);
            }
        }

        private void enumRadioControl_positionType_EnumItemSelected(string arg1, bool arg2)
        {
            var opt = this.CheckOrBuildGnssOption();
            if (arg2)
            {
                if (arg1.Contains("动态") && arg2)
                {
                    opt.IsUpdateEstimatePostition = true;
                    log.Warn("您已选择 " + enumRadioControl1_GnssSolverType.CurrentText + " " + arg1 + "， 将更新坐标");
                }
                else
                {
                    opt.IsUpdateEstimatePostition = false;
                    log.Warn("您已选择 " + enumRadioControl1_GnssSolverType.CurrentText + " " + arg1 + "， 将不更新坐标");
                }
            }
        }

        private void enumRadioControl1_GnssSolverType_Load(object sender, EventArgs e)
        {

        }
    }
}
