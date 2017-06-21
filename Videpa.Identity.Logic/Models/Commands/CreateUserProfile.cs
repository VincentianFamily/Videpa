using Videpa.Core;

namespace Videpa.Identity.Logic.Models.Commands
{
    public class CreateUserProfile : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordRetyped { get; set; }
        public string Name { get; set; }
        public string Cellphone { get; set; }
    }
}