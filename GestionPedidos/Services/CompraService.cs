using GestionPedidos.Data;
using GestionPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionPedidos.Services;

public class CompraService
{
    private readonly AppDbContext _db;

    public CompraService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Compra>> ObtenerActivosAsync()
    {
        return await _db.Compras
            .Include(c => c.Proveedor)
            .Include(c => c.Lineas)
            .Where(c => c.Activo)
            .OrderByDescending(c => c.Fecha)
            .ToListAsync();
    }

    public async Task<Compra?> ObtenerPorIdAsync(int id)
    {
        return await _db.Compras
            .Include(c => c.Proveedor)
            .Include(c => c.Lineas)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Compra> CrearAsync(Compra compra)
    {
        _db.Compras.Add(compra);
        await _db.SaveChangesAsync();
        return compra;
    }

    public async Task ActualizarAsync(Compra compra)
    {
        var existente = await _db.Compras
            .Include(c => c.Lineas)
            .FirstAsync(c => c.Id == compra.Id);

        existente.NumeroFactura = compra.NumeroFactura;
        existente.Fecha = compra.Fecha;
        existente.ProveedorId = compra.ProveedorId;
        existente.Observaciones = compra.Observaciones;

        var idsFormulario = compra.Lineas.Select(l => l.Id).ToHashSet();
        existente.Lineas.RemoveAll(l => !idsFormulario.Contains(l.Id));

        foreach (var lineaFormulario in compra.Lineas)
        {
            var lineaExistente = existente.Lineas.FirstOrDefault(l => l.Id == lineaFormulario.Id && l.Id != 0);
            if (lineaExistente != null)
            {
                lineaExistente.Concepto = lineaFormulario.Concepto;
                lineaExistente.Cantidad = lineaFormulario.Cantidad;
                lineaExistente.PrecioUnitario = lineaFormulario.PrecioUnitario;
            }
            else
            {
                existente.Lineas.Add(new LineaCompra
                {
                    Concepto = lineaFormulario.Concepto,
                    Cantidad = lineaFormulario.Cantidad,
                    PrecioUnitario = lineaFormulario.PrecioUnitario
                });
            }
        }

        await _db.SaveChangesAsync();
    }

    public async Task DesactivarAsync(int id)
    {
        var compra = await _db.Compras.FindAsync(id);
        if (compra == null) return;

        compra.Activo = false;
        await _db.SaveChangesAsync();
    }
}
