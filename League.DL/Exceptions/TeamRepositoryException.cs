using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.DL.Exceptions
{
    public class TeamRepositoryException : Exception
    {
        public TeamRepositoryException(string? message) : base(message)
        {
        }

        public TeamRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
