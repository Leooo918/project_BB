using System.Collections.Generic;
using UnityEngine;

public struct RemainBlockChangeEvent : IGameEvent
{
    public List<BlockSO> remainBlock;

    public RemainBlockChangeEvent(BlockParent[] blockParents)
    {
        remainBlock = new List<BlockSO>();
        foreach (BlockParent block in blockParents)
        {
            if(block != null) remainBlock.Add(block.BlockInfo);
        }
    }
}
