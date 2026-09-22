# CLAUDE.md — GestionPedidos

## Propósito del proyecto

Aplicación web de gestión para el obrador de repostería de mi suegro. Uso interno (uno o pocos usuarios). No es un producto comercial ni tiene ambición de serlo. Objetivos:

1. Que él lleve control de clientes, proveedores, pedidos y compras.
2. Que yo aprenda .NET moderno partiendo de un fondo de WebForms + C# 5.

El proyecto también sirve como pieza de portfolio.

## Contexto del desarrollador

Vengo de un stack legacy en el trabajo (C# 5, ASP.NET WebForms, .NET Framework 4.x, SQL Server con ADO.NET/DataSets, Crystal Reports, jQuery). Este es mi primer proyecto real en el stack .NET moderno. Prioridad de aprendizaje sobre velocidad de entrega: **si hay dos formas de hacer algo, prefiero la que enseñe más aunque sea más lenta**.

No usar Claude Code para generar bloques grandes sin que yo entienda cada línea. Explicación primero, código después, y siempre que sea posible dejar que lo escriba yo.

## Stack técnico

- **Lenguaje**: C# 14
- **Framework**: .NET 10 (LTS)
- **Frontend/Backend**: Blazor Web App con render mode `InteractiveServer` global (declarado en `App.razor` sobre `Routes` y `HeadOutlet`)
- **ORM**: Entity Framework Core 10 con migraciones code-first
- **BBDD**: SQLite (fichero `GestionPedidos.db` en la raíz del proyecto, ignorado por Git)
- **UI**: MudBlazor (componentes Material Design)
- **IDE**: Visual Studio 2026 Community
- **Control de versiones**: Git + GitHub (repo público: `alvaro-pj/GestionPedidos`)

## Estructura de carpetas

```
GestionPedidos/
├── Components/
│   ├── Layout/          # MainLayout, NavMenu (providers de MudBlazor aquí)
│   ├── Pages/           # Páginas con @page (ruta)
│   ├── Shared/          # Componentes reutilizables (formularios, diálogos)
│   ├── App.razor        # Raíz, define render mode global
│   └── _Imports.razor   # @using compartidos
├── Data/
│   └── AppDbContext.cs  # DbContext con los DbSet<T>
├── Migrations/          # Migraciones EF Core (versionadas en Git)
├── Models/              # Entidades del dominio
├── Services/            # Lógica de negocio + acceso a datos
├── Properties/
├── wwwroot/
├── Program.cs           # Configuración de servicios y pipeline
└── appsettings.json
```

## Arquitectura y convenciones

- **Separación en capas**: `Página → Service → DbContext → BBDD`. Las páginas nunca hablan directamente con `AppDbContext`.
- **Servicios registrados como `Scoped`** en `Program.cs` (mismo ciclo de vida que el DbContext).
- **Baja lógica siempre, nunca física**. Las entidades tienen `Activo` y `FechaAlta`. Los listados filtran por `Activo = true` por defecto.
- **Validación con Data Annotations** en los modelos (`[Required]`, `[StringLength]`, `[RegularExpression]`, `[EmailAddress]`).
- **Async/await en todo el flujo de peticiones** (`SaveChangesAsync`, `ToListAsync`, etc.). Solo síncrono en el arranque de `Program.cs` (seeding).
- **Formularios compartidos**: alta y edición usan el mismo componente `ClienteFormulario` (evitar duplicación).
- **Confirmación explícita** antes de acciones destructivas mediante `ConfirmarDialogo` (componente reutilizable).
- **Nombres en español** para modelos, propiedades y rutas (`Cliente`, `Nombre`, `/clientes/editar/{id}`). El código de framework en inglés como es habitual.

## Modelo de dominio (planificado)

```
Cliente 1───N Pedido 1───N LineaPedido N───1 Producto
Proveedor 1───N Compra 1───N LineaCompra
```

- `Cliente` — empresas o particulares. Enum `TipoCliente`.
- `Proveedor` — agenda de proveedores.
- `Producto` — con `PrecioBase` orientativo (los precios reales se guardan en cada `LineaPedido` para conservar histórico).
- `Pedido` — cabecera (número albarán, fecha, cliente) + líneas.
- `LineaPedido` — cantidad, precio unitario en el momento de la venta.
- `Compra` — cabecera (fecha, factura, proveedor) + líneas.
- `LineaCompra` — concepto libre (no hay catálogo de productos de proveedor).

Se ha descartado deliberadamente: alérgenos, facturación con IVA, inventario/stock, autenticación (por ahora).

## Estado actual del proyecto (fases)

- ✅ Fase 1: EF Core + SQLite + entidad `Cliente` + seeding + listado.
- ✅ Fase 2: CRUD completo de `Cliente` (servicio, formulario compartido, alta, edición, baja lógica, diálogo de confirmación).
- ⏳ Fase 3: `Proveedor` (mismo patrón).
- ⏳ Fase 4: `Producto` (introducir `decimal` para precios).
- ⏳ Fase 5: `Pedido` + `LineaPedido` (relaciones, formulario con líneas dinámicas).
- ⏳ Fase 6: `Compra` + `LineaCompra`.

## Convenciones de Git

- **Commits pequeños y frecuentes**, al final de cada bloque que funciona.
- **Mensajes en español**, descriptivos, en primera persona ("Añado...", "Refactorizo...", "Corrijo...").
- **Nunca subir**: `.db`, `bin/`, `obj/`, ficheros `.user`, secretos.
- **`GestionPedidos.db` está ignorado** en `.gitignore` (líneas `*.db`, `*.db-shm`, `*.db-wal`).

## Cuándo ejecutar migraciones

Cuando se toque cualquier propiedad de una entidad (añadir, quitar, cambiar tipo, cambiar anotación que afecte al schema):

```bash
cd GestionPedidos
dotnet ef migrations add NombreDescriptivo
dotnet ef database update
```

El `Program.cs` aplica `Database.Migrate()` al arrancar, así que en desarrollo con recargar la app también se aplican.

## Detalles operativos

- La app arranca en `https://localhost:7218` y `http://localhost:5102`.
- **Firefox** no confía en el certificado de desarrollo por defecto (usa su propio almacén). Chrome/Edge sí. Para Firefox, aceptar excepción manualmente.
- **Consola de logs visible al ejecutar**: muy útil para ver el SQL que genera EF Core en cada consulta. No cerrarla con la X (mata el proceso), usar el botón rojo de VS o Ctrl+C.
- **DB Browser for SQLite** instalado para inspeccionar el fichero `.db` directamente.

## Estilo de trabajo con el asistente

- Explicar teoría antes del código cuando se introduce un concepto nuevo.
- Comparar con conceptos que ya conozco cuando ayude: WebForms (base sólida), Angular (algo de experiencia con componentes y signals), Java/JPA (referencia mental para ORM), JavaScript async/await.
- Cerrar cada bloque de trabajo con un **🔵 MOMENTO DE COMMIT** y mensaje sugerido.
- Cuando algo salga mal, diagnosticar antes de parchear. Entender el porqué siempre gana.
- No introducir capas ni patrones "por si acaso" (repositorio genérico, MediatR, DTOs con AutoMapper, etc.) hasta que haya una necesidad clara. YAGNI.