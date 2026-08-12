using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.microCliente.entities.models
{
    public class HistorialCompra : EntityBase
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int VentaId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal Total { get; set; }

        [Required]
        public DateTime FechaCompra { get; set; }

        public Cliente? Cliente { get; set; }
    }
}