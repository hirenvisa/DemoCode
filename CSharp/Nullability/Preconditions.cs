using System.Diagnostics.CodeAnalysis;

namespace Nullability;

public class Preconditions
{
    private static void Main()
    {
        var text = MaybeNull();
    }

    internal static T CheckNotNull<T>([NotNull] T? input) where T : class
    {
        return input ?? throw new ArgumentNullException(nameof(input));
    }

    internal static string? MaybeNull()
    {
        return DateTime.UtcNow.Second == 0 ? null : "Not null";
    }
}