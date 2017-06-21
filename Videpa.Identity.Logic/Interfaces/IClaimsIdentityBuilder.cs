using System.Security.Claims;
using Videpa.Identity.Logic.Models;

namespace Videpa.Identity.Logic.Interfaces
{
    public interface IClaimsIdentityBuilder
    {
        ClaimsIdentity BuildIdentity(UserProfile userProfile);
    }
}