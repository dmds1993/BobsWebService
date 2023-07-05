using Domain.Interfaces.Service;
using Domain.Models;
using Infra.SqlServer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Service.Factories;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;

namespace Domain.Service.Services
{
    public class UserAuthService : IUserAuthService
    {
        private static string UserId = "UserId";

        private readonly SqlServerContext sqlServerContext;
        private readonly IConfiguration configuration;

        public UserAuthService(
            SqlServerContext sqlServerContext, 
            IConfiguration configuration) 
        {
            this.sqlServerContext = sqlServerContext;
            this.configuration = configuration;
        }

        public async Task<UserRegisterRequest> CreateUser(UserRegisterModel userRegisterModel)
        {
            if (await sqlServerContext.UserEntity.AnyAsync(u => u.Username == userRegisterModel.Name))
            {
                userRegisterModel.CreateWithError("ERROR TO CREATE USER");
            }

            await sqlServerContext.UserEntity.AddAsync(userRegisterModel.CreateUserEntity());
            await sqlServerContext.SaveChangesAsync();

            return userRegisterModel.CreateWithSuccess("User create with success");
        }

        public async Task<UserLoginRequest> GenerateTokenAsync(LoginModel loginModel)
        {
            var user = await sqlServerContext.UserEntity.FirstAsync(c => c.Username == loginModel.Name);

            if (user == null || !user.IsEqualPassword(loginModel.Password))
            {
                return user?.CreateAuthWithError();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(UserId, user.Id.ToString())
            };

            var secretKey = configuration["JwtSettings:SecretKey"];
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Set the token expiration time
                SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(token);

            return user.CreateAuthWithSucess(encodedToken);
        }
    }
}
