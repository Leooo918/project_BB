using UnityEngine;

public struct BlockReturnEvent : IGameEvent
{
    public BlockParent block;

    public BlockReturnEvent(BlockParent block)
    {
        this.block = block;
    }
}
