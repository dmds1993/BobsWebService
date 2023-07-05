using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service.Factories
{
    public static class UserRegisterRequestFactory
    {
        public static UserRegisterRequest CreateWithError(
            this UserRegisterModel userRegisterModel,
            string message)
        {
            return new UserRegisterRequest
            {
                Message = message,
                Status = false
            };
        }

        public static UserRegisterRequest CreateWithSuccess(
            this UserRegisterModel userRegisterModel,
            string message)
        {
            return new UserRegisterRequest
            {
                Message = message,
                Status = true
            };
        }
    }
}
