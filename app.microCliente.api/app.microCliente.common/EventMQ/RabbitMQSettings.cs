namespace app.microCliente.common.EventMQ
{
    public class RabbitMQSettings
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string VirtualHost { get; set; } = "/";
        public int Port { get; set; }
        public string Hostname { get; set; } = string.Empty;
    }
}