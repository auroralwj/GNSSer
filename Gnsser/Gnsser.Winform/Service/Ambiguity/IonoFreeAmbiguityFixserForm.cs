//2019.03.03, czs, create in hongqing, 无电离层组合模糊度单独固定

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo;
using Geo.IO;
using Geo.Algorithm;
using Gnsser.Service;
using Geo.Algorithm.Adjust;
using Geo.Times;

namespace Gnsser.Winform
{
    public partial class IonoFreeAmbiguityFixserForm : Form
    {
        Log log = new Log(typeof(IonoFreeAmbiguityFixserForm));
        public IonoFreeAmbiguityFixserForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 无电离层模糊度系数计算
        /// </summary>
        public IonoFreeAmbiguitySolverManager IonoFreeAmbiguitySolverManager { get; set; }

        SatelliteNumber CurrentBasePrn;
        double MaxAmbiDifferOfIntAndFloat => namedFloatControl_maxIntFloatDiffer.GetValue();

        private void button_run_Click(object sender, EventArgs e)
        {
            var floatPath = fileOpenControl_ionoFreeFloat.FilePath;
            var mwPath = this.fileOpenControl_mwWide.FilePath;
            var mwTable = ObjectTableReader.Read(mwPath);
            var floatTable = ObjectTableReader.Read(floatPath);

            //Group 文件包括， Index	Site	Name	Group	Value	Rms
            //参数文件包括，ParamName,Value, Rms
            //首先求取宽项模糊度
            GroupedValueService wmValues = new GroupedValueService(mwTable);
            ParamValueService paramValueService = new ParamValueService(floatTable);
            NameRmsedNumeralVector wideLaneVector = GetIntMwDoubleDiffers(paramValueService, wmValues);
            WeightedVector rawFloatAmbiCycles = paramValueService.GetWeightedVector();
            
            WeightedVector fixedParams = DoFixIonoFreeDoubleDifferAmbiguity(rawFloatAmbiCycles, wideLaneVector);
        
            //输出结果
            ObjectTableStorage fixedParam = BuildFixedParamTable(fixedParams);
            objectTableControl1.DataBind(fixedParam);

            var path = Path.Combine(directorySelectionControl1.Path, "FixedParams" + Setting.AmbiguityFileExtension);
            ObjectTableWriter.Write(fixedParam, path);

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(path);
        }

        /// <summary>
        /// 构建表
        /// </summary>
        /// <param name="fixedParams"></param>
        /// <returns></returns>
        private static ObjectTableStorage BuildFixedParamTable(WeightedVector fixedParams)
        {
            ObjectTableStorage result = new ObjectTableStorage("FixedParams");
            for (int i = 0; i < fixedParams.Count; i++)
            {
                result.NewRow();
                result.AddItem("Index", i);
                result.AddItem("Name", fixedParams.ParamNames[i]);
                result.AddItem("TimePeriod", TimePeriod.MaxPeriod);
                result.AddItem("Value", fixedParams[i]);
                result.AddItem("Rms", 1e-10);
            }
            return result;
        }

        private NameRmsedNumeralVector GetIntMwDoubleDiffers(ParamValueService paramValueService, GroupedValueService wmValues)
        {
            NameRmsedNumeralVector wideLaneVector = new NameRmsedNumeralVector();
            // RmsedVector wideLaneVector = new RmsedVector();

            var paramNames = paramValueService.ParamNames;
            foreach (var item in paramNames)
            {
                if (!NetDoubleDifferName.IsDifferParam(item)) { continue; }
                var paramName = NetDoubleDifferName.Parse(item);
                CurrentBasePrn = paramName.RefPrn;

                if (!paramName.IsValid) { continue; }
                var floatWideAmbi = wmValues.GetFirstDoubleDiffer(paramName);
                if (floatWideAmbi == null) { continue; }

                var wideAmbi = new RmsedNumeral(Math.Round(floatWideAmbi.Value), 1e-10);
                var differ = (floatWideAmbi.Value - wideAmbi.Value);
                if (Math.Abs(differ) > 0.3)
                {
                    log.Info(item + " 宽项(MW)整数偏差为 " + differ + "取消固定。");
                    continue;
                }

                wideLaneVector[item] = wideAmbi;
            }

            return wideLaneVector;
        }

