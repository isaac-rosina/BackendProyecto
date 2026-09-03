using System.ComponentModel.DataAnnotations;

namespace ProyectoPedido.Models;

public class Producto
{
    [Key]
    public int ProductoID { get; set; }
    public string? Nombres { get; set; }
    public string? Descripcion { get; set; }
    public decimal Costo { get; set; }
    public decimal Venta { get; set; }
    public int Stock { get; set; }

    public int CategoriaID { get; set; }
    public virtual Categoria? Categoria { get; set; }

    public virtual ICollection<DetallePedido>? DetallePedidos { get; set; }

}

public class vistaProducto
{
    public int ProductoID { get; set; }
    public string? NombreProducto { get; set; }
    public string? DescripcionProducto { get; set; }
    public decimal CostoProducto { get; set; }
    public decimal VentaProducto { get; set; }
    public int StockProducto { get; set; }

    public int CategoriaID { get; set; }
    public string? NombreCategoria { get; set; }
}