using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Services;
using Entities;
using Repository;
using DTOs;


namespace WebAPIShop.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> Get(string? name, int[]? categories, int? minPrice, int? maxPrice, int? limit, string? orderBy, int offset=1)
        {
            List<ProductDTO> products = await _productService.GetProducts( name, categories,  minPrice, maxPrice, limit,  orderBy,  offset);
            if(products!=null)
            {
                return Ok(products);
            }
            return NoContent();
        }
    }
}
