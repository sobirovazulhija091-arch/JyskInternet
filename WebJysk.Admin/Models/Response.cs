namespace WebJysk.Admin.Models;

public class Response<T>
{
    public int StatusCode { get; set; }
    public List<string> Description { get; set; } = [];
    public T? Data { get; set; }
}
