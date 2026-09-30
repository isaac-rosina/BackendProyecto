using ProyectoPedido.Data;
using ProyectoPedido.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProyectoPedido.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PedidoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListadoPedido()
        {
            var pedidos = await _context.Pedidos.Select(p => new VistaPedido
            {
                PedidoID = p.PedidoID,
                NombreDelCliente = p.Nombre,
                FechaFormateada = p.Fecha.ToString("dd/MM/yyyy"),
                EstadoPedido = p.Estado.ToString()

            }).ToListAsync();
            return Ok(pedidos);
        }


        [HttpGet("productos")]
        public IActionResult ObtenerProductos()
        {
                var productos = _context.Productos.ToList();
                return Ok(productos); 
        }


        [HttpPost]
        public async Task<IActionResult> CrearPedidoConDetalles([FromBody] Pedido pedido)
        {
            try
            {
                var nuevoPedido = new Pedido
                {
                    Nombre = pedido.Nombre,
                    Fecha = pedido.Fecha,
                    Estado = pedido.Estado,
                    DetallePedidos = pedido.DetallePedidos.Select(d => new DetallePedido
                    {
                        ProductoID = d.ProductoID,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario
                    }).ToList()
                };

                await _context.Pedidos.AddAsync(nuevoPedido);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(CrearPedidoConDetalles), new { id = nuevoPedido.PedidoID }, new
                {
                    mensaje = "Pedido creado exitosamente",
                    pedidoID = nuevoPedido.PedidoID
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = "Error al guardar: " + ex.Message }) { StatusCode = 500 };
            }
        }
    }
}