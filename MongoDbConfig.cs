public class MongoDbConfig
{
    public required string Name { get; init; }
    public required string Host { get; init; }
    public int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    
    public string ConnectionString => 
        $"mongodb://{Username}:{Password}@{Host}:{Port}/{Name}";
}