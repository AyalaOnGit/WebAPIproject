namespace Services;

using AutoMapper;
using DTOs;
using Entities;
using Microsoft.Extensions.Logging;
using Repository;


public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository orderRepository,IMapper mapper, IProductRepository productRepository, ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<OrderDTO> AddOrder(OrderDTO oreder)
    {
        double sum = 0;
        foreach (var item in oreder.OrderItems)
        {
            Product product = await _productRepository.GetProductById(item.ProductID);
            if (product != null)
                sum += product.Price * item.Quantity;
        }
        if (sum != oreder.OrderSum)
        {
            _logger.LogWarning("Order sum mismatch for user {UserId}. Client sent: {ClientSum}, Server calculated: {ServerSum} 😖",oreder.UserId, oreder.OrderSum, sum);
            oreder = oreder with { OrderSum = sum };
        }
        return _mapper.Map <Order, OrderDTO > (await _orderRepository.AddOrder(_mapper.Map < OrderDTO, Order > (oreder)));
    }

    public async Task<OrderDTO> GetOrderById(int id)
    {
        return _mapper.Map < Order, OrderDTO > (await _orderRepository.GetOrderById(id));
    }

}
