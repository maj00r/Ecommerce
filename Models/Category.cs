
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; } // MongoDB ObjectId

    [BsonElement("name")]
    public required string Name { get; set; }
}