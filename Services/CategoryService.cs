namespace Services;
using AutoMapper;
using DTOs;
using Entities;
using Repository;


public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper   )
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    public async Task<List<CategoryDTO>> GetCategories()
    {
        return _mapper.Map <List<Category>, List<CategoryDTO>>(await _categoryRepository.GetCategories());
    }

}
