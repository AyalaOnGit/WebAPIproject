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
    private readonly IKafkaProducerService _kafkaProducer;

    public OrderService(IOrderRepository orderRepository, IMapper mapper, IProductRepository productRepository, ILogger<OrderService> logger, IKafkaProducerService kafkaProducer)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _productRepository = productRepository;
        _logger = logger;
        _kafkaProducer = kafkaProducer;
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
            _logger.LogWarning("Order sum mismatch for user {UserId}. Client sent: {ClientSum}, Server calculated: {ServerSum} 😖", oreder.UserId, oreder.OrderSum, sum);
            oreder = oreder with { OrderSum = sum };
        }
        OrderDTO createdOrder = _mapper.Map<Order, OrderDTO>(await _orderRepository.AddOrder(_mapper.Map<OrderDTO, Order>(oreder)));
        try { await _kafkaProducer.PublishOrderCreatedAsync(createdOrder); }
        catch (Exception ex) { _logger.LogWarning("Kafka unavailable, order saved without event: {Message}", ex.Message); }
        return createdOrder;
    }

    public async Task<OrderDTO> GetOrderById(int id)
    {
        return _mapper.Map<Order, OrderDTO>(await _orderRepository.GetOrderById(id));
    }

    public async Task<List<OrderDTO>> GetAllOrders()
    {
        return _mapper.Map<List<Order>, List<OrderDTO>>(await _orderRepository.GetAllOrders());
    }

}
