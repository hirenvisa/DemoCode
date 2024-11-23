namespace SingletonPattern;

public class SingletonImpl
{
    private static SingletonImpl _instance = default!;
    private static object _lock = new();

    public static SingletonImpl Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    if (_instance is null)
                        _instance = new SingletonImpl();
                }
            }

            return _instance;
        }
    }

    private SingletonImpl()
    {
        Console.WriteLine("Singleton thread safe implementation is initiated.");
    }
}