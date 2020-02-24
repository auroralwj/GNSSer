using System;
namespace Gnsser.Data.Sinex
{
    public interface IBlock
    {
        System.Collections.Generic.List<string> Comments { get; set; }
        string Label { get; set; }
        
    }
    public abstract class DataBlock : IBlock
    {
        public System.Collections.Generic.List<string> Comments { get; set; }

        public string Label { get; set; }
    }
}
