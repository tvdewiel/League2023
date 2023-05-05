using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.DL.Exceptions
{
    public class SpelerRepositoryException : Exception
    {
        public SpelerRepositoryException(string? message) : base(message)
        {
        }

        public SpelerRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
