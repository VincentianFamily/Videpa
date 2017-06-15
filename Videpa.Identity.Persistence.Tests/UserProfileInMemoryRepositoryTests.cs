using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Videpa.Identity.Logic.Exceptions;
using Videpa.Identity.Logic.Models;
using Videpa.Identity.Logic.Services;
using Videpa.Identity.Persistence.InMemory;

namespace Videpa.Identity.Persistence.Tests
{
    [TestClass]
    public class UserProfileInMemoryRepositoryTests
    {
        private const string VinceEmail = "vince@videpa.com";

        [TestMethod]
        public void GetUserProfile_ProfileExists_ReturnsUserProfile()
        {
            var service = new UserProfileInMemoryRepository(new PasswordService());

            var maybeUserProfile = service.GetUserProfile(VinceEmail);

            Assert.IsTrue(maybeUserProfile.HasValue);
            Assert.AreEqual(VinceEmail, maybeUserProfile.Value.Email);
        }

        [TestMethod]
        public void GetUserProfile_CheckPw()
        {
            var attemptedPw = "Pouy1581";

            var pwService = new PasswordService();
            var service = new UserProfileInMemoryRepository(pwService);

            var maybeUserProfile = service.GetUserProfile(VinceEmail);

            Assert.IsTrue(maybeUserProfile.HasValue);

            var pwVerify = pwService.VerifyPassword(attemptedPw, maybeUserProfile.Value.Salt, maybeUserProfile.Value.PasswordHash);
            Assert.IsTrue(pwVerify);
        }

        [TestMethod]
        public void GetUserProfile_ProfileDoesNotExist_ReturnsEmptyMaybe()
        {
            var service = new UserProfileInMemoryRepository(new PasswordService());

            var mUserProfile = service.GetUserProfile("pølle@fjumsenumse.ass");

            Assert.IsTrue(mUserProfile.IsEmpty);
        }

        [TestMethod]
        public void CreateUserProfile_ProfileDoesAlreadyExist_ThrowsException()
        {
            var service = new UserProfileInMemoryRepository(new PasswordService());
            var cmd = new CreateUserProfile
            {
                Email = VinceEmail
            };

            try
            {
                service.AddUserProfile(cmd);
            }
            catch (VidepaArgumentException)
            {
                return;
            }

            Assert.Fail("Expected VidepaArgumentException");
        }

        [TestMethod]
        public void CreateUserProfile_ProfileDoesNotExist_AddsUserProfileToDictionary()
        {
            var service = new UserProfileInMemoryRepository(new PasswordService());
            var cmd = new CreateUserProfile
            {
                Email = "taske.lone@gmail.com",
                Password = "LoneHar2LangeTasker!",
                Name = "Lone",
                Cellphone = "12345678"

            };

            service.AddUserProfile(cmd);
            var up = service.GetUserProfile(cmd.Email);

            Assert.IsTrue(up.HasValue);
            Assert.AreNotEqual(Guid.Empty, up.Value.Id);
        }
    }
}
