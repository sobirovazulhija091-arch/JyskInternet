
using Microsoft.EntityFrameworkCore;
using System.Net;
public class WarehouseService(ApplicationDbContext dbContext):IWarehouseService
{
    private readonly ApplicationDbContext context = dbContext;

    public async Task<Response<string>> CreateAsync(WarehouseDto dto)
    {
        if (dto.Location==null)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Location is required");
        }
        if (dto.Capacity <= 0)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Capacity must be greater than 0");
        }

        var warehouse = new Warehouse
        {
            Location = dto.Location.Trim(),
            Capacity = dto.Capacity
        };

        await context.Warehouses.AddAsync(warehouse);
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "Warehouse created successfully");
    }

    public async Task<Response<string>> UpdateAsync(int id, UpdateWarehouseDto dto)
    {
        var warehouse = await context.Warehouses.FindAsync(id);
        if (warehouse == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Warehouse not found");
        }
        if (dto.Location==null)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Location is required");
        }
        if (dto.Capacity <= 0)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Capacity must be greater than 0");
        }

        warehouse.Location = dto.Location;
        warehouse.Capacity = dto.Capacity;
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "Warehouse updated successfully");
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var warehouse = await context.Warehouses.FindAsync(id);
        if (warehouse == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Warehouse not found");
        }

        var hasProducts = await context.WarehouseProducts.AnyAsync(wp => wp.WarehouseId == id);
        if (hasProducts)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Warehouse has products and cannot be deleted");
        }

        context.Warehouses.Remove(warehouse);
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "Warehouse deleted successfully");
    }

    public async Task<Response<List<Warehouse>>> GetAllAsync()
    {
        var warehouses = await context.Warehouses.ToListAsync();
        return new Response<List<Warehouse>>(HttpStatusCode.OK, "Ok", warehouses);
    }

    public async Task<Response<Warehouse>> GetByIdAsync(int id)
    {
        var warehouse = await context.Warehouses.FindAsync(id);
        if (warehouse == null)
        {
            return new Response<Warehouse>(HttpStatusCode.NotFound, "Warehouse not found");
        }
        return new Response<Warehouse>(HttpStatusCode.OK, "Ok", warehouse);
    }
//stock-складские остатки
  public async Task<Response<string>> AddStockAsync(AddStockDto dto)
{
    if (dto.Quantity <= 0)
    {
        return new Response<string>(HttpStatusCode.BadRequest, "Quantity must be greater than 0");
    }

    var product = await context.Products
        .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

    if (product == null)
    {
        return new Response<string>(HttpStatusCode.NotFound, "Product not found");
    }

    var warehouse = await context.Warehouses
        .FirstOrDefaultAsync(w => w.Id == dto.WarehouseId);

    if (warehouse == null)
    {
        return new Response<string>(HttpStatusCode.NotFound, "Warehouse not found");
    }

    var warehouseProduct = await context.WarehouseProducts
        .FirstOrDefaultAsync(wp =>
            wp.ProductId == dto.ProductId &&
            wp.WarehouseId == dto.WarehouseId);

    if (warehouseProduct == null)
    {
        warehouseProduct = new WarehouseProduct
        {
            ProductId = dto.ProductId,
            WarehouseId = dto.WarehouseId,
            Quantity = dto.Quantity
        };

        await context.WarehouseProducts.AddAsync(warehouseProduct);
    }
    else
    {
        warehouseProduct.Quantity += dto.Quantity;
    }

    await context.SaveChangesAsync();

    return new Response<string>(HttpStatusCode.OK, "Stock added successfully");
}

    public async Task<Response<int>> CheckStockAsync(int productId)
    {
         var product = await context.Products.FirstOrDefaultAsync(o => o.Id == productId);
    if (product == null)
    {
        return new Response<int>(HttpStatusCode.NotFound, "Product not found");
    }

    var totalStock = await context.WarehouseProducts
        .Where(o => o.ProductId == productId)
        .SumAsync(o => o.Quantity);
    return new Response<int>(HttpStatusCode.OK, "Stock retrieved", totalStock);
    }
public async Task<Response<string>> DecreaseStockAsync(DecrStockDto dto)
{
    if (dto.Quantity <= 0)
    {
        return new Response<string>(HttpStatusCode.BadRequest, "Quantity must be greater than 0");
    }

    var warehouseProduct = await context.WarehouseProducts
        .Where(wp => wp.ProductId == dto.ProductId)
        .OrderByDescending(wp => wp.Quantity)
        .FirstOrDefaultAsync();

    if (warehouseProduct == null)
    {
        return new Response<string>(HttpStatusCode.NotFound, "Product not found in warehouses");
    }

    if (warehouseProduct.Quantity < dto.Quantity)
    {
        return new Response<string>(HttpStatusCode.BadRequest, "Not enough stock");
    }

    warehouseProduct.Quantity -= dto.Quantity;

    await context.SaveChangesAsync();

    return new Response<string>(HttpStatusCode.OK, "Stock decreased successfully");
}
}
