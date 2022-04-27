namespace Masa.Utils.Data.Mapping.Tests.Requests.Users;

public class CreateFullUserRequest : CreateUserRequest
{
    public int Age { get; set; }

    public string Description { get; set; } = default!;

    public DateTime Birthday { get; set; }

    public AddressItemRequest Hometown { get; set; } = default!;
}
