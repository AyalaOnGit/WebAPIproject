using DTOs;
using Entities;
using Repository;

namespace Services
{
    public interface IProductService
    {
        Task<PageResponseDTO<ProductDTO>> GetProducts(string? description, int[]? categories, int? minPrice, int? maxPrice, int? skip, string? orderBy, int? position);
       
    }
}