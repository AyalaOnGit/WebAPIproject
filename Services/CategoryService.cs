namespace Services;
using AutoMapper;
using DTOs;
using Entities;
using Repository;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.Extensions.Configuration;


public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;
    private const string CacheKey = "categories";

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IDistributedCache cache, IConfiguration configuration)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _cache = cache;
        _configuration = configuration;
    }
    public async Task<List<CategoryDTO>> GetCategories()
    {
        var cachedCategories = await _cache.GetStringAsync(CacheKey);
        if (cachedCategories != null)
        {
            return JsonSerializer.Deserialize<List<CategoryDTO>>(cachedCategories);
        }

        var categories = _mapper.Map<List<Category>, List<CategoryDTO>>(await _categoryRepository.GetCategories());
        var ttlString = _configuration["Redis:TTL"];
        var ttl = string.IsNullOrEmpty(ttlString) ? 3600 : int.Parse(ttlString);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ttl)
        };
        await _cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(categories), options);
        return categories;
    }

    public async Task InvalidateCategoryCache()
    {
        await _cache.RemoveAsync(CacheKey);
    }

}
