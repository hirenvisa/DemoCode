using System.Threading.Channels;

namespace BackgroundServiceWebApi.Services;

public class BackgroundQueueService<T>
{
    private readonly Channel<T> _channel = Channel.CreateUnbounded<T>(
        new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = true,
            AllowSynchronousContinuations = false
           
        });
    
    public async ValueTask QueueBackgroundWorkItemAsync(T item)
    {
        await _channel.Writer.WriteAsync(item);
    }
    
    public async ValueTask<T> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }
    
    public async ValueTask<int> GetQueueCountAsync()
    {
        return await Task.FromResult(_channel.Reader.Count);
    }
}