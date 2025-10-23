using UnityEngine;

public struct BlockReturnEvent : IGameEvent
{
    public Block block;

    public BlockReturnEvent(Block block)
    {
        this.block = block;
    }
}
