public interface IJwtService
{
    Task<string> CreateTokenAsync(User user);
}