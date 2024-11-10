namespace Nullability;

public class ChangingProperty
{
    private string? Name => DateTime.UtcNow.Second == 0 ? null : "Not null";

    public static void Main()
    {
        var instance = new ChangingProperty();
        instance.PrintNameLength();
    }

    public void PrintNameLength()
    {
        if (Name is not null) Console.WriteLine(Name.Length);
    }
}