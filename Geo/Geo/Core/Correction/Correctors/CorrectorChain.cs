//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Correction
{ 

    /// <summary>
    /// 改正器顶层接口.责任链,是一组改正对象的组合。一般采用此类将各种改正进行组合。
    /// </summary>
    /// <typeparam name="TCorrection">改正数类型</typeparam>
    /// <typeparam name="TInput">改正时需要输入的对象类型</typeparam>
    public abstract class CorrectorChain<TCorrection, TInput> : AbstractCorrector<TCorrection, TInput>,ICorrectorChian<TCorrection, TInput> , ICorrector  
    {  /// <summary>
        /// 构造函数
        /// </summary>
        public CorrectorChain()
        { 
            this.Name = "改正组合";
            this.Correctors = new List<ICorrector<TCorrection, TInput>>();
        }
        /// <summary>
        /// 改正器。
        /// </summary>
        public List<ICorrector<TCorrection, TInput>> Correctors { get; set; }
        /// <summary>
        /// 改正数字典，记录改正数细节，便于调试。
        /// </summary>
        public Dictionary<string, TCorrection> Corrections { get; protected set; }
      
        /// <summary>
        /// 执行改正
        /// </summary>
        /// <param name="input">输入对象</param>
        public override void Correct(TInput input)
        {
            this.Corrections = new Dictionary<string, TCorrection>();
             
            foreach (var item in Correctors)
            {
                item.Correct(input);                

                Corrections.Add(item.Name, item.Correction);
            }
        }
      
        /// <summary>
        /// 通过这种方法添加责任链的后继者。
        /// </summary>
        /// <param name="node">改正器</param>
        /// <returns></returns>
        public CorrectorChain<TCorrection, TInput> Add(ICorrector<TCorrection, TInput> node)
        {
            this.Correctors.Add(node);
            return this;
        }
         
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        { 
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + ", Count： " + Correctors.Count);
            if (this.Correctors.Count > 0)
            {
                sb.AppendLine(", { ");
                int i = 1;
                foreach (var p in Correctors)
                {
                    if (i != 1) { sb.AppendLine(","); }
                    sb.Append(p.ToString());

                    i++;
                }
                sb.Append("}");
            }
            return sb.ToString();
        }
        public IEnumerator<ICorrector<TCorrection, TInput>> GetEnumerator()
        {
            return Correctors.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        { 
            return Correctors.GetEnumerator();
        }
    } 
}
