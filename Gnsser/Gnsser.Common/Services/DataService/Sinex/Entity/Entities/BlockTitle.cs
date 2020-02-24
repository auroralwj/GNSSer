using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// Each SINEX line has at most 80 ASCII characters.
    /// The SINEX fileB is subdivided in groups of satData called blocks. Each fileRefferblock is
    /// enclosed by coeffOfParams header and trailer line. Each fileRefferblock has coeffOfParams fixed format. The
    /// blocks contain information on the fileB, its input, the sites and the solution.  
    /// </summary>
    public class BlockTitle
    {
        //The following blocks are defined:
        public const string FILE_REFERENCE = "FILE/REFERENCE";
        public const string FILE_COMMENT = "FILE/COMMENT";
        public const string INPUT_HISTORY = "INPUT/HISTORY";
        public const string INPUT_FILES = "INPUT/FILES";
        public const string INPUT_ACKNOWLEDGEMENTS = "INPUT/ACKNOWLEDGMENTS";//INPUT/ACKNOWLEDGMENTS
        public const string NUTATION_DATA = "NUTATION/DATA";
        public const string PRECESSION_DATA = "PRECESSION/DATA";
        public const string SOURCE_ID = "SOURCE/ID";
        public const string SITE_ID = "SITE/ID";
        public const string SITE_DATA = "SITE/DATA";
        public const string SITE_RECEIVER = "SITE/RECEIVER";
        public const string SITE_ANTENNA = "SITE/ANTENNA";
        public const string SITE_GPS_PHASE_CENTER = "SITE/GPS_PHASE_CENTER";
        public const string SITE_GAL_PHASE_CENTER = "SITE/GAL_PHASE_CENTER";
        public const string SITE_ECCENTRICITY = "SITE/ECCENTRICITY";
        public const string SATELLITE_ID = "SATELLITE/ID";
        public const string SATELLITE_PHASE_CENTER = "SATELLITE/PHASE_CENTER";
        public const string BIAS_EPOCHS = "BIAS/EPOCHS";
        public const string SOLUTION_EPOCHS = "SOLUTION/EPOCHS";
        public const string SOLUTION_STATISTICS = "SOLUTION/STATISTICS";
        public const string SOLUTION_ESTIMATE = "SOLUTION/ESTIMATE";
        public const string SOLUTION_APRIORI = "SOLUTION/APRIORI";
        public const string SOLUTION_MATRIX_ESTIMATE_L_CORR = "SOLUTION/MATRIX_ESTIMATE L CORR";
        public const string SOLUTION_MATRIX_ESTIMATE_L_COVA = "SOLUTION/MATRIX_ESTIMATE L COVA";
        public const string SOLUTION_MATRIX_ESTIMATE_L_INFO = "SOLUTION/MATRIX_ESTIMATE L INFO";
        public const string SOLUTION_MATRIX_ESTIMATE_U_CORR = "SOLUTION/MATRIX_ESTIMATE U CORR";
        public const string SOLUTION_MATRIX_ESTIMATE_U_COVA = "SOLUTION/MATRIX_ESTIMATE U COVA";
        public const string SOLUTION_MATRIX_ESTIMATE_U_INFO = "SOLUTION/MATRIX_ESTIMATE U INFO";

        public const string SOLUTION_MATRIX_APRIORI_L_CORR = "SOLUTION/MATRIX_APRIORI L CORR";
        public const string SOLUTION_MATRIX_APRIORI_L_COVA = "SOLUTION/MATRIX_APRIORI L COVA";
        public const string SOLUTION_MATRIX_APRIORI_L_INFO = "SOLUTION/MATRIX_APRIORI L INFO";
        public const string SOLUTION_MATRIX_APRIORI_U_CORR = "SOLUTION/MATRIX_APRIORI U CORR";
        public const string SOLUTION_MATRIX_APRIORI_U_COVA = "SOLUTION/MATRIX_APRIORI U COVA";
        public const string SOLUTION_MATRIX_APRIORI_U_INFO = "SOLUTION/MATRIX_APRIORI U INFO";

        public const string SOLUTION_NORMAL_EQUATION_VECTOR = "SOLUTION/NORMAL_EQUATION_VECTOR";
        public const string SOLUTION_NORMAL_EQUATION_MATRIX_L = "SOLUTION/NORMAL_EQUATION_MATRIX L";
        public const string SOLUTION_NORMAL_EQUATION_MATRIX_U = "SOLUTION/NORMAL_EQUATION_MATRIX U";

    }
}
