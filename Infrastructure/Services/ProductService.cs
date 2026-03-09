using System.Net;
using Microsoft.AspNetCore.Http;
using Domain.DTOs;
using Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
public class ProductService(ApplicationDbContext dbContext):IProductService
{
    private readonly ApplicationDbContext context= dbContext;

    public async Task<Response<string>> AddAsync(ProductDto dto)
    {
        var categoryExists = await context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        var brandExists = await context.Brands.AnyAsync(b => b.Id == dto.BrandId);
        if (!categoryExists || !brandExists)
        {
            return new Response<string>(HttpStatusCode.BadRequest,"Category or brand not found");
        }
        if (dto.DiscountPrice > dto.Price)
        {
            return new Response<string>(HttpStatusCode.BadRequest,"Discount price cannot be greater than price");
        }

        var product = new Product
        {
         Name = dto.Name,
         Description = dto.Description,
         Price = dto.Price,
         StockQuantity=dto.StockQuantity,
         CategoryId = dto.CategoryId,
         DiscountPrice = dto.DiscountPrice,
         BrandId = dto.BrandId,
         IsActive = dto.IsActive,
         ImageUrl = dto.ImageUrl
        };
       await context.Products.AddAsync(product);
       await context.SaveChangesAsync();
       return  new Response<string>(HttpStatusCode.OK,"Product added successfully");
    }

   public async Task<Response<string>> DeleteAsync(int productid)
    {
        var product = await  context.Products.FindAsync(productid);
        if(product == null){return new Response<string>(HttpStatusCode.NotFound,"Product not found");}
         context.Products.Remove(product);
          await context.SaveChangesAsync();
          return new Response<string>(HttpStatusCode.OK,"Deleted !");
    }

   public async Task<Response<List<Product>>> FilterAsync(decimal? minPrice, decimal? maxPrice, int? brandId)
    {
            IQueryable<Product> products = context.Products.AsQueryable();
    if (minPrice.HasValue)
    {
        products = products.Where(p => p.Price >= minPrice.Value);
    }
    if (maxPrice.HasValue)
    {
        products = products.Where(p => p.Price <= maxPrice.Value);
    }

    if (brandId.HasValue)
    {
        products = products.Where(p => p.BrandId == brandId.Value);
    }
    var result = await products.ToListAsync();
    return new Response<List<Product>>(HttpStatusCode.OK, "Filtered products", result);

    }

   public async Task<PagedResult<Product>> GetAllAsync(FilterProduct filter, PagedQuery query)
    {
            IQueryable<Product> products = context.Products.Include(p => p.Category).Include(p => p.Brand).AsQueryable();
    if (filter.MinPrice.HasValue)
    {
        products = products.Where(p => p.Price >= filter.MinPrice.Value);
    }
    if (filter.MaxPrice.HasValue)
    {
        products = products.Where(p => p.Price <= filter.MaxPrice.Value);
    }

    if (filter.BrandId.HasValue)
    {
        products = products.Where(p => p.BrandId == filter.BrandId.Value);
    }
    if (filter.CategoryId.HasValue)
    {
        products = products.Where(p => p.CategoryId == filter.CategoryId.Value);
    }
    if (filter.Name!=null)
    {
        products = products.Where(p => p.Name.Contains(filter.Name));
    }

    if (filter.IsActive!=null)
    {
        products = products.Where(p => p.IsActive == filter.IsActive);
    }
    var total = await products.CountAsync();
    var page = query.Page > 0 ? query.Page : 1;
    var pageSize = query.PageSize > 0 ? query.PageSize : 10;
    products =  products.Skip((page-1)*pageSize).Take(pageSize);
    var product = await products.ToListAsync();

    return new PagedResult<Product>
    {
        Items = product,
        Page = page,
        PageSize = pageSize,
        TotalCount = total,
        TotalPages = (int)Math.Ceiling((double)total / pageSize)
    };
    }
   public async Task<Response<List<Product>>> GetByBrandAsync(int brandId)
    {
        var brand = await context.Products.Where(p => p.BrandId == brandId).Include(b=>b.Brand).ToListAsync();
        return new Response<List<Product>>(HttpStatusCode.OK,"ok",brand);   
    }
   public async Task<Response<List<Product>>> GetByCategoryAsync(int categoryid)
{
    var products = await context.Products.Where(p => p.CategoryId == categoryid).ToListAsync();
    return new Response<List<Product>>(HttpStatusCode.OK, "ok", products);
}

    public async Task<Response<Product>> GetByIdAsync(int productid)
    {
        var product = await context.Products.FindAsync(productid);
        if (product == null)
        {
            return new Response<Product>(HttpStatusCode.NotFound,"Product not found");
        }
        return new Response<Product>(HttpStatusCode.OK,"Ok",product);
    }

   public async Task<Response<List<Product>>> GetTopSellingAsync()
    {
       var products = await context.Products.OrderByDescending(p => p.OrderItems.Sum(o => o.Quantity))
        .ToListAsync();
        return new Response<List<Product>>(HttpStatusCode.OK,"Ok",products);
    }

   public async Task<Response<Product>> GetWithReviewsAsync(int id)
    {
       var product = await context.Products.Include(p=>p.Reviews).ThenInclude(r=>r.User).FirstOrDefaultAsync(p=>p.Id==id);
         if (product == null)
    {
        return new Response<Product>(HttpStatusCode.NotFound, "Product not found");
    }
        return new Response<Product>(HttpStatusCode.OK,"ok",product);   
    }
   public async Task<Response<List<Product>>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new Response<List<Product>>(HttpStatusCode.BadRequest,"keyword is required");
        }
        var product = await context.Products.Where(p=>p.Name.Contains(keyword) || p.Description.Contains(keyword)).ToListAsync();
        return new Response<List<Product>>(HttpStatusCode.OK,"ok",product); 
    }

    public async Task<Response<List<Product>>> SortByPriceAsync(bool ascending)
    {
        var product = ascending ? await context.Products.OrderBy(p=>p.Price).ToListAsync()
        : await context.Products.OrderByDescending(p=>p.Price).ToListAsync();
        return new Response<List<Product>>(HttpStatusCode.OK,"ok",product); 

    }

   public async Task<Response<string>> UpdateAsync(int productid, UpdateProductDto dto)
    {
        var p = await context.Products.FindAsync(productid);
        if (p == null)
        {
            return new Response<string>(HttpStatusCode.NotFound,"Product not found");
        }
        if (dto.DiscountPrice > dto.Price)
        {
            return new Response<string>(HttpStatusCode.BadRequest,"Discount price cannot be greater than price");
        }
        p.Name = dto.Name;
        p.Description = dto.Description;
        p.Price = dto.Price;
        p.StockQuantity = dto.StockQuantity;
        p.CategoryId = dto.CategoryId;
        p.DiscountPrice = dto.DiscountPrice;
        p.BrandId = dto.BrandId;
        p.IsActive = dto.IsActive;
        p.ImageUrl = dto.ImageUrl;
        await context.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK,"ok" );
    }

    public async Task<Response<string>> UploadImageAsync(int productId, IFormFile file)
    {
        var product = await context.Products.FindAsync(productId);
        if (product == null)
            return new Response<string>(HttpStatusCode.NotFound, "Product not found");
        var folder = Path.Combine("wwwroot", "images", "products");
        Directory.CreateDirectory(folder);
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(folder, fileName);
        await using (var fs = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fs);
        }
        product.ImageUrl = $"/images/products/{fileName}";
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "Image uploaded", product.ImageUrl);
    }
}
