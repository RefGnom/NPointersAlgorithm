namespace NPointersAlgorithm.Test;

public class CollectionMergerFunctionsTestImplementation : ICollectionMergerFunctions<long, long>
{
    public long CalculatePointer(long item)
    {
        return item;
    }

    public int Compare(long left, long right)
    {
        return left.CompareTo(right);
    }

    public bool IsValid(long pointer)
    {
        return true;
    }
}