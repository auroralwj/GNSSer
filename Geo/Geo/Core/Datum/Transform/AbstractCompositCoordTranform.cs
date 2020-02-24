//2014.06.11,czs,creat

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 抽象坐标转换实现类,调用前需要指定坐标参考系。
    /// 组合模式的父类。
    /// </summary>
    public abstract class AbstractCompositCoordTranform: AbstractCoordTranform
    {
        /// <summary>
        /// 创建一个实例。
        /// </summary>
        /// <param name="sourceCrs">待转换参考系统</param>
        /// <param name="targetCrs">目标参考系统</param>
        public AbstractCompositCoordTranform(ICoordinateReferenceSystem sourceCrs, ICoordinateReferenceSystem targetCrs)
            : base(sourceCrs, targetCrs)
        { 
        }
        /// <summary>
        /// 责任链的首节点。在组合模式中，需要设置（在构造函数中设置）。
        /// </summary>
        public AbstractCoordTranform TransformChain { get; protected set; } 

        /// <summary>
        /// 将坐标转换到目标参考系统.
        /// 如果是本类的责任，则只用实现 Matches 方法和本方法 ，而不用实现 Trans 方法。
        /// 可以直接调用本方法获取本实例的执行结果，这样不用每次做 IsMatched 比较，因而可以获得更快的执行速度。
        /// </summary>
        /// <param name="oldCoord">原坐标</param>
        /// <returns>转换后的坐标</returns>
        public override ICoordinate MatchedTrans(ICoordinate oldCoord)
        {
            return TransformChain.Trans(oldCoord);
        }
        /// <summary>
        /// 子模式中都有判断，在这里可以默认为真，或添加更简便的实现，以节约时间。
        /// </summary>
        public override bool IsMatched        {            get { return true; }        }


        /// <summary>
        /// 获取逆向转换
        /// </summary>
        /// <returns></returns>
      //  public abstract ICrsTranform GetInverse();
    }
}
