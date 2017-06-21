using Videpa.Core;

namespace Videpa.Identity.Logic.Models.Commands
{
    public class UpdateUserProfile : ICommand
    {
        public string Name { get; set; }
        public string Cellphone { get; set; }
    }
}