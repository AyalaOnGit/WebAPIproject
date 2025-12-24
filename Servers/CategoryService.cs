namespace Services;

using Entities;
using Repository;


public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<List<Category>> GetCategories()
    {
        return await _categoryRepository.GetCategories();
    }

}
