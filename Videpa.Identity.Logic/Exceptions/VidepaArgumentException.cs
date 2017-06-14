using System;
using System.Security.Authentication;
using Videpa.Core.Exceptions;

namespace Videpa.Identity.Logic.Exceptions
{
    public class VidepaArgumentException : VidepaException
    {
        public VidepaArgumentException() { }
        public VidepaArgumentException(string message) : base(message) { }
    }

    public class VidepaAuthenticationException : VidepaException
    {
        public VidepaAuthenticationException() { }
        public VidepaAuthenticationException(string message) : base(message) { }
    }

    public class VidepaNotFoundException : VidepaException
    {
        public VidepaNotFoundException() { }
        public VidepaNotFoundException(string message) : base(message) { }
    }

    public class VidepaAuthorizationException : VidepaException
    {
        public VidepaAuthorizationException() { }
        public VidepaAuthorizationException(string message) : base(message) { }
    }
}
