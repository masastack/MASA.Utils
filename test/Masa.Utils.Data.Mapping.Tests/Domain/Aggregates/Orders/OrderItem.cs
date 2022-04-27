﻿namespace Masa.Utils.Data.Mapping.Tests.Domain.Aggregates.Orders;

public class OrderItem
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    public int Number { get; set; }

    public OrderItem(string name, decimal price) : this(name, price, 1)
    {

    }

    public OrderItem(string name, decimal price, int number)
    {
        Name = name;
        Price = price;
        Number = number;
    }
}
