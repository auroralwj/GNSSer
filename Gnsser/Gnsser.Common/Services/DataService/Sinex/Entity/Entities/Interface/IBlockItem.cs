using System;
namespace Gnsser.Data.Sinex
{
    public interface IBlockItem
    {
        IBlockItem Init(string line);
    } 

    //public class IBlockItem : IBlockItem
    //{
    //    public IBlockItem() { }
    //    public virtual IBlockItem InitDetect(string line) { throw new Exception("没有实现"); }
    //}

}
