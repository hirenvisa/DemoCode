using System.Collections;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
[MemoryDiagnoser]
[ShortRunJob]
public class Test
{
    [Benchmark]
    public int SumCompiler()
    {
        int sum = 0;
        foreach (var i in SelectCompiler(_source, i => i * 2))
        {
            sum += i;
        }
        return sum;
    }

    [Benchmark]
    public int SumManual()
    {
        int sum = 0;
        foreach (var i in SelectManual(_source, i => i * 2))
        {
            sum += i;
        }
        return sum;
    }

    [Benchmark]
    public int SumLinq()
    {
        int sum = 0;
        foreach (var i in _source.Select(i => i * 2))
        {
            sum += i;
        }
        return sum;
    }

    private readonly IEnumerable<int> _source = Enumerable.Range(0, 1000);

    private static IEnumerable<TResult> SelectCompiler<TSource, TResult>(IEnumerable<TSource> source,
        Func<TSource, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        return Impl(source, selector);

        static IEnumerable<TResult> Impl(IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }
    }


    private static IEnumerable<TResult> SelectManual<TSource, TResult>(IEnumerable<TSource> source,
        Func<TSource, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        if (source is TSource[] array)
        {
            return ArrayImpl(array, selector);
        }

        return EnumerableImpl(source, selector);

        static IEnumerable<TResult> EnumerableImpl(IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        static IEnumerable<TResult> ArrayImpl(TSource[] source, Func<TSource, TResult> selector)
        {
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }
    }

    sealed class SelectManualArray<TSource, TResult> : IEnumerable<TResult>, IEnumerator<TResult>
    {
        private readonly TSource[] _source;
        private readonly Func<TSource, TResult> _selector;
        private readonly int _threadId = Environment.CurrentManagedThreadId;
        private TResult _current = default!;
        private int _state = 1;

        public SelectManualArray(TSource[] source, Func<TSource, TResult> selector)
        {
            _source = source;
            _selector = selector;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            if (_threadId == Environment.CurrentManagedThreadId && _state == 0)
            {
                _state = 1;
                return this;
            }

            return new SelectManualArray<TSource, TResult>(_source, _selector) { _state = 1 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public bool MoveNext()
        {
            int i = _state - 1;
            TSource[] source = _source;
            while ((uint)i < (uint)source.Length)
            {
                _current = _selector(source[i]);
                _state++;
                return true;
            }
            Dispose();
            return false;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public TResult Current => _current;

        object? IEnumerator.Current => Current;

        public void Dispose()
        {
            _state = -1;
        }
    }

    sealed class SelectManualEnumerable<TSource, TResult> : IEnumerable<TResult>, IEnumerator<TResult>
    {
        private IEnumerable<TSource> _source;
        private Func<TSource, TResult> _selector;
        private TResult _current = default!;
        private IEnumerator<TSource>? _enumerator;
        private int _state = 1;

        public SelectManualEnumerable(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            _source = source;
            _selector = selector;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool MoveNext()
        {
            switch (_state)
            {
                case 1:
                    _enumerator = _source.GetEnumerator();
                    _state = 2;
                    goto case 2;
                case 2:
                    try
                    {
                        while (_enumerator!.MoveNext())
                        {
                            _current = _selector(_enumerator.Current);
                            return true;
                        }
                    }
                    catch
                    {
                        Dispose();
                        throw;
                    }

                    break;
            }

            Dispose();
            return false;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public TResult Current => _current;

        object? IEnumerator.Current => Current;

        public void Dispose()
        {
            _state = -1;
            _enumerator?.Dispose();
        }
    }
}