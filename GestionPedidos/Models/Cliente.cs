namespace GestionPedidos.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";
        public TipoCliente Tipo { get; set; } = TipoCliente.Empresa;
        public string? Cif { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public string? Poblacion { get; set; }
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
