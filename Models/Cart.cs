using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Cart
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } // Unique cart identifier

    public string UserId { get; set; } // The ID of the authenticated user who owns the cart

    public List<CartItem> Items { get; set; } = new List<CartItem>(); // List of items in the cart

    // Computed property to get the total price of the cart
    public decimal TotalPrice => Items.Sum(item => item.Product!.Price * item.Quantity);
}
