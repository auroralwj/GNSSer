using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Utils
{

 
    /// <summary>
    ///  地理区划
    /// </summary>
    public class GeoRegionNode : IntIdTreeNode<GeoRegionNode>
    {
        private GeoRegionNode()
        {

        }
         static GeoRegionNode instance = new GeoRegionNode();
        public static GeoRegionNode Instance
        {
            get { return instance; }
        }
        /// <summary>
        /// 地球区划，分为各州分区和国家、地区
        /// </summary>
        public GeoRegionNode EarthAreaTree { get; private set; }
        /// <summary>
        /// 中国行政区划，到县
        /// </summary>
        public GeoRegionNode ChineseAdminRegionTree { get;  private set; }
        private void Init()
        {
            InitEarthAreaTree();

            InitChineseAdminRegionTree(); 
        }

        private void InitChineseAdminRegionTree()
        {
            ChineseAdminRegionTree = new GeoRegionNode
            {
                Id = 0,
                Name = "中国"
            };

            var data = ChineseAdminRegion.Instance.GetData();
            int i = 1;
            foreach (var item in data)
            {
                var provence = new GeoRegionNode
                {
                    Id = i++,
                    Name = item.Key.ToString()
                };
                provence.SetParent(ChineseAdminRegionTree);

                foreach (var city in item.Value)
                {
                    var cityNode = new GeoRegionNode
                    {
                        Id = i++,
                        Name = city.Key
                    };
                    cityNode.SetParent(provence);


                    foreach (var county in city.Value)
                    {
                        var countyNode = new GeoRegionNode
                        {
                            Id = i++,
                            Name = county
                        };
                        countyNode.SetParent(provence);
                    }
                }
            }
        }

        private void InitEarthAreaTree()
        {
            var data = EarthAdminRegion.Instance.GetData();
            EarthAreaTree = new GeoRegionNode
            {
                Id = 0,
                Name = "地球区划"
            };
            int i = 1;
            foreach (var item in data)
            {
                var earthArea = new GeoRegionNode
                {
                    Id = i++,
                    Name = item.Key.ToString()
                };
                earthArea.SetParent(EarthAreaTree);

                foreach (var contry in item.Value)
                {
                    var contryNode = new GeoRegionNode
                    {
                        Id = i++,
                        Name = contry
                    };
                    contryNode.SetParent(earthArea);
                }
            }
        }

 
    }


    #region 中国各省市地名列表
    public enum China
    {
        北京,
        天津,
        上海,
        重庆,
        黑龙江,
        吉林,
        辽宁,
        内蒙,
        山东,
        山西,
        河北,
        河南,
        湖南,
        湖北,
        江苏,
        浙江,
        安徽,
        江西,
        陕西,
        广西,
        福建,
        四川,
        贵州,
        云南,
        甘肃,
        广东,
        青海,
        宁夏,
        新疆,
        海南,
        西藏,
    }
     
    public enum 石家庄市
    {
        长安区,
        桥东区,
        桥西区,
        新华区,
        郊区,
        井陉矿区,
        井陉县,
        正定县,
        栾城县,
        行唐县,
        灵寿县,
        高邑县,
        深泽县,
        赞皇县,
        无极县,
        平山县,
        元氏县,
        赵县,
        辛集市,
        藁城市,
        晋州市,
        新乐市,
        鹿泉市,

    }


    public enum 北京市
    {
        东城区, 西城区, 海淀区, 朝阳区, 丰台区, 石景山区, 通州区, 顺义区, 房山区, 大兴区, 昌平区, 怀柔区, 平谷区, 门头沟区, 密云县, 延庆县
    }
    public enum 天津市
    {
        和平区, 南开区, 河西区, 河东区, 河北区, 红桥区, 东丽区, 西青区, 津南区, 北辰区, 武清区, 宝坻区, 滨海新区_由天津高新区, 天津港, 天津经济技术开发区, 天津保税区_塘沽_汉沽,_大港三个行政区合并而成, 静海县, 宁河县, 蓟县
    }
    public enum 河北省
    {
        石家庄市, 张家口市, 承德市, 秦皇岛市, 唐山市, 廊坊市, 保定市, 衡水市, 沧州市, 邢台市, 邯郸市,
    }

    public enum 山西省
    {
        太原市, 朔州市, 大同市, 阳泉市, 长治市, 晋城市, 忻州市, 晋中市, 临汾市, 吕梁市, 运城市,
    }
    public enum 内蒙古自治区
    {
        呼和浩特市, 包头市, 乌海市, 赤峰市, 通辽市, 呼伦贝尔市, 鄂尔多斯市, 乌兰察布市, 巴彦淖尔市, 兴安盟, 锡林郭勒盟, 阿拉善盟,
    }
    public enum 辽宁省
    {
        沈阳市, 朝阳市, 阜新市, 铁岭市, 抚顺市, 本溪市, 辽阳市, 鞍山市, 丹东市, 大连市, 营口市, 盘锦市, 锦州市, 葫芦岛市,
    }
    public enum 吉林省
    {
        长春市, 白城市, 松原市, 吉林市, 四平市, 辽源市, 通化市, 白山市, 延边朝鲜族自治州,
    }
    public enum 黑龙江省
    {
        哈尔滨市, 七台河市, 齐齐哈尔市, 黑河市, 大庆市, 鹤岗市, 伊春市, 佳木斯市, 双鸭山市, 鸡西市, 牡丹江市, 绥化市, 大兴安岭地区_加格达奇,
    }
    public enum 上海市
    {
        黄浦区, 卢湾区, 徐汇区, 长宁区, 静安区, 普陀区, 闸北区, 虹口区, 杨浦区, 宝山区, 闵行区_莘庄镇, 定区, 浦东新区, 松江区, 金山区, 青浦区_夏阳街道, 南汇区_惠南镇, 奉贤区_南桥镇, 崇明县_城桥镇
    }
    public enum 江苏省
    {
        南京市, 徐州市, 连云港市, 宿迁市, 淮安市, 盐城市, 扬州市, 泰州市, 南通市, 镇江市, 常州市, 无锡市, 苏州市,
    }
    public enum 浙江省
    {
        杭州市, 湖州市, 嘉兴市, 舟山市, 宁波市, 绍兴市, 衢州市, 金华市, 台州市, 温州市, 丽水市,
    }
    public enum 安徽省
    {
        合肥市, 宿州市, 淮北市, 亳州市, 阜阳市, 蚌埠市, 淮南市, 滁州市, 马鞍山市, 芜湖市, 铜陵市, 安庆市, 黄山市, 六安市, 巢湖市, 池州市, 宣城市,
    }
    public enum 福建省
    {
        福州市, 南平市, 莆田市, 三明市, 泉州市, 厦门市, 漳州市, 龙岩市, 宁德市,
    }
    public enum 江西省
    {
        南昌市, 九江市, 景德镇市, 鹰潭市, 新余市, 萍乡市, 赣州市, 上饶市, 抚州市, 宜春市, 吉安市,
    }
    public enum 山东省
    {
        济南市, 聊城市, 德州市, 东营市, 淄博市, 潍坊市, 烟台市, 威海市, 青岛市, 日照市, 临沂市, 枣庄市, 济宁市, 泰安市, 莱芜市, 滨州市, 菏泽市,
    }
    public enum 河南省
    {
        郑州市, 三门峡市, 洛阳市, 焦作市, 新乡市, 鹤壁市, 安阳市, 濮阳市, 开封市, 商丘市, 许昌市, 漯河市, 平顶山市, 南阳市, 信阳市, 周口市, 驻马店市, 省直辖县级行政单位,
    }
    public enum 湖北省
    {
        武汉市, 十堰市, 襄樊市, 荆门市, 孝感市, 黄冈市, 鄂州市, 黄石市, 咸宁市, 荆州市, 宜昌市, 随州市, 省直辖县级行政单位, 恩施土家族苗族自治州,
    }
    public enum 湖南省
    {
        长沙市, 张家界市, 常德市, 益阳市, 岳阳市, 株洲市, 湘潭市, 衡阳市, 郴州市, 永州市, 邵阳市, 怀化市, 娄底市, 湘西土家族苗族自治州,
    }
    public enum 广东省
    {
        广州市, 清远市, 韶关市, 河源市, 梅州市, 潮州市, 汕头市, 揭阳市, 汕尾市, 惠州市, 东莞市, 深圳市, 珠海市, 中山市, 江门市, 佛山市, 肇庆市, 云浮市, 阳江市, 茂名市, 湛江市,
    }
    public enum 广西壮族自治区
    {
        南宁市, 桂林市, 柳州市, 梧州市, 贵港市, 玉林市, 钦州市, 北海市, 防城港市, 崇左市, 百色市, 河池市, 来宾市, 贺州市,
    }

    public enum 海南省
    {
        海口市, 三亚市, 省直辖行政单位,
    }

    public enum 重庆市
    {
        渝中区, 大渡口区, 江北区, 沙坪坝区, 九龙坡区, 南岸区, 北碚区, 綦江区, 双桥区, 渝北区, 巴南区, 万州区, 涪陵区, 黔江区, 长寿区, 江津区, 合川区, 永川区, 南川区, 綦江县, 潼南县, 铜梁县, 大足县, 荣昌县, 璧山县, 梁平县, 城口县, 丰都县, 垫江县, 武隆县, 忠县, 开县, 云阳县, 奉节县, 巫山县, 巫溪县, 石柱土家族自治县, 秀山土家族苗族自治县, 酉阳土家族苗族自治县, 彭水苗族土家族自治县,
    }

    public enum 四川省
    {
        成都市, 广元市, 绵阳市, 德阳市, 南充市, 广安市, 遂宁市, 内江市, 乐山市, 自贡市, 泸州市, 宜宾市, 攀枝花市, 巴中市, 达州市, 资阳市, 眉山市, 雅安市, 阿坝藏族羌族自治州, 甘孜藏族自治州, 凉山彝族自治州,
    }

    public enum 贵州省
    {
        贵阳市, 六盘水市, 遵义市, 安顺市, 毕节地区, 铜仁地区, 黔东南苗族侗族自治州, 黔南布依族苗族自治州, 黔西南布依族苗族自治州,
    }

    public enum 云南省
    {
        昆明市, 曲靖市, 玉溪市, 保山市, 昭通市, 丽江市, 思茅市, 临沧市, 德宏傣族景颇族自治州, 怒江傈僳族自治州_泸水县六库镇, 迪庆藏族自治州, 大理白族自治州, 楚雄彝族自治州, 红河哈尼族彝族自治州, 文山壮族苗族自治州, 西双版纳傣族自治州,
    }

    public enum 西藏自治区
    {
        拉萨市, 那曲地区, 昌都地区, 林芝地区_林芝县八一镇, 山南地区, 日喀则地区, 阿里地区,
    }

    public enum 陕西省
    {
        西安市, 延安市, 铜川市, 渭南市, 咸阳市, 宝鸡市, 汉中市, 榆林市, 安康市, 商洛市,
    }

    public enum 甘肃省
    {
        兰州市, 嘉峪关市, 金昌市, 白银市, 天水市, 武威市, 酒泉市, 张掖市, 庆阳市, 平凉市, 定西市, 陇南市, 临夏回族自治州, 甘南藏族自治州,
    }

    public enum 青海省
    {
        西宁市, 海东地区, 海北藏族自治州_海晏县西海镇, 海南藏族自治州, 黄南藏族自治州, 果洛藏族自治州, 玉树藏族自治州, 海西蒙古族藏族自治州,
    }

    public enum 宁夏回族自治区
    {
        银川市, 石嘴山市, 吴忠市, 固原市, 中卫市,
    }

    public enum 新疆维吾尔自治区
    {
        乌鲁木齐市, 克拉玛依市, 自治区直辖县级行政单位, 喀什地区, 阿克苏地区, 和田地区, 吐鲁番地区, 哈密地区, 克孜勒苏柯尔克孜自治州, 博尔塔拉蒙古自治州, 昌吉回族自治州, 巴音郭楞蒙古自治州, 伊犁哈萨克自治州, 塔城地区, 阿勒泰地区,
    }

    public enum 香港特别行政区
    {
        香港特别行政区,
    }

    public enum 澳门特别行政区
    {
        澳门特别行政区,
    }

    public enum 台湾省
    {
        台湾省,
    }
    #endregion
}