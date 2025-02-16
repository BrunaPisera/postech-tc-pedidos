namespace Pedidos.UseCases.DTOs
{
    public class PedidoDto
    {
        public Guid IdPedido { get; set; } 
        public string? CpfCliente { get; set; }
        public string? NomeCliente { get; set; }
        public short CodigoAcompanhamento { get; set; }
        public double ValorTotal { get; set; }
        public List<ItemPedidoDto> Itens { get; set; } = new List<ItemPedidoDto>();
        public DateTime HorarioRecebimento { get; set; }
    }
}
