//2017.08.30, czs, create, 手动矩阵生成器

using System;
using System.Collections.Generic; 
using System.Text;  
using Geo.Algorithm.Adjust; 
using Geo.Utils;
using Geo;
using Gnsser;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 手动矩阵生成器
    /// </summary>
    public class ManualAdjustMatrixBuilder : BaseAdjustMatrixBuilder
    {
        /// <summary>
        /// 手动矩阵生成器
        /// </summary>
        public ManualAdjustMatrixBuilder()
        {

        }
        #region  内部参数
        WeightedVector _AprioriParam;
        bool _IsParamsChanged;
        Matrix _CoeffOfDesign;
        WeightedVector _ObsMinusApprox;
        #endregion

        #region 重写属性
        /// <summary>
        /// 参数近似值
        /// </summary>
        public override WeightedVector AprioriParam { get { return _AprioriParam; } }
        /// <summary>
        /// 参数是否改变
        /// </summary>
        public override bool IsParamsChanged { get { return _IsParamsChanged; } }
        /// <summary>
        /// 系数阵
        /// </summary>
        public override Matrix Coefficient { get { return _CoeffOfDesign; } }
        /// <summary>
        /// 观测值减去近似值
        /// </summary>
        public override WeightedVector Observation { get { return _ObsMinusApprox; } }

 
         
        #endregion

        #region 方法
        /// <summary>
        /// 参数先验值
        /// </summary>
        /// <param name="AprioriParam"></param>
        /// <returns></returns>
        public ManualAdjustMatrixBuilder SetAprioriParam(WeightedVector AprioriParam)
        {
            this._AprioriParam = AprioriParam; return this;
        }
        /// <summary>
        /// 设置参数是否改变
        /// </summary>
        /// <param name="IsParamsChanged"></param>
        /// <returns></returns>
        public ManualAdjustMatrixBuilder SetIsParamsChanged(bool IsParamsChanged)
        {
            this._IsParamsChanged = IsParamsChanged; return this;
        }
        /// <summary>
        /// 设置设计阵
        /// </summary>
        /// <param name="CoeffOfDesign"></param>
        /// <returns></returns>
        public ManualAdjustMatrixBuilder SetCoeffOfDesign(Matrix CoeffOfDesign)
        {
            this._CoeffOfDesign = CoeffOfDesign; return this;
        }
        /// <summary>
        /// 设置观测值减去近似值
        /// </summary>
        /// <param name="ObsMinusApprox"></param>
        /// <returns></returns>
        public ManualAdjustMatrixBuilder SetObsMinusApprox(WeightedVector ObsMinusApprox)
        {
            this._ObsMinusApprox = ObsMinusApprox; return this;
        } 
        /// <summary>
        /// 设置转移矩阵
        /// </summary>
        /// <param name="Transfer"></param>
        /// <returns></returns>
        public ManualAdjustMatrixBuilder SetTransfer(WeightedMatrix Transfer)
        {
            this.Transfer = Transfer; return this;
        }
        /// <summary>
        /// 设置参数名称
        /// </summary>
        /// <param name="ParamNames"></param>
        /// <returns></returns>
        public ManualAdjustMatrixBuilder SetParamNames(List<string> ParamNames)
        {
            this.ParamNames = ParamNames; return this;
        }
        #endregion
    }
}