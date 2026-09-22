using GestionPedidos.Data;
using GestionPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionPedidos.Services;

public class ProveedorService
{
    private readonly AppDbContext _db;

    public ProveedorService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Proveedor>> ObtenerActivosAsync()
    {
        return await _db.Proveedores
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Proveedor?> ObtenerPorIdAsync(int id)
    {
        return await _db.Proveedores.FindAsync(id);
    }

    public async Task<Proveedor> CrearAsync(Proveedor proveedor)
    {
        _db.Proveedores.Add(proveedor);
        await _db.SaveChangesAsync();
        return proveedor;
    }

    public async Task ActualizarAsync(Proveedor proveedor)
    {
        _db.Proveedores.Update(proveedor);
        await _db.SaveChangesAsync();
    }

    public async Task DesactivarAsync(int id)
    {
        var proveedor = await _db.Proveedores.FindAsync(id);
        if (proveedor == null) return;

        proveedor.Activo = false;
        await _db.SaveChangesAsync();
    }
}
