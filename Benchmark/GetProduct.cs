using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Infrastructure.Services;
using Domain.DTOs;

[MemoryDiagnoser]
public class GetProductsBenchmark
{
    private ApplicationDbContext _context = null!;
    private ProductService _service = null!;

    [Params(100, 1000, 10000)]
    public int ProductCount { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"products-{ProductCount}")
            .Options;

        _context = new ApplicationDbContext(options);

        var category = new Category { Id = 1, Name = "Test" };
        var brand = new Brand { Id = 1, Name = "Test" };
        await _context.Categories.AddAsync(category);
        await _context.Brands.AddAsync(brand);
        await _context.SaveChangesAsync();

        _service = new ProductService(_context);

        var products = Enumerable.Range(1, ProductCount)
            .Select(index => new Product
            {
                Id = index,
                Name = $"Product {index}",
                Price = index * 10,
                Description = $"Product description{index}",
                CategoryId = 1,
                BrandId = 1,
                StockQuantity = 10,
                DiscountPrice = 0,
                IsActive = true
            });

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();
    }

    [Benchmark]
    public async Task GetProducts()
    {
        var filter = new FilterProduct();
        var query = new PagedQuery { Page = 1, PageSize = ProductCount };
        _ = await _service.GetAllAsync(filter, query);
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _context.DisposeAsync();
    }
}
