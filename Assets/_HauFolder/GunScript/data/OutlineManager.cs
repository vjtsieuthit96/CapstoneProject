public class OutlineManager
{
    private IOutlineTarget lastTarget;

    public void Highlight(IOutlineTarget target, int outlineLayer)
    {
        if (target == lastTarget) return;

        lastTarget?.DisableOutline();
        target.EnableOutline(outlineLayer);
        lastTarget = target;
    }

    public void ClearHighlight()
    {
        lastTarget?.DisableOutline();
        lastTarget = null;
    }
}
