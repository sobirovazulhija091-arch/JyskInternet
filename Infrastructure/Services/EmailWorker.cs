using Microsoft.Extensions.Hosting;

public class EmailWorker : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Email worker started");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Email worker stopped");
        return Task.CompletedTask;
    }
}