        /// <summary>
        /// 执行无电离层双差模糊度固定
        /// </summary>
        /// <param name="rawFloatAmbiCycles"></param>
        /// <param name="isNetSolve">是否网解</param>
        /// <returns></returns>
        protected WeightedVector DoFixIonoFreeDoubleDifferAmbiguity(WeightedVector rawFloatAmbiCycles, NameRmsedNumeralVector intMwDoubleDiffers)
        {

            IonoFreeAmbiguitySolverManager = new IonoFreeAmbiguitySolverManager();
            //指定系统的无电离层组合参数计算器
            var IonoFreeAmbiguitySolver = IonoFreeAmbiguitySolverManager.GetOrCreate(CurrentBasePrn.SatelliteType);
            IonoFreeAmbiguitySolver.CheckOrInit(CurrentBasePrn, new Time(), true);

            //----------------------第一步 MW 宽巷双差 ------------------------  

            //----------------------第二步 MW 宽巷和模糊度浮点解求窄巷模糊度--------
            var ambiFloatVal = rawFloatAmbiCycles.GetNameRmsedNumeralVector();

            //求窄巷模糊度浮点解//单位周
            var narrowFloat = IonoFreeAmbiguitySolver.GetNarrowFloatValue(intMwDoubleDiffers, ambiFloatVal);

            var narrowFloatVect = narrowFloat.GetWeightedVector();
            narrowFloatVect.InverseWeight = rawFloatAmbiCycles.GetWeightedVector(narrowFloatVect.ParamNames).InverseWeight; //追加系数阵，按照顺序------ 

            //方法1：
            var intNarrowVector = DoFixAmbiguity(narrowFloatVect);
            var intNarrow = intNarrowVector.GetNameRmsedNumeralVector();// ParseVector(intNarrowVector); 
            //方法2：直接取整
            //var intNarrow = narrowFloatVect.GetNameRmsedNumeralVector().GetRound();//不推荐使用直接取整 

            //检核窄巷
            var intDiffer = intNarrow - narrowFloat;
            var toRemoves = intDiffer.GetAbsLargerThan(this.MaxAmbiDifferOfIntAndFloat);
            intNarrow.Remove(toRemoves);//移除
            if(toRemoves.Count > 0)
            {
                log.Info("窄巷移除了 " + Geo.Utils.StringUtil.ToString(toRemoves));
            }
            //判断是否超限
            //计算双差载波模糊度固定值
            var fixedVal = IonoFreeAmbiguitySolver.GetIonoFreeAmbiValue(intMwDoubleDiffers, intNarrow);

            var result = fixedVal.GetWeightedVector();

            return result;
        }

        /// <summary>
        /// 默认采用Lambda算法直接固定。
        /// 如果是无电离层组合，则需要分别对待，不能直接固定，需要子类进行实现，//2018.11.06，czs， hmx
        /// </summary>
        /// <param name="rawFloatAmbiCycles"></param>
        /// <returns></returns>
        protected virtual WeightedVector DoFixAmbiguity(WeightedVector rawFloatAmbiCycles)
        {
            //实时固定，采用Lambda算法，按照权逆阵排序，大的在后面，如果失败后，则删除之
            var orderedFloatAmbiCycles = rawFloatAmbiCycles.GetCovaOrdered();
            return Gnsser.LambdaAmbiguitySearcher.GetAmbiguity(orderedFloatAmbiCycles, 3.0, 1e-20);
        }



        private void IonoFreeAmbiguityFixserForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_ionoFreeFloat.Filter = Setting.ParamFileFilter;

            fileOpenControl_mwWide.Filter = Setting.TextGroupFileFilter;

        }

        private void fileOpenControl_ionoFreeFloat_FilePathSetted(object sender, EventArgs e)
        {
            directorySelectionControl1.Path = Path.GetDirectoryName(fileOpenControl_ionoFreeFloat.FilePath);
        }
    }

    /// <summary>
    /// 分组数据服务器
    /// </summary>
    public class GroupedValueService : ObjectTableBasedService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table"></param>
        public GroupedValueService(ObjectTableStorage table) : base(table)
        {
        }

        /// <summary>
        /// 双差
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public RmsedNumeral GetFirstDoubleDiffer(NetDoubleDifferName paramName)
        {
            return GetFirstDoubleDiffer(paramName.RovName, paramName.RefName, paramName.RovPrn.ToString(), paramName.RefPrn.ToString());
        }

        /// <summary>
        /// 获取双差
        /// </summary>
        /// <param name="rovSite"></param>
        /// <param name="rovParam"></param>
        /// <param name="baseSite"></param>
        /// <param name="baseParam"></param>
        /// <returns></returns>
        public RmsedNumeral GetFirstDoubleDiffer(string rovSite, string baseSite, string rovParam, string baseParam)
        {
            var rovRov = GetFirst(rovSite, rovParam);
            var rovRef = GetFirst(rovSite, baseParam);
            var refRov = GetFirst(baseSite, rovParam);
            var refRef = GetFirst(baseSite, baseParam);
            if (rovRov == null || rovRef == null || refRov == null || refRef == null)
            {
                return null;
            }
            var rovDiffer = rovRov - rovRef;
            var refDiffer = refRov - refRef;

            var result = rovDiffer - refDiffer;
            return result;
        }


        /// <summary>
        /// 获取第一个
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public RmsedNumeral GetFirst(string siteName, string name)
        {
            if (Storage == null) { return null; }

            var rowOK = Storage.GetRow(row => (String.Equals(row["Site"].ToString(), siteName, StringComparison.InvariantCultureIgnoreCase)
                 && String.Equals(row["Name"].ToString(), name, StringComparison.InvariantCultureIgnoreCase)));
            if (rowOK == null) { return null; }

            var val = (double)rowOK["Value"];
            var rms = (double)rowOK["Rms"];
            return new RmsedNumeral(val, rms);
        }
    }
}
