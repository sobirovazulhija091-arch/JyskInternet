using System.Text.Json;
using Microsoft.JSInterop;
using WebJysk.Marketplace.Models;

namespace WebJysk.Marketplace.Services;

public class CartService
{
    private const string StorageKey = "jysk_cart";
    private readonly IJSRuntime _js;
    private List<CartItem> _items = [];
    private bool _loaded;

    public event Action? OnChange;

    public CartService(IJSRuntime js)
    {
        _js = js;
    }

    public IReadOnlyList<CartItem> Items => _items;
    public int Count => _items.Sum(i => i.Quantity);
    public decimal Total => _items.Sum(i => i.Price * i.Quantity);

    private async Task LoadAsync()
    {
        if (_loaded) return;
        try
        {
            var json = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(json))
                _items = JsonSerializer.Deserialize<List<CartItem>>(json) ?? [];
        }
        catch { }
        _loaded = true;
    }

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_items);
        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        OnChange?.Invoke();
    }

    public async Task AddAsync(Product product, int quantity = 1)
    {
        await LoadAsync();
        var existing = _items.FirstOrDefault(i => i.ProductId == product.Id);
        if (existing != null)
            existing.Quantity += quantity;
        else
            _items.Add(new CartItem { ProductId = product.Id, ProductName = product.Name, Price = product.DiscountPrice > 0 ? product.DiscountPrice : product.Price, Quantity = quantity, ImageUrl = product.ImageUrl });
        await SaveAsync();
    }

    public async Task UpdateQuantityAsync(int productId, int quantity)
    {
        await LoadAsync();
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null) return;
        if (quantity <= 0)
        {
            _items.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
        }
        await SaveAsync();
    }

    public async Task RemoveAsync(int productId)
    {
        await LoadAsync();
        _items.RemoveAll(i => i.ProductId == productId);
        await SaveAsync();
    }

    public async Task<List<CartItem>> GetItemsAsync()
    {
        await LoadAsync();
        return _items.ToList();
    }

    public async Task ClearAsync()
    {
        _items = [];
        await SaveAsync();
    }
}
