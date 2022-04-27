namespace Masa.Utils.Data.Mapping.Tests.Requests.Orders;

public class OrderItemRequest
{
    public string Name { get; set; }= default!;

    public decimal Price { get; set; }

    public int Number { get; set; }
}
