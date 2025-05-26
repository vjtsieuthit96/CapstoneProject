using UnityEngine;

public interface IOutlineTarget
{
    void EnableOutline(int outlineLayer);
    void DisableOutline();
}

public interface IOutlineResolver
{
    bool TryGetOutlineLayer(string tag, out int outlineLayer);
}
