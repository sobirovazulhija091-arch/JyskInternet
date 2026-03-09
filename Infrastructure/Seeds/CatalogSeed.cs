using Microsoft.EntityFrameworkCore;

public static class CatalogSeed
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (await db.Categories.AnyAsync())
            return;

        var brands = await SeedBrandsAsync(db);
        var categories = await SeedCategoriesAsync(db);
        await SeedProductsAsync(db, brands, categories);
    }

    private static async Task<Dictionary<string, int>> SeedBrandsAsync(ApplicationDbContext db)
    {
        var map = new Dictionary<string, int>();
        var names = new[] { "JYSK", "Nordic Home", "SmartTech", "EcoLight" };
        foreach (var name in names)
        {
            var b = new Brand { Name = name };
            db.Brands.Add(b);
            await db.SaveChangesAsync();
            map[name] = b.Id;
        }
        return map;
    }

    private static async Task<Dictionary<string, int>> SeedCategoriesAsync(ApplicationDbContext db)
    {
        var map = new Dictionary<string, int>();
        var names = new[]
        {
            "Beds & Mattresses",
            "Sofas & Armchairs",
            "Tables & Chairs",
            "Storage",
            "Lighting",
            "Textiles",
            "Electronics"
        };
        foreach (var name in names)
        {
            var c = new Category { Name = name };
            db.Categories.Add(c);
            await db.SaveChangesAsync();
            map[name] = c.Id;
        }
        return map;
    }

    private static async Task SeedProductsAsync(ApplicationDbContext db, Dictionary<string, int> brands, Dictionary<string, int> categories)
    {
        var products = new List<Product>
        {
            // Beds & Mattresses
            new() { Name = "Nordic Double Bed", Description = "Scandinavian-style double bed frame.", Price = 599, DiscountPrice = 0, StockQuantity = 10, BrandId = brands["JYSK"], CategoryId = categories["Beds & Mattresses"], IsActive = true },
            new() { Name = "Memory Foam Mattress", Description = "Comfortable memory foam mattress.", Price = 299, DiscountPrice = 249, StockQuantity = 15, BrandId = brands["JYSK"], CategoryId = categories["Beds & Mattresses"], IsActive = true },
            new() { Name = "Single Bed Frame", Description = "Simple single bed frame in pine.", Price = 199, DiscountPrice = 0, StockQuantity = 20, BrandId = brands["Nordic Home"], CategoryId = categories["Beds & Mattresses"], IsActive = true },
            // Sofas & Armchairs
            new() { Name = "Living Room Sofa", Description = "3-seater fabric sofa.", Price = 799, DiscountPrice = 0, StockQuantity = 5, BrandId = brands["JYSK"], CategoryId = categories["Sofas & Armchairs"], IsActive = true },
            new() { Name = "Reading Armchair", Description = "Cozy armchair for reading.", Price = 349, DiscountPrice = 299, StockQuantity = 8, BrandId = brands["JYSK"], CategoryId = categories["Sofas & Armchairs"], IsActive = true },
            new() { Name = "Corner Sofa", Description = "L-shaped corner sofa.", Price = 999, DiscountPrice = 899, StockQuantity = 4, BrandId = brands["Nordic Home"], CategoryId = categories["Sofas & Armchairs"], IsActive = true },
            // Tables & Chairs - only tables and chairs
            new() { Name = "Dining Table Oak", Description = "Solid oak dining table.", Price = 449, DiscountPrice = 0, StockQuantity = 6, BrandId = brands["JYSK"], CategoryId = categories["Tables & Chairs"], IsActive = true },
            new() { Name = "Dining Chairs Set of 4", Description = "Set of 4 wooden dining chairs.", Price = 199, DiscountPrice = 169, StockQuantity = 12, BrandId = brands["JYSK"], CategoryId = categories["Tables & Chairs"], IsActive = true },
            new() { Name = "Coffee Table", Description = "Modern coffee table with storage.", Price = 179, DiscountPrice = 0, StockQuantity = 15, BrandId = brands["Nordic Home"], CategoryId = categories["Tables & Chairs"], IsActive = true },
            new() { Name = "Office Chair", Description = "Ergonomic office chair.", Price = 249, DiscountPrice = 199, StockQuantity = 10, BrandId = brands["JYSK"], CategoryId = categories["Tables & Chairs"], IsActive = true },
            // Storage
            new() { Name = "Bookshelf Pine", Description = "5-shelf pine bookshelf.", Price = 159, DiscountPrice = 0, StockQuantity = 20, BrandId = brands["JYSK"], CategoryId = categories["Storage"], IsActive = true },
            new() { Name = "Wardrobe White", Description = "White sliding-door wardrobe.", Price = 699, DiscountPrice = 599, StockQuantity = 4, BrandId = brands["JYSK"], CategoryId = categories["Storage"], IsActive = true },
            new() { Name = "Chest of Drawers", Description = "5-drawer chest in white.", Price = 279, DiscountPrice = 0, StockQuantity = 8, BrandId = brands["Nordic Home"], CategoryId = categories["Storage"], IsActive = true },
            // Lighting
            new() { Name = "Floor Lamp", Description = "Modern floor lamp.", Price = 79, DiscountPrice = 0, StockQuantity = 25, BrandId = brands["EcoLight"], CategoryId = categories["Lighting"], IsActive = true },
            new() { Name = "Table Lamp", Description = "LED table lamp with dimmer.", Price = 49, DiscountPrice = 39, StockQuantity = 30, BrandId = brands["EcoLight"], CategoryId = categories["Lighting"], IsActive = true },
            new() { Name = "Ceiling Light", Description = "Scandinavian ceiling pendant.", Price = 89, DiscountPrice = 0, StockQuantity = 18, BrandId = brands["EcoLight"], CategoryId = categories["Lighting"], IsActive = true },
            // Textiles
            new() { Name = "Bed Linen Set", Description = "Cotton bed linen set.", Price = 49, DiscountPrice = 39, StockQuantity = 50, BrandId = brands["JYSK"], CategoryId = categories["Textiles"], IsActive = true },
            new() { Name = "Curtains Pair", Description = "Blackout curtains.", Price = 69, DiscountPrice = 0, StockQuantity = 40, BrandId = brands["Nordic Home"], CategoryId = categories["Textiles"], IsActive = true },
            new() { Name = "Cushion Cover Set", Description = "Set of 4 cushion covers.", Price = 29, DiscountPrice = 24, StockQuantity = 60, BrandId = brands["JYSK"], CategoryId = categories["Textiles"], IsActive = true },
            // Electronics - only electronics
            new() { Name = "Desk Lamp LED", Description = "USB rechargeable desk lamp.", Price = 45, DiscountPrice = 0, StockQuantity = 25, BrandId = brands["SmartTech"], CategoryId = categories["Electronics"], IsActive = true },
            new() { Name = "Bluetooth Speaker", Description = "Portable wireless speaker.", Price = 59, DiscountPrice = 49, StockQuantity = 20, BrandId = brands["SmartTech"], CategoryId = categories["Electronics"], IsActive = true },
            new() { Name = "Smart Clock", Description = "Alarm clock with FM radio.", Price = 35, DiscountPrice = 0, StockQuantity = 35, BrandId = brands["SmartTech"], CategoryId = categories["Electronics"], IsActive = true },
            new() { Name = "LED Strip Light", Description = "RGB LED strip for ambient lighting.", Price = 29, DiscountPrice = 24, StockQuantity = 50, BrandId = brands["SmartTech"], CategoryId = categories["Electronics"], IsActive = true },
        };

        db.Products.AddRange(products);
        await db.SaveChangesAsync();
    }
}
