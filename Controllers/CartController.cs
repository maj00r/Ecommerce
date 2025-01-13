using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    // Get cart for the authenticated user
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = User.Identity.Name;  // Get the authenticated user ID
        var cart = await _cartService.GetCartAsync(userId);

        if (cart == null)
        {
            return NotFound("Cart not found.");
        }

        return Ok(cart);
    }

    // Add an item to the cart
    [HttpPost("add")]
    public async Task<IActionResult> AddItemToCart([FromBody] CartItem item)
    {
        var userId = User.Identity.Name;  // Get the authenticated user ID

        if (item == null)
        {
            return BadRequest("Item is null.");
        }

        await _cartService.AddItemToCartAsync(userId, item);
        return Ok("Item added to cart.");
    }

    // Update the quantity of an item in the cart
    [HttpPut("update/{productId}")]
    public async Task<IActionResult> UpdateItemQuantity(string productId, [FromQuery] int quantity)
    {
        var userId = User.Identity.Name;  // Get the authenticated user ID

        if (quantity <= 0)
        {
            return BadRequest("Quantity must be greater than zero.");
        }

        await _cartService.UpdateItemQuantityAsync(userId, productId, quantity);
        return Ok("Item quantity updated.");
    }

    // Remove an item from the cart
    [HttpDelete("remove/{productId}")]
    public async Task<IActionResult> RemoveItemFromCart(string productId)
    {
        var userId = User.Identity.Name;  // Get the authenticated user ID

        await _cartService.RemoveItemFromCartAsync(userId, productId);
        return Ok("Item removed from cart.");
    }
}
