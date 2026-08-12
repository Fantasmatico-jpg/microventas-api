namespace app.microCliente.common.EventMQ
{
    public class VentaCreadaEvent
    {
        public int VentaId { get; set; }

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public decimal Total { get; set; }

        public DateOnly Fecha { get; set; }
    }
}