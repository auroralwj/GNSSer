//2018.08.22, czs, create in hmx, 对象编辑接口



namespace Geo
{
    /// <summary>
    /// 界面编辑器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityEditor<TEntity>
    {
        /// <summary>
        /// 对象
        /// </summary>
        TEntity Entity { get; set; }
        /// <summary>
        /// 对象到界面的转换
        /// </summary>
        void EntityToUi();
        /// <summary>
        /// 界面到对象
        /// </summary>
        void UiToEntity();
    }
}