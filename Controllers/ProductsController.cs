// Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] string? categoryId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 9)
    {
        var (products, totalCount) = await _productService.GetProductsAsync(
            categoryId, minPrice, maxPrice, page, pageSize);

        return Ok(new
        {
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Products = products
        });
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _productService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id}/related")]
    public async Task<IActionResult> GetRelatedProducts(string id)
    {
        // Fetch the base product to identify its category
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound(new { message = "Product not found." });
        }

        // Fetch related products from the same category, excluding the current product
        var relatedProducts = await _productService.GetRelatedProductsAsync(product.CategoryId, id);

        return Ok(relatedProducts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound(new { message = "Product not found." });
        }

        return Ok(product);
    }
}
