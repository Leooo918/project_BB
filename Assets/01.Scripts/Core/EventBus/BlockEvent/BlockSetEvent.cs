using UnityEngine;

public struct BlockSetEvent : IGameEvent
{
    public BlockParent block;

    public BlockSetEvent(BlockParent block)
    {
        this.block = block;
    }
}
