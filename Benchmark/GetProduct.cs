using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

[MemoryDiagnoser]
public class GetProducts
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
        _service = new ProductService(_context);

        var products = Enumerable.Range(1, ProductCount)
            .Select(index => new Product
            {
                Id = index,
                Name = $"Product {index}",
                Price = index * 10,
                Description = $"Product description{index}"
            });

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();
    }

    [Benchmark]
    public Task<List<ProductDto>> GetProducts()
    {
        return _service.GetProducts();
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _context.DisposeAsync();
    }
}