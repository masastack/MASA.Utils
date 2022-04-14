namespace Masa.Utils.Data.EntityFrameworkCore.Test.Models;

public class Student : ISoftDelete
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public int Age { get; set; }

    public bool IsDeleted { get; private set; }

    public Address Address { get; set; }

    public List<Hobby> Hobbies { get; set; }
}

public class Address
{
    public string City { get; set; }

    public string Street { get; set; }
}

public class Hobby
{
    public string Name { get; set; }

    public string Description { get; set; }
}
