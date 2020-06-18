using System;

namespace Simple.Bus.Core
{
    public class RetryException : Exception
    {
        public RetryException(string message) : base(message)
        {

        }
    }
}
