using System.Text;
using System.Text.Json;
using app.microCliente.common.DTOs;
using app.microCliente.common.EventMQ;
using app.microCliente.services.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;

namespace app.microCliente.services.EventMQ
{
    public class RabbitMQConsumerService : IRabbitMQConsumerService
    {
        private readonly RabbitMQSettings _settings;
        private readonly IServiceScopeFactory _scopeFactory;

        private const string ExchangeName = "ventas.exchange";
        private const string QueueName = "microclientes.historial";
        private const string RoutingKey = "ventas.creada";

        public RabbitMQConsumerService(
            IOptions<RabbitMQSettings> settings,
            IServiceScopeFactory scopeFactory)
        {
            _settings = settings.Value;
            _scopeFactory = scopeFactory;
        }

        public async Task StartConsumerAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Hostname,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false);

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            await channel.QueueBindAsync(
                queue: QueueName,
                exchange: ExchangeName,
                routingKey: RoutingKey);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(
                        args.Body.ToArray());

                    Console.WriteLine(
                        $"[RabbitMQ] Evento recibido: {json}");

                    var evento =
                        JsonSerializer.Deserialize<VentaCreadaEvent>(
                            json,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                    if (evento == null)
                    {
                        Console.WriteLine(
                            "[RabbitMQ] No se pudo deserializar el evento.");

                        await channel.BasicNackAsync(
                            args.DeliveryTag,
                            multiple: false,
                            requeue: false);

                        return;
                    }

                    Console.WriteLine(
                        $"[RabbitMQ] VentaId: {evento.VentaId}");

                    Console.WriteLine(
                        $"[RabbitMQ] ClienteId: {evento.ClienteId}");

                    Console.WriteLine(
                        $"[RabbitMQ] ProductoId: {evento.ProductoId}");

                    Console.WriteLine(
                        $"[RabbitMQ] Cantidad: {evento.Cantidad}");

                    Console.WriteLine(
                        $"[RabbitMQ] Total: {evento.Total}");

                    Console.WriteLine(
                        $"[RabbitMQ] Fecha: {evento.Fecha}");

                    var historial = new HistorialCompraDTO
                    {
                        ClienteId = (int)evento.ClienteId,
                        VentaId = (int)evento.VentaId,
                        ProductoId = (int)evento.ProductoId,
                        Cantidad = evento.Cantidad,
                        Total = evento.Total,
                        FechaCompra = evento.Fecha,
                        Estado = true
                    };

                    Console.WriteLine(
                        "[RabbitMQ] Creando scope para guardar historial...");

                    using var scope = _scopeFactory.CreateScope();

                    var historialService =
                        scope.ServiceProvider
                            .GetRequiredService<IHistorialCompraService>();

                    Console.WriteLine(
                        "[RabbitMQ] Intentando guardar historial...");

                    var resultado =
                        await historialService.Insertar(historial);

                    if (resultado.Success)
                    {
                        Console.WriteLine(
                            $"[RabbitMQ] Historial guardado correctamente. " +
                            $"Id: {resultado.Result?.Id}");

                        await channel.BasicAckAsync(
                            args.DeliveryTag,
                            multiple: false);
                    }
                    else
                    {
                        Console.WriteLine(
                            $"[RabbitMQ] Error al guardar historial: " +
                            $"{resultado.ErrorMessage}");

                        await channel.BasicNackAsync(
                            args.DeliveryTag,
                            multiple: false,
                            requeue: false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"[RabbitMQ] Error procesando evento: {ex.Message}");

                    await channel.BasicNackAsync(
                        args.DeliveryTag,
                        multiple: false,
                        requeue: true);
                }
            };

            await channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,
                consumer: consumer);

            Console.WriteLine(
                $"[RabbitMQ] Consumer iniciado. " +
                $"Cola: {QueueName}");
        }
    }
}