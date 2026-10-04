using System.ComponentModel.DataAnnotations;
using System;

namespace ProyectoPedido.Models;

public class Pedido
{
    [Key]
    public int PedidoID { get; set; }
    public string? Nombre { get; set; }
    public Estado Estado { get; set; }
    public DateTime Fecha { get; set; }

    public virtual ICollection<DetallePedido>? DetallePedidos { get; set; }
}

public enum Estado
{
    Pendiente,
    Enviado,
    Entregado
}

public class VistaPedido
{
    public int PedidoID { get; set; }
    public string? NombreDelCliente { get; set; }
    public string EstadoPedido { get; set; }

    public string FechaFormateada { get; set; }
}