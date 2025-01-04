using MongoDB.Bson;
using MongoDB.Driver;

public class ProductService
{
    private readonly IMongoCollection<Product> _products;
    private readonly IMongoCollection<Category> _categories;

    public ProductService(IMongoDatabase database)
    {
        _products = database.GetCollection<Product>("Products");
        _categories = database.GetCollection<Category>("Categories");
    }
public async Task<(List<Product>, long)> GetProductsAsync(
    string? categoryId,
    decimal? minPrice,
    decimal? maxPrice,
    int page,
    int pageSize)
{
    // Validate input parameters
    if (pageSize <= 0 || pageSize > 100) pageSize = 9; // Default page size
    if (page <= 0) page = 1; // Default to first page

    // Build MongoDB filter
    var filter = Builders<Product>.Filter.Empty;

    if (!string.IsNullOrEmpty(categoryId))
    {
        Console.WriteLine($"Filtering by categoryId: {categoryId}");
        filter &= Builders<Product>.Filter.Eq(p => p.CategoryId,  new ObjectId(categoryId).ToString()); // Convert to ObjectId
    }

    if (minPrice.HasValue)
    {
        Console.WriteLine($"Filtering by minPrice: {minPrice}");
        filter &= Builders<Product>.Filter.Gte(p => p.Price, minPrice);
    }

    if (maxPrice.HasValue)
    {
        Console.WriteLine($"Filtering by maxPrice: {maxPrice}");
        filter &= Builders<Product>.Filter.Lte(p => p.Price, maxPrice);
    }

    Console.WriteLine($"Filter: {filter}");

    // Count total documents matching the filter
    var totalCount = await _products.CountDocumentsAsync(filter);
    Console.WriteLine($"Total products matching filter: {totalCount}");

    if (totalCount == 0)
        return (new List<Product>(), 0);

    // Fetch products with pagination
    var products = await _products.Find(filter)
        .Skip((page - 1) * pageSize)
        .Limit(pageSize)
        .ToListAsync();

    // Fetch and map categories
    var categories = await GetCategoriesAsync();
    if (categories == null || !categories.Any())
    {
        Console.WriteLine("Categories are empty or null.");
        return (products, totalCount);
    }

    foreach (var product in products)
    {
        product.Category = categories.FirstOrDefault(x => x.Id == product.CategoryId);
    }

    return (products, totalCount);
}
   /// <summary>
    /// Creates a new product in the database.
    /// </summary>
    public async Task CreateProductAsync(Product product)
    {
        await _products.InsertOneAsync(product);
    }

    /// <summary>
    /// Updates an existing product in the database.
    /// </summary>
    public async Task UpdateProductAsync(string id, Product updatedProduct)
    {
        await _products.ReplaceOneAsync(p => p.Id == id, updatedProduct);
    }

    /// <summary>
    /// Deletes a product by ID.
    /// </summary>
    public async Task DeleteProductAsync(string id)
    {
        await _products.DeleteOneAsync(p => p.Id == id);
    }

    /// <summary>
    /// Retrieves a single product by ID.
    /// </summary>
    public async Task<Product?> GetProductByIdAsync(string id)
    {
        ObjectId.TryParse(id, out ObjectId objectId);
        var product = await _products.Find(p => p.Id == objectId.ToString()).FirstOrDefaultAsync();
        if (product != null)
        {
            var categories = await GetCategoriesAsync();
            product.Category = categories
                .FirstOrDefault(c => c.Id == product.CategoryId);
        }
        return product;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var filter = Builders<Category>.Filter.Empty;
        return await _categories.Find(filter).ToListAsync();
    } 

    public async Task<List<Product>> GetRelatedProductsAsync(string categoryId, string excludeProductId, int limit = 3)
    {
        ObjectId.TryParse(excludeProductId, out ObjectId objectId);
        var filter = Builders<Product>.Filter.And(
            Builders<Product>.Filter.Eq(p => p.CategoryId, categoryId),
            Builders<Product>.Filter.Ne(p => p.Id, objectId.ToString())
        );

        return await _products.Find(filter).Limit(limit).ToListAsync();
    }
}
