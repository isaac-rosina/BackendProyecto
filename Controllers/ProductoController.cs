using ProyectoPedido.Data;
using ProyectoPedido.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProyectoPedido.Controllers
{
    [Route("api/Producto")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListadoProducto()
        {
            var listadoProducto = await _context.Productos.Include(p => p.Categoria).ToListAsync();

            var ProductoMostrar = listadoProducto.Select(p => new vistaProducto
            {
                ProductoID = p.ProductoID,
                NombreProducto = p.Nombres,
                DescripcionProducto = p.Descripcion,
                CostoProducto = p.Costo,
                VentaProducto = p.Venta,
                StockProducto = p.Stock,
                CategoriaID = p.CategoriaID,

                NombreCategoria = p.Categoria.Nombres
            }).ToList();
            return Ok(ProductoMostrar);

        }
  
        [HttpGet("idCategorias")]
        public async Task<IActionResult> ObtenerCategoria()
        {
            var categorias = await _context.Categorias
                .OrderBy(c => c.Nombres)
                .Select(c => new
                {
                    id = c.CategoriaID,
                    nombre = c.Nombres
                })
                .ToListAsync();

            return Ok(categorias);
        }

        [HttpGet("{productoId}")]
        public async Task<IActionResult> ObtenerProducto(int productoId)
        {
            var producto = await _context.Productos
                .Where(p => p.ProductoID == productoId)
                .Select(p => new
                {
                    productoId = p.ProductoID,
                    nombres = p.Nombres,
                    costo = p.Costo,
                    venta = p.Venta,
                    stock = p.Stock,
                    categoriaID = p.CategoriaID,
        
                    nombreCategoria = p.Categoria.Nombres
                })
                .FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }


        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] Producto producto)
        {
            var existeCategoria = await _context.Categorias.AnyAsync(c => c.CategoriaID == producto.CategoriaID);

            if (!existeCategoria)
            {
                return BadRequest($"Error: La categoría con ID {producto.CategoriaID} no existe.");
            }

            var nuevoProducto = new Producto
            {
                Nombres = producto.Nombres,
                Costo = producto.Costo,
                Venta = producto.Venta,
                Stock = producto.Stock,
                CategoriaID = producto.CategoriaID,
                Descripcion = producto.Descripcion,
            };
            {
            _context.Add(nuevoProducto);
            await _context.SaveChangesAsync();

            return Ok("Producto guardado exitosamente");
            }
        }


        [HttpPut("{productoId}")]
        public async Task<IActionResult> EditarProducto(int productoId, [FromBody] Producto producto)
        {
            if (producto.Nombres == null){
                return NotFound("El nombre esta vacío");
            }

            var nombreMayuscula = producto.Nombres.Trim().ToUpper();

            var editarProducto = await _context.Productos.Where(p => p.ProductoID == productoId).FirstOrDefaultAsync();

            if(editarProducto == null) {
                return NotFound("La categoría no existe.");
            }

            var existeNombre = await _context.Productos.Where(p => p.Nombres == nombreMayuscula && p.ProductoID == productoId).AnyAsync();
            if (!existeNombre) {
                editarProducto.Nombres = nombreMayuscula;
                editarProducto.CategoriaID = producto.CategoriaID;
                editarProducto.Costo = producto.Costo;
                editarProducto.Venta = producto.Venta;
                editarProducto.Stock = producto.Stock;
                await _context.SaveChangesAsync();

                return Ok("Producto editado exitosamente.");
            }
            else {
                return NotFound("Ya existe un producto con ese nombre.");
            }
        }

         [HttpDelete("{productoid}")]
        public async Task<IActionResult> Eliminar(int productoid)
        {

            var producto = await _context.Productos.FindAsync(productoid);

            // pedimos que busque  la categoría directamente por su Id
            if (producto == null)
            {
                return NotFound("Producto no encontrado");
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}