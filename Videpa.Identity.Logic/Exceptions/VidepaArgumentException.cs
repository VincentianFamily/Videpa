using System;
using System.Security.Authentication;

namespace Videpa.Identity.Logic.Exceptions
{
    public class VidepaArgumentException : Exception
    {
        public VidepaArgumentException() { }
        public VidepaArgumentException(string message) : base(message) { }
    }

    public class VidepaAuthenticationException : AuthenticationException
    {
        public VidepaAuthenticationException() { }
        public VidepaAuthenticationException(string message) : base(message) { }
    }
}
