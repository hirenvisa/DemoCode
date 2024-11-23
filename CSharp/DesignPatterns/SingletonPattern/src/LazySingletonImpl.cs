namespace SingletonPattern;

public class LazySingletonImpl
{

    private static readonly Lazy<LazySingletonImpl> _instance = new(() => new());

    public static LazySingletonImpl Instance => _instance.Value;

    private LazySingletonImpl()
    {
        Console.WriteLine("Lazy singleton implementation is initiated.");
    }
}