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

        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] Producto producto)
        {
            var nombreMayuscula = producto.Nombres.Trim().ToUpper();

            var existeProducto = await _context.Productos.AnyAsync(e => e.Nombres == nombreMayuscula);

            if(!existeProducto){
                var nuevoProducto = new Producto
                {
                    Nombres = nombreMayuscula,
                    Descripcion = producto.Descripcion,
                    Costo = producto.Costo,
                    Venta = producto.Venta,
                    Stock = producto.Stock,

                    CategoriaID = producto.CategoriaID,
                };

                _context.Add(nuevoProducto);
                await _context.SaveChangesAsync();

                return Ok("Producto guardado");
            } 
            else {
                return BadRequest("El producto ya existe");
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

        // [HttpDelete]
        // public async Task<IActionResult> EliminarProducto()
        // {

        // }
    }
}