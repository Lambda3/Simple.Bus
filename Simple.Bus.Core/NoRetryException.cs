using System;

namespace Simple.Bus.Core
{
    public class NoRetryException : Exception
    {
        public NoRetryException(string message) : base(message)
        {

        }
    }
}
