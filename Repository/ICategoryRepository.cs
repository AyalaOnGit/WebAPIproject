using Entities;

namespace Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
        

    }
}