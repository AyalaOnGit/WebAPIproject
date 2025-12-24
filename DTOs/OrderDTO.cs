using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public record OrderDTO(DateOnly OrderDate, int OrderId, double OrderSum, ICollection<OrderItemDTO> OrderItems, int UserId);
}
