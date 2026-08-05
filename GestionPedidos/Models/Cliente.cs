using System.ComponentModel.DataAnnotations;

namespace GestionPedidos.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        //Validaciones de datos hechas con Data Annotations
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string Nombre { get; set; } = "";
        public TipoCliente Tipo { get; set; } = TipoCliente.Empresa;

        [StringLength(20)]
        public string? Cif { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^[0-9\s+\-()]*$", ErrorMessage = "Solo se permiten números, espacios y los símbolos + - ( )")]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Formato de email no válido")]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(300)]
        public string? Direccion { get; set; }


        [StringLength(100)]
        public string? Poblacion { get; set; }

        [StringLength(1000)]
        public string? Observaciones { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaAlta { get; set; } = DateTime.Now;
    }

    public enum TipoCliente 
    {
        Empresa,
        Particular
    }
}
