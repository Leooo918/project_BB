using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class BlockMapController
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="slots"></param>
    /// <param name="block"></param>
    /// <param name="selectedBlockIndex"></param>
    /// <param name="selectedSlotPosition"></param>
    /// <param name="selectedPositions"></param>
    /// <returns>Return can InsertBlock</returns>
    public static bool TryInsertBlock(Block[,] slots, BlockSO block, int selectedBlockIndex, Vector2Int selectedSlotPosition, out List<Vector2Int> selectedPositions, out List<Vector2Int> destroyBlocks)
    {
        destroyBlocks = new List<Vector2Int>();
        selectedPositions = block.blockPositions.ToList();

        for (int i = 0; i < selectedPositions.Count; i++)
        {
            Vector2Int offset = block.blockPositions[selectedBlockIndex];
            selectedPositions[i] += selectedSlotPosition - offset;

            if (selectedPositions[i].x < 0 || selectedPositions[i].y < 0 ||
                selectedPositions[i].x >= slots.GetLength(0) || selectedPositions[i].y >= slots.GetLength(1)) return false;

            if (slots[selectedPositions[i].x, selectedPositions[i].y] != null) return false;
        }

        foreach (Vector2Int position in selectedPositions)
            slots[position.x, position.y] = block.blockPrefab;

        destroyBlocks = CheckBreakingBlocks(slots);

        foreach (Vector2Int position in selectedPositions)
            slots[position.x, position.y] = null;

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slots"></param>
    /// <param name="block"></param>
    /// <param name="selectedBlockIndex"></param>
    /// <param name="selectedSlotPosition"></param>
    /// <returns>Return Block to Destroy</returns>
    public static List<Vector2Int> CheckBreakingBlocks(Block[,] slots)
    {
        List<Vector2Int> destroyPositions = new List<Vector2Int>();

        for (int i = 0; i < slots.GetLength(0); i++)
        {
            if (CheckLineComplete(slots, false, i))
            {
                for (int j = 0; j < slots.GetLength(1); j++)
                {
                    destroyPositions.Add(new Vector2Int(i, j));
                }
            }
        }
        for (int i = 0; i < slots.GetLength(1); i++)
        {
            if (CheckLineComplete(slots, true, i))
            {
                for (int j = 0; j < slots.GetLength(0); j++)
                {
                    destroyPositions.Add(new Vector2Int(j, i));
                }
            }
        }

        return destroyPositions;
    }

    public static bool CheckLineComplete(Block[,] slots, bool isHorizontal, int index)
    {
        int length = isHorizontal ? slots.GetLength(0) : slots.GetLength(1);
        for (int i = 0; i < length; i++)
        {
            Block slot = isHorizontal ? slots[i, index] : slots[index, i];
            if (slot == null) return false;
        }
        return true;
    }

    public static bool CanInsertBlock(Block[,] blockArray, List<BlockSO> remainBlock)
    {
        //remainBlock will not be bigger than 3
        foreach (BlockSO block in remainBlock)
        {
            for (int x = 0; x < blockArray.GetLength(0); x++)
            {
                for (int y = 0; y < blockArray.GetLength(1); y++)
                {
                    if (blockArray[x, y] != null) continue;

                    if (TryInsertBlock(blockArray, block, 0, new Vector2Int(x, y), out _, out _))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
