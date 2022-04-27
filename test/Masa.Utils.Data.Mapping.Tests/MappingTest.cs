﻿namespace Masa.Utils.Data.Mapping.Tests;

[TestClass]
public class MappingTest : BaseMappingTest
{
    [TestMethod]
    public void TestCreateUserRequestMapToUserReturnUserNameEqualRequestName()
    {
        var request = new CreateUserRequest()
        {
            Name = "Jim",
        };
        var user = _mapper.Map<CreateUserRequest, User>(request);
        Assert.IsNotNull(user);
        Assert.IsTrue(user.Name == request.Name);
    }

    [TestMethod]
    public void TestObjectMapToUserReturnUserIsNotNull()
    {
        var request = new
        {
            Name = "Jim",
            Age = 18,
            Birthday = DateTime.Now,
            Description = "i am jim"
        };

        var user = _mapper.Map<User>(request);
        Assert.IsNotNull(user);
        Assert.AreEqual(request.Name, user.Name);
        Assert.AreEqual(request.Age, user.Age);
        Assert.AreEqual(request.Birthday, user.Birthday);
        Assert.AreEqual(request.Description, user.Description);
    }

    [TestMethod]
    public void TestObjectMapToUserAndSourceParameterGreatherThanDestinationControllerParameterLength()
    {
        var request = new
        {
            Name = "Jim",
            Age = 18,
            Birthday = DateTime.Now,
            Description = "i am jim",
            Tag = Array.Empty<string>()
        };

        var user = _mapper.Map<object, User>(request);
        Assert.IsNotNull(user);
        Assert.AreEqual(request.Name, user.Name);
        Assert.AreEqual(request.Age, user.Age);
        Assert.AreEqual(request.Birthday, user.Birthday);
        Assert.AreEqual(request.Description, user.Description);
    }

    [TestMethod]
    public void TestCreateFullUserRequestMapToUserReturnHometownIsNotNull()
    {
        var request = new CreateFullUserRequest()
        {
            Name = "Jim",
            Age = 18,
            Birthday = DateTime.Now,
            Hometown = new AddressItemRequest()
            {
                Province = "BeiJing",
                City = "BeiJing",
                Address = "National Sport Stadium"
            }
        };

        var user = _mapper.Map<CreateFullUserRequest, User>(request);
        Assert.IsNotNull(user);
        Assert.AreEqual(request.Name, user.Name);
        Assert.AreEqual(request.Age, user.Age);
        Assert.AreEqual(request.Birthday, user.Birthday);
        Assert.AreEqual(request.Description, user.Description);
        Assert.IsNotNull(request.Hometown);
        Assert.AreEqual(request.Hometown.Province, user.Hometown.Province);
        Assert.AreEqual(request.Hometown.City, user.Hometown.City);
        Assert.AreEqual(request.Hometown.Address, user.Hometown.Address);
    }

    [TestMethod]
    public void TestOrderRequestMapToOrderReturnTotalPriceIs10()
    {
        var request = new OrderRequest()
        {
            Name = "orderName",
            OrderItem = new OrderItem("apple", 10)
        };

        var order = _mapper.Map<OrderRequest, Order>(request);
        Assert.IsNotNull(order);
        Assert.AreEqual(order.Name, request.Name);
        Assert.AreEqual(order.OrderItems.Count, 1);
        Assert.AreEqual(order.OrderItems[0].Name, request.OrderItem.Name);
        Assert.AreEqual(order.OrderItems[0].Price, request.OrderItem.Price);
        Assert.AreEqual(order.OrderItems[0].Number, 1);
        Assert.AreEqual(order.TotalPrice, 1 * 10);
    }

    [TestMethod]
    public void TestOrderMultiRequestMapToOrderReturnOrderItemsCountIs1AndTotalPriceIs10()
    {
        var request = new
        {
            Name = "Order Name",
            OrderItems = new List<OrderItem>()
            {
                new("Apple", 10)
            }
        };

        var order = _mapper.Map<Order>(request);
        Assert.IsNotNull(order);
        Assert.AreEqual(order.Name, request.Name);
        Assert.AreEqual(order.OrderItems.Count, 1);
        Assert.AreEqual(order.OrderItems[0].Name, request.OrderItems[0].Name);
        Assert.AreEqual(order.OrderItems[0].Price, request.OrderItems[0].Price);
        Assert.AreEqual(order.OrderItems[0].Number, 1);
        Assert.AreEqual(order.TotalPrice, 10);
    }

    [TestMethod]
    public void TestOrderMultiRequestMapToOrderReturnOrderItemsCountIs1()
    {
        var request = new OrderMultiRequest()
        {
            Name = "Order Name",
            OrderItems = new List<OrderItemRequest>()
            {
                new()
                {
                    Name = "Apple",
                    Price = 10,
                    Number = 1
                }
            }
        };

        var order = _mapper.Map<OrderMultiRequest, Order>(request);
        Assert.IsNotNull(order);
        Assert.AreEqual(order.Name, request.Name);
        Assert.AreEqual(order.OrderItems.Count, 1);
        Assert.AreEqual(order.OrderItems[0].Name, request.OrderItems[0].Name);
        Assert.AreEqual(order.OrderItems[0].Price, request.OrderItems[0].Price);
        Assert.AreEqual(order.OrderItems[0].Number, 1);
        Assert.AreEqual(order.TotalPrice, 0);
    }
}
