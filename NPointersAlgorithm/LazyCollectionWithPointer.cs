using System;
using System.Collections.Generic;

namespace NPointersAlgorithm;

public class LazyCollectionWithPointer<TItem, TPointer> : IDisposable
{
    private readonly ICollectionMergerFunctions<TItem, TPointer> _functions;
    private readonly IEnumerator<TItem> _enumerator;

    public bool IsEmpty { get; private set; }

    public TPointer CurrentPointer
    {
        get
        {
            if (IsEmpty)
            {
                throw new NPointersAlgorithmException("Нельзя получить указатель пустой или законченной колекции");
            }

            return _functions.CalculatePointer(_enumerator.Current);
        }
    }

    public LazyCollectionWithPointer(
        IEnumerable<TItem> collection,
        ICollectionMergerFunctions<TItem, TPointer> functions
    )
    {
        _functions = functions;
        _enumerator = collection.GetEnumerator();

        IsEmpty = !_enumerator.MoveNext();
    }

    public TItem GetItem()
    {
        if (IsEmpty)
        {
            throw new NPointersAlgorithmException("Нельзя получить элемент пустой или законченной колекции");
        }

        var item = _enumerator.Current;
        var previousPointer = CurrentPointer;

        IsEmpty = !_enumerator.MoveNext();

        if (!IsEmpty && _functions.Compare(CurrentPointer, previousPointer) >= 0)
        {
            throw new NPointersAlgorithmException(
                $"Исходные коллекции должны быть отсортированы по возрастанию. " +
                $"Предыдущий указатель: {previousPointer}, текущий: {CurrentPointer}"
            );
        }

        return item;
    }

    ~LazyCollectionWithPointer()
    {
        Dispose();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _enumerator.Dispose();
    }
}