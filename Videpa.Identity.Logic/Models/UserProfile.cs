namespace Videpa.Identity.Logic.Models
{
    public class UserProfile
    {
        public string Name { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        
        public byte[] Salt { get; set; }

        public string PasswordHash { get; set; }
    }
}
