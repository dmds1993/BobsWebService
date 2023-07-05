using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Service
{
    public interface IUserAuthService
    {
        Task<UserRegisterRequest> CreateUser(UserRegisterModel userRegisterModel);
        Task<UserLoginRequest> GenerateTokenAsync(LoginModel loginModel);
    }
}
