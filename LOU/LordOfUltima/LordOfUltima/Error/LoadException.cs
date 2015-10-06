using System;

namespace LordOfUltima.Error
{
    [Serializable]
    public class LoadException : Exception
    {
        public LoadException(string message) : base(message) { }
    }
}
