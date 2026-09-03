using ProyectoPedido.Data;
using ProyectoPedido.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProyectoPedido.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListadoCategoria()
        {
            var categorias = await _context.Categorias.ToListAsync();

            return Ok(categorias);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCategoria([FromBody] Categoria categoria )
        {
            var nombreMayuscula = categoria.Nombres?.Trim().ToUpper();

            var existeCategoria = await _context.Categorias.AnyAsync(e => e.Nombres == nombreMayuscula);

            if(!existeCategoria) {
                var nuevaCategoria = new Categoria
                {
                    Nombres = nombreMayuscula,
                };
                _context.Add(nuevaCategoria);
                await _context.SaveChangesAsync();
                return Ok("Categoria guardada");
            }

            return Ok();
        }

        [HttpPut("{categoriaID}")]
        public async Task<IActionResult> EditarCategoria(int categoriaID, [FromBody] Categoria categoria)
        {
            var nombreMayuscula = categoria.Nombres?.Trim().ToUpper();

            var editarCategoria = await _context.Categorias.Where(e => e.CategoriaID == categoriaID).SingleOrDefaultAsync();

            if(editarCategoria == null){
                return Ok("La categoria que quiere editar no existe");
            };

            var existeNombre = await _context.Categorias.AnyAsync(e => e.Nombres == nombreMayuscula && e.CategoriaID != categoriaID);

            if(!existeNombre) {
                editarCategoria.Nombres = nombreMayuscula;
                await _context.SaveChangesAsync();

                return Ok("Categoria editada exitosamente");
            } 
            else {
                return Ok("Ya existe otra categoria con ese nombre");
            }            
        }

        [HttpDelete("{categoriaID}")]
        public async Task<IActionResult> Eliminar(int categoriaID)
        {
            var categoria = await _context.Categorias.FindAsync(categoriaID);

            if(categoria == null) {
                return NotFound("Categoria no encontrada");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}