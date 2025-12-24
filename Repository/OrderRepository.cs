using System.Text.Json;
using Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        db_shopContext _ShopContext;
        public OrderRepository(db_shopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }
        public async Task<Order> AddOrder(Order oreder)
        {
            await _ShopContext.Orders.AddAsync(oreder);
            await _ShopContext.SaveChangesAsync();
            return await _ShopContext.FindAsync<Order>(oreder.OrderId);
        }
        public async Task<Order> GetOrderById(int id)
        {
            return await _ShopContext.FindAsync<Order>(id);
        }

    }
}
