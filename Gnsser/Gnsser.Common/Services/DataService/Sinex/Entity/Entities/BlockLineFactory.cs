using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Gnsser.Data.Sinex
{  
    public class BlockLineFactory
    {
        public static ICollectionBlock<T> Read<M, T>(StreamReader reader, string label)
            where T : IBlockItem, new()
            where M : ICollectionBlock<T>, new()
        {
            ICollectionBlock<T> b = new M();
            b.Label = label;
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Trim().StartsWith(CharDefinition.TITLE_END))
                {
                    b.Label = line.Substring(1).Trim();
                    if (b.Label != label) throw new ArgumentException("传入的参数与读取的不一致。");
                    break;
                }
                if (line.StartsWith(CharDefinition.COMMENT))
                {
                    b.Comments.Add(line.Trim());
                    continue;//注释
                }
                b.Items.Add( (T)(new T().Init(line)));
            }
            return b;
        }
        public static ICollectionBlock<T> Read<T>(StreamReader reader, string label) where T : IBlockItem, new()
        {
            CollectionBlock<T> b = new CollectionBlock<T>(label);
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(CharDefinition.TITLE_END))
                {
                    b.Label = line.Substring(1).Trim();
                    if (b.Label != label) throw new ArgumentException("传入的参数与读取的不一致。");
                    break;
                }
                if (line.StartsWith(CharDefinition.COMMENT))
                {
                    b.Comments.Add(line.Trim());
                    continue;//注释
                }
                b.Items.Add( (T)(new T().Init(line)));
            }
            return b;
        }
    }

}
