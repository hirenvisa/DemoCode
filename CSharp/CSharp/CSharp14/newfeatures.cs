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