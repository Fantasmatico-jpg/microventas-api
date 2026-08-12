namespace app.microCliente.common.DTOs
{
    public class HistorialCompraDTO
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public int VentaId { get; set; }

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public decimal Total { get; set; }

        public DateTime FechaCompra { get; set; }

        public bool Estado { get; set; }
    }
}