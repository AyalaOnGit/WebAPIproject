using Microsoft.AspNetCore.Mvc;
using Services;
using DTOs;
using Repository;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService) 
        { 
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> Get() 
        {
            List<CategoryDTO> categories = await _categoryService.GetCategories();
            if (categories != null)
            {
                return Ok(categories);
            }
            return NoContent();
        }
    }
}
