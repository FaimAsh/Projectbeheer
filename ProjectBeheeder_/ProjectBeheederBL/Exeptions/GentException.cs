using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.Exeptions
{
    public class GentException : Exception
    {
        public GentException(string message) : base(message) { }
        public GentException(string message, Exception innerException) : base(message, innerException) { }
    }
}
