using DTOs;
using Entities;
using Repository;

namespace Services
{
    public interface IOrderService
    {
        Task<OrderDTO> AddOrder(OrderDTO oreder);
        Task<OrderDTO> GetOrderById(int id);
    }
}