using DTOs;
using Entities;
using Repository;

namespace Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetProducts(string? name, int[]? categories, int? nimPrice, int? maxPrice, int? limit, string? orderBy, int? offset);
       
    }
}