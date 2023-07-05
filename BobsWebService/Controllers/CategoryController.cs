using Domain.Interfaces.Service;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BobsWebService.ExtensionMethods.SimpleValidator;

namespace BobsWebService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryManagementService categoryManagementService;
        public CategoryController(ICategoryManagementService categoryManagementService) 
        {
            this.categoryManagementService = categoryManagementService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Details(string categoryName)
        {
            try
            {
                var result = categoryManagementService.GetCategory(categoryName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the processing
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("")]
        
        public async Task<IActionResult> CreateCategory([FromBody] Dictionary<string, string> categoryDictionary)
        {
            try
            {
                if(categoryDictionary == null || categoryDictionary.IsValid()) 
                {
                    return BadRequest("Payload invalid, please send with MAX LENGHT of 300 KEY and VALUE");
                }

                await categoryManagementService.CreateCategory(categoryDictionary);
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the processing
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
