using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Videpa.Identity.Logic.Tests
{
    [TestClass]
    public class PasswordServiceTests
    {
        [TestMethod]
        public void VerifyPassword_PasswordsAndSaltMatch_ReturnsTrue()
        {
            var service = new PasswordService();

            var salt = service.GenerateSalt();

            var password1 = "123456";
            var password2 = password1;

            var password1Hash = service.HashPassword(salt, service.GetPasswordBytes(password1));

            var isValid = service.VerifyPassword(password2, salt, password1Hash);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void VerifyPassword_PasswordsMatchButSaltMismatch_ReturnsFalse()
        {
            var service = new PasswordService();

            var salt1 = service.GenerateSalt();
            var salt2 = service.GenerateSalt();

            var password1 = "123456";
            var password2 = password1;

            var password1Hash = service.HashPassword(salt1, service.GetPasswordBytes(password1));

            var isValid = service.VerifyPassword(password2, salt2, password1Hash);

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void VerifyPassword_PasswordsMismatchButSaltMatch_ReturnsFalse()
        {
            var service = new PasswordService();

            var salt = service.GenerateSalt();
            
            var password1 = "123456";
            var password2 = "123457";

            var password1Hash = service.HashPassword(salt, service.GetPasswordBytes(password1));

            var isValid = service.VerifyPassword(password2, salt, password1Hash);

            Assert.IsFalse(isValid);
        }
    }
}
