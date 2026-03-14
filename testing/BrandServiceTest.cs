using System.Threading.Tasks;

public class BrandServiceTest
{
     [Fact]
     public async Task AddBrandName()
     {
          await using var context = new TestAppDbContextFactory.CreateContext(nameof(AddBrandName));
          var brandservice = new BrandServiceTest(context);
     
            var brand = new BrandDto
            {
                 Name = "Apple"
            };
            await brandservice.AddAsync(brand);
            var response = await brandservice.GetByIdAsync(1);
            Assert.Equal("Add Brand  successfully", response.Message);
     }
     [Fact]
     public async Task GetAllTest()
     {
         await using var context = new TestAppDbContextFactory.CreateContext(nameof(GetAllTest));
          var brandservice = new BrandServiceTest(context);
     
            var brand = new BrandDto
            {
                 Name = "Apple"
            };
             await brandservice.AddAsync(brand);
            var response = await brandservice.GetAllAsync();
            Assert.Equal("ok", response.Message);
     }
     [Fact]
     public async Task DeleteBrandTest()
     {
         await using var context = new TestAppDbContextFactory.CreateContext(nameof(DeleteBrandTest));
          var brandservice = new BrandServiceTest(context);
     
            var brand = new BrandDto
            {
                 Name = "Apple"
            };
             await brandservice.AddAsync(brand);
             var deletebrand = await brandservice.DeleteAsync(1);
             Assert.Equal("Deleted Brand successfully", deletebrand.Message);
     }
    [Fact]
    public async Task UpdateBrandTest()
    {
         await using var context = new TestAppDbContextFactory.CreateContext(nameof(UpdateBrandTest));
          var brandservice = new BrandServiceTest(context);
     
            var brand = new BrandDto
            {
                 Name = "Apple"
            };
             await brandservice.AddAsync(brand);
             var updatebrand = new BrandDto
             {
                  Name = "Samsung"
             };
             var response = await brandservice.UpdateAsync(1, updatebrand);
             Assert.Equal("ok", response.Message);
    }
    [Fact]
    public async Task GetByIdBrandTest()
    {
         await using var context = new TestAppDbContextFactory.CreateContext(nameof(GetByIdBrandTest));
          var brandservice = new BrandServiceTest(context);
     
            var brand = new BrandDto
            {
                 Name = "Apple"
            };
             await brandservice.AddAsync(brand);
             var response = await brandservice.GetByIdAsync(1);
             Assert.Equal("ok", response.Message);
    }

}