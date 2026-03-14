using Infrastructure.Services;
using Domain.DTOs;
using Test.HelperDb;

public class ProductServiceTest
{
   [Fact]
   public async Task AddProductTest()
   {
     await using var context = TestAppDbContextFactory.CreateContext(nameof(AddProductTest));
        var category = new Category { Id = 1, Name = "Test Category" };
        var brand = new Brand { Id = 1, Name = "Test Brand" };
        await context.Categories.AddAsync(category);
        await context.Brands.AddAsync(brand);
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var product = new ProductDto
        {
            Name = "Apple",
            Price = 50,
            Description = "This is test",
            IsActive = false,
            CategoryId = 1,
            BrandId = 1,
            StockQuantity = 10,
            DiscountPrice = 0
        };

        var response = await service.AddAsync(product);
        Assert.Equal("Product added successfully", response.Description?.FirstOrDefault());
   }

   [Fact]
   public async Task GetProductByIdTest()
   {
     await using var context = TestAppDbContextFactory.CreateContext(nameof(GetProductByIdTest));
        var category = new Category { Id = 1, Name = "Test Category" };
        var brand = new Brand { Id = 1, Name = "Test Brand" };
        await context.Categories.AddAsync(category);
        await context.Brands.AddAsync(brand);
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var product1 = new ProductDto
        {
            Name = "Apple",
            Price = 50,
            Description = "This is to get products test",
            IsActive = false,
            CategoryId = 1,
            BrandId = 1,
            StockQuantity = 10,
            DiscountPrice = 0
        };
        await service.AddAsync(product1);

        var response = await service.GetByIdAsync(1);
        Assert.Equal("Apple", response.Data!.Name);
   }

   [Fact]
   public async Task DeleteProductTest()
   {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(DeleteProductTest));
        var category = new Category { Id = 1, Name = "Test Category" };
        var brand = new Brand { Id = 1, Name = "Test Brand" };
        await context.Categories.AddAsync(category);
        await context.Brands.AddAsync(brand);
        await context.SaveChangesAsync();

        var product = new Product
        {
            Name = "Apple",
            Price = 50,
            Description = "This is to delete products test",
            IsActive = true,
            CategoryId = 1,
            BrandId = 1,
            StockQuantity = 10,
            DiscountPrice = 0
        };
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var deleteResponse = await service.DeleteAsync(1);
        Assert.Equal("Deleted !", deleteResponse.Description?.FirstOrDefault());
   }

   [Fact]
   public async Task UpdateProductTest()
   {
       await using var context = TestAppDbContextFactory.CreateContext(nameof(UpdateProductTest));
        var category = new Category { Id = 1, Name = "Test Category" };
        var brand = new Brand { Id = 1, Name = "Test Brand" };
        await context.Categories.AddAsync(category);
        await context.Brands.AddAsync(brand);
        await context.SaveChangesAsync();

       var service = new ProductService(context);
       var product = new ProductDto
       {
            Name = "Apple",
            Price = 50,
            Description = "This is to update products test",
            IsActive = true,
            CategoryId = 1,
            BrandId = 1,
            StockQuantity = 10,
            DiscountPrice = 0
       };
       await service.AddAsync(product);
       var updateProduct = new UpdateProductDto
       {
            Name = "Samsung",
            Price = 60,
            Description = "This is updated product",
            IsActive = true,
            CategoryId = 1,
            BrandId = 1,
            StockQuantity = 10,
            DiscountPrice = 0
       };
       var response = await service.UpdateAsync(1, updateProduct);
       Assert.Equal("ok", response.Description?.FirstOrDefault());
   }
}
