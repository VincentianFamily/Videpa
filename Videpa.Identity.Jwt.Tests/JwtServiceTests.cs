using System.Linq;
using System.Security.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Videpa.Identity.Jwt.Tests
{
    [TestClass]
    public class JwtServiceTests
    {
        [TestMethod]
        public void GenerateJwt()
        {
            const string nameClaimType = "x-name";
            const string emailClaimType = "x-email";
            const string cellphoneClaimType = "x-cellphone";

            const string name = "Frederik Kiær";
            const string email = "frk@bluegarden.dk";
            const string cellphone = "+4542480481";
            const string userRole = "user";
            const string leaderRole = "leader";
            const string adminRole = "administrator";

            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim(nameClaimType, name));
            claimsIdentity.AddClaim(new Claim(emailClaimType, email));
            claimsIdentity.AddClaim(new Claim(cellphoneClaimType, cellphone));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, userRole));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, leaderRole));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, adminRole));



            var service = new JwtService();

            var jwt = service.Generate(claimsIdentity);

            // Kopier jwt strengen til jwt.io for at se den offentlige del af jwt.
            var claimsPlincipal = service.Consume(jwt);

            var nameClaim = claimsPlincipal.Claims.FirstOrDefault(p => p.Type.Equals(nameClaimType));
            var emailClaim = claimsPlincipal.Claims.FirstOrDefault(p => p.Type.Equals(emailClaimType));
            var cellphoneClaim = claimsPlincipal.Claims.FirstOrDefault(p => p.Type.Equals(cellphoneClaimType));

            Assert.AreEqual(nameClaim.Value, name);
            Assert.AreEqual(emailClaim.Value, email);
            Assert.AreEqual(cellphoneClaim.Value, cellphone);

            Assert.IsTrue(claimsPlincipal.HasClaim(p => p.Type == ClaimTypes.Role && p.Value == userRole));
            Assert.IsTrue(claimsPlincipal.HasClaim(p => p.Type == ClaimTypes.Role && p.Value == leaderRole));
            Assert.IsTrue(claimsPlincipal.HasClaim(p => p.Type == ClaimTypes.Role && p.Value == adminRole));
        }
    }
}
