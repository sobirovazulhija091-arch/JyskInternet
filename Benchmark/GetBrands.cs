using System.CodeDom.Compiler;

[MemoryDiagnoser]
public class GetBrands
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
               Description = $"Brand description {index}"
           });

       await _context.Brands.AddRangeAsync(brands);
       await _context.SaveChangesAsync();
   }

   [Benchmark]
   public Task<List<BrandDto>> GetBrands()
   {
       return _service.GetBrands();
   }

   [GlobalCleanup]
   public async Task Cleanup()
   {
       await _context.DisposeAsync();
   }
}