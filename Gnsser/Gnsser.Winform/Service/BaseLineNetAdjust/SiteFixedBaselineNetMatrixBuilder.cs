//2018.12.04, czs, create in hmx, 网平差单独移出来

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo;
using Geo.IO;
using System.IO;
using Geo.Coordinates;
using Geo.Times;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;

namespace Gnsser
{

    /// <summary>
    /// 基线网平差矩阵生成器，将条件增加到观测方程上。
    /// </summary>
    public class SiteFixedBaselineNetMatrixBuilder : BaseAdjustMatrixBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="independetnLines">不能用net,net不允许重复基线</param>
        /// <param name="FixedSiteNames"></param>
        public SiteFixedBaselineNetMatrixBuilder(List<EstimatedBaseline> independetnLines, List<string> totalSites, List<string> FixedSiteNames)
        {
            this.IndependentLines = independetnLines;

            this.SiteNames = totalSites;

            if (FixedSiteNames == null) { FixedSiteNames = new List<string>(); }

            if (FixedSiteNames.Count == 0)
            {
                FixedSiteNames.Add(SiteNames.First());
            }
            this.FixedSiteNames = FixedSiteNames;
            this.ParamNames = new List<string>();
            foreach (var item in this.SiteNames)
            {
                ParamNames.Add(item + "_" + Gnsser.ParamNames.Dx);
                ParamNames.Add(item + "_" + Gnsser.ParamNames.Dy);
                ParamNames.Add(item + "_" + Gnsser.ParamNames.Dz);
            }
        }
        /// <summary>
        /// 固定坐标的测站
        /// </summary>
        public List<string> FixedSiteNames { get; set; }
        /// <summary>
        /// 所有测站名称
        /// </summary>
        public List<string> SiteNames { get; set; }

        /// <summary>
        /// 独立基线
        /// </summary>
        public List<EstimatedBaseline> IndependentLines { get; set; }
        /// <summary>
        /// (观测值数量为基线 + 固定测站数量) 乘以 3 , 
        /// </summary>
        public override int ObsCount => (IndependentLines.Count + FixedSiteNames.Count) * 3;
        /// <summary>
        /// 参数数量，为测站数量乘以 3 
        /// </summary>
        public override int ParamCount => base.ParamCount;

        public override WeightedVector AprioriParam => null;

        public override bool IsParamsChanged => false;

        public override void Build()
        {

            //CoeffIncrementOfNormalEquation = new Matrix()

            base.Build();
        }

        public override WeightedVector Observation
        {
            get
            {
                //构建 L 矩阵
                Vector observation = new Vector(ObsCount);
                int i = 0;
                foreach (var line in IndependentLines)
                {
                    var approx = line.ApproxVector;
                    var obs = line.EstimatedVector;
                    var delta = obs - approx;
                    observation[i++] = delta.X;
                    observation[i++] = delta.Y;
                    observation[i++] = delta.Z;
                }
                //固定测站放在最后
                foreach (var name in FixedSiteNames)
                {
                    //var xyz = BaseLineNet.GetSiteCoord(name);
                    observation[i++] = 0;
                    observation[i++] = 0;
                    observation[i++] = 0;
                }

                Matrix cova = new Matrix(new SymmetricMatrix(ObsCount, 0));
                i = 0;
                foreach (var line in IndependentLines)
                {
                    var approxXyz = line.ApproxVector;
                    var startIndex = i * 3;
                    var lineCovaMatrix = line.CovaMatrix;
                    //这里是否该引入基线闭合差


                    cova.SetSub(lineCovaMatrix, startIndex, startIndex);
                    i++;
                }
                //固定站设置小量
                foreach (var name in FixedSiteNames)
                {
                    var startIndex = i * 3;
                    DiagonalMatrix diagonal = new DiagonalMatrix(3, 1e-20);
                    cova.SetSub(diagonal, startIndex, startIndex);
                    i++;
                }

                WeightedVector vs = new WeightedVector(observation, cova);
                vs.ParamNames = BuildObsNames();
                return vs;
            }
        }
        /// <summary>
        /// 构建观测值名称
        /// </summary>
        /// <returns></returns>
        public List<string> BuildObsNames()
        {
            List<string> names = new List<string>();
            int i = 0;
            foreach (var line in IndependentLines)
            {
                names.Add(line.Name + Gnsser.ParamNames.Divider + Gnsser.ParamNames.Dx);
                names.Add(line.Name + Gnsser.ParamNames.Divider + Gnsser.ParamNames.Dy);
                names.Add(line.Name + Gnsser.ParamNames.Divider + Gnsser.ParamNames.Dz);
            }
            //固定测站放在最后
            foreach (var name in FixedSiteNames)
            { 
                names.Add(name + Gnsser.ParamNames.Divider + Gnsser.ParamNames.Dx);
                names.Add(name + Gnsser.ParamNames.Divider + Gnsser.ParamNames.Dy);
                names.Add(name + Gnsser.ParamNames.Divider + Gnsser.ParamNames.Dz);
            }
            return names;
        }


        public override Matrix Coefficient
        {
            get
            {
                Matrix coef = new Matrix(ObsCount, ParamCount);
                int i = 0;
                foreach (var line in IndependentLines)
                {
                    var indexOfRef = this.SiteNames.IndexOf(line.BaseLineName.RefName);
                    var indexOfRov = this.SiteNames.IndexOf(line.BaseLineName.RovName);

                    int startOfRef = indexOfRef * 3;
                    int startOfRov = indexOfRov * 3;

                    coef[i + 0, startOfRef + 0] = -1;
                    coef[i + 1, startOfRef + 1] = -1;
                    coef[i + 2, startOfRef + 2] = -1;

                    coef[i + 0, startOfRov + 0] = 1;
                    coef[i + 1, startOfRov + 1] = 1;
                    coef[i + 2, startOfRov + 2] = 1;

                    i = i + 3;
                }
                foreach (var name in FixedSiteNames)
                {
                    var indexOfFixed = this.SiteNames.IndexOf(name) * 3;

                    coef[i + 0, indexOfFixed + 0] = 1;
                    coef[i + 1, indexOfFixed + 1] = 1;
                    coef[i + 2, indexOfFixed + 2] = 1;

                    i = i + 3;
                }
                return coef;
            }
        }
    }

}
