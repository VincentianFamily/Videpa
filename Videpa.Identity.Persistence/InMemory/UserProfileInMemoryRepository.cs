using System;
using System.Collections.Generic;
using System.Linq;
using Videpa.Core;
using Videpa.Identity.Logic.Exceptions;
using Videpa.Identity.Logic.Interfaces;
using Videpa.Identity.Logic.Models;
using Videpa.Identity.Logic.Models.Commands;
using Videpa.Identity.Logic.Ports;

namespace Videpa.Identity.Persistence.InMemory
{
    public class UserProfileInMemoryRepository : IUserProfileRepository
    {
        private readonly IPasswordService _passwordService;
        private readonly Dictionary<string, UserProfile> _userProfiles = new Dictionary<string, UserProfile>();

        public UserProfileInMemoryRepository(IPasswordService passwordService)
        {
            _passwordService = passwordService;

            var salt = _passwordService.GenerateSalt();
            var hashedPassword = _passwordService.HashPassword(salt, "Pouy1581");
            var email = "vince@videpa.com";

            _userProfiles.Add(email, new UserProfile
            {
                Salt = salt,
                PasswordHash = hashedPassword,
                Email = email,
                Name = "Vincent de Paul",
                Cellphone = "42480481",
                Id = Guid.Parse("73860982-022c-442d-b814-57c99a9812bc")
            });
        }

        public Maybe<UserProfile> GetUserProfile(string email)
        {
            return new Maybe<UserProfile>(_userProfiles.GetValueOrDefault(email));
        }

        public Maybe<UserProfile> GetUserProfile(Guid userId)
        {
            return new Maybe<UserProfile>(_userProfiles.Values.SingleOrDefault(p => p.Id == userId));
        }

        public void AddUserProfile(CreateUserProfile createUserProfile)
        {
            if(_userProfiles.ContainsKey(createUserProfile.Email))
                throw new VidepaArgumentException("A user profile with the same email already exists");

            var salt = _passwordService.GenerateSalt();
            var pw = _passwordService.HashPassword(salt, createUserProfile.Password);

            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = createUserProfile.Email,
                Cellphone = createUserProfile.Cellphone,
                Name = createUserProfile.Name,
                Salt = salt,
                PasswordHash = pw
            };

            _userProfiles.Add(userProfile.Email, userProfile);
        }
    }
}
