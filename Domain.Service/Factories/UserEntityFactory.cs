using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service.Factories
{
    public static class UserEntityFactory
    {
        public static UserEntity CreateUserEntity(this UserRegisterModel userRegisterModel) 
        {
            return new UserEntity
            {
                Username = userRegisterModel.Name,
                HashedPassword = HashPassword(userRegisterModel.Password)
            };
        }

        public static UserLoginRequest CreateAuthWithError(this UserEntity userEntity)
        {
            return new UserLoginRequest
            {
                Status = false,
                Message = "User or password invalid"
            };
        }

        public static UserLoginRequest CreateAuthWithSucess(
            this UserEntity userEntity, 
            string? token)
        {
            return new UserLoginRequest
            {
                Token = token,
                Status = true,
                Message = ""
            };
        }

        public static bool IsEqualPassword(this UserEntity userEntity, string password)
        {
            string hashedPasswordFromUser = HashPassword(password); // Gera o hash da senha inserida pelo usuário
            return userEntity.HashedPassword.Equals(hashedPasswordFromUser);
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
                return hashedPassword;
            }
        }
    }
}
