namespace NPointersAlgorithm;

public interface ICollectionMergerFunctions<in TItem, TPointer>
{
    TPointer CalculatePointer(TItem item);
    int Compare(TPointer left, TPointer right);
    bool IsValid(TPointer pointer);
}