using Microsoft.WindowsAzure.Storage.Table;

namespace Videpa.Identity.Persistence
{
    internal class UserProfileTableEntity : TableEntity
    {
        public UserProfileTableEntity() { }

        public UserProfileTableEntity(string partitionKey, string email)
        {
            PartitionKey = partitionKey;
            RowKey = email;
            Email = email;
        }

        public string Name { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public byte[] Salt { get; set; }
        public string PasswordHash { get; set; }
    }
}