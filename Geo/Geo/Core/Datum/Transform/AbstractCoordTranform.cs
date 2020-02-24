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
    /// </summary>
    public abstract class AbstractCoordTranform : ICrsTranform
    {
        /// <summary>
        /// 创建一个实例。
        /// </summary>
        /// <param name="sourceCrs">待转换参考系统</param>
        /// <param name="targetCrs">目标参考系统</param>
        public AbstractCoordTranform(ICoordinateReferenceSystem sourceCrs, ICoordinateReferenceSystem targetCrs)
        {
            this.SourceCrs = sourceCrs;
            this.TargetCrs = targetCrs;
            this.CoordinateFactory = new CoordinateFactory(targetCrs);
        }

        /// <summary>
        /// 坐标工厂。用于创建坐标，在构造函数中，以目标参考系初始化。
        /// </summary>
        public ICoordinateFactory CoordinateFactory { get; protected set; }

        /// <summary>
        /// 下一个转换器,与本类是同类型（输入与输出）转换器
        /// </summary>
        public ICrsTranform Successor { protected get; set; }
        /// <summary>
        /// 待转换参考系统
        /// </summary>
        public ICoordinateReferenceSystem SourceCrs { protected set; get; }
        /// <summary>
        /// 目标参考系统
        /// </summary>
        public ICoordinateReferenceSystem TargetCrs { get; set; }
        /// <summary>
        /// 坐标转换，包含责任链。
        /// </summary>
        /// <param name="oldCoord">待转坐标，只取其数字部分，参考系取自属性本对象的TargetCrs属性 </param>
        /// <returns></returns>
        public virtual ICoordinate Trans(ICoordinate oldCoord)
        {
            if (oldCoord == null) throw new ArgumentNullException("输入坐标不能为 null", "oldCoord");

            if (IsMatched)//是否不用判断，以节约时间。
            {
                ICoordinate middle = MatchedTrans(oldCoord);

                if (Successor != null)
                    return Successor.Trans(middle);
                return middle;
            } 
            throw new NotImplementedException("没有实现，或该转换不可实现。");
        }

        /// <summary>
        /// 将坐标转换到目标参考系统.
        /// 如果是本类的责任，则只用实现 Matches 方法和本方法 ，而不用实现 Trans 方法。
        /// 可以直接调用本方法获取本实例的执行结果，这样不用每次做 IsMatched 比较，因而可以获得更快的执行速度。
        /// </summary>
        /// <param name="oldCoord">原坐标</param>
        /// <returns>转换后的坐标</returns>
        public abstract ICoordinate MatchedTrans(ICoordinate oldCoord);

        /// <summary>
        /// 判断是否该我的责任
        /// </summary>
        public abstract bool IsMatched { get; }

        /// <summary>
        /// 将坐标批量转换到目标参考系统
        /// </summary>
        /// <param name="oldCoords">待转换坐标数组</param>
        /// <returns></returns>
        public IEnumerable<ICoordinate> Trans(IEnumerable<ICoordinate> oldCoords)
        {
            List<ICoordinate> list = new List<ICoordinate>();
            foreach (var item in oldCoords)
            {
                list.Add(Trans(item));
            }
            return list;
        }

        /// <summary>
        /// 获取逆向转换
        /// </summary>
        /// <returns></returns>
        public abstract ICrsTranform GetInverse();
    }
}
