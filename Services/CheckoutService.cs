using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CheckoutService
{
    private readonly IMongoCollection<Order> _ordersCollection;

    public CheckoutService(IMongoDatabase database)
    {
        _ordersCollection = database.GetCollection<Order>("Orders"); // Specify the "Orders" collection in MongoDB
    }

    // Create Order from Cart
    public async Task<Order> CreateOrderAsync(string userId, Cart cart, OrderDestination orderDestination)
    {
        // Ensure the cart is not empty
        if (cart.Items.Count == 0)
        {
            throw new ArgumentException("Cannot create order with an empty cart.");
        }
        // Create an Order
        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            TotalPrice = cart.TotalPrice,
            Items = new List<OrderItem>(),
            OrderDestination = orderDestination
        };

        // Map Cart items to Order items
        foreach (var item in cart.Items)
        {
            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Product!.Price,
                TotalPrice = item.Quantity * item.Product!.Price
            });
        }

        // Insert the order into MongoDB
        await _ordersCollection.InsertOneAsync(order);

        // Optionally: Perform any additional tasks like sending confirmation email, etc.

        return order;
    }

    // Get Orders by UserId (For History)
    public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await _ordersCollection
            .Find(order => order.UserId == userId)
            .ToListAsync();
    }
}
