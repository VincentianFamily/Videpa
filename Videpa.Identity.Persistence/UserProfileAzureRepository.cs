using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Videpa.Core;
using Videpa.Identity.Logic.Exceptions;
using Videpa.Identity.Logic.Interfaces;
using Videpa.Identity.Logic.Models;
using Videpa.Identity.Logic.Ports;

namespace Videpa.Identity.Persistence
{
    public class UserProfileAzureRepository : IUserProfileRepository
    {
        private readonly IPasswordService _passwordService;
        //private const string AzureAccessKey = "0u4T/XEngQED2UBd5iztGkXW582nlUOrCb6Nvxvn1PF/HvRw7DB1syZTUf2xpBoujrhQ8mAPm4g7tgHdvspg8Q==";
        private const string AzureStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=videpa;AccountKey=0u4T/XEngQED2UBd5iztGkXW582nlUOrCb6Nvxvn1PF/HvRw7DB1syZTUf2xpBoujrhQ8mAPm4g7tgHdvspg8Q==;EndpointSuffix=core.windows.net";

        private const string PartitionKey = "test";

        public UserProfileAzureRepository(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public Maybe<UserProfile> GetUserProfile(string email)
        {
            var table = GetTable();
            var entity = GetEntity(email, table);

            if (entity == null)
                return new Maybe<UserProfile>();

            return new Maybe<UserProfile>(Map(entity));
        }

        public void DeleteUserProfile(string email)
        {
            var table = GetTable();
            var entity = GetEntity(email, table);

            if (entity == null) return;

            var operation = TableOperation.Delete(entity);

            table.Execute(operation);
        }

        public void AddUserProfile(CreateUserProfile createUserProfile)
        {
            var table = GetTable();
            var entity = GetEntity(createUserProfile.Email, table);

            if(entity != null)
                throw new VidepaArgumentException("A user profile with the same email already exists");

            var salt = _passwordService.GenerateSalt();
            var pw = _passwordService.HashPassword(salt, createUserProfile.Password);

            var userProfile = new UserProfileTableEntity(PartitionKey, createUserProfile.Email)
            {
                Cellphone = createUserProfile.Cellphone,
                Name = createUserProfile.Name,
                Salt = salt,
                PasswordHash = pw
            };

            var insertOperation = TableOperation.Insert(userProfile);

            table.Execute(insertOperation);
        }

        private UserProfileTableEntity GetEntity(string key, CloudTable table)
        {
            var retrieveOperation = TableOperation.Retrieve<UserProfileTableEntity>(PartitionKey, key);
            var retrievedResult = table.Execute(retrieveOperation);

            return retrievedResult.Result as UserProfileTableEntity;
        }

        private CloudTable GetTable()
        {
            var storageAccount = CloudStorageAccount.Parse(AzureStorageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("userprofiles");
            
            table.CreateIfNotExists();

            return table;
        }

        private UserProfile Map(UserProfileTableEntity entity)
        {
            return new UserProfile
            {
                PasswordHash = entity.PasswordHash,
                Email = entity.Email,
                Salt = entity.Salt,
                Name = entity.Name,
                Cellphone = entity.Cellphone
            };
        }

        private UserProfileTableEntity Map(string partitionKey, UserProfile model)
        {
            return new UserProfileTableEntity(partitionKey, model.Email)
            {
                PasswordHash = model.PasswordHash,
                Salt = model.Salt,
                Name = model.Name,
                Cellphone = model.Cellphone
            };
        }
    }
}
