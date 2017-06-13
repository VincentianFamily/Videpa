using Videpa.Core;
using Videpa.Identity.Logic.Models;

namespace Videpa.Identity.Logic.Ports
{
    public interface IUserProfileRepository
    {
        Maybe<UserProfile> GetUserProfile(string email);
        void AddUserProfile(CreateUserProfile createUserProfile);
    }
}