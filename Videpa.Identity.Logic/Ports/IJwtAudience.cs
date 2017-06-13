using System.Security.Claims;

namespace Videpa.Identity.Logic.Ports
{
    public interface IJwtAudience
    {
        ClaimsIdentity Consume(string jwt);
    }
}