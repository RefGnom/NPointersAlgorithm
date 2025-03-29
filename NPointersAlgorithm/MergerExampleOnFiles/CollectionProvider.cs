using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace NPointersAlgorithm.MergerExampleOnFiles;

public static class CollectionProvider
{
    public static IEnumerable<UserItem> GetCollection(string path)
    {
        var fileStream = File.OpenRead(path);
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