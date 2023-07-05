using Domain.Interfaces.Service;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BobsWebService.Controllers
{
    [Route("auth")]
    public class AuthenticationController : Controller
    {
        private readonly IUserAuthService authService;

        public AuthenticationController(IUserAuthService userAuthService)
        {
            this.authService = userAuthService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            try
            {
                // Authentication logic
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values);
                }

                var result = await authService.GenerateTokenAsync(model);

                if (result.Status)
                    return Ok(result);

                return BadRequest(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
