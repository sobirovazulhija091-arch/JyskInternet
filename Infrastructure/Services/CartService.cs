using System.Net;
using Microsoft.EntityFrameworkCore;
using Quartz.Xml.JobSchedulingData20;
public class CartService(ApplicationDbContext dbContext):ICartService
{
    private readonly ApplicationDbContext context = dbContext;

    public async Task<Response<string>> AddItemAsync(string userId, int productId, int quantity)
    {
       if (quantity <= 0)
       {
        return new Response<string>(HttpStatusCode.BadRequest, "Quantity must be greater than 0");
       }
       var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

if (product == null)
   { 
    return new Response<string>(HttpStatusCode.NotFound, "Product not found");
   }
var cart = await context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
if (cart == null)
{
    cart = new Cart
    {
        UserId = userId,
        CartItems = new List<CartItem>()
    };
    await context.Carts.AddAsync(cart);
}

var existing = cart.CartItems
    .FirstOrDefault(ci => ci.ProductId == productId);

int newQuantity = quantity;

if (existing != null)
{
    newQuantity = existing.Quantity + quantity;
}

if (newQuantity > product.StockQuantity)
    return new Response<string>(HttpStatusCode.BadRequest, "Not enough stock");

if (existing != null)
{
    existing.Quantity = newQuantity;
    existing.Price = product.DiscountPrice > 0 ? product.DiscountPrice : product.Price;
}
else
    cart.CartItems.Add(new CartItem
    {
        ProductId = productId,
        Quantity = quantity,
        Price = product.DiscountPrice > 0 ? product.DiscountPrice : product.Price
    });

await context.SaveChangesAsync();

    return new Response<string>(HttpStatusCode.OK,"Item added to cart");
    }

    public async Task<Response<string>> DeltetCartAsync(string userId)
    {
        var del = await context.Carts.Include(a=>a.CartItems).FirstOrDefaultAsync(a=>a.UserId==userId);
        if (del == null)
        {
            return new Response<string>(HttpStatusCode.NotFound,"Cart not found");
        }
          context.CartItems.RemoveRange(del.CartItems);
          context.Carts.Remove(del);
         await context.SaveChangesAsync();
         return new Response<string>(HttpStatusCode.OK,"Cart deleted successfully");
    }

    public async Task<Response<Cart>> GetUserCartAsync(string userId)
    {
        var get = await context.Carts.Include(c=>c.CartItems).ThenInclude(p=>p.Products)
        .FirstOrDefaultAsync(c=>c.UserId==userId);
        if (get == null)
        {
            return new Response<Cart>(HttpStatusCode.NotFound,"Cart not found");
        }
       return new Response<Cart>(HttpStatusCode.OK,"Ok,",get);
    
    }
    public async Task<Response<string>> RemoveItemAsync(string userId, int productId)
    {
        var cart = await context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
    if (cart == null)
    {
        return new Response<string>(HttpStatusCode.NotFound, "Cart not found");
    }
    var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
    if (item == null)
        {
          return new Response<string>(HttpStatusCode.NotFound, "Product not in cart");   
        }
    context.CartItems.Remove(item);
    await context.SaveChangesAsync();
    return new Response<string>(HttpStatusCode.OK, "Item removed successfully");
    }

    public async Task<Response<string>> UpdateQuantityAsync(string userId, int productId, int quantity)
    {
       if (quantity <= 0)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Quantity must be greater than 0");
        }

       var cart = await context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);

if (cart == null)
        {
          return new Response<string>(HttpStatusCode.NotFound, "Cart not found");   
        }

var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

if (item == null)
        {
    return new Response<string>(HttpStatusCode.NotFound, "Item not found");         
        }
   
var product = await context.Products
    .FirstOrDefaultAsync(p => p.Id == productId);

if (product == null || quantity > product.StockQuantity)
        {
           return new Response<string>(HttpStatusCode.BadRequest, "Not enough stock");   
        }

item.Quantity = quantity;
item.Price = product.DiscountPrice > 0 ? product.DiscountPrice : product.Price;
await context.SaveChangesAsync();
          return new Response<string>(HttpStatusCode.OK,"Updated successfully");  
    }
}
