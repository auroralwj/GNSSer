using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Gnsser.Data.Sinex
{ 
    
    /// <summary>
    /// 集合区。
    /// </summary>
    /// <typeparam name="TProduct"></typeparam>
    public class CollectionBlock<T> : ICollectionBlock<T>
        where T : IBlockItem, new()
    {
        public CollectionBlock(){
            this.Items = new List<T>();
            this.Comments = new List<string>();  
        }
        public CollectionBlock(string label):this(){
            this.Label = label;
        }
        public int Count { get { if (Items != null) return Items.Count; return 0; } }

        public bool HasItems
        {
            get
            {
                if (Items != null && Items.Count != 0) return true;
                return false;
            }
        }
        public string Label { get; set; }
        public System.Collections.Generic.List<string> Comments { get; set; }
        public System.Collections.Generic.List<T> Items { get; set; }
  
        public string ToString(string label)
        {
            this.Label = label;
            return this.ToString();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("+" + Label);
            switch (Label)
            {
                case BlockTitle.SOLUTION_ESTIMATE:
                    sb.AppendLine("*INDEX TYPE__ CODE PT SOLN _REF_EPOCH__ UNIT S __ESTIMATED VALUE____ _STD_DEV___"); break;
                case BlockTitle.SOLUTION_APRIORI:
                    sb.AppendLine("*INDEX TYPE__ CODE PT SOLN _REF_EPOCH__ UNIT S __APRIORI VALUE______ _STD_DEV___"); break;
                case BlockTitle.SOLUTION_EPOCHS:
                    sb.AppendLine("*CODE PT SOLN T _DATA_START_ __DATA_END__ _MEAN_EPOCH_"); break;
                case BlockTitle.SITE_ECCENTRICITY:
                    sb.AppendLine("*                                             UP______ NORTH___ EAST____");
                    sb.AppendLine("*SITE PT SOLN T DATA_START__ DATA_END____ AXE ARP->BENCHMARK(M)_________"); break;
                case BlockTitle.SITE_GPS_PHASE_CENTER:
                    sb.AppendLine("*                           UP____ NORTH_ EAST__ UP____ NORTH_ EAST__");
                    sb.AppendLine("*DESCRIPTION_________ S/N__ L1->ARP(M)__________ L2->ARP(M)__________"); break;
                case BlockTitle.SITE_ANTENNA:
                    sb.AppendLine("*SITE PT SOLN T DATA_START__ DATA_END____ DESCRIPTION_________ S/N__"); break;
                case BlockTitle.SITE_RECEIVER:
                    sb.AppendLine("*SITE PT SOLN T DATA_START__ DATA_END____ DESCRIPTION_________ S/N__ FIRMWARE___"); break;
                case BlockTitle.SITE_ID:
                    sb.AppendLine("*CODE PT __DOMES__ T _STATION DESCRIPTION__ APPROX_LON_ APPROX_LAT_ _APP_H_"); break;
                case BlockTitle.SOLUTION_STATISTICS:
                    sb.AppendLine("*_STATISTICAL PARAMETER________ __VALUE(S)____________"); break;
                case BlockTitle.FILE_REFERENCE:
                    sb.AppendLine("*INFO_TYPE_________ INFO________________________________________________________"); break;
                case BlockTitle.INPUT_ACKNOWLEDGEMENTS:
                    sb.AppendLine("*AGY DESCRIPTION________________________________________________________________"); break;
                case BlockTitle.SOLUTION_MATRIX_APRIORI_L_COVA:
                case BlockTitle.SOLUTION_MATRIX_APRIORI_L_CORR:
                case BlockTitle.SOLUTION_MATRIX_ESTIMATE_L_CORR:
                case BlockTitle.SOLUTION_MATRIX_ESTIMATE_L_COVA:
                    sb.AppendLine("*PARA1 PARA2 ____PARA2+0__________ ____PARA2+1__________ ____PARA2+2__________"); break;
                default:
                    break;
            }

            foreach (var item in Comments) sb.AppendLine(item);
            foreach (var item in Items)
            {
                sb.AppendLine(item.ToString());
            }
            sb.Append("-" + Label);
            return sb.ToString();
        }


    }
 }
