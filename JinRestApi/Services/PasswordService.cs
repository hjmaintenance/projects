using Microsoft.AspNetCore.Identity;
using JinRestApi.Models;

namespace JinRestApi.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<Customer> _hasher = new();

        public string HashPassword(Customer user, string password)
        {
            return _hasher.HashPassword(user, password);
        }

        public bool VerifyPassword(Customer user, string password)
        {
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}