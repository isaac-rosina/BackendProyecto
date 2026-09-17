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

  
        [HttpGet("idCategorias")]
        public async Task<IActionResult> ObtenerCategoria()
        {
            var categorias = await _context.Categorias
                .OrderBy(c => c.Nombres) // le decimos que la ordene por nombre
                .Select(c => new
                {
                    id = c.CategoriaID,
                    nombre = c.Nombres
                })
                .ToListAsync();

            return Ok(categorias);
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