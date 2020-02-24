using System;
namespace Gnsser.Interoperation.Bernese
{
    /// <summary>
    /// Bernese 文件
    /// </summary>
    public interface IBerFile
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="path"></param>
        void Save(string path);

        /// <summary>
        /// 数量
        /// </summary>
        int Count { get; }
    }
}
