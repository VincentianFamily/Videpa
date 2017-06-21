using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Videpa.Core;
using Videpa.Identity.Logic.Exceptions;
using Videpa.Identity.Logic.Interfaces;
using Videpa.Identity.Logic.Models;
using Videpa.Identity.Logic.Models.Commands;
using Videpa.Identity.Logic.Ports;
using Videpa.Identity.Logic.Services;

namespace Videpa.Identity.Logic.Tests
{
    [TestClass]
    public class UserProfileServiceTests
    {
        [TestMethod]
        public void Authenticate_NoUserProfileAssociatedToEmail_ThrowsException()
        {
            var email = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            var jwtIssuerMock = new Mock<IJwtIssuer>();
            var repoMock = new Mock<IUserProfileRepository>();

            repoMock.Setup(p => p.GetUserProfile(email)).Returns(new Maybe<UserProfile>());

            var service = new UserProfileCommandHandler(new PasswordService(), jwtIssuerMock.Object, repoMock.Object);

            try
            {
                service.Authenticate(new AuthenticateUserProfile {Email = email, Password = password});
            }
            catch (VidepaAuthenticationException)
            {
                jwtIssuerMock.Verify(p => p.Generate(It.IsAny<UserProfile>(), It.IsAny<IClaimsIdentityBuilder>()), Times.Never);
                return;
            }

            Assert.Fail("Expected VidepaAuthenticationException");
        }

        [TestMethod]
        public void Authenticate_UserProfileFoundButWrongPassword_ThrowsException()
        {
            var email = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            var jwtFake = Guid.NewGuid().ToString();
            var userProfile = new UserProfile
            {
                Salt = new byte[0],
                PasswordHash = Guid.NewGuid().ToString()
            };

            var jwtIssuerMock = new Mock<IJwtIssuer>();
            var repoMock = new Mock<IUserProfileRepository>();
            var pwMock = new Mock<IPasswordService>();

            repoMock.Setup(p => p.GetUserProfile(email)).Returns(new Maybe<UserProfile>(userProfile)).Verifiable();
            pwMock.Setup(p => p.VerifyPassword(password, userProfile.Salt, userProfile.PasswordHash)).Returns(false).Verifiable();

            var service = new UserProfileCommandHandler(pwMock.Object, jwtIssuerMock.Object, repoMock.Object);

            try
            {
                service.Authenticate(new AuthenticateUserProfile { Email = email, Password = password });
            }
            catch (VidepaAuthenticationException)
            {
                jwtIssuerMock.Verify(p => p.Generate(It.IsAny<UserProfile>(), It.IsAny<IClaimsIdentityBuilder>()), Times.Never);
                repoMock.Verify();
                pwMock.Verify();
                return;
            }

            Assert.Fail("Expected VidepaAuthenticationException");
        }

        [TestMethod]
        public void Authenticate_CredentialsAreCorrect_JwtIsReturned()
        {
            var email = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            var jwtFake = Guid.NewGuid().ToString();
            var userProfile = new UserProfile
            {
                Salt = new byte[0],
                PasswordHash = Guid.NewGuid().ToString()
            };

            var jwtIssuerMock = new Mock<IJwtIssuer>();
            var repoMock = new Mock<IUserProfileRepository>();
            var pwMock = new Mock<IPasswordService>();

            jwtIssuerMock.Setup(p => p.Generate(userProfile, It.IsAny<IClaimsIdentityBuilder>())).Returns(new AuthenticatedUserProfile(userProfile) { Jwt = jwtFake }).Verifiable();
            repoMock.Setup(p => p.GetUserProfile(email)).Returns(new Maybe<UserProfile>(userProfile));
            pwMock.Setup(p => p.VerifyPassword(password, userProfile.Salt, userProfile.PasswordHash)).Returns(true).Verifiable();

            var service = new UserProfileCommandHandler(pwMock.Object, jwtIssuerMock.Object, repoMock.Object);

            var result = service.Authenticate(new AuthenticateUserProfile { Email = email, Password = password });
            
            Assert.AreEqual(jwtFake, result.Jwt);

            jwtIssuerMock.Verify();
            repoMock.Verify();
            pwMock.Verify();
        }
    }
}