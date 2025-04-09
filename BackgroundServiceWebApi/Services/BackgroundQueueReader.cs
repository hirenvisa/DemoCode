namespace BackgroundServiceWebApi.Services;

public class BackgroundQueueReader(
    BackgroundQueueService<int> backgroundQueueService,
    ILogger<BackgroundQueueReader> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var item = await backgroundQueueService.DequeueAsync(stoppingToken);
            Console.WriteLine("Processing item: {Item}", item);
            logger.LogInformation("Processing item: {Item}", item);
        }
    }
}