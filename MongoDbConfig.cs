public class MongoDbConfig
{
    public required string Name { get; init; }
    public required string Host { get; init; }
    public int Port { get; init; }
    public string ConnectionString => $"mongodb://{Host}:{Port}";
}