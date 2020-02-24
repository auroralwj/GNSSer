using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{
    class CoordinateConversionException : ApplicationException
    {
        public CoordinateConversionException() { }
        public CoordinateConversionException(string message) : base(message) { }
        public CoordinateConversionException(string message, Exception inner) : base(message, inner) { }
        protected CoordinateConversionException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
