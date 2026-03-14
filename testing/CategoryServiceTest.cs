using Infrastructure.Services;
using Domain.DTOs;
using Test.HelperDb;

public class CategoryServiceTest
{
 [Fact]
 public async Task AddCategoryTest()
 {
     await using var context = TestAppDbContextFactory.CreateContext(nameof(AddCategoryTest));
     var service = new CategoryService(context);
     var category = new CategoryDto
     {
          Name = "Phone"
     };
     var response = await service.AddAsync(category);
     Assert.Equal("Category created", response.Description?.FirstOrDefault());} 
}