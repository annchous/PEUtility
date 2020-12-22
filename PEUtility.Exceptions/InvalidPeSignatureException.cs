using System;

namespace PEUtility.Exceptions
{
    public class InvalidPeSignatureException : Exception
    {
        public InvalidPeSignatureException() : base("Invalid PE header signature! Cannot read that file.")
        {
        }
    }
}
