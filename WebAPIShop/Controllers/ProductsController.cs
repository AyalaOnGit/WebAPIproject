using Microsoft.AspNetCore.Mvc;
using Services;
using Repository;
using DTOs;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> Get(string? description, [FromQuery] int[]? categories, int? minPrice, int? maxPrice, int? skip, string? orderBy, int position = 1)
        {
            PageResponseDTO<ProductDTO> metaData = await _productService.GetProducts(description, categories, minPrice, maxPrice, skip, orderBy, position);
            if (metaData != null)
            {
                return Ok(metaData);
            }
            return NoContent();
        }
    }
}
