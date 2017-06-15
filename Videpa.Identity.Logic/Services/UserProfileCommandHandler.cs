using System;
using Videpa.Identity.Logic.Exceptions;
using Videpa.Identity.Logic.Interfaces;
using Videpa.Identity.Logic.Models;
using Videpa.Identity.Logic.Ports;

namespace Videpa.Identity.Logic.Services
{
    public class UserProfileCommandHandler : IUserProfileCommandHandler
    {
        private readonly IPasswordService _passwordService;
        private readonly IJwtIssuer _jwtIssuer;
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileCommandHandler(IPasswordService passwordService, IJwtIssuer jwtIssuer, IUserProfileRepository userProfileRepository)
        {
            _passwordService = passwordService;
            _jwtIssuer = jwtIssuer;
            _userProfileRepository = userProfileRepository;
        }

        /// <summary>
        /// Returns JWT
        /// </summary>
        /// <param name="authenticate"></param>
        /// <returns></returns>
        public AuthenticatedUserProfile Authenticate(AuthenticateUserProfile authenticate)
        {
            var authEx = new VidepaAuthenticationException("Invalid email or password");

            // Find UserProfile based on e-mail - add 
            var userProfile = _userProfileRepository.GetUserProfile(authenticate.Email);

            if (userProfile.IsEmpty) 
                throw authEx;

            if (!_passwordService.VerifyPassword(authenticate.Password, userProfile.Value.Salt, userProfile.Value.PasswordHash))
                throw authEx;

            return _jwtIssuer.Generate(userProfile.Value);
        }

        public AuthenticatedUserProfile Create(CreateUserProfile cmd)
        {
            _userProfileRepository.AddUserProfile(cmd);
            var userProfile = _userProfileRepository.GetUserProfile(cmd.Email);

            if (userProfile.IsEmpty)
                throw new Exception("User profile was not created");

            return _jwtIssuer.Generate(userProfile.Value);
        }

        public AuthenticatedUserProfile Update(Guid userId, UpdateUserProfile cmd)
        {
            throw new NotImplementedException();
        }

        public AuthenticatedUserProfile ChangePassword(Guid userId, ChangeUserProfilePassword cmd)
        {
            throw new NotImplementedException();
        }

        public AuthenticatedUserProfile ChangeEmail(Guid userId, ChangeUserProfileEmail cmd)
        {
            throw new NotImplementedException();
        }
    }
}
