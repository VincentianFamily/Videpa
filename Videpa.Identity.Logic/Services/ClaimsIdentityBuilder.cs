using System.Security.Claims;
using Videpa.Identity.Logic.Interfaces;
using Videpa.Identity.Logic.Models;

namespace Videpa.Identity.Logic.Services
{
    public class ClaimsIdentityBuilder : IClaimsIdentityBuilder
    {
        public ClaimsIdentity BuildIdentity(UserProfile userProfile)
        {
            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim(VidepaClaims.Id, userProfile.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(VidepaClaims.Name, userProfile.Name));
            claimsIdentity.AddClaim(new Claim(VidepaClaims.Email, userProfile.Email));
            claimsIdentity.AddClaim(new Claim(VidepaClaims.Cellphone, userProfile.Cellphone));

            return claimsIdentity;
        }
    }
}