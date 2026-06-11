using DTOs;

namespace Services;

public interface IKafkaProducerService
{
    Task PublishOrderCreatedAsync(OrderDTO order);
}
