using AutoMapper;
using Entities;
using DTOs;

namespace WebAPIShop
{
    public class AutoMapping:Profile
    {
        public AutoMapping() 
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, LoginUserDTO>();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>();
            CreateMap<Category, CategoryDTO>();
        }
    }
}
