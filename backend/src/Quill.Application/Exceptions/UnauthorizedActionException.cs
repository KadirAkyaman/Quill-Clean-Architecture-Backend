using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Application.Exceptions
{
    public class UnauthorizedActionException : Exception
    {
        public UnauthorizedActionException(string message) : base(message)
        {
            
        }
    }
}