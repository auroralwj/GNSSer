using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 读取解析一个 WKT文本流。
    /// </summary>
    public class WktStreamTokenizer :Geo.Common.IO.StreamTokenizer
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WktStreamTokenizer class.
        /// </summary>
        /// <remarks>The WktStreamTokenizer class ais in reading WKT streams.</remarks>
        /// <param name="reader">A TextReader that contains </param>
        public WktStreamTokenizer(TextReader reader)
            : base(reader, true)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
        }
        #endregion

        #region Methods

         

        /// <summary>
        /// Reads the authority and authority code.
        /// </summary>
        /// <param name="authority">String to place the authority in.</param>
        /// <param name="authorityCode">String to place the authority code in.</param>
        public void ReadAuthority(ref string authority, ref long authorityCode)
        {
            //AUTHORITY["EPGS","9102"]]
            if (GetStringValue() != "AUTHORITY")
                ReadToken("AUTHORITY");
            ReadToken("[");
            authority = this.ReadDoubleQuotedWord();
            ReadToken(",");
#if(!Silverlight)
            long.TryParse(this.ReadDoubleQuotedWord(),
                NumberStyles.Any,
                CultureInfo.InvariantCulture.NumberFormat,
                out authorityCode);
#else
			try { authorityCode = long.Parse(this.ReadDoubleQuotedWord(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat); }
			catch { }
#endif
            ReadToken("]");
        }
        #endregion

    }
}
