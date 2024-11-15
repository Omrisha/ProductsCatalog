using AutoMapper;
using DomainModel;
using DomainModel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogService.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogsController : ControllerBase
{
    private readonly ICatalogService catalogService;
    private readonly ILogger<CatalogsController> _logger;

    public CatalogsController(
        ILogger<CatalogsController> logger,
        ICatalogService catalogService)
    {
        _logger = logger;
        this.catalogService = catalogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<CatalogDto> catalogDtos = (await this.catalogService
            .GetCatalogsAsync())
            .ToList();
        
        return Ok(catalogDtos);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        CatalogDto catalogDto = await this.catalogService
            .GetCatalogByIdAsync(id);
        
        return Ok(catalogDto);
    }
    
    [HttpGet("Product/{productId}")]
    public async Task<IActionResult> GetByProductId(Guid productId)
    {
        CatalogDto catalogDto = await this.catalogService
            .GetCatalogByProductIdAsync(productId);
        
        return Ok(catalogDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCatalog([FromBody] CreateCatalogInput catalog)
    {
        CreateCatalogOutput output = await this.catalogService
            .CreateCatalogAsync(catalog);

        if (output.Errors.Count != 0)
        {
            return BadRequest(output.Errors);
        }
        
        return Ok(output.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCatalog(Guid id, [FromBody] UpdateCatalogInput catalog)
    {
        catalog.Id = id;
        UpdateCatalogOutput output = await this.catalogService
            .UpdateCatalogAsync(catalog);

        if (output.Errors.Count != 0)
        {
            return BadRequest(output.Errors);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCatalog(Guid id)
    {
        DeleteCatalogOutput output = await this.catalogService
            .DeleteCatalogAsync(id);

        if (output.Errors.Count != 0)
        {
            return BadRequest(output.Errors);
        }

        return NoContent();
    }
}