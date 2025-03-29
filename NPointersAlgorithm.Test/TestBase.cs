using System.Collections.Generic;

namespace NPointersAlgorithm.Test;

[TestFixture]
public abstract class TestBase
{
    protected static LazyCollectionWithPointer<long, long> CreateLazyCollectionWithPointer(IEnumerable<long> collection)
    {
        return new LazyCollectionWithPointer<long, long>(
            collection,
            new CollectionMergerFunctionsTestImplementation()
        );
    }
}