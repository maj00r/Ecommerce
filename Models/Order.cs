using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

public class Order
{
    [BsonId] // The MongoDB ID field
    public ObjectId Id { get; set; }

    public string UserId { get; set; } // User who placed the order
    public DateTime OrderDate { get; set; } // Date the order was placed
    public decimal TotalPrice { get; set; } // Total price of the order

    public OrderDestination OrderDestination { get; set;}

    public List<OrderItem> Items { get; set; } // List of order items
}

public class OrderItem
{
    [BsonId] // The MongoDB ID field
    public ObjectId Id { get; set; }

    public string ProductId { get; set; }  // Product identifier
    public int Quantity { get; set; }  // Quantity of the product
    public decimal Price { get; set; }  // Price per unit
    public decimal TotalPrice { get; set; }  // Total price for this item (Quantity * Price)
}
