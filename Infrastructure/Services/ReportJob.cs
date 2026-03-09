using Quartz;

public class ReportJob(IEmailService email):IJob
{
     private readonly IEmailService service = email;
    
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Generating report...");
        await service.SendAsync("sobirovazulhija091@gmail.com", "testing background jobs", "this inertent magazin project");
        await Task.CompletedTask;
    }

}