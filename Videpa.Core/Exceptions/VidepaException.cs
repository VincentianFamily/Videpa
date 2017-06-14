using System;

namespace Videpa.Core.Exceptions
{
    public class VidepaException : Exception
    {
        public VidepaException() { }
        public VidepaException(string message) : base(message) { }
    }
}
