using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace NPointersAlgorithm.MergerExampleOnFiles;

public static class CollectionProvider
{
    public static IReadOnlyCollection<IEnumerable<UserItem>> GetCollections(string path, string namePrefix, int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => $"{namePrefix}_{i}")
            .Select(n => GetCollection(path, n))
            .ToArray();
    }

    public static IEnumerable<UserItem> GetCollection(string path, string name)
    {
        var fileStream = File.OpenRead(Path.Combine(path, Path.ChangeExtension(name, "txt")));
        var textReader = new StreamReader(fileStream, Encoding.UTF8);

        while (true)
        {
            var line = textReader.ReadLine();

            if (line is null)
            {
                yield break;
            }

            yield return JsonSerializer.Deserialize<UserItem>(line)!;
        }
    }
}