using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPedidos.Models
{
    public class Pedido : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de albarán es obligatorio")]
        [StringLength(30)]
        public string NumeroAlbaran { get; set; } = "";

        public DateTime Fecha { get; set; } = DateTime.Today;

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public List<LineaPedido> Lineas { get; set; } = new();

        [StringLength(1000)]
        public string? Observaciones { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        [NotMapped]
        public decimal Total => Lineas.Sum(l => l.Subtotal);

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Lineas.Any())
            {
                yield return new ValidationResult(
                    "El pedido debe tener al menos una línea",
                    new[] { nameof(Lineas) });
            }
        }
    }

    public class LineaPedido
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;

        [Column(TypeName = "decimal(10,3)")]
        [Range(0.001, 100000, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public decimal Cantidad { get; set; } = 1;

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 100000, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal PrecioUnitario { get; set; }

        [NotMapped]
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
