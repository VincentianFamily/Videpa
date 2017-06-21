using Videpa.Core;

namespace Videpa.Identity.Logic.Models.Commands
{
    public class ChangeUserProfileEmail : ICommand
    {
        public string Email { get; set; }
    }
}