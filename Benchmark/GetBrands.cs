using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Infrastructure.Services;

[MemoryDiagnoser]
public class GetBrandsBenchmark
{
    private ApplicationDbContext _context = null!;
    private BrandService _service = null!;

    [Params(100, 1000, 10000)]
    public int BrandCount { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"brands-{BrandCount}")
            .Options;

        _context = new ApplicationDbContext(options);
        _service = new BrandService(_context);

        var brands = Enumerable.Range(1, BrandCount)
            .Select(index => new Brand
            {
                Id = index,
                Name = $"Brand {index}",
                Logo = $"Logo {index}"
            });

        await _context.Brands.AddRangeAsync(brands);
        await _context.SaveChangesAsync();
    }

    [Benchmark]
    public async Task GetBrands()
    {
        _ = await _service.GetAllAsync();
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _context.DisposeAsync();
    }
}
