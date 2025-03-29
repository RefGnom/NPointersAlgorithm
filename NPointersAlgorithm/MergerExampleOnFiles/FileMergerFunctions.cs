namespace NPointersAlgorithm.MergerExampleOnFiles;

public class FileMergerFunctions(
    long timestampFrom,
    long timestampTo
) : ICollectionMergerFunctions<UserItem, long>
{
    private readonly long _timestampFrom = timestampFrom;
    private readonly long _timestampTo = timestampTo;

    public long CalculatePointer(UserItem item)
    {
        return item.Timestamp;
    }

    public int Compare(long left, long right)
    {
        return left.CompareTo(right);
    }

    public bool IsValid(long pointer)
    {
        return _timestampFrom <= pointer && pointer <= _timestampTo;
    }
}