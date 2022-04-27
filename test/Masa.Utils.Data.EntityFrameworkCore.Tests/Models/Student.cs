// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

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
