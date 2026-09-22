using GestionPedidos.Data;
using GestionPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionPedidos.Services;

public class PedidoService
{
    private readonly AppDbContext _db;

    public PedidoService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Pedido>> ObtenerActivosAsync()
    {
        return await _db.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Lineas)
            .Where(p => p.Activo)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();
    }

    public async Task<Pedido?> ObtenerPorIdAsync(int id)
    {
        return await _db.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Lineas)
                .ThenInclude(l => l.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pedido> CrearAsync(Pedido pedido)
    {
        _db.Pedidos.Add(pedido);
        await _db.SaveChangesAsync();
        return pedido;
    }

    public async Task ActualizarAsync(Pedido pedido)
    {
        var existente = await _db.Pedidos
            .Include(p => p.Lineas)
            .FirstAsync(p => p.Id == pedido.Id);

        // Copiamos los campos de cabecera a mano
        existente.NumeroAlbaran = pedido.NumeroAlbaran;
        existente.Fecha = pedido.Fecha;
        existente.ClienteId = pedido.ClienteId;
        existente.Observaciones = pedido.Observaciones;

        // Quitamos de la colección las líneas que ya no están en el formulario.
        // Al eliminarlas de existente.Lineas (una colección ya cargada por EF Core),
        // EF Core las detecta como "huérfanas" y genera un DELETE al hacer SaveChanges.
        var idsFormulario = pedido.Lineas.Select(l => l.Id).ToHashSet();
        existente.Lineas.RemoveAll(l => !idsFormulario.Contains(l.Id));

        foreach (var lineaFormulario in pedido.Lineas)
        {
            var lineaExistente = existente.Lineas.FirstOrDefault(l => l.Id == lineaFormulario.Id && l.Id != 0);
            if (lineaExistente != null)
            {
                // Línea que ya existía: actualizamos sus valores
                lineaExistente.ProductoId = lineaFormulario.ProductoId;
                lineaExistente.Cantidad = lineaFormulario.Cantidad;
                lineaExistente.PrecioUnitario = lineaFormulario.PrecioUnitario;
            }
            else
            {
                // Línea nueva (Id == 0, todavía no existe en la BBDD)
                existente.Lineas.Add(new LineaPedido
                {
                    ProductoId = lineaFormulario.ProductoId,
                    Cantidad = lineaFormulario.Cantidad,
                    PrecioUnitario = lineaFormulario.PrecioUnitario
                });
            }
        }

        await _db.SaveChangesAsync();
    }

    public async Task DesactivarAsync(int id)
    {
        var pedido = await _db.Pedidos.FindAsync(id);
        if (pedido == null) return;

        pedido.Activo = false;
        await _db.SaveChangesAsync();
    }
}
