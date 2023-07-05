using Domain.Interfaces.Service;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BobsWebService.Controllers
{
    [Route("api/v1/[controller]")]
    public class RegisterUserController : Controller
    {
        private readonly IUserAuthService userAuthService;
        
        public RegisterUserController(IUserAuthService userAuthService)
        {
            this.userAuthService = userAuthService;
        }

        [HttpPost("")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterModel userRegisterModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await userAuthService.CreateUser(userRegisterModel);

                if (!result.Status)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
