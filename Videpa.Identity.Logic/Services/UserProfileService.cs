using Videpa.Identity.Logic.Exceptions;
using Videpa.Identity.Logic.Interfaces;
using Videpa.Identity.Logic.Ports;

namespace Videpa.Identity.Logic.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IPasswordService _passwordService;
        private readonly IJwtIssuer _jwtIssuer;
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IPasswordService passwordService, IJwtIssuer jwtIssuer, IUserProfileRepository userProfileRepository)
        {
            _passwordService = passwordService;
            _jwtIssuer = jwtIssuer;
            _userProfileRepository = userProfileRepository;
        }

        /// <summary>
        /// Returns JWT
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Authenticate(string email, string password)
        {
            var authEx = new VidepaAuthenticationException("Invalid email or password");

            // Find UserProfile based on e-mail - add 
            var userProfile = _userProfileRepository.GetUserProfile(email);

            if (userProfile.IsEmpty) 
                throw authEx;

            if (!_passwordService.VerifyPassword(password, userProfile.Value.Salt, userProfile.Value.PasswordHash))
                throw authEx;
            
            return _jwtIssuer.Generate(userProfile.Value);
        }
    }
}
