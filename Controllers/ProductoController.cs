using ProyectoPedido.Data;
using ProyectoPedido.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProyectoPedido.Controllers
{
    [Route("api/[controller]")]
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
    }
}