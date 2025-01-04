using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/checkout")]
[ApiController]
public class CheckoutController : ControllerBase
{
    private readonly CartService _cartService;
    private readonly CheckoutService _orderService;

    public CheckoutController(CartService cartService, CheckoutService orderService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }

    // Checkout Endpoint
    [HttpPost]
    public async Task<IActionResult> Checkout([FromBody] CheckoutPayload checkoutRequest)
    {
        var userId = User.Identity.Name;  // Get the authenticated user ID

        // Validate the cart for the user
        var cart = await _cartService.GetCartAsync(userId);
        if (cart == null || cart.Items.Count == 0)
        {
            return BadRequest("Your cart is empty.");
        }

        // Process the payment (this is a placeholder)
        var paymentSuccess = await ProcessPaymentAsync(checkoutRequest);
        if (!paymentSuccess)
        {
            return BadRequest("Payment failed. Please try again.");
        }

        var orderDestination = new OrderDestination()
        {
            Name = checkoutRequest.Name,
            Address = checkoutRequest.Address,
            City = checkoutRequest.City,
            PostalCode = checkoutRequest.PostalCode,
        };
        
        var order = await _orderService.CreateOrderAsync(userId, cart, orderDestination);

        // Clear the cart after successful checkout
        await _cartService.ClearCartAsync(userId);

        return Ok(new { OrderId = order.Id, Message = "Order completed successfully!" });
    }

    // A placeholder method for payment processing
    private async Task<bool> ProcessPaymentAsync(CheckoutPayload checkoutRequest)
    {
        // Here you would integrate with a payment gateway (Stripe, PayPal, etc.)
        // This is a simplified example where payment is always successful
        return await Task.FromResult(true);
    }
}
