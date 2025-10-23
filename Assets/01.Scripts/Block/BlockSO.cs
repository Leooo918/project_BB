using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BlockSO")]
public class BlockSO : ScriptableObject
{
    [field: SerializeField] public List<Vector2Int> blockPositions { get; private set; }
    [field: SerializeField] public BlockSlot blockPrefab { get; private set; }
}
