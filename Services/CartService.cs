using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

public class CartService
{
    private readonly IMongoCollection<Cart> _carts;
    private readonly IMongoCollection<Product> _products;

    public CartService(IMongoDatabase database)
    {
        _carts = database.GetCollection<Cart>("Carts");
        _products = database.GetCollection<Product>("Products");
    }

    // Get the cart for a user (if exists) or create a new one
    public async Task<Cart> GetCartAsync(string userId)
    {
        var cart = await _carts.Find(c => c.UserId == userId).FirstOrDefaultAsync();

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await _carts.InsertOneAsync(cart);
        }

        return cart;
    }

    // Add an item to the cart
    public async Task AddItemToCartAsync(string userId, CartItem item)
    {
        var cart = await GetCartAsync(userId);

        // Check if the item already exists in the cart
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

        if (existingItem != null)
        {
            // If item exists, update the quantity
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            ObjectId.TryParse(item.ProductId, out ObjectId objectId);
            var product = await _products.Find(p => p.Id == objectId.ToString()).FirstOrDefaultAsync();
            cart.Items.Add(new CartItem{
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Product = product
            });
        }

        // Update the cart in the database
        await _carts.ReplaceOneAsync(c => c.UserId == userId, cart);
    }

    // Update the quantity of an item in the cart
    public async Task UpdateItemQuantityAsync(string userId, string productId, int quantity)
    {
        var cart = await GetCartAsync(userId);

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            // Ensure quantity is within valid bounds (e.g., 1 to 99)
            item.Quantity = Math.Max(1, Math.Min(99, quantity));
            await _carts.ReplaceOneAsync(c => c.UserId == userId, cart);
        }
    }

    // Remove an item from the cart
    public async Task RemoveItemFromCartAsync(string userId, string productId)
    {
        var cart = await GetCartAsync(userId);

        // Remove the item from the cart
        cart.Items = cart.Items.Where(i => i.ProductId != productId).ToList();

        // Update the cart in the database
        await _carts.ReplaceOneAsync(c => c.UserId == userId, cart);
    }

    internal async Task ClearCartAsync(string userId)
    {
        await _carts.DeleteManyAsync(c => c.UserId == userId);
    }
}
