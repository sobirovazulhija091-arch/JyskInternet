using System.Net;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
public class BrandService(ApplicationDbContext dbContext):IBrandService
{
    private readonly ApplicationDbContext context = dbContext;

    public async Task<Response<string>> AddAsync(BrandDto dto)
    {
       var  brand = new Brand
       {
           Name = dto.Name,
           Logo = dto.Logo
       };
       await context.Brands.AddAsync(brand);
       await context.SaveChangesAsync();
       return new Response<string>(HttpStatusCode.OK,"Add Brand  successfully");
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var del = await context.Brands.FindAsync(id);
        if (del == null)
        {
            return new Response<string>(HttpStatusCode.NotFound,"Brand not found");
        }
        context.Brands.Remove(del);
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK,"Deleted Brand successfully");
    }

    public async Task<Response<List<Brand>>> GetAllAsync()
    {
       return new Response<List<Brand>>(HttpStatusCode.OK,"ok", await context.Brands.ToListAsync());
    }

    public async Task<Response<Brand>> GetByIdAsync(int id)
    {
        var get = await context.Brands.FindAsync(id);
        if (get == null)
        {
            return new Response<Brand>(HttpStatusCode.NotFound,"Brand not found");
        }
        return new Response<Brand>(HttpStatusCode.OK,"ok",get);
    }

    public async Task<Response<string>> UpdateAsync(int id, BrandDto dto)
    {
        var update = await context.Brands.FindAsync(id);
        if (update == null)
        {
            return new Response<string>(HttpStatusCode.NotFound,"Brand not found");
        }
        update. Name = dto.Name;
        update.Logo = dto.Logo;
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK,"ok" );
    }
}
