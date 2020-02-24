using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Utils
{
    //http://www.cnblogs.com/bicabo/archive/2010/11/23/1885106.html
    //    ---------------------------------------
    //EAN•UCC前缀  EAN•UCC prefix
    //国际物品编码协会分配的产品标识编码。
    //---------------------------------------
    //校验码  check digit
    //中国标准书号的最后一位，由校验码前面的12位数字通过特定的数学算法计算得出，用以检查中国标准书号编号的正确性。
    //---------------------------------------
    //ISBN
    //国际标准书号英文International Standard Book Number的缩写，国际上通用的出版物标识编码的标识符。
    //---------------------------------------
    //出版者  publisher
    //向中国ISBN 管理机构申请并获得出版者号的出版机构或组织。
    //---------------------------------------
    //组区  registration group
    //由国际ISBN管理机构指定的，以国家、地理区域、语言及其他社会集团划分的工作区域。

    //=======================================

    //中国标准书号的结构：

    //---------------------------------------
    //中国标准书号的构成
    //中国标准书号由标识符“ISBN”和13位数字组成。其中13位数字分为以下五部分：
    //1）EAN•UCC前缀  
    //2）组区号
    //3）出版者号  
    //4）出版序号  
    //5）校验码
    //书写或印刷中国标准书号时，标识符“ISBN”使用大写英文字母，其后留半个汉字空，数字的各部分应以半字线隔开。如下所示：
    //ISBN EAN•UCC前缀-组区号-出版者号-出版序号-校验码
    //示例：ISBN 978-7-5064-2595-7
    //---------------------------------------
    //EAN•UCC前缀
    //中国标准书号数字的第一部分。由国际物品编码（EAN•UCC）系统专门提供给国际ISBN管理系统的产品标识编码。
    //--------------------------------------- 
    //组区号 
    //中国标准书号数字的第二部分。它由国际ISBN管理机构分配。中国的组区号为“7”。
    //---------------------------------------
    //出版者号
    //中国标准书号数字的第三部分。标识具体的出版者。其长度为2至7位，由中国ISBN管理机构设置和分配。
    //---------------------------------------
    //出版序号
    //中国标准书号数字的第四部分。由出版者按出版物的出版次序管理和编制。
    //---------------------------------------
    //校验码
    //中国标准书号数字的第五部分，也是其最后一位。采用模数10加权算法计算得出。
    //---------------------------------------
    //出版者号的取值范围和出版量

    //出版者号设置范围
    //00 09
    //100 499
    //5000 7999
    //80000 89999
    //900000 989999
    //9900000 9999999

    //每一出版者号含有的出版量（与上面一一对应，如：00 09对应1000000）
    //1000000
    //100000
    //10000
    //1000
    //100
    //10

    //---------------------------------------
    public class BookCheckDigit
    {

        /// <summary>
        /// 10位数字中国标准书号校验码的计算。
        /// <remarks>     
        /// 10位数字中国标准书号校验码采用模数11的加权算法计算得出。
        /// 
        /// 数学公式为：
        /// 校验码 = mod 11 {11-[mod 11 (加权乘积之和)]}
        ///        = mod 11 {11-[mod 11 (248)]}
        ///        = 5
        /// 
        /// 以ISBN 7-5064-2595-5为例。
        /// </remarks>
        /// </summary>
        /// <param name="sCode"></param>
        /// <returns></returns>
        public static string GetF10ISBN(string sCode)
        {
            string coreCode = sCode.Replace("-", "");
            coreCode = coreCode.Substring(0, 9);

            int sum = 0;
            for (int i = 10; i > 1; i--)
            {
                // 从高位至低位,分别乘以(10-i)
                sum += i * Convert.ToInt32(coreCode.Substring((10 - i), 1));
            }

            string checkCode = "";
            if (sum % 11 == 0)
            {
                checkCode = "0";
            }
            else if (sum % 11 == 1)
            {
                checkCode = "X";
            }
            else
            {
                checkCode = (11 - (sum % 11)).ToString();
            }

            return string.Concat(coreCode, checkCode);
        }

        /// <summary>
        /// 13位数字中国标准书号的校验码的计算。
        /// <remarks>
        /// 13位数字中国标准书号的校验码采用模数10的加权算法计算得出。
        /// 
        /// 数学算式为：
        /// 校验码 = mod 10 {10 – [mod 10 (中国标准书号前12位数字的加权乘积之和)]}
        ///        = mod 10 {10 – [mod 10 (123)]}
        ///        = 7
        /// 
        /// 以ISBN 978-7-5064-2595-7为例。
        /// </remarks>
        /// </summary>
        /// <param name="sCode"></param>
        /// <returns></returns>
        public static string GetF13ISBN(string sCode)
        {
            string coreCode = sCode.Replace("-", "");
            coreCode = coreCode.Substring(0, 12);

            int oddSum = 0; //奇数和
            int evenSum = 0;//偶数和
            for (int i = 0; i < 12; i++)
            {
                if (i % 2 == 0)
                {
                    evenSum += Convert.ToInt32(coreCode.Substring(i, 1));
                }
                else
                {
                    oddSum += Convert.ToInt32(coreCode.Substring(i, 1));
                }
            }

            int sum = oddSum + evenSum * 3;

            string checkCode = null;
            if (sum % 10 == 0)
            {
                checkCode = "0";
            }
            else
            {
                checkCode = (10 - (sum % 10)).ToString();
            }

            return string.Concat(coreCode, checkCode);
        }


        /// <summary>
        /// 10位数字的中国标准书号转换为13位数字的中国标准书号。
        /// <remarks>
        /// 10位数字的中国标准书号转换为13位数字的中国标准书号，在其前9位数字之前加EAN•UCC前缀978，
        /// 以模数10加权算法计算得出的校验码取代10位数字中国标准书号的校验码。
        /// </remarks>
        /// </summary>
        /// <param name="sISBN"></param>
        /// <returns></returns>
        public static string ISBN10T13(string sISBN)
        {
            return GetF13ISBN(string.Concat("978-", sISBN));
        }
    }
}
