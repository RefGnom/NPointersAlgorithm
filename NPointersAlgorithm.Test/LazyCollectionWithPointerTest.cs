using System;
using FluentAssertions;

namespace NPointersAlgorithm.Test;

public class LazyCollectionWithPointerTest : TestBase
{
    [Test]
    public void TestCreateWithEmptyCollection_ShouldBeEmpty()
    {
        var lazyCollectionWithPointer = CreateLazyCollectionWithPointer(Array.Empty<long>());

        lazyCollectionWithPointer.IsEmpty.Should().BeTrue();
    }

    [Test]
    public void TestCreateWithEmptyCollection_CurrentPointerShouldThrowsException()
    {
        var lazyCollectionWithPointer = CreateLazyCollectionWithPointer(Array.Empty<long>());

        Assert.Throws<NPointersAlgorithmException>(() => _ = lazyCollectionWithPointer.CurrentPointer);
    }

    [Test]
    public void TestCreateWithEmptyCollection_GetItemShouldThrowsException()
    {
        var lazyCollectionWithPointer = CreateLazyCollectionWithPointer(Array.Empty<long>());

        Assert.Throws<NPointersAlgorithmException>(() => lazyCollectionWithPointer.GetItem());
    }
}