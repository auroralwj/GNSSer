using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Utils;

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// SOLUTION/MATRIX_ESTIMATE Block (Mandatory)
    /// Description:
    /// The Estimate Matrix can be stored in an Upper or Lower triangular form. Only
    /// the Upper or Lower portion needs to be stored because the matrix is always
    /// symmetrical.
    /// The matrix contents can be:
    /// CORR - Correlation Matrix 相互关系, 关连, 相关
    /// COVA - Covariance Matrix 协方差
    /// INFO - Information Matrix (of Normals 正常, 标准), time.east. COVA^(-1)
    /// The distinction between the form and its contents is given by the title block
    /// which must take one of the following forms:
    /// SOLUTION/MATRIX_ESTIMATE L CORR
    /// SOLUTION/MATRIX_ESTIMATE U CORR
    /// SOLUTION/MATRIX_ESTIMATE L COVA
    /// SOLUTION/MATRIX_ESTIMATE U COVA
    /// SOLUTION/MATRIX_ESTIMATE L INFO
    /// SOLUTION/MATRIX_ESTIMATE U INFO
    /// Comment:
    ///	The Matrix Estimate Row/Column Number correspond to the Estimated Parameters
    ///	Index in the SOLUTION/ESTIMATE block.
    ///	If the CORR matrix is used, standard deviations must be stored in the diagonal
    ///	elements instead of 1.000.
    ///	Missing elements in the matrix are assumed to be zero (0); consequently, zero
    ///	elements may be omitted to reduce the aboutSize of this block.
    ///	NOTE: The same scale (variance) factor MUST be used for both MATRIX_ESTIMATE
    ///	and MATRIX_APRIORI, as well as for the standard deviations in the ESTIMATE
    ///	and APRIORI Blocks. This scale factor should be stored as 'VarianceOfUnitWeight Factor' in the
    ///	SOLUTION/STATISTICS block.
    ///	If you use the INFO type this block should contain the constrained normal equation
    ///	matrix of your least square adjustment.
    ///	
    ///	SOLUTION/NORMAL_EQUATION_MATRIX Block (Mandatory for normal equations)
    ///	Description:
    ///	This block is mandatory if the normal equation is to be provided directly in the
    ///	SINEX fileB.
    ///	The block should contain the original (reduced) normal equation matrix (time.east.,
    ///	without constraints).
    ///	The normal equation matrix can be stored in an Upper or Lower triangular form. Only
    ///	the Upper or Lower portion needs to be stored because the matrix is always
    ///	symmetrical. The distinction between the forms is given by the title block
    ///	which must take one of the following forms:
    ///	SOLUTION/NORMAL_EQUATION_MATRIX L
    ///	SOLUTION/NORMAL_EQUATION_MATRIX U
    ///	Comment:
    ///	The NEQ-Matrix Row/Column Number correspond to the Estimated Parameters
    ///	Index in the SOLUTION/ESTIMATE block.
    ///	Missing elements in the matrix are assumed to be zero (0); consequently, zero
    ///	elements may be omitted to reduce the aboutSize of this block.
    /// 
    /// </summary>
    public class SolutionMatrixBlock : CollectionBlock<MatrixLine>
    {
        

    }
}
