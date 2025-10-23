using UnityEngine;

public struct BlockSelectEvent : IGameEvent
{
    public int selectedIndex;
    public BlockParent selectedBlock;

    public BlockSelectEvent(BlockParent block, int index)
    {
        selectedBlock = block;
        selectedIndex = index;
    }
}
