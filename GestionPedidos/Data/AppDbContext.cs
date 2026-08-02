using GestionPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionPedidos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Cliente> Clientes => Set<Cliente>();
    }
}
