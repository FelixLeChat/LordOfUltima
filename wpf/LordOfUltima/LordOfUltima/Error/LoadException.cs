using System;

namespace LordOfUltima.Error
{
    public class LoadException : Exception
    {
        public LoadException(string message) : base(message) { }
    }
}
