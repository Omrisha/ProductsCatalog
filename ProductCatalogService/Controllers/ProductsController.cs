using AutoMapper;
using DomainModel;
using DomainModel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        ILogger<ProductsController> logger,
        IProductService productService)
    {
        _logger = logger;
        this.productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<ProductDto> productDtos = (await this.productService
            .GetProductsAsync())
            .ToList();
        
        return this.Ok(productDtos);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        ProductDto productDto = await this.productService
            .GetProductByIdAsync(id);
        
        return this.Ok(productDto);
    }
    
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(CategoryEnum category)
    {
        List<ProductDto> productDtos = (await this.productService
            .GetProductsByCategoryAsync(category))
            .ToList();
        
        return this.Ok(productDtos);
    }
    
    [HttpGet("price/{priceLimit}")]
    public async Task<IActionResult> GetByPriceLimit(decimal priceLimit)
    {
        List<ProductDto> productDtos = (await this.productService
            .GetProductsByPriceLimitAsync(priceLimit))
            .ToList();
        
        return this.Ok(productDtos);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateNewProductInput product)
    {
        CreateNewProductOutput output = await this.productService
            .CreateProductAsync(product);

        if (output.Errors.Count != 0)
        {
            return this.BadRequest(output.Errors);
        }
        
        return this.Ok(output.ProductId);
    }
}