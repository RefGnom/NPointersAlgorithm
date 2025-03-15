using System.Collections.Generic;
using System.Linq;

namespace NPointersAlgorithm;

public class LazyOrderedCollectionMerger<TItem, TPointer>
{
    private readonly PriorityQueue<LazyCollectionWithPointer<TItem, TPointer>, TPointer> _orderedQueues;
    private readonly ICollectionMergerFunctions<TItem, TPointer> _functions;

    public LazyOrderedCollectionMerger(
        IReadOnlyCollection<IEnumerable<TItem>> collections,
        ICollectionMergerFunctions<TItem, TPointer> functions
    )
    {
        _orderedQueues = new PriorityQueue<LazyCollectionWithPointer<TItem, TPointer>, TPointer>(collections.Count);
        _functions = functions;

        var enumerableWithPointers = collections.Select(x => new LazyCollectionWithPointer<TItem, TPointer>(x, functions));
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

            if (!_functions.IsValid(collectionWithMinPointer.CurrentPointer))
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