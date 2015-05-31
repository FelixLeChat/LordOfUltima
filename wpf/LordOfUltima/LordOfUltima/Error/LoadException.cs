using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordOfUltima.Error
{
    public class LoadException : Exception
    {
        public LoadException(string message) : base(message) { }
    }
}
