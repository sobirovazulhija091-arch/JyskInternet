using System.Net;
using Microsoft.EntityFrameworkCore;

public class StoreService(ApplicationDbContext dbContext) : IStoreService
{
    private readonly ApplicationDbContext context = dbContext;

    public async Task<Response<string>> AddAsync(StoreDto dto)
    {
        var store = new Store
        {
            Name = dto.Name,
            Address = dto.Address,
            City = dto.City,
            Phone = dto.Phone,
            WorkingHours = dto.WorkingHours
        };
        await context.Stores.AddAsync(store);
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "Store added successfully");
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var del = await context.Stores.FindAsync(id);
        if (del == null)
            return new Response<string>(HttpStatusCode.NotFound, "Store not found");

        context.Stores.Remove(del);
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "Deleted store successfully");
    }

    public async Task<Response<List<Store>>> GetAllAsync()
    {
        var stores = await context.Stores.ToListAsync();
        return new Response<List<Store>>(HttpStatusCode.OK, "ok", stores);
    }

    public async Task<Response<Store>> GetByIdAsync(int id)
    {
        var store = await context.Stores.FindAsync(id);
        if (store == null)
            return new Response<Store>(HttpStatusCode.NotFound, "Store not found");
        return new Response<Store>(HttpStatusCode.OK, "ok", store);
    }

    public async Task<Response<string>> UpdateAsync(int id, UpdateStoreDto dto)
    {
        var store = await context.Stores.FindAsync(id);
        if (store == null)
            return new Response<string>(HttpStatusCode.NotFound, "Store not found");

        store.Name = dto.Name;
        store.Address = dto.Address;
        store.City = dto.City;
        store.Phone = dto.Phone;
        store.WorkingHours = dto.WorkingHours;
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "ok");
    }
}