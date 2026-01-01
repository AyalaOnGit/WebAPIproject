namespace Services;

using AutoMapper;
using DTOs;
using Entities;
using Repository;


public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository,IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
   
    public async Task<PageResponseDTO<ProductDTO>> GetProducts(string? description, int[]? categories, int? minPrice, int? maxPrice, int? skip, string? orderBy, int? position)
    {
        (List<Product> Items, int TotalCount) = await _productRepository.GetProducts(description, categories, minPrice, maxPrice, skip, orderBy, position);
        List<ProductDTO> ItemsDTO = _mapper.Map < List<Product>, List<ProductDTO> >(Items);     
        var currentPage = (position ?? 1);
        var pageSize = skip ?? 10;
        PageResponseDTO<ProductDTO> responseDTO = new PageResponseDTO<ProductDTO>()
        {
            Data = ItemsDTO,
            TotalItems = TotalCount,
            CurrentPage = currentPage,
            PageSize = pageSize,
            HasPreviousPage = currentPage > 1,
            HasNextPage = (currentPage * pageSize) < TotalCount
        };
        return responseDTO;

    }

}
