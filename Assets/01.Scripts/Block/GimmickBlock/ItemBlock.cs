public abstract class ItemBlock : Block
{
    public override void DestroyBlock()
    {
        base.DestroyBlock();
        GiveItem();
    }

    protected abstract void GiveItem();
}
