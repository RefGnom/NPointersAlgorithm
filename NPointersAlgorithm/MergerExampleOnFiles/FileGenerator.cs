using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NPointersAlgorithm.MergerExampleOnFiles;

public static class FileGenerator
{
    private const long stepSize = 10000;
    private static readonly Type SerializeType = typeof(UserItem);
    private static readonly byte EndLineByte = Convert.ToByte('\n');

    public static async Task GenerateFilesAsync(string path, string namePrefix, long eachFileSize, int count)
    {
        var tasks = Enumerable.Range(1, count)
            .Select(i => $"{namePrefix}_{i}")
            .Select(n => GenerateAsync(path, n, eachFileSize))
            .ToArray();
        await Task.WhenAll(tasks);
    }

    public static async Task GenerateAsync(string path, string name, long size)
    {
        var fileStream = File.OpenWrite(Path.Combine(path, Path.ChangeExtension(name, "txt")));
        var random = new Random();

        var timestamp = random.NextInt64(stepSize);
        for (var i = 0; i < size; i++)
        {
            var userItem = UserItemFactory.Create(timestamp);
            var bytes = JsonSerializer.SerializeToUtf8Bytes(userItem, SerializeType);
            await fileStream.WriteAsync(bytes);
            fileStream.WriteByte(EndLineByte);

            if (i % 1000 == 0)
            {
                await fileStream.FlushAsync();
                Console.WriteLine($"file \"{name}\" progress: {i} from {size}");
            }

            timestamp += random.NextInt64(stepSize);
        }

        Console.WriteLine($"file \"{name}\" progress: {size} from {size}");
        await fileStream.DisposeAsync();
    }
}