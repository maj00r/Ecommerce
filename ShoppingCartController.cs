// using Microsoft.AspNetCore.Mvc;
// using System.Threading.Tasks;

// [Route("api/[controller]")]
// [ApiController]
// public class ShoppingCartController : ControllerBase
// {
//     private readonly ShoppingCartRepository _cartRepository;

//     public ShoppingCartController(ShoppingCartRepository cartRepository)
//     {
//         _cartRepository = cartRepository;
//     }

//     [HttpGet("{userId}")]
//     public async Task<IActionResult> GetCart(string userId)
//     {
//         var x = User.Identity.IsAuthenticated;
//         var cart = await _cartRepository.GetCartByUserIdAsync(userId);
//         if (cart == null)
//         {
//             return NotFound("Cart not found.");
//         }
//         return Ok(cart);
//     }

//     [HttpPost("{userId}/add")]
//     public async Task<IActionResult> AddToCart(string userId, [FromBody] CartItem newItem)
//     {
//         await _cartRepository.AddItemToCartAsync(userId, newItem);
//         return Ok("Item added to cart.");
//     }

//     [HttpPut("{userId}/update/{productId}")]
//     public async Task<IActionResult> UpdateItemQuantity(string userId, string productId, [FromBody] int quantity)
//     {
//         await _cartRepository.UpdateItemQuantityAsync(userId, productId, quantity);
//         return Ok("Cart item quantity updated.");
//     }

//     [HttpDelete("{userId}/remove/{productId}")]
//     public async Task<IActionResult> RemoveItem(string userId, string productId)
//     {
//         await _cartRepository.RemoveItemFromCartAsync(userId, productId);
//         return Ok("Item removed from cart.");
//     }
// }
