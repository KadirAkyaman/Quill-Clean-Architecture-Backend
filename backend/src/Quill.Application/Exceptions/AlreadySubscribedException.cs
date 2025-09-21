using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Application.Exceptions
{
    public class AlreadySubscribedException : Exception
    {
        public AlreadySubscribedException(string message) : base(message)
        {
            
        }
    }
}