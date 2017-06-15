using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Videpa.Identity.Logic.Models;
using Videpa.Identity.Logic.Services;
using Videpa.Identity.Persistence.AzureTables;

namespace Videpa.Identity.Persistence.Tests
{
    [TestClass]
    public class UserProfileAzureRepositoryTests
    {
        private UserProfileAzureRepository _repo;

        [TestInitialize]
        public void Init()
        {
            _repo = new UserProfileAzureRepository(new PasswordService());
        }
        
        [TestMethod]
        public void DeleteUserProfile_ProfileDoesNotExists_DoesNothing()
        {
            _repo.DeleteUserProfile(Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void GetUserProfile_ProfileDoesNotExists_ReturnsEmpty()
        {
            var maybeUserProfile = _repo.GetUserProfile(Guid.NewGuid().ToString());

            Assert.IsTrue(maybeUserProfile.IsEmpty);
        }

        [TestMethod]
        public void GetUserProfile_ProfileExists_ReturnsUserProfile()
        {
            var email = Guid.NewGuid().ToString();
            _repo.AddUserProfile(new CreateUserProfile
            {
                Email = email,
                Name = "Vincent de Paul",
                Cellphone = "42480481",
                Password = "LoneHar2LangeTasker"
            });

            var maybeUserProfile = _repo.GetUserProfile(email);

            Assert.IsTrue(maybeUserProfile.HasValue);
            Assert.AreNotEqual(Guid.Empty, maybeUserProfile.Value.Id);
        }

        [TestMethod]
        public void FullFlowUserProfile_Create_Get_VerifyPassword_Delete()
        {
            var email = Guid.NewGuid().ToString();
            var cmd = new CreateUserProfile
            {
                Email = email,
                Name = "Palle",
                Cellphone = "123454678",
                Password = "PalleElskerPølse"
            };

            _repo.AddUserProfile(cmd);

            var maybeUserProfile = _repo.GetUserProfile(email);

            Assert.IsTrue(maybeUserProfile.HasValue);

            var userProfile = maybeUserProfile.Value;

            Assert.AreEqual(cmd.Email, userProfile.Email);
            Assert.AreEqual(cmd.Cellphone, userProfile.Cellphone);
            Assert.AreEqual(cmd.Name, userProfile.Name);
            
            var pwService = new PasswordService();

            Assert.IsTrue(pwService.VerifyPassword(cmd.Password, userProfile.Salt, userProfile.PasswordHash));

            _repo.DeleteUserProfile(email);

            Assert.IsTrue(_repo.GetUserProfile(email).IsEmpty);
        }
    }
}