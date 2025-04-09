namespace CSharp;

public class OutVar
{
    static void Main()
    {
        int value1;
        if (int.TryParse("100", out value1))
            Console.WriteLine(value1);
        Console.WriteLine($"Value1 is in  scope {value1}");

        if (int.TryParse("100", out int value2))
            Console.WriteLine(value2);
        Console.WriteLine($"Value2 is in  scope {value2}");

        if (int.TryParse("100", out var value3))
            Console.WriteLine(value3);
        Console.WriteLine($"Value3 is in  scope {value3}");
        
        if(!int.TryParse("100", out int value4))
            Console.WriteLine("something is wrong!!");
        else
            Console.WriteLine($"parsed value: {value4}");
        
        Console.WriteLine(value4);

        if (int.TryParse("foo", out int value5) && int.TryParse("bar", out int value6))
            Console.WriteLine("both done");
        Console.WriteLine(value5);
        
        
        //Console.WriteLine(value6);


        value6 = 999;
        var isParsed = CheckParse("one-ninety-nine");
        Console.WriteLine($"Data is parsed to integer: {isParsed}");
    }

    public static bool CheckParse(string text) => int.TryParse(text, out var _);
}