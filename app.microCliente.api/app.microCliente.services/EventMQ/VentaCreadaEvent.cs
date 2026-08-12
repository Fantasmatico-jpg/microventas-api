namespace app.microCliente.services.EventMQ
{
    public class VentaCreadaEvent
    {
        public long VentaId { get; set; }
        public long ClienteId { get; set; }
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
    }
}