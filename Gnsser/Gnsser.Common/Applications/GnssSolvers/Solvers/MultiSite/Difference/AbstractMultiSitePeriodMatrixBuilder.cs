//2014.09.04, czs, edit, 基本理清思路
//2014.12.11， czs, edit in jinxinliangmao shuangliao, 差分定位矩阵生成器
//2014.12.14， czs, edit in namu shuangliao, 抽象载波相位差分定位矩阵生成器

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Geo.Utils;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Models;

namespace Gnsser.Service
{ 
    /// <summary>
    /// 差分定位的矩阵生成器。适用于参数平差、卡尔曼滤波等。
    /// </summary>
    public abstract class AbstractMultiSitePeriodMatrixBuilder : MultiSitePeroidMatrixBuilder
    {
        #region 构造函数
        /// <summary>
        /// 差分定位的矩阵生成器，构造函数。
        /// </summary>  
        /// <param name="model">解算选项</param> 
        public AbstractMultiSitePeriodMatrixBuilder( 
            GnssProcessOption model):base(model)
        { 
            foreach (var item in Gnsser.ParamNames.Dxyz)
            {
                ParamStateTransferModelManager.Add(item, new StaticTransferModel(Option.StdDevOfStaticTransferModel));
            }
        }

        #endregion

      
        /// <summary>
        /// 构建名称
        /// </summary>
        public override List<string> BuildParamNames()
        {
            var nameBuilder = GnssParamNameBuilder;
            nameBuilder.Epoches = this.CurrentMaterial.Epoches;
            nameBuilder.BasePrn = CurrentBasePrn;
            nameBuilder.EnabledPrns = this.EnabledPrns;
            this.ParamNames = nameBuilder.Build();
            return ParamNames;
        }
         





        /// <summary>
        /// 先验信息。滤波需要的
        /// </summary>
        public override WeightedVector AprioriParam
        {
            get
            {

                return  base.AprioriParam;

                WeightedVector invWeight = null;

                if (PreviousProduct == null)//第一次初始参数向量
                {
                    invWeight = WeightedVector.GetZeroVector(ParamCount, 1);
                    //初始模糊度和钟差等为 10
                    for (int i = 0; i < ParamCount; i++)
                    {
                        if (i < 3) { invWeight.InverseWeight[i, i] = 25; }
                        else { invWeight.InverseWeight[i, i] = 100; }
                    }
                    invWeight.ParamNames = ParamNames;
                }
                else if (IsParamsChanged)//参数已经改变，则重新查找
                {
                    invWeight = GetNewWeighedVectorInOrder(this.ParamNames, PreviousProduct.ResultMatrix.Estimated, 0, 1e4);
                }
                else//参数没有改变，直接将上一平差值作为下一先验值。
                {
                    invWeight = PreviousProduct.ResultMatrix.Estimated;
                }
                 
                return invWeight;
            }
        }

    }
}
