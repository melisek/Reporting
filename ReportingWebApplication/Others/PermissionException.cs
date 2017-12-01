using System;

namespace szakdoga.Others
{
    public class PermissionException : Exception
    {
        public PermissionException(string message) : base(message)
        {
        }
    }
}