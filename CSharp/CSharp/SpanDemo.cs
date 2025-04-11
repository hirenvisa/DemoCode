namespace CSharp;

public class SpanDemo
{
    static void Main()
    {
        Action<int> a = new int[0].M;
        a(5);
        Action<char> a2 = new char[0].M;
        a2('I');
    }
}

static class Extensions
{
    public static void M<T>(this Span<T> span, T value) => Console.WriteLine(1);
    public static void M<T>(this IEnumerable<T> span, T value) => Console.WriteLine(2);
}