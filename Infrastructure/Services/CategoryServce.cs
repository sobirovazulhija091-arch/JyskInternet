using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml;
public class CategoryServce(ApplicationDbContext dbContext):ICategoryService
{
    private readonly ApplicationDbContext context = dbContext;

    public async Task<Response<string>> AddAsync(CategoryDto dto)
    {
       if (dto.ParentId != null)
    {
        var parentExists = await context.Categories
            .AnyAsync(c => c.Id == dto.ParentId);

        if (!parentExists)
            return new Response<string>(
                HttpStatusCode.BadRequest,
                "Parent category not found");
    }

    var category = new Category
    {
        Name = dto.Name,
        ParentId = dto.ParentId
    };

    await context.Categories.AddAsync(category);
    await context.SaveChangesAsync();

    return new Response<string>(HttpStatusCode.OK, "Category created");
    }

    public async Task<Response<string>> DeleteAsync(int categoryid)
    {
        var del = await context.Categories.FindAsync(categoryid);
        if (del == null)
        {
            return new Response<string>(HttpStatusCode.NotFound,"Category not found");
        }
        context.Categories.Remove(del);
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK,"Deleted successfuly!");
    }

    public async Task<Response<List<Category>>> GetAllAsync()
    {
         return new Response<List<Category>>(HttpStatusCode.OK,"ok",await context.Categories.ToListAsync());
    }

    public async Task<Response<Category>> GetByIdAsync(int categoryid)
    {
       var category = await context.Categories.FindAsync(categoryid);
       if (category == null)
       {
        return new Response<Category>(HttpStatusCode.NotFound,"Category not found");
       }
       return new Response<Category>(HttpStatusCode.OK,"Ok",category);
    }

    public async Task<Response<string>> UpdateAsync(int categoryid, UpdateCategoryDto dto)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryid);

    if (category == null)
    {
        return new Response<string>(HttpStatusCode.NotFound,"Category not found");
    }
    category.Name = dto.Name;
    if (dto.ParentId != null)
    {
        if (dto.ParentId == categoryid)
        {
            return new Response<string>(HttpStatusCode.BadRequest,"Category cannot be parent of itself");
        }
        var parentExists = await context.Categories
            .AnyAsync(c => c.Id == dto.ParentId);

        if (!parentExists)
        {
     return new Response<string>(HttpStatusCode.BadRequest,"Parent category not found");
        }
    }
    category.ParentId = dto.ParentId;
    category.ImageUrl = dto.ImageUrl;
 await context.SaveChangesAsync();
    return new Response<string>(HttpStatusCode.OK, "Updated successfully");
    }
    public async     Task<Response<List<Category>>> GetSubCategoriesAsync(int parentId)
    {
       var sub = await context.Categories.Where(p=>p.ParentId==parentId).ToListAsync();
       return new Response<List<Category>>(HttpStatusCode.OK,"ok",sub);
    }

    public async Task<Response<string>> UploadImageAsync(int categoryId, IFormFile file)
    {
        var category = await context.Categories.FindAsync(categoryId);
        if (category == null)
            return new Response<string>(HttpStatusCode.NotFound, "Category not found");
        var folder = Path.Combine("wwwroot", "images", "categories");
        Directory.CreateDirectory(folder);
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(folder, fileName);
        await using (var fs = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fs);
        }
        category.ImageUrl = $"/images/categories/{fileName}";
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "Image uploaded", category.ImageUrl);
    }
}

