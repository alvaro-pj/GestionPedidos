using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPedidos.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string Nombre { get; set; } = "";

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 100000, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal PrecioBase { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaAlta { get; set; } = DateTime.Now;
    }
}
