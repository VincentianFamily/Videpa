namespace Videpa.Identity.Logic.Interfaces
{
    public interface IPasswordService
    {
        byte[] GenerateSalt();
        string HashPassword(byte[] salt, string password);
        bool VerifyPassword(string attemptedPassword, byte[] salt, string passwordHash);
    }
}