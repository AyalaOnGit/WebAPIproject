using DTOs;
using Entities;
using Repository;

namespace Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetCategories();
      
    }
}