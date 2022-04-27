namespace Masa.Utils.Data.Mapping.Tests.Requests.Orders;

public class OrderRequest
{
    public string Name { get; set; }= default!;

    public OrderItem OrderItem { get; set; }= default!;
}
