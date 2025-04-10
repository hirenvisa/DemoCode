namespace BackgroundServiceWebApi.Services;

public class BackgroundTaskService: BackgroundService
{
    private readonly BackgroundTaskQueue _taskQueue;
    public BackgroundTaskService(BackgroundTaskQueue taskQueue)
    {
        _taskQueue = taskQueue;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _taskQueue.DequeueAsync(stoppingToken);
            await workItem(stoppingToken);
        }
    }

}