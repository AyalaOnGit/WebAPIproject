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
        // 1. ניסיון שליפה מה-Cache עם הגנה
        try
        {
            var cachedCategories = await _cache.GetStringAsync(CacheKey);
            if (cachedCategories != null)
            {
                return JsonSerializer.Deserialize<List<CategoryDTO>>(cachedCategories);
            }
        }
        catch (Exception ex)
        {
            // רישום ללוג שה-Cache לא זמין, אבל לא עוצרים את הריצה
            Console.WriteLine($"Redis is unavailable: {ex.Message}");
        }

        // 2. שליפה מה-Repository (תמיד יקרה אם ה-Cache ריק או אם Redis נפל)
        var categories = _mapper.Map<List<Category>, List<CategoryDTO>>(await _categoryRepository.GetCategories());

        // 3. ניסיון שמירה ב-Cache לפעם הבאה
        try
        {
            var ttlString = _configuration["Redis:TTL"];
            var ttl = string.IsNullOrEmpty(ttlString) ? 3600 : int.Parse(ttlString);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ttl)
            };
            await _cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(categories), options);
        }
        catch
        {
            // ממשיכים כרגיל גם אם השמירה נכשלה
        }

        return categories;
    }

    public async Task InvalidateCategoryCache()
    {
        try
        {
            await _cache.RemoveAsync(CacheKey);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to invalidate cache: {ex.Message}");
        }
    }

}
