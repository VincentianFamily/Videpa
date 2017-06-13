namespace Videpa.Identity.Logic.Interfaces
{
    public interface IPasswordService
    {
        byte[] GetPasswordBytes(string password);
        string HashPassword(byte[] salt, byte[] password);
        bool VerifyPassword(string attemptedPassword, byte[] salt, string passwordHash);
    }
}