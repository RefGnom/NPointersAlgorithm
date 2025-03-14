using System.Collections.Generic;
using System.Linq;

namespace NPointersAlgorithm;

public class LazyOrderedCollectionsMerger<TItem, TPointer>
{
    private readonly PriorityQueue<LazyCollectionWithPointer<TItem, TPointer>, TPointer> _orderedQueues;
    private readonly INPointersAlgorithmSettingsProvider<TItem, TPointer> _settingsProvider;

    public LazyOrderedCollectionsMerger(
        IReadOnlyCollection<IEnumerable<TItem>> collections,
        INPointersAlgorithmSettingsProvider<TItem, TPointer> settingsProvider
    )
    {
        _orderedQueues = new PriorityQueue<LazyCollectionWithPointer<TItem, TPointer>, TPointer>(collections.Count);
        _settingsProvider = settingsProvider;

        var enumerableWithPointers = collections.Select(x => new LazyCollectionWithPointer<TItem, TPointer>(x, settingsProvider));
        foreach (var orderedQueueWithPointer in enumerableWithPointers)
        {
            if (!orderedQueueWithPointer.IsEmpty)
            {
                _orderedQueues.Enqueue(orderedQueueWithPointer, orderedQueueWithPointer.CurrentPointer);
            }
        }
    }

    public IEnumerable<TItem> Enumerate()
    {
        while (_orderedQueues.Count > 0)
        {
            var collectionWithMinPointer = _orderedQueues.Dequeue();

            if (!_settingsProvider.IsValid(collectionWithMinPointer.CurrentPointer))
            {
                continue;
            }

            yield return collectionWithMinPointer.GetItem();

            if (!collectionWithMinPointer.IsEmpty)
            {
                _orderedQueues.Enqueue(collectionWithMinPointer, collectionWithMinPointer.CurrentPointer);
            }
        }
    }
}