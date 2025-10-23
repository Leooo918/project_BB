using UnityEngine;

public struct BlockSetEvent : IGameEvent
{
    public Block block;

    public BlockSetEvent(Block block)
    {
        this.block = block;
    }
}
