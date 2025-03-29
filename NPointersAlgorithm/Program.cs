using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NPointersAlgorithm.MergerExampleOnFiles;

namespace NPointersAlgorithm;

public class Program
{
    private const string path = "example";
    private const string namePrefix = "big";
    private const int filesCount = 20;

    private static async Task Main()
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        Directory.CreateDirectory(path);

        var files = await FileGenerator.GenerateFilesAsync(path, namePrefix, 10_000, filesCount);
        var collections = files.Select(CollectionProvider.GetCollection).ToArray();
        var lazyOrderedCollectionMerger = new LazyOrderedCollectionMerger<UserItem, long>(
            collections,
            new FileMergerFunctions(0, long.MaxValue)
        );

        await FileGenerator.GenerateFileAsync(path, "!mega_big", lazyOrderedCollectionMerger.Enumerate());
        //await FileGenerator.GenerateFileAsync(path, "!mega_big_timestamps", lazyOrderedCollectionMerger.Enumerate().Select(x => x.Timestamp));
    }
}