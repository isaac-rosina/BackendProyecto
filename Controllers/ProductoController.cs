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

            var productoMostrar = listadoProducto.Select(p => new vistaProducto
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
            return Ok(productoMostrar);
        }

        [HttpGet("{idCategorias}")]
        public async Task<IActionResult> TraerCategoria(int idCategorias)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.CategoriaID == idCategorias);

            if (categoria == null) {
                return NotFound("La categoria no encontrada.");
            }
            else {
                return Ok(categoria);
            }
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

            _context.Add(nuevoProducto);
            await _context.SaveChangesAsync();

            return Ok("Producto guardado exitosamente");
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

        // [HttpDelete]
        // public async Task<IActionResult> EliminarProducto()
        // {

        // }
    }
}