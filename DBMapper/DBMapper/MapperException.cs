using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBMapper
{
    public class MapperException : Exception
    {
        public MapperException(string message)
            : base(message)
        {
            
        }
        public MapperException(string message, Exception innerException)
            : base(message, innerException)
        {


        }
    }
}
