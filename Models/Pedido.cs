using System.ComponentModel.DataAnnotations;
using System;

namespace ProyectoPedido.Models;

public class Pedido
{
    [Key]
    public int PedidoID { get; set; }
    public Estado Estado { get; set; }
    public decimal Total { get; set; }
    public DateTime Fecha { get; set; }

    public virtual ICollection<DetallePedido>? DetallePedidos { get; set; }
}

public enum Estado
{
    Pendiente,
    Enviado,
    Entragado
}