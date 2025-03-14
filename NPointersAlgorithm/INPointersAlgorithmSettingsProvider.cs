namespace NPointersAlgorithm;

public interface INPointersAlgorithmSettingsProvider<in TItem, TPointer>
{
    TPointer CalculatePointer(TItem item);
    bool IsNoLess(TPointer original, TPointer other);
    bool IsValid(TPointer pointer);
}