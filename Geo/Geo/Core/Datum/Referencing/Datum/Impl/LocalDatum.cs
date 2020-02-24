//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{
    /// <summary>
    /// Local datum. If two local datum objects have the same datum type and name, 
    /// then they can be considered equal. This means that coordinates can be
    /// transformed between two different local coordinate systems, as long as
    /// they are based on the same local datum.
    /// </summary>
    public class LocalDatum : Datum
    {

    }
}
