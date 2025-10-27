using System.Collections.Generic;
using UnityEngine;

public class BlockSetSabotage : Sabotage
{
    //[SerializeField] private int spawnBlockCount = 1;
    [SerializeField] private Block _blockPrefab;

    public override void DoSabotage(BlockMap map)
    {
        Block[,] blockMap = map.MapInfo.blockMap;
        List<Vector2Int> empty = new List<Vector2Int>();

        for (int i = 0; i < blockMap.GetLength(0); i++)
        {
            for (int j = 0; j < blockMap.GetLength(1); j++)
            {
                if (blockMap[i, j] == null)
                {
                    empty.Add(new Vector2Int(i, j));
                }
            }
        }

        Vector2Int blockPos = empty.GetSingleElementInList();
        Block blockInstance = GameObject.Instantiate(_blockPrefab);

        map.SetBlock(blockInstance, blockPos);
        map.CheckBlockDestroy();
    }
}
