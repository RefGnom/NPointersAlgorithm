using System;
using System.IO;
using System.Threading.Tasks;
using NPointersAlgorithm.MergerExampleOnFiles;

namespace NPointersAlgorithm;

public class Program
{
    private const string path = "example";
    private const string namePrefix = "big";
    private const int filesCount = 100;

    private static async Task Main()
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        Directory.CreateDirectory(path);

        await FileGenerator.GenerateFilesAsync(path, namePrefix, 100, filesCount);

        var lazyOrderedCollectionMerger = new LazyOrderedCollectionMerger<UserItem, long>(
            CollectionProvider.GetCollections(path, namePrefix, filesCount),
            new FileMergerFunctions(0, long.MaxValue)
        );

        var i = 0L;
        foreach (var userItem in lazyOrderedCollectionMerger.Enumerate())
        {
            Console.WriteLine($"{++i}: {userItem.Timestamp}");
        }
    }
}