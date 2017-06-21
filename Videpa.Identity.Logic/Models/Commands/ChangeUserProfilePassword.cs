using Videpa.Core;

namespace Videpa.Identity.Logic.Models.Commands
{
    public class ChangeUserProfilePassword : ICommand
    {
        public string Password { get; set; }
        public string PasswordRetyped { get; set; }
    }
}