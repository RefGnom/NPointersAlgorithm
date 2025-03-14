using System;
using System.Collections.Generic;

namespace NPointersAlgorithm;

public class LazyCollectionWithPointer<TItem, TPointer> : IDisposable
{
    private readonly INPointersAlgorithmSettingsProvider<TItem, TPointer> _settingsProvider;
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

            return _settingsProvider.CalculatePointer(_enumerator.Current);
        }
    }

    public LazyCollectionWithPointer(
        IEnumerable<TItem> collection,
        INPointersAlgorithmSettingsProvider<TItem, TPointer> settingsProvider
    )
    {
        _settingsProvider = settingsProvider;
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

        if (!IsEmpty && !_settingsProvider.IsNoLess(CurrentPointer, previousPointer))
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