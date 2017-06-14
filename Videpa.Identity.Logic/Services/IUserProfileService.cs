namespace Videpa.Identity.Logic.Services
{
    public interface IUserProfileService
    {
        string Authenticate(string email, string password);
    }
}