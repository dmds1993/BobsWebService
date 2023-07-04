using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserLoginRequest
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string? Token { get; set; }
    }
}
