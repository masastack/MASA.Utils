namespace Masa.Utils.Data.EntityFrameworkCore.Test.Models;

public class Student : ISoftDelete
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public int Age { get; set; }

    public bool IsDeleted { get; private set; } = default!;

    public Address Address { get; set; } = default!;

    public List<Hobby> Hobbies { get; set; } = default!;
}

public class Address
{
    public string City { get; set; } = default!;

    public string Street { get; set; } = default!;
}

public class Hobby
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
}
