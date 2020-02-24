using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gnsser.Interoperation.Bernese;

namespace Gnsser.Winform
{
    public class BerFileFormFactory
    {
        public static BerFileGenForm CreateBerFileGenForm(BerFileType berFileType)
        {
            return new BerFileGenForm(berFileType);
        }

        public static Form CreateBerFileMergeForm(BerFileType berFileType)
        {
            return new BerFileMergeForm(berFileType);
        }
    }
}
