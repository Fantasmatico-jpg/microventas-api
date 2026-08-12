namespace app.microCliente.services.EventMQ
{
    public interface IRabbitMQConsumerService
    {
        Task StartConsumerAsync();
    }
}