using System;
using System.Security.Authentication;
using Videpa.Identity.Logic.Interfaces;
using Videpa.Identity.Logic.Models;
using Videpa.Identity.Logic.Ports;

namespace Videpa.Identity.Logic.Services
{
    public class UserProfileService
    {
        private readonly IPasswordService _passwordService;
        private readonly IJwtIssuer _jwtIssuer;

        public UserProfileService(IPasswordService passwordService, IJwtIssuer jwtIssuer)
        {
            _passwordService = passwordService;
            _jwtIssuer = jwtIssuer;
        }

        /// <summary>
        /// Returns JWT
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Authenticate(string email, string password)
        {
            // Find UserProfile based on e-mail - add 
            var userProfile = new UserProfile();

            // Match passwords
            if(!_passwordService.VerifyPassword(password, userProfile.Salt, userProfile.PasswordHash))
                throw new AuthenticationException();

            // Build and return Jwt

            throw new NotImplementedException();
        }
    }
}
