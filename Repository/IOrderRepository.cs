using Entities;

namespace Repository
{
    public interface IOrderRepository
    {
        Task<Order> AddOrder(Order oreder);
        Task<Order> GetOrderById(int id);
        
    }
}