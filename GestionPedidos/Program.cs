using GestionPedidos.Components;
using GestionPedidos.Data;
using GestionPedidos.Models;
using GestionPedidos.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;      // 4 segundos
    config.SnackbarConfiguration.HideTransitionDuration = 500;     // suave al desaparecer
    config.SnackbarConfiguration.ShowTransitionDuration = 500;     // suave al aparecer
    config.SnackbarConfiguration.ShowCloseIcon = true;             // X para cerrarlo manualmente
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ClienteService>();

var app = builder.Build();

// Aplicar migraciones y datos de prueba al arrancar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Clientes.Any())
    {
        db.Clientes.AddRange(
            new Cliente
            {
                Nombre = "OSLO",
                Tipo = TipoCliente.Empresa,
                Cif = "B12345678",
                Telefono = "947123456",
                Email = "info@oslo.com",
                Direccion = "Calle Mayor 10",
                Poblacion = "Burgos"
            },
            new Cliente
            {
                Nombre = "LA VARGA",
                Tipo = TipoCliente.Empresa,
                Cif = "B87654321",
                Telefono = "947654321",
                Direccion = "Avenida del Cid 25",
                Poblacion = "Burgos"
            },
            new Cliente
            {
                Nombre = "María Fernández",
                Tipo = TipoCliente.Particular,
                Cif = "12345678A",
                Telefono = "666111222",
                Email = "maria@ejemplo.com",
                Poblacion = "Burgos"
            }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
