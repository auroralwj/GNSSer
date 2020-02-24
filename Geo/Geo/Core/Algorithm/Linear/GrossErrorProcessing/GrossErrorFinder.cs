//2014.09.18, czs, create, in hailutu, 线性粗差探测器 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Algorithm.Adjust
{
    //使用场景描述：流式和块式
    //预先指定中误差和计算中误差
    //分为两部分，计算中误差（赋值）， 查找（剔除）

    /// <summary>
    /// 线性粗差探测器。
    /// 从粗差列表、数组中，找出粗差。
    /// 在相同的测量条件下的测量值序列中，超过n（3、4或2）倍中误差的测量误差
    /// </summary>
    public class GrossErrorFinder
    {
        /// <summary>
        /// 线性粗差探测器。构造函数。
        /// </summary>
        /// <param name="ThresholdTimes"></param>
        public GrossErrorFinder(double ThresholdTimes = 3, int StDev = 0)
        {
            this.ThresholdTimes = ThresholdTimes;
            this.Rms = StDev;
        }

        #region 内部变量
        private double _thresholdTimes;
        private double _rms;
        //1.limit error
        //2.在一定观测条件下偶然误差的绝对值不应超过的限值
        private double _limitError;//极限误差
        #endregion

        #region 属性
        /// <summary>
        /// 限差。一般为 3 倍中误差。
        /// </summary>
        public double LimitError { get { return _limitError; } }

        /// <summary>
        /// 中误差。
        /// </summary>
        public double Rms
        {
            get { return _rms; }
            set
            {
                _rms = value;
                UpdateThreshold();
            }
        }
        /// <summary>
        /// 中误差倍数
        /// </summary>
        public double ThresholdTimes
        {
            get { return _thresholdTimes; }
            set
            {
                _thresholdTimes = value;
                UpdateThreshold();
            }
        }
        #endregion 

        /// <summary>
        /// 计算中误差并赋值。
        /// </summary>
        /// <param name="errors">误差序列</param>
        public void SetRms(double[] errors)
        {
            RmsCalculator rmsCalculator = new RmsCalculator(errors);
            rmsCalculator.Calculate(); 
            this.Rms = rmsCalculator.Rms;
        }
        /// <summary>
        /// 是否是粗差。与 N 倍中误差比较，是否大于阈值。
        /// </summary>
        public bool IsGrossError(double number)
        {
            double abs = Math.Abs(number);
            // double differ = Math.Abs(number - _stDev);
            if (_limitError <= abs)
                return true;

            return false;
        }
        /// <summary>
        /// 查找出粗差
        /// </summary>
        /// <param name="errors">误数组</param>
        /// <returns></returns>
        public Dictionary<int, Double> Find(double[] errors)
        {
            SetRms(errors);

            Dictionary<int, Double>  grossDic = new Dictionary<int,double>();
            int i = 0;
            foreach (var item in errors)
            {
                if (IsGrossError(item)) grossDic.Add(i, item);
                i++;
            }
            return grossDic;

        }

        /// <summary>
        /// 更新粗差阈值
        /// </summary>
        private void UpdateThreshold()
        {
            this._limitError = _rms * _thresholdTimes;
        }

        /// <summary>
        /// 得到过滤后的数据
        /// </summary>
        /// <param name="data">待处理数据</param>
        /// <param name="grosses">粗差字典</param>
        /// <returns></returns>
        public static List<double> GetSmartData(double[] data, Dictionary<int, double> grosses)
        {
            List<double> list = new List<double>();
            foreach (var item in data)
            {
                if (grosses.ContainsValue(item)) continue;
                list.Add(item);
            }
            return list;
        }
    }
}
