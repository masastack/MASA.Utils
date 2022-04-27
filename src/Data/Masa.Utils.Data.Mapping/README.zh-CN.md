中 | [EN](README.md)

## Masa.Utils.Data.Mapping

Masa.Utils.Data.Mapping是基于[Mapster](https://github.com/MapsterMapper/Mapster)的一个对象到对象的映射器，在原来的基础上增加自动获取并使用最佳构造函数映射，支持嵌套映射，减轻映射的工作量。

## 用例:

1. 安装`Masa.Utils.Data.Mapping`

    ```c#
    Install-Package Masa.Utils.Data.Mapping
    ```

2. 使用`Mapping`

    ``` C#
    builder.Services.AddMapping();
    ```

3. 映射对象

    ```
    IMapping mapper;// 通过DI获取

    var request = new
    {
        Name = "Teach you to learn Dapr ……",
        OrderItem = new OrderItem("Teach you to learn Dapr hand by hand", 49.9m)
    };
    var order = mapper.Map<Order>(request);// 将request映射到新的对象
    ```

    映射类`Order`:

    ``` Order.cs
    public class Order
    {
        public string Name { get; set; }

        public decimal TotalPrice { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public Order(string name)
        {
            Name = name;
        }

        public Order(string name, OrderItem orderItem) : this(name)
        {
            OrderItems = new List<OrderItem> { orderItem };
            TotalPrice = OrderItems.Sum(item => item.Price * item.Number);
        }
    }

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
    ```