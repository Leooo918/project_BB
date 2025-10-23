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
    /// <param name="positions"></param>
    /// <returns>Return can InsertBlock</returns>
    public static bool TryInsertBlock(BlockSlot[,] slots, BlockSO block, int selectedBlockIndex, Vector2Int selectedSlotPosition, out List<Vector2Int> positions)
    {
        positions = block.blockPositions.ToList();

        for (int i = 0; i < positions.Count; i++)
        {
            Vector2Int offset = block.blockPositions[selectedBlockIndex];
            positions[i] += selectedSlotPosition - offset;

            if (positions[i].x < 0 || positions[i].y < 0 ||
                positions[i].x >= slots.GetLength(0) || positions[i].y >= slots.GetLength(1)) return false;

            if (slots[positions[i].x, positions[i].y] != null) return false;
        }
        
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
    public static List<Vector2Int> CheckBreakingBlocks(BlockSlot[,] slots)
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

    public static bool CheckLineComplete(BlockSlot[,] slots, bool isHorizontal, int index)
    {
        int length = isHorizontal ? slots.GetLength(0) : slots.GetLength(1);
        for (int i = 0; i < length; i++)
        {
            BlockSlot slot = isHorizontal ? slots[i, index] : slots[index, i];
            if (slot == null) return false;
        }
        return true;
    }
}
