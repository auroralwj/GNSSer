//2015.06.08, czs, create in namu, 树形节点缓存池
//2016.02.18, czs, edit in hongqing, 缓存

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Data.Entity; 
using Geo; 

namespace Geo
{

    #region 缓存池
    /// <summary>
    /// 类型缓存池。可以自动管理多个不同类型的高速缓存。
    /// 不负责读取数据，只负责标记和存储数据，且为单例模式。
    /// </summary>
    public class ClassCashe<TEntity> : BaseDictionary<string, TEntity>
    {
        #region 单例模式
        static private ClassCashe<TEntity> instance = new ClassCashe<TEntity>();

        protected ClassCashe() { AvailableDic = new Dictionary<string, bool>(); }

        static public ClassCashe<TEntity> Instance { get { return instance; } }
        #endregion
        /// <summary>
        /// 缓存是否可用，标记数据是否改变。指示是否需要从数据库获取数据。
        /// </summary>
        private Dictionary<string, bool> AvailableDic { get; set; }

        public string key { get { return typeof(TEntity).FullName; } }
        /// <summary>
        /// 缓存的数据
        /// </summary>
        public TEntity CasheData { get { return Get(key); } set { Set(value); } }
        /// <summary>
        /// 缓存是否可用。查看是否需要直接从数据库读取数据。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsAvailable
        {
            get
            {
                //初始化检核
                if (!this.AvailableDic.ContainsKey(key))
                {
                    AvailableDic[key] = false;
                }

                return AvailableDic[key];
            }
            set
            {
                AvailableDic[key] = value; 
            }
        }
        /// <summary>
        /// 设置缓存可用性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="trueOrFalse"></param>
        public void SetAvailable(bool trueOrFalse)
        {
            AvailableDic[key] = trueOrFalse;
        }

        /// <summary>
        /// 设置缓存数据，同时将该数据关键字标记为可用。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Set(TEntity val)
        {
            base.Set(key, val);
            SetAvailable(true);
        }
    }
    #endregion

    #region 缓存池
    /// <summary>
    /// 对象缓存池。
    /// 不负责读取数据，只负责标记和存储数据，且为单例模式。
    /// </summary>
    public class EntityCashe<TEntity> : BaseDictionary<string, TEntity>
    {
        #region 单例模式
        static private EntityCashe<TEntity> instance = new EntityCashe<TEntity>();

        protected EntityCashe() { AvailableDic = new Dictionary<string, bool>(); }

        static public EntityCashe<TEntity> Instance { get { return instance; } }
        #endregion
        /// <summary>
        /// 缓存是否可用，标记数据是否改变。指示是否需要从数据库获取数据。
        /// </summary>
        private Dictionary<string, bool> AvailableDic { get; set; }
        /// <summary>
        /// 指示数据源是否改变，缓存失效否
        /// </summary>
        public bool IsDataSourceChanged { get; set; }
        /// <summary>
        /// 缓存是否可用。查看是否需要直接从数据库读取数据。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsAvailable(string key)
        {
            if (IsDataSourceChanged) return false;

            //初始化检核
            if (!this.AvailableDic.ContainsKey(key))
            {
                AvailableDic[key] = false;
            }

            return AvailableDic[key];
        }
        /// <summary>
        /// 设置缓存可用性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="trueOrFalse"></param>
        public void SetAvailable(string key, bool trueOrFalse)
        {
            AvailableDic[key] = trueOrFalse;
        }

        /// <summary>
        /// 设置缓存数据，同时将该数据关键字标记为可用。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public override void Set(string key, TEntity val)
        {
            base.Set(key, val);
            SetAvailable(key, true);
            IsDataSourceChanged = false;
        }
    }
    #endregion

}
