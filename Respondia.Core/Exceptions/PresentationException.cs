using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Exceptions
{
    public class PresentationException : Exception
    {
        public PresentationException() { }

        public PresentationException(string message) : base(message) { }

        public PresentationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
