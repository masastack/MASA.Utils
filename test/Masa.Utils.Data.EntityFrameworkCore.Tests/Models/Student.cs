namespace Masa.Utils.Data.EntityFrameworkCore.Test.Models;

public class Student : ISoftDelete
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public bool IsDeleted { get; private set; }
}
