﻿using System;
using Videpa.Core;
using Videpa.Identity.Logic.Models;
using Videpa.Identity.Logic.Models.Commands;

namespace Videpa.Identity.Logic.Ports
{
    public interface IUserProfileRepository : IUserProfileQueries
    {
        void AddUserProfile(CreateUserProfile createUserProfile);
    }

    public interface IUserProfileQueries
    {
        Maybe<UserProfile> GetUserProfile(string email);
        Maybe<UserProfile> GetUserProfile(Guid userId);
    }
}