using System.ComponentModel.DataAnnotations;

namespace ProyectoPedido.Models;

public class Categoria
{
    [Key]
    public int CategoriaID { get; set; }
    public string? Nombres { get; set; }

    public virtual ICollection<Producto>? Productos { get; set; }
}

