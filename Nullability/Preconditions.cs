using System.Diagnostics.CodeAnalysis;

namespace Nullability;

public class Preconditions
{
    static void Main()
    {
        string? text = MaybeNull();
    }

    internal static T CheckNotNull<T>([NotNull] T? input) where T : class =>
        input ?? throw new ArgumentNullException(nameof(input));

    internal static string? MaybeNull() => DateTime.UtcNow.Second == 0 ? null : "Not null";
}