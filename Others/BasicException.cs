using System;

namespace szakdoga
{
    public class BasicException : Exception
    {
        public BasicException(string message) : base(message)
        { }
    }
}
