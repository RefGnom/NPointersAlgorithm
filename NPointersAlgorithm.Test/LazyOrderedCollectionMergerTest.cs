using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;

namespace NPointersAlgorithm.Test;

public class LazyOrderedCollectionMergerTest : TestBase
{
    private readonly Fixture _fixture = new();
    private readonly Random _random = new();

    [TestCase(1, 1)]
    [TestCase(1, 2)]
    [TestCase(1, 7)]
    [TestCase(2, 1)]
    [TestCase(2, 4)]
    [TestCase(7, 1)]
    [TestCase(7, 13)]
    [TestCase(100, 1)]
    [TestCase(100, 500)]
    public void TestSomeOrderedCollections_ShouldMergeWithOrder(int collectionsCount, int collectionSize)
    {
        var collections = new List<long[]>();
        for (var i = 0; i < collectionsCount; i++)
        {
            collections.Add(_fixture.CreateMany<long>(collectionSize).Order().ToArray());
        }

        var lazyOrderedCollectionMerger = new LazyOrderedCollectionMerger<long, long>(
            collections,
            new CollectionMergerFunctionsTestImplementation()
        );

        var mergedCollection = lazyOrderedCollectionMerger.Enumerate().ToArray();
        TestContext.Out.WriteLine(string.Join(", ", mergedCollection));
        mergedCollection.Should().BeInAscendingOrder();
        mergedCollection.Should().HaveCount(collectionsCount * collectionSize);
    }

    [Test]
    public void TestSomeOrderedCollectionsWithBigStep_ShouldMergeWithOrder()
    {
        const int collectionsCount = 1000;
        const int collectionSize = 1000;

        var collections = new List<List<long>>();
        for (var i = 0; i < collectionsCount; i++)
        {
            var collection = new List<long>
            {
                _random.Next(),
            };

            for (var j = 1; j < collectionSize; j++)
            {
                collection.Add(collection.Last() + _random.Next());
            }

            collections.Add(collection);
        }

        var lazyOrderedCollectionMerger = new LazyOrderedCollectionMerger<long, long>(
            collections,
            new CollectionMergerFunctionsTestImplementation()
        );

        var mergedCollection = lazyOrderedCollectionMerger.Enumerate().ToArray();
        mergedCollection.Should().BeInAscendingOrder();
        mergedCollection.Should().HaveCount(collectionsCount * collectionSize);
    }
}