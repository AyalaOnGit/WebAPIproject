using Entities;

namespace Repository
{
    public interface IProductRepository
    {
       Task<(List<Product> Items, int TotalCount)> GetProducts(string? name, int[]? categories, int? nimPrice, int? maxPrice, int? limit, string? orderBy, int? offset);

    }
}