using System;
using Videpa.Identity.Logic.Models;

namespace Videpa.Identity.Logic.Interfaces
{
    public interface IUserProfileCommandHandler
    {
        AuthenticatedUserProfile Authenticate(AuthenticateUserProfile authenticate);

        AuthenticatedUserProfile Create(CreateUserProfile cmd);

        AuthenticatedUserProfile Update(Guid userId, UpdateUserProfile cmd);

        AuthenticatedUserProfile ChangeEmail(Guid userId, ChangeUserProfileEmail cmd);

        AuthenticatedUserProfile ChangePassword(Guid userId, ChangeUserProfilePassword cmd);
    }
}