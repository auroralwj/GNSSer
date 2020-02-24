using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Interoperation.Teqc
{
    /// <summary>
    /// TEQC 的参数选项。
    /// 输入指定的功能，按照指定设计生成命令参数。
    /// </summary>
    public class TeqcOption :  IExeOption
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="func"></param>
        /// <param name="files"></param>
        public TeqcOption(TeqcFunction func, params string[] files)
        {
            this.func = func;
            this.inFiles = files;
        }

        /// <summary>
        /// 将功能转换为参数。
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        private static string FunctionToParam(TeqcFunction func)
        {
            string param;
            switch (func)
            {
                case TeqcFunction.ViewMetadata:
                    param = Meta;
                    break;
                case TeqcFunction.QualityChecking:
                    param = QualityChecking;
                    break;
                case TeqcFunction.Translation:
                    param = QualityChecking;
                    break;
                default:
                    param = "";
                    break;
            }
            return param;
        }

        /// <summary>
        /// 转换成 TEQC 命令。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //输入检查
            if (func == TeqcFunction.None) throw new ArgumentNullException("TeqcFunction 不可为空");
            if (inFiles == null) throw new ArgumentNullException("输入文件不可为空"); 

            StringBuilder sb = new StringBuilder();

            string param = FunctionToParam(func);
            sb.Append(param);
            //设置输入文件
            foreach (var item in inFiles)
            {
                sb.Append(" ");
                sb.Append(item);
            }
            //设置运行结果文件
            if (resultFile != null)
            {
                sb.Append(">");
                sb.Append(resultFile);
            }


            return sb.ToString();
        }

        private const string Meta = "+meta";
        private const string QualityChecking = "+qc";
        private const string Translation = "";
        private TeqcFunction func;
        /// <summary>
        /// 输入文件
        /// </summary>
        private string[] inFiles = null;
        /// <summary>
        /// 输出文件，与输入文件相对。
        /// </summary>
        private string[] outFiles = null;
        /// <summary>
        /// 运行输出文件
        /// </summary>
        private string resultFile = null;
    }
}
