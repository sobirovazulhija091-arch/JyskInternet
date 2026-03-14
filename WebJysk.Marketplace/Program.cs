using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebJysk.Marketplace;
using WebJysk.Marketplace.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5244";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductApiService>();
builder.Services.AddScoped<CategoryApiService>();
builder.Services.AddScoped<BrandApiService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<OrderApiService>();
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<ReviewApiService>();

await builder.Build().RunAsync();
