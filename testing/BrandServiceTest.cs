using Infrastructure.Services;
using Domain.DTOs;
using Test.HelperDb;

public class BrandServiceTest
{
     [Fact]
     public async Task AddBrandName()
     {
          await using var context = TestAppDbContextFactory.CreateContext(nameof(AddBrandName));
          var brandservice = new BrandService(context);
     
            var brand = new BrandDto
            {
                 Name = "Apple"
            };
            await brandservice.AddAsync(brand);
            var response = await brandservice.GetByIdAsync(1);
            Assert.Equal("ok", response.Description?.FirstOrDefault());
     }
     [Fact]
     public async Task GetAllTest()
     {
         await using var context = TestAppDbContextFactory.CreateContext(nameof(GetAllTest));
          var brandservice = new BrandService(context);
     
            var brand = new BrandDto
            {
                 Name = "Apple"
            };
             await brandservice.AddAsync(brand);
            var response = await brandservice.GetAllAsync();
            Assert.Equal("ok", response.Description?.FirstOrDefault());
     }
     [Fact]
     public async Task DeleteBrandTest()
     {
         await using var context = TestAppDbContextFactory.CreateContext(nameof(DeleteBrandTest));
          var brandservice = new BrandService(context);
     
            var brand = new BrandDto
            {
                 Name = "Apple"
            };
             await brandservice.AddAsync(brand);
             var deletebrand = await brandservice.DeleteAsync(1);
             Assert.Equal("Deleted Brand successfully", deletebrand.Description?.FirstOrDefault());
     }
    [Fact]
    public async Task UpdateBrandTest()
    {
         await using var context = TestAppDbContextFactory.CreateContext(nameof(UpdateBrandTest));
          var brandservice = new BrandService(context);
     
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
             Assert.Equal("ok", response.Description?.FirstOrDefault());
    }
    [Fact]
    public async Task GetByIdBrandTest()
    {
         await using var context = TestAppDbContextFactory.CreateContext(nameof(GetByIdBrandTest));
          var brandservice = new BrandService(context);
     
            var brand = new BrandDto
            {
                 Name = "Apple"
            };
             await brandservice.AddAsync(brand);
             var response = await brandservice.GetByIdAsync(1);
             Assert.Equal("ok", response.Description?.FirstOrDefault());
    }

}