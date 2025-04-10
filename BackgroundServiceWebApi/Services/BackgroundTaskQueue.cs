using System.Threading.Channels;

namespace BackgroundServiceWebApi.Services;

public class BackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, Task>> _channel;
    public BackgroundTaskQueue(int capacity)
    {
        _channel = Channel.CreateBounded<Func<CancellationToken, Task>>(capacity);
    }
    
    public async Task QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem)
    {
        if (workItem == null) throw new ArgumentNullException(nameof(workItem));
        await _channel.Writer.WriteAsync(workItem);
    }
    
    public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }
}