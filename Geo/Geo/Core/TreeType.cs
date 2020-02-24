//2015.06.11, czs, create in namu, 修改  Temp21-测站文件， Temp23->工程文件， Temp22->测量网文件

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 顶层树的类型。
    /// </summary>
    public enum TreeType
    {
        根节点 = 0,
        文档节点 = 1,
        用户单位 = 2,
        工程节点 = 3,
        阵地节点 = 4,
        财务节点 = 5,
        地理区划 = 6,
        Temp10 = 7,
        Temp11 = 8,
        Temp12 = 9,
        Temp13 = 10,
        材料节点 = 11,
        供应商节点 = 12,
        门店节点 = 13,
        Temp14 = 14,
        Temp15 = 15,
        Temp16 = 16,
        Temp17 = 17,
        Temp18 = 18,
        Temp19 = 19,
        Temp20 = 20,
        测站文件 = 21,
        工程文件 = 22,
        测量网文件 = 23,
        Temp24 = 24,
        Temp25 = 25,
        Temp26 = 26,
        Temp27 = 27,
        Temp28 = 28,
        Temp29 = 29,
        Temp30 = 30,
        Temp31 = 31,
        Temp32 = 32,
        Temp33 = 33,
        Temp34 = 34,
        Temp35 = 35,
        Temp36 = 36,
        Temp37 = 37,
        Temp38 = 38,
        Temp39 = 39,
        Temp40 = 40,
        Temp41 = 41,
        Temp42 = 42,
        Temp43 = 43,
        Temp44 = 44,
        Temp45 = 45,
        Temp46 = 46,
        Temp47 = 47,
        Temp48 = 48,
        Temp49 = 49,
        Temp50 = 50,
        Temp51 = 51,
        Temp52 = 52,
        Temp53 = 53,
        Temp54 = 54,
        Temp55 = 55,
        Temp56 = 56,
        Temp57 = 57,
        Temp58 = 58,
        Temp59 = 59,
        Temp60 = 60,
        Temp61 = 61,
        Temp62 = 62,
        Temp63 = 63,
        Temp64 = 64,
        Temp65 = 65,
        Temp66 = 66,
        Temp67 = 67,
        Temp68 = 68,
        Temp69 = 69,
        Temp70 = 70,
        Temp71 = 71,
        Temp72 = 72,
        Temp73 = 73,
        Temp74 = 74,
        Temp75 = 75,
        Temp76 = 76,
        Temp77 = 77,
        Temp78 = 78,
        Temp79 = 79,
        Temp80 = 80,
        Temp81 = 81,
        Temp82 = 82,
        Temp83 = 83,
        Temp84 = 84,
        Temp85 = 85,
        Temp86 = 86,
        Temp87 = 87,
        Temp88 = 88,
        Temp89 = 89,
        Temp90 = 90,
        Temp91 = 91,
        Temp92 = 92,
        Temp93 = 93,
        Temp94 = 94,
        Temp95 = 95,
        Temp96 = 96,
        Temp97 = 97,
        Temp98 = 98,
        Temp99 = 99,
        Temp100 = 100,
        Unknown=-1
    }


    public class TreeTypeHelper
    {



        #region 工具

        /// <summary>
        /// 负责存储顶层树类型对应的对象ID标识。与Type相对对应。
        /// </summary>
        public static Dictionary<TreeType, int> TopTreeTypes = new Dictionary<TreeType, int>();

        /// <summary>
        /// 获取类型的根 ID。
        /// </summary>
        /// <param name="TreeType"></param>
        /// <returns></returns>
        public static int GetRootNodeId(TreeType TreeType)
        {
            return TopTreeTypes[TreeType];
        }
        #endregion
    }



    /// <summary>
    /// 一个字典，用于维护类型和图像关键字
    /// </summary>
    public class TreeNodeImageKeyManager : Geo.BaseDictionary<TreeType, string>

    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TreeNodeImageKeyManager()
        { 
        }

        /// <summary>
        /// 获取指定的图像关键字。如果没有预设，则返回 “folder”。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override string Get(TreeType key)
        {
            if ( this.Contains(key)) return Data[key];
            return "folder";
        }         

        /// <summary>
        /// 默认的字典。
        /// </summary>
        public static TreeNodeImageKeyManager Default
        {
            get
            {
                TreeNodeImageKeyManager ImageKeyManager = new TreeNodeImageKeyManager();
                ImageKeyManager[TreeType.材料节点] = "file";
                ImageKeyManager[TreeType.文档节点] = "file";
                ImageKeyManager[TreeType.用户单位] = "group";
                ImageKeyManager[TreeType.阵地节点] = "position";
                ImageKeyManager[TreeType.工程节点] = "folder";
                ImageKeyManager[TreeType.根节点] = "folder";

                return ImageKeyManager;
            }
        }
    }



}
