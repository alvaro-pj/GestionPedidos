using GestionPedidos.Data;
using GestionPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionPedidos.Services;

public class ProductoService
{
    private readonly AppDbContext _db;

    public ProductoService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Producto>> ObtenerActivosAsync()
    {
        return await _db.Productos
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return await _db.Productos.FindAsync(id);
    }

    public async Task<Producto> CrearAsync(Producto producto)
    {
        _db.Productos.Add(producto);
        await _db.SaveChangesAsync();
        return producto;
    }

    public async Task ActualizarAsync(Producto producto)
    {
        _db.Productos.Update(producto);
        await _db.SaveChangesAsync();
    }

    public async Task DesactivarAsync(int id)
    {
        var producto = await _db.Productos.FindAsync(id);
        if (producto == null) return;

        producto.Activo = false;
        await _db.SaveChangesAsync();
    }
}
