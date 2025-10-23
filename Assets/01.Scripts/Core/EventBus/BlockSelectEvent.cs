using UnityEngine;

public struct BlockSelectEvent : IGameEvent
{
    public int selectedIndex;
    public Block selectedBlock;

    public BlockSelectEvent(Block block, int index)
    {
        selectedBlock = block;
        selectedIndex = index;
    }
}
