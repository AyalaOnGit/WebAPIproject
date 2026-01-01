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
        public async Task<ActionResult<List<ProductDTO>>> Get(string? description, int[]? categories, int? minPrice, int? maxPrice, int? skip, string? orderBy, int position=1)
        {
            PageResponseDTO<ProductDTO> metaData = await _productService.GetProducts(description, categories, minPrice, maxPrice, skip, orderBy, position);
            if(metaData != null)
            {
                return Ok(metaData);
            }
            return NoContent();
        }
    }
}
