namespace CSharp.CSharp14;

public class newfeatures
{
    static void Main()
    {
        Console.WriteLine("======== LOCK OBJECTS ==============");
        ClassThatUseResource classThatUseResource = new ClassThatUseResource();
        classThatUseResource.DoSomething();
        
        Console.WriteLine("======== BACKING FIELD=============");
        Card card = new Card();
        Console.WriteLine($"Card Number: {card.Number}");
        card.Number = "    1234567890123456";
        Console.WriteLine($"Card Number: {card.Number}");
        
        Console.WriteLine("======== IMPLICIT CONVERSION - SPAN ==============");
        char[] array = ['H','E','L','L','O', 'Ø', 'W','O','R','L','D'];
        Span<char> span = array;
        ReadOnlySpan<char> readOnlySpan = span;
        
        Console.WriteLine("Conversion: FROM array -> span -> readOnlySpan");
        Console.WriteLine($"array -> {array.HasSingle('Ø')}");
        Console.WriteLine($"span -> {span.HasSingle('Ø')}");
        Console.WriteLine($"readonly span -> {readOnlySpan.HasSingle('Ø')}");
    }
}

public class ClassThatUseResource
{
    private Lock lockObject = new Lock();
    
    public void DoSomething()
    {
        // Using the lock object
        lock (lockObject)
        {
            Console.WriteLine("Doing something with the lock object.");
        }
    }
}

public class Card
{ 
    public string? Number
    {
        get;
        set => field = value?.Trim();
    }
}

public static class ExtensionMethods
{
    public static bool HasSingle<T>(this ReadOnlySpan<T> span, T value)
    {
        foreach (var item in span)
        {
            if (EqualityComparer<T>.Default.Equals(item, value))
            {
                return true;
            }
        }
        return false;
    }
}