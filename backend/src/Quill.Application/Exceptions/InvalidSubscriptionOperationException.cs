using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Application.Exceptions
{
    public class InvalidSubscriptionOperationException : Exception
    {
        public InvalidSubscriptionOperationException(string message) : base(message)
        {
            
        }
    }
}