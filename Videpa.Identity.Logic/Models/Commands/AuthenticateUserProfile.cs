using Videpa.Core;

namespace Videpa.Identity.Logic.Models.Commands
{
    public class AuthenticateUserProfile : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}