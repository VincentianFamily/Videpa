using System;

namespace Videpa.Identity.Logic.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        
        public byte[] Salt { get; set; }
        public string PasswordHash { get; set; }
    }

    public class AuthenticatedUserProfile : UserProfile
    {
        public AuthenticatedUserProfile() { }

        public AuthenticatedUserProfile(UserProfile userProfile, string jwt = null)
        {
            Id = userProfile.Id;
            Name = userProfile.Name;
            Email = userProfile.Email;
            Cellphone = userProfile.Cellphone;
            PasswordHash = userProfile.PasswordHash;
            Salt = userProfile.Salt;
            Jwt = jwt;
        }

        public string Jwt { get; set; }
    }
}
