namespace Masa.Utils.Data.Mapping.Tests.Requests.Orders;

public class OrderMultiRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public List<OrderItemRequest> OrderItems { get; set; } = default!;
}
