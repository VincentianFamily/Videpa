﻿using System;
using System.Security.Cryptography;
using System.Text;
using Videpa.Identity.Logic.Interfaces;

namespace Videpa.Identity.Logic.Services
{
    public class PasswordService : IPasswordService
    {
        private const string HashKey = "NissePok";

        public byte[] GenerateSalt()
        {
            var saltBytes = new byte[16];
            var p = new RNGCryptoServiceProvider();
            p.GetBytes(saltBytes);

            return saltBytes;
        }
        
        public string HashPassword(byte[] salt, string password)
        {
            var pwBytes = GetPasswordBytes(password);

            var allBytes = new byte[salt.Length + password.Length];

            Buffer.BlockCopy(salt, 0, allBytes, 0, salt.Length);
            Buffer.BlockCopy(pwBytes, 0, allBytes, salt.Length, password.Length);

            var bHashKey = Encoding.UTF8.GetBytes(HashKey);

            using (var s = new HMACSHA256(bHashKey))
            {
                var hash = s.ComputeHash(allBytes);
                return Convert.ToBase64String(hash);
            }
        }
        
        public bool VerifyPassword(string attemptedPassword, byte[] salt, string passwordHash)
        {
            var hAttemptedPassword = HashPassword(salt, attemptedPassword);

            return hAttemptedPassword == passwordHash;
        }

        private byte[] GetPasswordBytes(string password)
        {
            return Encoding.UTF8.GetBytes(password);
        }
    }
}