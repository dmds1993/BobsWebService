using Domain.Interfaces.Service;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BobsWebService.Controllers
{
    [ApiController]
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
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var result = categoryManagementService.GetCategory(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the processing
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("")]
        public ActionResult GetList()
        {
            return Ok(default);
        }

        [HttpPost("categories")]
        
        public async Task<IActionResult> CreateCategory([FromBody] Dictionary<string, string> categoryDictionary)
        {
            try
            {
                await categoryManagementService.Create(categoryDictionary);
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the processing
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult Edit(int id)
        {
            return Ok(default);
        }

        [HttpDelete]
        [Route("")]
        public ActionResult Delete(int id)
        {
            return Ok(default);
        }
    }
}
