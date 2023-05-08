using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.DL.Exceptions
{
    public class TransferRepositoryException : Exception
    {
        public TransferRepositoryException(string? message) : base(message)
        {
        }

        public TransferRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
