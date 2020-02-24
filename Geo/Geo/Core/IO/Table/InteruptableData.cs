//2018.08.10, czs, create in hmx,  可以打断的数据，断断续续的数据。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo 
{
    /// <summary>
    /// 可以打断的数据，断断续续的数据。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class InteruptableData<TKey> : InteruptableData<TKey, double>
        where TKey : IComparable<TKey>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="KeyToDouble"></param>
        /// <param name="InterruptSpan"></param>
        public InteruptableData(Func<TKey, double> KeyToDouble, double InterruptSpan = 5) : base(KeyToDouble, InterruptSpan)
        {
            this.Cashes = new BaseDictionary<Segment<TKey>, NumeralWindowData<TKey>>();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="KeyToDouble"></param>
        /// <param name="InterruptSpan"></param>
        public InteruptableData(BaseDictionary<TKey, double> dic, Func<TKey, double> KeyToDouble, double InterruptSpan = 5) : base(dic, KeyToDouble, InterruptSpan)
        {
            this.Cashes = new BaseDictionary<Segment<TKey>, NumeralWindowData<TKey>>();
        }
        /// <summary>
        /// 缓存
        /// </summary>
        BaseDictionary<Segment<TKey>, NumeralWindowData<TKey>> Cashes { get; set; }

        /// <summary>
        /// 获取数窗口
        /// </summary>
        /// <param name="refKey"></param>
        /// <returns></returns>
        public NumeralWindowData<TKey> GetNumeralWindowData(TKey refKey)
        {
            foreach (var item in Cashes.Data)
            {
                if (item.Key.Contains(refKey))
                {
                    return Cashes[item.Key];
                }
            }

            //提取数据
            NumeralWindowData<TKey> window = new NumeralWindowData<TKey>(int.MaxValue);
            var seg = this.GetSegmentKeyValue(refKey);
            if(seg.Value == null)
            {
                return null;
            }

            foreach (var item in seg.Value.Data)
            {
                window.Add(item.Key, item.Value);
            }

            //缓存
            Cashes[seg.Key]= window;


            return window;
        }

    }

    /// <summary>
    /// 可以打断的数据，断断续续的数据。
    /// 初始化时，必须连续输入。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class InteruptableData<TKey, TValue> : BaseDictionary<Segment<TKey>, BaseDictionary<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="KeyToDouble"></param>
        /// <param name="InterruptSpan"></param>
        public InteruptableData(BaseDictionary<TKey, TValue> dic, Func<TKey, double> KeyToDouble, double InterruptSpan = 5)
            : this(KeyToDouble, InterruptSpan)
        {
            foreach (var item in dic.Data)
            {
                this.Add(item.Key, item.Value);
            }
            Flash();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="KeyToDouble"></param>
        /// <param name="InterruptSpan"></param>
        public InteruptableData(Func<TKey, double> KeyToDouble, double InterruptSpan = 5)
        {
            this.InterruptSpan = InterruptSpan;

            Geo.Utils.DoubleUtil.AutoSetKeyToDouble<TKey>(out KeyToDouble);
            this.KeyToDouble = KeyToDouble;
            LastInputedKeyValue = double.MinValue;
        }
        Func<TKey, double> KeyToDouble;

        /// <summary>
        /// 超过此，认为断裂
        /// </summary>
        public double InterruptSpan { get; set; }
        /// <summary>
        /// 最后一个输入的键
        /// </summary>
        public TKey LastInputedKey { get; set; }
        /// <summary>
        /// 最后一个输入的键值
        /// </summary>
        public double LastInputedKeyValue { get; set; }
        /// <summary>
        /// 当前数据
        /// </summary>
        BaseDictionary<TKey, TValue> CurrentData { get; set; }
        /// <summary>
        /// 当前时段
        /// </summary>
        Segment<TKey> CurrentSegment { get; set; }
        #region 增加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public InteruptableData<TKey, TValue> Add(TKey key, TValue val)
        {
            var curentVal = KeyToDouble(key);
            if (LastInputedKey == null || LastInputedKeyValue == double.MinValue)//第一次
            {
                CreateNewSegment(key);
            }
            else
            {
                var differ = Math.Abs(curentVal - LastInputedKeyValue);
                if (differ > InterruptSpan)
                {
                    CreateNewSegment(key);
                }
            }
            //添加
            this.CurrentSegment.End = key;
            this.CurrentData.Add(key, val);
            //this[this.CurrentSegment].Add(key, val);

            //记录当前，作为下一个的比较
            this.UpdateLastKey(key, curentVal);
            return this;
        }

        /// <summary>
        /// 记录，作为下一个的比较
        /// </summary>
        /// <param name="key"></param>
        /// <param name="curentVal"></param>
        private void UpdateLastKey(TKey key, double curentVal)
        {
            this.LastInputedKey = key;
            this.LastInputedKeyValue = curentVal;
        }

        private void CreateNewSegment(TKey key)
        {
            Flash();

            this.CurrentSegment = new Segment<TKey>(key, key);
            this.CurrentData = new BaseDictionary<TKey, TValue>();
            //this.GetOrCreate(CurrentSegment);
        }
        /// <summary>
        /// 保存当前,写完后调用。
        /// </summary>
        public void Flash()
        {
            if (CurrentData != null)
            {
                this[CurrentSegment] = CurrentData;
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override BaseDictionary<TKey, TValue> Create(Segment<TKey> key)
        {
            return new BaseDictionary<TKey, TValue>();
        }
        #endregion

        /// <summary>
        /// 获取包含key的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BaseDictionary<TKey, TValue> GetSegmentValue(TKey key)
        {
            foreach (var item in this.Keys)
            {
                if (item.Contains(key)) { return this[item]; }
            }
            return null;
        }
        /// <summary>
        /// 获取包含key的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KeyValuePair<Segment<TKey>, BaseDictionary<TKey, TValue>> GetSegmentKeyValue(TKey key)
        {
            foreach (var item in this.Keys)
            {
                if (item.Contains(key)) { return new KeyValuePair<Segment<TKey>, BaseDictionary<TKey, TValue>>(item, this[item]); }
            }
            return default(KeyValuePair<Segment<TKey>, BaseDictionary<TKey, TValue>>);
        }
    }
}
