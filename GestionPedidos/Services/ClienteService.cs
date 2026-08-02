using GestionPedidos.Data;
using GestionPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionPedidos.Services;

public class ClienteService
{
    private readonly AppDbContext _db;

    public ClienteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Cliente>> ObtenerActivosAsync()
    {
        return await _db.Clientes
            .Where(c => c.Activo)
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<Cliente?> ObtenerPorIdAsync(int id)
    {
        return await _db.Clientes.FindAsync(id);
    }

    public async Task<Cliente> CrearAsync(Cliente cliente)
    {
        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();
        return cliente;
    }

    public async Task ActualizarAsync(Cliente cliente)
    {
        _db.Clientes.Update(cliente);
        await _db.SaveChangesAsync();
    }

    public async Task DesactivarAsync(int id)
    {
        var cliente = await _db.Clientes.FindAsync(id);
        if (cliente == null) return;

        cliente.Activo = false;
        await _db.SaveChangesAsync();
    }
}