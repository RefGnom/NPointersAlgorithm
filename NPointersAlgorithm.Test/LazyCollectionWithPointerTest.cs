using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NSubstitute;

namespace NPointersAlgorithm.Test;

public class LazyCollectionWithPointerTest : TestBase
{
    private readonly Fixture _fixture = new();

    [Test]
    public void TestEmptyCollection_ShouldBeEmpty()
    {
        var lazyCollectionWithPointer = CreateLazyCollectionWithPointer(Array.Empty<long>());

        lazyCollectionWithPointer.IsEmpty.Should().BeTrue();
    }

    [Test]
    public void TestNonEmptyCollection_ShouldBeNonEmpty()
    {
        var lazyCollectionWithPointer = CreateLazyCollectionWithPointer(new long[] { 9 });

        lazyCollectionWithPointer.IsEmpty.Should().BeFalse();
    }

    [Test]
    public void TestEmptyCollection_CurrentPointerShouldThrowsException()
    {
        var lazyCollectionWithPointer = CreateLazyCollectionWithPointer(Array.Empty<long>());

        Assert.Throws<NPointersAlgorithmException>(() => _ = lazyCollectionWithPointer.CurrentPointer);
    }

    [Test]
    public void TestEmptyCollection_GetItemShouldThrowsException()
    {
        var lazyCollectionWithPointer = CreateLazyCollectionWithPointer(Array.Empty<long>());

        Assert.Throws<NPointersAlgorithmException>(() => lazyCollectionWithPointer.GetItem());
    }

    [Test]
    public void TestThatLazyCollectionWithPointerReturnsAllItemsInCorrectOrder()
    {
        var expectedValues = new long[] { 1, 3, 5, 7, 9, 11 };
        var lazyCollectionWithPointer = CreateLazyCollectionWithPointer(expectedValues);

        var actualValues = new List<long>();
        while (!lazyCollectionWithPointer.IsEmpty)
        {
            actualValues.Add(lazyCollectionWithPointer.GetItem());
        }

        actualValues.Should().BeEquivalentTo(expectedValues);
    }

    [Test]
    public void TestCreateWithNonOrderCollection_GetItemShouldThrowsException()
    {
        var expectedValues = new long[] { 3, 3, 5, 2, 7 };
        var lazyCollectionWithPointer = CreateLazyCollectionWithPointer(expectedValues);

        for (var i = 0; i < 2; i++)
        {
            lazyCollectionWithPointer.GetItem().Should().Be(expectedValues[i]);
        }

        Assert.Throws<NPointersAlgorithmException>(() => lazyCollectionWithPointer.GetItem());
    }

    [TestCase(1)]
    [TestCase(40)]
    public void TestCreateWithSomeItems_ShouldBeEmptyAfterSomeCallsGetItem(int count)
    {
        var expectedValues = _fixture.CreateMany<long>(count).Order().ToArray();
        var lazyCollectionWithPointer = CreateLazyCollectionWithPointer(expectedValues);

        foreach (var expectedItem in expectedValues)
        {
            lazyCollectionWithPointer.IsEmpty.Should().BeFalse();
            lazyCollectionWithPointer.GetItem().Should().Be(expectedItem);
        }

        lazyCollectionWithPointer.IsEmpty.Should().BeTrue();
    }

    [Test]
    public void TestThatCurrentPointerCallsCalculatePointer()
    {
        var values = new[] { "bara", "bara", "bere", "bere", "e", "alex", "ferrari", "fazendo", "bara", "bere" };
        var collectionMergerFunctions = Substitute.For<ICollectionMergerFunctions<string, int>>();
        var lazyCollectionWithPointer = new LazyCollectionWithPointer<string, int>(
            values,
            collectionMergerFunctions
        );

        collectionMergerFunctions.Compare(Arg.Any<int>(), Arg.Any<int>()).Returns(1);

        foreach (var value in values)
        {
            var expectedPointer = value.Length;
            collectionMergerFunctions.CalculatePointer(value).Returns(expectedPointer);

            var actualPointer = lazyCollectionWithPointer.CurrentPointer;
            actualPointer.Should().Be(expectedPointer);

            collectionMergerFunctions.Received().CalculatePointer(value);

            lazyCollectionWithPointer.GetItem();
        }
    }
}