using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NPointersAlgorithm.MergerExampleOnFiles;

public static class FileGenerator
{
    private const long stepSize = 100_000;
    private static readonly byte EndLineByte = Convert.ToByte('\n');

    public static Task<string[]> GenerateFilesAsync(string path, string namePrefix, long eachFileSize, int count)
    {
        var tasks = Enumerable.Range(1, count)
            .Select(i => $"{namePrefix}_{i}")
            .Select(n => GenerateFileAsync(path, n, eachFileSize))
            .ToArray();
        return Task.WhenAll(tasks);
    }

    public static async Task<string> GenerateFileAsync<TLine>(string path, string name, IEnumerable<TLine> collection, long? size = null)
    {
        var fullPath = Path.Combine(path, Path.ChangeExtension(name, "txt"));
        var fileStream = File.OpenWrite(fullPath);

        var i = 0L;
        foreach (var userItem in collection)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(userItem, typeof(TLine));
            await fileStream.WriteAsync(bytes);
            fileStream.WriteByte(EndLineByte);

            if (++i % 1000 == 0)
            {
                await fileStream.FlushAsync();
                WriteLogString();
            }
        }

        await fileStream.DisposeAsync();
        return fullPath;

        void WriteLogString()
        {
            var progressLogWithoutSize = $"file \"{name}\" progress: {i}";
            var progressLog = size == null
                ? progressLogWithoutSize
                : $"{progressLogWithoutSize} from {size}";
            Console.WriteLine(progressLog);
        }
    }

    private static Task<string> GenerateFileAsync(string path, string name, long size)
    {
        return GenerateFileAsync(path, name, GenerateCollection(size), size);
    }

    private static IEnumerable<UserItem> GenerateCollection(long size)
    {
        var random = new Random();
        var timestamp = 0L;

        for (var i = 0; i < size; i++)
        {
            timestamp += random.NextInt64(stepSize);
            yield return UserItemFactory.Create(timestamp);
        }
    }
}