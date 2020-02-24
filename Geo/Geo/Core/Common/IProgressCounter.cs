//2015.10.06.04.01, edit in pengzhou 置信, 完善进度通知接口

using System;
using System.Collections.Generic;
using System.Collections;

namespace Geo
{

    /// <summary>
    /// 进度条计数器。分为分类步骤和步骤的进度。
    /// 一次任务分为多个分类步骤，每个分类步骤又有不同的进度。
    /// </summary>
    public interface IProgressCounter : IProgressViewer
    {
        /// <summary>
        /// 分类进度值改变事件。
        /// </summary>
        event IntValueChangedEventHandler ClassifyValueChanged;
          
        /// <summary>
        /// 具有多次分类进度的初始化，并且直接设置第一个分类的个数。
        /// </summary>
        /// <param name="classfies">进度</param>
        /// <param name="firstProcessCount">第一个分类的个数</param>
        void Init(List<string> classfies, long firstProcessCount);
          
        /// <summary>
        /// 当前分类向前一步,重新设置进度为0开始。
        /// </summary>
        void PerformClassifyStep(int nextProcessCount);

        #region 属性
        /// <summary>
        /// 分类进度列表。
        /// </summary>
        List<string> Classifies { get; set; }

        /// <summary>
        /// 当前分类步骤编号 从 0 开始。
        /// </summary>
        int ClassifyIndex { get; set; }

        /// <summary>
        /// 总共分类步骤数量
        /// </summary>
        int ClassifyCount { get; }


        /// <summary>
        ///当前分类总共进度数量
        /// </summary>
        //int SetProgressCount(int count);
        #endregion
    }

}