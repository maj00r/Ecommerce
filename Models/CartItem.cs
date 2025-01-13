using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class CartItem
{
    [BsonElement("productId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ProductId { get; set; } = null!;

    [BsonElement("product")]
    public Product? Product { get; set; }
    public int Quantity { get; set; }
}
