
namespace SingletonPattern;

public class Singleton
{
    private static Singleton _instance = default!;

    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
                _instance = new();
            return _instance;
        }
    }

    private Singleton()
    {
        Console.WriteLine("Singleton initiated.");
    }
}
