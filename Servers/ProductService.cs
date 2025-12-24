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
   
    public async Task<List<ProductDTO>> GetProducts(string? name, int[]? categories, int? nimPrice, int? maxPrice, int? limit, string? orderBy, int? offset)
    {
        return _mapper.Map < List<Product>, List<ProductDTO> >(await _productRepository.GetProducts( name, categories,  nimPrice, maxPrice,  limit,  orderBy, offset));
    }

}
