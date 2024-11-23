namespace SingletonPattern;

sealed class SingletonIternalImpl
{
    public static String ClassName;
    public static SingletonIternalImpl Instance => Nested.Instance;

    private class Nested
    {
        public static SingletonIternalImpl Instance { get; } = new();
        static Nested() {}
    }

    private SingletonIternalImpl()
    {
    }

    static SingletonIternalImpl()
    {
        ClassName = "Singleton";
    }
}