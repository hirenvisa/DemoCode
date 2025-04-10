namespace CSharp;

public class UnboundGenericTypesDemo
{
 
    delegate int DelP(string s);
    delegate void DelQ(string s, out int x);
    
    static void Main()
    {
        // Unbound generic type in nameof
        var a = nameof(List<string>);
        var b = nameof(List<string>);

        var c = nameof(List<>);
        
        // Modified on lambda parameters        
        DelP p = (string s) => s.Length;
        DelQ q = (string s, out int i) => i = s.Length;
        
        Console.WriteLine($"a: {a}");
        Console.WriteLine($"b: {b}");
        Console.WriteLine($"p: {p("Hello")}");


        var fields = new Fields();
        fields.FirstName = "Tester";
        fields.Lastname ="Automation";
        fields.Age = 25;
        fields.field = fields.Age.Value;
        
        Console.WriteLine($"FirstName: {fields.FirstName}; Lastname: {fields.Lastname}; Age: {fields.Age}");
        Console.WriteLine($"field : {fields.field}");

    }
}

class Fields
{
    string? lastName;
    public string? FirstName { get; set; }
    public string? Lastname
    {
        get => lastName;
        set => lastName = value?.Trim();
    }

    public int field;
    public int? Age
    {
        get;
        set => field = value ?? 0;
    }
}