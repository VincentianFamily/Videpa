using System.Security.Claims;
using Videpa.Identity.Logic.Interfaces;
using Videpa.Identity.Logic.Models;

namespace Videpa.Identity.Logic.Ports
{
    public interface IJwtIssuer
    {
        string Generate(ClaimsIdentity identity);

        AuthenticatedUserProfile Generate(UserProfile userProfile, IClaimsIdentityBuilder claimsIdentityBuilder);

    }
}