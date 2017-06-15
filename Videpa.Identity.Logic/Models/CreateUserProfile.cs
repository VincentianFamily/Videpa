using Videpa.Core;

namespace Videpa.Identity.Logic.Models
{
    public class CreateUserProfile : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordRetyped { get; set; }
        public string Name { get; set; }
        public string Cellphone { get; set; }
    }

    public class AuthenticateUserProfile : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UpdateUserProfile : ICommand
    {
        public string Name { get; set; }
        public string Cellphone { get; set; }
    }

    public class ChangeUserProfileEmail : ICommand
    {
        public string Email { get; set; }
    }

    public class ChangeUserProfilePassword : ICommand
    {
        public string Password { get; set; }
        public string PasswordRetyped { get; set; }
    }
}