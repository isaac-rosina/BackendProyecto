using System.ComponentModel.DataAnnotations;

namespace ProyectoPedido.Models;

public class DetallePedido
{
    [Key]
    public int DetallePedidoID { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int Cantidad { get; set; }

    public int ProductoID { get; set; }
    public virtual Producto? Productos { get; set; }
    public int PedidoID { get; set; }
    public virtual Pedido? Pedidos { get; set; }

}