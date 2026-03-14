using System.Threading.Tasks;
using Test.HelperDb;
using Xunit;
using Infrastructure.Services;
using Domain.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
public class ProductServiceTest
{
   [Fact]
   public async Task AddProductTest()
   {
     await using var context = TestAppDbContextFactory.CreateContext(nameof(AddProductTest));
        
        var service = new ProductService(context);
        var product = new  ProductDto
        {
            Name = "Apple",
            Price = 50,
            Description = "This is test",
            IsActive = false
        };

        var response = await service.AddAsync(product);
        Assert.Equal("Product added successfully", response.Message);
   }
   [Fact]
   public async Task GetProductByIdTest()
   {
     await using var context = TestAppDbContextFactory.CreateContext(nameof(GetProductByIdTest));
        
        var service = new ProductService(context);
        var product1 = new  ProductDto
        {
            Name = "Apple",
            Price = 50,
            Description = "This is to get products test",
            IsActive = false
        };
        await service.AddAsync(product1);

        var response = await service.GetByIdAsync(1);
        Assert.Equal("Ok", response.Data.Name);
   }
   [Fact]
   public async Task DeleteProductTest()
    {
        await using var context = TestAppDbcontextFactory.CreateContext(nameof(DeleteProductTest));
        var service = new ProductServiceTest(context);
       var product = new Porduct
       {
        Name = "Apple",
        Price = 50,
        Description = "This is to delete products test",
        IsActive = True
       };
       await context.Products.AddAsync(product);
       await ContextStack.SaveChangesAsync();
       var deleteproduct = await context.Products.Delete(1);
        Assert.Equl("Deleted !", deleteproduct.Message);

    }
  [Fact]
  public async Task UpdateProductTest()
  {
       await using var context = TestAppDbContextFactory.CreateContext(nameof(UpdateProductTest));
       var service = new ProductService(context);
       var product = new ProductDto
       {
            Name = "Apple",
            Price = 50,
            Description = "This is to update products test",
            IsActive = true
       };
       await service.AddAsync(product);
       var updateProduct = new ProductDto
       {
            Name = "Samsung",
            Price = 60,
            Description = "This is updated product",
            IsActive = true
       };
       var response = await service.UpdateAsync(1, updateProduct);
       Assert.Equal("Product updated successfully", response.Message);
  }
   
}