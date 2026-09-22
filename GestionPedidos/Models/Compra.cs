using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPedidos.Models
{
    public class Compra : IValidatableObject
    {
        public int Id { get; set; }

        [StringLength(30)]
        public string? NumeroFactura { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Today;

        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; } = null!;

        public List<LineaCompra> Lineas { get; set; } = new();

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
                    "La compra debe tener al menos una línea",
                    new[] { nameof(Lineas) });
            }
        }
    }

    public class LineaCompra
    {
        public int Id { get; set; }

        public int CompraId { get; set; }
        public Compra Compra { get; set; } = null!;

        [Required(ErrorMessage = "El concepto es obligatorio")]
        [StringLength(200)]
        public string Concepto { get; set; } = "";

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
