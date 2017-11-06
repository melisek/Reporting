using System;

namespace szakdoga
{
    public static class CreateGUID
    {
        public static string GetGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
